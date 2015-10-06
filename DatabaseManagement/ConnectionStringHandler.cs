using NHibernateRepo.Repos;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DatabaseManagement
{
    internal class ConnectionStringHandler
    {

        internal static void SubsituateConnectionString(BaseRepo repo, string configFilePath)
        {
            if(!File.Exists(configFilePath)) return;

            var str = File.ReadAllText(configFilePath);
            var xmlStr = XElement.Parse(str);

            var result1 = xmlStr.Elements("word").Where(x => x.Element("connectionString").Value.Equals("conn"));
            var result2 = xmlStr.Elements("word");

        }
    }
}
