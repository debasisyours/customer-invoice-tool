using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CustomerInvoice.Common;

namespace CustomerInvoice.Data.Helpers
{
    public class SettingsHelper:IDisposable
    {
        #region Field declaration

        private string _ConnectionString=string.Empty;

        #endregion

        #region Constructor

        public SettingsHelper(string connectionString)
        {
            this._ConnectionString = connectionString;
        }

        #endregion

        #region Methods

        protected internal GlobalSetting FetchSettings(int companyId)
        {
            GlobalSetting setting = null;
            using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
            {
                setting = context.GlobalSettings.Where(s => s.CompanyId==companyId).SingleOrDefault();
            }
            return setting;
        }

        protected internal bool SaveSetting(GlobalSetting setting)
        {
            GlobalSetting currentValue = null;
            bool newRow = false;
            bool success = false;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    currentValue = context.GlobalSettings.Where(s => s.CompanyId==setting.CompanyId).SingleOrDefault();
                    if (currentValue == null)
                    {
                        currentValue = new GlobalSetting();
                        newRow = true;
                    }
                    else
                    {
                        context.GlobalSettings.DeleteOnSubmit(currentValue);
                        context.SubmitChanges();
                        currentValue = new GlobalSetting();
                        newRow = true;
                    }

                    currentValue.CustomerExportPath = setting.CustomerExportPath;
                    currentValue.InvoiceExportPath = setting.InvoiceExportPath;
                    currentValue.PdfExportPath = setting.PdfExportPath;
                    currentValue.CompanyName = setting.CompanyName;
                    currentValue.CompanyAddress = setting.CompanyAddress;
                    currentValue.AccountName = setting.AccountName;
                    currentValue.AccountNumber = setting.AccountNumber;
                    currentValue.SortCode = setting.SortCode;
                    currentValue.SmtpFromAddress = setting.SmtpFromAddress;
                    currentValue.SmtpPassword = setting.SmtpPassword;
                    currentValue.SmtpUser = setting.SmtpUser;
                    currentValue.CompanyId = setting.CompanyId;

                    if (newRow)
                    {
                        context.GlobalSettings.InsertOnSubmit(currentValue);
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

        protected internal bool SaveLetterContent(int companyId, string letterContent)
        {
            bool result = false;

            try
            {
                using (var context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    var settings = context.GlobalSettings.FirstOrDefault(s => s.CompanyId == companyId);
                    if (settings != null)
                    {
                        settings.LetterContent = letterContent;
                        context.SubmitChanges();
                        result = true;
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLog(ex);
            }

            return result;
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
