using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CustomerInvoice.Common;
using CustomerInvoice.Data;

namespace CustomerInvoice.UI
{
    public partial class SettingsForm : Form
    {
        #region Field declaration

        private GlobalSetting _Settings = null;

        #endregion

        public SettingsForm()
        {
            InitializeComponent();
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.PopulatePath();
            this.AssignEventHandlers();
        }

        #endregion

        #region Custom functions

        private void AssignEventHandlers()
        {
            this.btnBrowseCustomer.Click += new EventHandler(OnBrowse);
            this.btnBrowseInvoice.Click += new EventHandler(OnBrowse);
            this.btnBrowsePdf.Click += new EventHandler(OnBrowse);
            this.btnSave.Click += new EventHandler(OnSave);
            this.btnCancel.Click += new EventHandler(OnExit);
        }

        private void OnBrowse(object sender, EventArgs e)
        {
            Button source = (Button)sender;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Application.StartupPath;
            dialog.Multiselect = false;
            dialog.RestoreDirectory = true;
            dialog.Title = "Browse to select a path";
            DialogResult result;

            switch (source.Name.Substring(9))
            {
                case "Invoice":
                    {
                        result = dialog.ShowDialog();
                        if (result == DialogResult.OK)
                            this.txtInvoiceExportPath.Text = dialog.FileName.Substring(0, dialog.FileName.LastIndexOf(@"\"));
                        break;
                    }
                case "Customer":
                    {
                        result = dialog.ShowDialog();
                        if (result == DialogResult.OK)
                            this.txtCustomerExportPath.Text = dialog.FileName.Substring(0, dialog.FileName.LastIndexOf(@"\"));
                        break;
                    }
                case "Pdf":
                    {
                        result = dialog.ShowDialog();
                        if (result == DialogResult.OK)
                            this.txtPdfExportPath.Text = dialog.FileName.Substring(0, dialog.FileName.LastIndexOf(@"\"));
                        break;
                    }
            }
        }

        private void OnSave(object sender, EventArgs e)
        {
            if (this._Settings == null) this._Settings = new GlobalSetting();
            
            this._Settings.CustomerExportPath = this.txtCustomerExportPath.Text;
            this._Settings.InvoiceExportPath = this.txtInvoiceExportPath.Text;
            this._Settings.PdfExportPath = this.txtPdfExportPath.Text;
            this._Settings.CompanyName = this.txtCompanyName.Text;
            this._Settings.CompanyAddress = this.txtCompanyAddress.Text;
            this._Settings.AccountName = this.txtAccountName.Text;
            this._Settings.AccountNumber = this.txtAccountNumber.Text;
            this._Settings.SortCode = this.txtSortCode.Text;
            this._Settings.SmtpFromAddress = this.txtFromAddress.Text;
            this._Settings.SmtpUser = this.txtUserName.Text;
            this._Settings.SmtpPassword = this.txtPassword.Text;
            this._Settings.CompanyId = Program.LoggedInCompanyId;

            if (DataLayer.SaveSetting(this._Settings))
            {
                MessageBox.Show(this, "Settings saved successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            else
            {
                MessageBox.Show(this, "Settings could not be saved, please check log file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void PopulatePath()
        {
            this._Settings = DataLayer.PopulateGlobalSettings(Program.LoggedInCompanyId);
            if (this._Settings != null)
            {
                this.txtInvoiceExportPath.Text = this._Settings.InvoiceExportPath;
                this.txtCustomerExportPath.Text = this._Settings.CustomerExportPath;
                this.txtPdfExportPath.Text = this._Settings.PdfExportPath;
                this.txtCompanyName.Text = this._Settings.CompanyName;
                this.txtCompanyAddress.Text = this._Settings.CompanyAddress;
                this.txtAccountName.Text = this._Settings.AccountName;
                this.txtAccountNumber.Text = this._Settings.AccountNumber;
                this.txtSortCode.Text = this._Settings.SortCode;
                this.txtFromAddress.Text = this._Settings.SmtpFromAddress;
                this.txtUserName.Text = this._Settings.SmtpUser;
                this.txtPassword.Text = this._Settings.SmtpPassword;
            }
            else
            {
                this.txtCustomerExportPath.Clear();
                this.txtInvoiceExportPath.Clear();
                this.txtPdfExportPath.Clear();
                this.txtCompanyName.Clear();
                this.txtCompanyAddress.Clear();
                this.txtAccountName.Clear();
                this.txtAccountNumber.Clear();
                this.txtSortCode.Clear();
                this.txtFromAddress.Clear();
                this.txtUserName.Clear();
                this.txtPassword.Clear();
                this._Settings = new GlobalSetting();
            }
        }

        #endregion
    }
}
