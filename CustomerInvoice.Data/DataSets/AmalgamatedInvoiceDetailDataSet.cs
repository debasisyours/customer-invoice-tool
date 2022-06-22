using System.Data;
using System.Globalization;

namespace CustomerInvoice.Data.DataSets
{
    public class AmalgamatedInvoiceDetailDataSet : DataSet
    {
        #region Declaration

        public const string TableAmalgamatedInvoiceDetail = "InvoiceDetail";

        public const string IdColumn = "Id";
        public const string ClientIdColumn = "ClientId";
        public const string ClientNameColumn = "ClientName";
        public const string ChargeHeadIdColumn = "ChargeHeadId";
        public const string ChargeHeadNameColumn = "ChargeHeadName";
        public const string RateColumn = "Rate";
        public const string DaysColumn = "Days";
        public const string SubTotalColumn = "SubTotal";
        public const string ExtraHeadColumn = "ExtraHead";
        public const string ExtraAmountColumn = "ExtraAmount";
        public const string LessHeadColumn = "LessHead";
        public const string LessAmountColumn = "LessAmount";
        public const string TotalAmountColumn = "TotalAmount";

        #endregion

        #region Constructor

        public AmalgamatedInvoiceDetailDataSet()
        {
            this.Locale = CultureInfo.CurrentCulture;
            this.BuildTable();
        }

        #endregion

        #region Private methods

        private void BuildTable()
        {
            var table = new DataTable
            {
                Locale = CultureInfo.CurrentCulture,
                TableName = TableAmalgamatedInvoiceDetail
            };

            table.Columns.Add(new DataColumn
            {
                ColumnName = IdColumn,
                Caption = IdColumn,
                DataType = typeof(int),
                DefaultValue = 0
            });

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
                ColumnName = ChargeHeadIdColumn,
                Caption = ChargeHeadIdColumn,
                DataType = typeof(int),
                DefaultValue = 0
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = ChargeHeadNameColumn,
                Caption = ChargeHeadNameColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
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
                ColumnName = DaysColumn,
                Caption = DaysColumn,
                DataType = typeof(int),
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
