using System.Collections.Generic;

namespace ExampleRepo.Models
{
    public class CustomerEntity
    {
        public CustomerEntity()
        {
            Accounts = new List<BankAccountEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public IList<BankAccountEntity> Accounts { get; set; }
//        public int Age { get; set; }
    }
}