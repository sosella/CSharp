using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSharp.TestSuites
{
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
                Console.WriteLine($"Adding {num} & {i} yields {add(num, i)}");
            }
        }

        // 1) Define the delegate Type
        private delegate void FuncDelegateType(int n);

        private void Func1(int x)
        {
            Console.WriteLine($"In Func1: {x}");
        }

        private void Func2(int y)
        {
            Console.WriteLine($"In Func2: {y}");
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
}
