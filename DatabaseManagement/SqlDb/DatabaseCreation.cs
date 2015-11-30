using System.Data.SqlClient;

namespace DatabaseManagement.SqlDb
{
    internal class DatabaseCreation
    {
        internal static void CreateDatabase(string connectionString, string databaseName)
        {
            //need to remove the actual database name so that it will not try and login to that individual database, but the whole DB server
            connectionString = ConnectionStringHandler.RemoveDatabaseNameFromConnectionString(connectionString);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "CREATE DATABASE " + databaseName;
                command.ExecuteNonQuery();

                connection.Close();
            } 

            //this is here as a real hack.. when schema update is called straight afterwards something still has an effect on the db and it wont run
            Logging.LoggerBase.Log("10 second pause to attempt to ensure that the database has been fully created and schema update will run against it successfully", isDebugMessage: true);
            System.Threading.Thread.Sleep(10000);
        }
    }
}
