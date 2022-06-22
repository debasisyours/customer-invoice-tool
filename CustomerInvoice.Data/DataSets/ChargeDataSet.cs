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
    public class ChargeDataSet:DataSet,ISerializable
    {
        #region Constants

        public const string TableChargeHead = "ChargeHead";

        public const string IdColumn = "Id";
        public const string NameColumn = "Name";

        #endregion

        #region Constructor

        public ChargeDataSet()
        {
            this.Locale = CultureInfo.CurrentCulture;
            this.BuildChargeTable();
        }

        protected ChargeDataSet(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region Private methods

        private void BuildChargeTable()
        {
            DataTable table = new DataTable(TableChargeHead);
            table.Locale = CultureInfo.CurrentCulture;

            DataColumn column = new DataColumn(IdColumn);
            column.Caption = IdColumn;
            column.DataType = typeof(int);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            column = new DataColumn(NameColumn);
            column.Caption = NameColumn;
            column.DataType = typeof(string);
            column.DefaultValue = string.Empty;
            table.Columns.Add(column);

            this.Tables.Add(table);
        }

        #endregion
    }
}
