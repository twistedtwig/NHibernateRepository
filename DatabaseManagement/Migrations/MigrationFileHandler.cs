using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DatabaseManagement.Models;
using NHibernate.Tool.hbm2ddl;

namespace DatabaseManagement.Migrations
{
    /// <summary>
    /// note that this has static referenes in it so can not be run in a threaded situation.
    /// </summary>
    internal class MigrationFileHandler
    {
        private readonly SchemaUpdate _schemaUpdater;
        private static readonly List<string> Scripts = new List<string>();
        
        public MigrationFileHandler(SchemaUpdate schemaUpdater)
        {
            _schemaUpdater = schemaUpdater;
        }

        public string CreateFile(CreationCriteria criteria)
        {
            var projectNamespace = new ProjectFileHandler().RootNameSpace(criteria.ProjectFileLocation);
            var migrationFileInfo = GenerateFileLocation(criteria);

            if (!Directory.Exists(migrationFileInfo.Folder))
            {
                Directory.CreateDirectory(migrationFileInfo.Folder);
            }

            //if it already exists delete it.. it should not at this point. this is a just incase.
            if (File.Exists(migrationFileInfo.FullFilePath))
            {
                File.Delete(migrationFileInfo.FullFilePath);
            }
            
            _schemaUpdater.Execute(_logScript, false);

            var fileBuilder = new StringBuilder();
            fileBuilder.AppendLine("using NHibernateRepo.Migrations;");
            fileBuilder.AppendLine("using System;");
            fileBuilder.AppendLine("");
            fileBuilder.AppendLine(string.Format("namespace {0}.{1}", projectNamespace, criteria.MigrationPath.Replace("\\", ".").Replace("-", "."))); 
            fileBuilder.AppendLine("{");
            fileBuilder.AppendLine("    public class " + migrationFileInfo.ClassName + " : BaseMigration<" + criteria.RepoName + ">");
            fileBuilder.AppendLine("    {");
            fileBuilder.AppendLine("        public override void Execute()");
            fileBuilder.AppendLine("        {");
            
            foreach (var script in Scripts)
            {
                fileBuilder.AppendLine("            ");
                fileBuilder.AppendLine("            ExecuteSql(@\"" + script +"\");");                
            }

            fileBuilder.AppendLine("            ");
            fileBuilder.AppendLine("        }");
            fileBuilder.AppendLine("    }");
            fileBuilder.AppendLine("}");
            
            File.WriteAllText(migrationFileInfo.FullFilePath, fileBuilder.ToString());
            return migrationFileInfo.FullFilePath;
        }
        


        private readonly Action<string> _logScript = x =>
        {
            if (!string.IsNullOrWhiteSpace(x))
            {
                Scripts.Add(x);
            }
        };



        /// <summary>
        /// Determine the path for the file to be created at.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        private MigrationFileInfo GenerateFileLocation(CreationCriteria criteria)
        {

            if (string.IsNullOrWhiteSpace(criteria.FileName))
            {
                throw new ArgumentNullException("criteria", "File name can not be null");
            }

            /*
             * 
             * get fully qualifed folder location.
             * 
             * from folder location use regex to find how many files with that name exist (ignore date stamp at the begninning and possible brackets and number at the end)
             * 
             * if zero then datastamp and name = name
             * 
             * else datastamp, name and brackets with count plus one = name
             * 
             * 
             * */



            string folder = Path.Combine(criteria.ProjectFileLocation.Substring(0, criteria.ProjectFileLocation.LastIndexOf("\\")), criteria.MigrationPath);

            string pattern = @"\\\d+_" + criteria.FileName + @"(\(\d+\)){0,1}.cs";
            var reg = new Regex(pattern);

            //only if the folder exists do we need to check if files already exist.
            var files = new List<string>();
            if (Directory.Exists(folder))
            {
                files = Directory.GetFiles(folder, "*.cs", SearchOption.AllDirectories)
                    .Where(path => reg.IsMatch(path))
                    .ToList();    
            }

            var now = DateTime.Now;
            var dateStamp = now.ToString("yyyyMMddHHmmss");

            var result = new MigrationFileInfo();
            result.Extension = ".cs";
            result.Folder = folder;

            const string fileNamePre = "Migration";

            if (files.Count == 0)
            {
                result.Name = dateStamp + "_" + criteria.FileName;
                result.ClassName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Format("{0}_{1}_{2}",fileNamePre, dateStamp, criteria.FileName));
            }
            else
            {
                result.Name = string.Format("{0}_{1}({2})", dateStamp, criteria.FileName, files.Count);
                result.ClassName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Format("{0}_{1}_{2}{3}", fileNamePre, dateStamp, criteria.FileName, files.Count));
            }

            result.FullFilePath = Path.Combine(folder, result.Name + result.Extension);
            return result;
        }
    }
}
