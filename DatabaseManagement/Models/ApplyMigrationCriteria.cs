
namespace DatabaseManagement.Models
{
    public class ApplyMigrationCriteria
    {
        /// <summary>
        /// Manditory project path 
        /// </summary>
        public string ProjectPath { get; set; }
        /// <summary>
        /// Optional repo name
        /// </summary>
        public string RepoName { get; set; }
        /// <summary>
        /// Optional path to the configuration file used for connection string info
        /// </summary>
        public string ConfigFilePath { get; set; }
    }
}
