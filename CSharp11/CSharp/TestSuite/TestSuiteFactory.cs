namespace CSharp.TestSuites
{
    public static class TestSuiteFactory
    {
        public static TTestSuite Create<TTestSuite>() where TTestSuite : TestSuite, ITestSuite, new()
        {
            return new TTestSuite();
        }
    }
}
