using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp.TestSuites
{
    public class MultiThreadingTestSuite : TestSuite, ITestSuite
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

            Console.WriteLine($"Response Length: {respLen}");
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
            HttpClient client = new HttpClient();

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
                Console.WriteLine($"AccessTheWebAsync Exception= {ex.Message}");
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
                Console.WriteLine($"Exception: {e.Message}");
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

            Console.WriteLine($"result = {result}");
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
            for (int i = 1; i < int.MaxValue; i++)
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
            Console.WriteLine($"Waiting for {taskName}");
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
}
