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
    public class BreakdownDetailDataSet:DataSet,ISerializable
    {
        #region Constant

        public const string TableBreakdownDetail = "BreakDownDetail";

        public const string CustomerIdColumn = "CustomerId";
        public const string CustomerCodeColumn = "CustomerCode";
        public const string CustomerNameColumn = "CustomerName";
        public const string ChargeHeadIdColumn = "ChargeHeadId";
        public const string ChargeHeadNameColumn = "ChargeHeadName";
        public const string RateColumn = "Rate";
        public const string InvoiceCycleColumn = "InvoiceCycle";

        #endregion

        #region Constructor

        public BreakdownDetailDataSet()
        {
            this.Locale = CultureInfo.CurrentCulture;
            this.BuildBreakdownTable();
        }

        protected BreakdownDetailDataSet(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region Private methods

        private void BuildBreakdownTable()
        {
            DataTable table = new DataTable(TableBreakdownDetail);
            table.Locale = CultureInfo.CurrentCulture;

            DataColumn column = new DataColumn(CustomerIdColumn);
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

            column = new DataColumn(RateColumn);
            column.Caption = RateColumn;
            column.DataType = typeof(double);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(InvoiceCycleColumn);
            column.Caption = InvoiceCycleColumn;
            column.DataType = typeof(short);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            this.Tables.Add(table);
        }

        #endregion
    }
}
