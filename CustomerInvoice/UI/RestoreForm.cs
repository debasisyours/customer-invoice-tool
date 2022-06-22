using CustomerInvoice.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomerInvoice.UI
{
    public partial class RestoreForm : Form
    {
        #region Constructor

        public RestoreForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.InitializeControls();
            this.AssignEventHandlers();
        }

        #endregion

        #region Custom functions

        private void AssignEventHandlers()
        {
            this.btnBrowse.Click += new EventHandler(OnBrowse);
            this.btnCancel.Click += new EventHandler(OnClose);
            this.btnRestore.Click += new EventHandler(OnRestore);
        }

        private void InitializeControls()
        {
            string serverName = string.Empty;
            string connectionProperty = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            List<string> properties = connectionProperty.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            serverName = properties.Where(s => s.StartsWith("Data Source")).FirstOrDefault().Substring(12).Trim();

            Microsoft.SqlServer.Management.Smo.Server server = new Microsoft.SqlServer.Management.Smo.Server(serverName);

            this.cboDatabase.Items.Clear();
            foreach (var database in server.Databases)
            {
                this.cboDatabase.Items.Add(database);
            }
        }

        private void OnBrowse(object sender,EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                DefaultExt = ".bak",
                Title = "Select backup file",
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "SQL Server Backup Files(*.bak)|*.bak|All Files(*.*)|*.*",
                FilterIndex = 0
            };

            DialogResult result = openFileDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                this.txtFile.Text = openFileDialog.FileName;
            }
        }

        private void OnClose(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OnRestore(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(this.cboDatabase.Text))
            {
                MessageBox.Show(this, "Database should be selected first.", "Database not selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.cboDatabase.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(this.txtFile.Text))
            {
                MessageBox.Show(this, "Back file is not selected.", "Backup file not selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.btnBrowse.Focus();
                return;
            }

            bool success = DataLayer.RestoreDatabase(this.txtFile.Text, this.cboDatabase.Text);
            if (success)
            {
                MessageBox.Show(this, "Database restoration completed successfully", "Restore complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "Database restoration could not be completed, please check log file", "Restore failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion
    }
}
