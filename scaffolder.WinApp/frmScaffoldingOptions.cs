using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using scaffolder.Generators;
using System.Diagnostics;
using System.IO;
using scaffolder.Types;
using scaffolder.Factories;

namespace scaffolder.WinApp
{
    public partial class frmScaffoldingOptions : Form
    {
        private ScaffoldingParameters _scaffoldingParams;
        private Form _previousForm;

        public frmScaffoldingOptions(ScaffoldingParameters parameters, Form previousForm)
        {
            _previousForm = previousForm;
            _scaffoldingParams = parameters;
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (txtNamespace.Text.Trim().Length == 0)
            {
                MessageBox.Show("I need to know the root namespace to use.");
                txtNamespace.Focus();
                return;
            }

            if (txtOutputPath.Text.Trim().Length == 0)
            {
                MessageBox.Show("I need to know where to store the generated files.");
                txtOutputPath.Focus();
                return;
            }

            try
            {
                if (Int32.Parse(appPort.Text) <= 0)
                {
                    MessageBox.Show("You cannot use negative numbers or 0 as a port");
                    return;
                }
                if (Int32.Parse(appPort.Text) <= 1080)
                {
                    var dialog = MessageBox.Show("Ports from 1 to 1080 are reserved for windows, you may need to run VS as admin, do you want to continue?", "Port Reserverd", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.No)
                    {
                        return;
                    }
                }
            }
            catch
            {
                MessageBox.Show("The port must be an non decimal number");
                return;
            }

            if(LogicDelete.Checked == true && String.IsNullOrWhiteSpace(LogicDeleteName.Text))
            {
                MessageBox.Show("Please specify the delete field");
                return;
            }

            if (!Directory.Exists(txtOutputPath.Text.Trim()))
            {
                Directory.CreateDirectory(txtOutputPath.Text.Trim());
            }

            if (!chkGenerateModels.Checked && !chkGenerateControllers.Checked && !chkIncludeDataAnnotations.Checked)
            {
                MessageBox.Show("OK, here goes.... NOTHING!! Choose something");
                return;               
            }

            Scaffold();
        }

        private void chkGenerateModels_CheckedChanged(object sender, EventArgs e)
        {
            chkIncludeDataAnnotations.Enabled = chkGenerateModels.Checked;
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            if (folderDlg.ShowDialog() != DialogResult.OK)
                return;

            txtOutputPath.Text = folderDlg.SelectedPath;
        }

        private void Scaffold()
        {
            var conf = new Configuration();
            conf.Namespace = txtNamespace.Text.Trim();
            conf.ConnectionString = _scaffoldingParams.SelectedProvider.ConnectionString;
            conf.ApplicationPort = Int32.Parse(appPort.Text);
            conf.OutputPath = txtOutputPath.Text.Trim();
            conf.GenerateClasses = chkGenerateModels.Checked;
            conf.IncludeDataAnnotations = chkIncludeDataAnnotations.Enabled && chkIncludeDataAnnotations.Checked;
            conf.IncludeRepositoryMethods = false;
            conf.GenerateControllers = chkGenerateControllers.Checked;
            conf.UseLogicDelete = LogicDelete.Checked;
            conf.DeleteField = LogicDeleteName.Text;

            var generator = new CSCodeGenerator();
            generator.Generate(_scaffoldingParams.TablesToScaffold, conf, _scaffoldingParams.Database);
            Process.Start(conf.OutputPath);
        }

        /*Deprecated Function*/
        private IRepositoryGenerator GetChosenRepoGenerator()
        {
            
                return null;

            /*var chosenGenerator = (RepositoryGeneratorType)Enum.Parse(typeof(RepositoryGeneratorType), cbxRepoGenerator.SelectedItem.ToString());
            
            return RepositoryGeneratorFactory.GetGenerator(chosenGenerator);*/
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _previousForm.Show();
            this.Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmScaffoldingOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void chkGenerateControllers_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void frmScaffoldingOptions_Load(object sender, EventArgs e)
        {

        }

        private void appPort_Leave(object sender, EventArgs e)
        {
            
        }

        private void appPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
        }

        private void appPort_TextChanged(object sender, EventArgs e)
        {
               
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            LogicDeleteName.Visible = ((CheckBox)sender).Checked;
        }
    }
}
