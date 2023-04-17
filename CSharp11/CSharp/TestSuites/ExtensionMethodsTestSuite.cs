using System;
using System.Reflection;

namespace CSharp.TestSuites
{
    /*
        To define and call the extension method

        1. Define a static class to contain the extension method.  The class must be visible to client code.
        2. Implement the extension method as a static method with at least the same visibility as the containing class.
        3. The first parameter of the method specifies the type that the method operates on; it must be preceded with the this modifier.
        4. Call the methods as if they were instance methods on the type
    */

    public class ExtensionMethodsTestSuite : TestSuite, ITestSuite
    {
        public void Run()
        {
            WriteTestSuiteName();

            ExtensionMethods1();

            WriteTestSuiteName();
        }

        private void ExtensionMethods1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            string s = "The quick brown fox jumped over the lazy dog.";
            Console.WriteLine($"Word count of s is {s.WordCount()}");
        }
    }

    //Extension methods must be defined in a static class
    public static class StringExtension
    {
        // This is the extension method.
        // The first parameter takes the "this" modifier, and specifies the type for which the method is defined.
        public static int WordCount(this string str)
        {
            return str.Split(new char[] { ' ', '.', '?', ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}
