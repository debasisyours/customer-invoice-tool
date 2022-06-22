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
    public class InvoiceHistoryDataSet:DataSet,ISerializable
    {
        #region Const

        public const string TableInvoiceHistory = "InvoiceHistory";

        public const string SelectColumn = "Select";
        public const string IdColumn = "Id";
        public const string InvoiceNumberColumn = "InvoiceNumber";
        public const string InvoiceDateColumn = "InvoiceDate";
        public const string ClientCodeColumn = "ClientCode";
        public const string ClientNameColumn = "ClientName";
        public const string CustomerCodeColumn = "CustomerCode";
        public const string CustomerNameColumn = "CustomerName";
        public const string ChargeNameColumn = "ChargeName";
        public const string WeeklyRateColumn = "WeeklyRate";
        public const string DaysColumn = "Days";
        public const string SubTotalColumn = "SubTotal";
        public const string ExtraHeadColumn = "ExtraHead";
        public const string ExtraAmountColumn = "ExtraAmount";
        public const string LessHeadColumn = "LessHead";
        public const string LessAmountColumn = "LessAmount";
        public const string NetAmountColumn = "NetAmount";
        public const string ChargePeriodColumn = "ChargePeriod";
        public const string InvoiceTypeColumn = "InvoiceType";
        public const string DeletedColumn = "Deleted";
        public const string MultiMonthColumn = "MultiMonth";

        #endregion

        #region Constructor

        public InvoiceHistoryDataSet()
        {
            this.Locale = CultureInfo.CurrentCulture;
            this.BuildHistoryDataTable();
        }

        protected InvoiceHistoryDataSet(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region Private methods

        private void BuildHistoryDataTable()
        {
            DataTable table = new DataTable(TableInvoiceHistory);
            table.Locale = CultureInfo.CurrentCulture;

            DataColumn column = new DataColumn(IdColumn);
            column.Caption = IdColumn;
            column.DataType = typeof(long);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(SelectColumn);
            column.Caption = SelectColumn;
            column.DataType = typeof(bool);
            column.DefaultValue = false;
            table.Columns.Add(column);

            column = new DataColumn(InvoiceNumberColumn);
            column.Caption = InvoiceNumberColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(InvoiceDateColumn);
            column.Caption = InvoiceDateColumn;
            column.DataType = typeof(DateTime);
            column.DefaultValue = null;
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

            column = new DataColumn(ChargeNameColumn);
            column.Caption = ChargeNameColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(WeeklyRateColumn);
            column.Caption = WeeklyRateColumn;
            column.DataType = typeof(decimal);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(DaysColumn);
            column.Caption = DaysColumn;
            column.DataType = typeof(int);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(SubTotalColumn);
            column.Caption = SubTotalColumn;
            column.DataType = typeof(decimal);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(ExtraHeadColumn);
            column.Caption = ExtraHeadColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(ExtraAmountColumn);
            column.Caption = ExtraAmountColumn;
            column.DataType = typeof(decimal);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(LessHeadColumn);
            column.Caption = LessHeadColumn;
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

            column = new DataColumn(ChargePeriodColumn);
            column.Caption = ChargePeriodColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(InvoiceTypeColumn);
            column.Caption = InvoiceTypeColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(DeletedColumn);
            column.Caption = DeletedColumn;
            column.DataType = typeof(bool);
            column.DefaultValue = false;
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
