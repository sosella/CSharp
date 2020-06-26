using Newtonsoft.Json;
using System;

namespace CSharp.TestSuites
{
    public class JSONSerializationTestSuite : TestSuite, ITestSuite
    {
        public void Run()
        {
            WriteTestSuiteName();

            JSONSerialization1();

            WriteTestSuiteName();
        }

        private void JSONSerialization1()
        {
            Customer customer1 = new Customer();
            string customer1Json = JsonConvert.SerializeObject(customer1, Formatting.Indented);
            Console.WriteLine(customer1Json);
        }
    }
}
