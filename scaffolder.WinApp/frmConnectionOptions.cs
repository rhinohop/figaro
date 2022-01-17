using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using scaffolder.Providers;
using scaffolder.Types;
using scaffolder.Factories;

namespace scaffolder.WinApp
{
    public partial class frmConnectionOptions : Form
    {
        public frmConnectionOptions()
        {
            InitializeComponent();

            var sqlProviders = Enum.GetNames(typeof(SqlProviderType));
            cbxDbEngine.Items.Clear();
            cbxDbEngine.Items.AddRange(sqlProviders);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (!HasAllRequiredFields())
            {
                MessageBox.Show("I can't move on until you tell me about your database.");
                return;
            }

            if (!TestConnection())
            {
                MessageBox.Show("I couldn't connect to the database. Check the connection settings.");
                return;            
            }

            var parameters = new ScaffoldingParameters();
            parameters.Database = this.Catalog.Text;
            parameters.SelectedProvider = GetChosenProvider();
            parameters.SelectedProvider.ConnectionString = GenerateConnectionString();

            var frmdbObjects = new frmDatabaseObjects(parameters, this);
            frmdbObjects.Show();

            this.Hide();
        }

        private bool HasAllRequiredFields()
        {
            return (this.ServerList.Text.Trim().Length > 0 && (AuthType.SelectedIndex == 0 || (AuthType.SelectedIndex == 1 && UserText.Text.Trim().Length > 0 && PasswordText.Text.Trim().Length > 0)) &&
                    cbxDbEngine.SelectedIndex >= 0);
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            if (!HasAllRequiredFields())
            {
                MessageBox.Show("I need more information about your database in order to test it.");
                return;
            } 
            
            string testResult = TestConnection() ? "succeeded." : "failed :(";

            MessageBox.Show("The connection test " + testResult);        
        }

        private bool TestConnection()
        { 
            var provider = GetChosenProvider();
            provider.ConnectionString = GenerateConnectionString();
            return provider.TestConnection();
        }

        private String GenerateConnectionString()
        {
            if(cbxDbEngine.SelectedIndex == -1)
            {
                return null;
            }
            if(AuthType.SelectedIndex == -1)
            {
                return null;
            }
            if (cbxDbEngine.SelectedIndex == 0)
            {
                if (AuthType.SelectedIndex == 0)
                {
                    return String.Format("Data Source={0};Initial Catalog={1};Integrated Security=SSPI;MultipleActiveResultSets=True;", ServerList.Text.Trim(), Catalog.Text.Trim());
                }
                else
                {
                    return String.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};MultipleActiveResultSets=True;", ServerList.Text.Trim(), Catalog.Text.Trim(), UserText.Text.Trim(), PasswordText.Text.Trim());
                }
            }
            else
            {
                if (AuthType.SelectedIndex == 0)
                {
                    MessageBox.Show("PostgreSQL no soporta SSPI");
                    return null;
                }
                else
                {
                    return String.Format("Provider=PostgreSQL OLE DB Provider;Data Source={0};location={1};User ID={2};password={3};", ServerList.Text.Trim(), Catalog.Text.Trim(), UserText.Text.Trim(), PasswordText.Text.Trim());
                }
            }
        }

        private ISqlProvider GetChosenProvider()
        {
            if (cbxDbEngine.SelectedIndex == -1)
                return null;

            SqlProviderType selectedProvider = (SqlProviderType)Enum.Parse(typeof(SqlProviderType), cbxDbEngine.SelectedItem.ToString());
            
            return SqlProviderFactory.GetSqlProvider(selectedProvider);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmConnectionOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void ServerList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void frmConnectionOptions_Load(object sender, EventArgs e)
        {

        }

        private void cbxDbEngine_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
