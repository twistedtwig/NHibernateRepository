using System.Collections.Generic;

namespace ExampleRepo.Models
{
    public class BankEntity
    {
        public BankEntity()
        {
            Customers = new List<CustomerEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Town { get; set; }
        public IList<CustomerEntity> Customers { get; set; }
    }
}
