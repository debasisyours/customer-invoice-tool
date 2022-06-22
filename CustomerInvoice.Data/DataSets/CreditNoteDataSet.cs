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
    public class CreditNoteDataSet:DataSet,ISerializable
    {
        #region Constant declaration

        public const string TableCreditNote = "CreditNote";

        public const string IdColumn = "Id";
        public const string ClientIdColumn = "ClientId";
        public const string ClientNameColumn = "ClientName";
        public const string ClientAddressColumn = "ClientAddress";
        public const string TransactionNumberColumn = "TransactionNumber";
        public const string TransactionDateColumn = "TransactionDate";
        public const string DescriptionColumn = "Description";
        public const string AmountColumn = "Amount";
        public const string CustomerIdColumn = "CustomerId";
        public const string CustomerNameColumn = "CustomerName";

        #endregion

        #region Constructor

        public CreditNoteDataSet()
        {
            this.Locale = CultureInfo.CurrentCulture;
            this.BuildCreditNoteTable();
        }

        protected CreditNoteDataSet(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region Private methods

        private void BuildCreditNoteTable()
        {
            DataTable table = new DataTable(TableCreditNote);
            table.Locale = CultureInfo.CurrentCulture;

            DataColumn column = new DataColumn(IdColumn);
            column.Caption = IdColumn;
            column.DataType = typeof(int);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(ClientIdColumn);
            column.Caption = ClientIdColumn;
            column.DataType = typeof(int);
            column.DefaultValue = 0;
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

            column = new DataColumn(TransactionNumberColumn);
            column.Caption = TransactionNumberColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(TransactionDateColumn);
            column.Caption = TransactionDateColumn;
            column.DataType = typeof(DateTime);
            column.DefaultValue = null;
            table.Columns.Add(column);

            column = new DataColumn(DescriptionColumn);
            column.Caption = DescriptionColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(AmountColumn);
            column.Caption = AmountColumn;
            column.DataType = typeof(decimal);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(CustomerIdColumn);
            column.Caption = CustomerIdColumn;
            column.DataType = typeof(int);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(CustomerNameColumn);
            column.Caption = CustomerNameColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            this.Tables.Add(table);
        }

        #endregion
    }
}
