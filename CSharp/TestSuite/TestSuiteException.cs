using System;

namespace CSharp.TestSuites
{
    public class TestSuiteException : Exception
    {
        public TestSuiteException() : base("Unknown Test Suite Type")
        {
        }
    }
}
