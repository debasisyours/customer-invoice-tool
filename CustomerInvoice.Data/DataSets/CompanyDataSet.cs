using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Data;
using System.Runtime.Serialization;

namespace CustomerInvoice.Data.DataSets
{
    [Serializable]
    public class CompanyDataSet:DataSet,ISerializable
    {
        #region Consts

        public const string TableCompany = "Company";

        public const string IdColumn = "Id";
        public const string CodeColumn = "Code";
        public const string NameColumn = "Name";
        public const string AddressColumn = "Address";
        public const string CityColumn = "City";
        public const string StateColumn = "State";
        public const string CountryColumn = "Country";
        public const string ZipColumn = "Zip";
        public const string PhoneColumn = "Phone";
        public const string FaxColumn = "Fax";
        public const string EmailColumn = "Email";
        public const string UrlColumn = "URL";

        #endregion

        #region Constructor

        public CompanyDataSet()
        {
            this.Locale = CultureInfo.CurrentCulture;
            this.BuildCompanyDataTable();
        }

        protected CompanyDataSet(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region Custom methods

        private void BuildCompanyDataTable()
        {
            DataTable table = new DataTable(TableCompany);

            DataColumn column = new DataColumn(IdColumn);
            column.Caption = IdColumn;
            column.DataType = typeof(int);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(CodeColumn);
            column.Caption = CodeColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(NameColumn);
            column.Caption = NameColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(AddressColumn);
            column.Caption = AddressColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(CityColumn);
            column.Caption = CityColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(StateColumn);
            column.Caption = StateColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(CountryColumn);
            column.Caption = CountryColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(ZipColumn);
            column.Caption = ZipColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(PhoneColumn);
            column.Caption = PhoneColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(FaxColumn);
            column.Caption = FaxColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(EmailColumn);
            column.Caption = EmailColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(UrlColumn);
            column.Caption = UrlColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            this.Tables.Add(table);
        }

        #endregion
    }
}
