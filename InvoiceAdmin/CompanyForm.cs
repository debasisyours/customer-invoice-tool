using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using CustomerInvoice.Common;
using CustomerInvoice.Data;
using CustomerInvoice.Data.DataSets;

namespace InvoiceAdmin
{
    public partial class CompanyForm : Form
    {
        #region Field declaration

        private CompanyDataSet _CompanyData = null;
        private int _SelectedCompanyId = 0;

        #endregion

        public CompanyForm()
        {
            InitializeComponent();
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DataLayer.UpdateDatabase();
            this.InitializeFormControls();
            this.AssignEventHandlers();
        }

        #endregion

        #region Custom methods

        private void InitializeFormControls()
        {
            this.FormatGrid();
        }

        private void AssignEventHandlers()
        {
            this.btnClose.Click += new EventHandler(OnClose);
            this.btnCancel.Click += new EventHandler(OnClearForm);
            this.btnSave.Click += new EventHandler(OnSave);
            this.btnUsers.Click += new EventHandler(OnUsersClick);
            this.dgvCompanies.CellClick += new DataGridViewCellEventHandler(OnCompanySelected);
        }

        private void OnClose(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OnUsersClick(object sender, EventArgs e)
        {
            using (UserForm users = new UserForm())
            {
                DialogResult result = users.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.InitializeFormControls();
                }
            }
        }

        private void OnSave(object sender, EventArgs e)
        {
            if (!this.IsDataValid()) return;

            Company company = null;

            if (this._SelectedCompanyId > 0)
            {
                company = DataLayer.GetCompanySingle(this._SelectedCompanyId);
            }
            else
            {
                company = new Company();
            }
            company.Address = this.txtAddress.Text;
            company.City = this.txtCity.Text;
            company.Code = this.txtCode.Text;
            company.Country = this.txtCountry.Text;
            company.Email = this.txtEmail.Text;
            company.Fax = this.txtFax.Text;
            company.Name = this.txtName.Text;
            company.Phone = this.txtPhone.Text;
            company.State = this.txtState.Text;
            company.URL = this.txtUrl.Text;
            company.ZIP = this.txtZip.Text;
            company.AccountCode = this.txtAccountCode.Text;
            company.AccountNumber = this.txtAccountNo.Text;

            if (DataLayer.SaveCompany(company))
            {
                MessageBox.Show(this, "Company details saved successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this._SelectedCompanyId = 0;
                this.FormatGrid();
            }
            else
            {
                MessageBox.Show(this, "Company details could not be saved please see log file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool IsDataValid()
        {
            bool valid = true;
            if (string.IsNullOrEmpty(this.txtCode.Text))
            {
                MessageBox.Show(this, "Company code can not be blank", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtCode.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtName.Text))
            {
                MessageBox.Show(this, "Company name can not be blank", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtName.Focus();
                return false;
            }
            return valid;
        }

        private void OnClearForm(object sender, EventArgs e)
        {
            this.txtAddress.Clear();
            this.txtCity.Clear();
            this.txtCode.Clear();
            this.txtCountry.Clear();
            this.txtEmail.Clear();
            this.txtFax.Clear();
            this.txtName.Clear();
            this.txtPhone.Clear();
            this.txtState.Clear();
            this.txtUrl.Clear();
            this.txtZip.Clear();
            this.txtAccountCode.Clear();
            this.txtAccountNo.Clear();
            this._SelectedCompanyId = 0;
        }

        private void FormatGrid()
        {
            DataGridViewColumn column = null;

            this.dgvCompanies.AllowUserToAddRows = false;
            this.dgvCompanies.AllowUserToDeleteRows = false;
            this.dgvCompanies.AllowUserToResizeRows = false;
            this.dgvCompanies.AutoGenerateColumns = false;
            this.dgvCompanies.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCompanies.BackgroundColor = Color.White;
            this.dgvCompanies.MultiSelect = false;
            this.dgvCompanies.ReadOnly = true;
            this.dgvCompanies.RowHeadersVisible = false;
            this.dgvCompanies.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.dgvCompanies.Columns.Clear();

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CompanyDataSet.IdColumn;
            column.FillWeight = 20;
            column.HeaderText = "ID";
            column.Name = CompanyDataSet.IdColumn;
            column.Width = 20;
            column.Visible = false;
            this.dgvCompanies.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CompanyDataSet.CodeColumn;
            column.FillWeight = 50;
            column.HeaderText = "Code";
            column.Name = CompanyDataSet.CodeColumn;
            column.Width = 50;
            this.dgvCompanies.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CompanyDataSet.NameColumn;
            column.FillWeight = 100;
            column.HeaderText = "Name";
            column.Name = CompanyDataSet.NameColumn;
            column.Width = 100;
            this.dgvCompanies.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CompanyDataSet.CityColumn;
            column.FillWeight = 100;
            column.HeaderText = "City";
            column.Name = CompanyDataSet.CityColumn;
            column.Width = 100;
            this.dgvCompanies.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CompanyDataSet.StateColumn;
            column.FillWeight = 100;
            column.HeaderText = "State";
            column.Name = CompanyDataSet.StateColumn;
            column.Width = 100;
            this.dgvCompanies.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CompanyDataSet.CountryColumn;
            column.FillWeight = 100;
            column.HeaderText = "Country";
            column.Name = CompanyDataSet.CountryColumn;
            column.Width = 100;
            this.dgvCompanies.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CompanyDataSet.PhoneColumn;
            column.FillWeight = 100;
            column.HeaderText = "Phone";
            column.Name = CompanyDataSet.PhoneColumn;
            column.Width = 100;
            this.dgvCompanies.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CompanyDataSet.EmailColumn;
            column.FillWeight = 100;
            column.HeaderText = "Email";
            column.Name = CompanyDataSet.EmailColumn;
            column.Width = 100;
            this.dgvCompanies.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CompanyDataSet.UrlColumn;
            column.FillWeight = 100;
            column.HeaderText = "URL";
            column.Name = CompanyDataSet.UrlColumn;
            column.Width = 100;
            this.dgvCompanies.Columns.Add(column);

            this._CompanyData = DataLayer.PopulateCompanies();
            this.dgvCompanies.DataSource = this._CompanyData.Tables[CompanyDataSet.TableCompany];
            this.dgvCompanies.Columns[CompanyDataSet.IdColumn].Visible = false;
        }

        private void PopulateFormControls(int companyId)
        {
            Company companyDetails = DataLayer.GetCompanySingle(companyId);
            if (companyDetails != null)
            {
                this.txtAddress.Text = companyDetails.Address;
                this.txtCity.Text = companyDetails.City;
                this.txtCode.Text = companyDetails.Code;
                this.txtCountry.Text = companyDetails.Country;
                this.txtEmail.Text = companyDetails.Email;
                this.txtFax.Text = companyDetails.Fax;
                this.txtName.Text = companyDetails.Name;
                this.txtPhone.Text = companyDetails.Phone;
                this.txtState.Text = companyDetails.State;
                this.txtUrl.Text = companyDetails.URL;
                this.txtZip.Text = companyDetails.ZIP;
                this.txtAccountCode.Text = companyDetails.AccountCode;
                this.txtAccountNo.Text = companyDetails.AccountNumber;
            }
        }

        #endregion

        #region DataGridView events

        private void OnCompanySelected(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow rowSelected = null;

            if (this.dgvCompanies.SelectedRows.Count == 0) return;
            rowSelected = this.dgvCompanies.SelectedRows[0];
            this._SelectedCompanyId = Convert.ToInt32(rowSelected.Cells[CompanyDataSet.IdColumn].Value);
            this.PopulateFormControls(this._SelectedCompanyId);
        }

        #endregion
    }
}
