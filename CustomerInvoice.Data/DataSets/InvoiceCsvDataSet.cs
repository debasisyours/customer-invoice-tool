using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Runtime.Serialization;
using System.Globalization;

namespace CustomerInvoice.Data.DataSets
{
    [Serializable]
    public class InvoiceCsvDataSet:DataSet,ISerializable
    {
        #region Const

        public const string TableInvoiceExport = "InvoiceExport";

        public const string TypeColumn = "Type";
        public const string SageReferenceColumn = "SageReference";
        public const string AccountCodeColumn = "AccountCode";
        public const string AccountNumberColumn = "AccountNumber";
        public const string TransactionDateColumn = "TransactionDate";
        public const string TransactionNumberColumn = "TransactionNumber";
        public const string NarrationColumn = "Narration";
        public const string InvoiceAmountColumn = "InvoiceAmount";
        public const string TaxCodeColumn = "TaxCode";
        public const string TaxAmountColumn = "TaxAmount";
        public const string ClientCodeColumn = "ClientCode";
        public const string ClientNameColumn = "ClientName";
        public const string ExportFormatColumn = "ExportFormat";
        public const string PrintColumn = "Print";
        public const string InternalIdColumn = "InternalId";
        public const string MultiMonthColumn = "MultiMonth";

        #endregion

        #region Constructor

        public InvoiceCsvDataSet()
        {
            this.Locale = CultureInfo.CurrentCulture;
            this.BuildInvoiceDataTable();
        }

        protected InvoiceCsvDataSet(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region Private methods

        private void BuildInvoiceDataTable()
        {
            DataTable table = new DataTable(TableInvoiceExport);
            table.Locale = CultureInfo.CurrentCulture;

            DataColumn column = new DataColumn(TypeColumn);
            column.Caption = TypeColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(SageReferenceColumn);
            column.Caption = SageReferenceColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(AccountCodeColumn);
            column.Caption = AccountCodeColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(AccountNumberColumn);
            column.Caption = AccountNumberColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(TransactionDateColumn);
            column.Caption = TransactionDateColumn;
            column.DataType = typeof(DateTime);
            column.DefaultValue = null;
            table.Columns.Add(column);

            column = new DataColumn(TransactionNumberColumn);
            column.Caption = TransactionNumberColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(NarrationColumn);
            column.Caption = NarrationColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(InvoiceAmountColumn);
            column.Caption = InvoiceAmountColumn;
            column.DataType = typeof(decimal);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(TaxCodeColumn);
            column.Caption = TaxCodeColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(TaxAmountColumn);
            column.Caption = TaxAmountColumn;
            column.DataType = typeof(decimal);
            column.DefaultValue = 0;
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

            column = new DataColumn(ExportFormatColumn);
            column.Caption = ExportFormatColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(PrintColumn);
            column.Caption = PrintColumn;
            column.DataType = typeof(bool);
            column.DefaultValue = false;
            table.Columns.Add(column);

            column = new DataColumn(InternalIdColumn);
            column.Caption = InternalIdColumn;
            column.DataType = typeof(int);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(MultiMonthColumn);
            column.Caption = MultiMonthColumn;
            column.DataType = typeof(bool);
            column.DefaultValue = false;
            table.Columns.Add(column);

            this.Tables.Add(table);
        }

        #endregion
    }
}
