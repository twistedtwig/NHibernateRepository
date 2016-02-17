
namespace DatabaseManagement.Models
{
    public class CreationCriteria
    {
        public string FileName { get; set; }
        public string ProjectFileLocation { get; set; }
        public string RepoName { get; set; }

        internal string MigrationPath { get; set; }
        public string ConfigFilePath { get; set; }

    }
}
