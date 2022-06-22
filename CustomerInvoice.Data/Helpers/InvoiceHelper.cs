using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using CustomerInvoice.Common;
using CustomerInvoice.Data.DataSets;
using CustomerInvoice.Data.Models;

namespace CustomerInvoice.Data.Helpers
{
    public class InvoiceHelper:IDisposable
    {
        #region Field declaration

        private string _ConnectionString = string.Empty;

        #endregion

        #region Constructor

        public InvoiceHelper(string connectionString)
        {
            this._ConnectionString = connectionString;
        }

        #endregion

        #region Protected methods

        protected internal bool GenerateInvoices(int clientId, DateTime invoiceDate, int days, int companyId, string narration,DateTime startDate, DateTime endDate)
        {
            List<int> customerList = new List<int>();
            List<Invoice> headerExisting = null;
            bool success = false;
            Invoice header = null;
            Invoice existingHeader = null;
            decimal invoiceAmount = 0;
            List<InvoiceDetail> invoiceList = new List<InvoiceDetail>();
            InvoiceDetail invoiceItem = null;
            BreakDown breakdown = null;
            List<BreakDownDetail> detail = null;
            long newInvoice = 0;
            bool headerAssigned = false;
            int multiMonths = 1;

            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    breakdown = context.BreakDowns.Where(s => s.ClientID == clientId).SingleOrDefault();
                    if (breakdown != null)
                    {
                        detail =
                            context.BreakDownDetails.Where(s => s.BreakDownID == breakdown.ID && s.IsActive.Value)
                                .ToList();

                        if (detail == null || detail.Count == 0)
                        {
                            Logger.WriteInformation(string.Format(CultureInfo.CurrentCulture, "Customer allocation not available for client {0}", clientId));
                        }

                        foreach (BreakDownDetail detailEntry in detail)
                        {
                            if (customerList.Count == 0 || (!customerList.Contains(detailEntry.CustomerID)))
                            {
                                customerList.Add(detailEntry.CustomerID);
                                header = new Invoice();
                                header.ClientID = clientId;
                                header.InvoiceDate = invoiceDate;
                                header.CompanyId = companyId;
                                header.Narration = narration;
                                header.StartDate = startDate;
                                if (detailEntry.InvoiceCycle.HasValue && detailEntry.InvoiceCycle.Value > 1)
                                {
                                    header.EndDate = startDate.AddDays(detailEntry.InvoiceCycle.Value*28);
                                    multiMonths = detailEntry.InvoiceCycle.Value;
                                    header.MultiMonth = true;
                                }
                                else
                                {
                                    header.EndDate = endDate;
                                    header.MultiMonth = false;
                                    multiMonths = 1;
                                }

                                header.InvoiceNumber = GenerateInvoiceNumber(companyId);
                                context.Invoices.InsertOnSubmit(header);
                                context.SubmitChanges();

                                newInvoice = header.ID;

                                invoiceItem = new InvoiceDetail();
                                invoiceItem.ChargeHeadID = detailEntry.ChargeHeadID;
                                invoiceItem.CustomerID = detailEntry.CustomerID;
                                invoiceItem.Days = days;
                                invoiceItem.ExtraAmount = 0;
                                invoiceItem.ExtraHead = string.Empty;
                                invoiceItem.InvoiceID = newInvoice;
                                invoiceItem.LessAmount = 0;
                                invoiceItem.LessHead = string.Empty;
                                invoiceItem.WeeklyRate = detailEntry.Rate;
                                invoiceItem.SubTotal = Convert.ToDecimal(detailEntry.Rate) / Convert.ToDecimal("7.0") *
                                                       days * multiMonths;
                                invoiceItem.TotalAmount = Convert.ToDecimal(detailEntry.Rate) / Convert.ToDecimal("7.0") *
                                                          days * multiMonths;
                                invoiceAmount = invoiceItem.TotalAmount;
                                context.InvoiceDetails.InsertOnSubmit(invoiceItem);
                                header.Narration = " ";
                                header.NetAmount = invoiceAmount;
                                context.SubmitChanges();
                            }
                            else
                            {
                                headerExisting =
                                    context.Invoices.Where(
                                        s =>
                                            s.CompanyId == companyId && s.ClientID == clientId &&
                                            s.StartDate == startDate && s.EndDate == endDate).ToList();
                                invoiceItem = new InvoiceDetail();

                                multiMonths = detailEntry.InvoiceCycle.HasValue ? detailEntry.InvoiceCycle.Value : 1;

                                headerAssigned = false;
                                foreach (Invoice existingInvoice in headerExisting)
                                {
                                    List<InvoiceDetail> existingDetailList =
                                        context.InvoiceDetails.Where(s => s.InvoiceID == existingInvoice.ID).ToList();
                                    if (existingDetailList != null && existingDetailList.Count > 0)
                                    {
                                        foreach (InvoiceDetail detailItem in existingDetailList)
                                        {
                                            if (detailItem.CustomerID == detailEntry.CustomerID)
                                            {
                                                invoiceItem.InvoiceID = detailItem.InvoiceID;
                                                headerAssigned = true;
                                                break;
                                            }
                                        }
                                        if (headerAssigned) break;
                                    }
                                }

                                invoiceItem.ChargeHeadID = detailEntry.ChargeHeadID;
                                invoiceItem.CustomerID = detailEntry.CustomerID;
                                invoiceItem.Days = days;
                                invoiceItem.ExtraAmount = 0;
                                invoiceItem.ExtraHead = string.Empty;
                                invoiceItem.LessAmount = 0;
                                invoiceItem.LessHead = string.Empty;
                                invoiceItem.WeeklyRate = detailEntry.Rate;
                                invoiceItem.SubTotal = Convert.ToDecimal(detailEntry.Rate) / Convert.ToDecimal("7.0") *
                                                       days * multiMonths;
                                invoiceItem.TotalAmount = Convert.ToDecimal(detailEntry.Rate) / Convert.ToDecimal("7.0") *
                                                          days * multiMonths;
                                invoiceAmount = invoiceItem.TotalAmount;
                                context.InvoiceDetails.InsertOnSubmit(invoiceItem);

                                existingHeader =
                                    context.Invoices.Where(s => s.ID == invoiceItem.InvoiceID).SingleOrDefault();
                                existingHeader.Narration = " ";

                                existingHeader.NetAmount += invoiceAmount;
                                context.SubmitChanges();
                            }
                        }
                        success = true;
                    }
                    else
                    {
                        Logger.WriteInformation(string.Format(CultureInfo.CurrentCulture, "Breakdown not available for client {0}", clientId));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return success;
        }

        protected internal List<InvoiceExportModel> GetInvoiceExportModels(int companyId, List<int> invoiceIdList)
        {
            var invoiceList = new List<InvoiceExportModel>();

            try
            {
                using (var context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    foreach(var invoiceId in invoiceIdList)
                    {
                        var invoiceHeader = context.AmalgamatedInvoices.FirstOrDefault(s => s.CompanyId==companyId && s.ID == invoiceId);
                        
                        if(invoiceHeader != null)
                        {
                            var customer = context.Customers.FirstOrDefault(s => s.CompanyId == companyId && s.ID == invoiceHeader.CustomerID);
                            var details = (from tmpDetail in context.AmalgamatedInvoiceDetails
                                           join tmpClient in context.Clients on tmpDetail.ClientID equals tmpClient.ID
                                           join tmpCharge in context.ChargeHeads on tmpDetail.ChargeHeadID equals tmpCharge.ID
                                           where tmpClient.CompanyId == companyId
                                           && tmpCharge.CompanyId == companyId
                                           && tmpDetail.AmalgamatedInvoiceID == invoiceHeader.ID
                                           select new
                                           {
                                               clientName = tmpClient.Name,
                                               unitAmount = tmpDetail.TotalAmount
                                           }).ToList();

                            foreach(var detailItem in details)
                            {
                                invoiceList.Add(new InvoiceExportModel
                                {
                                    ClientName = detailItem.clientName,
                                    ContactName = customer.SageReference,
                                    Description = $"{detailItem.clientName} {invoiceHeader.StartDate.ToString("dd/MM/yyyy")} To {invoiceHeader.EndDate.ToString("dd/MM/yyyy")}",
                                    EndDate = invoiceHeader.EndDate,
                                    StartDate = invoiceHeader.StartDate,
                                    Id = invoiceHeader.ID,
                                    InvoiceDate = invoiceHeader.InvoiceDate,
                                    InvoiceNumber = invoiceHeader.InvoiceNumber,
                                    Quantity = 1,
                                    TaxType = "No VAT",
                                    UnitAmount = detailItem.unitAmount,
                                    Reference = $"{customer.Name} {invoiceHeader.StartDate.ToString("dd/MM/yyyy")} To {invoiceHeader.EndDate.ToString("dd/MM/yyyy")}"
                                }) ;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }

            return invoiceList;
        }

        protected internal bool SaveAmalgamatedInvoice(AmalgamatedInvoice invoice, AmalgamatedInvoiceDetailDataSet invoiceDetail)
        {
            bool success = false;

            try
            {
                if (invoice.ID > 0)
                {
                    using (var context = new InvoiceContextDataContext(this._ConnectionString))
                    {
                        var existing = context.AmalgamatedInvoices.FirstOrDefault(s => s.CompanyId == invoice.CompanyId && s.ID == invoice.ID);
                        if (existing != null)
                        {
                            existing.InvoiceNumber = invoice.InvoiceNumber;
                            existing.InvoiceDate = invoice.InvoiceDate;
                            existing.EndDate = invoice.EndDate;
                            existing.StartDate = invoice.StartDate;
                            existing.Narration = invoice.Narration;
                            existing.UserPrinted = false;
                            existing.Deleted = false;
                            existing.MultiMonth = false;
                            existing.CompanyId = invoice.CompanyId;
                            existing.CustomerID = invoice.CustomerID;
                            existing.Printed = false;
                            existing.NetAmount = invoice.NetAmount;
                        }

                        var existingDetails = context.AmalgamatedInvoiceDetails.Where(s => s.AmalgamatedInvoiceID == invoice.ID).ToList();
                        foreach(var detailItem in existingDetails)
                        {
                            DataRow currentRow = invoiceDetail.Tables[AmalgamatedInvoiceDetailDataSet.TableAmalgamatedInvoiceDetail].Select($"{AmalgamatedInvoiceDetailDataSet.IdColumn}={detailItem.ID}")[0];
                            detailItem.ChargeHeadID = Convert.ToInt32(currentRow[AmalgamatedInvoiceDetailDataSet.ChargeHeadIdColumn]);
                            detailItem.LessHead = currentRow[AmalgamatedInvoiceDetailDataSet.LessHeadColumn].ToString();
                            detailItem.LessAmount = Convert.ToDecimal(currentRow[AmalgamatedInvoiceDetailDataSet.LessAmountColumn]);
                            detailItem.WeeklyRate = Convert.ToDecimal(currentRow[AmalgamatedInvoiceDetailDataSet.RateColumn]);
                            detailItem.ClientID = Convert.ToInt32(currentRow[AmalgamatedInvoiceDetailDataSet.ClientIdColumn]);
                            detailItem.Days = Convert.ToInt32(currentRow[AmalgamatedInvoiceDetailDataSet.DaysColumn]);
                            detailItem.ExtraHead = Convert.ToString(currentRow[AmalgamatedInvoiceDetailDataSet.ExtraHeadColumn]);
                            detailItem.ExtraAmount = Convert.ToDecimal(currentRow[AmalgamatedInvoiceDetailDataSet.ExtraAmountColumn]);
                            detailItem.SubTotal = Convert.ToDecimal(currentRow[AmalgamatedInvoiceDetailDataSet.SubTotalColumn]);
                            detailItem.TotalAmount = Convert.ToDecimal(currentRow[AmalgamatedInvoiceDetailDataSet.TotalAmountColumn]);
                        }

                        context.SubmitChanges();
                        success = true;
                    }
                }
                else
                {
                    using (var context = new InvoiceContextDataContext(this._ConnectionString))
                    {
                        invoice.InvoiceNumber = GenerateInvoiceNumber(invoice.CompanyId);
                        context.AmalgamatedInvoices.InsertOnSubmit(invoice);
                        context.SubmitChanges();
                        foreach (DataRow entry in invoiceDetail.Tables[AmalgamatedInvoiceDetailDataSet.TableAmalgamatedInvoiceDetail].Rows)
                        {
                            var detailItem = new AmalgamatedInvoiceDetail
                            {
                                AmalgamatedInvoiceID = invoice.ID,
                                ChargeHeadID = Convert.ToInt32(entry[AmalgamatedInvoiceDetailDataSet.ChargeHeadIdColumn]),
                                ClientID = Convert.ToInt32(entry[AmalgamatedInvoiceDetailDataSet.ClientIdColumn]),
                                Days = Convert.ToInt32(entry[AmalgamatedInvoiceDetailDataSet.DaysColumn]),
                                ExtraHead = Convert.ToString(entry[AmalgamatedInvoiceDetailDataSet.ExtraHeadColumn]),
                                ExtraAmount = Convert.ToDecimal(entry[AmalgamatedInvoiceDetailDataSet.ExtraAmountColumn]),
                                LessHead = Convert.ToString(entry[AmalgamatedInvoiceDetailDataSet.LessHeadColumn]),
                                LessAmount = Convert.ToDecimal(entry[AmalgamatedInvoiceDetailDataSet.LessAmountColumn]),
                                SubTotal = Convert.ToDecimal(entry[AmalgamatedInvoiceDetailDataSet.SubTotalColumn]),
                                WeeklyRate = Convert.ToDecimal(entry[AmalgamatedInvoiceDetailDataSet.RateColumn]),
                                TotalAmount = Convert.ToDecimal(entry[AmalgamatedInvoiceDetailDataSet.TotalAmountColumn])
                            };

                            context.AmalgamatedInvoiceDetails.InsertOnSubmit(detailItem);
                        }

                        context.SubmitChanges();
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }

            return success;
        }

        protected internal bool GenerateAmalgamatedInvoice(int companyId, int customerId, DateTime invoiceDate, string narration, int days, DateTime startDate, DateTime endDate)
        {
            bool success = false;
            decimal netAmount = 0;
            List<AmalgamatedInvoiceDetail> details = new List<AmalgamatedInvoiceDetail>();

            try
            {
                using (var context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    var invoice = new AmalgamatedInvoice
                    {
                        CompanyId = companyId,
                        CustomerID = customerId,
                        Deleted = false,
                        InvoiceDate = invoiceDate,
                        InvoiceNumber = GenerateInvoiceNumber(companyId),
                        EndDate = endDate,
                        MultiMonth = false,
                        Narration = narration,
                        Printed = false,
                        UserPrinted = false,
                        StartDate = startDate,
                        NetAmount = 0
                    };

                    var breakDowns = (from tmpBreak in context.BreakDowns
                                      join tmpDetails in context.BreakDownDetails on tmpBreak.ID equals tmpDetails.BreakDownID
                                      where tmpBreak.CompanyId == companyId && tmpDetails.CustomerID == customerId
                                      select new
                                      {
                                          clientId = tmpBreak.ClientID,
                                          chargeHeadId = tmpDetails.ChargeHeadID,
                                          weeklyRate = tmpDetails.Rate
                                      }).ToList();
                    
                    foreach(var breakDownItem in breakDowns)
                    {

                        details.Add(new AmalgamatedInvoiceDetail
                        {
                            ChargeHeadID = breakDownItem.chargeHeadId,
                            ClientID = breakDownItem.clientId.Value,
                            Days = days,
                            WeeklyRate = breakDownItem.weeklyRate,
                            SubTotal = Convert.ToDecimal(breakDownItem.weeklyRate)*days/Convert.ToDecimal(7.0),
                            TotalAmount = Convert.ToDecimal(breakDownItem.weeklyRate) * days / Convert.ToDecimal(7.0)
                        });
                        netAmount += Convert.ToDecimal(breakDownItem.weeklyRate) * days / Convert.ToDecimal(7.0);
                    }

                    invoice.NetAmount = netAmount;
                    context.AmalgamatedInvoices.InsertOnSubmit(invoice);
                    var invoiceId = invoice.ID;
                    details.ForEach(s =>
                    {
                        s.AmalgamatedInvoiceID = invoiceId;
                    });

                    context.AmalgamatedInvoiceDetails.InsertAllOnSubmit(details);
                    context.SubmitChanges();
                    success = true;
                }                    
            }
            catch(Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }

            return success;
        }

        protected internal AmalgamatedInvoice GetAmalgamatedInvoice(int companyId, int invoiceId)
        {
            AmalgamatedInvoice invoice = null;

            try
            {
                using (var context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    invoice = context.AmalgamatedInvoices.FirstOrDefault(s => s.CompanyId == companyId && s.ID == invoiceId);
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }

            return invoice;
        }

        protected internal AmalgamatedInvoiceDetailDataSet PopulateAmalgamatedInvoiceDetail(int companyId, int invoiceId, int customerId)
        {
            AmalgamatedInvoiceDetailDataSet invoiceDetailDataSet = new AmalgamatedInvoiceDetailDataSet();

            try
            {
                using (var context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    var exists = context.AmalgamatedInvoices.FirstOrDefault(s => s.CompanyId == companyId && s.ID == invoiceId);
                    if (exists != null)
                    {
                        var details = (from tmpDetail in context.AmalgamatedInvoiceDetails 
                                       join tmpClients in context.Clients on tmpDetail.ClientID equals tmpClients.ID
                                       join tmpCharges in context.ChargeHeads on tmpDetail.ChargeHeadID equals tmpCharges.ID
                                       where tmpDetail.AmalgamatedInvoiceID == exists.ID select new
                                       {
                                           Id = tmpDetail.ID,
                                           ChargeHeadId = tmpDetail.ChargeHeadID,
                                           ClientId = tmpDetail.ClientID,
                                           ChargeHeadName = tmpCharges.Name,
                                           ClientName = tmpClients.Name,
                                           LessHead = tmpDetail.LessHead,
                                           LessAmount = tmpDetail.LessAmount,
                                           ExtraHead = tmpDetail.ExtraHead,
                                           ExtraAmount = tmpDetail.ExtraAmount,
                                           SubTotalAmount = tmpDetail.SubTotal,
                                           Rate = tmpDetail.WeeklyRate,
                                           Days = tmpDetail.Days,
                                           TotalAmount =  tmpDetail.TotalAmount
                                       }).ToList();
                        details.ForEach(s =>
                        {
                            DataRow row = invoiceDetailDataSet.Tables[AmalgamatedInvoiceDetailDataSet.TableAmalgamatedInvoiceDetail].NewRow();
                            row[AmalgamatedInvoiceDetailDataSet.IdColumn] = s.Id;
                            row[AmalgamatedInvoiceDetailDataSet.ChargeHeadIdColumn] = s.ChargeHeadId;
                            row[AmalgamatedInvoiceDetailDataSet.ChargeHeadNameColumn] = s.ChargeHeadName;
                            row[AmalgamatedInvoiceDetailDataSet.ClientIdColumn] = s.ClientId;
                            row[AmalgamatedInvoiceDetailDataSet.ClientNameColumn] = s.ClientName;
                            row[AmalgamatedInvoiceDetailDataSet.LessHeadColumn] = s.LessHead;
                            row[AmalgamatedInvoiceDetailDataSet.LessAmountColumn] = s.LessAmount;
                            row[AmalgamatedInvoiceDetailDataSet.ExtraHeadColumn] = s.ExtraHead;
                            row[AmalgamatedInvoiceDetailDataSet.ExtraAmountColumn] = s.ExtraAmount;
                            row[AmalgamatedInvoiceDetailDataSet.SubTotalColumn] = s.SubTotalAmount;
                            row[AmalgamatedInvoiceDetailDataSet.RateColumn] = s.Rate;
                            row[AmalgamatedInvoiceDetailDataSet.DaysColumn] = s.Days;
                            invoiceDetailDataSet.Tables[AmalgamatedInvoiceDetailDataSet.TableAmalgamatedInvoiceDetail].Rows.Add(row);
                        });
                    }
                    else
                    {
                        var details = (from tmpBreakdown in context.BreakDowns
                                       join tmpDetail in context.BreakDownDetails on tmpBreakdown.ID equals tmpDetail.BreakDownID
                                       join tmpCharges in context.ChargeHeads on tmpDetail.ChargeHeadID equals tmpCharges.ID
                                       join tmpClient in context.Clients on tmpBreakdown.ClientID equals tmpClient.ID
                                       where tmpBreakdown.CompanyId == companyId
                                       && tmpCharges.CompanyId == companyId
                                       && tmpClient.CompanyId == companyId
                                       && tmpDetail.CustomerID == customerId
                                       select new
                                       {
                                           Id = 0,
                                           ChargeHeadId = tmpDetail.ChargeHeadID,
                                           ClientId = tmpBreakdown.ClientID,
                                           ChargeHeadName = tmpCharges.Name,
                                           ClientName = tmpClient.Name,
                                           LessHead = string.Empty,
                                           LessAmount = 0,
                                           ExtraHead = string.Empty,
                                           ExtraAmount = 0,
                                           SubTotalAmount = 0,
                                           Rate = tmpDetail.Rate,
                                           Days = 0,
                                           TotalAmount = 0
                                       }).ToList();
                        details.ForEach(s =>
                        {
                            DataRow row = invoiceDetailDataSet.Tables[AmalgamatedInvoiceDetailDataSet.TableAmalgamatedInvoiceDetail].NewRow();
                            row[AmalgamatedInvoiceDetailDataSet.IdColumn] = s.Id;
                            row[AmalgamatedInvoiceDetailDataSet.ChargeHeadIdColumn] = s.ChargeHeadId;
                            row[AmalgamatedInvoiceDetailDataSet.ChargeHeadNameColumn] = s.ChargeHeadName;
                            row[AmalgamatedInvoiceDetailDataSet.ClientIdColumn] = s.ClientId;
                            row[AmalgamatedInvoiceDetailDataSet.ClientNameColumn] = s.ClientName;
                            row[AmalgamatedInvoiceDetailDataSet.LessHeadColumn] = s.LessHead;
                            row[AmalgamatedInvoiceDetailDataSet.LessAmountColumn] = s.LessAmount;
                            row[AmalgamatedInvoiceDetailDataSet.ExtraHeadColumn] = s.ExtraHead;
                            row[AmalgamatedInvoiceDetailDataSet.ExtraAmountColumn] = s.LessAmount;
                            row[AmalgamatedInvoiceDetailDataSet.SubTotalColumn] = s.SubTotalAmount;
                            row[AmalgamatedInvoiceDetailDataSet.RateColumn] = s.Rate;
                            row[AmalgamatedInvoiceDetailDataSet.DaysColumn] = s.Days;
                            invoiceDetailDataSet.Tables[AmalgamatedInvoiceDetailDataSet.TableAmalgamatedInvoiceDetail].Rows.Add(row);
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }
            return invoiceDetailDataSet;
        }

        protected internal AmalgamatedDataSet PopulateAmalgamatedInvoiceForPrint(int companyId, int invoiceId)
        {
            AmalgamatedDataSet invoice = new AmalgamatedDataSet();

            try
            {
                using (var context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    var settingDetail = context.GlobalSettings.FirstOrDefault(s => s.CompanyId == companyId);
                    var companyDetail = context.Companies.FirstOrDefault(s => s.ID == companyId);
                    var invoiceDetails = (from tmpInvoice in context.AmalgamatedInvoices
                                          join tmpCustomer in context.Customers on tmpInvoice.CustomerID equals tmpCustomer.ID
                                          where tmpInvoice.CompanyId == companyId
                                          && tmpCustomer.CompanyId == companyId
                                          select new
                                          {
                                              invoiceNumber = tmpInvoice.InvoiceNumber,
                                              customerName = tmpCustomer.Name,
                                              customerAddress = tmpCustomer.Address,
                                              invoiceDate = tmpInvoice.InvoiceDate,
                                              startDate = tmpInvoice.StartDate,
                                              endDate = tmpInvoice.EndDate,
                                              totalDue = tmpInvoice.NetAmount
                                          }).FirstOrDefault();

                    DataRow row = invoice.Tables[invoice.Invoice.TableName].NewRow();
                    row[AmalgamatedPrintDataSet.AccountNameColumn] = settingDetail.AccountName;
                    row[AmalgamatedPrintDataSet.AccountNumberColumn] = settingDetail.AccountNumber;
                    row[AmalgamatedPrintDataSet.InvoiceNumberColumn] = invoiceDetails.invoiceNumber;
                    row[AmalgamatedPrintDataSet.CompanyNameColumn] = settingDetail.CompanyName;
                    row[AmalgamatedPrintDataSet.CompanyAddressColumn] = settingDetail.CompanyAddress;
                    row[AmalgamatedPrintDataSet.InvoiceDateColumn] = Convert.ToDateTime(invoiceDetails.invoiceDate);
                    row[AmalgamatedPrintDataSet.StartDateColumn] = Convert.ToDateTime(invoiceDetails.startDate);
                    row[AmalgamatedPrintDataSet.EndDateColumn] = Convert.ToDateTime(invoiceDetails.endDate);
                    row[AmalgamatedPrintDataSet.CustomerNameColumn] = invoiceDetails.customerName;
                    row[AmalgamatedPrintDataSet.CustomerAddressColumn] = invoiceDetails.customerAddress;
                    row[AmalgamatedPrintDataSet.TotalDueColumn] = Convert.ToDecimal(invoiceDetails.totalDue);
                    row[AmalgamatedPrintDataSet.CompanyCodeColumn] = companyDetail.Code;
                    row[AmalgamatedPrintDataSet.AccountNameColumn] = settingDetail.AccountName;
                    row[AmalgamatedPrintDataSet.AccountNumberColumn] = settingDetail.AccountNumber;
                    row[AmalgamatedPrintDataSet.SortCodeColumn] = settingDetail.SortCode;
                    invoice.Tables[invoice.Invoice.TableName].Rows.Add(row);
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }

            return invoice;
        }

        protected internal AmalgamatedPrintDetailDataSet PopulateAmalgamatedPrintDetail(int companyId, int invoiceId)
        {
            AmalgamatedPrintDetailDataSet invoice = new AmalgamatedPrintDetailDataSet();

            try
            {
                using (var context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    var invoiceDetails = (from tmpDetail in context.AmalgamatedInvoiceDetails
                                          join tmpClient in context.Clients on tmpDetail.ClientID equals tmpClient.ID
                                          join tmpCharges in context.ChargeHeads on tmpDetail.ChargeHeadID equals tmpCharges.ID
                                          where tmpDetail.AmalgamatedInvoiceID == invoiceId
                                          && tmpClient.CompanyId == companyId
                                          && tmpCharges.CompanyId == companyId
                                          select new
                                          {
                                              clientName = tmpClient.Name,
                                              clientId = tmpClient.ID,
                                              chargeId = tmpCharges.ID,
                                              chargeName = tmpCharges.Name,
                                              lessHead = tmpDetail.LessHead,
                                              lessAmount = tmpDetail.LessAmount,
                                              extraHead = tmpDetail.ExtraHead,
                                              extraAmount = tmpDetail.ExtraAmount,
                                              days = tmpDetail.Days,
                                              rate = tmpDetail.WeeklyRate,
                                              subTotal = tmpDetail.SubTotal,
                                              totalAmount = tmpDetail.TotalAmount
                                          }).ToList();
                    foreach(var details in invoiceDetails)
                    {
                        DataRow row = invoice.Tables[AmalgamatedPrintDetailDataSet.TableInvoiceDetail].NewRow();
                        row[AmalgamatedPrintDetailDataSet.ClientNameColumn] = Convert.ToString(details.clientName);
                        row[AmalgamatedPrintDetailDataSet.ChargeNameColumn] = Convert.ToString(details.chargeName);
                        row[AmalgamatedPrintDetailDataSet.ChargeIdColumn] = Convert.ToInt32(details.chargeId);
                        row[AmalgamatedPrintDetailDataSet.ClientIdColumn] = Convert.ToInt32(details.clientId);
                        row[AmalgamatedPrintDetailDataSet.LessHeadColumn] = Convert.ToString(details.lessHead);
                        row[AmalgamatedPrintDetailDataSet.LessAmountColumn] = Convert.ToDecimal(details.lessAmount);
                        row[AmalgamatedPrintDetailDataSet.ExtraHeadColumn] = Convert.ToString(details.extraHead);
                        row[AmalgamatedPrintDetailDataSet.ExtraAmountColumn] = Convert.ToDecimal(details.extraAmount);
                        row[AmalgamatedPrintDetailDataSet.DaysColumn] = Convert.ToInt32(details.days);
                        row[AmalgamatedPrintDetailDataSet.RateColumn] = Convert.ToDecimal(details.rate);
                        row[AmalgamatedPrintDetailDataSet.SubTotalColumn] = Convert.ToDecimal(details.subTotal); 
                        row[AmalgamatedPrintDetailDataSet.TotalAmountColumn] = Convert.ToDecimal(details.totalAmount);
                        invoice.Tables[AmalgamatedPrintDetailDataSet.TableInvoiceDetail].Rows.Add(row);
                    }                    
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }

            return invoice;
        }

        protected internal InvoiceDataSet PopulateAmalgamatedInvoices(int companyId)
        {
            var invoiceDataSet = new InvoiceDataSet();

            try
            {
                using (var context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    var invoices = (from tmpInvoice in context.AmalgamatedInvoices
                                    join tmpCustomer in context.Customers on tmpInvoice.CustomerID equals tmpCustomer.ID
                                    where tmpInvoice.CompanyId == companyId
                                    && tmpCustomer.CompanyId == companyId
                                    && !tmpInvoice.Printed
                                    select new
                                    {
                                        InvoiceId = tmpInvoice.ID,
                                        InvoiceNumber = tmpInvoice.InvoiceNumber,
                                        InvoiceDate = tmpInvoice.InvoiceDate,
                                        NetAmount = tmpInvoice.NetAmount,
                                        CustomerName = tmpCustomer.Name,
                                        CustomerId = tmpInvoice.CustomerID
                                    }).ToList();

                    if (invoices.Count > 0)
                    {
                        invoices.ForEach(invoice =>
                        {
                            DataRow row = invoiceDataSet.Tables[InvoiceDataSet.TableInvoice].NewRow();
                            row[InvoiceDataSet.IdColumn] = invoice.InvoiceId;
                            row[InvoiceDataSet.InvoiceNumberColumn] = invoice.InvoiceNumber;
                            row[InvoiceDataSet.ClientNameColumn] = invoice.CustomerName;
                            row[InvoiceDataSet.ClientIdColumn] = invoice.CustomerId;
                            row[InvoiceDataSet.InvoiceDateColumn] = invoice.InvoiceDate;
                            row[InvoiceDataSet.NetAmountColumn] = invoice.NetAmount;
                            invoiceDataSet.Tables[InvoiceDataSet.TableInvoice].Rows.Add(row);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }
            
            return invoiceDataSet;
        }

        protected internal InvoiceDataSet PopulateInvoiceData(int companyId)
        {
            InvoiceDataSet invoices = new InvoiceDataSet();
            List<Invoice> invoiceList = null;
            Client tmpClient = null;
            DataRow invoiceRow = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    invoiceList = context.Invoices.Where(s => s.CompanyId==companyId && s.Printed==false && s.Deleted==false).OrderByDescending(s => s.InvoiceDate).ToList();
                    if (invoiceList != null && invoiceList.Count > 0)
                    {
                        foreach(Invoice invoiceEntry in invoiceList)
                        {
                            invoiceRow = invoices.Tables[InvoiceDataSet.TableInvoice].NewRow();
                            invoiceRow[InvoiceDataSet.IdColumn] = invoiceEntry.ID;
                            tmpClient = context.Clients.Where(s => s.ID == invoiceEntry.ClientID).SingleOrDefault();
                            if (tmpClient != null)
                            {
                                invoiceRow[InvoiceDataSet.ClientNameColumn] = tmpClient.Name;
                            }
                            invoiceRow[InvoiceDataSet.InvoiceDateColumn] = invoiceEntry.InvoiceDate;
                            invoiceRow[InvoiceDataSet.InvoiceNumberColumn] = invoiceEntry.InvoiceNumber;
                            invoiceRow[InvoiceDataSet.NetAmountColumn] = invoiceEntry.NetAmount;
                            invoiceRow[InvoiceDataSet.MultiMonthColumn] = invoiceEntry.MultiMonth;
                            invoices.Tables[InvoiceDataSet.TableInvoice].Rows.Add(invoiceRow);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return invoices;
        }

        protected internal InvoiceDetailDataSet PopulateInvoiceData(long invoiceId)
        {
            InvoiceDetailDataSet invoiceDetail = new InvoiceDetailDataSet();
            Customer tmpCustomer = null;
            ChargeHead tmpCharge = null;
            List<InvoiceDetail> details = null;
            DataRow row = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    details = context.InvoiceDetails.Where(s => s.InvoiceID == invoiceId).ToList();
                    if (details != null && details.Count > 0)
                    {
                        foreach (InvoiceDetail detailEntry in details)
                        {
                            tmpCustomer = context.Customers.Where(s => s.ID == detailEntry.CustomerID).SingleOrDefault();
                            tmpCharge = context.ChargeHeads.Where(s => s.ID == detailEntry.ChargeHeadID).SingleOrDefault();

                            row = invoiceDetail.Tables[InvoiceDetailDataSet.TableInvoiceDetail].NewRow();
                            row[InvoiceDetailDataSet.ChargeHeadIdColumn] = detailEntry.ChargeHeadID;
                            row[InvoiceDetailDataSet.ChargeHeadNameColumn] = tmpCharge.Name;
                            row[InvoiceDetailDataSet.CustomerIdColumn] = detailEntry.CustomerID;
                            row[InvoiceDetailDataSet.CustomerNameColumn] = tmpCustomer.Name;
                            row[InvoiceDetailDataSet.ExtraAmountColumn] = detailEntry.ExtraAmount;
                            row[InvoiceDetailDataSet.ExtraPayHeadColumn] = detailEntry.ExtraHead;
                            row[InvoiceDetailDataSet.IdColumn] = detailEntry.ID;
                            row[InvoiceDetailDataSet.LessAmountColumn] = detailEntry.LessAmount;
                            row[InvoiceDetailDataSet.LessPayHeadColumn] = detailEntry.LessHead;
                            row[InvoiceDetailDataSet.NetAmountColumn] = detailEntry.TotalAmount;
                            row[InvoiceDetailDataSet.SubTotalColumn] = detailEntry.SubTotal;
                            row[InvoiceDetailDataSet.TotalDaysColumn] = detailEntry.Days;
                            row[InvoiceDetailDataSet.WeeklyRateColumn] = detailEntry.WeeklyRate;
                            invoiceDetail.Tables[InvoiceDetailDataSet.TableInvoiceDetail].Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return invoiceDetail;
        }

        protected internal Invoice GetInvoiceSingle(long invoiceId)
        {
            Invoice invoiceRecord = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    invoiceRecord = context.Invoices.Where(s => s.ID == invoiceId).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return invoiceRecord;
        }

        protected internal bool SaveInvoiceEntries(long invoiceId, InvoiceDetailDataSet details, string narration, DateTime startDate, DateTime endDate, bool historyInvoice)
        {
            bool success = false;
            List<InvoiceDetail> oldEntries = null;
            InvoiceDetail detailItem = null;
            Invoice invoice = null;
            decimal invoiceAmount = 0;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    oldEntries = context.InvoiceDetails.Where(s => s.InvoiceID == invoiceId).ToList();
                    if (oldEntries != null && oldEntries.Count > 0)
                    {
                        foreach (InvoiceDetail detailEntry in oldEntries)
                        {
                            context.InvoiceDetails.DeleteOnSubmit(detailEntry);
                        }
                        context.SubmitChanges();
                    }

                    invoice = context.Invoices.Where(s => s.ID == invoiceId).SingleOrDefault();

                    foreach (DataRow row in details.Tables[InvoiceDetailDataSet.TableInvoiceDetail].Rows)
                    {
                        detailItem = new InvoiceDetail();
                        detailItem.ChargeHeadID = Convert.ToInt32(row[InvoiceDetailDataSet.ChargeHeadIdColumn]);
                        detailItem.CustomerID = Convert.ToInt32(row[InvoiceDetailDataSet.CustomerIdColumn]);
                        detailItem.Days = Convert.ToInt32(row[InvoiceDetailDataSet.TotalDaysColumn]);
                        detailItem.ExtraAmount = Convert.ToDecimal(row[InvoiceDetailDataSet.ExtraAmountColumn]);
                        detailItem.ExtraHead = Convert.ToString(row[InvoiceDetailDataSet.ExtraPayHeadColumn]);
                        detailItem.InvoiceID = invoiceId;
                        detailItem.LessAmount = Convert.ToDecimal(row[InvoiceDetailDataSet.LessAmountColumn]);
                        detailItem.LessHead = Convert.ToString(row[InvoiceDetailDataSet.LessPayHeadColumn]);
                        detailItem.WeeklyRate = Convert.ToDecimal(row[InvoiceDetailDataSet.WeeklyRateColumn]);
                        detailItem.SubTotal = Convert.ToDecimal(row[InvoiceDetailDataSet.SubTotalColumn]);
                        detailItem.TotalAmount = Convert.ToDecimal(row[InvoiceDetailDataSet.NetAmountColumn]);
                        invoiceAmount += detailItem.TotalAmount;
                        invoice.InvoiceDetails.Add(detailItem);
                    }

                    invoice.NetAmount = invoiceAmount;
                    invoice.Narration = narration;
                    invoice.StartDate = startDate;
                    invoice.EndDate = endDate;
                    invoice.Printed = historyInvoice?true:false;
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

        protected internal InvoicePrintDataSet PopulatePrint(long invoiceId, int customerId,int companyId)
        {
            InvoicePrintDataSet invoiceData = new InvoicePrintDataSet();
            DataRow row = null;
            Company tmpCompany = null;
            List<InvoiceDetail> detailList = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    GlobalSetting settings = context.GlobalSettings.Where(s => s.CompanyId==companyId).SingleOrDefault();
                    Customer customer = context.Customers.Where(s => s.ID == customerId).SingleOrDefault();
                    Invoice invoice = context.Invoices.Where(s => s.ID==invoiceId).SingleOrDefault();
                    InvoiceDetail detail = context.InvoiceDetails.Where(s => s.InvoiceID==invoiceId && s.CustomerID==customerId).FirstOrDefault();
                    Client client = context.Clients.Where(s => s.ID == invoice.ClientID).SingleOrDefault();
                    ChargeHead charge = context.ChargeHeads.Where(s => s.ID == detail.ChargeHeadID).SingleOrDefault();

                    detailList = context.InvoiceDetails.Where(s => s.InvoiceID == invoiceId).ToList();

                    tmpCompany = context.Companies.Where(s => s.ID == companyId).SingleOrDefault();

                    row = invoiceData.Tables[InvoicePrintDataSet.TableInvoicePrint].NewRow();
                    row[InvoicePrintDataSet.AccountNameColumn] = settings.AccountName;
                    row[InvoicePrintDataSet.AccountNumberColumn] = settings.AccountNumber;
                    row[InvoicePrintDataSet.ClientCodeColumn] = client.Code;
                    row[InvoicePrintDataSet.ClientNameColumn] = client.Name;
                    row[InvoicePrintDataSet.TheirRefColumn] = client.TheirReference;
                    row[InvoicePrintDataSet.CompanyAddressColumn] = settings.CompanyAddress;
                    row[InvoicePrintDataSet.CompanyNameColumn] = settings.CompanyName;
                    row[InvoicePrintDataSet.CustomerAddressColumn] = customer.Address;
                    row[InvoicePrintDataSet.StartDateColumn] = invoice.StartDate;
                    row[InvoicePrintDataSet.EndDateColumn] = invoice.EndDate;
                    row[InvoicePrintDataSet.CustomerNameColumn] = customer.Name;
                    row[InvoicePrintDataSet.DateOfAdmissionColumn] = client.DateOfAdmission;
                    row[InvoicePrintDataSet.DateOfBirthColumn] = client.DateOfBirth;
                    row[InvoicePrintDataSet.ExtraAmountColumn] = detail.ExtraAmount;
                    row[InvoicePrintDataSet.ExtraPayHeadColumn] = detail.ExtraHead;
                    row[InvoicePrintDataSet.InvoiceDateColumn] = invoice.InvoiceDate;
                    row[InvoicePrintDataSet.InvoiceNumberColumn] = invoice.InvoiceNumber;
                    row[InvoicePrintDataSet.LessAmountColumn] = detail.LessAmount;
                    row[InvoicePrintDataSet.LessPayHeadColumn] = detail.LessHead;
                    row[InvoicePrintDataSet.NetAmountColumn] = invoice.NetAmount;
                    row[InvoicePrintDataSet.SortCodeColumn] = settings.SortCode;

                    if (detailList.Count > 1)
                    {
                        charge = context.ChargeHeads.Where(s => s.ID==detailList[0].ChargeHeadID).SingleOrDefault();
                        row[InvoicePrintDataSet.ChargeHeadColumn] = charge.Name;
                        row[InvoicePrintDataSet.WeeklyRateColumn] = detailList[0].WeeklyRate;

                        charge = context.ChargeHeads.Where(s => s.ID == detailList[1].ChargeHeadID).SingleOrDefault();
                        row[InvoicePrintDataSet.SecondChargeHeadColumn] = charge.Name;
                        row[InvoicePrintDataSet.SecondWeeklyRateColumn] = detailList[1].WeeklyRate;

                        row[InvoicePrintDataSet.SubTotalColumn] = detailList[0].SubTotal + detailList[1].SubTotal;
                    }
                    else
                    {
                        row[InvoicePrintDataSet.ChargeHeadColumn] = charge.Name;
                        row[InvoicePrintDataSet.WeeklyRateColumn] = detail.WeeklyRate;

                        row[InvoicePrintDataSet.SecondChargeHeadColumn] = string.Empty;
                        row[InvoicePrintDataSet.SecondWeeklyRateColumn] = 0;

                        row[InvoicePrintDataSet.SubTotalColumn] = detail.SubTotal;
                        row[InvoicePrintDataSet.ChargeHeadColumn] = charge.Name;
                        row[InvoicePrintDataSet.WeeklyRateColumn] = detail.WeeklyRate;
                    }

                    row[InvoicePrintDataSet.TotalDaysColumn] = detail.Days;
                    
                    if (tmpCompany != null)
                    {
                        row[InvoicePrintDataSet.CompanyCodeColumn] = tmpCompany.Code;
                    }
                    invoiceData.Tables[InvoicePrintDataSet.TableInvoicePrint].Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return invoiceData;
        }

        protected internal InvoiceBreakDownDataSet PopulateInvoiceBreakdown(long invoiceId)
        {
            InvoiceBreakDownDataSet invoiceBreakDown = new InvoiceBreakDownDataSet();
            DataRow invoiceDataRow = null;
            int invoicePartCount = 0;
            DateTime startDate;
            DateTime endDate;
            DateTime tempDate;
            DateTime tempEndDate;
            DateTime tempDueDate;
            decimal feeAmount = 0;
            using (var context = new InvoiceContextDataContext(this._ConnectionString))
            {
                var invoice = context.Invoices.FirstOrDefault(s => s.ID == invoiceId);
                var invoiceList = context.InvoiceDetails.Where(s => s.InvoiceID == invoiceId).ToList();
                startDate = invoice.StartDate;
                endDate = invoice.EndDate;
                tempDate = startDate;

                foreach (var invoiceDetail in invoiceList)
                {
                    feeAmount += invoiceDetail.WeeklyRate / Convert.ToDecimal(7.00) * invoiceDetail.Days;
                }

                while (tempDate <= endDate)
                {
                    invoiceDataRow = invoiceBreakDown.Tables[InvoiceBreakDownDataSet.TableInvoiceBreakDown].NewRow();
                    invoicePartCount++;
                    invoiceDataRow[InvoiceBreakDownDataSet.InvoicePartColumn] = string.Format(
                        CultureInfo.CurrentCulture, "{0}/{1}", invoiceId, invoicePartCount);
                    invoiceDataRow[InvoiceBreakDownDataSet.InvoiceSubPartColumn] = string.Format(
                        CultureInfo.CurrentCulture, "{0}/{1}", invoiceId, invoicePartCount);
                    invoiceDataRow[InvoiceBreakDownDataSet.StartDateColumn] =
                        string.Concat(tempDate.Day.ToString().PadLeft(2, '0'), "/",
                            tempDate.Month.ToString().PadLeft(2, '0'), "/", tempDate.Year);
                    tempDueDate = tempDate.AddDays(5);
                    invoiceDataRow[InvoiceBreakDownDataSet.DueDateColumn] =
                        string.Concat(tempDueDate.Day.ToString().PadLeft(2, '0'), "/",
                            tempDueDate.Month.ToString().PadLeft(2, '0'), "/", tempDueDate.Year);
                    invoiceDataRow[InvoiceBreakDownDataSet.TotalDaysColumn] = 28;
                    tempEndDate = tempDate.AddDays(28);
                    invoiceDataRow[InvoiceBreakDownDataSet.EndDateColumn] =
                        string.Concat(tempEndDate.Day.ToString().PadLeft(2, '0'), "/",
                            tempEndDate.Month.ToString().PadLeft(2, '0'), "/", tempEndDate.Year);
                    invoiceDataRow[InvoiceBreakDownDataSet.FeeAmountColumn] = feeAmount;
                    tempDate = tempDate.AddDays(29);
                    invoiceBreakDown.Tables[InvoiceBreakDownDataSet.TableInvoiceBreakDown].Rows.Add(invoiceDataRow);
                }
            }
            return invoiceBreakDown;
        }

        protected internal List<int> GetCustomerForInvoice(long invoiceId)
        {
            List<int> customerList = new List<int>();
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    List<InvoiceDetail> detailList = context.InvoiceDetails.Where(s => s.InvoiceID == invoiceId).ToList();
                    if (detailList != null && detailList.Count > 0)
                    {
                        foreach (InvoiceDetail detailItem in detailList)
                        {
                            customerList.Add(detailItem.CustomerID);
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

        private string GetMonthName(int month)
        {
            string name = string.Empty;
            switch (month)
            {
                case 1:
                    {
                        name = "Jan";
                        break;
                    }
                case 2:
                    {
                        name = "Feb";
                        break;
                    }
                case 3:
                    {
                        name = "Mar";
                        break;
                    }
                case 4:
                    {
                        name = "Apr";
                        break;
                    }
                case 5:
                    {
                        name = "May";
                        break;
                    }
                case 6:
                    {
                        name = "Jun";
                        break;
                    }
                case 7:
                    {
                        name = "Jul";
                        break;
                    }
                case 8:
                    {
                        name = "Aug";
                        break;
                    }
                case 9:
                    {
                        name = "Sep";
                        break;
                    }
                case 10:
                    {
                        name = "Oct";
                        break;
                    }
                case 11:
                    {
                        name = "Nov";
                        break;
                    }
                case 12:
                    {
                        name = "Dec";
                        break;
                    }
            }
            return name;
        }

        protected internal List<CustomerFeeModel> GetAllActiveCustomersInBreakdowns(int companyId, List<int> selectedClients)
        {
            List<CustomerFeeModel> customerList = null;

            try
            {
                using (var context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    var customerFeeMapping = (from tmpBreakdown in context.BreakDowns
                                              join
                                              tmpBreakdownDetail in context.BreakDownDetails on tmpBreakdown.ID equals tmpBreakdownDetail.BreakDownID
                                              join
                                              tmpCustomer in context.Customers on tmpBreakdownDetail.CustomerID equals tmpCustomer.ID
                                              join
                                              tmpCharge in context.ChargeHeads on tmpBreakdownDetail.ChargeHeadID equals tmpCharge.ID
                                              where tmpBreakdown.CompanyId == companyId && tmpCustomer.CompanyId == companyId && !tmpCustomer.IsDeleted && selectedClients.Contains(tmpBreakdown.ClientID.Value)
                                              orderby tmpCustomer.Name
                                              select new
                                              {
                                                  ChargeHeadId = tmpBreakdownDetail.ChargeHeadID,
                                                  ChargeHeadName = tmpCharge.Name,
                                                  CustomerCode = tmpCustomer.Code,
                                                  CustomerId = tmpCustomer.ID,
                                                  SageReference = tmpCustomer.SageReference
                                              }).AsEnumerable().Distinct().ToList();
                    
                    if(customerFeeMapping!=null && customerFeeMapping.Count > 0)
                    {
                        customerList = new List<CustomerFeeModel>();
                        customerFeeMapping.ForEach(s =>
                        {
                            customerList.Add(new CustomerFeeModel
                            {
                                ChargeHeadId = s.ChargeHeadId,
                                ChargeHeadName = s.ChargeHeadName,
                                CustomerCode = s.CustomerCode,
                                CustomerId = s.CustomerId,
                                SageReference = s.SageReference
                            });
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }

            return customerList;
        }

        protected internal decimal GetBreakdownValueForCustomer(int companyId, int clientId, int customerId, int chargeHeadId)
        {
            decimal result = 0;

            try
            {
                using (var context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    var breakdown = (from tmpBreakdown in context.BreakDowns
                                     join tmpDetail in context.BreakDownDetails on tmpBreakdown.ID equals tmpDetail.BreakDownID
                                     where tmpBreakdown.CompanyId == companyId && tmpBreakdown.ClientID == clientId && tmpDetail.CustomerID == customerId && tmpDetail.ChargeHeadID == chargeHeadId
                                     select tmpDetail).FirstOrDefault();
                    if (breakdown != null) result = breakdown.Rate;
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }

            return result;
        }

        protected internal InvoiceCsvDataSet GenerateExportData(DateTime startDate, DateTime endDate, bool includeInvoice, bool includeCreditNote, int companyId, bool includeExported)
        {
            InvoiceCsvDataSet csvData = new InvoiceCsvDataSet();
            InvoiceDetail detail = null;
            Customer tmpCustomer = null;
            Company companyData = null;
            Client tmpClient = null;
            DataRow row = null;
            startDate = Convert.ToDateTime(startDate.Day.ToString().PadLeft(2,'0') + '/' + GetMonthName(startDate.Month) + '/' + startDate.Year.ToString() + " 12:00:00 AM");
            endDate = Convert.ToDateTime(endDate.Day.ToString().PadLeft(2, '0') + '/' + GetMonthName(endDate.Month) + '/' + endDate.Year.ToString() + " 11:59:59 PM");
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    companyData = context.Companies.Where(s => s.ID==companyId).SingleOrDefault();
                    List<Invoice> invoiceList = context.Invoices.Where(s => (s.InvoiceDate >= startDate && s.InvoiceDate <= endDate && s.CompanyId == companyId && s.Deleted==false)).ToList();
                    if (!includeExported) invoiceList = invoiceList.Where(s => s.Printed == false).ToList();
                    if ((invoiceList != null) && (invoiceList.Count > 0) && (includeInvoice))
                    {
                        foreach (Invoice invoiceEntry in invoiceList)
                        {
                            tmpClient = context.Clients.Where(s => s.ID == invoiceEntry.ClientID).SingleOrDefault();

                            row = csvData.Tables[InvoiceCsvDataSet.TableInvoiceExport].NewRow();
                            row[InvoiceCsvDataSet.AccountCodeColumn] = companyData.AccountCode;
                            row[InvoiceCsvDataSet.AccountNumberColumn] = companyData.AccountNumber;
                            row[InvoiceCsvDataSet.ClientCodeColumn] = tmpClient.Code;
                            row[InvoiceCsvDataSet.ClientNameColumn] = tmpClient.Name;
                            row[InvoiceCsvDataSet.ExportFormatColumn] = "EXCEL";
                            row[InvoiceCsvDataSet.InvoiceAmountColumn] = invoiceEntry.NetAmount.ToString("N2");
                            detail = context.InvoiceDetails.Where(s => s.InvoiceID == invoiceEntry.ID).FirstOrDefault();
                            if (detail != null)
                            {
                                tmpCustomer = context.Customers.Where(s => s.ID == detail.CustomerID).SingleOrDefault();
                                if (tmpCustomer != null)
                                {
                                    if (tmpCustomer.ID == 3 || tmpCustomer.ID == 20 || tmpCustomer.ID == 28)
                                    {
                                        row[InvoiceCsvDataSet.SageReferenceColumn] = tmpCustomer.Code;
                                        row[InvoiceCsvDataSet.NarrationColumn] = string.Format(CultureInfo.CurrentCulture, "{0} {1} To {2}", tmpClient.Name, invoiceEntry.StartDate.ToShortDateString(), invoiceEntry.EndDate.ToShortDateString());
                                    }
                                    else
                                    {
                                        row[InvoiceCsvDataSet.SageReferenceColumn] = tmpClient.SageReference;
                                        row[InvoiceCsvDataSet.NarrationColumn] = string.Format(CultureInfo.CurrentCulture, "{0} {1} To {2}", tmpClient.Name, invoiceEntry.StartDate.ToShortDateString(), invoiceEntry.EndDate.ToShortDateString());
                                    }
                                }
                            }
                            
                            row[InvoiceCsvDataSet.TaxAmountColumn] = 0;
                            row[InvoiceCsvDataSet.TaxCodeColumn] = "T9";
                            row[InvoiceCsvDataSet.TypeColumn] = "SI";
                            row[InvoiceCsvDataSet.TransactionDateColumn] = invoiceEntry.InvoiceDate.ToShortDateString();
                            row[InvoiceCsvDataSet.TransactionNumberColumn] = invoiceEntry.InvoiceNumber;
                            row[InvoiceCsvDataSet.InternalIdColumn] = invoiceEntry.ID;
                            row[InvoiceCsvDataSet.MultiMonthColumn] = invoiceEntry.MultiMonth;
                            csvData.Tables[InvoiceCsvDataSet.TableInvoiceExport].Rows.Add(row);
                        }
                    }

                    List<CreditNote> noteList = context.CreditNotes.Where(s => (s.TransactionDate >= startDate && s.TransactionDate <= endDate && s.CompanyId == companyId && s.Deleted==false)).ToList();
                    if (!includeExported) noteList = noteList.Where(s => s.Printed == false).ToList();
                    if ((noteList != null) && (noteList.Count > 0) && (includeCreditNote))
                    {
                        foreach (CreditNote noteItem in noteList)
                        {
                            tmpClient = context.Clients.Where(s => s.ID == noteItem.ClientId).SingleOrDefault();
                            tmpCustomer = context.Customers.Where(s => s.ID == noteItem.CustomerId).SingleOrDefault();
                            row = csvData.Tables[InvoiceCsvDataSet.TableInvoiceExport].NewRow();
                            row[InvoiceCsvDataSet.AccountCodeColumn] = companyData.AccountCode;
                            row[InvoiceCsvDataSet.AccountNumberColumn] = companyData.AccountNumber;
                            row[InvoiceCsvDataSet.ClientCodeColumn] = tmpClient.Code;
                            row[InvoiceCsvDataSet.ExportFormatColumn] = "EXCEL";
                            row[InvoiceCsvDataSet.InvoiceAmountColumn] = noteItem.Amount.ToString("N2");
                            if (tmpCustomer != null)
                            {
                                row[InvoiceCsvDataSet.NarrationColumn] = string.Format(CultureInfo.CurrentCulture, "{0} Credit Note", tmpCustomer.Name);
                            }
                            row[InvoiceCsvDataSet.SageReferenceColumn] = tmpClient.SageReference;
                            row[InvoiceCsvDataSet.TaxAmountColumn] = 0;
                            row[InvoiceCsvDataSet.TaxCodeColumn] = "T9";
                            row[InvoiceCsvDataSet.TypeColumn] = "SC";
                            row[InvoiceCsvDataSet.TransactionDateColumn] = noteItem.TransactionDate.ToShortDateString();
                            row[InvoiceCsvDataSet.TransactionNumberColumn] = noteItem.TransactionNumber;
                            row[InvoiceCsvDataSet.InternalIdColumn] = noteItem.ID;
                            csvData.Tables[InvoiceCsvDataSet.TableInvoiceExport].Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return csvData;
        }

        protected internal string FormatDate(DateTime input)
        {
            string output = string.Format(CultureInfo.CurrentCulture, "{0}/{1}/{2}", input.Day, input.Month.ToString().PadLeft(2, '0'), input.Year);
            return output;
        }

        protected internal InvoiceHistoryDataSet PopulateInvoiceHistory_Old(int companyId)
        {
            DataRow[] existingRow = null;
            InvoiceHistoryDataSet historyData = new InvoiceHistoryDataSet();
            DataRow row = null;
            ChargeHead tmpCharge = null;
            Customer tmpCustomer = null;
            Client tmpClient = null;
            DateTime startDate;
            DateTime endDate;
            int weeks = 0;
            
            List<InvoiceDetail> details = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    List<Invoice> invoiceList = context.Invoices.Where(s => s.CompanyId == companyId && s.Printed==true).ToList();
                    foreach (Invoice invoiceItem in invoiceList)
                    {
                        tmpClient = context.Clients.Where(s => s.ID == invoiceItem.ClientID).SingleOrDefault();
                        startDate = invoiceItem.StartDate;
                        endDate = invoiceItem.EndDate;
                        weeks = Convert.ToInt32((endDate - startDate).TotalDays / 7);
                        details = context.InvoiceDetails.Where(s => s.InvoiceID == invoiceItem.ID).ToList();

                        foreach (InvoiceDetail detailItem in details)
                        {
                            tmpCustomer = context.Customers.Where(s => s.ID == detailItem.CustomerID).SingleOrDefault();
                            tmpCharge = context.ChargeHeads.Where(s => s.ID == detailItem.ChargeHeadID).SingleOrDefault();

                            string filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} = '{1}' AND {2} = '{3}' AND {4} = '{5}'"
                                , InvoiceHistoryDataSet.InvoiceNumberColumn, invoiceItem.InvoiceNumber.Replace("'", "''")
                                , InvoiceHistoryDataSet.CustomerCodeColumn, tmpCustomer.Code.Replace("'", "''")
                                , InvoiceHistoryDataSet.ClientCodeColumn, tmpClient.Code.Replace("'", "''")
                                , InvoiceHistoryDataSet.ChargeNameColumn, tmpCharge.Name.Replace("'", "''"));

                            existingRow = historyData.Tables[InvoiceHistoryDataSet.TableInvoiceHistory].Select(filterCondition);

                            if (existingRow != null && existingRow.Length > 0)
                            {
                                existingRow[0][InvoiceHistoryDataSet.ExtraAmountColumn] = Convert.ToDecimal(existingRow[0][InvoiceHistoryDataSet.ExtraAmountColumn]) + detailItem.ExtraAmount;
                                existingRow[0][InvoiceHistoryDataSet.LessAmountColumn] = Convert.ToDecimal(existingRow[0][InvoiceHistoryDataSet.LessAmountColumn]) + detailItem.LessAmount;
                                existingRow[0][InvoiceHistoryDataSet.NetAmountColumn] = Convert.ToDecimal(existingRow[0][InvoiceHistoryDataSet.NetAmountColumn]) + detailItem.TotalAmount;
                                existingRow[0][InvoiceHistoryDataSet.SubTotalColumn] = Convert.ToDecimal(existingRow[0][InvoiceHistoryDataSet.SubTotalColumn]) + detailItem.SubTotal;
                                existingRow[0][InvoiceHistoryDataSet.WeeklyRateColumn] = Convert.ToDecimal(existingRow[0][InvoiceHistoryDataSet.WeeklyRateColumn]) + detailItem.WeeklyRate;
                                existingRow[0][InvoiceHistoryDataSet.ChargeNameColumn] = string.Concat(Convert.ToString(existingRow[0][InvoiceHistoryDataSet.ChargeNameColumn])," + ",tmpCharge.Name);
                            }
                            else
                            {
                                row = historyData.Tables[InvoiceHistoryDataSet.TableInvoiceHistory].NewRow();
                                row[InvoiceHistoryDataSet.ChargeNameColumn] = tmpCharge.Name;
                                row[InvoiceHistoryDataSet.ChargePeriodColumn] = string.Format(CultureInfo.CurrentCulture, "{0} to {1} - {2} weeks", FormatDate(startDate), FormatDate(endDate), weeks);
                                row[InvoiceHistoryDataSet.ClientCodeColumn] = tmpClient.Code;
                                row[InvoiceHistoryDataSet.ClientNameColumn] = tmpClient.Name;
                                row[InvoiceHistoryDataSet.CustomerCodeColumn] = tmpCustomer.Code;
                                row[InvoiceHistoryDataSet.CustomerNameColumn] = tmpCustomer.Name;
                                row[InvoiceHistoryDataSet.DaysColumn] = detailItem.Days;
                                row[InvoiceHistoryDataSet.ExtraAmountColumn] = detailItem.ExtraAmount;
                                row[InvoiceHistoryDataSet.ExtraHeadColumn] = detailItem.ExtraHead;
                                row[InvoiceHistoryDataSet.IdColumn] = invoiceItem.ID;
                                row[InvoiceHistoryDataSet.InvoiceDateColumn] = invoiceItem.InvoiceDate;
                                row[InvoiceHistoryDataSet.InvoiceNumberColumn] = invoiceItem.InvoiceNumber;
                                row[InvoiceHistoryDataSet.LessAmountColumn] = detailItem.LessAmount;
                                row[InvoiceHistoryDataSet.LessHeadColumn] = detailItem.LessHead;
                                row[InvoiceHistoryDataSet.NetAmountColumn] = detailItem.TotalAmount;
                                row[InvoiceHistoryDataSet.SubTotalColumn] = detailItem.SubTotal;
                                row[InvoiceHistoryDataSet.WeeklyRateColumn] = detailItem.WeeklyRate;
                                row[InvoiceHistoryDataSet.InvoiceTypeColumn] = "SI";
                                row[InvoiceHistoryDataSet.DeletedColumn] = invoiceItem.Deleted;
                                row[InvoiceHistoryDataSet.MultiMonthColumn] = invoiceItem.MultiMonth;
                                historyData.Tables[InvoiceHistoryDataSet.TableInvoiceHistory].Rows.Add(row);
                            }
                        }
                    }
                    List<CreditNote> noteList = context.CreditNotes.Where(s => s.CompanyId == companyId && s.Printed==true).ToList();
                    foreach (CreditNote noteItem in noteList)
                    {
                        tmpClient = context.Clients.Where(s => s.ID == noteItem.ClientId).SingleOrDefault();
                        tmpCustomer = context.Customers.Where(s => s.ID == noteItem.CustomerId).SingleOrDefault();
                        
                        foreach (InvoiceDetail detailItem in details)
                        {
                            row = historyData.Tables[InvoiceHistoryDataSet.TableInvoiceHistory].NewRow();
                            row[InvoiceHistoryDataSet.ChargeNameColumn] = " ";
                            row[InvoiceHistoryDataSet.ChargePeriodColumn] = " ";
                            row[InvoiceHistoryDataSet.ClientCodeColumn] = tmpClient.Code;
                            row[InvoiceHistoryDataSet.ClientNameColumn] = tmpClient.Name;
                            if (tmpCustomer != null)
                            {
                                row[InvoiceHistoryDataSet.CustomerCodeColumn] = tmpCustomer.Code;
                                row[InvoiceHistoryDataSet.CustomerNameColumn] = tmpCustomer.Name;
                            }
                            else
                            {
                                row[InvoiceHistoryDataSet.CustomerCodeColumn] = " ";
                                row[InvoiceHistoryDataSet.CustomerNameColumn] = " ";
                            }
                            row[InvoiceHistoryDataSet.DaysColumn] = 0;
                            row[InvoiceHistoryDataSet.ExtraAmountColumn] = 0;
                            row[InvoiceHistoryDataSet.ExtraHeadColumn] = " ";
                            row[InvoiceHistoryDataSet.IdColumn] = noteItem.ID;
                            row[InvoiceHistoryDataSet.InvoiceDateColumn] = noteItem.TransactionDate;
                            row[InvoiceHistoryDataSet.InvoiceNumberColumn] = noteItem.TransactionNumber;
                            row[InvoiceHistoryDataSet.LessAmountColumn] = 0;
                            row[InvoiceHistoryDataSet.LessHeadColumn] = " ";
                            row[InvoiceHistoryDataSet.NetAmountColumn] = noteItem.Amount;
                            row[InvoiceHistoryDataSet.SubTotalColumn] = noteItem.Amount;
                            row[InvoiceHistoryDataSet.WeeklyRateColumn] = 0;
                            row[InvoiceHistoryDataSet.InvoiceTypeColumn] = "SC";
                            row[InvoiceHistoryDataSet.DeletedColumn] = noteItem.Deleted;
                            row[InvoiceHistoryDataSet.MultiMonthColumn] = false;
                            historyData.Tables[InvoiceHistoryDataSet.TableInvoiceHistory].Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return historyData;
        }

        protected internal InvoiceHistoryDataSet PopulateInvoiceHistory(int companyId)
        {
            InvoiceHistoryDataSet invoiceHistoryDataSet = new InvoiceHistoryDataSet();
            SqlParameter parameter = null;
            DataRow row = null;

            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    context.Connection.Open();
                    using (var command = context.Connection.CreateCommand())
                    {
                        command.CommandTimeout = 0;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "usp_Invoice_History";

                        parameter = new SqlParameter
                        {
                            ParameterName = "@CompanyId",
                            Direction = ParameterDirection.Input,
                            SqlDbType = SqlDbType.Int,
                            Value = companyId
                        };

                        command.Parameters.Add(parameter);

                        using (var reader = command.ExecuteReader(CommandBehavior.Default))
                        {
                            while (reader.Read())
                            {
                                row = invoiceHistoryDataSet.Tables[InvoiceHistoryDataSet.TableInvoiceHistory].NewRow();

                                row[InvoiceHistoryDataSet.ChargeNameColumn] = Convert.ToString(reader["ChargeName"]);
                                row[InvoiceHistoryDataSet.ChargePeriodColumn] = Convert.ToString(reader["ChargePeriod"]);
                                row[InvoiceHistoryDataSet.ClientCodeColumn]= Convert.ToString(reader["ClientCode"]);
                                row[InvoiceHistoryDataSet.ClientNameColumn] = Convert.ToString(reader["ClientName"]);
                                row[InvoiceHistoryDataSet.CustomerCodeColumn] = Convert.ToString(reader["CustomerCode"]);
                                row[InvoiceHistoryDataSet.CustomerNameColumn] = Convert.ToString(reader["CustomerName"]);
                                row[InvoiceHistoryDataSet.DaysColumn] = Convert.ToInt32(reader["Days"]);
                                row[InvoiceHistoryDataSet.DeletedColumn] = Convert.ToBoolean(reader["Deleted"]);
                                row[InvoiceHistoryDataSet.ExtraAmountColumn] = Convert.ToDecimal(reader["ExtraAmount"]);
                                row[InvoiceHistoryDataSet.ExtraHeadColumn] = Convert.ToString(reader["ExtraHead"]);
                                row[InvoiceHistoryDataSet.IdColumn] = Convert.ToInt64(reader["Id"]);
                                row[InvoiceHistoryDataSet.InvoiceDateColumn] = Convert.ToDateTime(reader["InvoiceDate"]);
                                row[InvoiceHistoryDataSet.InvoiceNumberColumn] = Convert.ToString(reader["InvoiceNumber"]);
                                row[InvoiceHistoryDataSet.InvoiceTypeColumn] = Convert.ToString(reader["InvoiceType"]);
                                row[InvoiceHistoryDataSet.LessAmountColumn] = Convert.ToDecimal(reader["LessAmount"]);
                                row[InvoiceHistoryDataSet.LessHeadColumn] = Convert.ToString(reader["LessHead"]);
                                row[InvoiceHistoryDataSet.MultiMonthColumn] = Convert.ToBoolean(reader["MultiMonth"]);
                                row[InvoiceHistoryDataSet.NetAmountColumn] = Convert.ToDecimal(reader["NetAmount"]);
                                row[InvoiceHistoryDataSet.SelectColumn] = Convert.ToBoolean(reader["Select"]);
                                row[InvoiceHistoryDataSet.SubTotalColumn] = Convert.ToDecimal(reader["SubTotal"]);
                                row[InvoiceHistoryDataSet.WeeklyRateColumn] = Convert.ToDecimal(reader["WeeklyRate"]);

                                invoiceHistoryDataSet.Tables[InvoiceHistoryDataSet.TableInvoiceHistory].Rows.Add(row);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }

            return invoiceHistoryDataSet;
        }

        protected internal bool UpdatedInvoicePrinted(long invoiceId)
        {
            bool success = false;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    Invoice tmpInvoice = context.Invoices.Where(s => s.ID == invoiceId).SingleOrDefault();
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

        protected internal bool UpdatedInvoicePrintedByUser(long invoiceId)
        {
            bool success = false;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    Invoice tmpInvoice = context.Invoices.Where(s => s.ID == invoiceId).SingleOrDefault();
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

        protected internal bool DeleteInvoice(long invoiceId)
        {
            bool success = false;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    Invoice tmpInvoice = context.Invoices.Where(s => s.ID==invoiceId).SingleOrDefault();
                    if (tmpInvoice != null) 
                    { 
                        tmpInvoice.Deleted = true;
                        tmpInvoice.Printed = true;
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

        protected internal string GenerateInvoiceNumber(int companyId)
        {
            string invoiceNumber = string.Empty;
            int invoiceCount = 0;
            int amalgamatedCount = 0;
            int maxNumber = 0;
            int maxAmalgamated = 0;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    invoiceCount = context.Invoices.Where(s => s.CompanyId == companyId).Count();
                    if (invoiceCount == 0)
                    {
                        invoiceNumber = "1";
                    }
                    else
                    {
                        maxNumber = Convert.ToInt32(context.Invoices.Where(s => s.CompanyId == companyId).ToList().Max(s => Convert.ToInt32(s.InvoiceNumber)));
                        invoiceNumber = (maxNumber + 1).ToString();
                    }

                    amalgamatedCount = context.AmalgamatedInvoices.Where((s) => s.CompanyId == companyId).Count();
                    if (amalgamatedCount > 0)
                    {
                        maxAmalgamated = context.AmalgamatedInvoices.Where ((s) => s.CompanyId == companyId).Max(s => Convert.ToInt32(s.InvoiceNumber));
                        if (maxAmalgamated >= maxNumber)
                        {
                            invoiceNumber = (maxAmalgamated + 1).ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return invoiceNumber;
        }

        protected internal bool InvoiceExists(int companyId, string invoiceNumber)
        {
            bool exists = false;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    Invoice invoice = context.Invoices.Where(s => s.CompanyId == companyId && string.Compare(s.InvoiceNumber, invoiceNumber, false) == 0).SingleOrDefault();
                    exists = (invoice != null);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return exists;
        }

        protected internal CustomerDataSet PopulateCustomerForClient(int companyId, int clientId)
        {
            CustomerDataSet customerList = new CustomerDataSet();
            BreakDown breakdown = null;
            List<BreakDownDetail> detailList = null;
            DataRow rowItem = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    breakdown = context.BreakDowns.Where(s => s.CompanyId == companyId && s.ClientID == clientId).SingleOrDefault();
                    if (breakdown != null)
                    {
                        detailList = context.BreakDownDetails.Where(s => s.BreakDownID == breakdown.ID).ToList();
                        
                        rowItem = customerList.Tables[CustomerDataSet.TableCustomer].NewRow();
                        rowItem[CustomerDataSet.IdColumn] = 0;
                        rowItem[CustomerDataSet.NameColumn] = "All";
                        customerList.Tables[CustomerDataSet.TableCustomer].Rows.Add(rowItem);
                        
                        foreach (BreakDownDetail detailItem in detailList)
                        {
                            rowItem = customerList.Tables[CustomerDataSet.TableCustomer].NewRow();
                            rowItem[CustomerDataSet.IdColumn] = detailItem.CustomerID;
                            rowItem[CustomerDataSet.NameColumn] = context.Customers.Where(s => s.ID == detailItem.CustomerID).SingleOrDefault().Name;
                            customerList.Tables[CustomerDataSet.TableCustomer].Rows.Add(rowItem);
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

        protected internal bool SaveManualInvoice(Invoice invoice, List<InvoiceDetail> invoiceDetail)
        {
            bool success = false;
            long invoiceId = 0;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    context.Invoices.InsertOnSubmit(invoice);
                    context.SubmitChanges();

                    invoiceId = invoice.ID;

                    foreach (InvoiceDetail detailItem in invoiceDetail)
                    {
                        detailItem.InvoiceID = invoiceId;
                        context.InvoiceDetails.InsertOnSubmit(detailItem);
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

        protected internal decimal GetWeeklyRate(int companyId, int clientId, int customerId)
        {
            decimal weeklyRate = 0;
            int breakDownId = 0;
            BreakDown breakDown = null;
            BreakDownDetail detail = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    breakDown = context.BreakDowns.Where(s => s.CompanyId == companyId && s.ClientID == clientId).SingleOrDefault();
                    if (breakDown != null)
                    {
                        breakDownId = breakDown.ID;
                        detail = context.BreakDownDetails.Where(s => s.BreakDownID == breakDownId && s.CustomerID == customerId).SingleOrDefault();
                        if (detail != null)
                        {
                            weeklyRate = detail.Rate;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return weeklyRate;
        }

        protected internal bool ResetInvoiceNumber(int invoiceNumber)
        {
            SqlParameter parameter = null;
            bool success = false;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    parameter = new SqlParameter();
                    parameter.ParameterName = "@InvoiceNumber";
                    parameter.SqlDbType = SqlDbType.Int;
                    parameter.Direction = ParameterDirection.Input;
                    parameter.Value = invoiceNumber;

                    using (SqlConnection connection = (SqlConnection)context.Connection)
                    {
                        connection.Open();
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandTimeout = 0;
                            command.CommandType = CommandType.Text;
                            command.CommandText = CustomerInvoice.Data.Properties.Resources.ResetInvoiceScript;
                            command.Parameters.Add(parameter);
                            command.ExecuteNonQuery();
                        }
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

        protected internal bool DeleteInvoiceWithAdjust()
        {
            bool success = false;
            int deletedNumber = 0;
            List<Invoice> deletedInvoiceList = null;
            string query = string.Empty;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    deletedInvoiceList = context.Invoices.Where(s => s.Deleted == true).OrderBy(s => s.ID).ToList();
                    if (deletedInvoiceList != null && deletedInvoiceList.Count > 0)
                    {
                        using (SqlConnection connection = (SqlConnection)context.Connection)
                        {
                            connection.Open();
                            using (SqlCommand command = connection.CreateCommand())
                            {
                                foreach (Invoice invoiceItem in deletedInvoiceList)
                                {
                                    deletedNumber = Convert.ToInt32(invoiceItem.InvoiceNumber);
                                    command.CommandTimeout = 0;
                                    command.CommandType = CommandType.Text;
                                    query = string.Format(CultureInfo.CurrentCulture, @"UPDATE Invoice SET InvoiceNumber = InvoiceNumber - 1 WHERE InvoiceNumber>{0}", deletedNumber);
                                    command.CommandText = query;
                                    command.ExecuteNonQuery();
                                }
                                success = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return success;
        }

        protected internal List<Client> CheckClientWithRip(int companyId, DateTime invoiceEndDate)
        {
            var clientList = new List<Client>();
            var clients = new List<int>();
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    context.Connection.Open();
                    using (SqlCommand command = (SqlCommand)context.Connection.CreateCommand())
                    {
                        command.CommandText = "usp_CheckClientRip";
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 0;
                        command.Parameters.Add(new SqlParameter { ParameterName = "CompanyId", SqlDbType = SqlDbType.Int, Value = companyId });
                        command.Parameters.Add(new SqlParameter { ParameterName = "InvoiceEndDate", SqlDbType = SqlDbType.DateTime, Value = invoiceEndDate });

                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.Default))
                        {
                            while (reader.Read())
                            {
                                clients.Add(Convert.ToInt32(reader[0]));
                            }
                        }
                    }

                    if(clients!=null && clients.Any())
                    {
                        foreach(var clientId in clients)
                        {
                            clientList.Add(context.Clients.FirstOrDefault(s => s.ID == clientId));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }

            return clientList;
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
