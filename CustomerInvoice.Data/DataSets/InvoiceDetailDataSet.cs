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
    public class InvoiceDetailDataSet:DataSet,ISerializable
    {
        #region Consts

        public const string TableInvoiceDetail = "InvoiceDetail";

        public const string IdColumn = "Id";
        public const string CustomerIdColumn = "CustomerId";
        public const string CustomerNameColumn = "CustomerName";
        public const string ChargeHeadIdColumn = "ChargeHeadId";
        public const string ChargeHeadNameColumn = "ChargeHeadName";
        public const string WeeklyRateColumn = "WeeklyRate";
        public const string TotalDaysColumn = "TotalDays";
        public const string SubTotalColumn = "SubTotal";
        public const string ExtraPayHeadColumn = "ExtraPayHead";
        public const string ExtraAmountColumn = "ExtraAmount";
        public const string LessPayHeadColumn = "LessPayHead";
        public const string LessAmountColumn = "LessAmount";
        public const string NetAmountColumn = "NetAmount";

        #endregion

        #region Constructor

        public InvoiceDetailDataSet()
        {
            this.Locale = CultureInfo.CurrentCulture;
            this.BuildInvoiceDetailTable();
        }

        protected InvoiceDetailDataSet(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region Private functions

        private void BuildInvoiceDetailTable()
        {
            DataTable table = new DataTable(TableInvoiceDetail);
            table.Locale = CultureInfo.CurrentCulture;

            DataColumn column = new DataColumn(IdColumn);
            column.Caption = IdColumn;
            column.DataType = typeof(long);
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

            column = new DataColumn(WeeklyRateColumn);
            column.Caption = WeeklyRateColumn;
            column.DataType = typeof(decimal);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(TotalDaysColumn);
            column.Caption = TotalDaysColumn;
            column.DataType = typeof(int);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(SubTotalColumn);
            column.Caption = SubTotalColumn;
            column.DataType = typeof(decimal);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(ExtraPayHeadColumn);
            column.Caption = ExtraPayHeadColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(ExtraAmountColumn);
            column.Caption = ExtraAmountColumn;
            column.DataType = typeof(decimal);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(LessPayHeadColumn);
            column.Caption = LessPayHeadColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(LessAmountColumn);
            column.Caption = LessAmountColumn;
            column.DataType = typeof(decimal);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(NetAmountColumn);
            column.Caption = NetAmountColumn;
            column.DataType = typeof(decimal);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            this.Tables.Add(table);
        }

        #endregion
    }
}
