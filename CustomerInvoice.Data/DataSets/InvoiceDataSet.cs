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
    public class InvoiceDataSet:DataSet,ISerializable
    {
        #region Constants

        public const string TableInvoice = "Invoice";

        public const string IdColumn = "Id";
        public const string InvoiceNumberColumn = "InvoiceNumber";
        public const string InvoiceDateColumn = "InvoiceDate";
        public const string ClientNameColumn = "ClientName";
        public const string ClientIdColumn = "ClientId";
        public const string NetAmountColumn = "NetAmount";
        public const string MultiMonthColumn = "MultiMonth";

        #endregion

        #region Constructor

        public InvoiceDataSet()
        {
            this.Locale = CultureInfo.CurrentCulture;
            this.BuildInvoiceTable();
        }

        protected InvoiceDataSet(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region Private functions

        private void BuildInvoiceTable()
        {
            DataTable table = new DataTable(TableInvoice);
            table.Locale = CultureInfo.CurrentCulture;

            DataColumn column = new DataColumn(IdColumn);
            column.Caption = IdColumn;
            column.DataType = typeof(long);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(InvoiceNumberColumn);
            column.Caption = InvoiceNumberColumn;
            column.DataType = typeof(long);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(InvoiceDateColumn);
            column.Caption = InvoiceDateColumn;
            column.DataType = typeof(DateTime);
            column.DefaultValue = null;
            table.Columns.Add(column);

            column = new DataColumn(ClientNameColumn);
            column.Caption = ClientNameColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(ClientIdColumn);
            column.Caption = ClientIdColumn;
            column.DataType = typeof(int);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(NetAmountColumn);
            column.Caption = NetAmountColumn;
            column.DataType = typeof(double);
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
