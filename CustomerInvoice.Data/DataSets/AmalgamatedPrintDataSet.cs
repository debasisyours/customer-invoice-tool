using System;
using System.Data;
using System.Globalization;

namespace CustomerInvoice.Data.DataSets
{
    [Serializable]
    public class AmalgamatedPrintDataSet : DataSet
    {
        #region Declaration

        public const string TableInvoice = "Invoice";

        public const string CompanyNameColumn = "CompanyName";
        public const string CompanyAddressColumn = "CompanyAddress";
        public const string InvoiceNumberColumn = "InvoiceNumber";
        public const string CustomerNameColumn = "CustomerName";
        public const string CustomerAddressColumn = "CustomerAddress";
        public const string InvoiceDateColumn = "InvoiceDate";
        public const string StartDateColumn = "StartDate";
        public const string EndDateColumn = "EndDate";
        public const string TotalDueColumn = "TotalDue";
        public const string CompanyCodeColumn = "CompanyCode";
        public const string AccountNameColumn = "AccountName";
        public const string AccountNumberColumn = "AccountNumber";
        public const string SortCodeColumn = "SortCode";

        #endregion

        #region Constructor

        public AmalgamatedPrintDataSet()
        {
            this.DataSetName = TableInvoice;
            this.Locale = CultureInfo.CurrentCulture;
            this.BuildTables();
        }

        #endregion

        #region Private methods

        private void BuildTables()
        {
            var table = new DataTable
            {
                TableName = TableInvoice,
                Locale = CultureInfo.CurrentCulture,
            };

            table.Columns.Add(new DataColumn
            {
                ColumnName = CompanyNameColumn,
                Caption = CompanyNameColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = CompanyAddressColumn,
                Caption = CompanyAddressColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = InvoiceNumberColumn,
                Caption = InvoiceNumberColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
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
                ColumnName = CustomerAddressColumn,
                Caption = CustomerAddressColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = InvoiceDateColumn,
                Caption = InvoiceDateColumn,
                DataType = typeof(DateTime?),
                DefaultValue = null
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = StartDateColumn,
                Caption = StartDateColumn,
                DataType = typeof(DateTime?),
                DefaultValue = null
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = EndDateColumn,
                Caption = EndDateColumn,
                DataType = typeof(DateTime?),
                DefaultValue = null
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = TotalDueColumn,
                Caption = TotalDueColumn,
                DataType = typeof(decimal),
                DefaultValue = 0
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = CompanyCodeColumn,
                Caption = CompanyCodeColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = AccountNameColumn,
                Caption = AccountNameColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = AccountNumberColumn,
                Caption = AccountNumberColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = SortCodeColumn,
                Caption = SortCodeColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            this.Tables.Add(table);
        }

        #endregion
    }
}
