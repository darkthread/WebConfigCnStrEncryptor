namespace WebConfigEncryptor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.cbxWebApp = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ddlLang = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnuENUS = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZHTW = new System.Windows.Forms.ToolStripMenuItem();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnReload = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRsaProvider = new System.Windows.Forms.TextBox();
            this.btmManageKeys = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.wbEditor = new System.Windows.Forms.WebBrowser();
            this.btnExecute = new System.Windows.Forms.Button();
            this.wkrQuery = new System.ComponentModel.BackgroundWorker();
            this.statusStrip1.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbxWebApp
            // 
            resources.ApplyResources(this.cbxWebApp, "cbxWebApp");
            this.cbxWebApp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxWebApp.FormattingEnabled = true;
            this.cbxWebApp.Name = "cbxWebApp";
            this.cbxWebApp.SelectedIndexChanged += new System.EventHandler(this.cbxWebApp_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ddlLang,
            this.lblStatus});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // ddlLang
            // 
            this.ddlLang.BackColor = System.Drawing.Color.Silver;
            this.ddlLang.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ddlLang.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuENUS,
            this.mnuZHTW});
            this.ddlLang.Name = "ddlLang";
            this.ddlLang.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            resources.ApplyResources(this.ddlLang, "ddlLang");
            // 
            // mnuENUS
            // 
            this.mnuENUS.Name = "mnuENUS";
            resources.ApplyResources(this.mnuENUS, "mnuENUS");
            this.mnuENUS.Click += new System.EventHandler(this.mnuENUS_Click);
            // 
            // mnuZHTW
            // 
            this.mnuZHTW.Name = "mnuZHTW";
            resources.ApplyResources(this.mnuZHTW, "mnuZHTW");
            this.mnuZHTW.Click += new System.EventHandler(this.mnuZHTW_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            resources.ApplyResources(this.lblStatus, "lblStatus");
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.label3);
            this.pnlMain.Controls.Add(this.btnReload);
            this.pnlMain.Controls.Add(this.label2);
            this.pnlMain.Controls.Add(this.txtRsaProvider);
            this.pnlMain.Controls.Add(this.btmManageKeys);
            this.pnlMain.Controls.Add(this.btnEdit);
            this.pnlMain.Controls.Add(this.wbEditor);
            this.pnlMain.Controls.Add(this.btnExecute);
            this.pnlMain.Controls.Add(this.label1);
            this.pnlMain.Controls.Add(this.cbxWebApp);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Name = "pnlMain";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // btnReload
            // 
            resources.ApplyResources(this.btnReload, "btnReload");
            this.btnReload.Name = "btnReload";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtRsaProvider
            // 
            resources.ApplyResources(this.txtRsaProvider, "txtRsaProvider");
            this.txtRsaProvider.Name = "txtRsaProvider";
            // 
            // btmManageKeys
            // 
            resources.ApplyResources(this.btmManageKeys, "btmManageKeys");
            this.btmManageKeys.Name = "btmManageKeys";
            this.btmManageKeys.UseVisualStyleBackColor = true;
            this.btmManageKeys.Click += new System.EventHandler(this.btmManageKeys_Click);
            // 
            // btnEdit
            // 
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // wbEditor
            // 
            resources.ApplyResources(this.wbEditor, "wbEditor");
            this.wbEditor.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbEditor.Name = "wbEditor";
            // 
            // btnExecute
            // 
            resources.ApplyResources(this.btnExecute, "btnExecute");
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // wkrQuery
            // 
            this.wkrQuery.WorkerReportsProgress = true;
            this.wkrQuery.DoWork += new System.ComponentModel.DoWorkEventHandler(this.wkrQuery_DoWork);
            this.wkrQuery.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.wkrQuery_ProgressChanged);
            this.wkrQuery.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.wkrQuery_RunWorkerCompleted);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbxWebApp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Button btnExecute;
        private System.ComponentModel.BackgroundWorker wkrQuery;
        private System.Windows.Forms.WebBrowser wbEditor;
        private System.Windows.Forms.Button btmManageKeys;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.TextBox txtRsaProvider;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripDropDownButton ddlLang;
        private System.Windows.Forms.ToolStripMenuItem mnuENUS;
        private System.Windows.Forms.ToolStripMenuItem mnuZHTW;
    }
}

