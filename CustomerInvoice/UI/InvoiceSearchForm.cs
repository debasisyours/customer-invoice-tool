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
using System.Globalization;
using CustomerInvoice.Data;
using CustomerInvoice.Data.DataSets;

namespace CustomerInvoice.UI
{
    public partial class InvoiceSearchForm : Form
    {
        #region Field declaration

        private InvoiceDataSet _Invoices;

        #endregion

        public InvoiceSearchForm()
        {
            InitializeComponent();
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.AssignEventHandlers();
            this.InitializeControls();
            this.FormatGrid();
        }

        #endregion

        #region Custom Methods

        private void AssignEventHandlers()
        {
            this.chkDateFilter.CheckedChanged += new EventHandler(OnCheckedFilter);
            this.btnAdd.Click += new EventHandler(OnAddInvoice);
            this.btnEdit.Click += new EventHandler(OnEditInvoice);
            this.btnCancel.Click += new EventHandler(OnExit);
            this.btnDelete.Click += new EventHandler(OnDeleteInvoice);
            this.btnPrint.Click += new EventHandler(OnPrint);
            this.btnManualInvoice.Click += new EventHandler(OnManualInvoice);
            this.dgvInvoices.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(OnGridColumnHeaderClick);
            this.dgvInvoices.KeyDown += new KeyEventHandler(OnKeyDown);
            this.dgvInvoices.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(OnGridDoubleClick);
            this.txtSearch.TextChanged += new EventHandler(OnTextChange);
            this.txtSearch.KeyDown += new KeyEventHandler(OnKeyDown);
            this.btnPrintList.Click += new EventHandler(OnPrintList);
            this.btnExcelExport.Click += new EventHandler(OnExcelExport);
        }

        private void OnExcelExport(object sender, EventArgs e)
        {
            Program.GenerateExcelReport(this.dgvInvoices, "InvoiceList", "Net Amount");
            MessageBox.Show(this, "Invoice list exported successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void InitializeControls()
        {
            this.dtpFrom.Value = Convert.ToDateTime("01/" + this.GetMonthShortName(DateTime.Today.Month) + "/" + DateTime.Today.Year);
            this.dtpTo.Value = this.dtpFrom.Value.AddMonths(1).AddDays(-1);
            this.chkDateFilter.Checked = false;
            this.dtpFrom.Enabled = false;
            this.dtpTo.Enabled = false;
        }

        private void OnCheckedFilter(object sender, EventArgs e)
        {
            if (this.chkDateFilter.Checked)
            {
                this.dtpFrom.Enabled = true;
                this.dtpTo.Enabled = true;
            }
            else
            {
                this.dtpFrom.Enabled = false;
                this.dtpTo.Enabled = false;
            }
        }

        private void FormatGrid()
        {
            DataGridViewColumn column = null;

            this.dgvInvoices.AllowUserToAddRows = false;
            this.dgvInvoices.AllowUserToDeleteRows = false;
            this.dgvInvoices.AllowUserToResizeRows = false;
            this.dgvInvoices.AutoGenerateColumns = false;
            this.dgvInvoices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvInvoices.BackgroundColor = Color.White;
            this.dgvInvoices.MultiSelect = true;
            this.dgvInvoices.ReadOnly = true;
            this.dgvInvoices.RowHeadersVisible = false;
            this.dgvInvoices.ScrollBars = ScrollBars.Vertical;
            this.dgvInvoices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.dgvInvoices.Columns.Clear();
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceDataSet.IdColumn;
            column.FillWeight = 50;
            column.HeaderText = "ID";
            column.Name = InvoiceDataSet.IdColumn;
            column.ReadOnly = true;
            column.Width = 50;
            column.Visible = false;
            this.dgvInvoices.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceDataSet.InvoiceNumberColumn;
            column.FillWeight = 120;
            column.HeaderText = "Invoice No";
            column.Name = InvoiceDataSet.InvoiceNumberColumn;
            column.ReadOnly = true;
            column.Width = 120;
            this.dgvInvoices.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceDataSet.InvoiceDateColumn;
            column.FillWeight = 100;
            column.HeaderText = "Invoice Date";
            column.Name = InvoiceDataSet.InvoiceDateColumn;
            column.ReadOnly = true;
            column.Width = 100;
            this.dgvInvoices.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceDataSet.ClientNameColumn;
            column.FillWeight = 200;
            column.HeaderText = "Client Name";
            column.Name = InvoiceDataSet.ClientNameColumn;
            column.ReadOnly = true;
            column.Width = 200;
            this.dgvInvoices.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceDataSet.NetAmountColumn;
            column.FillWeight = 100;
            column.HeaderText = "Net Amount";
            column.Name = InvoiceDataSet.NetAmountColumn;
            column.ReadOnly = true;
            column.Width = 100;
            column.DefaultCellStyle.Format = "#0.00";
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dgvInvoices.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceDataSet.MultiMonthColumn;
            column.FillWeight = 100;
            column.HeaderText = "MultiMonth";
            column.Name = InvoiceDataSet.MultiMonthColumn;
            column.ReadOnly = true;
            column.Width = 100;
            this.dgvInvoices.Columns.Add(column);

            this._Invoices = DataLayer.PopulateInvoices(Program.LoggedInCompanyId);
            this.dgvInvoices.DataSource = this._Invoices.Tables[InvoiceDataSet.TableInvoice];
            this.dgvInvoices.Columns[InvoiceDataSet.IdColumn].Visible = false;
            this.dgvInvoices.Columns[InvoiceDataSet.MultiMonthColumn].Visible = false;

            this.UpdateTotalAmount();
        }

        private void UpdateTotalAmount()
        {
            decimal totalAmount = 0;
            foreach (DataRow row in this._Invoices.Tables[InvoiceDataSet.TableInvoice].Rows)
            {
                totalAmount += Convert.ToDecimal(row[InvoiceDataSet.NetAmountColumn]);
            }
            this.txtTotalAmount.Text = totalAmount.ToString("N2");
        }

        private string GetMonthShortName(int month)
        {
            string shortName = string.Empty;
            switch (month)
            {
                case 1:
                    {
                        shortName = "Jan";
                        break;
                    }
                case 2:
                    {
                        shortName = "Feb";
                        break;
                    }
                case 3:
                    {
                        shortName = "Mar";
                        break;
                    }
                case 4:
                    {
                        shortName = "Apr";
                        break;
                    }
                case 5:
                    {
                        shortName = "May";
                        break;
                    }
                case 6:
                    {
                        shortName = "Jun";
                        break;
                    }
                case 7:
                    {
                        shortName = "Jul";
                        break;
                    }
                case 8:
                    {
                        shortName = "Aug";
                        break;
                    }
                case 9:
                    {
                        shortName = "Sep";
                        break;
                    }
                case 10:
                    {
                        shortName = "Oct";
                        break;
                    }
                case 11:
                    {
                        shortName = "Nov";
                        break;
                    }
                case 12:
                    {
                        shortName = "Dec";
                        break;
                    }
            }
            return shortName;
        }

        private void OnManualInvoice(object sender, EventArgs e)
        {
            DialogResult result;
            using (ManualInvoiceForm manualInvoice = new ManualInvoiceForm())
            {
                result = manualInvoice.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.FormatGrid();
                }
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OnAddInvoice(object sender, EventArgs e)
        {
            DialogResult result;
            using (InvoiceForm invoiceForm = new InvoiceForm())
            {
                result = invoiceForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.FormatGrid();
                }
            }
        }

        private void OnEditInvoice(object sender, EventArgs e)
        {
            DialogResult result;
            long invoiceId = 0;
            if (this.dgvInvoices.SelectedRows.Count == 0) return;
            invoiceId = Convert.ToInt64(this.dgvInvoices.SelectedRows[0].Cells[InvoiceDataSet.IdColumn].Value);

            Invoice tmpInvoice = DataLayer.GetInvoiceSingle(invoiceId);
            if (tmpInvoice.Printed)
            {
                MessageBox.Show(this, "Invoice already posted, modification not possible", Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }

            using (InvoiceEditForm invoiceForm = new InvoiceEditForm(invoiceId,false))
            {
                result = invoiceForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.FormatGrid();
                }
            }
        }

        private void OnDeleteInvoice(object sender, EventArgs e)
        {
            long invoiceId = 0;
            bool success = false;
            if (this.dgvInvoices.SelectedRows.Count == 0) return;
            
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
                        foreach (DataGridViewRow rows in this.dgvInvoices.SelectedRows)
                        {
                            invoiceId = Convert.ToInt64(rows.Cells[InvoiceDataSet.IdColumn].Value);
                            success = DataLayer.DeleteInvoice(invoiceId);
                            if (!success) break;
                        }
                        if (success)
                        {
                            MessageBox.Show(this, "Selected invoice(s) deleted successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.FormatGrid();
                            return;
                        }
                        else
                        {
                            MessageBox.Show(this, "One or more invoice could not be deleted please check log file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
            }
        }

        private void OnPrint(object sender, EventArgs e)
        {
            long invoiceId = 0;
            bool multiMonth = false;
            if (this.dgvInvoices.SelectedRows.Count == 0) return;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                foreach (DataGridViewRow row in this.dgvInvoices.SelectedRows)
                {
                    invoiceId = Convert.ToInt64(row.Cells[InvoiceDataSet.IdColumn].Value);
                    multiMonth = Convert.ToBoolean(row.Cells[InvoiceDataSet.MultiMonthColumn].Value);
                    List<int> customerList = DataLayer.GetCustomersForInvoice(invoiceId);
                    if (customerList != null && customerList.Count > 0)
                    {
                        for (int customerCount = 0; customerCount < customerList.Count; customerCount++)
                        {
                            using (PrintForm print = new PrintForm(invoiceId, customerList[customerCount],true,false,multiMonth))
                            {
                                if (this.chkShowPrint.Checked)
                                {
                                    print.ShowDialog();
                                }
                                DataLayer.UpdateInvoicePrintedByUser(invoiceId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(this,"Selected invoice(s) printed successfully",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        protected void OnPrintList(object sender, EventArgs e)
        {
            using (InvoicePrintForm printList = new InvoicePrintForm(this._Invoices))
            {
                printList.ShowDialog();
            }
        }

        #endregion

        #region GridView events

        protected void OnGridColumnHeaderClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch (this.dgvInvoices.Columns[e.ColumnIndex].HeaderText)
            {
                case "Invoice No":
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Invoice No");
                        break;
                    }
                case "Client Name":
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Client Name");
                        break;
                    }
            }
            this.txtSearch.Focus();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && this.dgvInvoices.SelectedRows.Count > 0)
            {
                this.btnEdit.PerformClick();
            }
        }

        private void OnGridDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.btnEdit.PerformClick();
        }

        private void OnTextChange(object sender, EventArgs e)
        {
            string filterCondition = string.Empty;
            InvoiceDataSet tmpDataset = new InvoiceDataSet();
            DataRow[] selectedRows = null;
            try
            {
                if (string.Compare(this.lblSearch.Text, "Search on", false) == 0) return;
                switch (this.lblSearch.Text.Substring(9).Trim())
                {
                    case "Invoice No":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", InvoiceDataSet.InvoiceNumberColumn, this.txtSearch.Text);
                            selectedRows = this._Invoices.Tables[InvoiceDataSet.TableInvoice].Select(filterCondition);
                            foreach (DataRow tmpRow in selectedRows)
                            {
                                tmpDataset.Tables[InvoiceDataSet.TableInvoice].ImportRow(tmpRow);
                            }
                            this.dgvInvoices.DataSource = tmpDataset.Tables[InvoiceDataSet.TableInvoice];
                            break;
                        }
                    case "Client Name":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", InvoiceDataSet.ClientNameColumn, this.txtSearch.Text);
                            selectedRows = this._Invoices.Tables[InvoiceDataSet.TableInvoice].Select(filterCondition);
                            foreach (DataRow tmpRow in selectedRows)
                            {
                                tmpDataset.Tables[InvoiceDataSet.TableInvoice].ImportRow(tmpRow);
                            }
                            this.dgvInvoices.DataSource = tmpDataset.Tables[InvoiceDataSet.TableInvoice];
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        #endregion
    }
}
