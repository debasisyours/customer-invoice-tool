using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CustomerInvoice.Common;
using System.IO;

namespace CustomerInvoice.Data.Helpers
{
    public class DatabaseHelper:IDisposable
    {
        #region Field declaration

        private string _ConnectionString = string.Empty;

        #endregion

        #region Constructor

        public DatabaseHelper(string connectionString)
        {
            this._ConnectionString = connectionString;
        }

        #endregion

        #region Methods

        protected internal bool UpdateDatabase()
        {
            string databaseScript = Properties.Resources.AlterTable;
            bool success = false;
            try
            {
                using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
                {
                    context.ExecuteCommand(databaseScript);
                    if (!ProcedureExists("usp_Invoice_History")) context.ExecuteCommand(Properties.Resources.usp_Invoice_History);
                    if (!ProcedureExists("usp_CheckClientRip")) context.ExecuteCommand(Properties.Resources.usp_CheckClientRip);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return success;
        }

        protected internal bool ProcedureExists(string procedureName)
        {
            bool exists = false;

            using (InvoiceContextDataContext context = new InvoiceContextDataContext(this._ConnectionString))
            {
                context.Connection.Open();
                using (var command = context.Connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = $"SELECT 1 AS CheckExists FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = '{procedureName}'";
                    using (var reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if(reader.Read())
                        {
                            if (Convert.ToInt32(reader["CheckExists"]) == 1) exists = true;
                        }
                    }
                }
            }

            return exists;
        }

        protected internal bool RestoreDatabase(string backupPath, string databaseName)
        {
            if (databaseName.Contains("[")) databaseName = databaseName.Replace("[", string.Empty).Replace("]", string.Empty);

            string executeStatement = $"RESTORE DATABASE {databaseName} FROM DISK = '" + backupPath + "'";
            string singleUser = $"ALTER DATABASE {databaseName} SET SINGLE_USER";
            bool result = false;

            try
            {
                using (var connection = new SqlConnection(_ConnectionString.Replace(databaseName,"master")))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandTimeout = 0;

                        command.CommandText = singleUser;
                        command.ExecuteNonQuery();

                        command.CommandText = executeStatement;
                        command.ExecuteNonQuery();
                        result = true;
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }

            return result;
        }

        #endregion

        #region Interface implementation

        void IDisposable.Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
