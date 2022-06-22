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
    public partial class InvoiceEditForm : Form
    {
        #region Field declaration

        private long _InvoiceId = 0;
        private InvoiceDetailDataSet _InvoiceData = null;
        private bool _HistoryInvoice = false;

        #endregion

        public InvoiceEditForm()
        {
            InitializeComponent();
        }

        public InvoiceEditForm(long invoiceId, bool historyInovice)
        {
            InitializeComponent();
            this._InvoiceId = invoiceId;
            Invoice invoice = DataLayer.GetInvoiceSingle(invoiceId);
            this.txtInvoiceNumber.Text = invoice.InvoiceNumber;
            this.dtpInvoiceDate.Value = invoice.InvoiceDate;
            this.dtpStart.Value = invoice.StartDate;
            this.dtpEnd.Value = invoice.EndDate;
            this.txtNarration.Text = invoice.Narration;
            Client clientRef = DataLayer.GetSingleClient(invoice.ClientID);
            if (clientRef != null)
            {
                this.txtClient.Text = clientRef.Name;
            }
            this.txtClient.Enabled = false;
            this.txtInvoiceNumber.Enabled = false;
            this.dtpInvoiceDate.Enabled = false;
            this._HistoryInvoice = historyInovice;
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.FormatGrid();
            this.AssignEventHandlers();
        }

        #endregion

        #region Custom methods

        private void AssignEventHandlers()
        {
            this.btnSave.Click += new EventHandler(OnSave);
            this.btnCancel.Click += new EventHandler(OnExit);
            this.dgvDetails.CellValueChanged += new DataGridViewCellEventHandler(OnColumnUpdated);
            this.dtpStart.ValueChanged += new EventHandler(OnDateUpdated);
            this.dtpEnd.ValueChanged += new EventHandler(OnDateUpdated);
        }

        private void FormatGrid()
        {
            DataGridViewColumn column = null;

            this.dgvDetails.AllowUserToAddRows = false;
            this.dgvDetails.AllowUserToDeleteRows = false;
            this.dgvDetails.AllowUserToResizeRows = false;
            this.dgvDetails.AutoGenerateColumns = false;
            this.dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDetails.BackgroundColor = Color.White;
            this.dgvDetails.MultiSelect = false;
            this.dgvDetails.RowHeadersVisible = false;
            this.dgvDetails.ScrollBars = ScrollBars.Vertical;
            this.dgvDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.dgvDetails.Columns.Clear();

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceDetailDataSet.IdColumn;
            column.FillWeight = 20;
            column.HeaderText = "ID";
            column.Name = InvoiceDetailDataSet.IdColumn;
            column.ReadOnly = true;
            column.Width = 20;
            column.Visible = false;
            this.dgvDetails.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceDetailDataSet.CustomerIdColumn;
            column.FillWeight = 20;
            column.HeaderText = "Customer ID";
            column.Name = InvoiceDetailDataSet.CustomerIdColumn;
            column.ReadOnly = true;
            column.Width = 20;
            column.Visible = false;
            this.dgvDetails.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceDetailDataSet.CustomerNameColumn;
            column.FillWeight = 100;
            column.HeaderText = "Customer Name";
            column.Name = InvoiceDetailDataSet.CustomerNameColumn;
            column.ReadOnly = true;
            column.DefaultCellStyle.BackColor = Color.LightGray;
            column.Width = 100;
            this.dgvDetails.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceDetailDataSet.ChargeHeadIdColumn;
            column.FillWeight = 20;
            column.HeaderText = "ChargeHead ID";
            column.Name = InvoiceDetailDataSet.ChargeHeadIdColumn;
            column.ReadOnly = true;
            column.Width = 20;
            column.Visible = false;
            this.dgvDetails.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceDetailDataSet.ChargeHeadNameColumn;
            column.FillWeight = 100;
            column.HeaderText = "Charge Head";
            column.Name = InvoiceDetailDataSet.ChargeHeadNameColumn;
            column.DefaultCellStyle.BackColor = Color.LightGray;
            column.ReadOnly = true;
            column.Width = 100;
            this.dgvDetails.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceDetailDataSet.WeeklyRateColumn;
            column.FillWeight = 80;
            column.HeaderText = "Weekly Rate";
            column.Name = InvoiceDetailDataSet.WeeklyRateColumn;
            column.ReadOnly = false;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.Width = 80;
            column.DefaultCellStyle.Format = "#0.00";
            this.dgvDetails.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceDetailDataSet.TotalDaysColumn;
            column.FillWeight = 80;
            column.HeaderText = "Total Days";
            column.Name = InvoiceDetailDataSet.TotalDaysColumn;
            column.ReadOnly = false;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.Width = 80;
            this.dgvDetails.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceDetailDataSet.SubTotalColumn;
            column.FillWeight = 80;
            column.HeaderText = "Sub Total";
            column.Name = InvoiceDetailDataSet.SubTotalColumn;
            column.DefaultCellStyle.BackColor = Color.LightGray;
            column.DefaultCellStyle.Format = "#0.00";
            column.ReadOnly = false;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.Width = 80;
            this.dgvDetails.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceDetailDataSet.ExtraPayHeadColumn;
            column.FillWeight = 100;
            column.HeaderText = "Extra Pay";
            column.Name = InvoiceDetailDataSet.ExtraPayHeadColumn;
            column.ReadOnly = false;
            column.Width = 100;
            this.dgvDetails.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceDetailDataSet.ExtraAmountColumn;
            column.FillWeight = 80;
            column.HeaderText = "Extra Amout";
            column.Name = InvoiceDetailDataSet.ExtraAmountColumn;
            column.DefaultCellStyle.Format = "#0.00";
            column.ReadOnly = false;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.Width = 80;
            this.dgvDetails.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceDetailDataSet.LessPayHeadColumn;
            column.FillWeight = 100;
            column.HeaderText = "Less Pay";
            column.Name = InvoiceDetailDataSet.LessPayHeadColumn;
            column.ReadOnly = false;
            column.Width = 100;
            this.dgvDetails.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceDetailDataSet.LessAmountColumn;
            column.FillWeight = 80;
            column.HeaderText = "Less Amout";
            column.Name = InvoiceDetailDataSet.LessAmountColumn;
            column.DefaultCellStyle.Format = "#0.00";
            column.ReadOnly = false;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.Width = 80;
            this.dgvDetails.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceDetailDataSet.NetAmountColumn;
            column.FillWeight = 80;
            column.HeaderText = "Net Amout";
            column.Name = InvoiceDetailDataSet.NetAmountColumn;
            column.DefaultCellStyle.BackColor = Color.LightGray;
            column.DefaultCellStyle.Format = "#0.00";
            column.ReadOnly = false;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.Width = 80;
            this.dgvDetails.Columns.Add(column);

            this._InvoiceData = DataLayer.PopulateInvoiceData(this._InvoiceId);
            this.dgvDetails.DataSource = this._InvoiceData.Tables[InvoiceDetailDataSet.TableInvoiceDetail];
            this.UpdateInvoiceAmount();

            this.dgvDetails.Columns[InvoiceDetailDataSet.IdColumn].Visible = false;
            this.dgvDetails.Columns[InvoiceDetailDataSet.ChargeHeadIdColumn].Visible = false;
            this.dgvDetails.Columns[InvoiceDetailDataSet.CustomerIdColumn].Visible = false;
        }

        private void OnExit(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OnSave(object sender, EventArgs e)
        {
            if (DataLayer.SaveInvoiceDetail(this._InvoiceId, this._InvoiceData,this.txtNarration.Text, this.dtpStart.Value, this.dtpEnd.Value, this._HistoryInvoice))
            {
                MessageBox.Show(this, "Invoice detail updated successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(this,"Invoice detail could not be updated please see log file for details",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        #endregion

        #region GridView events

        private void OnDateUpdated(object sender, EventArgs e)
        {
            decimal extraAmount = 0;
            decimal lessAmount = 0;
            decimal weeklyRate = 0;
            int dayCount = (this.dtpEnd.Value - this.dtpStart.Value).Days + 1;
            foreach(DataRow rowItem in this._InvoiceData.Tables[InvoiceDetailDataSet.TableInvoiceDetail].Rows)
            {
                rowItem[InvoiceDetailDataSet.TotalDaysColumn] = dayCount;

                weeklyRate = Convert.ToDecimal(rowItem[InvoiceDetailDataSet.WeeklyRateColumn]);
                extraAmount = Convert.ToDecimal(rowItem[InvoiceDetailDataSet.ExtraAmountColumn]);
                lessAmount = Convert.ToDecimal(rowItem[InvoiceDetailDataSet.LessAmountColumn]);
                rowItem[InvoiceDetailDataSet.SubTotalColumn] = Convert.ToDecimal(weeklyRate) / Convert.ToDecimal("7.0") * dayCount;
                rowItem[InvoiceDetailDataSet.NetAmountColumn] = Convert.ToDecimal(weeklyRate) / Convert.ToDecimal("7.0") * dayCount + extraAmount - lessAmount;
            }
            this.UpdateInvoiceAmount();
        }
        private void OnColumnUpdated(object sender, DataGridViewCellEventArgs e)
        {
            decimal weeklyRate=0;
            int days=0;
            decimal extraAmount=0;
            decimal lessAmount=0;
            switch (this.dgvDetails.Columns[e.ColumnIndex].Name)
            {
                case InvoiceDetailDataSet.ExtraAmountColumn:
                    {
                        weeklyRate = Convert.ToDecimal(this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.WeeklyRateColumn].Value);
                        days = Convert.ToInt32(this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.TotalDaysColumn].Value);
                        extraAmount=Convert.ToDecimal(this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.ExtraAmountColumn].Value);
                        lessAmount=Convert.ToDecimal(this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.LessAmountColumn].Value);
                        this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.NetAmountColumn].Value = Convert.ToDecimal(weeklyRate) / Convert.ToDecimal("7.0") * days + extraAmount - lessAmount;
                        break;
                    }
                case InvoiceDetailDataSet.LessAmountColumn:
                    {
                        weeklyRate = Convert.ToDecimal(this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.WeeklyRateColumn].Value);
                        days = Convert.ToInt32(this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.TotalDaysColumn].Value);
                        extraAmount = Convert.ToDecimal(this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.ExtraAmountColumn].Value);
                        lessAmount = Convert.ToDecimal(this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.LessAmountColumn].Value);
                        this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.NetAmountColumn].Value = Convert.ToDecimal(weeklyRate) / Convert.ToDecimal("7.0") * days + extraAmount - lessAmount;
                        break;
                    }
                case InvoiceDetailDataSet.TotalDaysColumn:
                    {
                        weeklyRate = Convert.ToDecimal(this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.WeeklyRateColumn].Value);
                        days = Convert.ToInt32(this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.TotalDaysColumn].Value);
                        extraAmount = Convert.ToDecimal(this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.ExtraAmountColumn].Value);
                        lessAmount = Convert.ToDecimal(this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.LessAmountColumn].Value);
                        this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.SubTotalColumn].Value = Convert.ToDecimal(weeklyRate) / Convert.ToDecimal("7.0") * days;
                        this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.NetAmountColumn].Value = Convert.ToDecimal(weeklyRate) / Convert.ToDecimal("7.0") * days + extraAmount - lessAmount;
                        break;
                    }
                case InvoiceDetailDataSet.WeeklyRateColumn:
                    {
                        weeklyRate = Convert.ToDecimal(this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.WeeklyRateColumn].Value);
                        days = Convert.ToInt32(this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.TotalDaysColumn].Value);
                        extraAmount = Convert.ToDecimal(this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.ExtraAmountColumn].Value);
                        lessAmount = Convert.ToDecimal(this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.LessAmountColumn].Value);
                        this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.SubTotalColumn].Value = Convert.ToDecimal(weeklyRate) / Convert.ToDecimal("7.0") * days;
                        this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.NetAmountColumn].Value = Convert.ToDecimal(weeklyRate) / Convert.ToDecimal("7.0") * days + extraAmount - lessAmount;
                        break;
                    }
                case InvoiceDetailDataSet.SubTotalColumn:
                    {
                        extraAmount = Convert.ToDecimal(this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.ExtraAmountColumn].Value);
                        lessAmount = Convert.ToDecimal(this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.LessAmountColumn].Value);
                        this.dgvDetails.Rows[e.RowIndex].Cells[InvoiceDetailDataSet.NetAmountColumn].Value = Convert.ToDecimal(this.dgvDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) + extraAmount - lessAmount;
                        break;
                    }
            }
            this.UpdateInvoiceAmount();
        }

        private void UpdateInvoiceAmount()
        {
            decimal totalAmount = 0;
            decimal weeklyRate=0;
            decimal days=0;
            decimal extra=0;
            decimal less=0;
            for (int rowCount = 0; rowCount < this._InvoiceData.Tables[InvoiceDetailDataSet.TableInvoiceDetail].Rows.Count; rowCount++)
            {
                weeklyRate=Convert.ToDecimal(this._InvoiceData.Tables[InvoiceDetailDataSet.TableInvoiceDetail].Rows[rowCount][InvoiceDetailDataSet.WeeklyRateColumn]);
                days = Convert.ToInt32(this._InvoiceData.Tables[InvoiceDetailDataSet.TableInvoiceDetail].Rows[rowCount][InvoiceDetailDataSet.TotalDaysColumn]);
                extra = Convert.ToDecimal(this._InvoiceData.Tables[InvoiceDetailDataSet.TableInvoiceDetail].Rows[rowCount][InvoiceDetailDataSet.ExtraAmountColumn]);
                less = Convert.ToDecimal(this._InvoiceData.Tables[InvoiceDetailDataSet.TableInvoiceDetail].Rows[rowCount][InvoiceDetailDataSet.LessAmountColumn]);
                //totalAmount += Convert.ToDecimal(weeklyRate * (days / Convert.ToDecimal(7.0))) + extra - less;

                totalAmount += Convert.ToDecimal(this._InvoiceData.Tables[InvoiceDetailDataSet.TableInvoiceDetail].Rows[rowCount][InvoiceDetailDataSet.NetAmountColumn]);
            }
            this.txtAmount.Text = totalAmount.ToString("N2");
        }

        #endregion
    }
}
