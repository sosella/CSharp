using System.Reflection;

namespace CSharp.TestSuites
{
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
}
