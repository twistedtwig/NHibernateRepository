using System;
using System.Reflection;

namespace DatabaseManagement.Migrations
{
    internal class RepoSearchResult
    {
        public RepoSearchResult(Assembly assembly, Type repoType)
        {
            Assembly = assembly;
            RepoType = repoType;
        }

        internal Assembly Assembly { get; set; }
        internal Type RepoType { get; set; }
    }
}
