using System;
using System.Reflection;

namespace CSharp.TestSuites
{
    public class DataTypesTestSuite : TestSuite, ITestSuite
    {
        public void Run()
        {
            WriteTestSuiteName();

            DataTypes1();
            DataTypes2();
            DataTypes3();

            WriteTestSuiteName();
        }

        private void DataTypes1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            int myInt = new int();
            Console.WriteLine($"myInt={myInt}");
        }

        private void DataTypes2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            int myInt2 = int.Parse("123");
            Console.WriteLine($"myInt2={myInt2}");

            int myInt3;
            bool parsed = int.TryParse("9876543210", out myInt3);
            Console.WriteLine($"parsed={parsed}, myInt3={myInt3}");
        }

        private void DataTypes3()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            int ten = 10;
            int i2 = 2147483647 + ten;
            Console.WriteLine(i2);

            try
            {
                checked
                {
                    int i3 = 2147483647 + ten;
                    Console.WriteLine(i3);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
