using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace CSharp.TestSuites
{
    public class InterfaceTestSuite : TestSuite, ITestSuite
    {
        public void Run()
        {
            WriteTestSuiteName();

            Interface1();
            Interface2();
            Interface3();
            Interface4();
            Interface5();
            Interface6();
            Interface7();

            WriteTestSuiteName();
        }

        private interface IReport
        {
            DateTime Date { get; set; }

            void Print();
        }

        private abstract class Report : IReport
        {
            public abstract DateTime Date { get; set; }

            public virtual void Print()
            {
                Console.WriteLine($"In Report::Print {Date}");
            }
        }

        // Implicitly implements IReport
        private class CustomerReport : Report, IReport
        {
            public override DateTime Date { get; set; }

            public override void Print()
            {
                Console.WriteLine($"In CustomerReport::Print {Date}");
            }
        }

        // Only implements the IReport interface
        private class OrdersReport : IReport
        {
            public DateTime Date { get; set; }

            public void Print()
            {
                Console.WriteLine($"In OrdersReport::Print {Date}");
            }
        }

        // Only derives from the Report class
        private class FinanceReport : Report
        {
            public override DateTime Date { get; set; }

            // Hides the base class implementation
            public new void Print()
            {
                Console.WriteLine($"In FinanceReport::Print {Date}");
            }
        }

        private class ReportFactory
        {
            public static TReport Create<TReport>() where TReport : IReport, new()
            {
                return new TReport() { Date = DateTime.Now };
            }
        }

        private void Interface1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            // Report is an abstract class that cannot be instantiated
            //Report report = ReportFactory.Create<Report>();

            CustomerReport customerReport = ReportFactory.Create<CustomerReport>();
            customerReport.Print();

            OrdersReport ordersReport = ReportFactory.Create<OrdersReport>();
            ordersReport.Print();

            FinanceReport financeReport = ReportFactory.Create<FinanceReport>();
            financeReport.Print();
        }

        private interface IDimensions
        {
            float Length();
            float Width();
        }

        /*
         * A class that implements an interface can explicitly implement a member of that interface. 
         * When a member is explicitly implemented, it cannot be accessed through a class instance, 
         * but only through an instance of the interface.
         */
        private class Box : IDimensions
        {
            float lengthInches;
            float widthInches;

            public Box(float length, float width)
            {
                lengthInches = length;
                widthInches = width;
            }

            // Explicit interface member implementation: 
            float IDimensions.Length()
            {
                return lengthInches;
            }
            // Explicit interface member implementation:
            float IDimensions.Width()
            {
                return widthInches;
            }
        }

        private void Interface2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            // Declare a class instance "myBox":
            Box myBox = new Box(30.0f, 20.0f);
            // Declare an interface instance "myDimensions":
            IDimensions myDimensions = myBox;

            // Print out the dimensions of the box:
            /* 
             * The following commented lines would produce compilation 
               errors because they try to access an explicitly implemented
               interface member from a class instance:
            */
            // Console.WriteLine($"Length: { myBox.Length() }, Width: { myBox.Width() }");

            /* Print out the dimensions of the box by calling the methods from an instance of the interface: */
            Console.WriteLine($"Length: {myDimensions.Length()}, Width: {myDimensions.Width()}");
        }

        private class ObjectToCompare : IComparable<ObjectToCompare>
        {
            public int Value { get; private set; }

            public ObjectToCompare(int value)
            {
                Value = value;
            }

            public int CompareTo(ObjectToCompare obj)
            {
                return obj.Value == Value ? 0 : 1;
            }
        }

        private void Interface3()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            ObjectToCompare compObj1 = new ObjectToCompare(1);
            ObjectToCompare compObj2 = new ObjectToCompare(2);

            int comp = compObj1.CompareTo(compObj2);

            Console.WriteLine($"comp={comp}");
        }

        private class ObjectComparer : IComparer<ObjectToCompare>
        {
            public int Value { get; set; }

            public int Compare(ObjectToCompare x, ObjectToCompare y)
            {
                return x.CompareTo(y);
            }
        }

        private void Interface4()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            ObjectToCompare compObj1 = new ObjectToCompare(1);
            ObjectToCompare compObj2 = new ObjectToCompare(2);

            ObjectComparer objCompr = new ObjectComparer();
            int comp = objCompr.Compare(compObj1, compObj2);

            Console.WriteLine($"comp={comp}");
        }

        private class ObjectToEquate : IEquatable<ObjectToEquate>
        {
            public int Value { get; private set; }

            public ObjectToEquate(int value)
            {
                Value = value;
            }

            public bool Equals(ObjectToEquate obj)
            {
                return obj.Value == Value;
            }
        }

        private void Interface5()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            ObjectToEquate compObj1 = new ObjectToEquate(1);
            ObjectToEquate compObj2 = new ObjectToEquate(2);

            bool eq = compObj1.Equals(compObj2);

            Console.WriteLine($"eq={eq}");
        }

        private class Node : IEnumerable<Node>
        {
            public IEnumerator<Node> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        private class ObjectToDispose : IDisposable
        {
            public string Name { get; private set; }

            public ObjectToDispose(string name)
            {
                Name = name;
            }

            ~ObjectToDispose()
            {
                // Free Unmanaged resources only
                FreeResources(false);
            }

            public void Dispose()
            {
                Console.WriteLine($"Disposing {Name}");
                // Free Managed and Unmanaged resources
                FreeResources(true);
            }

            private bool ResourcesFreed = false;
            private void FreeResources(bool freeManagedResources)
            {
                if (!ResourcesFreed)
                {
                    Console.WriteLine($"Freeing Resources for {Name}");
                    if (freeManagedResources)
                    {
                        // Free managed resources here
                    }
                    // Free unmanaged resources here
                    ResourcesFreed = true;
                    GC.SuppressFinalize(this);
                }
            }
        }

        private void Interface6()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            using (ObjectToDispose obj = new ObjectToDispose("obj"))
            {
                Console.WriteLine($"Using {obj.Name}");
            }
        }

        private interface IHouse
        {
            void HouseMethod();
        }

        private interface IBoat
        {
            void BoatMethod();
        }

        private class HouseBoat :
            IHouse, // Implemented Implicitly
            IBoat   // Implemented Explicitly
        {
            public void HouseMethod()
            {
                Console.WriteLine("HouseMethod");
            }

            void IBoat.BoatMethod()
            {
                Console.WriteLine("BoatMethod");
            }
        }

        private void Interface7()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            HouseBoat hb = new HouseBoat();
            hb.HouseMethod();
            //hb.BoatMethod(); not visible

            IHouse h = hb;
            h.HouseMethod();

            IBoat b = hb;
            b.BoatMethod();
        }
    }
}
