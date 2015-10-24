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

1) within VS using package manager console - should be used for development, it wraps up the EXE for ease of use.
2) using the EXE directly - only needs to be used for automated deployments


When updating a database this can be achieved it two ways.  Automatic updates so the shcema is brought into line with the schema as is by the code.  This doesn't allow for stored procedures or data changes.
The second approach is to migrations.  Every time a schema or data change is required create a migration within the repository project.

###Powershell Commands


- Enable-NHMigrations
- Add-NHMigration
- Update-NHMigrations

