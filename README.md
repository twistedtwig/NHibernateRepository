#NHibernateRepository
Generic NHibernate Repository

The goal of this project is to provide a clean repository layer that tries to return sensible objects from the db entities.  It allows abstraction from a database or ORM and makes testing easier.

The Repo object is an abstract class.  A suggestion is to create a Repository project, inherit from the Repo class, passing the connection string name (or actual connection string) to the abstract base class.  The Entity models can be either in this project or another, As well as database overrides.

```
Repository Models Project:
[
	Models:
	[
		StudentEntity
		ClassEntity
	]
	Overrides:
	[
		StudentEntityOverride
		ClassEntityOverride
	]	

    RepositoryObject
]

```
Here is an simple example of how to setup a RepositoryObject

```c#

public class RepositoryObject : Repo<StudentEntity, StudentEntityOverride>()
{

	RepositoryObject() : base("DbConnectionStringName") { }

...
}

...

var repo = new RepositoryObject();
var list = repo.List<StudentEntity, StudentSummary>(x => x.Age > 18);

using (var transcation = repo.BeginTransaction())
{
    var student = transcation.Get<StudentEntity, int>(12);
    student.Grade.Add(new GradeEntity { Name = "A+" });

    transcation.Update(student);
    transcation.Commit();
}
```

##Database Management


Within this project there is a tool called NHMigrate.exe.  This is designed to assist with managing the database in a code first style.  The application can be used in two ways:

1) within VS using package manager console - (should be used for development, it wraps up the EXE for ease of use).
2) using the EXE directly - (only needs to be used for automated deployments).

There are three basic commands:

- Enable Migrations (Enable-NHMigrations)
- Add Migration (Add-NHMigration)
- Update Database (Update-NHMigrations)

Before any command is issued a Repository class that inherits from IRepo needs to be created.  The project that it is created in will become the repository project.
 
###Enable Migrations

This is the first command that should issued when adding NHibernateRepository to a project.  It creates a configuration file in the specified project that takes the repository class you created as a generic parameter.  This is figured out automatically for you by default.  The configuration file sets up some default settings.  The two main settings are:

 - Migration Type
 - Root Migration Folder.
  
The migration type refers to automatic mgiration or manual migrations.  See Migration Types section below.  The Root Migraiton Folder is the default location within the repository project that the manual migration scripts will be placed.

###Add Migration

If Manual migrations have been enabled, when ever the model structure, i.e. schema, changes then a migration needs to be created.  A migration file will be created, it will contain SQL scripts to update the database schema to match the code model.

###Update Database

The update database command will bring the database schema up to date with the code for either automatic migrations or manual migrations.  

 - When automatic migrations are enabled it will simply update the schema to directly match the code model.
 - When Manual migrations are enabled it will run all the migrations scripts in chronological, (oldest to newest).

###Migration Types

When updating a database this can be achieved it two ways.  Automatic updates so the shcema is brought into line with the schema as is by the code.  This doesn't allow for stored procedures or data changes.
The second approach is to migrations.  Every time a schema or data change is required create a migration within the repository project.
  
##Running NHMigrate from Visual Studio

Nearly all commands should be issued from within Visual studio.  The first thing is to ensure that the repository project is selected as the Default Project dropdown within the Package Manager Console and the startup project is set to the project that will have the configuration file and connection string.

Assuming there is only one instance of the repository class within the repository project NHMigrate will automatically find it.  If there are multiple repositories within the same repository project then a flag will need to be set giving the class name of the project.

    -repo CLASSNAME

All NHMigrate commands are first class citizens of the PMC and are enabled when ever a solution is opened that has a project containing a nuget reference to NHibernateRepository package.  This means auto complete and tab will work.

###Powershell Commands

- Enable-NHMigrations
- Add-NHMigration
- Update-NHMigrations

####Enable-NHMigrations

Simply call "Enable-NHMigrations" from within the PMC, use the optional "-repo CLASSNAME" flag if required.

####Add-NHMigration

Only use when migration type set to manual migrations. Requires one parameter (without white space) as the name of the migration file, use the optional "-repo CLASSNAME" flag if required.

    Add-NHMigration Added-Customer-Details

####Update-NHMigrations

No extra parameters are required except if there are multiple repositories, use the optional "-repo CLASSNAME" flag if required.
