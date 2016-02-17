using System;
using System.Data.SqlClient;

namespace DatabaseManagement.SqlDb
{
    internal class DatabaseChecking
    {
        internal static bool CheckDatabaseExists(string connectionString, string databaseName)
        {
            bool result = false;

            try
            {               
                var tmpConn = new SqlConnection(connectionString);
                string sqlCreateDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", databaseName);

                using (tmpConn)
                {
                    using (var sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn))
                    {
                        tmpConn.Open();

                        object resultObj = sqlCmd.ExecuteScalar();

                        int databaseID = 0;

                        if (resultObj != null)
                        {
                            int.TryParse(resultObj.ToString(), out databaseID);
                        }

                        tmpConn.Close();

                        result = (databaseID > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }
    }
}
