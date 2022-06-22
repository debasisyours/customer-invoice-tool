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

namespace CustomerInvoice.UI
{
    public partial class InvoiceExportForm : Form
    {
        #region Private Field declaration

        private InvoiceCsvDataSet _CsvData = new InvoiceCsvDataSet();

        #endregion

        public InvoiceExportForm()
        {
            InitializeComponent();
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.AssignEventHandlers();
            this.FormatGrid();
        }

        #endregion

        #region Custom methods

        private void AssignEventHandlers()
        {
            this.btnGo.Click += new EventHandler(OnSelectionCriteria);
            this.btnClose.Click += new EventHandler(OnExit);
            this.btnExport.Click += new EventHandler(OnExport);
            this.btnPrint.Click += new EventHandler(OnExportPrint);
            this.chkPrint.Click += new EventHandler(OnSelectDeselect);
            this.chkIncludeExported.CheckedChanged += new EventHandler(OnSelectionCriteria);
            this.btnExportExcel.Click += new EventHandler(OnExcelExport);
        }

        private void OnExcelExport(object sender, EventArgs e)
        {
            bool success = false;
            if (this.dgvExport.RowCount > 0)
            {
                for(int loop=0; loop < this.dgvExport.RowCount; loop++)
                {
                    if (Convert.ToBoolean(this.dgvExport.Rows[loop].Cells[InvoiceCsvDataSet.PrintColumn].Value))
                    {
                        success = true;
                        break;
                    }
                }

                if (!success)
                {
                    MessageBox.Show(this, "Please select invoice(s) to export.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.dgvExport.Focus();
                    return;
                }
            }

            using(var formatSelection = new ExportFormatForm())
            {
                var result = formatSelection.ShowDialog();
                if(result == DialogResult.OK)
                {
                    ExportFormat format = formatSelection.GetFormatSelected;
                    bool markExported = formatSelection.MarkAsExported;
                    switch (format)
                    {
                        case ExportFormat.OldExcel:
                            {
                                Program.GenerateExcelReport(this.dgvExport, "InvoiceExport", "Net Amount");
                                break;
                            }
                        case ExportFormat.Xero:
                            {
                                Program.GenerateXeroCsvReport(this.dgvExport, "Invoice-CSV-Export", markExported);
                                break;
                            }
                    }
                }
            }
            
            MessageBox.Show(this, "Invoice(s) exported to Excel successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnExit(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OnSelectionCriteria(object sender, EventArgs e)
        {
            this.FormatGrid();
        }

        private void OnExport(object sender, EventArgs e)
        {
            long invoiceId = 0;
            bool printNeeded = false;
            List<int> customerId = null;
            int noteId = 0;
            string invoiceType = string.Empty;
            StringBuilder builder = null;
            bool multiMonth = false;
            bool printoutNeeded = false;
            List<string> ignoreColumns = new List<string> { "Print", "Internal ID", "MultiMonth", "Client Name" };
            GlobalSetting setting = DataLayer.PopulateGlobalSettings(Program.LoggedInCompanyId);
            if (this.dgvExport.RowCount == 0) return;
            ExcelPackage package = null;
            int[] columnWidth = new int[] { 15, 25, 25, 20, 20, 30, 40, 20, 20, 20 };
            
            string fileName = string.Format(CultureInfo.CurrentCulture, "InvoiceExport-{0}{1}{2}{3}{4}{5}.xlsx",
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

                FileInfo fileInfo = new FileInfo(Path.Combine(setting.InvoiceExportPath, fileName));
                package = new ExcelPackage(fileInfo);
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Invoice Export");
                for(int column = 1; column< this.dgvExport.Columns.Count; column++)
                {
                    if (ignoreColumns.Contains(this.dgvExport.Columns[column].HeaderText)) continue;
                    sheet.Cells[1, column].Value = this.dgvExport.Columns[column].HeaderText;
                    sheet.Cells[1, column].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);
                    sheet.Cells[1, column].Style.Font.Name = "Tahoma";
                    sheet.Cells[1, column].Style.Font.Size = 8;
                    sheet.Cells[1, column].Style.Font.Bold = true;
                    sheet.Column(column).Width = columnWidth[column - 1];
                    if (column == 8 || column == 10)
                    {
                        sheet.Column(column).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    }
                    else
                    {
                        sheet.Column(column).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        sheet.Column(column).Style.Numberformat.Format = "#.##";
                    }
                }

                for(int row = 0; row < this.dgvExport.Rows.Count; row++)
                {
                    for (int column = 1; column < this.dgvExport.ColumnCount; column++)
                    {
                        if (ignoreColumns.Contains(this.dgvExport.Columns[column].HeaderText)) continue;

                        sheet.Cells[row + 2, column].Style.Font.Name = "Tahoma";
                        sheet.Cells[row + 2, column].Style.Font.Size = 8;

                        if (column == 5)
                        {
                            DateTime parsedDate;
                            bool valid = DateTime.TryParseExact(this.dgvExport.Rows[row].Cells[column].Value.ToString(), "yyyy-MM-dd hh:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out parsedDate);
                            if (valid)
                            {
                                sheet.Cells[row + 2, column].Style.Numberformat.Format = "dd-MMM-yyyy";
                                sheet.Cells[row + 2, column].Formula = $"=DATE({parsedDate.Year},{parsedDate.Month},{parsedDate.Day})";
                            }
                            else
                            {
                                parsedDate = Convert.ToDateTime(this.dgvExport.Rows[row].Cells[column].Value);
                                sheet.Cells[row + 2, column].Value =  $"{parsedDate.Day.ToString().PadLeft(2,'0')}-{GetMonthName(parsedDate.Month)}-{parsedDate.Year}";
                            }
                        }
                        else
                        {
                            sheet.Cells[row + 2, column].Value = this.dgvExport.Rows[row].Cells[column].Value.ToString();
                        }
                    }

                    invoiceType = Convert.ToString(this.dgvExport.Rows[row].Cells[InvoiceCsvDataSet.TypeColumn].Value);
                    printNeeded = Convert.ToBoolean(this.dgvExport.Rows[row].Cells[InvoiceCsvDataSet.PrintColumn].Value);

                    if (printNeeded)
                    {
                        if (string.Compare(invoiceType, "SI", false) == 0)
                        {
                            invoiceId = Convert.ToInt64(this.dgvExport.Rows[row].Cells[InvoiceCsvDataSet.InternalIdColumn].Value);
                            multiMonth =
                                Convert.ToBoolean(
                                    this.dgvExport.Rows[row].Cells[InvoiceCsvDataSet.MultiMonthColumn].Value);
                            customerId = DataLayer.GetCustomersForInvoice(invoiceId);
                            printoutNeeded = Convert.ToBoolean(this.dgvExport.Rows[row].Cells[InvoiceCsvDataSet.PrintColumn].Value);

                            if (customerId != null && customerId.Count > 0)
                            {
                                foreach (int customerEntry in customerId)
                                {
                                    using (PrintForm print = new PrintForm(invoiceId, customerEntry, false, printoutNeeded, multiMonth))
                                    {
                                        // Do nothing
                                    }
                                }
                            }
                            DataLayer.UpdateInvoicePrinted(invoiceId);
                        }
                        else
                        {
                            noteId = Convert.ToInt32(this.dgvExport.Rows[row].Cells[InvoiceCsvDataSet.InternalIdColumn].Value);
                            using (CreditNotePrintForm print = new CreditNotePrintForm(noteId, false))
                            {
                                //Do nothing
                            }
                            DataLayer.UpdateCreditNotePrinted(noteId);
                        }
                    }
                }

                package.Save();
                package.Dispose();

                MessageBox.Show(this, "Export completed successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            finally 
            {
                this.Cursor = Cursors.Default;
            }
        }

        private string GetMonthName(int month)
        {
            string monthName = string.Empty;
            switch (month)
            {
                case 1:
                    {
                        monthName = "Jan";
                        break;
                    }
                case 2:
                    {
                        monthName = "Feb";
                        break;
                    }
                case 3:
                    {
                        monthName = "Mar";
                        break;
                    }
                case 4:
                    {
                        monthName = "Apr";
                        break;
                    }
                case 5:
                    {
                        monthName = "May";
                        break;
                    }
                case 6:
                    {
                        monthName = "Jun";
                        break;
                    }
                case 7:
                    {
                        monthName = "Jul";
                        break;
                    }
                case 8:
                    {
                        monthName = "Aug";
                        break;
                    }
                case 9:
                    {
                        monthName = "Sep";
                        break;
                    }
                case 10:
                    {
                        monthName = "Oct";
                        break;
                    }
                case 11:
                    {
                        monthName = "Nov";
                        break;
                    }
                case 12:
                    {
                        monthName = "Dec";
                        break;
                    }
            }
            return monthName;
        }

        private void OnExportPrint(object sender, EventArgs e)
        {
            using (ExportListPrintForm listPrint = new ExportListPrintForm(this._CsvData))
            {
                listPrint.ShowDialog();
            }
        }

        private void OnPrint(object sender, EventArgs e)
        {
            long invoiceId = 0;
            int creditNoteId = 0;
            string noteNumber = string.Empty;
            string invoiceType = string.Empty;
            bool multiMonth = false;
            DataRow[] tmpNote = null;
            List<int> customerId = null;
            
            if (this.dgvExport.SelectedRows.Count == 0) return;

            CreditNoteDataSet noteData = DataLayer.PopulateCreditNoteData(Program.LoggedInCompanyId);

            foreach (DataGridViewRow rowItem in this.dgvExport.SelectedRows)
            {
                invoiceType = rowItem.Cells[InvoiceCsvDataSet.TypeColumn].Value.ToString();
                if (string.Compare(invoiceType, "SI", false) == 0)
                {
                    invoiceId = Convert.ToInt64(rowItem.Cells[InvoiceCsvDataSet.TransactionNumberColumn].Value);
                    multiMonth = Convert.ToBoolean(rowItem.Cells[InvoiceCsvDataSet.MultiMonthColumn].Value);
                    customerId = DataLayer.GetCustomersForInvoice(invoiceId);
                    foreach (int customerItem in customerId)
                    {
                        using (PrintForm print = new PrintForm(invoiceId, customerItem,true,false,multiMonth))
                        {
                            // Invoice will get printed
                        }
                    }
                }
                else
                {
                    noteNumber = rowItem.Cells[InvoiceCsvDataSet.TransactionNumberColumn].Value.ToString();
                    tmpNote = noteData.Tables[CreditNoteDataSet.TableCreditNote].Select(string.Format(CultureInfo.CurrentCulture, "{0}='{1}'", CreditNoteDataSet.TransactionNumberColumn, noteNumber));
                    if (tmpNote.Length > 0)
                    {
                        creditNoteId = Convert.ToInt32(tmpNote[0][CreditNoteDataSet.IdColumn]);
                    }
                    
                    using (CreditNotePrintForm print = new CreditNotePrintForm(creditNoteId,true))
                    {
                        // Credit note will be printed
                    }
                }
            }

            MessageBox.Show(this, "Selected invoice(s) printed successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        private void FormatGrid()
        {
            DataGridViewColumn column = null;
            DataGridViewCheckBoxColumn checkColumn = null;

            this.dgvExport.AllowUserToAddRows = false;
            this.dgvExport.AllowUserToDeleteRows = false;
            this.dgvExport.AllowUserToResizeRows = false;
            this.dgvExport.AutoGenerateColumns = false;
            this.dgvExport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvExport.BackgroundColor = Color.White;
            this.dgvExport.MultiSelect = true;
            this.dgvExport.RowHeadersVisible = false;
            this.dgvExport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.dgvExport.Columns.Clear();

            checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.DataPropertyName = InvoiceCsvDataSet.PrintColumn;
            checkColumn.FillWeight = 40;
            checkColumn.FlatStyle = FlatStyle.Flat;
            checkColumn.HeaderText = "Print";
            checkColumn.Name = InvoiceCsvDataSet.PrintColumn;
            checkColumn.ReadOnly = false;
            checkColumn.Width = 40;
            this.dgvExport.Columns.Add(checkColumn);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceCsvDataSet.TypeColumn;
            column.FillWeight = 60;
            column.HeaderText = "Type";
            column.Name = InvoiceCsvDataSet.TypeColumn;
            column.Width = 60;
            this.dgvExport.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceCsvDataSet.SageReferenceColumn;
            column.FillWeight = 90;
            column.HeaderText = "Account Reference";
            column.Name = InvoiceCsvDataSet.SageReferenceColumn;
            column.Width = 90;
            this.dgvExport.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();   
            column.DataPropertyName = InvoiceCsvDataSet.AccountCodeColumn;
            column.FillWeight = 100;
            column.HeaderText = "Nominal A/C Ref";
            column.Name = InvoiceCsvDataSet.AccountCodeColumn;
            column.Width = 100;
            this.dgvExport.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceCsvDataSet.AccountNumberColumn;
            column.FillWeight = 90;
            column.HeaderText = "Department Code";
            column.Name = InvoiceCsvDataSet.AccountNumberColumn;
            column.Width = 90;
            this.dgvExport.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceCsvDataSet.TransactionDateColumn;
            column.FillWeight = 80;
            column.HeaderText = "Date";
            column.Name = InvoiceCsvDataSet.TransactionDateColumn;
            column.DefaultCellStyle.Format = "dd/MM/yyyy";
            column.Width = 80;
            this.dgvExport.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceCsvDataSet.TransactionNumberColumn;
            column.FillWeight = 80;
            column.HeaderText = "Reference";
            column.Name = InvoiceCsvDataSet.TransactionNumberColumn;
            column.Width = 80;
            this.dgvExport.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceCsvDataSet.NarrationColumn;
            column.FillWeight = 120;
            column.HeaderText = "Details";
            column.Name = InvoiceCsvDataSet.NarrationColumn;
            column.Width = 120;
            this.dgvExport.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceCsvDataSet.InvoiceAmountColumn;
            column.FillWeight = 80;
            column.HeaderText = "Net Amount";
            column.Name = InvoiceCsvDataSet.InvoiceAmountColumn;
            column.Width = 80;
            column.DefaultCellStyle.Format = "#0.00";
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dgvExport.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceCsvDataSet.TaxCodeColumn;
            column.FillWeight = 60;
            column.HeaderText = "Tax Code";
            column.Name = InvoiceCsvDataSet.TaxCodeColumn;
            column.Width = 60;
            this.dgvExport.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceCsvDataSet.TaxAmountColumn;
            column.FillWeight = 60;
            column.HeaderText = "Tax Amount";
            column.Name = InvoiceCsvDataSet.TaxAmountColumn;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.Width = 60;
            column.DefaultCellStyle.Format = "#0.00";
            this.dgvExport.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceCsvDataSet.InternalIdColumn;
            column.FillWeight = 10;
            column.HeaderText = "Internal ID";
            column.Name = InvoiceCsvDataSet.InternalIdColumn;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.Width = 10;
            this.dgvExport.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceCsvDataSet.MultiMonthColumn;
            column.FillWeight = 10;
            column.HeaderText = "MultiMonth";
            column.Name = InvoiceCsvDataSet.MultiMonthColumn;
            column.Width = 10;
            this.dgvExport.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceCsvDataSet.ClientNameColumn;
            column.FillWeight = 80;
            column.HeaderText = "Client Name";
            column.Name = InvoiceCsvDataSet.ClientNameColumn;
            column.Width = 0;
            this.dgvExport.Columns.Add(column);

            /*
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceCsvDataSet.ClientCodeColumn;
            column.FillWeight = 80;
            column.HeaderText = "Client Code";
            column.Name = InvoiceCsvDataSet.ClientCodeColumn;
            column.Width = 80;
            this.dgvExport.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = InvoiceCsvDataSet.ExportFormatColumn;
            column.FillWeight = 60;
            column.HeaderText = "Export Format";
            column.Name = InvoiceCsvDataSet.ExportFormatColumn;
            column.Width = 60;
            this.dgvExport.Columns.Add(column);
            */

            this._CsvData = DataLayer.PopulateInvoiceExport(this.dtpFrom.Value, this.dtpTo.Value, this.chkInvoice.Checked, this.chkCreditNotes.Checked, Program.LoggedInCompanyId, this.chkIncludeExported.Checked);
            this.dgvExport.DataSource = this._CsvData.Tables[InvoiceCsvDataSet.TableInvoiceExport];
            this.dgvExport.Columns[InvoiceCsvDataSet.InternalIdColumn].Visible = false;
            this.dgvExport.Columns[InvoiceCsvDataSet.MultiMonthColumn].Visible = false;
            this.dgvExport.Columns[InvoiceCsvDataSet.ClientNameColumn].Visible = false;
            this.UpdateTotal();
            this.dgvExport.CellValueChanged += new DataGridViewCellEventHandler(OnCheckPrint);
        }

        private void OnSelectDeselect(object sender, EventArgs e)
        {
            if (string.Compare(this.chkPrint.Text, "Select All", false) == 0)
            {
                foreach (DataRow rowItem in this._CsvData.Tables[InvoiceCsvDataSet.TableInvoiceExport].Rows)
                {
                    rowItem[InvoiceCsvDataSet.PrintColumn] = true;
                }
                this.chkPrint.Text = "Clear All";
            }
            else
            {
                foreach (DataRow rowItem in this._CsvData.Tables[InvoiceCsvDataSet.TableInvoiceExport].Rows)
                {
                    rowItem[InvoiceCsvDataSet.PrintColumn] = false;
                }
                this.chkPrint.Text = "Select All";
            }
        }

        private void OnCheckPrint(object sender, DataGridViewCellEventArgs e)
        {
            DataRow[] selectedRow = null;
            string filterCondition = string.Format(CultureInfo.CurrentCulture, "{0}='{1}'", InvoiceCsvDataSet.TransactionNumberColumn, this.dgvExport.Rows[e.RowIndex].Cells[InvoiceCsvDataSet.TransactionNumberColumn].Value.ToString());
            if (string.Compare(this.dgvExport.Columns[e.ColumnIndex].Name, InvoiceCsvDataSet.PrintColumn, false) == 0)
            {
                selectedRow = this._CsvData.Tables[InvoiceCsvDataSet.TableInvoiceExport].Select(filterCondition);
                if (selectedRow.Length > 0)
                {
                    selectedRow[0][InvoiceCsvDataSet.PrintColumn] = !(Convert.ToBoolean(selectedRow[0][InvoiceCsvDataSet.PrintColumn]));
                }
            }
        }

        private void UpdateTotal()
        {
            decimal totalAmount = 0;
            foreach (DataRow row in this._CsvData.Tables[InvoiceCsvDataSet.TableInvoiceExport].Rows)
            {
                if (string.Compare(Convert.ToString(row[InvoiceCsvDataSet.TypeColumn]), "SI",false)==0)
                {
                    totalAmount += Convert.ToDecimal(row[InvoiceCsvDataSet.InvoiceAmountColumn]);
                }
                else if (string.Compare(Convert.ToString(row[InvoiceCsvDataSet.TypeColumn]), "SC", false) == 0)
                {
                    totalAmount -= Convert.ToDecimal(row[InvoiceCsvDataSet.InvoiceAmountColumn]);
                }
            }
            this.txtTotal.Text = totalAmount.ToString("N2");
        }

        #endregion
    }
}
