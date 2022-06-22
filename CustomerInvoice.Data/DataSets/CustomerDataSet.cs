using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using System.Runtime.Serialization;

namespace CustomerInvoice.Data.DataSets
{
    [Serializable]
    public class CustomerDataSet:DataSet,ISerializable
    {
        #region Constants

        public const string TableCustomer = "Customer";

        public const string IdColumn = "Id";
        public const string CodeColumn = "Code";
        public const string NameColumn = "Name";
        public const string AddressColumn = "Address";
        public const string EmailColumn = "Email";
        public const string PhoneColumn = "Phone";
        public const string SageReferenceColumn = "SageReference";
        public const string ClientCodeColumn = "ClientCode";
        public const string SelectedColumn = "Selected";
        public const string HasRipColumn = "HasRip";

        #endregion

        #region Constructor

        public CustomerDataSet()
        {
            this.Locale = CultureInfo.CurrentCulture;
            this.BuildCustomerTable();
        }

        protected CustomerDataSet(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region Private functions

        private void BuildCustomerTable()
        {
            DataTable table = new DataTable(TableCustomer);
            table.Locale = CultureInfo.CurrentCulture;

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

            column = new DataColumn(PhoneColumn);
            column.Caption = PhoneColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(EmailColumn);
            column.Caption = EmailColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(SageReferenceColumn);
            column.Caption = SageReferenceColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(ClientCodeColumn);
            column.Caption = ClientCodeColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(SelectedColumn);
            column.Caption = SelectedColumn;
            column.DataType = typeof(bool);
            column.DefaultValue = false;
            table.Columns.Add(column);

            column = new DataColumn(HasRipColumn);
            column.Caption = HasRipColumn;
            column.DataType = typeof(bool);
            column.DefaultValue = false;
            table.Columns.Add(column);

            this.Tables.Add(table);
        }

        #endregion
    }
}
