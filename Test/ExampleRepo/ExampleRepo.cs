using ExampleRepo.Models;
using ExampleRepo.Overrides;
using NHibernateRepo.Repos;

namespace ExampleRepo
{
    public class ExampleRepo : RepoCombined<BankAccountEntity>
    {
        public ExampleRepo() : this("connectionString")
        {
        }

        public ExampleRepo(string connectionStringOrName) : base(connectionStringOrName)
        {
            
        }
    }

    public class OtherRepo : RepoSplit<BankAccountEntity, BankAccountEntityOverride>
    {

        public OtherRepo() : this("connectionString")
        {
        }

        public OtherRepo(string connectionStringOrName) : base(connectionStringOrName)
        {
        }
    }
}
