using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;
using Microsoft.Reporting.WinForms;
using CustomerInvoice.Common;
using CustomerInvoice.Data;
using CustomerInvoice.Data.DataSets;


namespace CustomerInvoice.UI
{
    public partial class PrintForm : Form
    {
        #region Field declaration

        private long _InvoiceId = 0;
        private int _CustomerId = 0;
        private bool _FromHistory = false;
        private bool _MultiMonth = false;
        private bool _amalgamatedInvoice = false;

        #endregion

        public PrintForm()
        {
            InitializeComponent();
        }

        public PrintForm(long invoiceId,int customerId,bool fromHistory, bool printRequired, bool multiMonth, bool amalgamatedInvoice = false)
        {
            InitializeComponent();
            this._InvoiceId = invoiceId;
            this._CustomerId = customerId;
            this._FromHistory = fromHistory;
            this._MultiMonth = multiMonth;
            this._amalgamatedInvoice = amalgamatedInvoice;

            try
            {
                if (!this._MultiMonth)
                {
                    if (this._amalgamatedInvoice)
                    {
                        this.viewerReport.LocalReport.ReportPath = "~/../../../Reports/InvoiceAmalgamatedReport.rdlc";
                        ReportDataSource source = new ReportDataSource("AmalgamatedDataSet",
                            DataLayer.PopulateAmalgamatedInvoiceForPrint(Program.LoggedInCompanyId, Convert.ToInt32(this._InvoiceId)).Tables
                                [0]);
                        this.viewerReport.LocalReport.DataSources.Add(source);
                        source = new ReportDataSource("AmalgamatedDetailDataSet",
                            DataLayer.PopulateAmalgamatedPrintDetail(Program.LoggedInCompanyId, Convert.ToInt32(this._InvoiceId)).Tables[
                                AmalgamatedPrintDetailDataSet.TableInvoiceDetail]);
                        this.viewerReport.LocalReport.DataSources.Add(source);
                        Customer tmpCustomer = DataLayer.GetSingleCustomer(customerId);
                        string paramValue = string.Empty;
                        paramValue = tmpCustomer.ShowName == true ? "1" : "0";
                        //Invoice tmpInvoice = DataLayer.GetInvoiceSingle(this._InvoiceId);
                        //this.viewerReport.LocalReport.SetParameters(new ReportParameter("ParameterNarration",
                        //    string.IsNullOrEmpty(tmpInvoice.Narration) ? " " : tmpInvoice.Narration));
                        this.viewerReport.RefreshReport();
                        //this.ExportInvoice(tmpInvoice.InvoiceNumber, printRequired);
                    }
                    else
                    {
                        this.viewerReport.LocalReport.ReportPath = "~/../../../Reports/InvoiceReport.rdlc";
                        ReportDataSource source = new ReportDataSource("DataSetInvoicePrint",
                            DataLayer.PopulatePrintData(this._InvoiceId, this._CustomerId, Program.LoggedInCompanyId).Tables
                                [InvoicePrintDataSet.TableInvoicePrint]);
                        this.viewerReport.LocalReport.DataSources.Add(source);
                        Customer tmpCustomer = DataLayer.GetSingleCustomer(customerId);
                        string paramValue = string.Empty;
                        paramValue = tmpCustomer.ShowName == true ? "1" : "0";
                        Invoice tmpInvoice = DataLayer.GetInvoiceSingle(this._InvoiceId);
                        this.viewerReport.LocalReport.SetParameters(new ReportParameter("ParameterShowName", paramValue));
                        this.viewerReport.LocalReport.SetParameters(new ReportParameter("ParameterNarration",
                            string.IsNullOrEmpty(tmpInvoice.Narration) ? " " : tmpInvoice.Narration));
                        this.viewerReport.RefreshReport();
                        this.ExportInvoice(tmpInvoice.InvoiceNumber, printRequired);
                    }
                }
                else
                {
                    this.viewerReport.LocalReport.ReportPath = "~/../../../Reports/InvoiceReportMultiMonth.rdlc";
                    ReportDataSource source = new ReportDataSource("DataSetInvoicePrint",
                        DataLayer.PopulatePrintData(this._InvoiceId, this._CustomerId, Program.LoggedInCompanyId).Tables
                            [InvoicePrintDataSet.TableInvoicePrint]);
                    this.viewerReport.LocalReport.DataSources.Add(source);
                    source = new ReportDataSource("InvoiceBreakdownDataSet",
                        DataLayer.PopulateInvoiceBreakdown(this._InvoiceId).Tables[
                            InvoiceBreakDownDataSet.TableInvoiceBreakDown]);
                    this.viewerReport.LocalReport.DataSources.Add(source);
                    Customer tmpCustomer = DataLayer.GetSingleCustomer(customerId);
                    string paramValue = string.Empty;
                    paramValue = tmpCustomer.ShowName == true ? "1" : "0";
                    Invoice tmpInvoice = DataLayer.GetInvoiceSingle(this._InvoiceId);
                    this.viewerReport.LocalReport.SetParameters(new ReportParameter("ParameterShowName", paramValue));
                    this.viewerReport.LocalReport.SetParameters(new ReportParameter("ParameterNarration",
                        string.IsNullOrEmpty(tmpInvoice.Narration) ? " " : tmpInvoice.Narration));
                    this.viewerReport.RefreshReport();
                    this.ExportInvoice(tmpInvoice.InvoiceNumber, printRequired);                    
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        #region Custom functions

        private void ExportInvoice(string invoiceNumber,bool printRequired)
        {
            bool mailSent = false;
            try
            {
                GlobalSetting setting = DataLayer.PopulateGlobalSettings(Program.LoggedInCompanyId);
                string exportPath = setting.PdfExportPath;
                string smtpFromAddress = setting.SmtpFromAddress;
                string smtpUser = setting.SmtpUser;
                string smtpPassword = setting.SmtpPassword;
                
                Customer tmpCustomer = DataLayer.GetSingleCustomer(this._CustomerId);

                string device = string.Empty;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                string[] streams = null;
                Warning[] warnings = null;
                byte[] renderedByte = this.viewerReport.LocalReport.Render("PDF", device, out mimeType, out encoding, out extension, out streams, out warnings);

                switch (tmpCustomer.Code.ToUpper())
                {
                    case "NHSPAYAB":
                        {
                            exportPath = Path.Combine(exportPath, "NHS FUNDING");
                            this.CheckAndCreateFolder(exportPath);
                            break;
                        }
                    case "BHAMSS":
                        {
                            exportPath = Path.Combine(exportPath, "BHAMSS");
                            this.CheckAndCreateFolder(exportPath);
                            break;
                        }
                    case "WALSSS":
                        {
                            exportPath = Path.Combine(exportPath, "WALSALL INCOME");
                            this.CheckAndCreateFolder(exportPath);
                            break;
                        }
                    case "SANDWELL":
                        {
                            exportPath = Path.Combine(exportPath, "SANDWELL");
                            this.CheckAndCreateFolder(exportPath);
                            break;
                        }
                    default:
                        {
                            if (tmpCustomer.IsFamily.HasValue && tmpCustomer.IsFamily.Value)
                            {
                                exportPath = Path.Combine(exportPath, "FAMILY");
                                this.CheckAndCreateFolder(exportPath);
                            }
                            break;
                        }
                }

                if (Directory.Exists(exportPath))
                {
                    using (FileStream stream = new FileStream(Path.Combine(exportPath, "Invoice " + invoiceNumber + ".pdf"), FileMode.Create, FileAccess.Write))
                    {
                        stream.Write(renderedByte, 0, renderedByte.Length);
                        stream.Flush();
                    }
                    if (!string.IsNullOrEmpty(tmpCustomer.Email))
                    {
                        if(!this._FromHistory)
                            mailSent = MailHelper.SendMailOutlook(smtpFromAddress, tmpCustomer.Email, "Ref: Invoice " + invoiceNumber, Path.Combine(exportPath, "Invoice " + invoiceNumber + ".pdf"), smtpUser, smtpPassword);
                    }
                    if (tmpCustomer.PhysicalPrintRequired && printRequired)
                    {
                        //File.Delete(Path.Combine(exportPath, "Invoice " + invoiceNumber + ".pdf"));
                        ProcessStartInfo info = new ProcessStartInfo();
                        info.Verb = "print";
                        info.FileName = Path.Combine(exportPath, "Invoice " + invoiceNumber + ".pdf");
                        info.CreateNoWindow = true;
                        info.WindowStyle = ProcessWindowStyle.Hidden;

                        Process process = new Process();
                        process.StartInfo = info;
                        process.Start();
                        process.WaitForInputIdle();
                        Thread.Sleep(3000);
                        if (false == process.CloseMainWindow())
                        {
                            process.Kill();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        private void CheckAndCreateFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        #endregion
    }
}
