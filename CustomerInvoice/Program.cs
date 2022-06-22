using CustomerInvoice.Common;
using CustomerInvoice.Data;
using CustomerInvoice.Data.DataSets;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomerInvoice
{
    static class Program
    {
        public static string LoggedUser = string.Empty;
        public static int LoggedInCompanyId = 0;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }

        public static void GenerateXeroCsvReport(DataGridView inputGrid, string reportTitle, bool markExported)
        {
            SalesInvoiceDataSet dataSet = new SalesInvoiceDataSet();
            DateTime currentTime = DateTime.Now;
            string fileName = $"{reportTitle}-{currentTime.Year}{currentTime.Month.ToString().PadLeft(2, '0')}" +
                $"{currentTime.Day.ToString().PadLeft(2, '0')}{currentTime.Hour.ToString().PadLeft(2, '0')}" +
                $"{currentTime.Minute.ToString().PadLeft(2, '0')}{currentTime.Second.ToString().PadLeft(2, '0')}" +
                $".{currentTime.Millisecond.ToString().PadLeft(3, '0')}.csv";

            try
            {
                var settings = DataLayer.PopulateGlobalSettings(LoggedInCompanyId);
                string headerString = string.Empty;
                string rowString = string.Empty;
                using (var stream = new FileStream(Path.Combine(settings.InvoiceExportPath, fileName), FileMode.Append, FileAccess.Write))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        foreach(DataColumn column in dataSet.Tables[0].Columns)
                        {
                            headerString = string.Concat(headerString, ",", column.AllowDBNull? "": "*", column.ColumnName);
                        }
                        writer.WriteLine(headerString.Substring(1));

                        for(int row = 0; row < inputGrid.Rows.Count; row++)
                        {
                            rowString = string.Empty;
                            bool selected = Convert.ToBoolean(inputGrid.Rows[row].Cells[InvoiceCsvDataSet.PrintColumn].Value, CultureInfo.CurrentCulture);
                            if (selected)
                            {
                                rowString = string.Concat(rowString, inputGrid.Rows[row].Cells[InvoiceCsvDataSet.SageReferenceColumn].Value.ToString());
                                rowString = string.Concat(rowString, ",,,,,,,,,,", inputGrid.Rows[row].Cells[InvoiceCsvDataSet.TransactionNumberColumn].Value.ToString());
                                rowString = string.Concat(rowString, ",", Convert.ToString(inputGrid.Rows[row].Cells[InvoiceCsvDataSet.ClientNameColumn].Value));
                                rowString = string.Concat(rowString, ",", Convert.ToDateTime(inputGrid.Rows[row].Cells[InvoiceCsvDataSet.TransactionDateColumn].Value).ToString("dd-MMM-yy"));
                                rowString = string.Concat(rowString, ",", Convert.ToDateTime(inputGrid.Rows[row].Cells[InvoiceCsvDataSet.TransactionDateColumn].Value).AddDays(5).ToString("dd-MM-yyyy"));
                                rowString = string.Concat(rowString, ",,,", inputGrid.Rows[row].Cells[InvoiceCsvDataSet.NarrationColumn].Value.ToString());
                                rowString = string.Concat(rowString, ",1");
                                rowString = string.Concat(rowString, ",", Convert.ToDecimal(inputGrid.Rows[row].Cells[InvoiceCsvDataSet.InvoiceAmountColumn].Value).ToString("N2").Replace(",", ""));
                                rowString = string.Concat(rowString, ",,", ConfigurationManager.AppSettings["AccountCode"]);
                                rowString = string.Concat(rowString, ",", ConfigurationManager.AppSettings["TaxType"]);
                                rowString = string.Concat(rowString, ",,", ConfigurationManager.AppSettings["TrackingName1"]);
                                rowString = string.Concat(rowString, ",", ConfigurationManager.AppSettings["TrackingOption1"], ",,,,");
                                writer.WriteLine(rowString);

                                string invoiceType = Convert.ToString(inputGrid.Rows[row].Cells[InvoiceCsvDataSet.TypeColumn].Value);
                                int invoiceId = Convert.ToInt32(inputGrid.Rows[row].Cells[InvoiceCsvDataSet.InternalIdColumn].Value);

                                if (markExported)
                                {
                                    if (invoiceType == "SI")
                                    {
                                        DataLayer.UpdateInvoicePrinted(invoiceId);
                                    }
                                    else
                                    {
                                        DataLayer.UpdateCreditNotePrinted(invoiceId);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }
        }

        public static void GenerateCsvForAmalgamatedInvoice(List<int> invoiceIdList, string reportTitle, bool markExported)
        {
            SalesInvoiceDataSet dataSet = new SalesInvoiceDataSet();
            DateTime currentTime = DateTime.Now;
            string fileName = $"{reportTitle}-{currentTime.Year}{currentTime.Month.ToString().PadLeft(2, '0')}" +
                $"{currentTime.Day.ToString().PadLeft(2, '0')}{currentTime.Hour.ToString().PadLeft(2, '0')}" +
                $"{currentTime.Minute.ToString().PadLeft(2, '0')}{currentTime.Second.ToString().PadLeft(2, '0')}" +
                $".{currentTime.Millisecond.ToString().PadLeft(3, '0')}.csv";

            try
            {
                var settings = DataLayer.PopulateGlobalSettings(LoggedInCompanyId);
                string headerString = string.Empty;
                string rowString = string.Empty;
                var invoiceList = DataLayer.GetInvoiceExportModels(LoggedInCompanyId, invoiceIdList);
                using (var stream = new FileStream(Path.Combine(settings.InvoiceExportPath, fileName), FileMode.Append, FileAccess.Write))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        foreach (DataColumn column in dataSet.Tables[0].Columns)
                        {
                            headerString = string.Concat(headerString, ",", column.AllowDBNull ? "" : "*", column.ColumnName);
                        }
                        writer.WriteLine(headerString.Substring(1));

                        foreach(var invoiceItem in invoiceList)
                        {
                            rowString = string.Empty;
                            
                            rowString = string.Concat(rowString, invoiceItem.ContactName);
                            rowString = string.Concat(rowString, ",,,,,,,,,,", invoiceItem.InvoiceNumber);
                            rowString = string.Concat(rowString, ",", invoiceItem.Reference);
                            rowString = string.Concat(rowString, ",", invoiceItem.InvoiceDate.ToString("dd-MM-yyyy"));
                            rowString = string.Concat(rowString, ",", invoiceItem.InvoiceDate.AddDays(5).ToString("dd-MM-yyyy"));
                            rowString = string.Concat(rowString, ",,,", invoiceItem.Description);
                            rowString = string.Concat(rowString, ",1");
                            rowString = string.Concat(rowString, ",", invoiceItem.UnitAmount.ToString("N2").Replace(",", ""));
                            rowString = string.Concat(rowString, ",,", ConfigurationManager.AppSettings["AccountCode"]);
                            rowString = string.Concat(rowString, ",", ConfigurationManager.AppSettings["TaxType"]);
                            rowString = string.Concat(rowString, ",,", ConfigurationManager.AppSettings["TrackingName1"]);
                            rowString = string.Concat(rowString, ",", ConfigurationManager.AppSettings["TrackingOption1"], ",,,,");
                            writer.WriteLine(rowString);

                            int invoiceId = invoiceItem.Id;

                            if (markExported)
                            {
                                DataLayer.UpdateInvoicePrinted(invoiceId);                                
                            }                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }
        }

        public static void GenerateExcelReport(DataGridView inputGrid, string reportTitle, string sumColumn, bool checkForDeleted = false)
        {
            DateTime currentTime = DateTime.Now;
            int row = 1;
            decimal breakdownValue = 0;
            int column = 1;
            decimal totalValue = 0;
            int totalColumnPosition = 0;
            string fileName = $"{reportTitle}-{currentTime.Year}{currentTime.Month.ToString().PadLeft(2, '0')}" +
                $"{currentTime.Day.ToString().PadLeft(2, '0')}{currentTime.Hour.ToString().PadLeft(2, '0')}" +
                $"{currentTime.Minute.ToString().PadLeft(2, '0')}{currentTime.Second.ToString().PadLeft(2, '0')}" +
                $".{currentTime.Millisecond.ToString().PadLeft(3, '0')}.xlsx";
            try
            {
                var settings = DataLayer.PopulateGlobalSettings(LoggedInCompanyId);
                List<int> selectedClients = new List<int>();
                foreach(DataGridViewRow rowItem in inputGrid.Rows)
                {
                    selectedClients.Add(Convert.ToInt32(rowItem.Cells[ClientDataSet.IdColumn].Value));
                }
                var activeCustomers = DataLayer.GetAllActiveCustomersInBreakdowns(LoggedInCompanyId, selectedClients);
                FileInfo file = new FileInfo(Path.Combine(settings.InvoiceExportPath, fileName));
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    var sheet = package.Workbook.Worksheets.Add(reportTitle);
                    for(int loop = 0; loop < inputGrid.Columns.Count; loop++)
                    {
                        if (inputGrid.Columns[loop].Visible)
                        {
                            sheet.Cells[row, column].Value = inputGrid.Columns[loop].HeaderText;
                            column += 1;

                            // Adding the customers
                            if(inputGrid.Columns[loop].HeaderText == "Total Rate")
                            {
                                sheet.Column(column).Style.Numberformat.Format = "0.00";
                                if(activeCustomers!=null && activeCustomers.Count > 0)
                                {
                                    for(int customerLoop = 0; customerLoop < activeCustomers.Count; customerLoop++)
                                    {
                                        sheet.Cells[row, column].Value = $"{activeCustomers[customerLoop].CustomerCode} ({activeCustomers[customerLoop].ChargeHeadName})";
                                        sheet.Column(column).Style.Numberformat.Format = "0.00";                                        
                                        column += 1;
                                    }
                                }
                            }
                        }
                    }
                    
                    sheet.Select(new ExcelAddress(1, 1, 1, column));
                    sheet.SelectedRange.Style.Font.Name = "Tahoma";
                    sheet.SelectedRange.Style.Font.Bold = true;
                    sheet.SelectedRange.Style.Font.Size = 8;
                    sheet.SelectedRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    for(int count = 0; count < inputGrid.Rows.Count; count++)
                    {
                        row += 1;
                        column = 1;
                        for (int loop = 0; loop < inputGrid.Columns.Count; loop++)
                        {
                            if (inputGrid.Columns[loop].Visible)
                            {
                                sheet.Cells[row, column].Value = inputGrid.Rows[count].Cells[loop].Value.ToString();

                                if (string.Compare(inputGrid.Columns[loop].HeaderText, sumColumn, true) == 0)
                                {
                                    totalValue += Convert.ToDecimal(inputGrid.Rows[count].Cells[loop].Value);
                                    sheet.Cells[row, column].Style.Numberformat.Format = "#,##0.00";
                                    sheet.Cells[row, column].Value = Convert.ToDecimal(inputGrid.Rows[count].Cells[loop].Value);
                                    totalColumnPosition = column;

                                    if(activeCustomers!=null && activeCustomers.Count > 0)
                                    {
                                        for (int customerLoop = 0; customerLoop < activeCustomers.Count; customerLoop++)
                                        {
                                            breakdownValue = DataLayer.GetBreakdownValueForCustomer(LoggedInCompanyId, Convert.ToInt32(inputGrid.Rows[count].Cells[ClientDataSet.IdColumn].Value), activeCustomers[customerLoop].CustomerId, activeCustomers[customerLoop].ChargeHeadId);
                                            column += 1;
                                            if (breakdownValue > 0)
                                            {
                                                sheet.Cells[row, column].Value = breakdownValue.ToString("N2");
                                            }
                                        }
                                    }
                                }

                                column += 1;
                            }

                            
                        }

                        if (checkForDeleted)
                        {
                            if (inputGrid.Rows[count].Cells[0].Style.BackColor == Color.Yellow)
                            {
                                sheet.Select(new ExcelAddress(row, 1, row, column));
                                sheet.SelectedRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.SelectedRange.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                            }
                        }
                    }

                    row += 1;

                    if (!string.IsNullOrEmpty(sumColumn))
                    {
                        sheet.Cells[row, 1].Value = "Total";
                        sheet.Cells[row, totalColumnPosition].Value = totalValue;

                    }

                    sheet.Select(new ExcelAddress(2, 1, row, column));
                    sheet.SelectedRange.Style.Font.Name = "Tahoma";
                    sheet.SelectedRange.Style.Font.Bold = false;
                    sheet.SelectedRange.Style.Font.Size = 8;

                    package.Save();
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }
        }
    }
}
