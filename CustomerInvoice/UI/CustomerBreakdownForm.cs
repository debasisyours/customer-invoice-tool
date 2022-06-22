using CustomerInvoice.Data;
using CustomerInvoice.Data.DataSets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomerInvoice.UI
{
    public partial class CustomerBreakdownForm : Form
    {
        #region Declaration

        private CustomerBreakDownDataSet _breakdownData = null;
        private bool _loading = true;

        #endregion

        #region Constructor
        public CustomerBreakdownForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.AssignEventHandlers();
            this.PopulateCustomers();
            this.FormatGrid();
            this._loading = false;
        }

        #endregion

        #region Custom functions

        private void OnGridHeaderClicked(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch (this.dgvBreakdown.Columns[e.ColumnIndex].DataPropertyName)
            {
                case CustomerBreakDownDataSet.ChargeHeadNameColumn:
                    {
                        this.lblFilter.Text = "Search by Charge head";
                        this.txtFilter.Focus();
                        break;
                    }
                case CustomerBreakDownDataSet.CustomerNameColumn:
                    {
                        this.lblFilter.Text = "Search by Customer";
                        this.txtFilter.Focus();
                        break;
                    }
                case CustomerBreakDownDataSet.ClientNameColumn:
                    {
                        this.lblFilter.Text = "Search by Client";
                        this.txtFilter.Focus();
                        break;
                    }
            }
        }

        private void OnFilterChanged(object sender, EventArgs e)
        {
            string filterString = string.Empty;

            string criteria = this.lblFilter.Text.Substring(9).Trim();
            switch (criteria)
            {
                case "Charge head":
                    {
                        filterString = $"{CustomerBreakDownDataSet.ChargeHeadNameColumn} LIKE '%{this.txtFilter.Text}%'";
                        break;
                    }
                case "Customer":
                    {
                        filterString = $"{CustomerBreakDownDataSet.CustomerNameColumn} LIKE '%{this.txtFilter.Text}%'";
                        break;
                    }
                case "Client":
                    {
                        filterString = $"{CustomerBreakDownDataSet.ClientNameColumn} LIKE '%{this.txtFilter.Text}%'";
                        break;
                    }
            }

            if (!string.IsNullOrWhiteSpace(filterString))
            {
                CustomerBreakDownDataSet filteredDataSet = new CustomerBreakDownDataSet();
                DataRow[] selectedRows = this._breakdownData.Tables[CustomerBreakDownDataSet.TableCustomerBreakdown].Select(filterString);
                foreach (DataRow row in selectedRows)
                {
                    filteredDataSet.Tables[CustomerBreakDownDataSet.TableCustomerBreakdown].ImportRow(row);
                }

                this.dgvBreakdown.DataSource = filteredDataSet.Tables[CustomerBreakDownDataSet.TableCustomerBreakdown];
                this.dgvBreakdown.Columns[CustomerBreakDownDataSet.ClientIdColumn].Visible = false;
                this.dgvBreakdown.Columns[CustomerBreakDownDataSet.ClientIdColumn].Width = 0;
                this.dgvBreakdown.Columns[CustomerBreakDownDataSet.CustomerIdColumn].Visible = false;
                this.dgvBreakdown.Columns[CustomerBreakDownDataSet.CustomerIdColumn].Width = 0;
                this.dgvBreakdown.Columns[CustomerBreakDownDataSet.ChargeHeadIdColumn].Visible = false;
                this.dgvBreakdown.Columns[CustomerBreakDownDataSet.ChargeHeadIdColumn].Width = 0;
                this.UpdateSum();
            }
        }

        private void PopulateCustomers()
        {
            var customers = DataLayer.PopulateCustomers(Program.LoggedInCompanyId, true);
            this.customerDropDown.Items.Clear();
            this.customerDropDown.DataSource = customers.Tables[CustomerDataSet.TableCustomer];
            this.customerDropDown.DisplayMember = CustomerDataSet.CodeColumn;
            this.customerDropDown.ValueMember = CustomerDataSet.IdColumn;

            this.customerDropDown.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.customerDropDown.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection customerList = new AutoCompleteStringCollection();
            foreach(DataRow row in customers.Tables[CustomerDataSet.TableCustomer].Rows)
            {
                customerList.Add(row[CustomerDataSet.CodeColumn].ToString());
            }
            this.customerDropDown.AutoCompleteCustomSource = customerList;
            this.customerDropDown.SelectedIndex = -1;
        }

        private void OnChangeCustomer(object sender, EventArgs e)
        {
            if (this._loading) return;

            if (this.customerDropDown.SelectedItem != null)
            {
                int customerId = Convert.ToInt32(this.customerDropDown.SelectedValue);
                this._breakdownData = DataLayer.PopulateCustomerBreakdowns(Program.LoggedInCompanyId, customerId);
                this.dgvBreakdown.DataSource = this._breakdownData.Tables[CustomerBreakDownDataSet.TableCustomerBreakdown];
                this.dgvBreakdown.Columns[CustomerBreakDownDataSet.ClientIdColumn].Visible = false;
                this.dgvBreakdown.Columns[CustomerBreakDownDataSet.ClientIdColumn].Width = 0;
                this.dgvBreakdown.Columns[CustomerBreakDownDataSet.CustomerIdColumn].Visible = false;
                this.dgvBreakdown.Columns[CustomerBreakDownDataSet.CustomerIdColumn].Width = 0;
                this.dgvBreakdown.Columns[CustomerBreakDownDataSet.ChargeHeadIdColumn].Visible = false;
                this.dgvBreakdown.Columns[CustomerBreakDownDataSet.ChargeHeadIdColumn].Width = 0;
                this.UpdateSum();
            }
        }

        private void UpdateSum()
        {
            decimal sum = 0;

            foreach(DataGridViewRow row in this.dgvBreakdown.Rows)
            {
                sum += Convert.ToDecimal(row.Cells[CustomerBreakDownDataSet.RateColumn].Value);
            }

            this.txtSumTotal.Text = sum.ToString("N2");
        }

        private void AssignEventHandlers()
        {
            this.customerDropDown.SelectedIndexChanged += new EventHandler(OnChangeCustomer);
            this.OkButton.Click += new EventHandler(OnOkClick);
            this.CancelButton.Click += new EventHandler(OnCancelClick);
            this.dgvBreakdown.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(OnGridHeaderClicked);
            this.txtFilter.TextChanged += new EventHandler(OnFilterChanged);
        }

        private void OnOkClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormatGrid()
        {
            DataGridViewTextBoxColumn column = null;

            this.dgvBreakdown.AllowUserToAddRows = false;
            this.dgvBreakdown.AllowUserToDeleteRows = false;
            this.dgvBreakdown.AllowUserToResizeColumns = false;
            this.dgvBreakdown.AllowUserToResizeRows = false;
            this.dgvBreakdown.AutoGenerateColumns= false;
            this.dgvBreakdown.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBreakdown.BackgroundColor = Color.White;
            this.dgvBreakdown.MultiSelect = false;
            this.dgvBreakdown.ReadOnly = false;
            this.dgvBreakdown.RowHeadersVisible = false;
            this.dgvBreakdown.AlternatingRowsDefaultCellStyle.BackColor = Color.BlanchedAlmond;
            this.dgvBreakdown.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvBreakdown.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBreakdown.Columns.Clear();

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerBreakDownDataSet.ClientIdColumn;
            column.HeaderText = CustomerBreakDownDataSet.ClientIdColumn;
            column.Name = CustomerBreakDownDataSet.ClientIdColumn;
            column.FillWeight = 10;
            column.Visible = false;
            column.ReadOnly = true;
            column.Width = 0;
            this.dgvBreakdown.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerBreakDownDataSet.ClientNameColumn;
            column.HeaderText = "Client Name";
            column.Name = CustomerBreakDownDataSet.ClientNameColumn;
            column.FillWeight = 40;
            column.Visible = true;
            column.ReadOnly = true;
            column.Width = 80;
            this.dgvBreakdown.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerBreakDownDataSet.CustomerIdColumn;
            column.HeaderText = CustomerBreakDownDataSet.CustomerIdColumn;
            column.Name = CustomerBreakDownDataSet.CustomerIdColumn;
            column.FillWeight = 10;
            column.Visible = false;
            column.ReadOnly = true;
            column.Width = 0;
            this.dgvBreakdown.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerBreakDownDataSet.CustomerNameColumn;
            column.HeaderText = "Customer Name";
            column.Name = CustomerBreakDownDataSet.CustomerNameColumn;
            column.FillWeight = 40;
            column.Visible = true;
            column.ReadOnly = true;
            column.Width = 80;
            this.dgvBreakdown.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerBreakDownDataSet.ChargeHeadIdColumn;
            column.HeaderText = CustomerBreakDownDataSet.ChargeHeadIdColumn;
            column.Name = CustomerBreakDownDataSet.ChargeHeadIdColumn;
            column.FillWeight = 10;
            column.Visible = false;
            column.ReadOnly = true;
            column.Width = 0;
            this.dgvBreakdown.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerBreakDownDataSet.ChargeHeadNameColumn;
            column.HeaderText = "Charge Head Name";
            column.Name = CustomerBreakDownDataSet.ChargeHeadNameColumn;
            column.FillWeight = 40;
            column.Visible = true;
            column.ReadOnly = true;
            column.Width = 80;
            this.dgvBreakdown.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerBreakDownDataSet.RateColumn;
            column.HeaderText = CustomerBreakDownDataSet.RateColumn;
            column.Name = CustomerBreakDownDataSet.RateColumn;
            column.FillWeight = 30;
            column.DefaultCellStyle.Format = "#0.00";
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.Visible = true;
            column.ReadOnly = true;
            column.Width = 70;
            this.dgvBreakdown.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CustomerBreakDownDataSet.InvoiceCycleColumn;
            column.HeaderText = "Invoice Cycle (Months)";
            column.Name = CustomerBreakDownDataSet.InvoiceCycleColumn;
            column.FillWeight = 30;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.Visible = true;
            column.ReadOnly = true;
            column.Width = 60;
            this.dgvBreakdown.Columns.Add(column);

            this._breakdownData = new CustomerBreakDownDataSet();
        }

        #endregion
    }
}
