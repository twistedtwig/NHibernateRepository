using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using NHibernateRepo.Repos;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DatabaseManagement
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

        internal static string FindConnectionString(BaseRepo repo, string configFilePath)
        {
            //if the repo already has the full connection string test this first.
            if (IsValidConnectionThatConnects(repo.ConnectionStringOrName))
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
    }
}
