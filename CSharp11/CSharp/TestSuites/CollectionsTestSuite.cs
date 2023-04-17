using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CSharp.TestSuites
{
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
                    Console.WriteLine($"In Foo stopAt = {stopAt}");
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
}
