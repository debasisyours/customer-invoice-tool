using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using CustomerInvoice.Common;
using System.Data.Linq;
using System.Globalization;
using CustomerInvoice.Data.DataSets;

namespace CustomerInvoice.Data.Helpers
{
    public class ClientHelper:IDisposable
    {
        #region Field declaration

        private string _ConnectionString = string.Empty;

        #endregion

        #region Constructor

        public ClientHelper(string connectionString)
        {
            this._ConnectionString = connectionString;
        }

        #endregion

        #region Methods

        protected internal ClientDataSet PopulateClientData(int companyId, bool showRip, bool fromClientSearch)
        {
            ClientDataSet clients = new ClientDataSet();
            BreakDown tmpBreakDown = null;
            List<BreakDownDetail> detailList = null;
            Customer tmpCustomer=null;
            DataRow row = null;
            string customerCode = string.Empty;
            string customerEmail = string.Empty;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    var clientQuery = context.Clients.Where(s => s.CompanyId==companyId && !s.IsDeleted);
                    List<Client> clientList = null;
                    if (fromClientSearch)
                    {
                        clientList = clientQuery.Where(s => (!s.Nursing.HasValue || s.Nursing.Value == false)
                                        && (!s.SelfFunding.HasValue || s.SelfFunding.Value == false)
                                        && (!s.Residential.HasValue || s.Residential.Value == false)).ToList();
                    }
                    else
                    {
                        clientList = clientQuery.ToList();
                    }

                    if (clientList != null && clientList.Count > 0)
                    {
                        foreach (Client clientEntry in clientList)
                        {
                            customerCode = string.Empty;
                            customerEmail = string.Empty;
                            row = clients.Tables[ClientDataSet.TableClient].NewRow();
                            row[ClientDataSet.CodeColumn] = clientEntry.Code;
                            if(clientEntry.DateOfAdmission!=null) row[ClientDataSet.DateOfAdmissionColumn] = clientEntry.DateOfAdmission;
                            if (clientEntry.DateOfBirth != null) row[ClientDataSet.DateOfBirthColumn] = clientEntry.DateOfBirth;
                            row[ClientDataSet.IdColumn] = clientEntry.ID;
                            row[ClientDataSet.NameColumn] = clientEntry.Name;
                            row[ClientDataSet.TotalRateColumn] = clientEntry.TotalRate;
                            row[ClientDataSet.SageReferenceColumn] = clientEntry.SageReference;
                            row[ClientDataSet.TheirReferenceColumn] = clientEntry.TheirReference;
                            if (clientEntry.Rip.HasValue) row[ClientDataSet.RipColumn] = clientEntry.Rip.Value;
                            if (clientEntry.Nursing.HasValue) row[ClientDataSet.NursingColumn] = clientEntry.Nursing.Value;
                            if (clientEntry.SelfFunding.HasValue) row[ClientDataSet.SelfFundingColumn] = clientEntry.SelfFunding.Value;
                            if (clientEntry.Residential.HasValue) row[ClientDataSet.ResidentialColumn] = clientEntry.Residential.Value;
                            tmpBreakDown = context.BreakDowns.Where(s => s.ClientID == clientEntry.ID).SingleOrDefault();
                            if (tmpBreakDown != null)
                            {
                                detailList = context.BreakDownDetails.Where(s => s.BreakDownID == tmpBreakDown.ID).ToList();
                                foreach (BreakDownDetail detailItem in detailList)
                                {
                                    tmpCustomer = context.Customers.Where(s => s.ID==detailItem.CustomerID).SingleOrDefault();
                                    customerCode = string.Format(CultureInfo.CurrentCulture, "{0}, {1}", customerCode, tmpCustomer.Code);
                                    if(!string.IsNullOrWhiteSpace(tmpCustomer.Email)) customerEmail = string.Format(CultureInfo.CurrentCulture, "{0}, {1}", customerEmail, tmpCustomer.Email);
                                }
                                row[ClientDataSet.CustomerCodeColumn] = customerCode.Substring(1);
                                row[ClientDataSet.CustomerEmailColumn] = string.IsNullOrWhiteSpace(customerEmail) ? string.Empty : customerEmail.Substring(1);
                            }

                            if(!clientEntry.Rip.HasValue || showRip) clients.Tables[ClientDataSet.TableClient].Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return clients;
        }

        protected internal ClientDataSet GetDeletedClients(int companyId)
        {
            var clients = new ClientDataSet();
            try
            {
                using (var context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    var clientList = context.Clients.Where(s => s.IsDeleted && s.CompanyId == companyId).ToList();
                    if (clientList.Any())
                    {
                        clientList.ForEach(s =>
                        {
                            DataRow row = clients.Tables[ClientDataSet.TableClient].NewRow();
                            row[ClientDataSet.IdColumn] = s.ID;
                            row[ClientDataSet.NameColumn] = s.Name;
                            row[ClientDataSet.SageReferenceColumn] = s.SageReference;
                            row[ClientDataSet.CodeColumn] = s.Code;
                            row[ClientDataSet.DateOfBirthColumn] = s.DateOfBirth;
                            row[ClientDataSet.DateOfAdmissionColumn] = s.DateOfAdmission;
                            row[ClientDataSet.TotalRateColumn] = s.TotalRate;
                            if (s.Rip.HasValue) row[ClientDataSet.RipColumn] = s.Rip.Value;
                            if (s.Nursing.HasValue) row[ClientDataSet.NursingColumn] = s.Nursing.Value;
                            if (s.SelfFunding.HasValue) row[ClientDataSet.SelfFundingColumn] = s.SelfFunding.Value;
                            if (s.Residential.HasValue) row[ClientDataSet.ResidentialColumn] = s.Residential.Value;

                            clients.Tables[ClientDataSet.TableClient].Rows.Add(row);
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }
            return clients;
        }

        protected internal ClientDataSet GetClientList(int companyId, string clientName)
        {
            var clientData = new ClientDataSet();
            using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
            {
                var clients = context.Clients.Where(s => s.CompanyId == companyId && !s.IsDeleted && s.Name.Contains(clientName)).ToList();
                if(clients!=null && clients.Any())
                {
                    foreach(var client in clients)
                    {
                        DataRow dataRow = clientData.Tables[ClientDataSet.TableClient].NewRow();
                        dataRow[ClientDataSet.CodeColumn] = client.Code;
                        dataRow[ClientDataSet.NameColumn] = client.Name;
                        dataRow[ClientDataSet.IdColumn] = client.ID;
                        clientData.Tables[ClientDataSet.TableClient].Rows.Add(dataRow);
                    }
                }
            }
            return clientData;
        }

        protected internal Client GetSingle(int clientId)
        {
            Client client = null;
            using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
            {
                client = context.Clients.Where(s => s.ID == clientId).SingleOrDefault();
            }
            return client;
        }

        protected internal Client GetPreviousClient(int clientId)
        {
            Client client = null;
            using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
            {
                List<Client> clientList = context.Clients.OrderByDescending(s => s.ID).Where(s => s.ID < clientId && s.IsDeleted==false).ToList();
                if (clientList.Count > 0)
                {
                    client = clientList.FirstOrDefault();
                }
            }
            return client;
        }

        protected internal Client GetNextClient(int clientId)
        {
            Client client = null;
            using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
            {
                List<Client> clientList = context.Clients.OrderBy(s => s.ID).Where(s => s.ID > clientId && s.IsDeleted==false).ToList();
                if (clientList.Count > 0)
                {
                    client = clientList.FirstOrDefault();
                }
            }
            return client;
        }

        protected internal Client GetSingleFromName(string name,int companyId)
        {
            Client client = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    client = context.Clients.Where(s => (string.Compare(s.Name, name, false) == 0) && s.CompanyId==companyId && !s.IsDeleted).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return client;
        }

        protected internal bool SaveClient(Client client, int companyId)
        {
            bool success = false;
            bool newRecord = false;
            Client currentRecord = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    currentRecord = context.Clients.Where(s => s.ID == client.ID).SingleOrDefault();
                    if (currentRecord == null)
                    {
                        currentRecord = new Client();
                        newRecord = true;
                    }
                    currentRecord.CompanyId = companyId;
                    currentRecord.Code = client.Code;
                    currentRecord.Name = client.Name;
                    currentRecord.DateOfAdmission = client.DateOfAdmission;
                    currentRecord.DateOfBirth = client.DateOfBirth;
                    currentRecord.TotalRate = client.TotalRate;
                    currentRecord.SageReference = client.SageReference;
                    currentRecord.TheirReference = client.TheirReference;
                    currentRecord.Narrative = client.Narrative;
                    currentRecord.IsDeleted = client.IsDeleted;
                    currentRecord.Rip = client.Rip;
                    currentRecord.Nursing = client.Nursing;
                    currentRecord.SelfFunding= client.SelfFunding;
                    currentRecord.Residential = client.Residential;

                    if (newRecord)
                    {
                        context.Clients.InsertOnSubmit(currentRecord);
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

        protected internal List<BreakDown> GetBreakDownForClient(int clientId)
        {
            List<BreakDown> breakdownList = new List<BreakDown>();
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    breakdownList = context.BreakDowns.Where(s => s.ClientID == clientId).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return breakdownList;
        }

        protected internal List<Customer> GetCustomerForBreakdown(int breakdownId)
        {
            List<Customer> customerList = new List<Customer>();
            Customer tmpCustomer = null;
            List<BreakDownDetail> details=null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    BreakDown breakdown = context.BreakDowns.Where(s => s.ID == breakdownId).SingleOrDefault();
                    if (breakdown != null)
                    {
                        details = context.BreakDownDetails.Where(s => s.BreakDownID == breakdown.ID).ToList();
                        foreach (BreakDownDetail detailEntry in details)
                        {
                            tmpCustomer = context.Customers.Where(s => s.ID == detailEntry.CustomerID).SingleOrDefault();
                            if (tmpCustomer != null) customerList.Add(tmpCustomer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return customerList;
        }

        protected internal BreakdownDetailDataSet GetBreakdownForClient(int clientId)
        {
            BreakdownDetailDataSet detail = new BreakdownDetailDataSet();
            List<BreakDownDetail> detailList = null;
            Customer tmpCustomer = null;
            ChargeHead tmpCharge = null;
            BreakDown breakdown = null;
            DataRow row = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    breakdown = context.BreakDowns.Where(s => s.ClientID == clientId).SingleOrDefault();
                    if (breakdown != null)
                    {
                        detailList = context.BreakDownDetails.Where(s => s.BreakDownID == breakdown.ID).ToList();
                        if (detailList != null && detailList.Count > 0)
                        {
                            foreach (BreakDownDetail detailEntry in detailList)
                            {
                                row = detail.Tables[BreakdownDetailDataSet.TableBreakdownDetail].NewRow();
                                row[BreakdownDetailDataSet.ChargeHeadIdColumn] = detailEntry.ChargeHeadID;
                                row[BreakdownDetailDataSet.CustomerIdColumn] = detailEntry.CustomerID;
                                row[BreakdownDetailDataSet.RateColumn] = detailEntry.Rate;
                                row[BreakdownDetailDataSet.InvoiceCycleColumn] = detailEntry.InvoiceCycle;
                                tmpCustomer = context.Customers.Where(s => s.ID == detailEntry.CustomerID).SingleOrDefault();
                                tmpCharge = context.ChargeHeads.Where(s => s.ID == detailEntry.ChargeHeadID).SingleOrDefault();
                                if (tmpCustomer != null)
                                {
                                    row[BreakdownDetailDataSet.CustomerNameColumn] = tmpCustomer.Name;
                                    row[BreakdownDetailDataSet.CustomerCodeColumn] = tmpCustomer.Code;
                                }
                                if (tmpCharge != null)
                                {
                                    row[BreakdownDetailDataSet.ChargeHeadNameColumn] = tmpCharge.Name;
                                }
                                detail.Tables[BreakdownDetailDataSet.TableBreakdownDetail].Rows.Add(row);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return detail;
        }

        protected internal bool SaveBreakdownForClient(int clientId, BreakdownDetailDataSet breakdown, int companyId)
        {
            bool success = false;
            decimal totalRate = 0;
            BreakDown tmpBreakdown = null;
            List<BreakDownDetail> detail = new List<BreakDownDetail>();
            BreakDownDetail detailEntry = null;
            
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    tmpBreakdown = context.BreakDowns.Where(s => s.ClientID == clientId).SingleOrDefault();
                    if (tmpBreakdown == null)
                    {
                        tmpBreakdown = new BreakDown();
                        tmpBreakdown.ClientID = clientId;
                        tmpBreakdown.CompanyId = companyId;
                        tmpBreakdown.DateCreated = DateTime.Now;

                        foreach (DataRow rowItem in breakdown.Tables[BreakdownDetailDataSet.TableBreakdownDetail].Rows)
                        {
                            detailEntry = new BreakDownDetail();
                            detailEntry.ChargeHeadID = Convert.ToInt32(rowItem[BreakdownDetailDataSet.ChargeHeadIdColumn]);
                            detailEntry.CustomerID = Convert.ToInt32(rowItem[BreakdownDetailDataSet.CustomerIdColumn]);
                            detailEntry.Rate = Convert.ToDecimal(rowItem[BreakdownDetailDataSet.RateColumn]);
                            detailEntry.InvoiceCycle = Convert.ToInt16(rowItem[BreakdownDetailDataSet.InvoiceCycleColumn]);
                            detailEntry.IsActive = true;
                            totalRate += Convert.ToDecimal(rowItem[BreakdownDetailDataSet.RateColumn]);
                            tmpBreakdown.BreakDownDetails.Add(detailEntry);
                        }
                        context.BreakDowns.InsertOnSubmit(tmpBreakdown);
                    }
                    else
                    {
                        List<BreakDownDetail> oldList = context.BreakDownDetails.Where(s => s.BreakDownID == tmpBreakdown.ID).ToList();
                        foreach (BreakDownDetail oldEntry in oldList)
                        {
                            context.BreakDownDetails.DeleteOnSubmit(oldEntry);
                        }
                        context.SubmitChanges();

                        foreach (DataRow rowItem in breakdown.Tables[BreakdownDetailDataSet.TableBreakdownDetail].Rows)
                        {
                            detailEntry = new BreakDownDetail();
                            detailEntry.ChargeHeadID = Convert.ToInt32(rowItem[BreakdownDetailDataSet.ChargeHeadIdColumn]);
                            detailEntry.CustomerID = Convert.ToInt32(rowItem[BreakdownDetailDataSet.CustomerIdColumn]);
                            detailEntry.Rate = Convert.ToDecimal(rowItem[BreakdownDetailDataSet.RateColumn]);
                            detailEntry.InvoiceCycle = Convert.ToInt16(rowItem[BreakdownDetailDataSet.InvoiceCycleColumn]);
                            detailEntry.IsActive = true;
                            totalRate += Convert.ToDecimal(rowItem[BreakdownDetailDataSet.RateColumn]);
                            tmpBreakdown.BreakDownDetails.Add(detailEntry);
                        }
                    }

                    var client = context.Clients.FirstOrDefault(s => s.ID == clientId);
                    if (client != null) client.TotalRate = totalRate;

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

        protected internal BreakDownDataSet PopulateBreakdown(int companyId)
        {
            BreakDownDataSet breakdowns = new BreakDownDataSet();
            List<BreakDownDetail> detailList = null;
            Customer tmpCustomer = null;
            Client tmpClient = null;
            ChargeHead tmpCharge = null;
            DataRow row = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    List<BreakDown> breakdownList = context.BreakDowns.Where(s => s.CompanyId == companyId).ToList();
                    foreach (BreakDown breakdownItem in breakdownList)
                    {
                        tmpClient = context.Clients.Where(s => s.ID == breakdownItem.ClientID && s.Rip == null).SingleOrDefault();
                        detailList = context.BreakDownDetails.Where(s => s.BreakDownID == breakdownItem.ID).ToList();
                        if (tmpClient != null)
                        {
                            foreach (BreakDownDetail detailItem in detailList)
                            {
                                tmpCustomer = context.Customers.Where(s => s.ID == detailItem.CustomerID).SingleOrDefault();
                                tmpCharge = context.ChargeHeads.Where(s => s.ID == detailItem.ChargeHeadID).SingleOrDefault();
                                row = breakdowns.Tables[BreakDownDataSet.TableBreakDown].NewRow();
                                row[BreakDownDataSet.AmountColumn] = detailItem.Rate;
                                row[BreakDownDataSet.ChargeHeadIdColumn] = tmpCharge.ID;
                                row[BreakDownDataSet.ChargeHeadNameColumn] = tmpCharge.Name;
                                row[BreakDownDataSet.ClientCodeColumn] = tmpClient.Code;
                                row[BreakDownDataSet.ClientIdColumn] = tmpClient.ID;
                                row[BreakDownDataSet.ClientNameColumn] = tmpClient.Name;
                                row[BreakDownDataSet.CustomerCodeColumn] = tmpCustomer.Code;
                                row[BreakDownDataSet.CustomerIdColumn] = tmpCustomer.ID;
                                row[BreakDownDataSet.CustomerNameColumn] = tmpCustomer.Name;
                                row[BreakDownDataSet.InvoiceCycleColumn] = detailItem.InvoiceCycle;
                                row[BreakDownDataSet.IsActiveColumn] = detailItem.IsActive;
                                row[BreakDownDataSet.IdColumn] = detailItem.ID;
                                breakdowns.Tables[BreakDownDataSet.TableBreakDown].Rows.Add(row);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return breakdowns;
        }

        protected internal bool DeleteBreakdown(int clientId,int companyId)
        {
            bool success = false;
            BreakDown header = null;
            List<BreakDownDetail> detailList = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    header = context.BreakDowns.Where(s => s.ClientID == clientId && s.CompanyId == companyId).SingleOrDefault();
                    if (header != null)
                    {
                        detailList = context.BreakDownDetails.Where(s => s.BreakDownID == header.ID).ToList();
                        foreach (BreakDownDetail detailItem in detailList)
                        {
                            context.BreakDownDetails.DeleteOnSubmit(detailItem);
                        }
                        context.BreakDowns.DeleteOnSubmit(header);
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

        protected internal bool SaveBreakDownEntry(int clientId, int customerId, int chargeId, decimal amount, int companyId, short invoiceCycle, bool isActive)
        {
            bool success = false;
            BreakDown header = null;
            BreakDownDetail detail = null;
            bool newRecord = false;
            Client client = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    client = context.Clients.FirstOrDefault(s => s.ID == clientId);
                    header = context.BreakDowns.Where(s => s.ClientID == clientId).SingleOrDefault();
                    if (header == null)
                    {
                        header = new BreakDown();
                        header.ClientID = clientId;
                        header.CompanyId = companyId;
                        header.DateCreated = DateTime.Now;

                        detail = new BreakDownDetail();
                        detail.ChargeHeadID = chargeId;
                        detail.CustomerID = customerId;
                        detail.Rate = amount;
                        detail.IsActive = isActive;

                        if (header.BreakDownDetails == null) header.BreakDownDetails = new EntitySet<BreakDownDetail>();

                        header.BreakDownDetails.Add(detail);
                        context.BreakDowns.InsertOnSubmit(header);
                    }
                    else
                    {
                        detail = context.BreakDownDetails.Where(s => s.BreakDownID == header.ID && s.CustomerID == customerId).SingleOrDefault();
                        if (detail == null)
                        {
                            detail = new BreakDownDetail();
                            newRecord = true;
                        }
                        detail.CustomerID = customerId;
                        detail.ChargeHeadID = chargeId;
                        detail.Rate = amount;
                        detail.InvoiceCycle = invoiceCycle;
                        detail.IsActive = isActive;
                        detail.BreakDownID = header.ID;
                        if(newRecord)
                        {
                            context.BreakDownDetails.InsertOnSubmit(detail);
                        }
                    }

                    context.SubmitChanges();

                    var breakdowns = (from tmpBreakDown in context.BreakDowns
                                      join tmpBreakDownDetail in context.BreakDownDetails on tmpBreakDown.ID equals tmpBreakDownDetail.BreakDownID
                                      where tmpBreakDown.ClientID == clientId
                                      select tmpBreakDownDetail).ToList();
                    if(breakdowns!=null && breakdowns.Any())
                    {
                        client.TotalRate = breakdowns.Select(s => s.Rate).Sum();
                        context.SubmitChanges();
                    }
                    
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return success;
        }

        protected internal int CheckBreakDown(int companyId)
        {
            int clientId = 0;
            Client tmpClient = null;
            decimal sumOfCharge = 0;
            List<BreakDown> breakdownList = null;
            List<BreakDownDetail> detailList = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    breakdownList = context.BreakDowns.Where(s => s.CompanyId==companyId).ToList();
                    foreach (BreakDown breakdownItem in breakdownList)
                    {
                        tmpClient = context.Clients.Where(s => s.ID == breakdownItem.ClientID).SingleOrDefault();
                        detailList = context.BreakDownDetails.Where(s => s.BreakDownID == breakdownItem.ID).ToList();
                        sumOfCharge = 0;
                        foreach (BreakDownDetail detailItem in detailList)
                        {
                            sumOfCharge += detailItem.Rate;
                        }
                        if(tmpClient.TotalRate!=sumOfCharge)
                        {
                            clientId = tmpClient.ID;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return clientId;
        }

        protected internal bool DeleteClient(int clientId,int companyId)
        {
            bool success = false;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    Client tmpClient = context.Clients.Where(s => s.ID == clientId).SingleOrDefault();
                    if (tmpClient != null)
                    {
                        tmpClient.IsDeleted = true;
                        context.SubmitChanges();
                    }
                }
                DeleteBreakdown(clientId, companyId);
                success = true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return success;
        }

        protected internal ClientDataSet PopulateDeletedClients(int companyId)
        {
            ClientDataSet clientData = new ClientDataSet();
            DataRow rowItem = null;
            List<Client> clientList = null;
            try
            {
                using(InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    clientList = context.Clients.Where(s => s.IsDeleted==true && s.CompanyId==companyId).ToList();

                    if(clientList!=null && clientList.Count>0)
                    {
                        foreach(Client clientItem in clientList)
                        {
                            rowItem = clientData.Tables[ClientDataSet.TableClient].NewRow();
                            rowItem[ClientDataSet.CodeColumn] = clientItem.Code;
                            rowItem[ClientDataSet.DateOfAdmissionColumn]=clientItem.DateOfAdmission;
                            rowItem[ClientDataSet.DateOfBirthColumn]=clientItem.DateOfBirth;
                            rowItem[ClientDataSet.IdColumn]=clientItem.ID;
                            rowItem[ClientDataSet.NameColumn]=clientItem.Name;
                            rowItem[ClientDataSet.SageReferenceColumn]=clientItem.SageReference;
                            rowItem[ClientDataSet.TheirReferenceColumn]=clientItem.TheirReference;
                            rowItem[ClientDataSet.TotalRateColumn]=clientItem.TotalRate;
                            if (clientItem.Rip.HasValue) rowItem[ClientDataSet.RipColumn] = clientItem.Rip.Value;
                            if (clientItem.Nursing.HasValue) rowItem[ClientDataSet.NursingColumn] = clientItem.Nursing.Value;
                            if (clientItem.SelfFunding.HasValue) rowItem[ClientDataSet.SelfFundingColumn] = clientItem.SelfFunding.Value;
                            if (clientItem.Residential.HasValue) rowItem[ClientDataSet.ResidentialColumn] = clientItem.Residential.Value;
                            clientData.Tables[ClientDataSet.TableClient].Rows.Add(rowItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return clientData;
        }

        protected internal List<Customer> GetCustomersForClient(int companyId, int clientId)
        {
            List<Customer> customers = null;
            try
            {
                using (var context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    customers = (from tmpBreakdown in context.BreakDowns
                                     join tmpDetail in context.BreakDownDetails on tmpBreakdown.ID equals tmpDetail.BreakDownID
                                     join tmpCustomer in context.Customers on tmpDetail.CustomerID equals tmpCustomer.ID
                                     where tmpBreakdown.ClientID == clientId && tmpBreakdown.CompanyId == companyId
                                     select tmpCustomer).ToList();
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return customers;
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
