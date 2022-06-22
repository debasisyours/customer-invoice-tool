using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Runtime.Serialization;
using System.Data;
using CustomerInvoice.Common;
using CustomerInvoice.Data;
using CustomerInvoice.Data.DataSets;

namespace CustomerInvoice.Data.Helpers
{
    public class AdminHelper:IDisposable
    {

        #region Private field declaration

        private string _ConnectionString = string.Empty;

        #endregion

        #region Constructor

        public AdminHelper(string connectionString)
        {
            this._ConnectionString = connectionString;
        }

        #endregion

        #region Methods

        protected internal CompanyDataSet PopulateCompanyData()
        {
            CompanyDataSet companyData = new CompanyDataSet();
            DataRow row = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    List<Company> companyList = context.Companies.ToList();
                    foreach (Company companyItem in companyList)
                    {
                        row = companyData.Tables[CompanyDataSet.TableCompany].NewRow();
                        row[CompanyDataSet.AddressColumn] = companyItem.Address;
                        row[CompanyDataSet.CityColumn] = companyItem.City;
                        row[CompanyDataSet.CodeColumn] = companyItem.Code;
                        row[CompanyDataSet.CountryColumn] = companyItem.Country;
                        row[CompanyDataSet.EmailColumn] = companyItem.Email;
                        row[CompanyDataSet.FaxColumn] = companyItem.Fax;
                        row[CompanyDataSet.IdColumn] = companyItem.ID;
                        row[CompanyDataSet.NameColumn] = companyItem.Name;
                        row[CompanyDataSet.PhoneColumn] = companyItem.Phone;
                        row[CompanyDataSet.StateColumn] = companyItem.State;
                        row[CompanyDataSet.UrlColumn] = companyItem.URL;
                        row[CompanyDataSet.ZipColumn] = companyItem.ZIP;
                        companyData.Tables[CompanyDataSet.TableCompany].Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return companyData;
        }

        protected internal Company GetCompanySingle(int companyId)
        {
            Company selected = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    selected = context.Companies.Where(s => s.ID == companyId).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return selected;
        }

        protected internal bool SaveCompany(Company company)
        {
            bool success = false;
            bool newRecord = false;
            Company currentCompany = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    currentCompany = context.Companies.Where(s => s.ID == company.ID).SingleOrDefault();
                    if (currentCompany == null)
                    {
                        currentCompany = new Company();
                        newRecord = true;
                    }
                    currentCompany.Address = company.Address;
                    currentCompany.City = company.City;
                    currentCompany.Code = company.Code;
                    currentCompany.Country = company.Country;
                    currentCompany.Email = company.Email;
                    currentCompany.Fax = company.Fax;
                    currentCompany.Name = company.Name;
                    currentCompany.Phone = company.Phone;
                    currentCompany.State = company.State;
                    currentCompany.URL = company.URL;
                    currentCompany.ZIP = company.ZIP;
                    currentCompany.AccountCode = company.AccountCode;
                    currentCompany.AccountNumber = company.AccountNumber;

                    if (newRecord)
                    {
                        context.Companies.InsertOnSubmit(currentCompany);
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

        protected internal Company GetSingleCompanyForName(string companyName)
        {
            Company company = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    company = context.Companies.Where(s => string.Compare(companyName, s.Name, false) == 0).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return company;
        }

        protected internal UserDataSet PopulateUsers()
        {
            UserDataSet usersData = new UserDataSet();
            DataRow row = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    List<User> userList = context.Users.ToList();
                    foreach (User userItem in userList)
                    {
                        row = usersData.Tables[UserDataSet.TableUsers].NewRow();
                        row[UserDataSet.IdColumn] = userItem.ID;
                        row[UserDataSet.IsActiveColumn] = userItem.IsActive;
                        row[UserDataSet.NameColumn] = userItem.Name;
                        row[UserDataSet.CompanyCountColumn] = context.CompanyUsers.Where(s => s.UserId == userItem.ID).Count();
                        usersData.Tables[UserDataSet.TableUsers].Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return usersData;
        }

        protected internal User GetUserSingle(int userId)
        {
            User userDetail = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    userDetail = context.Users.Where(s => s.ID == userId).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return userDetail;
        }

        protected internal User GetUserSingleFromName(string userName)
        {
            User userDetail = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    userDetail = context.Users.Where(s => string.Compare(s.Name,userName,false)==0).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return userDetail;
        }

        protected internal List<int> GetAssociatedCompanies(int userId)
        {
            List<int> companies = new List<int>();
            try
            {
                using(InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    List<CompanyUser> companyList = context.CompanyUsers.Where(s => s.UserId == userId).ToList();
                    foreach (CompanyUser companyItem in companyList)
                    {
                        companies.Add(companyItem.CompanyId.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return companies;
        }

        protected internal bool SaveUser(User user, List<int> companySelected)
        {
            bool success = false;
            bool newRecord = false;
            int userId = 0;
            User currentUser = null;
            CompanyUser userMapping = null;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    currentUser = context.Users.Where(s => s.ID == user.ID).SingleOrDefault();
                    if (currentUser == null)
                    {
                        newRecord = true;
                        currentUser = new User();
                    }

                    currentUser.Name = user.Name;
                    currentUser.Password = user.Password;
                    currentUser.IsActive = user.IsActive;
                    if (newRecord)
                    {
                        context.Users.InsertOnSubmit(currentUser);
                    }
                    context.SubmitChanges();

                    userId = currentUser.ID;
                    foreach (CompanyUser userMap in context.CompanyUsers.Where(s => s.UserId == userId).ToList())
                    {
                        context.CompanyUsers.DeleteOnSubmit(userMap);
                    }

                    context.SubmitChanges();

                    foreach (int companyId in companySelected)
                    {
                        userMapping = new CompanyUser();
                        userMapping.UserId = userId;
                        userMapping.CompanyId = companyId;
                        context.CompanyUsers.InsertOnSubmit(userMapping);
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

        protected internal bool IsUserAuthenticated(string userName, string password)
        {
            bool valid = false;
            User user = null;
            using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
            {
                user = context.Users.Where(s => (string.Compare(s.Name, userName, false) == 0) && (string.Compare(s.Password, password, false) == 0) && (s.IsActive)).SingleOrDefault();
                valid = (user != null);
            }
            return valid;
        }

        protected internal bool IsUserAuthorized(string userName, string password, int companyId)
        {
            bool valid = false;
            User user = null;
            using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
            {
                user = context.Users.Where(s => (string.Compare(s.Name, userName, false) == 0) && (string.Compare(s.Password, password, false) == 0) && (s.IsActive)).SingleOrDefault();
                List<CompanyUser> companies = context.CompanyUsers.Where(s => s.UserId == user.ID).ToList();
                foreach (CompanyUser companyItem in companies)
                {
                    if (companyItem.UserId == user.ID)
                    {
                        valid = true;
                        break;
                    }
                }
            }
            return valid;
        }

        #endregion

        #region Interface Implementation

        void IDisposable.Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
