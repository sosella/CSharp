namespace CSharp.TestSuites
{
    public interface ITestSuite
    {
        string Name { get; }

        void Run();
    }
}
