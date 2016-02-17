using NHibernateRepo.Repos;

namespace NHibernateRepo.Migrations
{
    /// <summary>
    /// Class is used to be able to find all migrations without knowing the generic types.
    /// NEVER CREATE INSTANCE OF THIS CLASS, ONLY USED FOR NON GENERIC REFERENCING
    /// </summary>
    public abstract class AbstractBaseMigration
    {
        internal BaseRepo BaseRepo { get; set; }
        public abstract void Execute();
    }

    public abstract class BaseMigration<T> : AbstractBaseMigration where T : BaseRepo 
    {
        
        //todo would be good to be in a transaciton
        protected void ExecuteSql(string sql)
        {
            var command = BaseRepo.Session.Connection.CreateCommand();
            command.CommandText = sql;
            command.ExecuteNonQuery();            
        }
    }
}
