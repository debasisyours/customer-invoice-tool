using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using CustomerInvoice.Common;
using CustomerInvoice.Data;
using CustomerInvoice.Data.DataSets;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace CustomerInvoice.UI
{
    public partial class InvoiceHistoryForm : Form
    {
        #region Private Field declaration

        private InvoiceHistoryDataSet _HistoryData = null;

        #endregion

        public InvoiceHistoryForm()
        {
            InitializeComponent();
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.AssignEventHandlers();
            this.FormatGrid();
            this.chkCreditNotes.Checked = true;
            this.chkInvoice.Checked = true;
        }

        #endregion

        #region Custom functions

        private void AssignEventHandlers()
        {
            this.btnPrint.Click += new EventHandler(OnPrint);
            this.btnSinglePrint.Click += new EventHandler(OnExport);
            this.btnClose.Click += new EventHandler(OnClose);
            this.chkSelect.CheckedChanged += new EventHandler(OnCheckUncheck);
            this.dgvHistory.CellFormatting += new DataGridViewCellFormattingEventHandler(OnHistoryRowLoaded);
            this.chkCreditNotes.CheckedChanged += new EventHandler(OnInvoiceChecked);
            this.chkInvoice.CheckedChanged += new EventHandler(OnCreditNoteChecked);
            this.txtSearch.TextChanged += new EventHandler(OnSearchUpdated);
            this.dgvHistory.CellMouseClick += new DataGridViewCellMouseEventHandler(OnGridHeaderClicked);
            this.btnEdit.Click += new EventHandler(OnEditHistoryInvoice);
            this.btnDelete.Click+=new EventHandler(OnDeleteInvoice);
            this.btnExportExcel.Click += new EventHandler(OnTotalExport);
        }

        private void OnClose(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OnDeleteInvoice(object sender, EventArgs e)
        {
            int selectedId = 0;
            if (this.dgvHistory.SelectedRows.Count == 0) return;
            selectedId = Convert.ToInt32((this.dgvHistory.SelectedRows[0].Cells[InvoiceHistoryDataSet.IdColumn].Value));
            if (
                MessageBox.Show(this, "Are you sure you want to delete the selected invoice?", Application.ProductName,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (DataLayer.DeleteInvoice(selectedId))
                {
                    MessageBox.Show(this, "Invoice has been deleted successfully", Application.ProductName,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.dgvHistory.CellFormatting -= new DataGridViewCellFormattingEventHandler(OnHistoryRowLoaded);
                    this.FormatGrid();
                    this.dgvHistory.CellFormatting += new DataGridViewCellFormattingEventHandler(OnHistoryRowLoaded);
                }
            }
        }

        private void OnEditHistoryInvoice(object sender, EventArgs e)
        {
            long invoiceId = 0;
            int creditNoteId = 0;
            string invoiceType = string.Empty;
            foreach(DataGridViewRow rowItem in this.dgvHistory.Rows)
            {
                if(Convert.ToBoolean(rowItem.Cells[InvoiceHistoryDataSet.SelectColumn].Value) == true)
                {
                    invoiceType = Convert.ToString(rowItem.Cells[InvoiceHistoryDataSet.InvoiceTypeColumn].Value);
                    if (string.Compare(invoiceType, "SI", true) == 0)
                    {
                        invoiceId = Convert.ToInt64(rowItem.Cells[InvoiceHistoryDataSet.InvoiceNumberColumn].Value);
                        using (InvoiceEditForm invoiceEdit = new InvoiceEditForm(invoiceId,true))
                        {
                            DialogResult result = invoiceEdit.ShowDialog();
                            if (result == DialogResult.OK)
                            {
                                this.dgvHistory.CellFormatting -= new DataGridViewCellFormattingEventHandler(OnHistoryRowLoaded);
                                this.FormatGrid();
                                this.dgvHistory.CellFormatting += new DataGridViewCellFormattingEventHandler(OnHistoryRowLoaded);
                            }
                        }
                    }
                    else if(string.Compare(invoiceType, "SC", true) == 0)
                    {
                        creditNoteId = Convert.ToInt32(rowItem.Cells[InvoiceHistoryDataSet.IdColumn].Value);
                        using (CreditNoteForm invoiceEdit = new CreditNoteForm(creditNoteId))
                        {
                            DialogResult result = invoiceEdit.ShowDialog();
                            if (result == DialogResult.OK)
                            {
                                this.dgvHistory.CellFormatting -= new DataGridViewCellFormattingEventHandler(OnHistoryRowLoaded);
                                this.FormatGrid();
                                this.dgvHistory.CellFormatting += new DataGridViewCellFormattingEventHandler(OnHistoryRowLoaded);
                            }
                        }
                    }
                }
            }
        }

        private void OnPrint(object sender, EventArgs e)
        {
            bool selected = false;
            int customerId = 0;
            string customerName=string.Empty;
            bool multiMonth = false;
            long invoiceId = 0;
            if (this.dgvHistory.RowCount == 0) return;

            foreach(DataGridViewRow row in this.dgvHistory.Rows)
            {
                if (Convert.ToBoolean(row.Cells[InvoiceHistoryDataSet.SelectColumn].Value) == true)
                {
                    selected = true;
                    break;
                }
            }

            if (!selected)
            {
                MessageBox.Show(this,"No Invoice(s) are selected for printing",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }

            foreach (DataGridViewRow row in this.dgvHistory.Rows)
            {
                if (Convert.ToBoolean(row.Cells[InvoiceHistoryDataSet.SelectColumn].Value) == true)
                {
                    if (string.Compare("SI", Convert.ToString(row.Cells[InvoiceHistoryDataSet.InvoiceTypeColumn].Value), false) == 0)
                    {
                        invoiceId = Convert.ToInt64(row.Cells[InvoiceHistoryDataSet.IdColumn].Value);
                        customerName = Convert.ToString(row.Cells[InvoiceHistoryDataSet.CustomerNameColumn].Value);
                        multiMonth = Convert.ToBoolean(row.Cells[InvoiceHistoryDataSet.MultiMonthColumn].Value);
                        customerId = DataLayer.GetSingleCustomerFromName(customerName, Program.LoggedInCompanyId).ID;
                        using (PrintForm print = new PrintForm(invoiceId, customerId,true,false, multiMonth))
                        {
                            print.ShowDialog();
                        }
                    }
                    else if (string.Compare("SC", Convert.ToString(row.Cells[InvoiceHistoryDataSet.InvoiceTypeColumn].Value), false) == 0)
                    {
                        invoiceId = Convert.ToInt64(row.Cells[InvoiceHistoryDataSet.IdColumn].Value);
                        using (CreditNotePrintForm creditPrint = new CreditNotePrintForm(Convert.ToInt32(invoiceId),true))
                        {
                            creditPrint.ShowDialog();
                        }
                    }
                }
            }
        }

        private void OnInvoiceChecked(object sender, EventArgs e)
        {
            this.ApplyFilter(this.chkInvoice.Checked, this.chkCreditNotes.Checked);
        }

        private void OnCreditNoteChecked(object sender, EventArgs e)
        {
            this.ApplyFilter(this.chkInvoice.Checked, this.chkCreditNotes.Checked);
        }

        private void ApplyFilter(bool showInvoice, bool showNote)
        {
            string filterCondition = string.Empty;
            if (showNote)
            {
                filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} = '{1}'", InvoiceHistoryDataSet.InvoiceTypeColumn, "SC");
            }
            else
            {
                filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} <> '{1}'", InvoiceHistoryDataSet.InvoiceTypeColumn, "SC");
            }

            if (showInvoice)
            {
                filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} OR {1} = '{2}'",filterCondition, InvoiceHistoryDataSet.InvoiceTypeColumn, "SI");
            }
            else
            {
                filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} OR {1} <> '{2}'",filterCondition, InvoiceHistoryDataSet.InvoiceTypeColumn, "SI");
            }
            DataRow[] selectedRows = this._HistoryData.Tables[InvoiceHistoryDataSet.TableInvoiceHistory].Select(filterCondition);
            InvoiceHistoryDataSet tmpData = new InvoiceHistoryDataSet();
            if (selectedRows != null && selectedRows.Length > 0)
            {
                foreach (DataRow row in selectedRows)
                {
                    tmpData.Tables[InvoiceHistoryDataSet.TableInvoiceHistory].ImportRow(row);
                }
            }
            this.dgvHistory.DataSource = tmpData.Tables[InvoiceHistoryDataSet.TableInvoiceHistory];
        }

        private void OnExport(object sender, EventArgs e)
        {
            bool selected = false;
            if (this.dgvHistory.RowCount == 0) return;
            DateTime invoiceDate;
            decimal invoiceAmount = 0;
            string dateValue = string.Empty;
            Company tmpCompany = DataLayer.GetCompanySingle(Program.LoggedInCompanyId);
            Client tmpClient = null;

            foreach (DataRow row in this._HistoryData.Tables[InvoiceHistoryDataSet.TableInvoiceHistory].Rows)
            {
                if (Convert.ToBoolean(row[InvoiceHistoryDataSet.SelectColumn]) == true)
                {
                    selected = true;
                    break;
                }
            }

            if (!selected)
            {
                MessageBox.Show(this, "No Invoice(s) are selected for printing", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            StringBuilder builder = null;
            GlobalSetting setting = DataLayer.PopulateGlobalSettings(Program.LoggedInCompanyId);
            string fileName = string.Format(CultureInfo.CurrentCulture, "InvoiceExport-{0}{1}{2}{3}{4}{5}.csv",
                                                                DateTime.Now.Year,
                                                                DateTime.Now.Month.ToString().PadLeft(2,'0'),
                                                                DateTime.Now.Day.ToString().PadLeft(2, '0'),
                                                                DateTime.Now.Hour.ToString().PadLeft(2, '0'),
                                                                DateTime.Now.Minute.ToString().PadLeft(2, '0'),
                                                                DateTime.Now.Second.ToString().PadLeft(2, '0'));
            try
            {
                if (setting == null || string.IsNullOrEmpty(setting.InvoiceExportPath))
                {
                    MessageBox.Show(this, "Invoice export path is not set in settings", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!Directory.Exists(setting.InvoiceExportPath))
                {
                    MessageBox.Show(this, "Invoice export path is not valid", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                using (FileStream stream = new FileStream(Path.Combine(setting.InvoiceExportPath, fileName), FileMode.CreateNew, FileAccess.Write))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        builder = new StringBuilder();
                        builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", "Type"));
                        builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", "Account Reference"));
                        builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", "Nominal A/C Ref"));
                        builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", "Department Code"));
                        builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", "Date"));
                        builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", "Reference"));
                        builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", "Details"));
                        builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", "Net Amount"));
                        builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", "Tax Code"));
                        builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", "Tax Amount"));
                        writer.WriteLine(builder.ToString().Substring(0, builder.ToString().Length - 1));

                        foreach (DataGridViewRow rowItem in this.dgvHistory.Rows)
                        {
                            if (Convert.ToBoolean(rowItem.Cells[InvoiceHistoryDataSet.SelectColumn].Value))
                            {
                                tmpClient = DataLayer.GetSingleClientFromName(rowItem.Cells[InvoiceHistoryDataSet.ClientNameColumn].Value.ToString(),Program.LoggedInCompanyId);
                                builder = new StringBuilder();
                                builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", "SI"));
                                invoiceDate = Convert.ToDateTime(rowItem.Cells[InvoiceHistoryDataSet.InvoiceDateColumn].Value);
                                dateValue = invoiceDate.Day.ToString().PadLeft(2, '0') + '/' + invoiceDate.Month.ToString().PadLeft(2, '0') + invoiceDate.Year.ToString();
                                invoiceAmount = Convert.ToDecimal(rowItem.Cells[InvoiceHistoryDataSet.NetAmountColumn].Value);
                                builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", rowItem.Cells[InvoiceHistoryDataSet.ClientCodeColumn].Value.ToString()));
                                builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", tmpCompany.AccountCode));
                                builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", tmpCompany.AccountNumber));
                                builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", dateValue));
                                builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", tmpClient.SageReference));
                                builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", rowItem.Cells[InvoiceHistoryDataSet.CustomerNameColumn].Value.ToString()));
                                builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", invoiceAmount.ToString("N2")));
                                builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", "T9"));
                                builder.Append(string.Format(CultureInfo.CurrentCulture, "{0},", "0"));
                                writer.WriteLine(builder.ToString().Substring(0, builder.ToString().Length - 1));
                            }
                        }
                    }
                }
                MessageBox.Show(this, "Export completed successfully",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }
            finally 
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void OnCheckUncheck(object sender, EventArgs e)
        {
            if (string.Compare(this.chkSelect.Text, "Select All", false, CultureInfo.CurrentCulture) == 0)
            {
                foreach (DataRow row in this._HistoryData.Tables[InvoiceHistoryDataSet.TableInvoiceHistory].Rows)
                {
                    row[InvoiceHistoryDataSet.SelectColumn] = true;
                }
                this._HistoryData.AcceptChanges();
                this.chkSelect.Text = "Clear All";
            }
            else
            {
                foreach (DataRow row in this._HistoryData.Tables[InvoiceHistoryDataSet.TableInvoiceHistory].Rows)
                {
                    row[InvoiceHistoryDataSet.SelectColumn] = false;
                }
                this._HistoryData.AcceptChanges();
                this.chkSelect.Text = "Select All";
            }
            this.dgvHistory.Refresh();
            this.Refresh();
        }

        private void FormatGrid()
        {
            DataGridViewColumn column = null;
            DataGridViewCheckBoxColumn checkColumn = null;

            this.dgvHistory.AllowUserToAddRows = false;
            this.dgvHistory.AllowUserToDeleteRows = false;
            this.dgvHistory.AllowUserToResizeRows = false;
            this.dgvHistory.AutoGenerateColumns = false;
            this.dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHistory.BackgroundColor = Color.White;
            this.dgvHistory.MultiSelect = false;
            this.dgvHistory.ReadOnly = false;
            this.dgvHistory.RowHeadersVisible = false;
            this.dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.dgvHistory.Columns.Clear();

            checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.DataPropertyName = InvoiceHistoryDataSet.SelectColumn;
            checkColumn.FillWeight = 40;
            checkColumn.FlatStyle = FlatStyle.Flat;
            checkColumn.HeaderText = "Select";
            checkColumn.Name = InvoiceHistoryDataSet.SelectColumn;
            checkColumn.ReadOnly = false;
            checkColumn.Width = 40;
            this.dgvHistory.Columns.Add(checkColumn);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.IdColumn;
            column.FillWeight = 40;
            column.HeaderText = "Id";
            column.Name = InvoiceHistoryDataSet.IdColumn;
            column.ReadOnly = true;
            column.Width = 40;
            column.Visible = false;
            this.dgvHistory.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.InvoiceTypeColumn;
            column.FillWeight = 60;
            column.HeaderText = "Type";
            column.Name = InvoiceHistoryDataSet.InvoiceTypeColumn;
            column.ReadOnly = true;
            column.Width = 60;
            this.dgvHistory.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.InvoiceNumberColumn;
            column.FillWeight = 80;
            column.HeaderText = "Invoice No";
            column.Name = InvoiceHistoryDataSet.InvoiceNumberColumn;
            column.ReadOnly = true;
            column.Width = 80;
            this.dgvHistory.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.InvoiceDateColumn;
            column.FillWeight = 80;
            column.HeaderText = "Date";
            column.Name = InvoiceHistoryDataSet.InvoiceDateColumn;
            column.DefaultCellStyle.Format = "dd/MM/yyyy";
            column.ReadOnly = true;
            column.Width = 80;
            this.dgvHistory.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.ClientCodeColumn;
            column.FillWeight = 80;
            column.HeaderText = "Client Code";
            column.Name = InvoiceHistoryDataSet.ClientCodeColumn;
            column.ReadOnly = true;
            column.Width = 80;
            this.dgvHistory.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.ClientNameColumn;
            column.FillWeight = 100;
            column.HeaderText = "Client Name";
            column.Name = InvoiceHistoryDataSet.ClientNameColumn;
            column.ReadOnly = true;
            column.Width = 100;
            this.dgvHistory.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.CustomerCodeColumn;
            column.FillWeight = 80;
            column.HeaderText = "Customer Code";
            column.Name = InvoiceHistoryDataSet.CustomerCodeColumn;
            column.ReadOnly = true;
            column.Width = 80;
            this.dgvHistory.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.CustomerNameColumn;
            column.FillWeight = 100;
            column.HeaderText = "Customer Name";
            column.Name = InvoiceHistoryDataSet.CustomerNameColumn;
            column.ReadOnly = true;
            column.Width = 100;
            this.dgvHistory.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.ChargeNameColumn;
            column.FillWeight = 90;
            column.HeaderText = "Charge Name";
            column.Name = InvoiceHistoryDataSet.ChargeNameColumn;
            column.ReadOnly = true;
            column.Width = 90;
            this.dgvHistory.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.WeeklyRateColumn;
            column.FillWeight = 80;
            column.HeaderText = "Weekly Rate";
            column.Name = InvoiceHistoryDataSet.WeeklyRateColumn;
            column.ReadOnly = true;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.Width = 80;
            column.DefaultCellStyle.Format = "#0.00";
            this.dgvHistory.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.DaysColumn;
            column.FillWeight = 60;
            column.HeaderText = "Days";
            column.Name = InvoiceHistoryDataSet.DaysColumn;
            column.ReadOnly = true;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.Width = 60;
            this.dgvHistory.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.SubTotalColumn;
            column.FillWeight = 80;
            column.HeaderText = "Sub Total";
            column.Name = InvoiceHistoryDataSet.SubTotalColumn;
            column.ReadOnly = true;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.Width = 80;
            column.DefaultCellStyle.Format = "#0.00";
            this.dgvHistory.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.LessHeadColumn;
            column.FillWeight = 90;
            column.HeaderText = "Less Desc.";
            column.Name = InvoiceHistoryDataSet.LessHeadColumn;
            column.ReadOnly = true;
            column.Width = 90;
            this.dgvHistory.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.LessAmountColumn;
            column.FillWeight = 80;
            column.HeaderText = "Less Amount";
            column.Name = InvoiceHistoryDataSet.LessAmountColumn;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.ReadOnly = true;
            column.Width = 80;
            column.DefaultCellStyle.Format = "#0.00";
            this.dgvHistory.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.ExtraHeadColumn;
            column.FillWeight = 90;
            column.HeaderText = "Extra Desc.";
            column.Name = InvoiceHistoryDataSet.ExtraHeadColumn;
            column.ReadOnly = true;
            column.Width = 90;
            this.dgvHistory.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.ExtraAmountColumn;
            column.FillWeight = 80;
            column.HeaderText = "Extra Amount";
            column.Name = InvoiceHistoryDataSet.ExtraAmountColumn;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.ReadOnly = true;
            column.Width = 80;
            column.DefaultCellStyle.Format = "#0.00";
            this.dgvHistory.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.NetAmountColumn;
            column.FillWeight = 80;
            column.HeaderText = "Net Amount";
            column.Name = InvoiceHistoryDataSet.NetAmountColumn;
            column.ReadOnly = true;
            column.Width = 80;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.DefaultCellStyle.Format = "#0.00";
            this.dgvHistory.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.ChargePeriodColumn;
            column.FillWeight = 100;
            column.HeaderText = "Charge period";
            column.Name = InvoiceHistoryDataSet.ChargePeriodColumn;
            column.ReadOnly = true;
            column.Width = 100;
            this.dgvHistory.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.DeletedColumn;
            column.FillWeight = 10;
            column.HeaderText = "Deleted";
            column.Name = InvoiceHistoryDataSet.DeletedColumn;
            column.ReadOnly = true;
            column.Width = 10;
            this.dgvHistory.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceHistoryDataSet.MultiMonthColumn;
            column.FillWeight = 10;
            column.HeaderText = "MultiMonth";
            column.Name = InvoiceHistoryDataSet.MultiMonthColumn;
            column.ReadOnly = true;
            column.Width = 10;
            this.dgvHistory.Columns.Add(column);

            this._HistoryData = DataLayer.PopulateInvoiceHistory(Program.LoggedInCompanyId);
            this.dgvHistory.DataSource = this._HistoryData.Tables[InvoiceHistoryDataSet.TableInvoiceHistory];
            this.dgvHistory.Columns[InvoiceHistoryDataSet.IdColumn].Visible = false;
            this.dgvHistory.Columns[InvoiceHistoryDataSet.DeletedColumn].Visible = false;
            this.dgvHistory.Columns[InvoiceHistoryDataSet.MultiMonthColumn].Visible = false;
        }

        private void OnHistoryRowLoaded(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (Convert.ToBoolean(this.dgvHistory.Rows[e.RowIndex].Cells[InvoiceHistoryDataSet.DeletedColumn].Value))
            {
                foreach (DataGridViewCell cellItem in this.dgvHistory.Rows[e.RowIndex].Cells)
                {
                    cellItem.Style.BackColor = Color.Yellow;
                }
            }
        }

        private void OnGridHeaderClicked(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch (this.dgvHistory.Columns[e.ColumnIndex].Name)
            {
                case InvoiceHistoryDataSet.ClientCodeColumn:
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Client code");
                        break;
                    }
                case InvoiceHistoryDataSet.ClientNameColumn:
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Client name");
                        break;
                    }
                case InvoiceHistoryDataSet.CustomerCodeColumn:
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Customer code");
                        break;
                    }
                case InvoiceHistoryDataSet.CustomerNameColumn:
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Customer name");
                        break;
                    }
                case InvoiceHistoryDataSet.ExtraHeadColumn:
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Extra head");
                        break;
                    }
                case InvoiceHistoryDataSet.InvoiceNumberColumn:
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Invoice number");
                        break;
                    }
                case InvoiceHistoryDataSet.InvoiceTypeColumn:
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Invoice type");
                        break;
                    }
                case InvoiceHistoryDataSet.LessHeadColumn:
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Less head");
                        break;
                    }
            }
            this.txtSearch.Focus();
        }

        private void OnSearchUpdated(object sender, EventArgs e)
        {
            string filterCondition = string.Empty;

            if (this.lblSearch.Text.Trim().Length > 9)
            {
                switch (this.lblSearch.Text.Substring(9).Trim())
                {
                    case "Client code":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", InvoiceHistoryDataSet.ClientCodeColumn, this.txtSearch.Text);
                            break;
                        }
                    case "Client name":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", InvoiceHistoryDataSet.ClientNameColumn, this.txtSearch.Text);
                            break;
                        }
                    case "Customer code":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", InvoiceHistoryDataSet.CustomerCodeColumn, this.txtSearch.Text);
                            break;
                        }
                    case "Customer name":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", InvoiceHistoryDataSet.CustomerNameColumn, this.txtSearch.Text);
                            break;
                        }
                    case "Extra head":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", InvoiceHistoryDataSet.ExtraHeadColumn, this.txtSearch.Text);
                            break;
                        }
                    case "Invoice number":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", InvoiceHistoryDataSet.InvoiceNumberColumn, this.txtSearch.Text);
                            break;
                        }
                    case "Invoice type":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", InvoiceHistoryDataSet.InvoiceTypeColumn, this.txtSearch.Text);
                            break;
                        }
                    case "Less head":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", InvoiceHistoryDataSet.LessHeadColumn, this.txtSearch.Text);
                            break;
                        }
                }
            }

            InvoiceHistoryDataSet tmpData = new InvoiceHistoryDataSet();
            DataRow[] selectedRows = this._HistoryData.Tables[InvoiceHistoryDataSet.TableInvoiceHistory].Select(filterCondition);

            foreach (DataRow rowItem in selectedRows)
            {
                tmpData.Tables[InvoiceHistoryDataSet.TableInvoiceHistory].ImportRow(rowItem);
            }

            this.dgvHistory.DataSource = tmpData.Tables[InvoiceHistoryDataSet.TableInvoiceHistory];
            this.dgvHistory.Columns[InvoiceHistoryDataSet.IdColumn].Visible = false;
            this.dgvHistory.Columns[InvoiceHistoryDataSet.DeletedColumn].Visible = false;
        }

        private void OnTotalExport(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            int row = 1;
            string exportFileName = $"InvoiceHistoryExport-{currentTime.Year}{currentTime.Month.ToString().PadLeft(2, '0')}" +
                $"{currentTime.Day.ToString().PadLeft(2, '0')}{currentTime.Hour.ToString().PadLeft(2, '0')}{currentTime.Minute.ToString().PadLeft(2, '0')}" +
                $"{currentTime.Second.ToString().PadLeft(2, '0')}.{currentTime.Millisecond.ToString().PadLeft(3, '0')}.xlsx";

            try
            {
                this.Cursor = Cursors.WaitCursor;
                var settings = DataLayer.PopulateGlobalSettings(Program.LoggedInCompanyId);

                using (var excelPackage = new ExcelPackage())
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Invoice Export");
                    var fileInfo = new FileInfo(Path.Combine(settings.InvoiceExportPath, exportFileName));

                    sheet.Cells[1, 1].Value = "Id";
                    sheet.Cells[1, 2].Value = "Type";
                    sheet.Cells[1, 3].Value = "Invoice No";
                    sheet.Cells[1, 4].Value = "Date";
                    sheet.Cells[1, 5].Value = "Client Code";
                    sheet.Cells[1, 6].Value = "Client Name";
                    sheet.Cells[1, 7].Value = "Customer Code";
                    sheet.Cells[1, 8].Value = "Customer Name";
                    sheet.Cells[1, 9].Value = "Charge Name";
                    sheet.Cells[1, 10].Value = "Weekly Rate";
                    sheet.Cells[1, 11].Value = "Days";
                    sheet.Cells[1, 12].Value = "Subtotal";
                    sheet.Cells[1, 13].Value = "Less Description";
                    sheet.Cells[1, 14].Value = "Less Amount";
                    sheet.Cells[1, 15].Value = "Extra Description";
                    sheet.Cells[1, 16].Value = "Extra Amount";
                    sheet.Cells[1, 17].Value = "Net Amount";
                    sheet.Cells[1, 18].Value = "Charge Period";

                    sheet.Select("A1:R1");
                    sheet.SelectedRange.Style.Font.Name = "Tahoma";
                    sheet.SelectedRange.Style.Font.Bold = true;
                    sheet.SelectedRange.Style.Font.Size = 9;

                    for(int loop = 0; loop < this.dgvHistory.Rows.Count; loop++)
                    {
                        sheet.Cells[row + loop + 1, 1].Value = this.dgvHistory.Rows[loop].Cells[InvoiceHistoryDataSet.IdColumn].Value.ToString();
                        sheet.Cells[row + loop + 1, 2].Value = this.dgvHistory.Rows[loop].Cells[InvoiceHistoryDataSet.InvoiceTypeColumn].Value.ToString();
                        sheet.Cells[row + loop + 1, 3].Value = this.dgvHistory.Rows[loop].Cells[InvoiceHistoryDataSet.InvoiceNumberColumn].Value.ToString();
                        sheet.Cells[row + loop + 1, 4].Value = this.dgvHistory.Rows[loop].Cells[InvoiceHistoryDataSet.InvoiceDateColumn].Value.ToString();
                        sheet.Cells[row + loop + 1, 5].Value = this.dgvHistory.Rows[loop].Cells[InvoiceHistoryDataSet.ClientCodeColumn].Value.ToString();
                        sheet.Cells[row + loop + 1, 6].Value = this.dgvHistory.Rows[loop].Cells[InvoiceHistoryDataSet.ClientNameColumn].Value.ToString();
                        sheet.Cells[row + loop + 1, 7].Value = this.dgvHistory.Rows[loop].Cells[InvoiceHistoryDataSet.CustomerCodeColumn].Value.ToString();
                        sheet.Cells[row + loop + 1, 8].Value = this.dgvHistory.Rows[loop].Cells[InvoiceHistoryDataSet.CustomerNameColumn].Value.ToString();
                        sheet.Cells[row + loop + 1, 9].Value = this.dgvHistory.Rows[loop].Cells[InvoiceHistoryDataSet.ChargeNameColumn].Value.ToString();
                        sheet.Cells[row + loop + 1, 10].Value = this.dgvHistory.Rows[loop].Cells[InvoiceHistoryDataSet.WeeklyRateColumn].Value.ToString();
                        sheet.Cells[row + loop + 1, 11].Value = this.dgvHistory.Rows[loop].Cells[InvoiceHistoryDataSet.DaysColumn].Value.ToString();
                        sheet.Cells[row + loop + 1, 12].Value = this.dgvHistory.Rows[loop].Cells[InvoiceHistoryDataSet.SubTotalColumn].Value.ToString();
                        sheet.Cells[row + loop + 1, 13].Value = this.dgvHistory.Rows[loop].Cells[InvoiceHistoryDataSet.LessHeadColumn].Value.ToString();
                        sheet.Cells[row + loop + 1, 14].Value = this.dgvHistory.Rows[loop].Cells[InvoiceHistoryDataSet.LessAmountColumn].Value.ToString();
                        sheet.Cells[row + loop + 1, 15].Value = this.dgvHistory.Rows[loop].Cells[InvoiceHistoryDataSet.ExtraHeadColumn].Value.ToString();
                        sheet.Cells[row + loop + 1, 16].Value = this.dgvHistory.Rows[loop].Cells[InvoiceHistoryDataSet.ExtraAmountColumn].Value.ToString();
                        sheet.Cells[row + loop + 1, 17].Value = this.dgvHistory.Rows[loop].Cells[InvoiceHistoryDataSet.NetAmountColumn].Value.ToString();
                        sheet.Cells[row + loop + 1, 18].Value = this.dgvHistory.Rows[loop].Cells[InvoiceHistoryDataSet.ChargePeriodColumn].Value.ToString();
                        if(this.dgvHistory.Rows[loop].Cells[InvoiceHistoryDataSet.ChargeNameColumn].Style.BackColor== Color.Yellow)
                        {
                            sheet.Select($"A{row + loop + 1}:R{row + loop + 1}");
                            sheet.SelectedRange.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                        }
                    }

                    sheet.Select($"A2:R{this.dgvHistory.Rows.Count+1}");
                    sheet.SelectedRange.Style.Font.Name = "Tahoma";
                    sheet.SelectedRange.Style.Font.Bold = false;
                    sheet.SelectedRange.Style.Font.Size = 8;

                    excelPackage.SaveAs(fileInfo);
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }
            finally
            {
                MessageBox.Show(this, "Invoice history has been successfully exported.", "Report generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Cursor = Cursors.Default;
            }
        }

        #endregion
    }
}
