using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using CustomerInvoice.Data.DataSets;
using CustomerInvoice.Data.Helpers;
using CustomerInvoice.Data.Models;

namespace CustomerInvoice.Data
{
    public sealed class DataLayer
    {
        #region Public methods

        public static bool GenerateAmalgamatedInvoice(int companyId, int customerId, DateTime invoiceDate, string narration, int days, DateTime startDate, DateTime endDate)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.GenerateAmalgamatedInvoice(companyId, customerId, invoiceDate, narration, days, startDate, endDate); ;
            }
        }

        public static InvoiceDataSet PopulateAmalgamatedInvoices(int companyId)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.PopulateAmalgamatedInvoices(companyId);
            }
        }

        public static bool SaveAmalgamatedInvoice(AmalgamatedInvoice invoice, AmalgamatedInvoiceDetailDataSet invoiceDetail)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.SaveAmalgamatedInvoice(invoice, invoiceDetail);
            }
        }

        public static List<InvoiceExportModel> GetInvoiceExportModels(int companyId, List<int> invoiceIdList)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.GetInvoiceExportModels(companyId, invoiceIdList);
            }
        }

        public static AmalgamatedDataSet PopulateAmalgamatedInvoiceForPrint(int companyId, int invoiceId)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.PopulateAmalgamatedInvoiceForPrint(companyId, invoiceId);
            }
        }

        public static AmalgamatedPrintDetailDataSet PopulateAmalgamatedPrintDetail(int companyId, int invoiceId)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.PopulateAmalgamatedPrintDetail(companyId, invoiceId);
            }
        }

        public static AmalgamatedInvoice GetAmalgamatedInvoice(int companyId, int invoiceId)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.GetAmalgamatedInvoice(companyId, invoiceId);
            }
        }

        public static AmalgamatedInvoiceDetailDataSet PopulateAmalgamatedInvoiceDetail(int companyId, int invoiceId, int customerId)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.PopulateAmalgamatedInvoiceDetail(companyId, invoiceId, customerId);
            }
        }

        public static CustomerDataSet PopulateCustomers(int companyId, bool hideRip = false)
        {
            using (CustomerHelper helper = new CustomerHelper(GetConnectionString()))
            {
                return helper.PopulateCustomerData(companyId, hideRip);
            }
        }

        public static CustomerBreakDownDataSet PopulateCustomerBreakdowns(int companyId, int customerId)
        {
            using (CustomerHelper helper = new CustomerHelper(GetConnectionString()))
            {
                return helper.PopulateBreakDownForCustomer(companyId, customerId);
            }
        }

        public static CustomerDataSet PopulateDeletedCustomers(int companyId)
        {
            using (CustomerHelper helper = new CustomerHelper(GetConnectionString()))
            {
                return helper.PopulateDeletedCustomers(companyId);
            }
        }

        public static ClientDataSet PopulateClients(int companyId, bool showRip)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.PopulateClientData(companyId, showRip);
            }
        }

        public static ClientDataSet PopulateClients(int companyId, string clientName)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.GetClientList(companyId, clientName);
            }
        }

        public static ClientDataSet PopulateDeletedClients(int companyId)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.GetDeletedClients(companyId);
            }
        }

        public static ClientDataSet PopulateClientsWithoutRip(int companyId)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.PopulateClientData(companyId,false);
            }
        }

        public static BreakdownDetailDataSet GetBreakdownForClient(int clientId)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.GetBreakdownForClient(clientId);
            }
        }

        public static GlobalSetting PopulateGlobalSettings(int companyId)
        {
            using (SettingsHelper helper = new SettingsHelper(GetConnectionString()))
            {
                return helper.FetchSettings(companyId);
            }
        }

        public static ChargeDataSet PopulateCharges(int companyId)
        {
            using (ChargeHelper helper = new ChargeHelper(GetConnectionString()))
            {
                return helper.PopulateCharges(companyId);
            }
        }

        public static CompanyDataSet PopulateCompanies()
        {
            using (AdminHelper helper = new AdminHelper(GetConnectionString()))
            {
                return helper.PopulateCompanyData();
            }
        }

        public static ChargeHead GetSingleCharge(int chargeId)
        {
            using (ChargeHelper helper = new ChargeHelper(GetConnectionString()))
            {
                return helper.GetSingle(chargeId);
            }
        }

        public static Client GetSingleClient(int clientId)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.GetSingle(clientId);
            }
        }

        public static Client GetPreviousClient(int clientId)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.GetPreviousClient(clientId);
            }
        }

        public static Client GetNextClient(int clientId)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.GetNextClient(clientId);
            }
        }

        public static Client GetSingleClientFromName(string name, int companyId)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.GetSingleFromName(name, companyId);
            }
        }

        public static Customer GetSingleCustomer(int customerId)
        {
            using (CustomerHelper helper = new CustomerHelper(GetConnectionString()))
            {
                return helper.GetSingle(customerId);
            }
        }

        public static Customer GetPreviousCustomer(int customerId)
        {
            using (CustomerHelper helper = new CustomerHelper(GetConnectionString()))
            {
                return helper.GetPreviousCustomer(customerId);
            }
        }

        public static Customer GetNextCustomer(int customerId)
        {
            using (CustomerHelper helper = new CustomerHelper(GetConnectionString()))
            {
                return helper.GetNextCustomer(customerId);
            }
        }

        public static Customer GetSingleCustomerFromName(string customerName, int companyId)
        {
            using (CustomerHelper helper = new CustomerHelper(GetConnectionString()))
            {
                return helper.GetSingleFromName(customerName, companyId);
            }
        }

        public static CreditNote GetSingleCreditNote(int noteId)
        {
            using (CreditNoteHelper helper = new CreditNoteHelper(GetConnectionString()))
            {
                return helper.GetNoteSingle(noteId);
            }
        }

        public static Invoice GetInvoiceSingle(long invoiceId)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.GetInvoiceSingle(invoiceId);
            }
        }

        public static bool UpdateInvoicePrinted(long invoiceId)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.UpdatedInvoicePrinted(invoiceId);
            }
        }

        public static bool UpdateInvoicePrintedByUser(long invoiceId)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.UpdatedInvoicePrintedByUser(invoiceId);
            }
        }

        public static bool UpdateCreditNotePrinted(int noteId)
        {
            using (CreditNoteHelper helper = new CreditNoteHelper(GetConnectionString()))
            {
                return helper.UpdatedCreditNotePrinted(noteId);
            }
        }

        public static bool UpdateCreditNotePrintedByUser(int noteId)
        {
            using (CreditNoteHelper helper = new CreditNoteHelper(GetConnectionString()))
            {
                return helper.UpdatedCreditNotePrintedByUser(noteId);
            }
        }

        public static Company GetCompanySingle(int companyId)
        {
            using (AdminHelper helper = new AdminHelper(GetConnectionString()))
            {
                return helper.GetCompanySingle(companyId);
            }
        }

        public static Company GetCompanySingleForName(string companyName)
        {
            using (AdminHelper helper = new AdminHelper(GetConnectionString()))
            {
                return helper.GetSingleCompanyForName(companyName);
            }
        }

        public static bool SaveSetting(GlobalSetting setting)
        {
            using (SettingsHelper helper = new SettingsHelper(GetConnectionString()))
            {
                return helper.SaveSetting(setting);
            }
        }

        public static bool SaveCharges(ChargeHead charge, int companyId)
        {
            using (ChargeHelper helper = new ChargeHelper(GetConnectionString()))
            {
                return helper.SaveChargeHead(charge, companyId);
            }
        }

        public static bool SaveClient(Client client, int companyId)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.SaveClient(client, companyId);
            }
        }

        public static bool SaveCustomer(Customer customer, int companyId)
        {
            using (CustomerHelper helper = new CustomerHelper(GetConnectionString()))
            {
                return helper.SaveCustomer(customer, companyId);
            }
        }

        public static List<BreakDown> GetBreakdownListForClient(int clientId)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.GetBreakDownForClient(clientId);
            }
        }

        public static bool SaveBreakdownForClient(int clientId, BreakdownDetailDataSet details, int companyId)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.SaveBreakdownForClient(clientId, details, companyId);
            }
        }

        public static bool SaveCompany(Company company)
        {
            using (AdminHelper helper = new AdminHelper(GetConnectionString()))
            {
                return helper.SaveCompany(company);
            }
        }

        public static bool SaveUser(User user, List<int> companySelected)
        {
            using (AdminHelper helper = new AdminHelper(GetConnectionString()))
            {
                return helper.SaveUser(user,companySelected);
            }
        }

        public static List<Customer> GetCustomerForBreakdown(int breakdownId)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.GetCustomerForBreakdown(breakdownId);
            }
        }

        public static BreakDownDataSet PopulateBreakDowns(int companyId)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.PopulateBreakdown(companyId);
            }
        }

        public static bool SaveBreakdownEntry(int clientId,int customerId,int chargeId,decimal amount, int companyId, short invoiceCycle, bool isActive)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.SaveBreakDownEntry(clientId,customerId,chargeId,amount,companyId, invoiceCycle, isActive);
            }
        }

        public static bool DeleteBreakdownEntry(int clientId, int companyId)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.DeleteBreakdown(clientId, companyId);
            }
        }

        public static int CheckBreakDown(int companyId)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.CheckBreakDown(companyId);
            }
        }

        public static bool GenerateInvoices(int clientId, DateTime invoiceDate, int days, int companyId,string narration, DateTime startDate, DateTime endDate)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.GenerateInvoices(clientId, invoiceDate, days, companyId,narration,startDate,endDate);
            }
        }

        public static bool SaveInvoiceDetail(long invoiceId, InvoiceDetailDataSet detail,string narration, DateTime startDate, DateTime endDate, bool historyInvoice)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.SaveInvoiceEntries(invoiceId, detail,narration, startDate, endDate, historyInvoice);
            }
        }

        public static InvoiceDataSet PopulateInvoices(int companyId)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.PopulateInvoiceData(companyId);
            }
        }

        public static InvoiceDetailDataSet PopulateInvoiceData(long invoiceId)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.PopulateInvoiceData(invoiceId);
            }
        }

        public static CreditNotePrintDataSet PopulateCreditNoteData(int noteId, int clientId,int companyId)
        {
            using (CreditNoteHelper helper = new CreditNoteHelper(GetConnectionString()))
            {
                return helper.PopulateCreditNotePrint(noteId,clientId,companyId);
            }
        }

        public static InvoicePrintDataSet PopulatePrintData(long invoiceId, int customerId,int companyId)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.PopulatePrint(invoiceId,customerId,companyId);
            }
        }

        public static InvoiceBreakDownDataSet PopulateInvoiceBreakdown(long invoiceId)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.PopulateInvoiceBreakdown(invoiceId);
            }
        }

        public static List<int> GetCustomersForInvoice(long invoiceId)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.GetCustomerForInvoice(invoiceId);
            }
        }

        public static InvoiceHistoryDataSet PopulateInvoiceHistory(int companyId)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.PopulateInvoiceHistory(companyId);
            }
        }

        public static CreditNoteDataSet PopulateCreditNoteData(int companyId)
        {
            using (CreditNoteHelper helper = new CreditNoteHelper(GetConnectionString()))
            {
                return helper.PopulateCreditNote(companyId);
            }
        }

        public static User GetUserSingle(int userId)
        {
            using (AdminHelper helper = new AdminHelper(GetConnectionString()))
            {
                return helper.GetUserSingle(userId);
            }
        }

        public static User GetUserSingleFromName(string userName)
        {
            using (AdminHelper helper = new AdminHelper(GetConnectionString()))
            {
                return helper.GetUserSingleFromName(userName);
            }
        }

        public static List<int> GetAssociatedCompanies(int userId)
        {
            using (AdminHelper helper = new AdminHelper(GetConnectionString()))
            {
                return helper.GetAssociatedCompanies(userId);
            }
        }

        public static UserDataSet PopulateUsers()
        {
            using (AdminHelper helper = new AdminHelper(GetConnectionString()))
            {
                return helper.PopulateUsers();
            }
        }

        public static bool SaveCreditNote(CreditNote noteDetails, int companyId)
        {
            using (CreditNoteHelper helper = new CreditNoteHelper(GetConnectionString()))
            {
                return helper.SaveCreditNote(noteDetails, companyId);
            }
        }

        public static bool IsUserAuthenticated(string userName, string password)
        {
            using (AdminHelper helper = new AdminHelper(GetConnectionString()))
            {
                return helper.IsUserAuthenticated(userName,password);
            }
        }

        public static bool IsUserAuthorized(string userName, string password, int companyId)
        {
            using (AdminHelper helper = new AdminHelper(GetConnectionString()))
            {
                return helper.IsUserAuthorized(userName, password,companyId);
            }
        }

        public static InvoiceCsvDataSet PopulateInvoiceExport(DateTime startDate, DateTime endDate, bool includeInvoice, bool includeCreditNote, int companyId, bool includeExported)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.GenerateExportData(startDate,endDate,includeInvoice,includeCreditNote,companyId,includeExported);
            }
        }

        public static bool DeleteInvoice(long invoiceId)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.DeleteInvoice(invoiceId);
            }
        }

        public static bool DeleteCreditNote(int noteId)
        {
            using (CreditNoteHelper helper = new CreditNoteHelper(GetConnectionString()))
            {
                return helper.DeleteCreditNote(noteId);
            }
        }

        public static bool DeleteCharge(int chargeId)
        {
            using (ChargeHelper helper = new ChargeHelper(GetConnectionString()))
            {
                return helper.DeleteCharge(chargeId);
            }
        }

        public static bool DeleteClient(int clientId,int companyId)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.DeleteClient(clientId,companyId);
            }
        }

        public static bool DeleteCustomer(int customerId)
        {
            using (CustomerHelper helper = new CustomerHelper(GetConnectionString()))
            {
                return helper.DeleteCustomer(customerId);
            }
        }

        public static string GenerateCreditNoteNumber(int companyId)
        {
            using (CreditNoteHelper helper = new CreditNoteHelper(GetConnectionString()))
            {
                return helper.GenerateCreditNoteNumber(companyId);
            }
        }

        public static string GenerateInvoiceNumber(int companyId)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.GenerateInvoiceNumber(companyId);
            }
        }

        public static bool InvoiceExists(int companyId, string invoiceNumber)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.InvoiceExists(companyId,invoiceNumber);
            }
        }

        public static CustomerDataSet PopulateCustomerForClient(int companyId, int clientId)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.PopulateCustomerForClient(companyId,clientId);
            }
        }

        public static bool SaveManualInvoice(Invoice invoice, List<InvoiceDetail> invoiceDetail)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.SaveManualInvoice(invoice, invoiceDetail);
            }
        }

        public static int GetChargeId(string chargeName)
        {
            using (ChargeHelper helper = new ChargeHelper(GetConnectionString()))
            {
                return helper.GetChargeId(chargeName);
            }
        }

        public static decimal GetWeeklyRate(int companyId, int clientId, int customerId)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.GetWeeklyRate(companyId,clientId,customerId);
            }
        }

        public static ClientDataSet PopulateDeletedClient(int companyId)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.PopulateDeletedClients(companyId);
            }
        }

        public static bool ResetInvoice(int invoiceNumber)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.ResetInvoiceNumber(invoiceNumber);
            }
        }

        public static bool DeleteInvoiceWithAdjust()
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.DeleteInvoiceWithAdjust();
            }
        }

        public static bool MarkCustomerActiveInBreakdown(int customerId, bool active)
        {
            using (CustomerHelper helper = new CustomerHelper(GetConnectionString()))
            {
                return helper.MarkCustomerActiveInBreakdown(customerId, active);
            }
        }

        public static bool MarkCustomerInactiveInAllBreakdowns(int companyId)
        {
            using (CustomerHelper helper = new CustomerHelper(GetConnectionString()))
            {
                return helper.MarkCustomerInactiveInAllBreakdown(companyId);
            }
        }

        public static List<Client> CheckClientWithRip(int companyId, DateTime invoiceDate)
        {
            using (InvoiceHelper helper = new InvoiceHelper(GetConnectionString()))
            {
                return helper.CheckClientWithRip(companyId, invoiceDate);
            }
        }

        public static bool SaveLetterContent(int companyId, string letterContent)
        {
            using (SettingsHelper helper = new SettingsHelper(GetConnectionString()))
            {
                return helper.SaveLetterContent(companyId, letterContent);
            }
        }

        public static List<Customer> GetCustomersForClient(int companyId, int clientId)
        {
            using (ClientHelper helper = new ClientHelper(GetConnectionString()))
            {
                return helper.GetCustomersForClient(companyId, clientId);
            }
        }

        public static bool IsCustomerLinkedToActiveClient(string customerEmail, int companyId)
        {
            using (CustomerHelper helper = new CustomerHelper(GetConnectionString()))
            {
                return helper.IsCustomerLinkedWithActiveClient(customerEmail, companyId);
            }
        }

        public static List<CustomerFeeModel> GetAllActiveCustomersInBreakdowns(int companyId, List<int> selectedClients)
        {
            using (var invoiceHelper = new InvoiceHelper(GetConnectionString()))
            {
                return invoiceHelper.GetAllActiveCustomersInBreakdowns(companyId, selectedClients);
            }
        }

        public static decimal GetBreakdownValueForCustomer(int companyId, int clientId, int customerId, int chargeHeadId)
        {
            using (var invoiceHelper = new InvoiceHelper(GetConnectionString()))
            {
                return invoiceHelper.GetBreakdownValueForCustomer(companyId, clientId, customerId, chargeHeadId);
            }
        }

        #endregion

        #region Database update

        public static bool UpdateDatabase()
        {
            using (DatabaseHelper helper = new DatabaseHelper(GetConnectionString()))
            {
                return helper.UpdateDatabase();
            }
        }

        public static bool RestoreDatabase(string backupFile, string databaseName)
        {
            using (var databaseHelper = new DatabaseHelper(GetConnectionString()))
            {
                return databaseHelper.RestoreDatabase(backupFile, databaseName);
            }
        }

        #endregion

        #region Private methods

        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        }

        #endregion
    }
}
