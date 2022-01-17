using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace scaffolder.WinApp
{
    public partial class frmDatabaseObjects : Form
    {
        private Form _previousForm;

        public ScaffoldingParameters ScaffoldingParams { get; set; }

        public frmDatabaseObjects()
        {
            InitializeComponent();
        }

        public frmDatabaseObjects(ScaffoldingParameters parameters, Form previousForm): this()
        {
            ScaffoldingParams = parameters;
            _previousForm = previousForm;
        }

        private void frmDatabaseObjects_Load(object sender, EventArgs e)
        {
            AddTablesToList();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (lbxDatabaseObjects.CheckedItems.Count <= 0)
            {
                MessageBox.Show("I can't move on until you select at least one table to scaffold.");
                return;
            }

            ScaffoldingParams.TablesToScaffold = ScaffoldingParams.SelectedProvider.GetFullTableInfo(lbxDatabaseObjects.CheckedItems.Cast<string>());
            var nextfrm = new frmScaffoldingOptions(ScaffoldingParams, this);
            nextfrm.Show();
            this.Visible = false;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _previousForm.Show();
            this.Hide();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            AddTablesToList();
        }

        private void AddTablesToList()
        { 
            lbxDatabaseObjects.Items.Clear();
            lbxDatabaseObjects.Items.AddRange(ScaffoldingParams.SelectedProvider.GetAvailableTables().ToArray<string>());       
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            SetCheckedForAllItems(false);
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            SetCheckedForAllItems(true);
        }

        private void SetCheckedForAllItems(bool checkedValue)
        {
            lbxDatabaseObjects.BeginUpdate();
            for (int i = 0; i < lbxDatabaseObjects.Items.Count; i++)
            {
                lbxDatabaseObjects.SetItemChecked(i, checkedValue);
            }
            lbxDatabaseObjects.EndUpdate();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmDatabaseObjects_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
