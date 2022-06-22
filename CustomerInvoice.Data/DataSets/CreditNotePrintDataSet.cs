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
    public class CreditNotePrintDataSet:DataSet, ISerializable
    {
        #region Const

        public const string TableCreditNotePrint = "CreditNotePrint";

        public const string CompanyNameColumn = "CompanyName";
        public const string CompanyAddressColumn = "CompanyAddress";
        public const string AccountNameColumn = "AccountName";
        public const string AccountNumberColumn = "AccountNumber";
        public const string SortCodeColumn = "SortCode";

        public const string ClientCodeColumn = "ClientCode";
        public const string ClientNameColumn = "ClientName";
        public const string ClientAddressColumn = "ClientAddress";

        public const string CustomerCodeColumn = "CustomerCode";
        public const string CustomerNameColumn = "CustomerName";
        public const string CustomerAddressColumn = "CustomerAddress";

        public const string CreditNoteNumberColumn = "NoteNumber";
        public const string CreditNoteDateColumn = "NoteDate";
        public const string NetAmountColumn = "NetAmount";
        public const string DescriptionColumn = "Description";

        public const string CompanyCodeColumn = "CompanyCode";

        #endregion

        #region Constructor

        public CreditNotePrintDataSet()
        {
            this.Locale = CultureInfo.CurrentCulture;
            this.BuildCreditNoteDataTable();
        }

        protected CreditNotePrintDataSet(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region Private methods

        private void BuildCreditNoteDataTable()
        {
            DataTable table = new DataTable(TableCreditNotePrint);
            table.Locale = CultureInfo.CurrentCulture;

            DataColumn column = new DataColumn(CompanyNameColumn);
            column.Caption = CompanyNameColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(CompanyAddressColumn);
            column.Caption = CompanyAddressColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(AccountNameColumn);
            column.Caption = AccountNameColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(AccountNumberColumn);
            column.Caption = AccountNumberColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(SortCodeColumn);
            column.Caption = SortCodeColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(ClientCodeColumn);
            column.Caption = ClientCodeColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(ClientNameColumn);
            column.Caption = ClientNameColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(ClientAddressColumn);
            column.Caption = ClientAddressColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(CustomerCodeColumn);
            column.Caption = CustomerCodeColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(CustomerNameColumn);
            column.Caption = CustomerNameColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(CustomerAddressColumn);
            column.Caption = CustomerAddressColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(CreditNoteNumberColumn);
            column.Caption = CreditNoteNumberColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(CreditNoteDateColumn);
            column.Caption = CreditNoteDateColumn;
            column.DataType = typeof(DateTime);
            column.DefaultValue = null;
            table.Columns.Add(column);

            column = new DataColumn(NetAmountColumn);
            column.Caption = NetAmountColumn;
            column.DataType = typeof(decimal);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(DescriptionColumn);
            column.Caption = DescriptionColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(CompanyCodeColumn);
            column.Caption = CompanyCodeColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            this.Tables.Add(table);
        }

        #endregion
    }
}
