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
    public partial class UserForm : Form
    {
        #region Private Field declaration

        private UserDataSet _userData = null;
        private int _SelectedUserId = 0;

        #endregion

        public UserForm()
        {
            InitializeComponent();
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.InitializeFormControls();
            this.AssignHandlers();
        }

        #endregion

        #region Custom methods

        private void AssignHandlers()
        {
            this.btnClose.Click += new EventHandler(OnClose);
            this.btnSave.Click += new EventHandler(OnSaveUser);
            this.btnCancel.Click += new EventHandler(ClearControls);
            this.dgvUsers.CellClick += new DataGridViewCellEventHandler(OnUserSelected);
        }

        private void OnClose(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool IsDataValid()
        {
            bool valid = true;
            bool companySelected = false;
            if (string.IsNullOrEmpty(this.txtUserName.Text))
            {
                MessageBox.Show(this,"User name can not be left blank",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.txtUserName.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtPassword.Text))
            {
                MessageBox.Show(this, "Password can not be left blank", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtPassword.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtConfirm.Text))
            {
                MessageBox.Show(this, "Confirm password can not be left blank", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtConfirm.Focus();
                return false;
            }
            if (string.Compare(this.txtPassword.Text, this.txtConfirm.Text, false) != 0)
            {
                MessageBox.Show(this, "Confirm password does not match with password", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtConfirm.Focus();
                return false;
            }

            if (this.lstCompanies.Items.Count > 0)
            {
                for (int count = 0; count < this.lstCompanies.Items.Count; count++)
                {
                    if (this.lstCompanies.GetItemChecked(count))
                    {
                        companySelected = true;
                        break;
                    }
                }
            }

            if (!companySelected)
            {
                MessageBox.Show(this, "At least one company has to be mapped with a user", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.lstCompanies.Focus();
                return false;
            }
            
            return valid;
        }

        private void OnSaveUser(object sender, EventArgs e)
        {
            if (!this.IsDataValid()) return;

            List<int> companies = new List<int>();
            User user = new User();
            Company tmpCompany=null;
            
            if (this._SelectedUserId > 0)
            {
                user.ID = this._SelectedUserId;
            }
            user.Name = this.txtUserName.Text;
            user.Password = this.txtPassword.Text;
            user.IsActive = this.chkActive.Checked;

            if (this.lstCompanies.Items.Count > 0)
            {
                for (int count = 0; count < this.lstCompanies.Items.Count; count++)
                {
                    if (this.lstCompanies.GetItemChecked(count))
                    {
                        tmpCompany = DataLayer.GetCompanySingleForName(this.lstCompanies.Items[count].ToString());
                        companies.Add(tmpCompany.ID);
                    }
                }
            }

            if (DataLayer.SaveUser(user, companies))
            {
                MessageBox.Show(this, "User saved successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this._SelectedUserId = 0;
                this.InitializeFormControls();
                this.btnCancel.PerformClick();
            }
            else
            {
                MessageBox.Show(this, "User could not be saved please check logfile", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ClearControls(object sender, EventArgs e)
        {
            this.txtUserName.Clear();
            this.txtPassword.Clear();
            this.txtConfirm.Clear();
            this._SelectedUserId = 0;
            this.chkActive.Checked = false;
            this.PopulateCompanyList();
        }

        private void InitializeFormControls()
        {
            this.FormatGrid();
            this.PopulateCompanyList();
        }

        private void PopulateCompanyList()
        {
            CompanyDataSet companyData = DataLayer.PopulateCompanies();
            this.lstCompanies.Items.Clear();
            if (companyData.Tables[CompanyDataSet.TableCompany].Rows.Count > 0)
            {
                foreach (DataRow row in companyData.Tables[CompanyDataSet.TableCompany].Rows)
                {
                    this.lstCompanies.Items.Add(row[CompanyDataSet.NameColumn], false);
                }
            }
        }

        private void FormatGrid()
        {
            DataGridViewColumn column = null;
            DataGridViewCheckBoxColumn checkColumn = null;

            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AllowUserToResizeRows = false;
            this.dgvUsers.AutoGenerateColumns = false;
            this.dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.BackgroundColor = Color.White;

            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.dgvUsers.Columns.Clear();

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = UserDataSet.IdColumn;
            column.FillWeight = 20;
            column.HeaderText = "ID";
            column.Name = UserDataSet.IdColumn;
            column.Width = 20;
            column.Visible = false;
            this.dgvUsers.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = UserDataSet.NameColumn;
            column.FillWeight = 100;
            column.HeaderText = "Name";
            column.Name = UserDataSet.NameColumn;
            column.Width = 100;
            this.dgvUsers.Columns.Add(column);

            checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.DataPropertyName = UserDataSet.IsActiveColumn;
            checkColumn.FillWeight = 60;
            checkColumn.HeaderText = "Active";
            checkColumn.Name = UserDataSet.IsActiveColumn;
            checkColumn.Width = 60;
            this.dgvUsers.Columns.Add(checkColumn);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = UserDataSet.CompanyCountColumn;
            column.FillWeight = 100;
            column.HeaderText = "No of companies";
            column.Name = UserDataSet.CompanyCountColumn;
            column.Width = 100;
            this.dgvUsers.Columns.Add(column);

            this._userData = DataLayer.PopulateUsers();

            this.dgvUsers.DataSource = this._userData.Tables[UserDataSet.TableUsers];
            this.dgvUsers.Columns[UserDataSet.IdColumn].Visible = false;
        }

        private void PopulateFormControls(int userId)
        {
            User userData = DataLayer.GetUserSingle(userId);
            Company tmpCompany = null;
            List<int> companies = DataLayer.GetAssociatedCompanies(userId);
            if (userData != null)
            {
                this.txtUserName.Text = userData.Name;
                this.txtPassword.Text = userData.Password;
                this.txtConfirm.Text = userData.Password;
                this.chkActive.Checked = userData.IsActive;

                for(int count=0;count<this.lstCompanies.Items.Count;count++)
                {
                    this.lstCompanies.SetItemChecked(count,false);
                }

                foreach (int companyId in companies)
                {
                    tmpCompany = DataLayer.GetCompanySingle(companyId);
                    for (int count = 0; count < this.lstCompanies.Items.Count; count++)
                    {
                        if (string.Compare(this.lstCompanies.Items[count].ToString(), tmpCompany.Name, false) == 0)
                        {
                            this.lstCompanies.SetItemChecked(count, true);
                        }
                    }
                }
            }
        }

        #endregion

        #region GridView events

        private void OnUserSelected(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = null;
            if (this.dgvUsers.SelectedRows.Count == 0) return;
            selectedRow = this.dgvUsers.SelectedRows[0];
            this._SelectedUserId = Convert.ToInt32(selectedRow.Cells[UserDataSet.IdColumn].Value);
            this.PopulateFormControls(this._SelectedUserId);
        }

        #endregion
    }
}
