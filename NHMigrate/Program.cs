using System;
using System.Linq;
using DatabaseManagement;
using DatabaseManagement.Models;

namespace NHMigrate
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.ProcessArgs(args);
        }

        private void ProcessArgs(string[] args)
        {
            if (!args.Any())
            {
                Console.WriteLine("No arguments provided. please use HELP for more information");
                return;
            }

            switch (args[0].ToUpper())
            {
                case "HELP":
                    DisplayHelp();
                    break;
                case "ENABLE-MIGRATIONS":
                    EnableMigrations(args);
                    break;
                case "ADD-MIGRATION":
                    AddMigration(args);
                    break;
                case "UPDATE-DATABASE":
                    UpdateDatabase(args);
                    break;
                default:
                    Console.WriteLine("Unknown command provided. please use HELP for more information");
                    return;                    
            }
        }

        private void EnableMigrations(string[] args)
        {
            var criteriaParmas = ParseParams(args);

            var criteria = new EnableMigrationsCriteria
            {
                ProjectPath = criteriaParmas.ProjectPath,
                RepoName = criteriaParmas.OptionalRepoName,
                ConfigFilePath = criteriaParmas.ConfigFilePath,
            };

            var setup = new SchemaSetup();
            setup.EnableMigrations(criteria);
        }

        private void AddMigration(string[] args)
        {
            var criteriaParmas = ParseParams(args);

            var criteria = new CreationCriteria
            {
                ProjectFileLocation = criteriaParmas.ProjectPath,
                FileName = criteriaParmas.MigrationName,
                RepoName = criteriaParmas.OptionalRepoName,
                ConfigFilePath = criteriaParmas.ConfigFilePath,
            };

            var setup = new SchemaSetup();
            setup.CreateScript(criteria);
        }

        private void UpdateDatabase(string[] args)
        {
            var criteriaParmas = ParseParams(args);

            var criteria = new ApplyMigrationCriteria
            {
                ProjectPath = criteriaParmas.ProjectPath,
                RepoName = criteriaParmas.OptionalRepoName,
                ConfigFilePath = criteriaParmas.ConfigFilePath,
            };

            var setup = new SchemaSetup();
            setup.ApplyMigrations(criteria);
        }

        private void DisplayHelp()
        {
            Console.WriteLine("");
            Console.WriteLine("NHMigrate.exe help:");
            Console.WriteLine("");
            Console.WriteLine("DESCRIPTION:");
            Console.WriteLine("     NHMigrate is a command line application that manages an NHibernate .net applicaiton.");
            Console.WriteLine("     A user can update their database from the class models, (code first model).");
            Console.WriteLine("");
            Console.WriteLine("     See https://github.com/twistedtwig/NHibernateRepositor for more inforation.");
            Console.WriteLine("");
            Console.WriteLine("USAGE:");
            Console.WriteLine("     nhmigration [command] <projectFilePath> <optional2> -repo <optionalRepoName>");
            Console.WriteLine("");
            Console.WriteLine("COMMANDS:");
            Console.WriteLine("");
            Console.WriteLine("     Enable-Migrations:          This has to be done before any migrations can be created or applied.");
            Console.WriteLine("                                 Creates a configuration file in the project specified. describing how");
            Console.WriteLine("                                 and where to apply the migrations.");
            Console.WriteLine("                                 ");
            Console.WriteLine("     Add-Migration:              ONLY required when configuration set to manual.  Requires an additional");
            Console.WriteLine("                                 parameter to name the migration.  Creates SQL script to update the");
            Console.WriteLine("                                 database to match the code first model.");
            Console.WriteLine("                                 ");
            Console.WriteLine("     Update-Database:            If configuraiton set to automatic it will directly update the database to");
            Console.WriteLine("                                 match the code first model.  If set to manual it will run all migration");
            Console.WriteLine("                                 files that have been created.");
            Console.WriteLine("");
            Console.WriteLine("To add debug messages use -debug flag.");
            Console.WriteLine("");
            Console.WriteLine("If a repo name is not provided the system will attempt to determine which repository to use.");
            Console.WriteLine("If there are multiple repositories in the same assembly it will not know how to choose one.");
            Console.WriteLine("");
            Console.WriteLine("Example:");
            Console.WriteLine(@"     NHMigrate Add-Migration C:\work\MyExampleProject\MyExampleproject.csproj Added-Customer-Details");
            Console.WriteLine("");
        }

        private CriteriaParams ParseParams(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = args[i].ToLower();
            }

            var projectPath = string.Empty;
            var migrationName = string.Empty;
            var optionaRepoName = string.Empty;
            var configFilePath = string.Empty;

            if (args.Length >= 2)
            {
                projectPath = args[1];
            }

            if (args.Contains("-debug"))
            {
                Logger.IsDebugging = true;
            }

            if (args.Contains("-repo"))
            {
                var repoSwitchIndex = args.ToList().IndexOf("-repo");
                if (args.Length >= repoSwitchIndex + 1)
                {
                    optionaRepoName = args[repoSwitchIndex + 1];
                }
                else
                {
                    Console.WriteLine("Repo switch provided but no repo name given");
                    Environment.Exit(1);
                }
            }

            if (args.Contains("-configfile"))
            {
                var configSwitchIndex = args.ToList().IndexOf("-configfile");
                if (args.Length >= configSwitchIndex + 1)
                {
                    configFilePath = args[configSwitchIndex + 1];
                }
                else
                {
                    Console.WriteLine("ConfigFile switch provided but no file path given");
                    Environment.Exit(1);
                }
            }

            if (args.Contains("-filename"))
            {
                var fileNameSwitchIndex = args.ToList().IndexOf("-filename");
                if (args.Length >= fileNameSwitchIndex + 1)
                {
                    migrationName = args[fileNameSwitchIndex + 1];
                }
                else
                {
                    Console.WriteLine("ConfigFile switch provided but no file path given");
                    Environment.Exit(1);
                }
            }

            if (string.IsNullOrWhiteSpace(projectPath))
            {
                Console.WriteLine("project file path has not been provided");                
                Environment.Exit(1);
            }

            return new CriteriaParams(projectPath, optionaRepoName, migrationName, configFilePath);
        }
    }
}
