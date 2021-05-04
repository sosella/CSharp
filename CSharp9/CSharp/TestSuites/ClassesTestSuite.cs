using System;
using System.Reflection;

namespace CSharp.TestSuites
{
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
            Console.WriteLine($"In Asset Constructor: {Name}");
        }

        // static
        // public, internal, private, protected
        // new, virtual, abstract, override, sealed
        // partial
        // unsafe, extern
        public virtual void Print()
        {
            Console.WriteLine($"Asset Name: {Name}, Net Value: {NetValue}");
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
            Console.WriteLine($"In Stock Constructor: {Name}");
        }
        public Stock(string name, long sharesOwned, decimal price, decimal purchasePrice) : this(name)
        {
            SharesOwned = sharesOwned;
            Price = price;
            PurchasePrice = purchasePrice;
            Console.WriteLine($"In Stock Constructor: {SharesOwned}, {Price}, {PurchasePrice}");
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Stock Shares Owned: {SharesOwned}, Price: {Price}, Purchase Price: {PurchasePrice}");
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
            Console.WriteLine($"In House Constructor: {Valuation}, {MortgageBalance}");
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"House Valuation: {Valuation}, Mortgage Balance: {MortgageBalance}");
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
            Employee danAsEmployee = (Employee)danAsPerson;   // Narrowing. Need cast (explicit conversion), but works because dan is an Employee
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
                Console.WriteLine($"danAsManager2 Exception: {ex.Message}");
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
                Console.WriteLine($"joeAsEmployee Exception: {ex.Message}");
            }
        }
    }
}
