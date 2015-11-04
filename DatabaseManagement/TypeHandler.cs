using DatabaseManagement.Migrations;
using DatabaseManagement.ProjectHelpers;
using NHibernateRepo.Configuration;
using NHibernateRepo.Migrations;
using NHibernateRepo.Repos;
using System;
using System.Linq;

namespace DatabaseManagement
{
    /// <summary>
    /// Is a helper class to find Types from given assemblies or project files.
    /// </summary>
    internal static class TypeHandler
    {        
        /// <summary>
        /// Creates a instance of the base repo class from the given type.
        /// </summary>
        /// <param name="projectDllPath"></param>
        /// <param name="typeofRepo"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static BaseRepo CreateRepoBase(string projectDllPath, Type typeofRepo, params object[] args)
        {
            AssemblyLoadingHelper.Reset();
            AssemblyLoadingHelper.AddHintPath(projectDllPath);
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyLoadingHelper.OnAssemblyResolve;

            var repoBase = args == null || !args.Any() ? Activator.CreateInstance(typeofRepo) : Activator.CreateInstance(typeofRepo, args);
            if (repoBase == null)
            {
                Logger.Log("Couldn't create repo, must have parameterless constructor");
                return null;
            }

            var repo = repoBase as BaseRepo;
            if (repo == null)
            {
                Logger.Log("Repo created does not inherit from BaseRepo");
                return null;
            }

            AssemblyLoadingHelper.Reset();
            AppDomain.CurrentDomain.AssemblyResolve -= AssemblyLoadingHelper.OnAssemblyResolve;
            return repo;
        }

        /// <summary>
        /// Tries to find the implementation of the base repo object from the given project.  
        /// Will return NULL if more than one is found.  The name can be passed in if the repo name is known, (required if multiple repos in same project).
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="optionalRepoName"></param>
        /// <returns></returns>
        internal static RepoSearchResult FindSingleRepo(string projectPath, string optionalRepoName)
        {
            var loadedProject = new ProjectEvalutionHelper().LoadEvalutionProject(projectPath);

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
                    Logger.Log("No repo class found with name: " + optionalRepoName);
                    return null;
                }

                if (repos.Count() > 1)
                {
                    Logger.Log("More than one repo with same name found, please ensure repo is uniquely named in repo project.");
                    return null;
                }

                return new RepoSearchResult(loadedProject, repos.Single()); 
            }

            if (!repoTypes.Any())
            {
                Logger.Log("No repo class found");
                return null;
            }

            if (repoTypes.Count() > 1)
            {
                Logger.Log("More than one repo found, please specify which repo to use.");
                return null;
            }

            return new RepoSearchResult(loadedProject, repoTypes.Single());
        }

        /// <summary>
        /// Tries to find the migration configuration file. 
        /// Will find the repo for the project and then look for the configuration for that repo.
        /// Creates instance of the base none generic version
        /// Will return NULL if none found
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="optionalRepoName"></param>
        /// <returns></returns>
        internal static RepoMigrationConfigurationBaseNoneGeneric FindConfiguration(string projectPath, string optionalRepoName)
        {
            var repoInfo = FindSingleRepo(projectPath, optionalRepoName);
            if (repoInfo == null) return null;

            return FindConfiguration(projectPath, repoInfo.RepoType);
        }

        /// <summary>
        /// Tries to find the migration configuration file for the type given.
        /// Will find the repo for the project and then look for the configuration for that repo.
        /// Creates instance of the base none generic version
        /// Will return NULL if none found
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="repoType"></param>
        /// <returns></returns>
        internal static RepoMigrationConfigurationBaseNoneGeneric FindConfiguration(string projectPath, Type repoType)
        {
            bool multipleFound;
            var configType = FindSingleConfiguration(projectPath, repoType, out multipleFound);

            if (multipleFound || configType == null) return null;

            return Activator.CreateInstance(configType) as RepoMigrationConfigurationBaseNoneGeneric;
        }

        /// <summary>
        /// Tries to find configuration Type.
        /// If none or multiple found NULL will be returned.
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="repoType"></param>
        /// <param name="multipleFound"></param>
        /// <returns></returns>
        internal static Type FindSingleConfiguration(string projectPath, Type repoType, out bool multipleFound)
        {
            multipleFound = false;

            var loadedProject = new ProjectEvalutionHelper().LoadEvalutionProject(projectPath);

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

        /// <summary>
        /// Finds all migration Types / Classes from the given project.
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="repoType"></param>
        /// <returns></returns>
        internal static Type[] FindAllMigrations(string projectPath, Type repoType)
        {
            var loadedProject = new ProjectEvalutionHelper().LoadEvalutionProject(projectPath);

            var baseType = typeof(BaseMigration<>).MakeGenericType(repoType);
            return loadedProject
                .GetTypes()
                .Where(baseType.IsAssignableFrom)
                .ToArray();
        }

        /// <summary>
        /// Determines if the type given has an empty constructor
        /// </summary>
        /// <param name="objType"></param>
        /// <returns></returns>
        internal static bool DoesTypeHaveEmptyConstructor(Type objType)
        {
            return objType.GetConstructor(Type.EmptyTypes) != null;
        }
    }
}
