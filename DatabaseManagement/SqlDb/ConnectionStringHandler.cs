using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using NHibernateRepo.Repos;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DatabaseManagement.SqlDb
{
    internal class ConnectionStringHandler
    {
        internal static bool IsValidConnectionThatConnects(string connectionString)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    return (conn.State == ConnectionState.Open);
                }
            }
            catch
            {
                return false;
            }
        }

        internal static bool CanConnectToDatabaseServer(string connectionString)
        {
            if (IsValidConnectionThatConnects(connectionString))
            {
                return true;
            }

            try
            {
                connectionString = RemoveDatabaseNameFromConnectionString(connectionString);
            }
            catch(ArgumentException ex)
            {
                //if it is not a connection string, i.e. connection string name then ignore this error.
                return false;                
            }
            return IsValidConnectionThatConnects(connectionString);
        }


        internal static string FindConnectionString(BaseRepo repo, string configFilePath)
        {
            //if the repo already has the full connection string test this first.
            if (CanConnectToDatabaseServer(repo.ConnectionStringOrName))
            {
                return repo.ConnectionStringOrName;
            }

            if (!File.Exists(configFilePath)) throw new FileNotFoundException("Could not find configuration file");

            var str = File.ReadAllText(configFilePath);
            var xmlRoot = XElement.Parse(str);

            var  connectionStringElement = xmlRoot.Elements("connectionStrings").FirstOrDefault();
            if(connectionStringElement == null) throw new ArgumentOutOfRangeException("configFilePath", "Could not find connection string section in config file");

            var connections = connectionStringElement.Elements("add").ToArray();
            if(connections == null || !connections.Any()) throw new ArgumentOutOfRangeException("configFilePath", "Could not find any connections within connection string section of config file");

            XElement connectionElement = null;
            connectionElement = !string.IsNullOrWhiteSpace(repo.ConnectionStringOrName) 
                ? connections.FirstOrDefault(c => c.Attribute("name").Value == repo.ConnectionStringOrName) 
                : connections.FirstOrDefault();

            if (connectionElement == null) throw new ArgumentOutOfRangeException("configFilePath", "Could not find connection string that matches repo connection: " + repo.ConnectionStringOrName);
            var connString = connectionElement.Attribute("connectionString").Value;
            if (string.IsNullOrWhiteSpace(connString)) throw new ArgumentNullException("configFilePath", "Connection string is empty for: " + repo.ConnectionStringOrName);

            return connString;                        
        }

        internal static void OverrideConnectionString(BaseRepo repo, string connectionString)
        {
            var t = repo.GetType();
            const string propName = "ConnectionStringOrName";
            
            t.InvokeMember(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, repo, new object[] { connectionString });
        }

        internal static string RemoveDatabaseNameFromConnectionString(string connectionString)
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

            return builder.ConnectionString;
        }
    }
}
