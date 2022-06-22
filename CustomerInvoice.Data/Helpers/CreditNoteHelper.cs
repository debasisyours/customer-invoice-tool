using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using CustomerInvoice.Common;
using CustomerInvoice.Data.DataSets;

namespace CustomerInvoice.Data.Helpers
{
    public class CreditNoteHelper:IDisposable
    {
        #region Field declaration

        private string _ConnectionString = string.Empty;

        #endregion

        #region Constructor

        public CreditNoteHelper(string connectionString)
        {
            this._ConnectionString = connectionString;
        }

        #endregion

        #region Methods

        protected internal bool SaveCreditNote(CreditNote note, int companyId)
        {
            bool success = false;
            bool newRecord = false;
            CreditNote currentNote = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    currentNote = context.CreditNotes.Where(s => s.ID == note.ID).SingleOrDefault();
                    if (currentNote == null)
                    {
                        newRecord = true;
                        currentNote = new CreditNote();
                    }
                    currentNote.CompanyId = companyId;
                    currentNote.Amount = note.Amount;
                    currentNote.ClientId = note.ClientId;
                    currentNote.Description = note.Description;
                    currentNote.Narration = note.Description;
                    currentNote.TransactionNumber = note.TransactionNumber;
                    currentNote.TransactionDate = note.TransactionDate;
                    currentNote.CustomerId = note.CustomerId;
                    if (newRecord)
                    {
                        context.CreditNotes.InsertOnSubmit(currentNote);
                    }
                    context.SubmitChanges();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return success;
        }

        protected internal CreditNoteDataSet PopulateCreditNote(int companyId)
        {
            CreditNoteDataSet creditNotes = new CreditNoteDataSet();
            Client tmpClient = null;
            Customer tmpCustomer = null;
            DataRow row = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    List<CreditNote> creditNoteList = context.CreditNotes.Where(s => s.CompanyId==companyId && s.Deleted==false).ToList();
                    foreach (CreditNote noteItem in creditNoteList)
                    {
                        row = creditNotes.Tables[CreditNoteDataSet.TableCreditNote].NewRow();
                        row[CreditNoteDataSet.AmountColumn] = noteItem.Amount;
                        row[CreditNoteDataSet.ClientIdColumn] = noteItem.ClientId;
                        row[CreditNoteDataSet.CustomerIdColumn] = noteItem.CustomerId;
                        tmpClient = context.Clients.Where(s => s.ID == noteItem.ClientId).SingleOrDefault();
                        if (tmpClient != null)
                            row[CreditNoteDataSet.ClientNameColumn] = tmpClient.Name;

                        tmpCustomer = context.Customers.Where(s => s.ID == noteItem.CustomerId).SingleOrDefault();
                        if (tmpCustomer!= null)
                            row[CreditNoteDataSet.CustomerNameColumn] = tmpCustomer.Name;

                        row[CreditNoteDataSet.DescriptionColumn] = noteItem.Description;
                        row[CreditNoteDataSet.IdColumn] = noteItem.ID;
                        row[CreditNoteDataSet.TransactionDateColumn] = noteItem.TransactionDate;
                        row[CreditNoteDataSet.TransactionNumberColumn] = noteItem.TransactionNumber;

                        creditNotes.Tables[CreditNoteDataSet.TableCreditNote].Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return creditNotes;
        }

        protected internal CreditNote GetNoteSingle(int creditNoteId)
        {
            CreditNote note = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    note = context.CreditNotes.Where(s => s.ID == creditNoteId).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return note;
        }

        protected internal string GenerateCreditNoteNumber(int companyId)
        {
            string noteId = string.Empty;
            int creditNoteCount = 0;
            int maxNumber=0;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    creditNoteCount = context.CreditNotes.Where(s => s.CompanyId == companyId).Count();
                    if (creditNoteCount == 0)
                    {
                        noteId = "CR/00001";
                    }
                    else
                    {
                        maxNumber = context.CreditNotes.Where(s =>s.CompanyId==companyId).ToList().Max(s => Convert.ToInt32(s.TransactionNumber.Substring(3)));
                        noteId = string.Format(CultureInfo.CurrentCulture, "CR/{0}", (maxNumber + 1).ToString().PadLeft(5, '0'));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return noteId;
        }

        protected internal CreditNotePrintDataSet PopulateCreditNotePrint(int creditNoteId, int clientId,int companyId)
        {
            CreditNotePrintDataSet noteData = new CreditNotePrintDataSet();
            GlobalSetting setting = DataLayer.PopulateGlobalSettings(companyId);
            Company tmpCompany = null;
            Client tmpClient = DataLayer.GetSingleClient(clientId);
            Customer tmpCustomer = null;
            CreditNote tmpNote = DataLayer.GetSingleCreditNote(creditNoteId);
            DataRow row = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    tmpCompany = context.Companies.Where(s => s.ID == companyId).SingleOrDefault();

                    row = noteData.Tables[CreditNotePrintDataSet.TableCreditNotePrint].NewRow();
                    row[CreditNotePrintDataSet.AccountNameColumn] = setting.AccountName;
                    row[CreditNotePrintDataSet.AccountNumberColumn] = setting.AccountNumber;
                    row[CreditNotePrintDataSet.ClientCodeColumn] = tmpClient.Code;
                    row[CreditNotePrintDataSet.ClientNameColumn] = tmpClient.Name;
                    row[CreditNotePrintDataSet.CompanyAddressColumn] = setting.CompanyAddress;
                    row[CreditNotePrintDataSet.CompanyNameColumn] = setting.CompanyName;
                    row[CreditNotePrintDataSet.CreditNoteDateColumn] = tmpNote.TransactionDate;
                    row[CreditNotePrintDataSet.CreditNoteNumberColumn] = tmpNote.TransactionNumber;
                    row[CreditNotePrintDataSet.DescriptionColumn] = tmpNote.Description;
                    row[CreditNotePrintDataSet.NetAmountColumn] = tmpNote.Amount;
                    row[CreditNotePrintDataSet.SortCodeColumn]=setting.SortCode;

                    if (tmpCompany != null)
                    {
                        row[CreditNotePrintDataSet.CompanyCodeColumn] = tmpCompany.Code;
                    }

                    tmpCustomer = context.Customers.Where(s => s.ID == tmpNote.CustomerId).SingleOrDefault();
                    if (tmpCustomer != null)
                    {
                        row[CreditNotePrintDataSet.CustomerNameColumn] = tmpCustomer.Name;
                        row[CreditNotePrintDataSet.CustomerCodeColumn] = tmpCustomer.Code;
                        row[CreditNotePrintDataSet.CustomerAddressColumn] = tmpCustomer.Address;
                    }

                    noteData.Tables[CreditNotePrintDataSet.TableCreditNotePrint].Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return noteData;
        }

        protected internal bool UpdatedCreditNotePrinted(int noteId)
        {
            bool success = false;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    CreditNote tmpInvoice = context.CreditNotes.Where(s => s.ID == noteId).SingleOrDefault();
                    if (tmpInvoice != null)
                    {
                        tmpInvoice.Printed = true;
                        context.SubmitChanges();
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return success;
        }

        protected internal bool UpdatedCreditNotePrintedByUser(int noteId)
        {
            bool success = false;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    CreditNote tmpInvoice = context.CreditNotes.Where(s => s.ID == noteId).SingleOrDefault();
                    if (tmpInvoice != null)
                    {
                        tmpInvoice.UserPrinted = true;
                        context.SubmitChanges();
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return success;
        }

        protected internal bool DeleteCreditNote(int noteId)
        {
            bool success = false;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    CreditNote tmpNote = context.CreditNotes.Where(s => s.ID == noteId).SingleOrDefault();
                    if (tmpNote != null)
                    {
                        tmpNote.Deleted = true;
                        context.SubmitChanges();
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return success;
        }

        #endregion

        #region Interface implementation

        void IDisposable.Dispose()
        {
            //throw new NotImplementedException();
        }
        
        #endregion
    }
}
