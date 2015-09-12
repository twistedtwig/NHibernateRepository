using DatabaseManagement.Migrations;
using NHibernateRepo.Configuration;
using NHibernateRepo.Migrations;
using NHibernateRepo.Repos;
using System;
using System.Linq;

namespace DatabaseManagement
{
    internal static class TypeHandler
    {        
        
        internal static BaseRepo CreateRepoBase(string projectDllPath, Type typeofRepo, params object[] args)
        {
            AssemblyLoadingHelper.Reset();
            AssemblyLoadingHelper.AddHintPath(projectDllPath);
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyLoadingHelper.OnAssemblyResolve;

            var repoBase = args == null || !args.Any() ? Activator.CreateInstance(typeofRepo) : Activator.CreateInstance(typeofRepo, args);
            if (repoBase == null)
            {
                Console.WriteLine("Couldn't create repo, must have parameterless constructor");
                return null;
            }

            var repo = repoBase as BaseRepo;
            if (repo == null)
            {
                Console.WriteLine("Repo created does not inherit from BaseRepo");
                return null;
            }

            AssemblyLoadingHelper.Reset();
            AppDomain.CurrentDomain.AssemblyResolve -= AssemblyLoadingHelper.OnAssemblyResolve;
            return repo;
        }

        internal static RepoSearchResult FindSingleRepo(string projectPath, string optionalRepoName)
        {
            var loadedProject = new ProjectFileHandler().LoadProject(projectPath);

            var repoTypes = loadedProject
                .GetTypes()
                .Where(t =>
                    t.IsSubclassOf(typeof(BaseRepo))
                    && !t.IsGenericType
                )
                .ToArray();

            if (!string.IsNullOrWhiteSpace(optionalRepoName))
            {
                var repos = repoTypes.Where(r => r.Name.Equals(optionalRepoName, StringComparison.InvariantCultureIgnoreCase)).ToArray();
                if (!repos.Any())
                {
                    Console.WriteLine("No repo class found with name: " + optionalRepoName);
                    return null;
                }

                if (repos.Count() > 1)
                {
                    Console.WriteLine("More than one repo with same name found, please ensure repo is uniquely named in repo project.");
                    return null;
                }

                return new RepoSearchResult(loadedProject, repos.Single()); 
            }

            if (!repoTypes.Any())
            {
                Console.WriteLine("No repo class found");
                return null;
            }

            if (repoTypes.Count() > 1)
            {
                Console.WriteLine("More than one repo found, please specify which repo to use.");
                return null;
            }

            return new RepoSearchResult(loadedProject, repoTypes.Single());
        }

        internal static RepoMigrationConfigurationBaseNoneGeneric FindConfiguration(string projectPath, string optionalRepoName)
        {
            var repoInfo = FindSingleRepo(projectPath, optionalRepoName);
            if (repoInfo == null) return null;

            return FindConfiguration(projectPath, repoInfo.RepoType);
        }

        internal static RepoMigrationConfigurationBaseNoneGeneric FindConfiguration(string projectPath, Type repoType)
        {
            bool multipleFound;
            var configType = FindSingleConfiguration(projectPath, repoType, out multipleFound);

            if (multipleFound || configType == null) return null;

            return Activator.CreateInstance(configType) as RepoMigrationConfigurationBaseNoneGeneric;
        }

        internal static Type FindSingleConfiguration(string projectPath, Type repoType, out bool multipleFound)
        {
            multipleFound = false;

            var loadedProject = new ProjectFileHandler().LoadProject(projectPath);

            var baseType = typeof (RepoMigrationConfigurationBase<>).MakeGenericType(repoType);
            var configTypes = loadedProject
                .GetTypes()
                .Where(baseType.IsAssignableFrom)                
                .ToArray();
            
            //if none found then we will end up creating one.
            if (!configTypes.Any())
            {
                return null;
            }

            if (configTypes.Length == 1)
            {
                return configTypes.Single();
            }

            multipleFound = true;
            return null;
        }

        internal static Type[] FindAllMigrations(string projectPath, Type repoType)
        {
            var loadedProject = new ProjectFileHandler().LoadProject(projectPath);

            var baseType = typeof(BaseMigration<>).MakeGenericType(repoType);
            return loadedProject
                .GetTypes()
                .Where(baseType.IsAssignableFrom)
                .ToArray();
        }
    }
}
