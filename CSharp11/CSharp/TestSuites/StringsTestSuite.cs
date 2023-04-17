using System;
using System.Reflection;
using System.Text;

namespace CSharp.TestSuites
{
    public class StringsTestSuite : TestSuite, ITestSuite
    {
        public void Run()
        {
            WriteTestSuiteName();

            Strings1();
            Strings2();

            WriteTestSuiteName();
        }

        private void Strings1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            string[] names =
            {
                "Able",
                "Baker",
                "Charlie",
                "Davis"
            };
            StringBuilder sb = new StringBuilder();
            foreach (string name in names)
            {
                sb.Append(" [" + name + "] " + Environment.NewLine);
            }
            string str = sb.ToString();
            Console.WriteLine($"str={str}");
        }

        private void Strings2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            decimal d = (decimal)1234.56;
            Console.WriteLine(d.ToString());
            //Console.WriteLine(d.ToCurrencyString());
            Console.WriteLine(d.ToString("c"));
            //Console.WriteLine(Format("{0:C}",d);
        }
    }
}
