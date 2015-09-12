
namespace DatabaseManagement.Models
{
    public class EnableMigrationsCriteria
    {
        /// <summary>
        /// Manditory project path 
        /// </summary>
        public string ProjectPath { get; set; }
        /// <summary>
        /// Optional repo name
        /// </summary>
        public string RepoName { get; set; }
    }
}
