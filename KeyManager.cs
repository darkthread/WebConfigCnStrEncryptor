using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;

namespace WebConfigEncryptor
{
    public partial class KeyManager : Form
    {
        public KeyManager()
        {
            InitializeComponent();
        }

        private void KeyManager_Load(object sender, EventArgs e)
        {
            //Detect runtime version
            //REF http://west-wind.com/weblog/posts/1646.aspx
            RegistryKey reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\ASP.NET");
            foreach (string ver in reg.GetSubKeyNames())
            {
                //Skip v1.1
                if (ver.StartsWith("1.1")) continue;
                RadioButton rb = new RadioButton();
                rb.Text = ver;
                string path = Convert.ToString(reg.OpenSubKey(ver).GetValue("Path"));
                if (string.IsNullOrEmpty(path)) continue;
                rb.Tag = path;
                if (File.Exists(Path.Combine(path, "aspnet_regiis.exe")))
                    flpVersions.Controls.Add(rb);
            }
            (flpVersions.Controls[0] as RadioButton).Checked = true;
        }

        //Execute external program and get the result string
        public string Shell(string exeFile, string argument)
        {
            Process pShell = new Process();
            pShell.StartInfo.FileName = exeFile;
            pShell.StartInfo.Arguments = argument;
            //For output redirect
            pShell.StartInfo.UseShellExecute = false;
            pShell.StartInfo.RedirectStandardOutput = true;
            //No window
            pShell.StartInfo.CreateNoWindow = true;
            //Start
            pShell.Start();
            pShell.WaitForExit();
            //Convert StandardOutput to string
            return pShell.StandardOutput.ReadToEnd();
        }

        private string getAspNetRegIisPath()
        {
            foreach (RadioButton rb in flpVersions.Controls)
            {
                if (rb.Checked)
                    return Path.Combine(rb.Tag.ToString(), "aspnet_regiis.exe");
            }
            throw new ApplicationException(Resources.Message.NoAspNetRegiis);
        }

        private bool containerNameValid()
        {
            if (string.IsNullOrEmpty(txtContainerName.Text))
            {
                MessageBox.Show(Resources.Message.ContainerNameMissing);
                return false;
            }
            else return true;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!containerNameValid()) return;
            string res = Shell(getAspNetRegIisPath(), string.Format("-pc \"{0}\" -exp", txtContainerName.Text));
            txtResult.Text = res;
        }

        private void btbDelete_Click(object sender, EventArgs e)
        {
            if (!containerNameValid()) return;
            if (MessageBox.Show(string.Format(
                Resources.Message.ContainerDeleteWarning, 
                txtContainerName.Text), 
                Resources.Message.ContainerDeleteWarningCaption, MessageBoxButtons.YesNo)
                == System.Windows.Forms.DialogResult.No) return;
            string res = Shell(getAspNetRegIisPath(), string.Format("-pz \"{0}\"", txtContainerName.Text));
            txtResult.Text = res;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (!containerNameValid()) return;
            saveFileDialog1.Filter = "XML|*.xml";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string res = Shell(getAspNetRegIisPath(), string.Format("-px \"{0}\" \"{1}\" -pri",
                    txtContainerName.Text, saveFileDialog1.FileName));
                txtResult.Text = res;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (!containerNameValid()) return;
            openFileDialog1.Filter = "XML|*.xml";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string res = Shell(getAspNetRegIisPath(), string.Format("-pi \"{0}\" \"{1}\"",
                    txtContainerName.Text, openFileDialog1.FileName));
                txtResult.Text = res;
            }

        }

        private void btnGrant_Click(object sender, EventArgs e)
        {
            if (!containerNameValid()) return;
            string res = Shell(getAspNetRegIisPath(), string.Format("-pa \"{0}\" \"{1}\"",
                txtContainerName.Text, txtIdentity.Text));
            txtResult.Text = res;
        }
    }
}
