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
    public partial class BreakdownForm : Form
    {
        #region Field declaration

        private Client _Client;
        private BreakdownDetailDataSet _BreakdownDetail;

        #endregion

        public BreakdownForm()
        {
            InitializeComponent();
            this._BreakdownDetail = new BreakdownDetailDataSet();
        }

        public BreakdownForm(Client client)
        {
            InitializeComponent();
            this._Client = client;
            this._BreakdownDetail = DataLayer.GetBreakdownForClient(client.ID);
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.PopulateFormControls();
            this.AssignEventHandlers();
            this.FormatGrid();
            this.dgvCustomers.DataSource = this._BreakdownDetail.Tables[BreakdownDetailDataSet.TableBreakdownDetail];
        }

        //protected override void OnFormClosing(FormClosingEventArgs e)
        //{
        //    base.OnFormClosing(e);
        //    decimal checkTotal = this.CheckTotalBreakdown();
        //    if (checkTotal > 0)
        //    {
        //        MessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, "Breakdown entered does not match with Client's total rate {0}, please check",checkTotal), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        e.Cancel = true;
        //    }
        //}

        #endregion

        #region Custom methods

        protected void PopulateFormControls()
        {
            if (this._Client != null)
            {
                this.txtCode.Text = this._Client.Code;
                this.txtName.Text = this._Client.Name;

            }
        }

        private void AssignEventHandlers()
        {
            this.btnOK.Click += new EventHandler(OnSave);
            this.btnCancel.Click += new EventHandler(OnExit);
            this.btnSelect.Click += new EventHandler(OnCustomerSelect);
            this.dgvCustomers.CellClick += new DataGridViewCellEventHandler(OnDeleteButtonClick);
            this.btnExportExcel.Click += new EventHandler(OnGenerateExcelExport);
            //this.dgvCustomers.CellValueChanged += new DataGridViewCellEventHandler(OnChargeSelected);
        }

        private void OnGenerateExcelExport(object sender, EventArgs e)
        {
            Program.GenerateExcelReport(this.dgvCustomers, "ClientWiseBreakdown", "Rate");
            MessageBox.Show(this, "Client wise breakdown report generated successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected void OnCustomerSelect(object sender, EventArgs e)
        {
            DialogResult result;
            int selectedCustomer = 0;
            int selectedCharge = 0;
            Customer tmpCustomer = null;
            ChargeHead tmpCharge = null;
            DataRow newRow = null;

            ChargeDataSet charges = DataLayer.PopulateCharges(Program.LoggedInCompanyId);
            if (charges.Tables[ChargeDataSet.TableChargeHead].Rows.Count == 0)
            {
                MessageBox.Show(this, "No charges have been created, please create at least one charge.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (CustomerSearchForm customerSelect = new CustomerSearchForm())
            {
                customerSelect.SetBreakdownFlag();
                result = customerSelect.ShowDialog();
                if (result == DialogResult.OK)
                {
                    selectedCustomer = customerSelect.GetSelectedCustomerId();
                    /*existingRow = this._BreakdownDetail.Tables[BreakdownDetailDataSet.TableBreakdownDetail].Select(string.Format(CultureInfo.CurrentCulture, "{0} = {1}", BreakdownDetailDataSet.CustomerIdColumn, selectedCustomer));
                    if (existingRow.Length > 0)
                    { 
                        MessageBox.Show(this,"Customer already added for this client",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
                        return;
                    }*/
                    if (selectedCustomer > 0)
                    {
                        tmpCustomer = DataLayer.GetSingleCustomer(selectedCustomer);
                        selectedCharge = Convert.ToInt32(DataLayer.PopulateCharges(Program.LoggedInCompanyId).Tables[ChargeDataSet.TableChargeHead].Rows[0][ChargeDataSet.IdColumn]);
                        tmpCharge = DataLayer.GetSingleCharge(selectedCharge);
                        newRow = this._BreakdownDetail.Tables[BreakdownDetailDataSet.TableBreakdownDetail].NewRow();
                        newRow[BreakdownDetailDataSet.CustomerIdColumn] = selectedCustomer;
                        newRow[BreakdownDetailDataSet.ChargeHeadIdColumn] = tmpCharge.ID;
                        newRow[BreakdownDetailDataSet.ChargeHeadNameColumn] = tmpCharge.Name;
                        newRow[BreakdownDetailDataSet.CustomerCodeColumn] = tmpCustomer.Code;
                        newRow[BreakdownDetailDataSet.CustomerNameColumn] = tmpCustomer.Name;
                        newRow[BreakdownDetailDataSet.RateColumn] = 0;
                        this._BreakdownDetail.Tables[BreakdownDetailDataSet.TableBreakdownDetail].Rows.Add(newRow);
                    }
                }
            }
        }

        protected void OnExit(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        //private decimal CheckTotalBreakdown()
        //{
        //    decimal totalAmount = this._Client.TotalRate;
        //    decimal entered = 0;

        //    foreach (DataGridViewRow rowItem in this.dgvCustomers.Rows)
        //    {
        //        entered += Convert.ToDecimal(rowItem.Cells[BreakdownDetailDataSet.RateColumn].Value);
        //    }
        //    return totalAmount != entered ? totalAmount : 0;
        //}

        protected void OnSave(object sender, EventArgs e)
        {
            if (this._BreakdownDetail.Tables[BreakdownDetailDataSet.TableBreakdownDetail].Rows.Count == 0)
            {
                MessageBox.Show(this,"No Breakdown has been entered",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            //else if (!this.CheckRateBreakdown())
            //{
            //    decimal totalRate = this.CheckTotalBreakdown();
            //    MessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, "Breakdown entered does not match with Client's total rate ({0}), please check",totalRate.ToString("N2")), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}

            if (DataLayer.SaveBreakdownForClient(this._Client.ID, this._BreakdownDetail, Program.LoggedInCompanyId))
            {
                MessageBox.Show(this, "Breakdown saved successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(this, "Breakdown could not be saved please check logfile", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool CheckRateBreakdown()
        {
            decimal totalRate = this._Client.TotalRate;
            decimal rates = 0;
            if (this._BreakdownDetail.Tables[BreakdownDetailDataSet.TableBreakdownDetail].Rows.Count > 0)
            {
                foreach (DataRow rowItem in this._BreakdownDetail.Tables[BreakdownDetailDataSet.TableBreakdownDetail].Rows)
                {
                    rates += Convert.ToDecimal(rowItem[BreakdownDetailDataSet.RateColumn]);
                }
            }
            return (totalRate == rates);
        }

        private void FormatGrid()
        {
            DataGridViewColumn column = null;
            DataGridViewButtonColumn buttonColumn = null;
            DataGridViewComboBoxColumn comboColumn = null;

            this.dgvCustomers.AllowUserToAddRows = false;
            this.dgvCustomers.AllowUserToDeleteRows = false;
            this.dgvCustomers.AllowUserToResizeRows = false;
            this.dgvCustomers.AutoGenerateColumns = false;
            this.dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCustomers.BackgroundColor = Color.White;
            this.dgvCustomers.MultiSelect = false;
            this.dgvCustomers.RowHeadersVisible = false;
            this.dgvCustomers.ScrollBars = ScrollBars.Vertical;
            this.dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.dgvCustomers.Columns.Clear();

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = BreakdownDetailDataSet.CustomerIdColumn;
            column.FillWeight = 20;
            column.HeaderText = "Customer ID";
            column.Name = BreakdownDetailDataSet.CustomerIdColumn;
            column.ReadOnly = true;
            column.Width = 20;
            this.dgvCustomers.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = BreakdownDetailDataSet.CustomerCodeColumn;
            column.FillWeight = 80;
            column.HeaderText = "Customer Code";
            column.Name = BreakdownDetailDataSet.CustomerCodeColumn;
            column.ReadOnly = true;
            column.Width = 80;
            this.dgvCustomers.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = BreakdownDetailDataSet.CustomerNameColumn;
            column.FillWeight = 150;
            column.HeaderText = "Customer Name";
            column.Name = BreakdownDetailDataSet.CustomerNameColumn;
            column.ReadOnly = true;
            column.Width = 150;
            this.dgvCustomers.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = BreakdownDetailDataSet.ChargeHeadIdColumn;
            column.FillWeight = 20;
            column.HeaderText = "Charge Head ID";
            column.Name = BreakdownDetailDataSet.ChargeHeadIdColumn;
            column.ReadOnly = true;
            column.Width = 20;
            this.dgvCustomers.Columns.Add(column);

            comboColumn = new DataGridViewComboBoxColumn();
            comboColumn.DataPropertyName = BreakdownDetailDataSet.ChargeHeadIdColumn;
            comboColumn.FillWeight = 120;
            comboColumn.HeaderText = "Charge Head";
            comboColumn.Name = BreakdownDetailDataSet.ChargeHeadNameColumn;
            comboColumn.DataSource = DataLayer.PopulateCharges(Program.LoggedInCompanyId).Tables[ChargeDataSet.TableChargeHead];
            comboColumn.DisplayMember = ChargeDataSet.NameColumn;
            comboColumn.ValueMember = ChargeDataSet.IdColumn;
            comboColumn.Width = 120;
            this.dgvCustomers.Columns.Add(comboColumn);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = BreakdownDetailDataSet.RateColumn;
            column.FillWeight = 100;
            column.HeaderText = "Rate";
            column.Name = BreakdownDetailDataSet.RateColumn;
            column.ReadOnly = false;
            column.Width = 100;
            column.DefaultCellStyle.Format = "#0.00";
            this.dgvCustomers.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = BreakdownDetailDataSet.InvoiceCycleColumn;
            column.FillWeight = 70;
            column.HeaderText = "Invoice cycle (months)";
            column.Name = BreakdownDetailDataSet.InvoiceCycleColumn;
            column.ReadOnly = false;
            column.Width = 70;
            column.DefaultCellStyle.Format = "#0";
            this.dgvCustomers.Columns.Add(column);

            buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.FillWeight = 80;
            buttonColumn.FlatStyle = FlatStyle.Flat;
            buttonColumn.Name = "DeleteButton";
            buttonColumn.Text = "Delete";
            buttonColumn.UseColumnTextForButtonValue = true;
            buttonColumn.HeaderText = string.Empty;
            buttonColumn.Width = 80;
            this.dgvCustomers.Columns.Add(buttonColumn);

            this.dgvCustomers.Columns[BreakdownDetailDataSet.CustomerIdColumn].Visible = false;
            this.dgvCustomers.Columns[BreakdownDetailDataSet.ChargeHeadIdColumn].Visible = false;
        }

        private void OnDeleteButtonClick(object sender, DataGridViewCellEventArgs e)
        {
            int customerId = 0;
            DataRow[] selectedRow = null;
            if (e.RowIndex < 0) return;
            switch (this.dgvCustomers.Columns[e.ColumnIndex].Name)
            {
                case "DeleteButton":
                    {
                        if (MessageBox.Show(this, "Are you sure you want to remove selected row?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            customerId = Convert.ToInt32(this.dgvCustomers.Rows[e.RowIndex].Cells[BreakdownDetailDataSet.CustomerIdColumn].Value);
                            selectedRow = this._BreakdownDetail.Tables[BreakdownDetailDataSet.TableBreakdownDetail].Select(string.Format(CultureInfo.CurrentCulture, "{0} = {1}", BreakdownDetailDataSet.CustomerIdColumn, customerId));
                            if (selectedRow.Length > 0)
                            {
                                this._BreakdownDetail.Tables[BreakdownDetailDataSet.TableBreakdownDetail].Rows.Remove(selectedRow[0]);
                                this._BreakdownDetail.AcceptChanges();
                            }
                        }
                        break;
                    }
            }
        }

        private void OnChargeSelected(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgvCustomers.SelectedRows.Count == 0) return;

            int customerId = Convert.ToInt32(this.dgvCustomers.SelectedRows[0].Cells[BreakdownDetailDataSet.CustomerIdColumn].Value);
            int chargeId = 0;
            switch (this.dgvCustomers.Columns[e.ColumnIndex].HeaderText)
            {
                case "Charge Head":
                    {
                        DataRow[] row = this._BreakdownDetail.Tables[BreakdownDetailDataSet.TableBreakdownDetail].Select(string.Format(CultureInfo.CurrentCulture, "{0} = {1}", BreakdownDetailDataSet.CustomerIdColumn, customerId));
                        if (row.Length > 0)
                        {
                            chargeId = Convert.ToInt32(this.dgvCustomers.SelectedRows[0].Cells[BreakdownDetailDataSet.ChargeHeadNameColumn].Value);
                            row[0][BreakdownDetailDataSet.ChargeHeadIdColumn] = chargeId;
                            this._BreakdownDetail.Tables[BreakdownDetailDataSet.TableBreakdownDetail].AcceptChanges();
                            this.dgvCustomers.SelectedRows[0].Cells[BreakdownDetailDataSet.ChargeHeadNameColumn].Value = chargeId;
                        }
                        break;
                    }
            }
        }

        #endregion
    }
}
