using System.Collections.Generic;

namespace ExampleApp
{
    public class BankView
    {
        public BankView()
        {
            Customers = new List<CustomerView>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public IList<CustomerView> Customers { get; set; }
    }

    public class CustomerView
    {
        public CustomerView()
        {
            Accounts = new List<BankAccountView>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public IList<BankAccountView> Accounts { get; set; }

    }

    public class BankAccountView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
    }
}
