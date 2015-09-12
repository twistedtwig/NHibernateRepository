
namespace NHMigrate
{
    internal class CriteriaParams
    {
        public CriteriaParams(string projectPath = "", string optionalRepoName = "", string migrationName = "")
        {
            ProjectPath = projectPath;
            OptionalRepoName = optionalRepoName;
            MigrationName = migrationName;
        }

        public string ProjectPath { get; private set; }
        public string OptionalRepoName { get; private set; }
        public string MigrationName { get; private set; }
    }
}
