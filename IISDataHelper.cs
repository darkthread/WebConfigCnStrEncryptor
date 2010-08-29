using System;
using System.Linq;
using System.Xml.Linq;
using System.DirectoryServices;
using System.Collections.Generic;


class IISDataHelper
{
    public static XDocument ReadSettings(string ip,
        string uid, string pwd, Action<string> progressCallback)
    {
        string path = String.Format("IIS://{0}/W3SVC", ip);
        DirectoryEntry w3svc =
            string.IsNullOrEmpty(uid) ?
            new DirectoryEntry(path) :
            new DirectoryEntry(path, uid, pwd);
        XDocument xd = XDocument.Parse("<root />");
        pools.Clear();
        exploreTree(w3svc, xd.Root, progressCallback);
        return xd;
    }

    static Dictionary<string, string> pools = 
        new Dictionary<string, string>();

    private static void exploreTree(
        DirectoryEntry de, XElement xe, Action<string> cb)
    {
        foreach (DirectoryEntry childEntry in de.Children)
        {
            XElement childElement = new XElement(
                childEntry.SchemaClassName,
                new XAttribute("Name", childEntry.Name),
                new XAttribute("Path", childEntry.Path)
                );
            //Get properties
            XElement propNode = new XElement("Properties");
            foreach (PropertyValueCollection pv in childEntry.Properties)
            {
                //Array
                if (pv.Value != null &&
                    pv.Value.GetType().IsArray)
                {
                    XElement propCol = new XElement(pv.PropertyName);
                    foreach (object obj in pv.Value as object[])
                    {
                        string v = Convert.ToString(obj);
                        //Set ASP.NET version
                        if (pools.Count == 0 && pv.PropertyName == "ScriptMaps"
                            && v.StartsWith(".aspx,"))
                        {
                            string aspNetVer =
                                v.Split(',')[1].Split('\\')
                                .Single(o => o.StartsWith("v"));
                            childElement.Add(
                                new XAttribute("AspNetVer", aspNetVer));
                        }
                        propCol.Add(new XElement("Entry", v));
                    }
                    propNode.Add(propCol);
                }
                else
                {
                    string v = Convert.ToString(pv.Value);
                    propNode.Add(new XElement(pv.PropertyName, v));
                //Set home directory
                if (pv.PropertyName == "Path")
                    childElement.Add(new XAttribute("HomeDir", v));
                //Try to find the runtime version
                else if (pools.Count > 0 && 
                    pv.PropertyName == "AppPoolId" && pools.ContainsKey(v))
                    childElement.Add(
                        new XAttribute("AspNetVer", pools[v]));
                }
            }
            //For IIS 7, use AppPool to decide ASP.NET runtime version
            if (childEntry.SchemaClassName == "IIsApplicationPool")
            {
                XElement runtimeVer = propNode.Element("ManagedRuntimeVersion");
                string ver = runtimeVer != null ? runtimeVer.Value : "v2.0";
                pools.Add(childEntry.Name, ver);
            }
            childElement.Add(propNode);
            
            xe.Add(childElement);
            if (cb != null) cb(childEntry.Name);
            exploreTree(childEntry, childElement, cb);
        }
    }
}
