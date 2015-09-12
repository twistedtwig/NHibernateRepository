using ExampleRepo.Models;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace ExampleRepo.Overrides
{
    public class BankAccountEntityOverride : IAutoMappingOverride<BankAccountEntity>
    {
        public void Override(AutoMapping<BankAccountEntity> mapping)
        {
            mapping.Table("BankAccounts");
        }
    }
}
