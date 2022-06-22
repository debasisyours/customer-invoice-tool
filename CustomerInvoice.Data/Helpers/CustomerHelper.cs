using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using CustomerInvoice.Data.DataSets;
using CustomerInvoice.Common;
using System.Globalization;

namespace CustomerInvoice.Data.Helpers
{
    public class CustomerHelper:IDisposable
    {

        #region Private members

        private string _ConnectionString = string.Empty;

        #endregion

        #region Constructor

        public CustomerHelper(string connectionString)
        {
            this._ConnectionString = connectionString;
        }

        #endregion

        #region Protected methods

        protected internal CustomerBreakDownDataSet PopulateBreakDownForCustomer(int companyId, int customerId)
        {
            CustomerBreakDownDataSet breakdowns = new CustomerBreakDownDataSet();
            try
            {
                using (var context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    var clients = (from tmpBreakdown in context.BreakDowns 
                                   join tmpDetail in context.BreakDownDetails 
                                   on tmpBreakdown.ID equals tmpDetail.BreakDownID 
                                   where tmpBreakdown.CompanyId == companyId 
                                   && tmpDetail.CustomerID == customerId select tmpBreakdown.ClientID).ToList().Distinct();

                    foreach(int clientId in clients)
                    {
                        var breakdownItem = (from tmpBreakdown in context.BreakDowns
                                             join tmpDetail in context.BreakDownDetails
                                             on tmpBreakdown.ID equals tmpDetail.BreakDownID
                                             join tmpClient in context.Clients
                                             on tmpBreakdown.ClientID equals tmpClient.ID
                                             join tmpCustomer in context.Customers
                                             on tmpDetail.CustomerID equals tmpCustomer.ID
                                             join tmpCharge in context.ChargeHeads
                                             on tmpDetail.ChargeHeadID equals tmpCharge.ID
                                             where tmpBreakdown.CompanyId == companyId
                                             && tmpClient.CompanyId == companyId
                                             && tmpCustomer.CompanyId == companyId
                                             && tmpCharge.CompanyId == companyId
                                             && tmpBreakdown.ClientID == clientId
                                             select new
                                             {
                                                 clientId = tmpClient.ID,
                                                 clientName = tmpClient.Name,
                                                 customerId = tmpCustomer.ID,
                                                 customerName = tmpCustomer.Name,
                                                 chargeId = tmpCharge.ID,
                                                 chargeName = tmpCharge.Name,
                                                 invoiceCycle = tmpDetail.InvoiceCycle,
                                                 rate = tmpDetail.Rate
                                             }).ToList();
                        foreach (var breakdownEntry in breakdownItem)
                        {
                            DataRow row = breakdowns.Tables[CustomerBreakDownDataSet.TableCustomerBreakdown].NewRow();
                            row[CustomerBreakDownDataSet.ClientIdColumn] = breakdownEntry.clientId;
                            row[CustomerBreakDownDataSet.ClientNameColumn] = breakdownEntry.clientName;
                            row[CustomerBreakDownDataSet.CustomerIdColumn] = breakdownEntry.customerId;
                            row[CustomerBreakDownDataSet.CustomerNameColumn] = breakdownEntry.customerName;
                            row[CustomerBreakDownDataSet.ChargeHeadIdColumn] = breakdownEntry.chargeId;
                            row[CustomerBreakDownDataSet.ChargeHeadNameColumn] = breakdownEntry.chargeName;
                            row[CustomerBreakDownDataSet.InvoiceCycleColumn] = breakdownEntry.invoiceCycle;
                            row[CustomerBreakDownDataSet.RateColumn] = breakdownEntry.rate;
                            breakdowns.Tables[CustomerBreakDownDataSet.TableCustomerBreakdown].Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }
            return breakdowns;
        }

        protected internal CustomerDataSet PopulateCustomerData(int companyId, bool hideRip = false)
        {
            CustomerDataSet customers = new CustomerDataSet();
            string relatedClientCodes = string.Empty;
            List<BreakDownDetail> detailList = null;
            BreakDown breakDown = null;
            string clientCode = string.Empty;
            Client client = null;
            DataRow row = null;
            bool hasActiveClient = false;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    List<Customer> customerList = context.Customers.Where(s => s.CompanyId==companyId && s.IsDeleted==false).ToList();
                    if (customerList!=null && customerList.Count > 0)
                    {
                        foreach (Customer customerEntry in customerList)
                        {
                            row = customers.Tables[CustomerDataSet.TableCustomer].NewRow();
                            hasActiveClient = false;
                            row[CustomerDataSet.IdColumn] = customerEntry.ID;
                            row[CustomerDataSet.CodeColumn] = customerEntry.Code;
                            row[CustomerDataSet.NameColumn] = customerEntry.Name;
                            row[CustomerDataSet.AddressColumn] = customerEntry.Address;
                            row[CustomerDataSet.EmailColumn] = customerEntry.Email;
                            row[CustomerDataSet.PhoneColumn] = customerEntry.Phone;
                            row[CustomerDataSet.SageReferenceColumn] = customerEntry.SageReference;

                            relatedClientCodes = string.Empty;
                            detailList = context.BreakDownDetails.Where(s => s.CustomerID == customerEntry.ID).ToList();
                            if (detailList != null && detailList.Count > 0)
                            {
                                foreach (BreakDownDetail detailItem in detailList)
                                {
                                    breakDown = context.BreakDowns.Where(s => s.ID == detailItem.BreakDownID).SingleOrDefault();
                                    if (breakDown != null)
                                    {
                                        client = (from tmpClient in context.Clients join tmpBreakDown in context.BreakDowns on tmpClient.ID equals tmpBreakDown.ClientID where tmpBreakDown.ID==breakDown.ID select tmpClient).SingleOrDefault();
                                        relatedClientCodes = string.Format(CultureInfo.CurrentCulture, "{0}, {1}", relatedClientCodes, client.Code);
                                        row[CustomerDataSet.ClientCodeColumn] = relatedClientCodes.Substring(1);
                                        if(!hasActiveClient) hasActiveClient = !client.IsDeleted && !client.Rip.HasValue;
                                    }
                                    //row[CustomerDataSet.SelectedColumn] = detailItem.IsActive;
                                    row[CustomerDataSet.SelectedColumn] = false;
                                }
                            }
                            row[CustomerDataSet.HasRipColumn] = !hasActiveClient;
                            if((hideRip && hasActiveClient)|| !hideRip) customers.Tables[CustomerDataSet.TableCustomer].Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return customers;
        }

        protected internal CustomerDataSet PopulateDeletedCustomers(int companyId)
        {
            CustomerDataSet customers = new CustomerDataSet();
            DataRow row = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    List<Customer> customerList = context.Customers.Where(s => s.CompanyId == companyId && s.IsDeleted).ToList();
                    if (customerList != null && customerList.Count > 0)
                    {
                        foreach (Customer customerEntry in customerList)
                        {
                            row = customers.Tables[CustomerDataSet.TableCustomer].NewRow();
                            row[CustomerDataSet.IdColumn] = customerEntry.ID;
                            row[CustomerDataSet.CodeColumn] = customerEntry.Code;
                            row[CustomerDataSet.NameColumn] = customerEntry.Name;
                            row[CustomerDataSet.AddressColumn] = customerEntry.Address;
                            row[CustomerDataSet.EmailColumn] = customerEntry.Email;
                            row[CustomerDataSet.PhoneColumn] = customerEntry.Phone;
                            row[CustomerDataSet.SageReferenceColumn] = customerEntry.SageReference;
                            customers.Tables[CustomerDataSet.TableCustomer].Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return customers;
        }

        protected internal Customer GetSingle(int customerId)
        {
            Customer customer = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    customer = context.Customers.Where(s => s.ID == customerId).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return customer;
        }

        protected internal Customer GetNextCustomer(int customerId)
        {
            Customer customer = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    List<Customer> customerList = context.Customers.OrderBy(s => s.ID).Where(s => s.ID > customerId && s.IsDeleted==false).ToList();
                    if (customerList.Count != 0)
                    {
                        customer = customerList.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return customer;
        }

        protected internal Customer GetPreviousCustomer(int customerId)
        {
            Customer customer = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    List<Customer> customerList = context.Customers.OrderByDescending(s => s.ID).Where(s => s.ID < customerId && s.IsDeleted==false).ToList();
                    if (customerList.Count != 0)
                    {
                        customer = customerList.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return customer;
        }

        protected internal Customer GetSingleFromName(string customerName, int companyId)
        {
            Customer customer = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    customer = context.Customers.Where(s => string.Compare(s.Name, customerName,false)==0 && s.CompanyId==companyId && !s.IsDeleted).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return customer;
        }

        protected internal bool SaveCustomer(Customer customer, int companyId)
        {
            bool success = false;
            bool newRecord = false;
            Customer currentRecord = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    currentRecord = context.Customers.Where(s => s.ID == customer.ID).SingleOrDefault();
                    if (currentRecord == null)
                    {
                        newRecord = true;
                        currentRecord = new Customer();
                    }
                    currentRecord.CompanyId = companyId;
                    currentRecord.Address = customer.Address;
                    currentRecord.Changed = customer.Changed;
                    currentRecord.Code = customer.Code;
                    currentRecord.Email = customer.Email;
                    currentRecord.Phone = customer.Phone;
                    currentRecord.Exported = customer.Exported;
                    currentRecord.IsFamily = customer.IsFamily;
                    currentRecord.Name = customer.Name;
                    currentRecord.SageReference = customer.SageReference;
                    currentRecord.ShowName = customer.ShowName;
                    currentRecord.IsDeleted = false;
                    currentRecord.PhysicalPrintRequired = customer.PhysicalPrintRequired;

                    if (newRecord)
                    {
                        context.Customers.InsertOnSubmit(currentRecord);
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

        protected internal bool DeleteCustomer(int customerId)
        {
            bool success = false;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    Customer tmpCustomer = context.Customers.Where(s => s.ID == customerId).SingleOrDefault();
                    if (tmpCustomer != null)
                    {
                        tmpCustomer.IsDeleted=true;
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

        protected internal bool MarkCustomerActiveInBreakdown(int customerId, bool active)
        {
            bool success = false;

            try
            {
                using (var context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    List<BreakDownDetail> breakdowsDetails =
                        context.BreakDownDetails.Where(s => s.CustomerID == customerId).ToList();

                    foreach (var breakdownItem in breakdowsDetails)
                    {
                        breakdownItem.IsActive = active;
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

        protected internal bool IsCustomerLinkedWithActiveClient(string customerEmail, int companyId)
        {
            bool linkedToActive = false;
            
            try
            {
                using (var context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    var activeClients = (from tmpBreakdown in context.BreakDowns
                                         join tmpDetail in context.BreakDownDetails on tmpBreakdown.ID equals tmpDetail.BreakDownID
                                         join tmpClient in context.Clients on tmpBreakdown.ClientID equals tmpClient.ID
                                         join tmpCustomer in context.Customers on tmpDetail.CustomerID equals tmpCustomer.ID
                                         where tmpBreakdown.CompanyId == companyId && tmpCustomer.Email == customerEmail && !tmpClient.Rip.HasValue
                                         select tmpClient).ToList();
                    linkedToActive = activeClients != null && activeClients.Any();
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLog(ex);
            }

            return linkedToActive;
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
