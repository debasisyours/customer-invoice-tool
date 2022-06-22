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

namespace CustomerInvoice.UI
{
    public partial class BreakdownEditForm : Form
    {
        #region Private field declaration

        private BreakDownDataSet _Breakdown = null;
        private int _SelectedId = 0;

        #endregion

        public BreakdownEditForm()
        {
            InitializeComponent();
            this.FormatGrid();
            this.AssignEventHandlers();
            this.PopulateDropdowns();
            this.btnCancel.PerformClick();
        }

        #region Form events

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            int clientId = DataLayer.CheckBreakDown(Program.LoggedInCompanyId);

            if (clientId > 0)
            {
                Client tmpClient = DataLayer.GetSingleClient(clientId);
                MessageBox.Show(this,string.Format(CultureInfo.CurrentCulture,"Breakdown for " + tmpClient.Name + " is not proper (Total amount should be {0}), please check",tmpClient.TotalRate.ToString("N2")), Application.ProductName, MessageBoxButtons.OK,   MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        #endregion

        #region Custom functions

        private void AssignEventHandlers()
        {
            this.btnClose.Click += new EventHandler(OnClose);
            this.btnSave.Click += new EventHandler(OnSave);
            this.btnCancel.Click += new EventHandler(OnCancel);
            this.btnDelete.Click += new EventHandler(OnDelete);
            this.dgvEntry.CellClick += new DataGridViewCellEventHandler(OnBreakdownSelect);
            this.btnExport.Click += new EventHandler(OnExport);
            this.btnExcelExport.Click += new EventHandler(OnExportToExcel);
        }

        private void OnExportToExcel(object sender, EventArgs e)
        {
            Program.GenerateExcelReport(this.dgvEntry, "Breakdown", "Amount");
            MessageBox.Show(this, "Breakdown report generated successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnClose(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void UpdateTotal()
        {
            decimal total = 0;
            foreach (DataRow row in this._Breakdown.Tables[BreakDownDataSet.TableBreakDown].Rows)
            {
                total += Convert.ToDecimal(row[BreakDownDataSet.AmountColumn]);
            }
            this.txtTotal.Text = total.ToString("N2");
        }

        private void OnSave(object sender, EventArgs e)
        {
            if (!IsDataValid()) return;

            if (DataLayer.SaveBreakdownEntry(Convert.ToInt32(this.cboClient.SelectedValue), Convert.ToInt32(this.cboCustomer.SelectedValue), 
                                             Convert.ToInt32(this.cboCharge.SelectedValue), Convert.ToDecimal(this.txtAmount.Text), 
                                             Program.LoggedInCompanyId, Convert.ToInt16(txtInvoiceCycle.Text), this.chkActive.Checked))
            {
                MessageBox.Show(this, "Breakdown saved successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.FormatGrid();
                this.btnCancel.PerformClick();
            }
            else
            {
                MessageBox.Show(this, "Breakdown could not be saved, please check log file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool IsDataValid()
        {
            bool validated = true;

            if (this.cboClient.SelectedIndex == -1)
            {
                MessageBox.Show(this,"Client is not selected",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Warning);
                this.cboClient.Focus();
                return false;
            }

            if (this.cboCustomer.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Customer is not selected", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.cboCustomer.Focus();
                return false;
            }

            if (this.cboCharge.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Charge is not selected", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.cboCharge.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(this.txtAmount.Text))
            {
                MessageBox.Show(this, "Charge Amount can not be blank", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtAmount.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(this.txtInvoiceCycle.Text))
            {
                MessageBox.Show(this, "Invoice cycle can not be blank", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtInvoiceCycle.Focus();
                return false;
            }

            decimal amount;
            bool parsed = Decimal.TryParse(this.txtAmount.Text, out amount);

            if (!parsed)
            {
                MessageBox.Show(this, "Charge Amount is not valid", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtAmount.Focus();
                return false;
            }

            short cycle;
            parsed = short.TryParse(this.txtInvoiceCycle.Text, out cycle);
            if (!parsed)
            {
                MessageBox.Show(this, "Invoice cycle should be numeric", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtInvoiceCycle.Focus();
                return false;
            }

            if(cycle>12)
            {
                MessageBox.Show(this, "Invoice cycle can be set to 12 at the maximum", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtInvoiceCycle.Focus();
                return false;
            }

            return validated;
        }

        private void OnDelete(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure to delete breakdown for selected client?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (DataLayer.DeleteBreakdownEntry(Convert.ToInt32(this.cboClient.SelectedValue), Program.LoggedInCompanyId))
                {
                    MessageBox.Show(this, "Breakdown deleted successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.FormatGrid();
                    this.btnCancel.PerformClick();
                }
                else
                {
                    MessageBox.Show(this, "Breakdown could not be deleted, please check log file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void OnCancel(object sender, EventArgs e)
        {
            this.cboClient.SelectedIndex = -1;
            this.cboCustomer.SelectedIndex = -1;
            this.cboCharge.SelectedIndex = -1;
            this.txtAmount.Clear();
            this._SelectedId = 0;
        }

        private void PopulateDropdowns()
        {
            this.cboClient.DataSource = DataLayer.PopulateClients(Program.LoggedInCompanyId, true).Tables[ClientDataSet.TableClient];
            this.cboClient.DisplayMember = ClientDataSet.NameColumn;
            this.cboClient.ValueMember = ClientDataSet.IdColumn;

            this.cboCustomer.DataSource = DataLayer.PopulateCustomers(Program.LoggedInCompanyId).Tables[CustomerDataSet.TableCustomer];
            this.cboCustomer.DisplayMember = CustomerDataSet.NameColumn;
            this.cboCustomer.ValueMember = CustomerDataSet.IdColumn;

            this.cboCharge.DataSource = DataLayer.PopulateCharges(Program.LoggedInCompanyId).Tables[ChargeDataSet.TableChargeHead];
            this.cboCharge.DisplayMember = ChargeDataSet.NameColumn;
            this.cboCharge.ValueMember = ChargeDataSet.IdColumn;
        }

        private void OnExport(object sender, EventArgs e)
        {
            using (BreakDownPrintForm print = new BreakDownPrintForm(this._Breakdown))
            {
                print.ShowDialog();
            }
        }

        private void FormatGrid()
        {
            DataGridViewColumn column = null;
            DataGridViewCheckBoxColumn checkColumn = null;

            this.dgvEntry.AllowUserToAddRows = false;
            this.dgvEntry.AllowUserToDeleteRows = false;
            this.dgvEntry.AllowUserToResizeRows = false;
            this.dgvEntry.AutoGenerateColumns = false;
            this.dgvEntry.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEntry.BackgroundColor = Color.White;
            this.dgvEntry.MultiSelect = false;
            this.dgvEntry.ReadOnly = true;
            this.dgvEntry.RowHeadersVisible = false;
            this.dgvEntry.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvEntry.ScrollBars = ScrollBars.Vertical;

            this.dgvEntry.Columns.Clear();

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = BreakDownDataSet.IdColumn;
            column.FillWeight = 20;
            column.HeaderText = "ID";
            column.Name = BreakDownDataSet.IdColumn;
            column.Width = 20;
            column.Visible = false;
            this.dgvEntry.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = BreakDownDataSet.ClientIdColumn;
            column.FillWeight = 20;
            column.HeaderText = "Client ID";
            column.Name = BreakDownDataSet.ClientIdColumn;
            column.Width = 20;
            column.Visible = false;
            this.dgvEntry.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = BreakDownDataSet.ClientCodeColumn;
            column.FillWeight = 80;
            column.HeaderText = "Client Code";
            column.Name = BreakDownDataSet.ClientCodeColumn;
            column.Width = 80;
            this.dgvEntry.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = BreakDownDataSet.ClientNameColumn;
            column.FillWeight = 120;
            column.HeaderText = "Client Name";
            column.Name = BreakDownDataSet.ClientNameColumn;
            column.Width = 120;
            this.dgvEntry.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = BreakDownDataSet.CustomerIdColumn;
            column.FillWeight = 20;
            column.HeaderText = "Customer ID";
            column.Name = BreakDownDataSet.CustomerIdColumn;
            column.Width = 20;
            column.Visible = false;
            this.dgvEntry.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = BreakDownDataSet.CustomerCodeColumn;
            column.FillWeight = 80;
            column.HeaderText = "Customer Code";
            column.Name = BreakDownDataSet.CustomerCodeColumn;
            column.Width = 80;
            this.dgvEntry.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = BreakDownDataSet.CustomerNameColumn;
            column.FillWeight = 120;
            column.HeaderText = "Customer Name";
            column.Name = BreakDownDataSet.CustomerNameColumn;
            column.Width = 120;
            this.dgvEntry.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = BreakDownDataSet.ChargeHeadIdColumn;
            column.FillWeight = 20;
            column.HeaderText = "Charge ID";
            column.Name = BreakDownDataSet.ChargeHeadIdColumn;
            column.Width = 20;
            column.Visible = false;
            this.dgvEntry.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = BreakDownDataSet.ChargeHeadNameColumn;
            column.FillWeight = 90;
            column.HeaderText = "Charge Name";
            column.Name = BreakDownDataSet.ChargeHeadNameColumn;
            column.Width = 90;
            this.dgvEntry.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = BreakDownDataSet.AmountColumn;
            column.FillWeight = 100;
            column.HeaderText = "Amount";
            column.Name = BreakDownDataSet.AmountColumn;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.DefaultCellStyle.Format = "#0.00";
            column.Width = 100;
            this.dgvEntry.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = BreakDownDataSet.InvoiceCycleColumn;
            column.FillWeight = 70;
            column.HeaderText = "Invoice Cycle (Month)";
            column.Name = BreakDownDataSet.InvoiceCycleColumn;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.DefaultCellStyle.Format = "#0";
            column.Width = 70;
            this.dgvEntry.Columns.Add(column);

            checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.DataPropertyName = BreakDownDataSet.IsActiveColumn;
            checkColumn.Name = BreakDownDataSet.IsActiveColumn;
            checkColumn.HeaderText = "Is Active";
            checkColumn.FillWeight = 40;
            checkColumn.Width = 40;
            checkColumn.ReadOnly = true;
            checkColumn.Visible = true;
            checkColumn.FlatStyle = FlatStyle.Flat;
            checkColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dgvEntry.Columns.Add(checkColumn);

            this._Breakdown = DataLayer.PopulateBreakDowns(Program.LoggedInCompanyId);
            this.dgvEntry.DataSource = this._Breakdown.Tables[BreakDownDataSet.TableBreakDown];
            this.dgvEntry.Columns[BreakDownDataSet.IdColumn].Visible = false;
            this.dgvEntry.Columns[BreakDownDataSet.ClientIdColumn].Visible = false;
            this.dgvEntry.Columns[BreakDownDataSet.CustomerIdColumn].Visible = false;
            this.dgvEntry.Columns[BreakDownDataSet.ChargeHeadIdColumn].Visible = false;

            ClientDataSet clients = DataLayer.PopulateClients(Program.LoggedInCompanyId, false);
            this.lblTotalRecords.Text = string.Format(CultureInfo.CurrentCulture, "Total Clients: {0}", clients.Tables[ClientDataSet.TableClient].Rows.Count);

            this.UpdateTotal();
        }

        #endregion

        #region Gridview events

        private void OnBreakdownSelect(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selected = null;
            if (this.dgvEntry.SelectedRows.Count == 0) return;

            selected = this.dgvEntry.SelectedRows[0];
            int clientId = Convert.ToInt32(selected.Cells[BreakDownDataSet.ClientIdColumn].Value);
            int customerId = Convert.ToInt32(selected.Cells[BreakDownDataSet.CustomerIdColumn].Value);
            int chargeId = Convert.ToInt32(selected.Cells[BreakDownDataSet.ChargeHeadIdColumn].Value);
            decimal amount = Convert.ToDecimal(selected.Cells[BreakDownDataSet.AmountColumn].Value);
            short invoiceCycle = Convert.ToInt16(selected.Cells[BreakDownDataSet.InvoiceCycleColumn].Value);
            bool isActive = Convert.ToBoolean(selected.Cells[BreakDownDataSet.IsActiveColumn].Value);

            this.txtAmount.Text = amount.ToString("N2");
            this.cboClient.SelectedValue = clientId;
            this.cboCustomer.SelectedValue = customerId;
            this.cboCharge.SelectedValue = chargeId;
            this.txtInvoiceCycle.Text = invoiceCycle.ToString("N0");
            this.chkActive.Checked = isActive;
            this._SelectedId = Convert.ToInt32(selected.Cells[BreakDownDataSet.IdColumn].Value);
        }

        #endregion
    }
}
