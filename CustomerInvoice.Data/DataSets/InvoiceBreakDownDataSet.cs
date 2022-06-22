using System;
using System.Data;
using System.Globalization;
using System.Runtime.Serialization;

namespace CustomerInvoice.Data.DataSets
{
    public class InvoiceBreakDownDataSet:DataSet,ISerializable
    {
        #region Const

        public const string TableInvoiceBreakDown = "InvoiceBreakDown";

        public const string InvoiceSubPartColumn = "InvoiceSubPart";
        public const string StartDateColumn = "StartDate";
        public const string EndDateColumn = "EndDate";
        public const string TotalDaysColumn = "TotalDays";
        public const string DueDateColumn = "DueDate";
        public const string FeeAmountColumn = "FeeAmount";
        public const string InvoicePartColumn = "InvoicePart";

        #endregion

        #region Constructor

        public InvoiceBreakDownDataSet()
        {
            this.Locale = CultureInfo.CurrentCulture;
            this.BuildInvoiceBreakDown();
        }

        protected InvoiceBreakDownDataSet(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        #endregion
        
        #region Private methods

        private void BuildInvoiceBreakDown()
        {
            DataTable table = new DataTable(TableInvoiceBreakDown);
            table.Locale = CultureInfo.CurrentCulture;

            DataColumn column = new DataColumn
            {
                ColumnName = InvoiceSubPartColumn,
                Caption = InvoiceSubPartColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            };
            table.Columns.Add(column);

            column = new DataColumn
            {
                ColumnName = StartDateColumn,
                Caption = StartDateColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            };
            table.Columns.Add(column);

            column = new DataColumn
            {
                ColumnName = EndDateColumn,
                Caption = EndDateColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            };
            table.Columns.Add(column);

            column = new DataColumn
            {
                ColumnName = TotalDaysColumn,
                Caption = TotalDaysColumn,
                DataType = typeof(int),
                DefaultValue = 0
            };
            table.Columns.Add(column);

            column = new DataColumn
            {
                ColumnName = DueDateColumn,
                Caption = DueDateColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            };
            table.Columns.Add(column);

            column = new DataColumn
            {
                ColumnName = FeeAmountColumn,
                Caption = FeeAmountColumn,
                DataType = typeof(decimal),
                DefaultValue = 0
            };
            table.Columns.Add(column);

            column = new DataColumn
            {
                ColumnName = InvoicePartColumn,
                Caption = InvoicePartColumn,
                DataType = typeof(string),
                DefaultValue = string.Empty
            };
            table.Columns.Add(column);

            this.Tables.Add(table);
        }

        #endregion
    }
}
