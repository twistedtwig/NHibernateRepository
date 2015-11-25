using System.Data.SqlClient;

namespace DatabaseManagement.SqlDb
{
    internal class DatabaseCreation
    {
        internal static void CreateDatabase(string connectionString, string databaseName)
        {
            //need to remove the actual database name so that it will not try and login to that individual database, but the whole DB server
            var builder = new SqlConnectionStringBuilder
            {
                ConnectionString = connectionString
            };

            if (connectionString.ToLower().Contains("database"))
            {
                builder.Remove("Database");
            }

            if (connectionString.ToLower().Contains("initial catalog"))
            {
                builder.Remove("initial catalog");
            }


            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "CREATE DATABASE " + databaseName;
                command.ExecuteNonQuery();
            } 
        }
    }
}
