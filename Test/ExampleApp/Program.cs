using ExampleRepo;

namespace ExampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var worker = new ExampleWorker(new OtherRepo());
            worker.SetupMappings();           

            //worker.AddItems();
            worker.TestGettingItems();
        }
    }
}
