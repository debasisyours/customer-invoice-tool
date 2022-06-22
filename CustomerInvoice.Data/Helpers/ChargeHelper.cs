using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CustomerInvoice.Common;
using CustomerInvoice.Data.DataSets;

namespace CustomerInvoice.Data.Helpers
{
    public class ChargeHelper:IDisposable
    {
        #region Field declaration

        private string _ConnectionString = string.Empty;

        #endregion

        #region Constructor

        public ChargeHelper(string connectionString)
        {
            this._ConnectionString = connectionString;
        }

        #endregion

        #region Methods

        protected internal ChargeDataSet PopulateCharges(int companyId)
        {
            ChargeDataSet charges = new ChargeDataSet();
            DataRow row = null;
            using(InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
            {
                List<ChargeHead> chargeList = context.ChargeHeads.Where(s => s.CompanyId==companyId).ToList();
                if (chargeList != null && chargeList.Count > 0)
                {
                    foreach(ChargeHead chargeEntry in chargeList)
                    {
                        row = charges.Tables[ChargeDataSet.TableChargeHead].NewRow();
                        row[ChargeDataSet.IdColumn] = Convert.ToInt32(chargeEntry.ID);
                        row[ChargeDataSet.NameColumn] = Convert.ToString(chargeEntry.Name);
                        charges.Tables[ChargeDataSet.TableChargeHead].Rows.Add(row);
                    }
                }
            }
            return charges;
        }

        protected internal ChargeHead GetSingle(int chargeId)
        {
            ChargeHead chargeEntry = null;
            using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
            {
                chargeEntry = context.ChargeHeads.Where(s => s.ID == chargeId).SingleOrDefault();
            }
            return chargeEntry;
        }

        protected internal bool SaveChargeHead(ChargeHead charge, int companyId)
        {
            bool success = false;
            bool newRow = false;
            ChargeHead currentValue = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    currentValue = context.ChargeHeads.Where(s => s.ID == charge.ID).SingleOrDefault();
                    if (currentValue == null)
                    {
                        currentValue = new ChargeHead();
                        newRow = true;
                    }
                    currentValue.Name = charge.Name;
                    currentValue.CompanyId = companyId;
                    if (newRow)
                    {
                        context.ChargeHeads.InsertOnSubmit(currentValue);
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

        protected internal bool DeleteCharge(int chargeId)
        {
            bool success = false;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    ChargeHead tmpCharge = context.ChargeHeads.Where(s => s.ID == chargeId).SingleOrDefault();
                    if (tmpCharge != null)
                    {
                        context.ChargeHeads.DeleteOnSubmit(tmpCharge);
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

        protected internal int GetChargeId(string chargeName)
        {
            int chargeId = 0;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    ChargeHead charge = context.ChargeHeads.Where(s => string.Compare(s.Name, chargeName, false) == 0).SingleOrDefault();
                    if (charge != null)
                    {
                        chargeId = charge.ID;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return chargeId;
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
