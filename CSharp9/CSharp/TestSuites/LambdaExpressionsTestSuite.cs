using System;
using System.Reflection;

namespace CSharp.TestSuites
{
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
            Console.WriteLine($"Is Valid: {isValid}");
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
                    Console.WriteLine($"Caller is a CalculatorButton: {sender is CalculatorButton} and is named {args.Name}");
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
}
