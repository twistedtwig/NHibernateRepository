using ExampleRepo.Models;
using NHibernate.Util;

namespace ExampleApp
{
    public class ExampleWorker
    {
        private readonly ExampleRepo.OtherRepo _context;

        public ExampleWorker(ExampleRepo.OtherRepo repo)
        {
            _context = repo;
        }

        public void SetupMappings()
        {
            AutoMapper.Mapper.CreateMap<BankEntity, BankView>();
            AutoMapper.Mapper.CreateMap<CustomerEntity, CustomerView>();
            AutoMapper.Mapper.CreateMap<BankAccountEntity, BankAccountView>();            
        }

        public void AddItems()
        {
            var bank = new BankEntity
            {
                Name = "Bank Account",
                Town = "Some Town",
            };

            var customer = new CustomerEntity
            {
                Name = "customer 1",
            };

            var account1 = new BankAccountEntity
            {
                Name = "acc1",
                Balance = 123
            };

            var account2 = new BankAccountEntity
            {
                Name = "acc2",
                Balance = 555
            };

            bank.Customers.Add(customer);
            customer.Accounts.Add(account1);
            customer.Accounts.Add(account2);

            using (var repo = _context.BeginTransaction())
            {
                repo.Create(bank);
                repo.Create(customer);
                repo.Create(account1);
                repo.Create(account2);

                repo.Commit();
            }
        }

        public void TestGettingItems()
        {
//            using (var repo = _context.BeginTransaction())
//            {
//                var bank = repo.Entities<BankEntity>().First();
//            }
//
            var accounts = _context.List<BankEntity, BankView>(a => true);

            var firstAccount1 = _context.First<BankEntity, BankView>(a => true);
            var firstAccount2 = _context.First<BankEntity, BankView>(a => a.Name == "Bank Account");
            var firstAccount3 = _context.FirstOrDefault<BankEntity, BankView>(a => a.Name == "acc2");
            var firstAccount4 = _context.Single<BankEntity, BankView>(a => a.Name == "Bank Account");
        }
    }
}
