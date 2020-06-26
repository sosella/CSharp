using System;
using System.Reflection;

namespace CSharp.TestSuites
{
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
            Console.WriteLine($"Caller is a CalculatorButton: {sender is CalculatorButton}, and is named {args.Name}");
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
}
