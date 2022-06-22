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
    public class InvoicePrintDataSet:DataSet,ISerializable
    {
        #region Consts

        public const string TableInvoicePrint = "InvoicePrint";

        public const string CompanyNameColumn = "CompanyName";
        public const string CompanyAddressColumn = "CompanyAddress";
        public const string AccountNameColumn = "AccountName";
        public const string AccountNumberColumn = "AccountNumber";
        public const string SortCodeColumn = "SortCode";

        public const string CustomerNameColumn = "CustomerName";
        public const string CustomerAddressColumn = "CustomerAddress";
        public const string StartDateColumn = "StartDate";
        public const string EndDateColumn = "EndDate";

        public const string ClientCodeColumn = "ClientCode";
        public const string ClientNameColumn = "ClientName";
        public const string DateOfBirthColumn = "DateOfBirth";
        public const string DateOfAdmissionColumn = "DateOfAdmission";
        public const string TheirRefColumn = "TheirReference";

        public const string InvoiceNumberColumn = "InvoiceNumber";
        public const string InvoiceDateColumn = "InvoiceDate";
        public const string WeeklyRateColumn = "WeeklyRate";
        public const string SecondWeeklyRateColumn = "SecondWeeklyRate";
        public const string TotalDaysColumn = "TotalDays";
        public const string ChargeHeadColumn = "ChargeHead";
        public const string SecondChargeHeadColumn = "SecondChargeHead";
        public const string SubTotalColumn = "SubTotal";
        public const string ExtraPayHeadColumn = "ExtraHead";
        public const string ExtraAmountColumn = "ExtraAmount";
        public const string LessPayHeadColumn = "LessHead";
        public const string LessAmountColumn = "LessAmount";
        public const string NetAmountColumn = "NetAmount";

        public const string CompanyCodeColumn = "CompanyCode";

        #endregion

        #region Constructor

        public InvoicePrintDataSet()
        {
            this.Locale = CultureInfo.CurrentCulture;
            this.BuildInvoicePrintTable();
        }

        protected InvoicePrintDataSet(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region Private Methods

        private void BuildInvoicePrintTable()
        {
            DataTable table = new DataTable(TableInvoicePrint);
            table.Locale = CultureInfo.CurrentCulture;

            DataColumn column = new DataColumn(CompanyNameColumn);
            column.Caption = CompanyNameColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(CompanyAddressColumn);
            column.Caption = CompanyAddressColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(AccountNameColumn);
            column.Caption = AccountNameColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(AccountNumberColumn);
            column.Caption = AccountNumberColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(SortCodeColumn);
            column.Caption = SortCodeColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(CustomerNameColumn);
            column.Caption = CustomerNameColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(CustomerAddressColumn);
            column.Caption = CustomerAddressColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(StartDateColumn);
            column.Caption = StartDateColumn;
            column.DataType = typeof(DateTime);
            column.DefaultValue = null;
            table.Columns.Add(column);

            column = new DataColumn(EndDateColumn);
            column.Caption = EndDateColumn;
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

            column = new DataColumn(DateOfBirthColumn);
            column.Caption = DateOfBirthColumn;
            column.DataType = typeof(DateTime);
            column.DefaultValue = null;
            table.Columns.Add(column);

            column = new DataColumn(DateOfAdmissionColumn);
            column.Caption = DateOfAdmissionColumn;
            column.DataType = typeof(DateTime);
            column.DefaultValue = null;
            table.Columns.Add(column);

            column = new DataColumn(TheirRefColumn);
            column.Caption = TheirRefColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
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

            column = new DataColumn(WeeklyRateColumn);
            column.Caption = WeeklyRateColumn;
            column.DataType = typeof(decimal);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(SecondWeeklyRateColumn);
            column.Caption = SecondWeeklyRateColumn;
            column.DataType = typeof(decimal);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(TotalDaysColumn);
            column.Caption = TotalDaysColumn;
            column.DataType = typeof(int);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(ChargeHeadColumn);
            column.Caption = ChargeHeadColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(SecondChargeHeadColumn);
            column.Caption = SecondChargeHeadColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
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

            column = new DataColumn(CompanyCodeColumn);
            column.Caption = CompanyCodeColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            this.Tables.Add(table);
        }

        #endregion
    }
}
