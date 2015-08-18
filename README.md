# NHibernateRepository
Generic NHibernate Repository

The goal of this project is to provide a clean repository layer that tries to return sensible objects from the db entities.  It allows abstraction from a database or ORM and makes testing easier.

The Repo object is an abstract class.  A suggestion is to create a Repository project, inherit from the Repo class, passing the connection string name (or actual connection string) to the abstract base class.  The Entity models can be either in this project or another, As well as database overrides.

```
Repository Project:
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


TODO:

Make a way of managing the database.  Want to imitate Entity Framework code first and migrations.
