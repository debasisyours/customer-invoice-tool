using System.Data;
using System.Globalization;

namespace CustomerInvoice.Data.DataSets
{
    public class AmalgamatedPrintDetailDataSet : DataSet
    {
        #region Declaration

        public const string TableInvoiceDetail = "InvoiceDetail";

        public const string ClientIdColumn = "ClientId";
        public const string ClientNameColumn = "ClientName";
        public const string ChargeIdColumn = "ChargeId";
        public const string ChargeNameColumn = "ChargeName";
        public const string DaysColumn = "Days";
        public const string RateColumn = "Rate";
        public const string SubTotalColumn = "SubTotal";
        public const string ExtraHeadColumn = "Extra";
        public const string ExtraAmountColumn = "ExtraAmount";
        public const string LessHeadColumn = "Less";
        public const string LessAmountColumn = "LessAmount";
        public const string TotalAmountColumn = "TotalAmount";

        #endregion

        #region Constructor

        public AmalgamatedPrintDetailDataSet()
        {
            this.Locale = CultureInfo.CurrentCulture;
            this.BuildTables();
        }

        #endregion

        #region Private methods

        private void BuildTables()
        {
            var table = new DataTable
            {
                TableName = TableInvoiceDetail,
                Locale = CultureInfo.CurrentCulture
            };

            table.Columns.Add(new DataColumn
            {
                ColumnName = ClientIdColumn,
                Caption = ClientIdColumn,
                DataType = typeof(int),
                DefaultValue = 0
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = ClientNameColumn,
                Caption = ClientNameColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = ChargeIdColumn,
                Caption = ChargeIdColumn,
                DataType = typeof(int),
                DefaultValue = 0
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = ChargeNameColumn,
                Caption = ChargeNameColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = DaysColumn,
                Caption = DaysColumn,
                DataType = typeof(int),
                DefaultValue = 0
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = RateColumn,
                Caption = RateColumn,
                DataType = typeof(decimal),
                DefaultValue = 0
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = SubTotalColumn,
                Caption = SubTotalColumn,
                DataType = typeof(decimal),
                DefaultValue = 0
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = ExtraHeadColumn,
                Caption = ExtraHeadColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = ExtraAmountColumn,
                Caption = ExtraAmountColumn,
                DataType = typeof(decimal),
                DefaultValue = 0
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = LessHeadColumn,
                Caption = LessHeadColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = LessAmountColumn,
                Caption = LessAmountColumn,
                DataType = typeof(decimal),
                DefaultValue = 0
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = TotalAmountColumn,
                Caption = TotalAmountColumn,
                DataType = typeof(decimal),
                DefaultValue = 0
            });

            this.Tables.Add(table);
        }

        #endregion
    }
}
