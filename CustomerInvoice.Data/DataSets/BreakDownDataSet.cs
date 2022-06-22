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
    public class BreakDownDataSet:DataSet,ISerializable
    {
        #region Const

        public const string TableBreakDown = "Breakdown";

        public const string IdColumn = "Id";
        public const string ClientIdColumn = "ClientId";
        public const string ClientCodeColumn = "ClientCode";
        public const string ClientNameColumn = "ClientName";
        public const string CustomerIdColumn = "CustomerId";
        public const string CustomerCodeColumn = "CustomerCode";
        public const string CustomerNameColumn = "CustomerName";
        public const string ChargeHeadIdColumn = "ChargeHeadId";
        public const string ChargeHeadNameColumn = "ChargeHeadName";
        public const string AmountColumn = "Amount";
        public const string InvoiceCycleColumn = "InvoiceCycle";
        public const string IsActiveColumn = "IsActive";

        #endregion

        #region Constructor

        public BreakDownDataSet()
        {
            this.Locale = CultureInfo.CurrentCulture;
            this.BuildBreakdownTable();
        }

        protected BreakDownDataSet(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region Private methods

        private void BuildBreakdownTable()
        {
            DataTable table = new DataTable(TableBreakDown);
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

            column = new DataColumn(ClientCodeColumn);
            column.Caption = ClientCodeColumn;
            column.DataType = typeof(long);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(ClientNameColumn);
            column.Caption = ClientNameColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(CustomerIdColumn);
            column.Caption = CustomerIdColumn;
            column.DataType = typeof(int);
            column.DefaultValue = 0;
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

            column = new DataColumn(ChargeHeadIdColumn);
            column.Caption = ChargeHeadIdColumn;
            column.DataType = typeof(int);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(ChargeHeadNameColumn);
            column.Caption = ChargeHeadNameColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(AmountColumn);
            column.Caption = AmountColumn;
            column.DataType = typeof(decimal);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(InvoiceCycleColumn);
            column.Caption = InvoiceCycleColumn;
            column.DataType = typeof(short);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(IsActiveColumn);
            column.Caption = IsActiveColumn;
            column.DataType = typeof(bool);
            column.DefaultValue = false;
            table.Columns.Add(column);

            this.Tables.Add(table);
        }

        #endregion
    }
}
