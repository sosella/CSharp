using System;
using System.Reflection;

namespace CSharp.TestSuites
{
    public class LoopingTestSuite : TestSuite, ITestSuite
    {
        public void Run()
        {
            WriteTestSuiteName();

            Looping1();

            WriteTestSuiteName();
        }

        private void Looping1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            int i = 0;
            do
            {
                Console.WriteLine($"loop = {i}");
            } while (++i < 10);
        }
    }
}
