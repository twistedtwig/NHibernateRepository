using System;

namespace NHibernateMigrationRepo
{
    public class MigrationLogEntity
    {
        public int Id { get; set; }
        public DateTime DateRun { get; set; }
        public string MigrationName { get; set; }
        public string RepoName { get; set; }
    }
}
