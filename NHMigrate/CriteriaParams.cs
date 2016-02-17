
namespace NHMigrate
{
    internal class CriteriaParams
    {
        public CriteriaParams(string projectPath = "", string optionalRepoName = "", string migrationName = "", string configFilePath = "")
        {
            ProjectPath = projectPath;
            OptionalRepoName = optionalRepoName;
            MigrationName = migrationName;
            ConfigFilePath = configFilePath;
        }

        public string ProjectPath { get; private set; }
        public string OptionalRepoName { get; private set; }
        public string MigrationName { get; private set; }
        public string ConfigFilePath { get; private set; }
    }
}
