using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Web.Configuration;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using System.Resources;

namespace WebConfigEncryptor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region BackgroundWorker for Query
        private void wkrQuery_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime nextReportTime = DateTime.Now.AddMilliseconds(20);
            e.Result = IISDataHelper.ReadSettings(
                "127.0.0.1", null, null,
                (s) =>
                {
                    if (DateTime.Now.CompareTo(nextReportTime) > 0)
                    {
                        wkrQuery.ReportProgress(50, s);
                        nextReportTime = DateTime.Now.AddMilliseconds(20);
                    }
                });
        }

        XDocument IisMeta = null;

        private void wkrQuery_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblStatus.Text = "Ready";
            pnlMain.Enabled = true;
            IisMeta = (e.Result as XDocument); //.Save("B:\\IIS.XML");
            SearchWebConfig();
        }

        private void wkrQuery_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblStatus.Text = string.Format(
                Resources.Message.ProgressProcessing
                , e.UserState.ToString());
        }
        #endregion

        class WebAppInfo
        {
            public string WebId { get; set; }
            public string Home { get; set; }
            public XElement XmlElement { get; set; } 
            public string WebConfigPath
            {
                get { return Path.Combine(Home, "web.config"); }
            }
        }
        private void SearchWebConfig()
        {
            //Find all HomeDir
            var q = from o in IisMeta.Root.Descendants()
                    where o.Attribute("HomeDir") != null &&
                          File.Exists(Path.Combine(
                            o.Attribute("HomeDir").Value, "web.config"))
                    orderby o.Attribute("Path").Value
                    select new WebAppInfo() { 
                        WebId = o.Attribute("Path").Value, 
                        Home = o.Attribute("HomeDir").Value,
                        XmlElement = o
                    };
            cbxWebApp.DataSource = q.ToList();
            cbxWebApp.DisplayMember = "WebId";
            cbxWebApp.ValueMember = "Home";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pnlMain.Enabled = false;
            wkrQuery.RunWorkerAsync();
        }

        private WebAppInfo currWebApp = null;
        Configuration currConf = null;

        private void chgEncryptButtonStatus()
        {
            btnExecute.Enabled = false;
            if (currConf == null) return;
            ConfigurationSection cnStrSec =
                currConf.ConnectionStrings;
            if (cnStrSec != null)
            {
                btnExecute.Enabled = true;
                btnExecute.Text = cnStrSec.SectionInformation.IsProtected ?
                    Resources.Message.Decrypt :
                    Resources.Message.Encrypt;
            }
        }

        private void cbxWebApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                currWebApp = cbxWebApp.SelectedItem as WebAppInfo;
                wbEditor.Navigate(currWebApp.WebConfigPath);
                //convert physical path to virtual path
                //ref: http://stackoverflow.com/questions/2368748/how-to-openwebconfiguration-with-physical-path
                FileInfo fi = new FileInfo(currWebApp.WebConfigPath);
                VirtualDirectoryMapping vdm = new VirtualDirectoryMapping(
                    fi.DirectoryName, true, fi.Name);
                WebConfigurationFileMap wcfm = new WebConfigurationFileMap();
                wcfm.VirtualDirectories.Add("/", vdm);

                currConf =
                    WebConfigurationManager.OpenMappedWebConfiguration(wcfm, "/");

                chgEncryptButtonStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                btnExecute.Enabled = false;
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (currConf == null) return;
            try
            {
                ConfigurationSection cnStrSec = currConf.ConnectionStrings;
                string provider = txtRsaProvider.Text;
                if (string.IsNullOrEmpty(provider))
                    provider = "RsaProtectedConfigurationProvider";
                else
                {
                    //Create custom provider
                    ProtectedConfigurationSection pdSec = 
                        currConf.GetSection("configProtectedData") as ProtectedConfigurationSection;
                    if (pdSec == null)
                    {
                        pdSec = new ProtectedConfigurationSection();
                        currConf.Sections.Add("configProtectedData", pdSec);
                     }
                    bool found = false;
                    foreach (ProviderSettings p in pdSec.Providers)
                        if (p.Name == provider) found = true;
                    if (!found)
                    {
                        ProviderSettings ps = new ProviderSettings();
                        ps.Name = provider;
                        ps.Type = "System.Configuration.RsaProtectedConfigurationProvider,System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
                        ps.Parameters.Add("keyContainerName", provider);
                        ps.Parameters.Add("useMachineContainer", "true");
                        pdSec.Providers.Add(ps);
                        //Note: A new key container will be created silently, if the key container name doesn't exist.
                        //      But the auto created key container is not exportable and inappropriate for shared key among
                        //      web farm servers.  Please use RSA key manager to create exportable key container for web farm
                        //      scenarios.
                    }
                }

                if (btnExecute.Text == Resources.Message.Encrypt)
                    cnStrSec.SectionInformation.ProtectSection(provider);
                else
                    cnStrSec.SectionInformation.UnprotectSection();
                currConf.Save();
                cbxWebApp_SelectedIndexChanged(cbxWebApp, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btmManageKeys_Click(object sender, EventArgs e)
        {
            KeyManager km = new KeyManager();
            km.ShowDialog();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            cbxWebApp_SelectedIndexChanged(cbxWebApp, e);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = "notepad.exe";
            p.StartInfo.Arguments = (cbxWebApp.SelectedItem as WebAppInfo).WebConfigPath;
            p.Start();
        }

        #region Langauge
        private void applyCulture(ComponentResourceManager res, Control c)
        {
            foreach (Control child in c.Controls)
            {
                res.ApplyResources(child, child.Name);
                applyCulture(res, child);
            }
        }

        private void changeLang(string culture)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            ComponentResourceManager res = new ComponentResourceManager(this.GetType());
            applyCulture(res, this);
            chgEncryptButtonStatus();
        }

        private void mnuENUS_Click(object sender, EventArgs e)
        {
           changeLang("en-US");
        }
        private void mnuZHTW_Click(object sender, EventArgs e)
        {
            changeLang("zh-TW");
        }
        #endregion


    }
}
