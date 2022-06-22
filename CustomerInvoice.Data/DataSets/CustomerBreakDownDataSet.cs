using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerInvoice.Data.DataSets
{
    public class CustomerBreakDownDataSet : DataSet
    {
        #region Declaration

        public const string TableCustomerBreakdown = "CustomerBreakdown";

        public const string ClientIdColumn = "ClientId";
        public const string ClientNameColumn = "ClientName";
        public const string CustomerIdColumn = "CustomerId";
        public const string CustomerNameColumn = "CustomerName";
        public const string ChargeHeadIdColumn = "ChargeHeadId";
        public const string ChargeHeadNameColumn = "ChargeHeadName";
        public const string RateColumn = "Rate";
        public const string InvoiceCycleColumn = "InvoiceCycle";

        #endregion

        #region Constructor

        public CustomerBreakDownDataSet()
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
                TableName = TableCustomerBreakdown,
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
                ColumnName = CustomerIdColumn,
                Caption = CustomerIdColumn,
                DataType = typeof(int),
                DefaultValue = 0
            });
            table.Columns.Add(new DataColumn
            {
                ColumnName = CustomerNameColumn,
                Caption = CustomerNameColumn,
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
                ColumnName = InvoiceCycleColumn,
                Caption = InvoiceCycleColumn,
                DataType = typeof(short),
                DefaultValue = 0
            });

            this.Tables.Add(table);
        }

        #endregion
    }
}
