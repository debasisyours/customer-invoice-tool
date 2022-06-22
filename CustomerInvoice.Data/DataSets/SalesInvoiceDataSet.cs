using System.Data;
using System.Globalization;

namespace CustomerInvoice.Data.DataSets
{
    public class SalesInvoiceDataSet : DataSet
    {
        #region Declaration

        public const string TableSalesInvoice = "SalesInvoice";

        public const string ContactNameColumn = "ContactName";
        public const string EmailAddressColumn = "EmailAddress";
        public const string POAddressLine1Column = "POAddressLine1";
        public const string POAddressLine2Column = "POAddressLine2";
        public const string POAddressLine3Column = "POAddressLine3";
        public const string POAddressLine4Column = "POAddressLine4";
        public const string POCityColumn = "POCity";
        public const string PORegionColumn = "PORegion";
        public const string POPostalCodeColumn = "POPostalCode";
        public const string POCountryColumn = "POCountry";
        public const string InvoiceNumberColumn = "InvoiceNumber";
        public const string ReferenceColumn = "Reference";
        public const string InvoiceDateColumn = "InvoiceDate";
        public const string DueDateColumn = "DueDate";
        public const string TotalColumn = "Total";
        public const string InventoryItemCodeColumn = "InventoryItemCode";
        public const string DescriptionColumn = "Description";
        public const string QuantityColumn = "Quantity";
        public const string UnitAmountColumn = "UnitAmount";
        public const string DiscountColumn = "Discount";
        public const string AccountCodeColumn = "AccountCode";
        public const string TaxTypeColumn = "TaxType";
        public const string TaxAmountColumn = "TaxAmount";
        public const string TrackingName1Column = "TrackingName1";
        public const string TrackingOption1Column = "TrackingOption1";
        public const string TrackingName2Column = "TrackingName2";
        public const string TrackingOption2Column = "TrackingOption2";
        public const string CurrencyColumn = "Currency";
        public const string BrandingThemeColumn = "BrandingTheme";

        #endregion

        #region Constructor

        public SalesInvoiceDataSet()
        {
            this.Locale = CultureInfo.CurrentCulture;
            this.BuildSalesInvoiceTable();
        }

        #endregion

        #region Private members

        private void BuildSalesInvoiceTable()
        {
            var table = new DataTable(TableSalesInvoice)
            {
                Locale = CultureInfo.CurrentCulture
            };

            table.Columns.Add(new DataColumn
            {
                ColumnName = ContactNameColumn,
                Caption = ContactNameColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty,
                AllowDBNull = false
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = EmailAddressColumn,
                Caption = EmailAddressColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = POAddressLine1Column,
                Caption = POAddressLine1Column,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = POAddressLine2Column,
                Caption = POAddressLine2Column,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = POAddressLine3Column,
                Caption = POAddressLine3Column,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = POAddressLine4Column,
                Caption = POAddressLine4Column,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = POCityColumn,
                Caption = POCityColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = PORegionColumn,
                Caption = PORegionColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = POPostalCodeColumn,
                Caption = POPostalCodeColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = POCountryColumn,
                Caption = POCountryColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = InvoiceNumberColumn,
                Caption = InvoiceNumberColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty,
                AllowDBNull = false
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = ReferenceColumn,
                Caption = ReferenceColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = InvoiceDateColumn,
                Caption = InvoiceDateColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty,
                AllowDBNull = false
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = DueDateColumn,
                Caption = DueDateColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty,
                AllowDBNull = false
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = TotalColumn,
                Caption = TotalColumn,
                DataType = typeof(decimal),
                DefaultValue = 0
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = InventoryItemCodeColumn,
                Caption = InventoryItemCodeColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = DescriptionColumn,
                Caption = DescriptionColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty,
                AllowDBNull = false
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = QuantityColumn,
                Caption = QuantityColumn,
                DataType = typeof(int),
                DefaultValue = 0,
                AllowDBNull = false
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = UnitAmountColumn,
                Caption = UnitAmountColumn,
                DataType = typeof(decimal),
                DefaultValue = 0,
                AllowDBNull = false
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = DiscountColumn,
                Caption = DiscountColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = AccountCodeColumn,
                Caption = AccountCodeColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty,
                AllowDBNull = false
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = TaxTypeColumn,
                Caption = TaxTypeColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty,
                AllowDBNull = false
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = TaxAmountColumn,
                Caption = TaxAmountColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = TrackingName1Column,
                Caption = TrackingName1Column,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = TrackingOption1Column,
                Caption = TrackingOption1Column,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = TrackingName2Column,
                Caption = TrackingName2Column,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = TrackingOption2Column,
                Caption = TrackingOption2Column,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = CurrencyColumn,
                Caption = CurrencyColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            table.Columns.Add(new DataColumn
            {
                ColumnName = BrandingThemeColumn,
                Caption = BrandingThemeColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            });

            this.Tables.Add(table);
        }

        #endregion
    }
}
