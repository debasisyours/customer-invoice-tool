using CustomerInvoice.Data;
using CustomerInvoice.Data.DataSets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomerInvoice.UI
{
    public partial class AmalgamatedInvoiceForm : Form
    {
        #region Declaration

        private int _InvoiceId = 0;
        private bool _loading = true;
        private AmalgamatedInvoiceDetailDataSet _detailDataSet = null;

        #endregion

        #region Constructor
        public AmalgamatedInvoiceForm()
        {
            InitializeComponent();
        }

        public AmalgamatedInvoiceForm(int invoiceId)
        {
            InitializeComponent();
            _InvoiceId = invoiceId;
        }

        #endregion

        #region Form Events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.AssignEventHandlers();
            this.PopulateCustomers();
            this.FormatGrid();
            this.InitializeFormControls();
            this._loading = false;
        }

        #endregion

        #region Custom Functions

        private void PopulateCustomers()
        {
            CustomerDataSet customers = DataLayer.PopulateCustomers(Program.LoggedInCompanyId, true);

            this.cboCustomer.Items.Clear();
            this.cboCustomer.DataSource = customers.Tables[CustomerDataSet.TableCustomer];
            this.cboCustomer.DisplayMember = CustomerDataSet.NameColumn;
            this.cboCustomer.ValueMember = CustomerDataSet.IdColumn;
            this.cboCustomer.SelectedIndex = -1;
        }

        private void InitializeFormControls()
        {
            if (this._InvoiceId > 0)
            {
                var invoice = DataLayer.GetAmalgamatedInvoice(Program.LoggedInCompanyId, this._InvoiceId);
                if (invoice != null)
                {
                    this.txtInvoiceNumber.Text = invoice.InvoiceNumber;
                    this.dtpInvoiceDate.Value = invoice.InvoiceDate;
                    this.cboCustomer.SelectedValue = invoice.CustomerID;
                    this.dtpStartDate.Value = invoice.StartDate;
                    this.dtpEndDate.Value = invoice.EndDate;
                    int dayCount = this.dtpEndDate.Value.Subtract(this.dtpStartDate.Value).Days;
                    this.txtDays.Text = (dayCount + 2).ToString("N0");
                    this.txtDays.ReadOnly = true;
                    this.txtDays.BackColor = Color.White;
                    this.txtInvoiceNumber.ReadOnly = true;
                    this.txtInvoiceNumber.BackColor = Color.White;

                    this._detailDataSet = DataLayer.PopulateAmalgamatedInvoiceDetail(Program.LoggedInCompanyId, this._InvoiceId, invoice.CustomerID);
                    this.dgvDetail.DataSource = this._detailDataSet.Tables[AmalgamatedInvoiceDetailDataSet.TableAmalgamatedInvoiceDetail];
                    this.ReCalculateOnDays();
                    this.dgvDetail.Columns[AmalgamatedInvoiceDetailDataSet.IdColumn].Width = 0;
                    this.dgvDetail.Columns[AmalgamatedInvoiceDetailDataSet.IdColumn].Visible = false;
                }
            }
            else
            {
                this.txtInvoiceNumber.Text = "NEW";
                this.txtInvoiceNumber.ReadOnly = true;
                this.txtInvoiceNumber.BackColor = Color.White;
                this.txtDays.ReadOnly = true;
                this.txtDays.Text = "1";
                this.txtDays.BackColor = Color.White;
                this.dtpInvoiceDate.Focus();
            }
        }

        private void OnDateChange(object sender, EventArgs e)
        {
            if(this.dtpStartDate.Value != null && this.dtpEndDate.Value != null)
            {
                if(this.dtpStartDate.Value.Year == this.dtpEndDate.Value.Year && this.dtpStartDate.Value.Month == this.dtpEndDate.Value.Month && this.dtpStartDate.Value.Day == this.dtpEndDate.Value.Day)
                {
                    this.txtDays.Text = "1";
                    this.ReCalculateOnDays();
                }
                else
                {
                    int dayCount = this.dtpEndDate.Value.Subtract(this.dtpStartDate.Value).Days;
                    this.txtDays.Text = (dayCount + 2).ToString("N0");
                    this.ReCalculateOnDays();
                }                
            }
        }

        private void AssignEventHandlers()
        {
            this.SaveButton.Click += new EventHandler(OnOkClick);
            this.CancelButton.Click += new EventHandler(OnCancelClick);
            this.cboCustomer.SelectedIndexChanged += new EventHandler(OnCustomerSelected);
            this.dtpStartDate.ValueChanged += new EventHandler(OnDateChange);
            this.dtpEndDate.ValueChanged += new EventHandler(OnDateChange);
            this.dgvDetail.CellValidated += new DataGridViewCellEventHandler(OnGridUpdate);
        }

        private void OnGridUpdate(object sender, DataGridViewCellEventArgs e)
        {
            switch (this.dgvDetail.Columns[e.ColumnIndex].HeaderText)
            {
                case "Extra Amount":
                    {
                        this.ReCalculateOnDays();
                        break;
                    }
                case "Less Amount":
                    {
                        this.ReCalculateOnDays();
                        break;
                    }
            }
        }

        private void OnCustomerSelected(object sender, EventArgs e)
        {
            if (this._loading) return;
            if (this.cboCustomer.SelectedItem != null)
            {
                int selectedCustomerId = Convert.ToInt32(this.cboCustomer.SelectedValue);
                this._detailDataSet = DataLayer.PopulateAmalgamatedInvoiceDetail(Program.LoggedInCompanyId, 0, selectedCustomerId);
                this.ReCalculateOnDays();
                this.dgvDetail.DataSource = this._detailDataSet.Tables[AmalgamatedInvoiceDetailDataSet.TableAmalgamatedInvoiceDetail];
                this.dgvDetail.Columns[AmalgamatedInvoiceDetailDataSet.IdColumn].Width = 0;
                this.dgvDetail.Columns[AmalgamatedInvoiceDetailDataSet.IdColumn].Visible = false;
            }
        }

        private void ReCalculateOnDays()
        {
            decimal netAmount = 0;
            decimal totalAmount = 0;
            if(this._detailDataSet!=null && this._detailDataSet.Tables[AmalgamatedInvoiceDetailDataSet.TableAmalgamatedInvoiceDetail].Rows.Count > 0)
            {
                foreach(DataRow row in this._detailDataSet.Tables[AmalgamatedInvoiceDetailDataSet.TableAmalgamatedInvoiceDetail].Rows)
                {
                    row[AmalgamatedInvoiceDetailDataSet.DaysColumn] = Convert.ToInt32(this.txtDays.Text);
                    row[AmalgamatedInvoiceDetailDataSet.SubTotalColumn] = Convert.ToDecimal(row[AmalgamatedInvoiceDetailDataSet.RateColumn]) * Convert.ToInt32(this.txtDays.Text);
                    totalAmount = Convert.ToDecimal(row[AmalgamatedInvoiceDetailDataSet.SubTotalColumn]) + Convert.ToDecimal(row[AmalgamatedInvoiceDetailDataSet.ExtraAmountColumn]) - Convert.ToDecimal(row[AmalgamatedInvoiceDetailDataSet.LessAmountColumn]);
                    row[AmalgamatedInvoiceDetailDataSet.TotalAmountColumn] = Convert.ToDecimal(totalAmount);
                    netAmount += totalAmount;
                }
                this.txtNetAmount.Text = netAmount.ToString("C2");
            }
        }

        private bool IsDataValid()
        {
            bool valid = true;

            DateTime startDate = Convert.ToDateTime($"{this.dtpStartDate.Value.Year}-{this.dtpStartDate.Value.Month.ToString().PadLeft(2, '0')}-{this.dtpStartDate.Value.Day.ToString().PadLeft(2, '0')} 00:00:00");
            DateTime endDate = Convert.ToDateTime($"{this.dtpEndDate.Value.Year}-{this.dtpEndDate.Value.Month.ToString().PadLeft(2, '0')}-{this.dtpEndDate.Value.Day.ToString().PadLeft(2, '0')} 23:59:59");
            if (startDate > endDate)
            {
                MessageBox.Show(this, "Invoice start date can't be more than end date.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dtpStartDate.Focus();
                valid = false;
                return valid;
            }

            if (this.cboCustomer.SelectedItem == null)
            {
                MessageBox.Show(this, "Please select a customer to proceed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.cboCustomer.Focus();
                valid = false;
                return valid;
            }

            return valid;
        }

        private void OnOkClick(object sender, EventArgs e)
        {
            var invoice = CreateInvoiceObject();
            bool success = DataLayer.SaveAmalgamatedInvoice(invoice, this._detailDataSet);
            if (success)
            {
                MessageBox.Show(this, "Amalgamated invoice saved successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(this, "Invoice could not be saved, please check log file for details.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private AmalgamatedInvoice CreateInvoiceObject()
        {
            return new AmalgamatedInvoice
            {
                CompanyId = Program.LoggedInCompanyId,
                ID = this._InvoiceId,
                CustomerID = Convert.ToInt32(this.cboCustomer.SelectedValue),
                EndDate = this.dtpEndDate.Value,
                Deleted = false,
                InvoiceDate = this.dtpInvoiceDate.Value,
                InvoiceNumber = this._InvoiceId == 0 ? "" : this.txtInvoiceNumber.Text,
                MultiMonth = false,
                Narration = String.Empty,
                NetAmount = Decimal.TryParse(this.txtNetAmount.Text, NumberStyles.AllowCurrencySymbol | NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal result) ? result : 0,
                Printed = false,
                StartDate = this.dtpStartDate.Value,
                UserPrinted = false
            };
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormatGrid()
        {
            DataGridViewTextBoxColumn column = null;

            this.dgvDetail.AllowUserToAddRows = false;
            this.dgvDetail.AllowUserToDeleteRows = false;
            this.dgvDetail.AllowUserToResizeRows = false;
            this.dgvDetail.AutoGenerateColumns = false;
            this.dgvDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDetail.BackgroundColor = Color.White;
            this.dgvDetail.ReadOnly = false;
            this.dgvDetail.MultiSelect = false;
            this.dgvDetail.RowHeadersVisible = false;
            this.dgvDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetail.Columns.Clear();

            column = new DataGridViewTextBoxColumn();
            column.Name = AmalgamatedInvoiceDetailDataSet.IdColumn;
            column.DataPropertyName = AmalgamatedInvoiceDetailDataSet.IdColumn;
            column.HeaderText = AmalgamatedInvoiceDetailDataSet.IdColumn;
            column.FillWeight = 10;
            column.Visible = false;
            column.Width = 0;
            column.ReadOnly = true;
            this.dgvDetail.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = AmalgamatedInvoiceDetailDataSet.ClientIdColumn;
            column.DataPropertyName = AmalgamatedInvoiceDetailDataSet.ClientIdColumn;
            column.HeaderText = AmalgamatedInvoiceDetailDataSet.ClientIdColumn;
            column.FillWeight = 10;
            column.Visible = false;
            column.Width = 0;
            column.ReadOnly = true;
            this.dgvDetail.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = AmalgamatedInvoiceDetailDataSet.ChargeHeadIdColumn;
            column.DataPropertyName = AmalgamatedInvoiceDetailDataSet.ChargeHeadIdColumn;
            column.HeaderText = AmalgamatedInvoiceDetailDataSet.ChargeHeadIdColumn;
            column.FillWeight = 10;
            column.Visible = false;
            column.Width = 0;
            column.ReadOnly = true;
            this.dgvDetail.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = AmalgamatedInvoiceDetailDataSet.ClientNameColumn;
            column.DataPropertyName = AmalgamatedInvoiceDetailDataSet.ClientNameColumn;
            column.HeaderText = "Client";
            column.FillWeight = 30;
            column.Visible = true;
            column.Width = 30;
            column.ReadOnly = true;
            this.dgvDetail.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = AmalgamatedInvoiceDetailDataSet.ChargeHeadNameColumn;
            column.DataPropertyName = AmalgamatedInvoiceDetailDataSet.ChargeHeadNameColumn;
            column.HeaderText = "Charge Head";
            column.FillWeight = 30;
            column.Visible = true;
            column.Width = 30;
            column.ReadOnly = true;
            this.dgvDetail.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = AmalgamatedInvoiceDetailDataSet.RateColumn;
            column.DataPropertyName = AmalgamatedInvoiceDetailDataSet.RateColumn;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.HeaderText = "Weekly Rate";
            column.DefaultCellStyle.Format = "#0.00";
            column.FillWeight = 20;
            column.Visible = true;
            column.Width = 20;
            column.ReadOnly = true;
            this.dgvDetail.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = AmalgamatedInvoiceDetailDataSet.DaysColumn;
            column.DataPropertyName = AmalgamatedInvoiceDetailDataSet.DaysColumn;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.HeaderText = "Days";
            column.FillWeight = 20;
            column.Visible = true;
            column.Width = 20;
            column.ReadOnly = true;
            this.dgvDetail.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = AmalgamatedInvoiceDetailDataSet.SubTotalColumn;
            column.DataPropertyName = AmalgamatedInvoiceDetailDataSet.SubTotalColumn;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.HeaderText = "Subtotal";
            column.DefaultCellStyle.Format = "#0.00";
            column.FillWeight = 20;
            column.Visible = true;
            column.Width = 20;
            column.ReadOnly = true;
            this.dgvDetail.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = AmalgamatedInvoiceDetailDataSet.ExtraHeadColumn;
            column.DataPropertyName = AmalgamatedInvoiceDetailDataSet.ExtraHeadColumn;
            column.HeaderText = "Extra Head";
            column.ReadOnly = false;
            column.FillWeight = 30;
            column.Visible = true;
            column.Width = 30;
            column.ReadOnly = false;
            this.dgvDetail.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = AmalgamatedInvoiceDetailDataSet.ExtraAmountColumn;
            column.DataPropertyName = AmalgamatedInvoiceDetailDataSet.ExtraAmountColumn;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.HeaderText = "Extra Amount";
            column.ReadOnly = false;
            column.DefaultCellStyle.Format = "#0.00";
            column.FillWeight = 20;
            column.Visible = true;
            column.Width = 20;
            column.ReadOnly = false;
            this.dgvDetail.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = AmalgamatedInvoiceDetailDataSet.LessHeadColumn;
            column.DataPropertyName = AmalgamatedInvoiceDetailDataSet.LessHeadColumn;
            column.HeaderText = "Less Head";
            column.ReadOnly = false;
            column.FillWeight = 30;
            column.Visible = true;
            column.Width = 30;
            column.ReadOnly = false;
            this.dgvDetail.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = AmalgamatedInvoiceDetailDataSet.LessAmountColumn;
            column.DataPropertyName = AmalgamatedInvoiceDetailDataSet.LessAmountColumn;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.HeaderText = "Less Amount";
            column.ReadOnly = false;
            column.DefaultCellStyle.Format = "#0.00";
            column.FillWeight = 20;
            column.Visible = true;
            column.Width = 20;
            column.ReadOnly = false;
            this.dgvDetail.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = AmalgamatedInvoiceDetailDataSet.TotalAmountColumn;
            column.DataPropertyName = AmalgamatedInvoiceDetailDataSet.TotalAmountColumn;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.HeaderText = "Total Amount";
            column.DefaultCellStyle.Format = "#0.00";
            column.FillWeight = 30;
            column.Visible = true;
            column.Width = 30;
            column.ReadOnly = true;
            this.dgvDetail.Columns.Add(column);
        }

        #endregion
    }
}
