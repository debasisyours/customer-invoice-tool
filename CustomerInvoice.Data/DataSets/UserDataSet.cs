using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Runtime.Serialization;
using System.Data;

namespace CustomerInvoice.Data.DataSets
{
    [Serializable]
    public class UserDataSet:DataSet,ISerializable
    {
        #region Const

        public const string TableUsers = "Users";

        public const string IdColumn = "Id";
        public const string NameColumn = "Name";
        public const string IsActiveColumn = "IsActive";
        public const string CompanyCountColumn = "CompanyCount";

        #endregion

        #region Constructor

        public UserDataSet()
        {
            this.Locale = CultureInfo.CurrentCulture;
            this.BuildUserTable();
        }

        protected UserDataSet(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region Private methods

        private void BuildUserTable()
        {
            DataTable table = new DataTable(TableUsers);
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

            column = new DataColumn(IsActiveColumn);
            column.Caption = IsActiveColumn;
            column.DataType = typeof(bool);
            column.DefaultValue = false;
            table.Columns.Add(column);

            column = new DataColumn(CompanyCountColumn);
            column.Caption = CompanyCountColumn;
            column.DataType = typeof(int);
            column.DefaultValue = 0;
            table.Columns.Add(column);

            this.Tables.Add(table);
        }

        #endregion
    }
}
