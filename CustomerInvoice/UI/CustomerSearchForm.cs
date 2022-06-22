using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CustomerInvoice.Data;
using CustomerInvoice.Data.DataSets;
using System.Globalization;
using CustomerInvoice.Common;

namespace CustomerInvoice.UI
{
    public partial class CustomerSearchForm : Form
    {

        #region Field declaration

        private CustomerDataSet _CustomerData = null;
        private CustomerDataSet _DeletedCustomerData = null;
        private bool _OpenedFromBreakdown = false;
        private int _SelectedCustomerId = 0;
        
        #endregion

        public CustomerSearchForm()
        {
            InitializeComponent();
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.InitializeControls();
            this.AssignEventHandlers();
            if (!this._OpenedFromBreakdown)
            {
                this.btnUse.Visible = false;
                this.btnAdd.Visible = true;
                this.btnEdit.Visible = true;
                this.btnDelete.Visible = true;
                this.btnSendLetter.Visible = true;
                this.chkRip.Checked = true;
            }
            else
            {
                this.btnUse.Visible = true;
                this.btnAdd.Visible = false;
                this.btnEdit.Visible = false;
                this.btnDelete.Visible = false;
                this.btnSendLetter.Visible = false;
            }
        }

        #endregion

        #region Custom methods

        private void AssignEventHandlers()
        {
            this.btnAdd.Click += new EventHandler(OnAddNewCustomer);
            this.btnEdit.Click += new EventHandler(OnEditCustomer);
            this.btnDelete.Click += new EventHandler(OnDeleteCustomer);
            this.btnCancel.Click += new EventHandler(OnExit);
            this.txtSearch.TextChanged += new EventHandler(OnSearchChanged);
            this.btnUse.Click += new EventHandler(OnUseCustomer);
            this.dgvCustomers.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(OnGridHeaderClick);
            this.dgvCustomers.CellFormatting += new DataGridViewCellFormattingEventHandler(OnRipFormatting);
            this.txtSearch.KeyDown += new KeyEventHandler(OnEnterKeyPress);
            this.dgvCustomers.KeyDown += new KeyEventHandler(OnEnterKeyPress);
            this.dgvCustomers.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(OnGridDoubleClick);
            this.btnSelectAll.Click+=new EventHandler(OnSelectAll);
            this.btnMakeActive.Click+=new EventHandler(OnUpdateActiveFlag);
            this.btnExportExcel.Click += new EventHandler(OnExcelExport);
            this.btnSendLetter.Click += new EventHandler(OnSendLetter);
            this.chkRip.CheckedChanged += new EventHandler(OnCheckRip);
        }

        private void OnExcelExport(object sender, EventArgs e)
        {
            Program.GenerateExcelReport(this.dgvCustomers, "CustomerList", "");
            MessageBox.Show(this, "Customer list exported successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnSendLetter(object sender, EventArgs e)
        {
            List<string> emailList = new List<string>();
            
            foreach(DataGridViewRow row in this.dgvCustomers.Rows)
            {
                if (Convert.ToBoolean(row.Cells[CustomerDataSet.SelectedColumn].Value))
                {
                    if (!string.IsNullOrWhiteSpace(row.Cells[CustomerDataSet.EmailColumn].Value.ToString()))
                    {
                        emailList.Add(row.Cells[CustomerDataSet.EmailColumn].Value.ToString());
                    }
                }
            }

            if (emailList.Count == 0)
            {
                MessageBox.Show(this, "No customer has been selected, please select.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var globalSettings = DataLayer.PopulateGlobalSettings(Program.LoggedInCompanyId);

            if(globalSettings==null || string.IsNullOrWhiteSpace(globalSettings.LetterContent))
            {
                MessageBox.Show(this, "Letter format is not created for selected Company, cannot proceed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (LetterFormatSelectorForm selector = new LetterFormatSelectorForm(emailList))
            {
                selector.ShowDialog();
            }   
        }

        private void InitializeControls()
        {
            this.FormatGrid();
            this.FormatRipGrid();
        }

        private void FormatGrid()
        {
            DataGridViewColumn column = null;
            DataGridViewCheckBoxColumn checkColumn = null;
            this.dgvCustomers.AllowUserToAddRows = false;
            this.dgvCustomers.AllowUserToDeleteRows = false;
            this.dgvCustomers.AllowUserToResizeRows = false;
            this.dgvCustomers.AutoGenerateColumns = false;
            this.dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCustomers.BackgroundColor = Color.White;
            this.dgvCustomers.MultiSelect = true;
            this.dgvCustomers.ReadOnly = false;
            this.dgvCustomers.RowHeadersVisible = false;
            this.dgvCustomers.ScrollBars = ScrollBars.Vertical;
            this.dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.dgvCustomers.Columns.Clear();
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerDataSet.IdColumn;
            column.FillWeight = 10;
            column.HeaderText = "ID";
            column.Name = CustomerDataSet.IdColumn;
            column.Width = 40;
            column.ReadOnly = true;
            column.Visible = false;
            this.dgvCustomers.Columns.Add(column);

            checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.DataPropertyName = CustomerDataSet.SelectedColumn;
            checkColumn.FillWeight = 30;
            checkColumn.HeaderText = "Select";
            checkColumn.Name = CustomerDataSet.SelectedColumn;
            checkColumn.Width = 30;
            checkColumn.ReadOnly = false;
            this.dgvCustomers.Columns.Add(checkColumn);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerDataSet.CodeColumn;
            column.FillWeight = 50;
            column.HeaderText = "Code";
            column.Name = CustomerDataSet.CodeColumn;
            column.Width = 50;
            column.ReadOnly = true;
            this.dgvCustomers.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerDataSet.NameColumn;
            column.FillWeight = 100;
            column.HeaderText = "Name";
            column.Name = CustomerDataSet.NameColumn;
            column.Width = 100;
            column.ReadOnly = true;
            this.dgvCustomers.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerDataSet.AddressColumn;
            column.FillWeight = 150;
            column.HeaderText = "Address";
            column.Name = CustomerDataSet.AddressColumn;
            column.Width = 150;
            column.ReadOnly = true;
            this.dgvCustomers.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerDataSet.PhoneColumn;
            column.FillWeight = 100;
            column.HeaderText = "Phone";
            column.Name = CustomerDataSet.PhoneColumn;
            column.Width = 100;
            column.ReadOnly = true;
            this.dgvCustomers.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerDataSet.EmailColumn;
            column.FillWeight = 100;
            column.HeaderText = "Email";
            column.Name = CustomerDataSet.EmailColumn;
            column.Width = 100;
            column.ReadOnly = true;
            this.dgvCustomers.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerDataSet.SageReferenceColumn;
            column.FillWeight = 80;
            column.HeaderText = "Sage Ref";
            column.Name = CustomerDataSet.SageReferenceColumn;
            column.Width = 80;
            column.ReadOnly = true;
            this.dgvCustomers.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerDataSet.ClientCodeColumn;
            column.FillWeight = 80;
            column.HeaderText = "Client Code(s)";
            column.Name = CustomerDataSet.ClientCodeColumn;
            column.Width = 80;
            column.ReadOnly = true;
            this.dgvCustomers.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerDataSet.HasRipColumn;
            column.FillWeight = 10;
            column.HeaderText = "RIP";
            column.Name = CustomerDataSet.HasRipColumn;
            column.Width = 10;
            column.ReadOnly = true;
            column.Visible = false;
            this.dgvCustomers.Columns.Add(column);

            this._CustomerData = DataLayer.PopulateCustomers(Program.LoggedInCompanyId);

            this.dgvCustomers.DataSource = this._CustomerData.Tables[CustomerDataSet.TableCustomer];
            this.dgvCustomers.Columns[CustomerDataSet.IdColumn].Visible = false;
            this.dgvCustomers.Columns[CustomerDataSet.SageReferenceColumn].Visible = false;
            this.dgvCustomers.Columns[CustomerDataSet.HasRipColumn].Visible = false;
        }

        private void FormatRipGrid()
        {
            DataGridViewColumn column = null;
            DataGridViewCheckBoxColumn checkColumn = null;
            this.dgvRip.AllowUserToAddRows = false;
            this.dgvRip.AllowUserToDeleteRows = false;
            this.dgvRip.AllowUserToResizeRows = false;
            this.dgvRip.AutoGenerateColumns = false;
            this.dgvRip.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRip.BackgroundColor = Color.White;
            this.dgvRip.MultiSelect = true;
            this.dgvRip.ReadOnly = false;
            this.dgvRip.RowHeadersVisible = false;
            this.dgvRip.ScrollBars = ScrollBars.Vertical;
            this.dgvRip.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.dgvRip.Columns.Clear();
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerDataSet.IdColumn;
            column.FillWeight = 10;
            column.HeaderText = "ID";
            column.Name = CustomerDataSet.IdColumn;
            column.Width = 40;
            column.ReadOnly = true;
            column.Visible = false;
            this.dgvRip.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerDataSet.CodeColumn;
            column.FillWeight = 50;
            column.HeaderText = "Code";
            column.Name = CustomerDataSet.CodeColumn;
            column.Width = 50;
            column.ReadOnly = true;
            this.dgvRip.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerDataSet.NameColumn;
            column.FillWeight = 100;
            column.HeaderText = "Name";
            column.Name = CustomerDataSet.NameColumn;
            column.Width = 100;
            column.ReadOnly = true;
            this.dgvRip.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerDataSet.AddressColumn;
            column.FillWeight = 150;
            column.HeaderText = "Address";
            column.Name = CustomerDataSet.AddressColumn;
            column.Width = 150;
            column.ReadOnly = true;
            this.dgvRip.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerDataSet.PhoneColumn;
            column.FillWeight = 100;
            column.HeaderText = "Phone";
            column.Name = CustomerDataSet.PhoneColumn;
            column.Width = 100;
            column.ReadOnly = true;
            this.dgvRip.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerDataSet.EmailColumn;
            column.FillWeight = 100;
            column.HeaderText = "Email";
            column.Name = CustomerDataSet.EmailColumn;
            column.Width = 100;
            column.ReadOnly = true;
            this.dgvRip.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerDataSet.SageReferenceColumn;
            column.FillWeight = 80;
            column.HeaderText = "Sage Ref";
            column.Name = CustomerDataSet.SageReferenceColumn;
            column.Width = 80;
            column.ReadOnly = true;
            this.dgvRip.Columns.Add(column);

            this._DeletedCustomerData = DataLayer.PopulateDeletedCustomers(Program.LoggedInCompanyId);

            this.dgvRip.DataSource = this._DeletedCustomerData.Tables[CustomerDataSet.TableCustomer];
            this.dgvRip.Columns[CustomerDataSet.IdColumn].Visible = false;
            this.dgvRip.Columns[CustomerDataSet.SageReferenceColumn].Visible = false;
        }

        private void OnAddNewCustomer(object sender, EventArgs e)
        {
            DialogResult result;
            using (CustomerForm customer = new CustomerForm())
            {
                result = customer.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.InitializeControls();
                }
            }
        }

        private void OnSelectAll(object sender, EventArgs e)
        {
            if (string.Compare(this.btnSelectAll.Text, "&Select All", StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                foreach (DataRow customerRow in this._CustomerData.Tables[CustomerDataSet.TableCustomer].Rows)
                {
                    customerRow[CustomerDataSet.SelectedColumn] = true;
                }
                this.btnSelectAll.Text = "&Clear All";
            }
            else
            {
                foreach (DataRow customerRow in this._CustomerData.Tables[CustomerDataSet.TableCustomer].Rows)
                {
                    customerRow[CustomerDataSet.SelectedColumn] = false;
                }
                this.btnSelectAll.Text = "&Select All";
            }
            this._CustomerData.AcceptChanges();
        }

        private void OnUpdateActiveFlag(object sender, EventArgs e)
        {
            bool success = false;
            if (this._CustomerData.Tables[CustomerDataSet.TableCustomer].Rows.Count > 0)
            {
                foreach (DataRow customerRow in this._CustomerData.Tables[CustomerDataSet.TableCustomer].Rows)
                {
                    success =
                        DataLayer.MarkCustomerActiveInBreakdown(Convert.ToInt32(customerRow[CustomerDataSet.IdColumn]),
                            Convert.ToBoolean(customerRow[CustomerDataSet.SelectedColumn]));
                    if (!success) break;
                }
            }
            if (success)
            {
                MessageBox.Show(this, "Customer preferences have been saved successfully", Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "Customer preferences could not be saved", Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OnEditCustomer(object sender, EventArgs e)
        {
            DialogResult result;
            int customerId = 0;
            if (this.dgvCustomers.SelectedRows.Count == 0) return;

            customerId = Convert.ToInt32(this.dgvCustomers.SelectedRows[0].Cells[CustomerDataSet.IdColumn].Value);
            Customer selectedCustomer = DataLayer.GetSingleCustomer(customerId);
            using (CustomerForm customer = new CustomerForm(selectedCustomer))
            {
                result = customer.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.InitializeControls();
                }
                
                this.dgvCustomers.ClearSelection();
                DataGridViewRow tmpRow = null;
                foreach (DataGridViewRow rowItem in this.dgvCustomers.Rows)
                {
                    if (Convert.ToInt32(rowItem.Cells[CustomerDataSet.IdColumn].Value) == customerId)
                    {
                        tmpRow = rowItem;
                        break;
                    }
                }
                if (tmpRow != null)
                {
                    this.dgvCustomers.Rows[tmpRow.Index].Selected = true;
                    this.dgvCustomers.CurrentCell = tmpRow.Cells[CustomerDataSet.NameColumn];
                }
            }
        }

        private void OnDeleteCustomer(object sender, EventArgs e)
        {
            int customerId = 0;
            bool success = false;
            if (this.dgvCustomers.SelectedRows.Count == 0) return;
            
            bool verified = false;
            using (VerificationForm verify = new VerificationForm())
            {
                DialogResult result = verify.ShowDialog();
                if (result == DialogResult.OK)
                {
                    verified = verify.IsVerified();
                    if (!verified)
                    {
                        MessageBox.Show(this, "Password verification failed", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        foreach (DataGridViewRow rowItem in this.dgvCustomers.SelectedRows)
                        {
                            customerId = Convert.ToInt32(rowItem.Cells[CustomerDataSet.IdColumn].Value);
                            success = DataLayer.DeleteCustomer(customerId);
                            if (!success)
                            {
                                MessageBox.Show(this, "One or more customer(s) could not be deleted please check log file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        MessageBox.Show(this, "Selected customer(s) deleted successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.FormatGrid();
                        return;
                    }
                }
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OnUseCustomer(object sender, EventArgs e)
        {
            if (this.dgvCustomers.SelectedRows.Count == 0) return;
            this._SelectedCustomerId = Convert.ToInt32(this.dgvCustomers.SelectedRows[0].Cells[CustomerDataSet.IdColumn].Value);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion

        #region Public property

        public void SetBreakdownFlag()
        {
            this._OpenedFromBreakdown = true;
        }

        public int GetSelectedCustomerId()
        {
            return this._SelectedCustomerId;
        }

        #endregion

        #region Grid events

        private void OnRipFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex == this.dgvCustomers.NewRowIndex) return;

            if (this.dgvCustomers.Columns[e.ColumnIndex].HeaderText == CustomerDataSet.NameColumn)
            {
                if (Convert.ToBoolean(this.dgvCustomers.Rows[e.RowIndex].Cells[CustomerDataSet.HasRipColumn].Value.ToString()))
                {
                    for (int loop = 0; loop < this.dgvCustomers.Columns.Count; loop++)
                    {
                        this.dgvCustomers.Rows[e.RowIndex].Cells[loop].Style.BackColor = Color.Yellow;
                    }
                }
            }
        }

        private void OnCheckRip(object sender, EventArgs e)
        {
            if(this.chkRip.Text == "Hide RIP")
            {
                this._CustomerData = DataLayer.PopulateCustomers(Program.LoggedInCompanyId, true);
                this.chkRip.Text = "Show RIP";
            }
            else
            {
                this._CustomerData = DataLayer.PopulateCustomers(Program.LoggedInCompanyId);
                this.chkRip.Text = "Hide RIP";
            }
            this.dgvCustomers.DataSource = this._CustomerData.Tables[CustomerDataSet.TableCustomer];
            this.dgvCustomers.Columns[CustomerDataSet.IdColumn].Visible = false;
            this.dgvCustomers.Columns[CustomerDataSet.SageReferenceColumn].Visible = false;
            this.dgvCustomers.Columns[CustomerDataSet.HasRipColumn].Visible = false;
        }

        private void OnGridHeaderClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch (this.dgvCustomers.Columns[e.ColumnIndex].HeaderText)
            {
                case "Code":
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Code");
                        break;
                    }
                case "Name":
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Name");
                        break;
                    }
                case "Address":
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Address");
                        break;
                    }
                case "Email":
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Email");
                        break;
                    }
                case "Phone":
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Phone");
                        break;
                    }
                case "Sage Ref":
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Sage Ref");
                        break;
                    }
                case "Client Code(s)":
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Client Code(s)");
                        break;
                    }
            }
            this.txtSearch.Focus();
        }

        private void OnSearchChanged(object sender, EventArgs e)
        {
            DataRow[] filteredRows=null;
            CustomerDataSet tmpData = new CustomerDataSet();
            string filterCondition = string.Empty;
            if (string.Compare(this.lblSearch.Text, "Search on", false) == 0) return;

            if (this.tabMain.SelectedIndex == 0)
            {
                switch (this.lblSearch.Text.Substring(9).Trim())
                {
                    case "Code":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", CustomerDataSet.CodeColumn, this.txtSearch.Text);
                            filteredRows = this._CustomerData.Tables[CustomerDataSet.TableCustomer].Select(filterCondition);
                            foreach (DataRow rowItem in filteredRows)
                            {
                                tmpData.Tables[CustomerDataSet.TableCustomer].ImportRow(rowItem);
                            }
                            this.dgvCustomers.DataSource = tmpData.Tables[CustomerDataSet.TableCustomer];
                            break;
                        }
                    case "Name":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", CustomerDataSet.NameColumn, this.txtSearch.Text);
                            filteredRows = this._CustomerData.Tables[CustomerDataSet.TableCustomer].Select(filterCondition);
                            foreach (DataRow rowItem in filteredRows)
                            {
                                tmpData.Tables[CustomerDataSet.TableCustomer].ImportRow(rowItem);
                            }
                            this.dgvCustomers.DataSource = tmpData.Tables[CustomerDataSet.TableCustomer];
                            break;
                        }
                    case "Address":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", CustomerDataSet.AddressColumn, this.txtSearch.Text);
                            filteredRows = this._CustomerData.Tables[CustomerDataSet.TableCustomer].Select(filterCondition);
                            foreach (DataRow rowItem in filteredRows)
                            {
                                tmpData.Tables[CustomerDataSet.TableCustomer].ImportRow(rowItem);
                            }
                            this.dgvCustomers.DataSource = tmpData.Tables[CustomerDataSet.TableCustomer];
                            break;
                        }
                    case "Email":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", CustomerDataSet.EmailColumn, this.txtSearch.Text);
                            filteredRows = this._CustomerData.Tables[CustomerDataSet.TableCustomer].Select(filterCondition);
                            foreach (DataRow rowItem in filteredRows)
                            {
                                tmpData.Tables[CustomerDataSet.TableCustomer].ImportRow(rowItem);
                            }
                            this.dgvCustomers.DataSource = tmpData.Tables[CustomerDataSet.TableCustomer];
                            break;
                        }
                    case "Phone":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", CustomerDataSet.PhoneColumn, this.txtSearch.Text);
                            filteredRows = this._CustomerData.Tables[CustomerDataSet.TableCustomer].Select(filterCondition);
                            foreach (DataRow rowItem in filteredRows)
                            {
                                tmpData.Tables[CustomerDataSet.TableCustomer].ImportRow(rowItem);
                            }
                            this.dgvCustomers.DataSource = tmpData.Tables[CustomerDataSet.TableCustomer];
                            break;
                        }
                    case "Sage Ref":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", CustomerDataSet.SageReferenceColumn, this.txtSearch.Text);
                            filteredRows = this._CustomerData.Tables[CustomerDataSet.TableCustomer].Select(filterCondition);
                            foreach (DataRow rowItem in filteredRows)
                            {
                                tmpData.Tables[CustomerDataSet.TableCustomer].ImportRow(rowItem);
                            }
                            this.dgvCustomers.DataSource = tmpData.Tables[CustomerDataSet.TableCustomer];
                            break;
                        }
                    case "Client Code(s)":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", CustomerDataSet.ClientCodeColumn, this.txtSearch.Text);
                            filteredRows = this._CustomerData.Tables[CustomerDataSet.TableCustomer].Select(filterCondition);
                            foreach (DataRow rowItem in filteredRows)
                            {
                                tmpData.Tables[CustomerDataSet.TableCustomer].ImportRow(rowItem);
                            }
                            this.dgvCustomers.DataSource = tmpData.Tables[CustomerDataSet.TableCustomer];
                            break;
                        }
                }
            }
            else
            {
                switch (this.lblSearch.Text.Substring(9).Trim())
                {
                    case "Code":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", CustomerDataSet.CodeColumn, this.txtSearch.Text);
                            filteredRows = this._DeletedCustomerData.Tables[CustomerDataSet.TableCustomer].Select(filterCondition);
                            foreach (DataRow rowItem in filteredRows)
                            {
                                tmpData.Tables[CustomerDataSet.TableCustomer].ImportRow(rowItem);
                            }
                            this.dgvRip.DataSource = tmpData.Tables[CustomerDataSet.TableCustomer];
                            break;
                        }
                    case "Name":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", CustomerDataSet.NameColumn, this.txtSearch.Text);
                            filteredRows = this._DeletedCustomerData.Tables[CustomerDataSet.TableCustomer].Select(filterCondition);
                            foreach (DataRow rowItem in filteredRows)
                            {
                                tmpData.Tables[CustomerDataSet.TableCustomer].ImportRow(rowItem);
                            }
                            this.dgvRip.DataSource = tmpData.Tables[CustomerDataSet.TableCustomer];
                            break;
                        }
                    case "Address":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", CustomerDataSet.AddressColumn, this.txtSearch.Text);
                            filteredRows = this._DeletedCustomerData.Tables[CustomerDataSet.TableCustomer].Select(filterCondition);
                            foreach (DataRow rowItem in filteredRows)
                            {
                                tmpData.Tables[CustomerDataSet.TableCustomer].ImportRow(rowItem);
                            }
                            this.dgvRip.DataSource = tmpData.Tables[CustomerDataSet.TableCustomer];
                            break;
                        }
                    case "Email":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", CustomerDataSet.EmailColumn, this.txtSearch.Text);
                            filteredRows = this._DeletedCustomerData.Tables[CustomerDataSet.TableCustomer].Select(filterCondition);
                            foreach (DataRow rowItem in filteredRows)
                            {
                                tmpData.Tables[CustomerDataSet.TableCustomer].ImportRow(rowItem);
                            }
                            this.dgvRip.DataSource = tmpData.Tables[CustomerDataSet.TableCustomer];
                            break;
                        }
                    case "Phone":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", CustomerDataSet.PhoneColumn, this.txtSearch.Text);
                            filteredRows = this._DeletedCustomerData.Tables[CustomerDataSet.TableCustomer].Select(filterCondition);
                            foreach (DataRow rowItem in filteredRows)
                            {
                                tmpData.Tables[CustomerDataSet.TableCustomer].ImportRow(rowItem);
                            }
                            this.dgvRip.DataSource = tmpData.Tables[CustomerDataSet.TableCustomer];
                            break;
                        }
                    case "Sage Ref":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", CustomerDataSet.SageReferenceColumn, this.txtSearch.Text);
                            filteredRows = this._DeletedCustomerData.Tables[CustomerDataSet.TableCustomer].Select(filterCondition);
                            foreach (DataRow rowItem in filteredRows)
                            {
                                tmpData.Tables[CustomerDataSet.TableCustomer].ImportRow(rowItem);
                            }
                            this.dgvRip.DataSource = tmpData.Tables[CustomerDataSet.TableCustomer];
                            break;
                        }
                }
            }
        }

        private void OnEnterKeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && this.dgvCustomers.SelectedRows.Count>0)
            {
                this.btnEdit.PerformClick();
            }
        }

        private void OnGridDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.btnEdit.PerformClick();
        }

        #endregion
    }
}
