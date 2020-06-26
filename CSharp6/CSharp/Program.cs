using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CSharp
{
    #region Test Suites

    public class TestSuiteException : Exception
    {
        public TestSuiteException() : base("Unknown Test Suite Type")
        {
        }
    }

    public abstract class TestSuite
    {
        public string Name { get { return this.GetType().Name;  } }

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

    public interface ITestSuite
    {
        string Name { get; }
        void Run();
    }

    public static class TestSuiteFactory
    {
        public static TTestSuite Create<TTestSuite>() where TTestSuite : TestSuite, ITestSuite, new()
        {
            return new TTestSuite();
        }
    }

    #endregion

    #region Data Types

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
            Console.WriteLine(string.Format("myInt={0}", myInt));
        }

        private void DataTypes2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            int myInt2 = int.Parse("123");
            Console.WriteLine(string.Format("myInt2={0}", myInt2));

            int myInt3;
            bool parsed = int.TryParse("9876543210", out myInt3);
            Console.WriteLine(string.Format("parsed={0}, myInt3={1}", parsed, myInt3));
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

    #endregion

    #region Strings

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
            Console.WriteLine(string.Format("str={0}", str));
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

    #endregion

    #region Date Time

    public class DateTimeTestSuite : TestSuite, ITestSuite
    {
        public void Run()
        {
            WriteTestSuiteName();

            DateTime1();
            DateTime2();
            DateTime3();

            WriteTestSuiteName();
        }

        private void DateTime1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            DateTime dt = DateTime.Now;

            Console.WriteLine(string.Format("Date Time:          {0}", dt));
        }

        private void DateTime2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            DateTimeOffset localTime = DateTimeOffset.Now;  // set to the current date and time, with the offset set to the local time's offset from Coordinated Universal Time (UTC)

            Console.WriteLine(string.Format("Local Time:          {0}", localTime));
            Console.WriteLine(string.Format("Difference from UTC: {0}", localTime.Offset));

            DateTimeOffset utcTime = DateTimeOffset.UtcNow; // set to the current Coordinated Universal Time (UTC) date and time and whose offset is TimeSpan.Zero

            Console.WriteLine(string.Format("UTC:                 {0}", utcTime));
            Console.WriteLine(string.Format("Difference from UTC: {0}", utcTime.Offset));
        }

        private void DateTime3()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            ReadOnlyCollection<TimeZoneInfo> tziList = TimeZoneInfo.GetSystemTimeZones();

            DateTimeOffset localTime = DateTimeOffset.Now;  // set to the current date and time, with the offset set to the local time's offset from Coordinated Universal Time (UTC)
            foreach (TimeZoneInfo tzi in tziList)
            {
                Console.WriteLine(tzi.Id);
                Console.WriteLine(tzi.DisplayName);
                Console.WriteLine(tzi.BaseUtcOffset);
                Console.WriteLine(tzi.SupportsDaylightSavingTime);
                Console.WriteLine();

                DateTimeOffset dto = TimeZoneInfo.ConvertTime(localTime, tzi);
                Console.WriteLine(string.Format("Current Time in TZ: {0}", dto));
                Console.WriteLine();
            }
        }
    }

    #endregion

    #region Looping

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
                Console.WriteLine(string.Format("loop = {0}", i));
            } while (++i < 10);
        }
    }

    #endregion

    #region Classes

    // public, internal
    // abstract, sealed
    // static
    // unsafe, partial
    public abstract class Asset
    {
        // Properties
        // static
        // public, internal, private, protected
        // new, virtual, abstract, override, sealed
        // unsafe, extern
        public abstract decimal NetValue { get; }
        public string Name { get; private set; }
        public string NickName { get; set; }

        // Constructor
        // public, internal, private, protected
        // unsafe, extern
        public Asset(string name)
        {
            Name = name;
            Console.WriteLine(string.Format("In Asset Constructor: {0}", Name));
        }

        // static
        // public, internal, private, protected
        // new, virtual, abstract, override, sealed
        // partial
        // unsafe, extern
        public virtual void Print()
        {
            Console.WriteLine(string.Format("Asset Name: {0}, Net Value: {1}", Name, NetValue));
        }

        // overloading
        void Foo(int x) { }
        void Foo(double x) { }
    }

    public class Stock : Asset
    {
        public long SharesOwned { get; private set; }
        public decimal Price { get; private set; }
        public decimal PurchasePrice { get; private set; }
        public override decimal NetValue
        {
            get
            {
                return SharesOwned * (Price - PurchasePrice);
            }
        }

        // Constructor
        public Stock(string name) : base(name)
        {
            SharesOwned = 0;
            Price = 0M;
            PurchasePrice = 0M;
            Console.WriteLine(string.Format("In Stock Constructor: {0}", Name));
        }
        public Stock(string name, long sharesOwned, decimal price, decimal purchasePrice) : this(name)
        {
            SharesOwned = sharesOwned;
            Price = price;
            PurchasePrice = purchasePrice;
            Console.WriteLine(string.Format("In Stock Constructor: {0}, {1}, {2}", SharesOwned, Price, PurchasePrice));
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine(string.Format("Stock Shares Owned: {0}, Price: {1}, Purchase Price: {2}", SharesOwned, Price, PurchasePrice));
        }
    }

    public class House : Asset
    {
        public decimal Valuation { get; private set; }
        public decimal MortgageBalance { get; private set; }
        public override decimal NetValue
        {
            get
            {
                return Valuation - MortgageBalance;
            }
        }

        // Constructor
        public House(string name, decimal valuation, decimal mortgageBalance) : base(name)
        {
            Valuation = valuation;
            MortgageBalance = mortgageBalance;
            Console.WriteLine(string.Format("In House Constructor: {0}, {1}", Valuation, MortgageBalance));
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine(string.Format("House Valuation: {0}, Mortgage Balance: {1}", Valuation, MortgageBalance));
        }
    }

    public class ClassesTestSuite : TestSuite, ITestSuite
    {
        public void Run()
        {
            WriteTestSuiteName();

            Classes1();
            Classes2();
            Classes3();

            WriteTestSuiteName();
        }

        static public void OverloadableMethod(Asset a)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + " Asset");
        }
        static public void OverloadableMethod(Stock s)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + " Stock");
        }
        static public void OverloadableMethod(House h)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + " House");
        }

        private void Classes1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Stock stock = new Stock("IBM", 100, 14.0m, 10.0m);
            House house = new House("6000 Shepherd Mountain Cv., UNIT 104", 149000, 124743.53m);

            stock.Print();
            house.Print();

            // Polymorphism - implicit upcast
            Asset a1 = stock;
            Asset a2 = house;

            // Polymorphism - explict downcast - can throw exception
            try { Stock s = (Stock)a1; } catch (Exception ex) { Console.WriteLine("Downcast of Asset to Stock failed: " + ex.Message); }
            try { Stock s = (Stock)a2; } catch (Exception ex) { Console.WriteLine("Downcast of Asset to Stock failed: " + ex.Message); }
            try { House h = (House)a1; } catch (Exception ex) { Console.WriteLine("Downcast of Asset to House failed: " + ex.Message); }
            try { House h = (House)a2; } catch (Exception ex) { Console.WriteLine("Downcast of Asset to House failed: " + ex.Message); }

            // as operartor
            Stock s1 = a1 as Stock;
            Stock s2 = a2 as Stock;
            if (s1 == null) { Console.WriteLine("s1 is null"); } else { Console.WriteLine("s1 is NOT null"); }
            if (s2 == null) { Console.WriteLine("s2 is null"); } else { Console.WriteLine("s2 is NOT null"); }

            // is operator
            if (s1 is Stock) { Console.WriteLine("s1 is Stock"); } else { Console.WriteLine("s1 is NOT Stock"); }
            if (s2 is Stock) { Console.WriteLine("s2 is Stock"); } else { Console.WriteLine("s2 is NOT Stock"); }

            OverloadableMethod(stock);
            OverloadableMethod(house);
            OverloadableMethod(a1);
            OverloadableMethod(a2);
            OverloadableMethod((dynamic)a1);
            OverloadableMethod((dynamic)a2);
        }

        private class IPAddress
        {
            private int[] ip;
            public int this[int index]
            {
                get
                {
                    return ip[index];
                }
                set
                {
                    if (value == 0 || value == 1)
                    {
                        ip[index] = value;
                    }
                    else
                    {
                        throw new Exception("Invalid value");
                    }
                }
            }

            public IPAddress()
            {
                ip = new int[32];
                for (int i = 0; i < 32; i++)
                {
                    ip[i] = 0;
                }
            }
        }

        private void Classes2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            IPAddress ipAddress = new IPAddress();
            for (int i = 0; i < 32; i++)
            {
                ipAddress[i] = 0;
            }
        }

        private abstract class Person
        {
            string Name { get; set; }
        }

        private class Employee : Person
        {
        }

        private class Manager : Employee
        {
        }

        private class Athlete : Person
        {
            public string Sport { get; private set; }
            public Athlete(string sport)
            {
                Sport = sport;
            }
        }

        private void Classes3()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Employee dan = new Employee();
            Person danAsPerson = dan;   // Widening. Don't need cast (implicit conversion), because Employee is a subclass of Person
            Employee danAsEmployee = (Employee) danAsPerson;   // Narrowing. Need cast (explicit conversion), but works because dan is an Employee
            Console.WriteLine((dan is Manager) ? "Dan is Manager" : "Dan is NOT Manager");
            Manager danAsManager = dan as Manager;  // Narrowing
            if (danAsManager == null)
            {
                Console.WriteLine("danAsManager == null");
            }
            try
            {
                Manager danAsManager2 = (Manager)dan;   // Generates a Runtime exception because dan is not a manager
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("danAsManager2 Exception: {0}", ex.Message));
            }

            Manager cindy = new Manager();
            Employee cindyAsEmployee = cindy;   // Widening.  Don't need cast (implicit conversion), because Manager is a subclass of Employee
            Person cindyAsPerson = cindy;   // Widening. Don't need cast (implicit conversion), because Manager is a subclass of Person
            Person cindyAsPerson2 = cindyAsEmployee;   // Widening. Don't need cast (implicit conversion), because Employee is a subclass of Person

            Athlete joe = new Athlete("Track");
            Person joeAsPerson = joe;   // Widening
            try
            {
                Employee joeAsEmployee = (Employee)joeAsPerson; // Generates a Runtime exception because joe is not an Employee
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("joeAsEmployee Exception: {0}", ex.Message));
            }
        }
    }

    #endregion

    #region Extension Methods

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
            Console.WriteLine(string.Format("Word count of s is {0}", s.WordCount()));
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

    #endregion

    #region Interfaces

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
                Console.WriteLine(string.Format("In Report::Print {0}", Date));
            }
        }

        // Implicitly implements IReport
        private class CustomerReport : Report, IReport
        {
            public override DateTime Date { get; set; }

            public override void Print()
            {
                Console.WriteLine(string.Format("In CustomerReport::Print {0}", Date));
            }
        }

        // Only implements the IReport interface
        private class OrdersReport : IReport
        {
            public DateTime Date { get; set; }

            public void Print()
            {
                Console.WriteLine(string.Format("In OrdersReport::Print {0}", Date));
            }
        }

        // Only derives from the Report class
        private class FinanceReport : Report
        {
            public override DateTime Date { get; set; }

            // Hides the base class implementation
            public new void Print()
            {
                Console.WriteLine(string.Format("In FinanceReport::Print {0}", Date));
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
            Console.WriteLine(string.Format("Length: {0}, Width: {1}", myDimensions.Length(), myDimensions.Width()));
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

            Console.WriteLine(string.Format("comp={0}", comp));
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

            Console.WriteLine(string.Format("comp={0}", comp));
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

            Console.WriteLine(string.Format("eq={0}", eq));
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
                Console.WriteLine(string.Format("Disposing {0}", Name));
                // Free Managed and Unmanaged resources
                FreeResources(true);
            }

            private bool ResourcesFreed = false;
            private void FreeResources(bool freeManagedResources)
            {
                if (!ResourcesFreed)
                {
                    Console.WriteLine(string.Format("Freeing Resources for {0}", Name));
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
                Console.WriteLine(string.Format("Using {0}", obj.Name));
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

    #endregion

    #region Generics

    class Node<T>
    {
        public T Item { get; set; }
        public Node<T> Next = null;

        public Node(T item)
        {
            Item = item;
        }
    }

    public class SAOLinkedList<T> // : IList<T>
    {
        private Node<T> head = null;
        private Node<T> tail = null;

        public void Add(T item)
        {
            var node = new Node<T>(item);

            if (head == null)
            {
                head = node;
            }
            else
            {
                tail.Next = node;
            }

            tail = node;
        }

        // Other IList members...
    }

    public class GenericsTestSuite : TestSuite, ITestSuite
    {
        public void Run()
        {
            WriteTestSuiteName();

            Generics1();

            WriteTestSuiteName();
        }

        private void Generics1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            var llist = new SAOLinkedList<string>();
            llist.Add("Jamie");
            llist.Add("Ron");
        }
    }

    #endregion

    #region Collections

    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Description { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CustomerViewModel
    {
        public string Name { get; set; }
    }

    public class Company
    {
        public string Name { get; set; }

        private int MaxCustomerId = 0;
        public List<Customer> Customers { get; private set; }

        public List<Order> Orders { get; private set; }
        private int MaxOrderId = 0;

        public Company()
        {
            Customers = new List<Customer>();

            Customer Mary = AddCustomer("Mary");
            Customer Gary = AddCustomer("Mark");
            Customer Jennifer = AddCustomer("Jennifer");
            Customer Frank = AddCustomer("Frank");

            Orders = new List<Order>();
            AddOrder(Mary.Id, "Shoes");
            AddOrder(Gary.Id, "Purse");
            AddOrder(Jennifer.Id, "Headphones");
            AddOrder(Frank.Id, "Laptop");
        }

        public Customer AddCustomer(string name)
        {
            Customer customer = new Customer { Id = MaxCustomerId++, Name = name };
            Customers.Add(customer);
            return customer;
        }

        public Order AddOrder(int customerId, string description)
        {
            Order order = new Order { Id = MaxOrderId++, CustomerId = customerId, Description = description };
            Orders.Add(order);
            return order;
        }
    }

    public class CollectionsTestSuite : TestSuite, ITestSuite
    {
        public void Run()
        {
            WriteTestSuiteName();

            Collections1();
            Collections2();
            Collections3();
            Collections4();
            Collections5();
            Collections6();
            Collections7();

            WriteTestSuiteName();
        }

        private void Collections1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            IList<Company> companies = new List<Company>
            {
                new Company { Name = "Syncfusion" },
                new Company { Name = "Microsoft" },
                new Company { Name = "Acme" }
            };

            foreach (Company company in companies)
            {
                Console.WriteLine(company.Name);
            }

            List<Company> companiesList = companies as List<Company>;
            companiesList.ForEach(company => Console.WriteLine(company.Name));

            // Delegate definition
            // public delegate void Action<in T>(T obj);
            Action<Company>
                // delegate instance
                action =
                // Lamda Expression:
                // Parameters
                company
                // Lambda operator
                =>
                // Method body
                Console.WriteLine(company.Name);

            // akin to calling action(company); for each company in the list where the ForEach provides the company argument
            companiesList.ForEach(action);
        }

        private void Collections2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
            Customer jane = new Customer { Id = 0, Name = "Jane" };
            Customer joe = new Customer { Id = 1, Name = "Joe" };
            customers.Add(jane.Id, jane);
            customers[joe.Id] = joe;
            foreach (int key in customers.Keys)
            {
                // customers[key] returns a Customer object (i.e., the value associated with the key)
                Console.WriteLine(customers[key].Name);
            }

            Dictionary<int, Customer> customers2 = new Dictionary<int, Customer>();
            customers2.Add(0, new Customer { Id = 0, Name = "Chris" });
            customers2.Add(1, new Customer { Id = 1, Name = "Alex" });
            customers2.Add(2, new Customer { Id = 2, Name = "Bill" });
            foreach (int key in customers2.Keys)
            {
                Console.WriteLine(customers2[key].Name);
            }
        }

        private void Collections3()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            foreach (int fib in Fibs(6))
            {
                Console.Write(fib + " ");
            }
            Console.WriteLine();
        }

        private IEnumerable<int> Fibs(int fibCount)
        {
            for (int i = 0, prevFib = 1, curFib = 1; i < fibCount; i++)
            {
                // use a yield return statement to return each element one at a time.
                yield return prevFib;

                int savePrevFib = prevFib;
                prevFib = curFib;
                curFib += savePrevFib;
            }
        }

        private void Collections4()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            foreach (string s in Foo(3))
            {
                Console.WriteLine(s);
            }
        }

        static private List<string> foos = new List<string>() { "One", "Two", "Three", "Four", "Five" };

        private IEnumerable<string> Foo(int stopAt = -1)
        {
            foreach (string s in foos)
            {
                yield return s;
                if (stopAt == foos.IndexOf(s))
                {
                    Console.WriteLine(string.Format("In Foo stopAt = {0}", stopAt));
                    // The iterator block will exit and not return more elements
                    yield break;
                }
            }
        }

        private void Collections5()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            // public class ArrayList : IList, ICollection, IEnumerable, ICloneable
            Console.WriteLine("ArrayList");
            ArrayList arrayList = new ArrayList() { "One", "Two", "Three", "Four", "Five" };
            foreach (string s in arrayList) { Console.WriteLine(s); }
            arrayList.Remove("One");
            arrayList.Add("Six");
            foreach (string s in arrayList) { Console.WriteLine(s); }

            // public class List<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection, IReadOnlyList<T>, IReadOnlyCollection<T>
            Console.WriteLine("List");
            List<string> list = new List<string>() { "One", "Two", "Three", "Four", "Five" };
            foreach (string s in list) { Console.WriteLine(s); }
            list.Remove("One");
            list.Add("Six");
            foreach (string s in list) { Console.WriteLine(s); }

            // public class LinkedList<T> : ICollection<T>, IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>, ISerializable, IDeserializationCallback
            Console.WriteLine("LinkedList");
            LinkedList<string> linkedList = new LinkedList<string>();
            linkedList.AddLast("One");
            linkedList.AddLast("Two");
            linkedList.AddLast("Three");
            linkedList.AddLast("Four");
            linkedList.AddLast("Five");
            foreach (string s in linkedList) { Console.WriteLine(s); }
            linkedList.Remove("One");
            linkedList.AddLast("Six");
            foreach (string s in linkedList) { Console.WriteLine(s); }

            Console.WriteLine("LinkedList as ICollection<>");
            LinkedList<string> linkedList2 = new LinkedList<string>();
            ICollection<string> linkedListAsICollection = linkedList2;
            linkedListAsICollection.Add("One");
            linkedListAsICollection.Add("Two");
            linkedListAsICollection.Add("Three");
            linkedListAsICollection.Add("Four");
            linkedListAsICollection.Add("Five");
            foreach (string s in linkedListAsICollection) { Console.WriteLine(s); }
            linkedListAsICollection.Remove("One");
            linkedListAsICollection.Add("Six");
            foreach (string s in linkedListAsICollection) { Console.WriteLine(s); }

            // public class Queue<T> : IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
            Console.WriteLine("Queue");
            Queue<string> queue = new Queue<string>();
            queue.Enqueue("One");
            queue.Enqueue("Two");
            queue.Enqueue("Three");
            queue.Enqueue("Four");
            queue.Enqueue("Five");
            foreach (string s in queue) { Console.WriteLine(s); }
            queue.Dequeue();
            queue.Enqueue("Six");
            foreach (string s in queue) { Console.WriteLine(s); }

            // public class Stack<T> : IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
            Console.WriteLine("Stack");
            Stack<string> stack = new Stack<string>();
            stack.Push("One");
            stack.Push("Two");
            stack.Push("Three");
            stack.Push("Four");
            stack.Push("Five");
            foreach (string s in stack) { Console.WriteLine(s); }
            stack.Pop();
            stack.Push("Six");
            foreach (string s in stack) { Console.WriteLine(s); }

            // public class HashSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable, ISerializable, IDeserializationCallback, ISet<T>, IReadOnlyCollection<T>
            Console.WriteLine("HashSet");
            HashSet<string> hashSet = new HashSet<string>();
            hashSet.Add("One");
            hashSet.Add("Two");
            hashSet.Add("Three");
            hashSet.Add("Four");
            hashSet.Add("Five");
            foreach (string s in hashSet) { Console.WriteLine(s); }
            hashSet.Remove("One");
            hashSet.Add("Six");
            foreach (string s in hashSet) { Console.WriteLine(s); }

            // public class Hashtable : IDictionary, ICollection, IEnumerable, ISerializable, IDeserializationCallback, ICloneable
            Console.WriteLine("Hashtable");
            Hashtable hashTable = new Hashtable();
            hashTable.Add(1, "One");
            hashTable.Add("One", 1);
            hashTable.Add(2, "Two");
            hashTable.Add("Two", 2);
            Console.WriteLine(hashTable[1]);
            Console.WriteLine(hashTable["One"]);
            Console.WriteLine(hashTable[2]);
            Console.WriteLine(hashTable["Two"]);
            hashTable.Remove(1);
            hashTable.Remove("Two");
            Console.WriteLine(hashTable[1]);
            Console.WriteLine(hashTable["One"]);
            Console.WriteLine(hashTable[2]);
            Console.WriteLine(hashTable["Two"]);
        }

        private class Animal
        {
            public string Name;
            public int Popularity;
            public Zoo Zoo { get; internal set; }

            public Animal(string name, int popularity)
            {
                Name = name;
                Popularity = popularity;
            }
        }

        // public class Collection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection, IReadOnlyList<T>, IReadOnlyCollection<T>
        private class AnimalCollection : Collection<Animal>
        {
            Zoo Zoo;
            public AnimalCollection(Zoo zoo)
            {
                Zoo = zoo;
            }

            protected override void InsertItem(int index, Animal item)
            {
                base.InsertItem(index, item);
            }

            protected override void SetItem(int index, Animal item)
            {
                base.SetItem(index, item);
            }

            protected override void RemoveItem(int index)
            {
                base.RemoveItem(index);
            }

            protected override void ClearItems()
            {
                base.ClearItems();
            }
        }

        private class Zoo
        {
            public AnimalCollection Animals { get; private set; }
            public Zoo()
            {
                Animals = new AnimalCollection(this);
            }
        }

        private void Collections6()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Zoo zoo = new Zoo();
            zoo.Animals.Add(new Animal("Kangaroo", 10));
            zoo.Animals.Add(new Animal("Mr. Sea Lion", 20));
            foreach (Animal a in zoo.Animals)
            {
                Console.WriteLine(a.Name);
            }
        }

        private void Collections7()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            int[] array1 = new int[10];
            for (int i = 0; i < 10; i++)
            {
                array1[i] = i;
            }

            int[,] array2 = new int[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    array2[i, j] = i * j;
                }
            }
        }
    }

    #endregion

    #region Delegates

    /*
     * 1) Declare a delegate type:  e.g., private delegate double Add (double num1, double num2);
     *    'Add' is the name of the (reference) type
     * 2) Implement a function that conforms to the delegate type's method signature (return value and parameters)
     * 3) Define a delegate instance of the delegate type (e.g., 'Add')
     * 4) assign, or increment the method name to the instance
     * 5) Use the delegate instance as a method invocation
     */

    public class DelegatesTestSuite : TestSuite, ITestSuite
    {
        public void Run()
        {
            WriteTestSuiteName();

            Delegates1();
            Delegates2();
            Delegates3();
            Delegates4();
            Delegates5();
            Delegates6();
            Delegates7();
            Delegates8();

            WriteTestSuiteName();
        }

        // A delegate definition is a reference type
        // Add is the (reference) type name for a function that takes 2 doubles as parameters, and returns a double
        private delegate double Add(double num1, double num2);

        // AddFunc is a function that takes 2 doubles as parameters, and returns a double
        // Conforms to delegate function Add signature
        private double AddFunc(double num1, double num2)
        {
            return num1 + num2;
        }

        private void Delegates1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            // Add is the delegate reference type
            // add is a delegate instance
            Add add = AddFunc;

            double num = 1.23;
            for (int i = 0; i < 10; i++)
            {
                // Use the add delegate instance to call AddFunc()
                Console.WriteLine(string.Format("Adding {0} & {1} yields {2}", num, i, add(num, i)));
            }
        }

        // 1) Define the delegate Type
        private delegate void FuncDelegateType(int n);

        private void Func1(int x)
        {
            Console.WriteLine(string.Format("In Func1: {0}", x));
        }

        private void Func2(int y)
        {
            Console.WriteLine(string.Format("In Func2: {0}", y));
        }

        private void Delegates2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            // Multicast capability
            FuncDelegateType fdt = Func1;
            fdt += Func2;
            fdt += Func1;

            // Call all methods in sequence they were added
            fdt(10);
        }

        // Definition of a delegate to a method with a Generic type
        private delegate void GenericFunc<T>(T arg);

        // Defined in System.Action
        // public delegate void Action<in T>(T obj);

        private void GenericFunc1<T>(T arg)
        {
            Console.WriteLine("In GenericFunc1");
        }

        private void Delegates3()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            GenericFunc<int> gf = GenericFunc1;
            gf(10);

            Action<int> af = GenericFunc1;
            af(10);
        }

        private delegate T Transformer<T>(T arg);

        private void Transform<T>(T[] values, Transformer<T> t)
        {
            for (int i = 0; i < values.Length; i++)
            {
                // t is a delegate instance
                values[i] = t(values[i]);
            }
        }

        private int Square(int n) { return n * n; }

        private void Delegates4()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            int[] values = { 1, 2, 3 };
            foreach (int i in values) { Console.Write(i + " "); }
            Console.WriteLine();
            Transform(values, Square);
            foreach (int i in values) { Console.Write(i + " "); }
            Console.WriteLine();
        }

        // Defined in System.Func
        // public delegate TResult Func<in T, out TResult>(T arg);
        private void TransformUsingFunc<T>(T[] values, Func<T, T> t)
        {
            for (int i = 0; i < values.Length; i++)
            {
                // t is a delegate instance
                values[i] = t(values[i]);
            }
        }

        private void Delegates5()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            int[] values = { 1, 2, 3 };
            foreach (int i in values) { Console.Write(i + " "); }
            Console.WriteLine();
            TransformUsingFunc(values, Square);
            foreach (int i in values) { Console.Write(i + " "); }
            Console.WriteLine();
        }

        private class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        private class Employee : Person
        {
            public int EmployeeId { get; set; }
        }

        private class Manager : Employee
        {
            public int DirectReports { get; set; }
        }

        private delegate Employee ReturnEmployeeDelegate();

        private Person ReturnPerson()
        {
            return new Person();
        }

        private Manager ReturnManager()
        {
            return new Manager();
        }

        private delegate void EmployeeParameterDelegate(Employee employee);

        private void PersonParameter(Person person)
        {
        }

        private void ManagerParameter(Manager manager)
        {
        }

        private void Delegates6()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            // Covariance: Lets a method return a value from a SUBCLASS of the result expected by a delegate
            // Return Type -> SUBCLASS

            // ReturnEmployeeDelegate expects to return a Employee

            // Not allowed: we are setting the variable to a method that returns an Person (SUPERCLASS)
            //ReturnEmployeeDelegate ReturnEmployeeMethod = ReturnPerson;

            // Allowed: we are setting the variable to a method that returns an Manager (SUBCLASS)
            ReturnEmployeeDelegate ReturnEmployeeMethod2 = ReturnManager;

            // Contravariance: Lets a method take parameters that are from a SUPERCLASS of the type expected by a delegate
            // Parameter -> SUPERCLASS

            // EmployeeParameterDelegate expects an Employee as a parameter

            // Allowed: we are setting the variable to a method that takes a Person as a parameter (SUPERCLASS)
            EmployeeParameterDelegate EmployeeParameterMethod = PersonParameter;

            // Not allowed since Manager is a SUBCLASS of Employee
            //EmployeeParameterDelegate EmployeeParameterMethod2 = ManagerParameter;
        }

        private struct DelegateTest
        {
            public Action action;
        }

        private void Delegates7()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            DelegateTest dt = new DelegateTest();
            dt.action = () => { Console.WriteLine("Hello"); };
            dt.action();

            List<Action> actionsList = new List<Action>();
            Action[] actionsArray = new Action[10];
        }

        private void Delegates8()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Func<float, float> result;
            result = (float x) => x * x;
            result = (x) => x * x;
            result = x => x * x;
            result = (float x) => { return (x * x); };
            result = x => { return (x * x); };
        }
    }

    #endregion

    #region Events

    /*
     * 1) Optionally, extend a class from EventArgs, e.g., ClickEventArgs
     * 2) Declare event handler delegate.
     *    e.g., public delegate void ClickEventHandler (object sender, ClickEventArgs args);
     *    'ClickEventHandler' is the name of the (reference) type
     * 3) Declare an event using the delegate type in the class that will publish the event: e.g., public event ClickEventHandler Clicked;
     * 4) Implement a method that conforms to delegate method signature: e.g., private void ClickHandlerFunc (object sender, ClickEventArgs args)
     * 5) Subscribe to event (by adding to the event member variable [delegate instance])
     * 6) Publish event
     */

    // 1) extend EventArgs
    public class ClickEventArgs : EventArgs
    {
        public string Name { get; private set; }

        public ClickEventArgs(string name)
        {
            Name = name;
        }
    }

    // 2) Define event handler method signature.  The method type name is 'ClickHandler'
    // Must have void return type
    // parameters must be (object, EventArgs) , or subclass as EventArgs, e.g., ClickEventArgs
    public delegate void ClickEventHandler(object sender, ClickEventArgs args);

    public class CalculatorButton
    {
        // 3) Define event (in the class that publishes the event)
        public event ClickEventHandler Clicked;

        public void SimulateClick(string name)
        {
            if (Clicked != null)
            {
                ClickEventArgs args = new ClickEventArgs(name);

                // 6) Publish Event
                Clicked(this, args);
            }
        }
    }

    public class EventsTestSuite : TestSuite, ITestSuite
    {
        public void Run()
        {
            WriteTestSuiteName();

            Events1();

            WriteTestSuiteName();
        }

        // 4) Conforms to delegate ClickHandler method Signature
        private void ClickHandlerFunc(object sender, ClickEventArgs args)
        {
            Console.WriteLine(string.Format("Caller is a CalculatorButton: {0}, and is named {1}", sender is CalculatorButton, args.Name));
        }

        private void Events1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            CalculatorButton calcBtn = new CalculatorButton();

            // 5) Subscribe to the event
            calcBtn.Clicked += ClickHandlerFunc;

            calcBtn.SimulateClick("SimulateClick");
        }
    }

    #endregion

    #region Lambda Expressions

    // A lambda expression is an anonymous function that you can use to create delegates

    public class LambdaExpressionsTestSuite : TestSuite, ITestSuite
    {
        public void Run()
        {
            WriteTestSuiteName();

            LambdaExpressions1();
            LambdaExpressions2();
            LambdaExpressions3();
            LambdaExpressions4();

            WriteTestSuiteName();
        }

        // public delegate void Action();

        private void LambdaExpressions1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            // Delegate definition
            Action
                // Method name (delegate instance)
                hello =
                // Lamda Expression:
                // Parameters
                ()
                // lambda operator
                =>
                // Method body
                Console.WriteLine("Hello!");

            // Use the delegate instance to call the anonymous method
            hello();
        }

        // public delegate bool Predicate<in T>(T obj);

        // Conforms to the Predicate signature, where T is a bool
        private bool validatorFunc(string word)
        {
            return word.Length > 3;
        }

        private void LambdaExpressions2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            // Delegate definition
            Predicate<string>
                // Method name (delegate instance)
                validator =
                // Lamda Expression:
                // Parameters
                word
                // lambda operator
                =>
                // Method body
                {
                    return word.Length > 3;
                };

            ValidateInput(validator);

            ValidateInput(validatorFunc);
        }

        private void ValidateInput(Predicate<string> validator)
        {
            string input = "Hello!";
            bool isValid = validator(input);
            Console.WriteLine(string.Format("Is Valid: {0}", isValid));
        }

        private void LambdaExpressions3()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            // Delegate definition
            ClickEventHandler
                // Method name (delegate instance)
                clickHandler =
                // Lamda Expression:
                // Parameters
                (object sender, ClickEventArgs args)
                // lambda operator
                =>
                // Method body
                {
                    Console.WriteLine(string.Format("Caller is a CalculatorButton: {0} and is named {1}", sender is CalculatorButton, args.Name));
                };

            CalculatorButton calcBtn = new CalculatorButton();

            calcBtn.Clicked += clickHandler;

            calcBtn.SimulateClick("SimulateClick LE 1");
            calcBtn.SimulateClick("SimulateClick LE 2");
        }

        private void LambdaExpressions4()
        {
            Func<float, float> f1 = (float x) => x * x;
            Func<float, float> f2 = x => x * x;
            Func<float, float> f3 = x => { return x * x; };
            Func<float, float, float> f4 = (x, y) => { return x * x; };
        }
    }

    #endregion

    #region LINQ

    public class LINQTestSuite : TestSuite, ITestSuite
    {
        public void Run()
        {
            WriteTestSuiteName();

            // Query Syntax
            LINQ_Basic();
            LINQ_Projection1();
            LINQ_ProjectionToAnonymousType();
            LINQ_Filtering();
            LINQ_Ordering();
            LINQ_Scalar();
            LINQ_ToList();
            LINQ_Let();
            LINQ_Into();
            LINQ_Join1();
            LINQ_Join2();
            LINQ_Join_Into();
            LINQ_Group();
            LINQ_Group_Ordered();
            LINQ_MultipleFromLetWhereClauses();
            LINQ_Scratch();

            // Method-based
            LINQ12();
            LINQ13();
            LINQ14();
            LINQ15();
            LINQ16();
            LINQ17();
            LINQ18();
            LINQ19();

            WriteTestSuiteName();
        }

        /// <summary>
        /// Uses query syntax to simply recreate the list of Customers
        /// </summary>
        private void LINQ_Basic()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            /*IEnumerable<Customer>*/ var customers =
                // From Clause: range variable.  The Type (Customer) is optional.  And collection (sequence)
                from /*Customer*/ c 
                in company.Customers
                // Select Clause:  what to query
                select c;

            foreach (Customer c in customers) { Console.WriteLine(c.Name); }
        }

        /// <summary>
        /// Uses query syntax to create a list of names from the input customer sequence.  This is called Projection.
        /// </summary>
        private void LINQ_Projection1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            /*IEnumerable<string>*/ var customerNames =
                // From Clause: range variable and collection (sequence)
                from /*Customer*/ c
                in company.Customers
                // Select Clause: what to query
                select c.Name;

            foreach (string customerName in customerNames) { Console.WriteLine(customerName); }
        }

        /// <summary>
        /// Uses query syntax to create a list of CustomerViewModel's from the input customer sequence.  This is called Projection.
        /// </summary>
        private void LINQ_ProjectionToAnonymousType()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            IEnumerable<CustomerViewModel> customerVMs =
                // From Clause range variable and collection (sequence)
                from /*Customer*/ c 
                in company.Customers
                // Select Clause what to query
                select new CustomerViewModel { Name = c.Name };

            foreach (CustomerViewModel customerVM in customerVMs) { Console.WriteLine(customerVM.Name); }
        }

        /// <summary>
        /// Use query syntax to  create a list of Customer's from the input customer sequence filtered by customer names.
        /// </summary>
        private void LINQ_Filtering()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            IEnumerable<Customer> customers =
                // From Clause: range variable.  The Type (Customer) is optional.  And collection (sequence)
                from /*Customer*/ c  
                in company.Customers
                // Filter Clause
                where c.Name.Length > 4 
                // Select Clause: what to query
                select c;

            foreach (Customer c in customers) { Console.WriteLine(c.Name); }
        }

        /// <summary>
        /// Use query syntax to  create a list of Customer's from the input customer sequence ordered by name descending
        /// </summary>
        private void LINQ_Ordering()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            IEnumerable<Customer> customers =
                // From Clause range variable and collection (sequence)
                from /*Customer*/ c
                in company.Customers
                // Order Clause
                orderby c.Name descending
                // Select Clause what to query
                select c;

            foreach (Customer c in customers) { Console.WriteLine(c.Name); }
        }

        /// <summary>
        /// Uses query syntax to count the number of Customers
        /// </summary>
        private void LINQ_Scalar()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            int nbrCustomers = (
                // From Clause: range variable.  The Type (Customer) is optional.  And collection (sequence)
                from /*Customer*/ c 
                in company.Customers
                // Select Clause: what to query
                select c).Count();

            Console.WriteLine(string.Format("Nbr. Customers={0}", nbrCustomers));
        }

        /// <summary>
        /// Uses query syntax to create a list of CustomerViewModel's from the input customer sequence converted to a list
        /// </summary>
        private void LINQ_ToList()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            List<Customer> customers = (
                // From Clause: range variable collection
                from c 
                in company.Customers
                // Select Clause: what to query
                select c).
                // convert to list
                ToList();

            customers.ForEach(c => Console.WriteLine(c.Name));
        }

        private class Ingredient
        {
            public string Name { get; set; }
            public int Calories { get; set; }
            public bool IsDairy { get; set; }

            public override string ToString()
            {
                return
                    "Name: " + Name + ", " +
                    "Calories: " + Calories + ", " +
                    "IsDairy: " + (IsDairy ? "true" : "false");
            }
        }

        class Kitchen
        {
            public Ingredient[] Ingredients { get; private set; }

            public Kitchen()
            {
                Ingredients = new Ingredient[]
                {
                    new Ingredient { Name = "Sugar", Calories = 500, IsDairy = false },
                    new Ingredient { Name = "Egg", Calories = 100, IsDairy = false },
                    new Ingredient { Name = "Milk", Calories = 150, IsDairy = true },
                    new Ingredient { Name = "Flour", Calories = 50, IsDairy = false },
                    new Ingredient { Name = "Butter", Calories = 200, IsDairy = true }
                };
            }
        }

        /// <summary>
        /// Uses syntax query with let clause
        /// </summary>
        private void LINQ_Let()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Kitchen kitchen = new Kitchen();

            IEnumerable<string> highCalorieIngredientNamesQuery =
                // range variable and collection
                from /*Ingredient*/ i
                in kitchen.Ingredients
                // let clause
                let isBad = i.Calories >= 150 && i.IsDairy
                // condition
                where isBad
                // ordering
                orderby i.Name
                // what to query
                select i.Name;

            foreach (string ingredientName in highCalorieIngredientNamesQuery) { Console.WriteLine(ingredientName); }
        }

        /// <summary>
        /// Uses syntax query with into clause
        /// </summary>
        private void LINQ_Into()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Kitchen kitchen = new Kitchen();

            IEnumerable<Ingredient> highCalDairyQuery =
                // range variable and collection
                from /*Ingredient*/ i
                in kitchen.Ingredients
                // Select Clause
                select new
                // anonymous type
                {
                    OriginalIngredient = i,
                    IsDairy = i.IsDairy,
                    IsHighCalorie = i.Calories >= 150
                }
                // new range variable, which is of the anonymous type created by the select above
                into temp
                    // condition
                    where temp.IsDairy && temp.IsHighCalorie
                    // cannot write "select i;" as into hides the previous range variable i 
                    select temp.OriginalIngredient;

            foreach (Ingredient ingredient in highCalDairyQuery) { Console.WriteLine(ingredient); }
        }

        /// <summary>
        /// Use query syntax to create an anonymous type from the input customer sequence joined with Order Descriptions on the customer ID and the order customer ID
        /// </summary>
        private void LINQ_Join1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            // var is an anonymous type that is defined below in the new
            var customerOrders =
                // From Clause: range variable and collection (sequence)
                from /*Customer*/ c
                in company.Customers
                // Join Clause
                join o in company.Orders on c.Id equals o.CustomerId
                // Select Clause
                // Anonymous Type
                select new
                {
                    Id = c.Id,
                    Name = c.Name,
                    Item = o.Description
                };

            foreach (var custOrd in customerOrders) { Console.WriteLine(string.Format("Customer ({0}): {1}, Item: {2}", custOrd.Id, custOrd.Name, custOrd.Item)); }
        }

        class Recipe {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Calories { get; set; }
        }

        class Review
        {
            public int Id { get; set; }
            public int RecipeId { get; set; }
            public string Text { get; set; }
        }

        class Cookbook
        {
            public Recipe[] Recipes { get; private set; }
            public Review[] Reviews { get; private set; }

            public Cookbook()
            {
                Recipes = new Recipe[]
                {
                    new Recipe { Id = 0, Name = "Crispy Duck", Calories = 500 },
                    new Recipe { Id = 1, Name = "Mashed Potato", Calories = 200 },
                    new Recipe { Id = 2, Name = "Sachertorte", Calories = 750 },
                    new Recipe { Id = 3, Name = "Panettone", Calories = 1000 }
                };
                Reviews = new Review[]
                {
                    new Review { Id = 0, RecipeId = 0, Text = "Tasty!" },
                    new Review { Id = 1, RecipeId = 0, Text = "Not nice :(" },
                    new Review { Id = 2, RecipeId = 0, Text = "Pretty good" },
                    new Review { Id = 3, RecipeId = 1, Text = "Too hard" },
                    new Review { Id = 4, RecipeId = 1, Text = "Loved it" },
                    new Review { Id = 5, RecipeId = 2, Text = "Love them goodies!" },
                    new Review { Id = 6, RecipeId = 3, Text = "Too sweet!" },
                    new Review { Id = 7, RecipeId = 3, Text = "Oh so good!" }
                };
            }
        }

        /// <summary>
        /// Uses syntax query with join clause
        /// </summary>
        private void LINQ_Join2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Cookbook cookbook = new Cookbook();

            var query =
                // range variable and collection
                from recipe in cookbook.Recipes
                // join
                join review in cookbook.Reviews on recipe.Id equals review.RecipeId
                // anonymous type
                select new
                {
                    RecipeName = recipe.Name,
                    ReviewText = review.Text
                };

            foreach (var recipeReview in query) { Console.WriteLine(string.Format("{0}-'{1}'", recipeReview.RecipeName, recipeReview.ReviewText)); }
        }

        /// <summary>
        /// Uses syntax query with outer join clause
        /// </summary>
        private void LINQ_Join_Into()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Cookbook cookbook = new Cookbook();

            var query =
                // range variable and Collection
                from Recipe recipe
                in cookbook.Recipes
                where recipe.Name.Length > 0 // bogus where clause
                // join
                join review in cookbook.Reviews on recipe.Id equals review.RecipeId
                into reviewGroup
                    from review in reviewGroup.DefaultIfEmpty(new Review())
                    select new
                    {
                        RecipeName = recipe.Name,
                        ReviewText = review.Text
                    };

            foreach (var recipeReview in query) { Console.WriteLine(string.Format("{0}-'{1}'", recipeReview.RecipeName, recipeReview.ReviewText)); }
        }

        /// <summary>
        /// Uses syntax query with outer group clause
        /// </summary>
        private void LINQ_Group()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Kitchen kitchen = new Kitchen();

            // Create a collection of groups of Ingredient's where Calories is the Key (same for each group)
            IEnumerable<IGrouping<int, Ingredient>> query =
                from /*Ingredient*/ i 
                in kitchen.Ingredients
                group i by i.Calories;

            // public interface IGrouping<out TKey, out TElement> : IEnumerable<TElement>, IEnumerable
            foreach (IGrouping<int, Ingredient> ig in query)
            {
                Console.WriteLine(string.Format("Ingredients with {0} calories", ig.Key));
                foreach (Ingredient i in ig)
                {
                    Console.WriteLine(string.Format("\t{0}", i.Name));
                }
            }

        }

        /// <summary>
        /// Uses syntax query with outer group clause
        /// </summary>
        private void LINQ_Group_Ordered()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Kitchen kitchen = new Kitchen();

            // Same as above but ordered by calories
            IEnumerable<IGrouping<int, Ingredient>> query =
                from i in kitchen.Ingredients
                group i by i.Calories
                into calorieGroup
                orderby calorieGroup.Key
                select calorieGroup;

            foreach (IGrouping<int, Ingredient> ig in query)
            {
                Console.WriteLine(string.Format("Ingredients with {0} calories", ig.Key));
                foreach (Ingredient i in ig)
                {
                    Console.WriteLine(string.Format("\t{0}", i.Name));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LINQ_MultipleFromLetWhereClauses()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Cookbook cookbook = new Cookbook();

            foreach (var r in cookbook.Recipes) { Console.WriteLine(string.Format("Id={0}, RecipeName={1}, Calories={2}", r.Id, r.Name, r.Calories)); }
            foreach (var r in cookbook.Reviews) { Console.WriteLine(string.Format("Id={0}, Id={1}, Text={2}", r.Id, r.RecipeId, r.Text)); }

            // Selects from both sequences, and creates tuples of recipe-review.  This does not do a join
            var query =
                from r in cookbook.Recipes

                let name = r.Name
                where name.Length > 4
                orderby name

                let calories = r.Calories
                where calories > 500
                orderby calories descending

                from v in cookbook.Reviews 
                let text = v.Text
                where text.Length > 6
                orderby text

                select new { RecipeName = name, Calories = calories, Text = text };

            foreach (var rr in query) { Console.WriteLine(string.Format("RecipeName={0}, Calories={1}, Text={2}", rr.RecipeName, rr.Calories, rr.Text)); }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LINQ_Scratch()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            Console.WriteLine("Basic");
            IEnumerable<Customer> customers =
                from c in company.Customers
                select c;

            foreach (Customer c in customers) { Console.WriteLine(c.Name); }

            Console.WriteLine("Filtered");
            IEnumerable<Customer> customers2 =
                from c in company.Customers
                let name = c.Name
                where name.Length > 4
                select c;

            foreach (Customer c in customers) { Console.WriteLine(c.Name); }

            Console.WriteLine("Grouped");
            var customerGroups =
                from c in company.Customers
                group c by c.Name[0];

            foreach (IGrouping<char, Customer> cg in customerGroups)
            {
                Console.WriteLine(cg.Key);
                foreach (Customer c in cg) { Console.WriteLine(c.Name); }
            }
        }

        /// <summary>
        /// Uses method-based (fluent) syntax to union 2 customer sequences
        /// </summary>
        private void LINQ12()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Company company = new Company();

            List<Customer> additionalCustomers = new List<Customer> { new Customer { Id = 1, Name = "Gary" } };

            // Method-based query
            List<Customer> customers = company.Customers.Union(additionalCustomers).ToList();

            customers.ForEach(c => Console.WriteLine(c.Name));
        }

        /// <summary>
        /// Uses both query and method-based syntax to perform the same query
        /// </summary>
        private void LINQ13()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            string[] names = { "Burke", "Connor", "Frank", "Everett", "Albert", "George", "Harris", "David" };

            // In query (expression) syntax
            IEnumerable<string> query =
                // range variable and collection
                from s in names
                // condition
                where s.Length == 5
                // ordering
                orderby s
                // what to query
                select s.ToUpper();

            foreach (string s in query) { Console.WriteLine(s); }

            // method-based query
            IEnumerable<string> query2 =
                names
                // Filter
                .Where(s => s.Length == 5)
                // Sorter
                .OrderBy(s => s)
                // Projector
                .Select(s => s.ToUpper());

            foreach (string s in query2) { Console.WriteLine(s); }
        }

        /// <summary>
        /// Creates a delegate instance using a lambda expression, and passes the instance to a query using method-based syntax
        /// </summary>
        private void LINQ14()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            string[] names = { "Abe", "Burke", "Connor", "Frank", "Everett", "Albert", "George", "Harris", "David", "Gus", "Burt" };

            // public delegate TResult Func<in T, out TResult>(T arg);
            Func<string, bool>
                // delegate instance
                condition =
                // parameters
                n
                // lambda operator
                =>
                // method
                n.Length > 4;

            IEnumerable<string> filteredNames = names.Where(condition);

            foreach (string s in filteredNames) { Console.WriteLine(s); }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LINQ15()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Cookbook cookbook = new Cookbook();

            var reviewsByRecipe = cookbook.Recipes.SelectMany(c => cookbook.Reviews.Where(v => c.Id == v.RecipeId).Select(r => new { RecipeName = c.Name, ReviewText = r.Text }));

            foreach (var review in reviewsByRecipe) { Console.WriteLine(string.Format("{0}-'{1}'", review.RecipeName, review.ReviewText)); }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LINQ16()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            List<string> array = new List<string>
            {
                "dot",
                "net",
                "perls"
            };

            // Convert each string in the string array to a character array.
            // ... Then combine those character arrays into one.
            var result = array.SelectMany(element => element.ToCharArray());

            // Display letters.
            foreach (char letter in result) { Console.WriteLine(letter); }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LINQ17()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            int[] numbers = { 2, 3, 8, 7, 9 };
            string[] strings = { "a", "ab", "abc", "abcd", "abcde", "abcdef", "abcdefg", "x", "xy", "xyz" };

            // For each 'n' in 'numbers', select the strings in 'strings' that are of length 'n'
            var query1 = numbers.SelectMany(n => strings.Where(s => s.Length == n));
            foreach (string s in query1) { Console.WriteLine(s); }

            // For each 'n' in 'numbers', select the strings in 'strings' that are of length 'n', and create an anonymous type with the length and string
            var query2 = numbers.SelectMany(n => strings.Where(s => s.Length == n).Select(s => new { Length = n, String = s }));
            foreach (var a in query2) { Console.WriteLine(string.Format("{0}-'{1}'", a.Length, a.String)); }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LINQ18()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Cookbook cookbook = new Cookbook();

            var reviewByRecipe = cookbook.Reviews.Join(
                cookbook.Recipes, 
                review => review.RecipeId, 
                recipe => recipe.Id, 
                (review, recipe) => new { RecipeName = recipe.Name, ReviewText = review.Text });

            foreach (var recipeReview in reviewByRecipe) { Console.WriteLine(string.Format("{0}-'{1}'", recipeReview.RecipeName, recipeReview.ReviewText)); }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LINQ19()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Kitchen kitchen = new Kitchen();

            // Create a collection of groups of Ingredient's where Calories is the Key (same for each group)
            IEnumerable<IGrouping<int, Ingredient>> query = kitchen.Ingredients.GroupBy(i => i.Calories);

            // public interface IGrouping<out TKey, out TElement> : IEnumerable<TElement>, IEnumerable
            foreach (IGrouping<int, Ingredient> ingredientGroup in query)
            {
                Console.WriteLine(string.Format("Ingredients with {0} calories", ingredientGroup.Key));
                foreach (Ingredient ingredient in ingredientGroup)
                {
                    Console.WriteLine(string.Format(" - {0}", ingredient.Name));
                }
            }
        }
    }

    #endregion

    #region Asynchronous Coding

    public class AsynchronousCodingTestSuite : TestSuite, ITestSuite
    {
        public void Run()
        {
            WriteTestSuiteName();

            Async1();
            Async2();
            Async3();
            Async4();
            Async5();
            Async6();
            Async7();
            Async8();

            WriteTestSuiteName();
        }

        private void Async1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            // Runs synchronously
            CreateFileAsync("test.txt").Wait();
        }

        // async modifier says that the method can contain async code
        private async Task CreateFileAsync(string filename)
        {
            using (System.IO.StreamWriter writer = System.IO.File.CreateText(filename))
            {
                // Use the await keyword on a Task to start the async operation
                await writer.WriteAsync("This is a test.");
            }
        }

        private void Async2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Console.WriteLine("Before call to ReturnStringAsync");
            Task task = ReturnStringAsync("Hello");
            Console.WriteLine("After call to ReturnStringAsync");

            Console.WriteLine("Waiting on the task");
            task.Wait();
            Console.WriteLine("After call to Wait on task");
        }

        // async modifier says that the method can contain async code
        private async Task<string> ReturnStringAsync(string text)
        {
            Console.WriteLine("In ReturnStringAsync, before 1st call to Task.Delay");
            await Task.Delay(3000);
            Console.WriteLine("In ReturnStringAsync, after 1st call to Task.Delay");

            Console.WriteLine("In ReturnStringAsync, before 2nd call to Task.Delay");
            await Task.Delay(3000);
            Console.WriteLine("In ReturnStringAsync, after 2nd call to Task.Delay");

            return text;
        }

        private void Async3()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Console.WriteLine("Before call to Async3Task");
            Task task = Async3Task();
            Console.WriteLine("After call to Async3Task.  Now waiting on the task.");
            task.Wait();
            Console.WriteLine("After call to Wait on task");
        }

        private async Task Async3Task()
        {
            // Async3Task() will wait here, but will return control to the caller
            int respLen = await AccessTheWebAsync("http://msdn.microsoft.com");

            Console.WriteLine(string.Format("Response Length: {0}", respLen));
        }

        // Three things to note in the signature:
        //  - The method has an async modifier.
        //  - The return type is Task or Task<T>
        //    Here, it is Task<int> because the return statement returns an integer.
        //  - The method has at least one await statement
        //  - The method name ends in "Async."
        private async Task<int> AccessTheWebAsync(string url)
        {
            // You need to add a reference to System.Net.Http to declare client.
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

            // GetStringAsync returns a Task<string>. That means that when you await the
            // task you'll get a string (urlContents).
            Task<string> getStringTask = client.GetStringAsync(url);

            // You can do work here that doesn't rely on the string from GetStringAsync.
            await Task.Delay(1000);

            // The await operator suspends AccessTheWebAsync.
            //  - AccessTheWebAsync can't continue until getStringTask is complete.
            //  - Meanwhile, control returns to the caller of AccessTheWebAsync.
            //  - Control resumes here when getStringTask is complete. 
            //  - The await operator then retrieves the string result from getStringTask.
            string urlContents = string.Empty;
            try
            {
                urlContents = await getStringTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("AccessTheWebAsync Exception= {0}", ex.Message));
            }

            // The return statement specifies an integer result.
            // Any methods that are awaiting AccessTheWebAsync retrieve the length value.
            return urlContents.Length;
        }

        private void Async4()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            BackgroundWorker bw = new BackgroundWorker();

            bool done = false;

            // DoWork is an event that conforms to the DoWorkEventHandler delegate
            // public delegate void DoWorkEventHandler(object sender, DoWorkEventArgs e);
            // public event DoWorkEventHandler DoWork;
            // Subscribe to the DoWork event
            bw.DoWork += (object sender, DoWorkEventArgs e) => { BWDoWork(); };

            // RunWorkerCompleted is an event that conforms to the RunWorkerCompletedEventHandler delegate
            // public delegate void RunWorkerCompletedEventHandler(object sender, RunWorkerCompletedEventArgs e);
            // public event RunWorkerCompletedEventHandler RunWorkerCompleted;
            // Subscribe to the RunWorkerCompleted event
            bw.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) => { BWRunWorkerCompleted(out done, e.Error); };

            bw.RunWorkerAsync();

            while (!done) { }

            Console.WriteLine("Done");
        }

        private static int RandomNbr(int minValue, int maxValue)
        {
            return (new Random(DateTime.Now.Millisecond)).Next(minValue, maxValue);
        }

        private void BWDoWork()
        {
            Console.WriteLine("In BWDoWork");

            // Simulate an exception 1/2 of the executions
            if (RandomNbr(0, 2) == 1)
            {
                throw new Exception("Exception thrown in BWDoWork");
            }
            else
            {
                Thread.Sleep(1000);
            }

            Console.WriteLine("Out of BWDoWork");
        }

        private void BWRunWorkerCompleted(out bool done, Exception e)
        {
            Console.WriteLine("In BWRunWorkerCompleted");
            if (e != null)
            {
                Console.WriteLine(string.Format("Exception: {0}", e.Message));
            }
            done = true;
        }

        private void Async5()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            double result = 0.0;
            Thread thread = new Thread(() => result = SleepFunc(5000));
            thread.Start();
            Console.WriteLine("Thread Started");

            double result2 = IntensiveFunc();
            Console.WriteLine("IntensiveFunc Done");

            thread.Join();
            Console.WriteLine("Thread Joined");

            result += result2;

            Console.WriteLine(string.Format("result = {0}", result));
        }

        private double SleepFunc(int ms)
        {
            Console.WriteLine("SleepFunc Before Sleep");
            Thread.Sleep(ms);
            Console.WriteLine("SleepFunc After Sleep");
            return 1.23;
        }

        private double IntensiveFunc()
        {
            double result = 100000000d;            
            for(int i = 1; i < int.MaxValue; i++)
            {
                result /= i;
            }
            return result + 10d;
        }

        private readonly int kPrintMessageDelay = 500;

        private void Async6()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            // public delegate void Action();

            // public Task StartNew(Action action);
            Action action1 = () => PrintMessage();
            Task task1 = Task.Factory.StartNew(action1);
            WaitingMessage("task1");
            task1.Wait();

            Action action2 = () => { PrintMessage(); };
            Task task2 = new Task(action2);
            task2.Start();
            WaitingMessage("task2");
            task2.Wait();

            Action action3 = delegate { PrintMessage(); };
            Task task3 = new Task(action3);
            task3.Start();
            WaitingMessage("task3");
            task3.Wait();

            Action action4 = new Action(PrintMessage);
            Task task4 = new Task(action4);
            task4.Start();
            WaitingMessage("task4");
            task4.Wait();

            Task task5 = DoPrintMessageWork();
            WaitingMessage("task5");
            task5.Wait();
        }

        private void WaitingMessage(string taskName)
        {
            Thread.Sleep((RandomNbr(0, 2) == 1) ? (kPrintMessageDelay * 2) : (kPrintMessageDelay / 2));
            Console.WriteLine(string.Format("Waiting for {0}", taskName));
        }

        public async Task DoPrintMessageWork()
        {
            await Task.Run(() => PrintMessage());
        }

        private void PrintMessage()
        {
            Thread.Sleep(kPrintMessageDelay);
            Console.WriteLine("Hello Task library!");
        }

        private void Async7()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Console.WriteLine("Before For Loop");
            Parallel.For(0, 10, i => Thread.Sleep(1000));
            Console.WriteLine("After For Loop");

            Action[] actions = new Action[5];
            Parallel.For(0, 5, i => actions[i] = () => Thread.Sleep(1000));

            Console.WriteLine("Before Invoke");
            Parallel.Invoke(actions);
            Console.WriteLine("After Invoke");
        }

        private void Async8()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

        }
    }

    #endregion

    #region Operator Overloading

    public class OperatorOverloadingTestSuite : TestSuite, ITestSuite
    {
        public void Run()
        {
            WriteTestSuiteName();

            OperatorOverloading1();
            OperatorOverloading2();

            WriteTestSuiteName();
        }

        private void OperatorOverloading1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Complex num1 = new Complex(2, 3);
            Complex num2 = new Complex(3, 4);

            // Use the overloaded operator
            Complex sum = num1 + num2;

            Console.WriteLine(string.Format("{0} + {1} = {2}", num1, num2, sum));
        }

        private void OperatorOverloading2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Note B = new Note(2);
            Note Csharp = B + 2;

            Console.WriteLine(string.Format("{0} + 2 = {1}", B, Csharp));

            Note n = (Note)554.37;  // explicit
            double x = n;           // implicit

            Console.WriteLine(string.Format("{0} = {1}", n, x));
        }
    }

    public struct Complex
    {
        public int real;
        public int imaginary;

        public Complex(int real, int imaginary)
        {
            this.real = real;
            this.imaginary = imaginary;
        }

        // Declare which operator function to overload (+), the types that can be added (two Complex objects), and the return type (Complex)
        // Must be static and public
        public static Complex operator +(Complex c1, Complex c2)
        {
            return new Complex(c1.real + c2.real, c1.imaginary + c2.imaginary);
        }

        // Override the ToString method to display an complex number in the suitable format
        public override string ToString()
        {
            return (string.Format(string.Format("({0} + {1}i)", real, imaginary)));
        }
    }

    public struct Note
    {
        private static readonly double Log2 = Math.Log(2.0);
        public int value;

        public Note(int semitonesFromA)
        {
            value = semitonesFromA;
        }

        public static Note operator +(Note x, int semitones)
        {
            return new Note(x.value + semitones);
        }

        public static implicit operator double(Note x)
        {
            return 444.0 * Math.Pow(2.0, (x.value / 12.0));
        }

        public static explicit operator Note(double x)
        {
            return new Note((int)(0.5 + 12.0 * (Math.Log(x / 440.0) / Log2)));
        }

        // Override the ToString method to display
        public override string ToString()
        {
            switch (value % 12)
            {
                case 0:
                    return "A";
                case 1:
                    return "A#";
                case 2:
                    return "B";
                case 3:
                    return "C";
                case 4:
                    return "C#";
                case 5:
                    return "D";
                case 6:
                    return "D#";
                case 7:
                    return "E";
                case 8:
                    return "F";
                case 9:
                    return "F#";
                case 10:
                    return "G";
                case 11:
                    return "G#";
                default:
                    return "Unknown";
            }
        }
    }

    #endregion

    #region ADO.NET

    public class ADODotNetTestSuite : TestSuite, ITestSuite
    {
        private static readonly string kNorthwindConnectionString = "Server=.\\SQLEXPRESS;Database=Northwind;Trusted_Connection=True;";
        private static readonly string kUniversityConnectionString = "Server=.\\SQLEXPRESS;Database=ContosoUniversity;Trusted_Connection=True;";

        public void Run()
        {
            WriteTestSuiteName();

            ADODotNet1();
            ADODotNet2();
            ADODotNet3();
            ADODotNet4();

            WriteTestSuiteName();
        }

        private void ADODotNet1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            try
            {
                using (SqlConnection connection = new SqlConnection(kNorthwindConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Categories", connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("Categories:");
                            while (reader.Read())
                            {
                                int CategoryID = reader.GetInt32(0);
                                string CategoryName = reader.GetString(1);
                                string Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                                SqlBytes Picture = reader.IsDBNull(3) ? null : reader.GetSqlBytes(3);

                                Console.WriteLine(string.Format("CategoryID: {0}, CategoryName: {1}, Description: {2}, Bytes in Picture: {3}", CategoryID, CategoryName, Description, Picture.Length));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ADODotNet2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            try
            {
                using (SqlConnection connection = new SqlConnection(kNorthwindConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Categories", connection))
                    {
                        DataTable categoriesTbl = new DataTable();
                        categoriesTbl.Load(cmd.ExecuteReader());

                        // A DataSet is a container for 1 or more DataTable's
                        DataSet categoriesDS = new DataSet();
                        categoriesDS.Tables.Add(categoriesTbl);

                        IEnumerable<DataRow> query =
                            from category
                            in categoriesTbl.AsEnumerable()
                            select category;

                        Console.WriteLine("Categories:");
                        foreach (DataRow c in query)
                        {
                            int CategoryID = c.Field<int>("CategoryID");
                            string CategoryName = c.Field<string>("CategoryName");
                            string Description = c.Field<string>("Description");
                            byte[] Picture = c.Field<byte[]>("Picture");

                            Console.WriteLine(string.Format("CategoryID: {0}, CategoryName: {1}, Description: {2}, Bytes in Picture: {3}", CategoryID, CategoryName, Description, Picture.Length));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ADODotNet3()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            try
            {
                using (SqlConnection connection = new SqlConnection(kNorthwindConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Categories", connection))
                    {
                        DataTable categoriesTbl = new DataTable();
                        categoriesTbl.Load(cmd.ExecuteReader());

                        DataSet categoriesDS = new DataSet();
                        categoriesDS.Tables.Add(categoriesTbl);

                        var query =
                            categoriesTbl.AsEnumerable().
                            Select(c => new
                            {
                                CategoryID = c.Field<int>("CategoryID"),
                                CategoryName = c.Field<string>("CategoryName"),
                                Description = c.Field<string>("Description"),
                                Picture = c.Field<byte[]>("Picture")
                            });

                        Console.WriteLine("Categories:");
                        foreach (var c in query)
                        {
                            Console.WriteLine(string.Format("CategoryID: {0}, CategoryName: {1}, Description: {2}, Bytes in Picture: {3}", c.CategoryID, c.CategoryName, c.Description, c.Picture.Length));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ADODotNet4()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            try
            {
                using (SqlConnection connection = new SqlConnection(kUniversityConnectionString))
                {
                    connection.Open();

                    string insertCmdText =
                        "INSERT INTO Student (FirstName, MiddleName, LastName, Gender, EnrollmentDate) " + 
                        "VALUES ('Nicholas', 'Antonio', 'Osella', 'Male', '" + DateTime.Now.ToShortDateString() + "')";

                    using (SqlCommand cmd = new SqlCommand(insertCmdText, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    LoadAndWriteStudents();

                    string deleteCmdText =
                        "DELETE FROM Student WHERE " + 
                        "FirstName='Nicholas' AND " + 
                        "MiddleName='Antonio' AND " +
                        "LastName='Osella' AND " +
                        "Gender='Male'";

                    using (SqlCommand cmd = new SqlCommand(deleteCmdText, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    LoadAndWriteStudents();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LoadAndWriteStudents()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(kUniversityConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Student", connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("Students:");
                            while (reader.Read())
                            {
                                int StudentID = reader.GetInt32(0);
                                string LastName = reader.GetString(1);
                                string MiddleName = reader.GetString(2);
                                string FirstName = reader.GetString(3);
                                string Gender = reader.GetString(4);
                                DateTime EnrollmentDate = reader.GetDateTime(5);

                                Console.WriteLine(string.Format("StudentID: {0}, FirstName: {1}, MiddleName: {2}, LastName: {3}, Gender: {4}, EnrollmentDate: {5}", StudentID, FirstName, MiddleName, LastName, Gender, EnrollmentDate));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    #endregion

    #region Entity Framework

    public class Student
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public virtual List<Enrollment> Enrollments { get; set; }
    }

    public class Course
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }

        public virtual List<Enrollment> Enrollments { get; set; }
    }

    public enum Grade
    {
        A, B, C, D, F
    }

    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        public Grade? Grade { get; set; }

        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }
    }

    public class StudentEnrollments_Result
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
    }

    public class UniversityDbContext : DbContext
    {
        public UniversityDbContext(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public Student FindOrAddStudent(string firstName, string middleName, string lastName, string gender)
        {
            Student student =
                (from s
                 in Students
                 where s.FirstName.Equals(firstName) && s.MiddleName.Equals(middleName) && s.LastName.Equals(lastName) && s.Gender.Equals(gender)
                 select s).SingleOrDefault();
            if (student == null)
            {
                student = new Student
                {
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    Gender = gender,
                    EnrollmentDate = DateTime.Now
                };
                Students.Add(student);
            }
            return student;
        }

        public Course FindOrAddCourse(string title, int credits)
        {
            Course course =
                (from c
                 in Courses
                 where c.Title.Equals(title)
                 select c).SingleOrDefault();
            if (course == null)
            {
                course = new Course
                {
                    Title = title,
                    Credits = credits,
                    Enrollments = new List<Enrollment>()
                };
                Courses.Add(course);
            }
            return course;
        }

        public Enrollment FindOrAddEnrollment(Student student, Course course)
        {
            Enrollment enrollment =
                (from e
                 in Enrollments
                 where e.StudentID.Equals(student.ID) && e.CourseID.Equals(course.ID)
                 select e).SingleOrDefault();
            if (enrollment == null)
            {
                enrollment = new Enrollment
                {
                    StudentID = student.ID,
                    CourseID = course.ID
                };
                Enrollments.Add(enrollment);
            }
            return enrollment;
        }

        public virtual List<StudentEnrollments_Result> StudentEnrollments(int? studentID)
        {
            SqlParameter studentIdParameter = studentID.HasValue ? 
                new SqlParameter("StudentID", studentID) : 
                new SqlParameter("StudentID", typeof(int));

            return Database.SqlQuery<StudentEnrollments_Result>("StudentEnrollments @StudentID", studentIdParameter).ToList();
        }
    }

    public class EntityFrameworkTestSuite : TestSuite, ITestSuite
    {
        private static readonly string kUniversityConnectionString = "Server=.\\SQLEXPRESS;Database=ContosoUniversity;Trusted_Connection=True;";

        public void Run()
        {
            WriteTestSuiteName();

            EntityFramework1();
            EntityFramework2();
            EntityFramework3();
            EntityFramework4();
            EntityFramework5();

            WriteTestSuiteName();
        }

        private void EntityFramework1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            using (UniversityDbContext ctx = new UniversityDbContext(kUniversityConnectionString))
            {
                Student stephen = ctx.FindOrAddStudent("Stephen", "Albert", "Osella", "Male");
                Student vicky = ctx.FindOrAddStudent("Vicky", "Chen", "Foisy", "Female");
                ctx.SaveChanges();
                WriteStudents(ctx.Students.ToList());

                Student nicholas = ctx.FindOrAddStudent("Nicholas", "", "Osella", "Male");
                ctx.SaveChanges();
                WriteStudents(ctx.Students.ToList());

                nicholas = ctx.Students.FirstOrDefault(s => s.FirstName.Equals("Nicholas") && s.LastName.Equals("Osella"));
                nicholas.MiddleName = "Antonio";
                ctx.SaveChanges();
                WriteStudents(ctx.Students.ToList());

                ctx.Students.Remove(nicholas);
                ctx.SaveChanges();
                WriteStudents(ctx.Students.ToList());
            }
        }

        private void EntityFramework2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            using (UniversityDbContext ctx = new UniversityDbContext(kUniversityConnectionString))
            {
                var Students = from s in ctx.Students select s;
                WriteStudents(Students);
            }
        }

        private void EntityFramework3()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            using (UniversityDbContext ctx = new UniversityDbContext(kUniversityConnectionString))
            {
                Course calculus = ctx.FindOrAddCourse("Calculus", 3);
                Course trigonometry = ctx.FindOrAddCourse("Trigonometry", 3);
                Course englishI = ctx.FindOrAddCourse("English I", 3);
                Course englishII = ctx.FindOrAddCourse("English II", 3);
                ctx.SaveChanges();
                WriteCourses(ctx.Courses.ToList());
            }
        }

        private void EntityFramework4()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            using (UniversityDbContext ctx = new UniversityDbContext(kUniversityConnectionString))
            {
                Student stephen = ctx.Students.FirstOrDefault(s => s.FirstName.Equals("Stephen") && s.MiddleName.Equals("Albert") && s.LastName.Equals("Osella") && s.Gender.Equals("Male"));
                if (stephen != null)
                {
                    Course calculus = ctx.Courses.FirstOrDefault(c => c.Title.Equals("Calculus"));
                    if (calculus != null)
                    {
                        ctx.FindOrAddEnrollment(stephen, calculus);
                    }
                    Course englishI = ctx.Courses.FirstOrDefault(c => c.Title.Equals("English I"));
                    if (englishI != null)
                    {
                        ctx.FindOrAddEnrollment(stephen, englishI);
                    }
                }
                ctx.SaveChanges();
                WriteEnrollments(ctx.Enrollments.ToList());
            }
        }

        private void EntityFramework5()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            using (UniversityDbContext ctx = new UniversityDbContext(kUniversityConnectionString))
            {
                List<StudentEnrollments_Result> results = ctx.StudentEnrollments(1);
                foreach (StudentEnrollments_Result result in results)
                {
                    Console.WriteLine(string.Format("First Name: {0}, Middle Name: {1}, Last Name: {2}, Title={3}", result.FirstName, result.MiddleName, result.LastName, result.Title));
                }
            }
        }

        public static void WriteStudents(IEnumerable<Student> students)
        {
            Console.WriteLine("Students");
            foreach (Student student in students)
            {
                WriteStudent(student);
            }
        }

        private static void WriteStudent(Student student)
        {
            Console.WriteLine(student.FirstName + " " + student.MiddleName + " " + student.LastName + ", " + student.Gender);
        }

        private static void WriteCourses(IEnumerable<Course> courses)
        {
            Console.WriteLine("Courses");
            foreach (Course course in courses)
            {
                WriteCourse(course);
            }
        }

        private static void WriteCourse(Course course)
        {
            Console.WriteLine(string.Format("{0} {1}", course.Title, course.Credits));
        }

        private static void WriteEnrollments(IEnumerable<Enrollment> enrollments)
        {
            Console.WriteLine("Enrollments");
            foreach (Enrollment enrollment in enrollments)
            {
                WriteEnrollment(enrollment);
            }
        }

        private static void WriteEnrollment(Enrollment enrollment)
        {
            Console.WriteLine(string.Format("{0} {1} - {2}", enrollment.Student.FirstName, enrollment.Student.LastName, enrollment.Course.Title));
        }
    }

    #endregion

    #region JSONSerializationTestSuite

    public class JSONSerializationTestSuite : TestSuite, ITestSuite
    {
        public void Run()
        {
            WriteTestSuiteName();

            JSONSerialization1();

            WriteTestSuiteName();
        }

        private void JSONSerialization1()
        {
            Customer customer1 = new Customer();
            string customer1Json = JsonConvert.SerializeObject(customer1, Formatting.Indented);
            Console.WriteLine(customer1Json);
        }
    }

    #endregion

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
                            TestSuiteFactory.Create<AsynchronousCodingTestSuite>(), 
                            TestSuiteFactory.Create<OperatorOverloadingTestSuite>(), 
                            TestSuiteFactory.Create<ADODotNetTestSuite>(), 
                            TestSuiteFactory.Create<EntityFrameworkTestSuite>(), 
                            TestSuiteFactory.Create<JSONSerializationTestSuite>()
                        };

                    for (int i = 0; i < testSuites.Length; i++)
                    {
                        Console.WriteLine(string.Format("{0}: {1}", i, testSuites[i].Name));
                    }
                    Console.Write(string.Format("Enter Suites to Execute (or {0} to exit): ", testSuites.Length));
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
