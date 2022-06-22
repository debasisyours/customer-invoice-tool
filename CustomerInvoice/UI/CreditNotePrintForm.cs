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
using Microsoft.Reporting.WinForms;
using System.IO;

namespace CustomerInvoice.UI
{
    public partial class CreditNotePrintForm : Form
    {
        #region Private declaration

        private int _CreditNoteId;
        private int _ClientId;
        private bool _FromHistory = false;

        #endregion

        public CreditNotePrintForm()
        {
            InitializeComponent();
        }

        public CreditNotePrintForm(int creditNoteId,bool fromHistory)
        {
            InitializeComponent();
            this.btnClose.Click += new EventHandler(OnClose);
            this._CreditNoteId = creditNoteId;
            this._ClientId = DataLayer.GetSingleCreditNote(this._CreditNoteId).ClientId;
            this._FromHistory = fromHistory;
            this.GenerateReport();
        }

        #region Custom methods

        private void GenerateReport()
        {
            CreditNotePrintDataSet printData = DataLayer.PopulateCreditNoteData(this._CreditNoteId, this._ClientId,Program.LoggedInCompanyId);
            ReportDataSource source = new ReportDataSource("DataSetReport", printData.Tables[CreditNotePrintDataSet.TableCreditNotePrint]);
            this.viewerReport.LocalReport.ReportPath = "~/../../../Reports/CreditNoteReport.rdlc";
            this.viewerReport.LocalReport.DataSources.Add(source);
            CreditNote tmpNote = DataLayer.GetSingleCreditNote(this._CreditNoteId);
            this.viewerReport.LocalReport.SetParameters(new ReportParameter("ParameterNarration", string.IsNullOrEmpty(tmpNote.Narration)?" ":tmpNote.Narration));
            this.viewerReport.RefreshReport();

            this.ExportInvoice();
        }

        private void ExportInvoice()
        {
            bool mailSent = false;
            string noteNumber = string.Empty;
            try
            {
                CreditNote tmpNote = DataLayer.GetSingleCreditNote(this._CreditNoteId);
                noteNumber = tmpNote.TransactionNumber.Substring(3);
                GlobalSetting setting = DataLayer.PopulateGlobalSettings(Program.LoggedInCompanyId);
                string exportPath = setting.PdfExportPath;
                string smtpFromAddress = setting.SmtpFromAddress;
                string smtpUser = setting.SmtpUser;
                string smtpPassword = setting.SmtpPassword;

                Customer tmpCustomer = DataLayer.GetSingleCustomer(tmpNote.CustomerId);

                string device = string.Empty;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                string[] streams = null;
                Warning[] warnings = null;
                byte[] renderedByte = this.viewerReport.LocalReport.Render("PDF", device, out mimeType, out encoding, out extension, out streams, out warnings);

                switch (tmpCustomer.Code.ToUpper())
                {
                    case "NHS FUNDING":
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
                    case "WALLSSS":
                        {
                            exportPath = Path.Combine(exportPath, "INCOME SECTION");
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
                    using (FileStream stream = new FileStream(Path.Combine(exportPath, "Credit Note " + noteNumber + ".pdf"), FileMode.Create, FileAccess.Write))
                    {
                        stream.Write(renderedByte, 0, renderedByte.Length);
                        stream.Flush();
                    }
                    if (!string.IsNullOrEmpty(tmpCustomer.Email))
                    {
                        if(!this._FromHistory) 
                            mailSent = MailHelper.SendMailOutlook(smtpFromAddress, tmpCustomer.Email, "Ref: Credit Note " + tmpNote.TransactionNumber, Path.Combine(exportPath, "Credit Note " + noteNumber + ".pdf"), smtpUser, smtpPassword);
                    }
                    
                    //if (!tmpCustomer.Exported.Value)
                    //{
                    //    File.Delete(Path.Combine(exportPath, "Credit Note " + noteNumber + ".pdf"));
                    //}
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

        private void OnClose(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion
    }
}
