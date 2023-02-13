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
    public class ClientDataSet:DataSet,ISerializable
    {
        #region Constants

        public const string TableClient = "Client";

        public const string IdColumn = "Id";
        public const string CodeColumn = "Code";
        public const string NameColumn = "Name";
        public const string DateOfBirthColumn = "DateOfBirth";
        public const string DateOfAdmissionColumn = "DateOfAdmission";
        public const string TotalRateColumn = "TotalRate";
        public const string SageReferenceColumn = "SageReference";
        public const string TheirReferenceColumn = "TheirReference";
        public const string CustomerCodeColumn = "CustomerCode";
        public const string RipColumn = "RIP";
        public const string CustomerEmailColumn = "CustomerEmail";
        public const string NursingColumn = "Nursing";
        public const string SelfFundingColumn = "SelfFunding";
        public const string ResidentialColumn = "Residential";

        #endregion

        #region Constructor

        public ClientDataSet()
        {
            this.Locale = CultureInfo.CurrentCulture;
            this.BuildClientTable();
        }

        protected ClientDataSet(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region Private functions

        private void BuildClientTable()
        {
            DataTable table = new DataTable(TableClient);
            table.Locale = CultureInfo.CurrentCulture;

            DataColumn column = new DataColumn(IdColumn);
            column.Caption = IdColumn;
            column.DataType = typeof(int);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(CodeColumn);
            column.Caption = CodeColumn;
            column.DataType = typeof(long);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(NameColumn);
            column.Caption = NameColumn;
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

            column = new DataColumn(TotalRateColumn);
            column.Caption = TotalRateColumn;
            column.DataType = typeof(double);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(SageReferenceColumn);
            column.Caption = SageReferenceColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(TheirReferenceColumn);
            column.Caption = TheirReferenceColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(CustomerCodeColumn);
            column.Caption = CustomerCodeColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(RipColumn);
            column.Caption = RipColumn;
            column.DataType = typeof(DateTime);
            column.DefaultValue = Convert.ToDateTime("1901-01-01");
            table.Columns.Add(column);

            column = new DataColumn(CustomerEmailColumn);
            column.Caption = CustomerEmailColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            column = new DataColumn(NursingColumn);
            column.Caption = NursingColumn;
            column.DataType = typeof(bool);
            column.DefaultValue = false;
            table.Columns.Add(column);

            column = new DataColumn(SelfFundingColumn);
            column.Caption = SelfFundingColumn;
            column.DataType = typeof(bool);
            column.DefaultValue = false;
            table.Columns.Add(column);

            column = new DataColumn(ResidentialColumn);
            column.Caption = ResidentialColumn;
            column.DataType = typeof(bool);
            column.DefaultValue = false;
            table.Columns.Add(column);

            this.Tables.Add(table);
        }

        #endregion
    }
}
