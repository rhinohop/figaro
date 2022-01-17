namespace scaffolder.WinApp
{
    partial class frmScaffoldingOptions
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.LogicDeleteName = new System.Windows.Forms.TextBox();
            this.LogicDelete = new System.Windows.Forms.CheckBox();
            this.chkGenerateControllers = new System.Windows.Forms.CheckBox();
            this.chkIncludeDataAnnotations = new System.Windows.Forms.CheckBox();
            this.chkGenerateModels = new System.Windows.Forms.CheckBox();
            this.txtNamespace = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.folderDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.btnExit = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.appPort = new System.Windows.Forms.TextBox();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(260, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Where should I place the generated files?";
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Location = new System.Drawing.Point(16, 29);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(374, 26);
            this.txtOutputPath.TabIndex = 1;
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Location = new System.Drawing.Point(396, 27);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseOutput.TabIndex = 2;
            this.btnBrowseOutput.Text = "Browse...";
            this.btnBrowseOutput.UseVisualStyleBackColor = true;
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.checkBox4);
            this.grpOptions.Controls.Add(this.checkBox3);
            this.grpOptions.Controls.Add(this.checkBox2);
            this.grpOptions.Controls.Add(this.checkBox1);
            this.grpOptions.Controls.Add(this.LogicDeleteName);
            this.grpOptions.Controls.Add(this.LogicDelete);
            this.grpOptions.Controls.Add(this.chkGenerateControllers);
            this.grpOptions.Controls.Add(this.chkIncludeDataAnnotations);
            this.grpOptions.Controls.Add(this.chkGenerateModels);
            this.grpOptions.Location = new System.Drawing.Point(16, 111);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(455, 232);
            this.grpOptions.TabIndex = 4;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "What exactly should I generate?";
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(6, 197);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(158, 23);
            this.checkBox4.TabIndex = 15;
            this.checkBox4.Text = "Generate VUE CRUD";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(6, 174);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(172, 23);
            this.checkBox3.TabIndex = 14;
            this.checkBox3.Text = "Generate REACT CRUD";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(6, 151);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(195, 23);
            this.checkBox2.TabIndex = 13;
            this.checkBox2.Text = "Generate ANGULAR CRUD";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 127);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(112, 23);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "Generate API";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // LogicDeleteName
            // 
            this.LogicDeleteName.Location = new System.Drawing.Point(261, 65);
            this.LogicDeleteName.Name = "LogicDeleteName";
            this.LogicDeleteName.Size = new System.Drawing.Size(188, 26);
            this.LogicDeleteName.TabIndex = 11;
            this.LogicDeleteName.Visible = false;
            // 
            // LogicDelete
            // 
            this.LogicDelete.Location = new System.Drawing.Point(23, 68);
            this.LogicDelete.Name = "LogicDelete";
            this.LogicDelete.Size = new System.Drawing.Size(149, 17);
            this.LogicDelete.TabIndex = 6;
            this.LogicDelete.Text = "Use a logic delete field";
            this.LogicDelete.UseVisualStyleBackColor = true;
            this.LogicDelete.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // chkGenerateControllers
            // 
            this.chkGenerateControllers.AutoSize = true;
            this.chkGenerateControllers.Checked = true;
            this.chkGenerateControllers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGenerateControllers.Location = new System.Drawing.Point(6, 94);
            this.chkGenerateControllers.Name = "chkGenerateControllers";
            this.chkGenerateControllers.Size = new System.Drawing.Size(346, 23);
            this.chkGenerateControllers.TabIndex = 5;
            this.chkGenerateControllers.Text = "Generate basic CRUD style API Controllers for MVC";
            this.chkGenerateControllers.UseVisualStyleBackColor = true;
            this.chkGenerateControllers.CheckedChanged += new System.EventHandler(this.chkGenerateControllers_CheckedChanged);
            // 
            // chkIncludeDataAnnotations
            // 
            this.chkIncludeDataAnnotations.AutoSize = true;
            this.chkIncludeDataAnnotations.Checked = true;
            this.chkIncludeDataAnnotations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeDataAnnotations.Location = new System.Drawing.Point(23, 45);
            this.chkIncludeDataAnnotations.Name = "chkIncludeDataAnnotations";
            this.chkIncludeDataAnnotations.Size = new System.Drawing.Size(274, 23);
            this.chkIncludeDataAnnotations.TabIndex = 2;
            this.chkIncludeDataAnnotations.Text = "Include Data Annotations for properties";
            this.chkIncludeDataAnnotations.UseVisualStyleBackColor = true;
            // 
            // chkGenerateModels
            // 
            this.chkGenerateModels.Checked = true;
            this.chkGenerateModels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGenerateModels.Location = new System.Drawing.Point(7, 22);
            this.chkGenerateModels.Name = "chkGenerateModels";
            this.chkGenerateModels.Size = new System.Drawing.Size(114, 17);
            this.chkGenerateModels.TabIndex = 1;
            this.chkGenerateModels.Text = "Generate Models";
            this.chkGenerateModels.UseVisualStyleBackColor = true;
            this.chkGenerateModels.CheckedChanged += new System.EventHandler(this.chkGenerateModels_CheckedChanged);
            // 
            // txtNamespace
            // 
            this.txtNamespace.Location = new System.Drawing.Point(16, 83);
            this.txtNamespace.Name = "txtNamespace";
            this.txtNamespace.Size = new System.Drawing.Size(238, 26);
            this.txtNamespace.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(287, 19);
            this.label2.TabIndex = 5;
            this.label2.Text = "What should I use as the default Namespace?";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(405, 349);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 6;
            this.btnGenerate.Text = "Generate!";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(315, 349);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 7;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(12, 349);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(274, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 19);
            this.label3.TabIndex = 9;
            this.label3.Text = "Which port should it use";
            // 
            // appPort
            // 
            this.appPort.Location = new System.Drawing.Point(277, 83);
            this.appPort.Name = "appPort";
            this.appPort.Size = new System.Drawing.Size(194, 26);
            this.appPort.TabIndex = 10;
            this.appPort.TextChanged += new System.EventHandler(this.appPort_TextChanged);
            this.appPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.appPort_KeyPress);
            this.appPort.Leave += new System.EventHandler(this.appPort_Leave);
            // 
            // frmScaffoldingOptions
            // 
            this.AcceptButton = this.btnGenerate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 384);
            this.Controls.Add(this.appPort);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNamespace);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.btnBrowseOutput);
            this.Controls.Add(this.txtOutputPath);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmScaffoldingOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Code Automatization";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmScaffoldingOptions_FormClosing);
            this.Load += new System.EventHandler(this.frmScaffoldingOptions_Load);
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.TextBox txtNamespace;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkIncludeDataAnnotations;
        private System.Windows.Forms.CheckBox chkGenerateModels;
        private System.Windows.Forms.CheckBox chkGenerateControllers;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.FolderBrowserDialog folderDlg;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox appPort;
        private System.Windows.Forms.CheckBox LogicDelete;
        private System.Windows.Forms.TextBox LogicDeleteName;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}