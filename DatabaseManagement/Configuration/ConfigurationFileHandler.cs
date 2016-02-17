using System.IO;
using System.Text;
using DatabaseManagement.ProjectHelpers;
using NHibernateRepo.Migrations;

namespace DatabaseManagement.Configuration
{
    /// <summary>
    /// Creates the Migration Configuration File in the given project location
    /// </summary>
    internal class ConfigurationFileHandler
    {
        internal string CreateConfigurationFile(string projectPath, string repoName, string migrationFolder, MigrationToUse migrationType)
        {
            var projectNamespace = new ProjectEvalutionHelper().RootNameSpace(projectPath);
            var className = repoName + "Configuration";

            var migrationFolderLocation = GetMigrationFolderLocation(projectPath, migrationFolder);
            if (!Directory.Exists(migrationFolderLocation))
            {                
                Directory.CreateDirectory(migrationFolderLocation);
            }

            var fileBuilder = new StringBuilder();
            fileBuilder.AppendLine("using NHibernateRepo.Configuration;");
            fileBuilder.AppendLine("using NHibernateRepo.Migrations;");
            fileBuilder.AppendLine("");
            fileBuilder.AppendLine(string.Format("namespace {0}.{1}", projectNamespace, migrationFolder));
            fileBuilder.AppendLine("{");
            fileBuilder.AppendLine("    public class " + className + " : RepoMigrationConfigurationBase<" + repoName + ">");
            fileBuilder.AppendLine("    {");
            fileBuilder.AppendLine("        public " + repoName + "Configuration()");
            fileBuilder.AppendLine("        {");
            fileBuilder.AppendLine("            Enabled = true;");
            fileBuilder.AppendLine("            MigrationType = MigrationToUse." + migrationType + ";");
            fileBuilder.AppendLine("            RootMigrationFolder = @\"Migrations\\" + repoName + "Migrations\";");
            fileBuilder.AppendLine("        }");
            fileBuilder.AppendLine("    }");
            fileBuilder.AppendLine("}");

            var path = Path.Combine(CreateFileLocationPath(projectPath, className, migrationFolder));
            File.WriteAllText(path, fileBuilder.ToString());
            return path;
        }

        private string CreateFileLocationPath(string projectPath, string className, string migrationFolder)
        {
            string folder = Path.Combine(projectPath.Substring(0, projectPath.LastIndexOf("\\")), migrationFolder);
            return Path.Combine(folder, className + ".cs");
        }

        private string GetMigrationFolderLocation(string projectPath, string migrationFolder)
        {
            return Path.Combine(projectPath.Substring(0, projectPath.LastIndexOf("\\")), migrationFolder);            
        }
    }
}
