using System;

namespace CSharp.TestSuites
{
    public abstract class TestSuite
    {
        public string Name { get { return this.GetType().Name; } }

        public void WriteTestSuiteName()
        {
            Console.WriteLine();
            Console.WriteLine("==================== " + Name + " ====================");
            Console.WriteLine();
        }

        public void WriteMethodName(string methodName)
        {
            Console.WriteLine("-------------------- " + methodName + " --------------------");
        }

    }
}
