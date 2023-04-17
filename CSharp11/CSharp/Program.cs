using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CSharp.TestSuites;

namespace CSharp
{
    public class Program
    {
        private static int[] GetTestSuiteIndexes(string strInput, int nbrTestSuites)
        {
            if (string.IsNullOrWhiteSpace(strInput))
            {
                int[] txIdxs = new int[nbrTestSuites];
                for (int i = 0; i < nbrTestSuites; i++)
                {
                    txIdxs[i] = i;
                }
                return txIdxs;
            }

            List<int> tsIdxList = new List<int>();

            string repStr = Regex.Replace(strInput, @"\s*", string.Empty);

            Regex re1 = new Regex(@"\d+-\d+|\d+");
            Regex re2 = new Regex(@"\d+");

            MatchCollection mc1 = re1.Matches(repStr);
            foreach (Match m1 in mc1)
            {
                foreach (Capture c1 in m1.Captures)
                {
                    if (c1.Value.Contains('-'))
                    {
                        MatchCollection mc2 = re2.Matches(c1.Value);
                        if (mc2.Count != 2)
                        {
                            throw new ArgumentException();
                        }
                        int[] tsIdxRange = new int[2] { 0, 0 };
                        int i = 0;
                        foreach (Match m2 in mc2)
                        {
                            if (m2.Captures.Count != 1)
                            {
                                throw new ArgumentException();
                            }
                            foreach (Capture c2 in m2.Captures)
                            {
                                int tsIdx = int.Parse(c2.Value);
                                if ((tsIdx < 0) || (tsIdx >= nbrTestSuites))
                                {
                                    throw new ArgumentException();
                                }
                                tsIdxRange[i++] = tsIdx;
                            }
                        }
                        if (tsIdxRange[0] > tsIdxRange[1])
                        {
                            int tmp = tsIdxRange[0];
                            tsIdxRange[0] = tsIdxRange[1];
                            tsIdxRange[1] = tmp;
                        }
                        for (int tsIdx = tsIdxRange[0]; tsIdx <= tsIdxRange[1]; tsIdx++)
                        {
                            tsIdxList.Add(tsIdx);
                        }
                    }
                    else
                    {
                        int tsIdx = int.Parse(c1.Value);
                        if ((tsIdx < 0) || (tsIdx >= nbrTestSuites))
                        {
                            throw new ArgumentException();
                        }
                        tsIdxList.Add(tsIdx);
                    }
                }
            }

            return tsIdxList.ToArray();
        }

        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    ITestSuite[] testSuites =
                        {
                            TestSuiteFactory.Create<DataTypesTestSuite>(),
                            TestSuiteFactory.Create<StringsTestSuite>(),
                            TestSuiteFactory.Create<DateTimeTestSuite>(),
                            TestSuiteFactory.Create<LoopingTestSuite>(),
                            TestSuiteFactory.Create<ClassesTestSuite>(),
                            TestSuiteFactory.Create<ExtensionMethodsTestSuite>(),
                            TestSuiteFactory.Create<InterfaceTestSuite>(),
                            TestSuiteFactory.Create<GenericsTestSuite>(),
                            TestSuiteFactory.Create<CollectionsTestSuite>(),
                            TestSuiteFactory.Create<DelegatesTestSuite>(),
                            TestSuiteFactory.Create<EventsTestSuite>(),
                            TestSuiteFactory.Create<LambdaExpressionsTestSuite>(),
                            TestSuiteFactory.Create<LINQTestSuite>(),
                            TestSuiteFactory.Create<MultiThreadingTestSuite>(),
                            TestSuiteFactory.Create<OperatorOverloadingTestSuite>(),
                            TestSuiteFactory.Create<ADODotNetTestSuite>(),
                            TestSuiteFactory.Create<EntityFrameworkTestSuite>(),
                            TestSuiteFactory.Create<JSONSerializationTestSuite>()
                        };

                    for (int i = 0; i < testSuites.Length; i++)
                    {
                        Console.WriteLine($"{i}: {testSuites[i].Name}");
                    }
                    Console.Write($"Enter Suites to Execute (or {testSuites.Length} to exit): ");
                    string strInput = Console.ReadLine();

                    int[] tsIdxs = GetTestSuiteIndexes(strInput, testSuites.Length + 1);

                    if (tsIdxs.Contains<int>(testSuites.Length))
                    {
                        break;
                    }

                    foreach (int tsIdx in tsIdxs)
                    {
                        testSuites[tsIdx].Run();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine();
            }
        }
    }
}
