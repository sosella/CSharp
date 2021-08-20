using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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

            /*AsyncTest1();
            AsyncTest2();
            AsyncTest3();
            AsyncTest4();
            AsyncTest5();
            AsyncTest6();
            AsyncTest7();
            AsyncTest8();*/
            AsyncTest9();

            WriteTestSuiteName();
        }

        private void AsyncTest1()
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

        private void AsyncTest2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Console.WriteLine("Before call to ReturnStringAsync");
            var task = ReturnStringAsync("Hello");
            Console.WriteLine("After call to ReturnStringAsync");

            Console.WriteLine("Waiting on the task");
            task.Wait();
            Console.WriteLine("After call to Wait on task");
        }

        private void AsyncTest3()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Console.WriteLine("Before call to Async3Task without calling Result");
            Async3Task(false).Wait();
            Console.WriteLine("After call to Async3Task without calling Result");

            Console.WriteLine("Before call to Async3Task with calling Result");
            Async3Task(true).Wait();
            Console.WriteLine("After call to Async3Task with calling Result");
        }

        private async Task Async3Task(bool callResult)
        {
            Console.WriteLine("Before call to ReturnStringAsync");
            var task = ReturnStringAsync("Hello", 3000);
            Console.WriteLine("After call to ReturnStringAsync");

            if (callResult)
            {
                Console.WriteLine("Before call to task.Result");
                var str = task.Result;
                Console.WriteLine($"After call to task.Result: {str}");
            }

            Console.WriteLine("Before first await");
            string str2 = (await task);
            Console.WriteLine($"After first await");

            Console.WriteLine($"str2: {str2}");

            int len = (await task).Length;
            Console.WriteLine($"After second await: {len}");
        }

        // async modifier says that the method can contain async code
        private async Task<string> ReturnStringAsync(string text, int millisecondsDelay = 1000)
        {
            Console.WriteLine("In ReturnStringAsync, before 1st call to Task.Delay");
            await Task.Delay(millisecondsDelay);
            Console.WriteLine("In ReturnStringAsync, after 1st call to Task.Delay");

            Console.WriteLine("In ReturnStringAsync, before 2nd call to Task.Delay");
            await Task.Delay(millisecondsDelay);
            Console.WriteLine("In ReturnStringAsync, after 2nd call to Task.Delay");

            return text;
        }

        private void AsyncTest4()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Console.WriteLine("Before call to Async4Task");
            Task task = Async4Task();
            Console.WriteLine("After call to Async4Task.  Now waiting on the task.");
            task.Wait();
            Console.WriteLine("After call to Wait on task");
        }

        private async Task Async4Task()
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

        private void AsyncTest5()
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

        private void AsyncTest6()
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

        private void AsyncTest7()
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

        private void AsyncTest8()
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

        private void AsyncTest9()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            for (int i = 0; i <= 5; i++)
            {
                AsyncTest9Main(i).Wait();
            }
        }

        private class TimedResult<T>
        {
            public TimeSpan elapsedTime;
            public T result;
        }

        private async Task AsyncTest9Main(int testCase)
        {
            Console.WriteLine($"Test Case: {testCase}");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            TimedResult<int> complexSum = null;
            TimedResult<string> complexWord = null;

            Console.WriteLine($"Time elapsed 1... {stopwatch.Elapsed}");

            // Synchronous at the awaits
            if (testCase == 0)
            {
                complexSum = await SlowAndComplexSumAsync();

                Console.WriteLine($"Time elapsed 2... {stopwatch.Elapsed}");

                complexWord = await SlowAndComplexWordAsync();

                Console.WriteLine("Time elapsed 3..." + stopwatch.Elapsed);
            }

            // Synchronous at the awaits
            else if (testCase == 1)
            {
                complexSum = new TimedResult<int>();
                await SlowAndComplexSumAsync();

                Console.WriteLine($"Time elapsed 2... {stopwatch.Elapsed}");

                complexWord = new TimedResult<string>();
                await SlowAndComplexWordAsync();

                Console.WriteLine("Time elapsed 3..." + stopwatch.Elapsed);
            }

            // Synchronous at the awaits, tasks start and execute in parallel
            else if (testCase == 2)
            {
                var complexSumTask = SlowAndComplexSumAsync();

                Console.WriteLine($"Time elapsed 2... {stopwatch.Elapsed}");

                var complexWordTask = SlowAndComplexWordAsync();

                Console.WriteLine("Time elapsed 3..." + stopwatch.Elapsed);

                complexSum = (await complexSumTask);

                Console.WriteLine("Time elapsed 4..." + stopwatch.Elapsed);

                complexWord = (await complexWordTask);

                Console.WriteLine("Time elapsed 5..." + stopwatch.Elapsed);
            }

            // Synchronous at the Result, tasks start and execute in parallel
            else if (testCase == 3)
            {
                var complexSumTask = SlowAndComplexSumAsync();

                Console.WriteLine($"Time elapsed 2... {stopwatch.Elapsed}");

                var complexWordTask = SlowAndComplexWordAsync();

                Console.WriteLine("Time elapsed 3..." + stopwatch.Elapsed);

                complexSum = complexSumTask.Result;

                Console.WriteLine("Time elapsed 4..." + stopwatch.Elapsed);

                complexWord = complexWordTask.Result;

                Console.WriteLine("Time elapsed 5..." + stopwatch.Elapsed);
            }

            // Synchronous at the await Task.WhenAll, tasks start and execute in parallel
            else if (testCase == 4)
            {
                var complexSumTask = SlowAndComplexSumAsync();

                Console.WriteLine($"Time elapsed 2... {stopwatch.Elapsed}");

                var complexWordTask = SlowAndComplexWordAsync();

                Console.WriteLine("Time elapsed 3..." + stopwatch.Elapsed);

                var complexSumTaskResultRef = complexSumTask.Result;

                Console.WriteLine("Time elapsed 4..." + stopwatch.Elapsed);

                var complexWordTaskResultRef = complexWordTask.Result;

                Console.WriteLine("Time elapsed 5..." + stopwatch.Elapsed);

                complexSum = complexSumTaskResultRef;

                Console.WriteLine("Time elapsed 6..." + stopwatch.Elapsed);

                complexWord = complexWordTaskResultRef;

                Console.WriteLine("Time elapsed 7..." + stopwatch.Elapsed);
            }

            // Synchronous at the await Task.WhenAll, tasks start and execute in parallel
            else if (testCase == 5)
            {
                var complexSumTask = SlowAndComplexSumAsync();

                Console.WriteLine($"Time elapsed 2... {stopwatch.Elapsed}");

                var complexWordTask = SlowAndComplexWordAsync();

                Console.WriteLine("Time elapsed 3..." + stopwatch.Elapsed);

                await Task.WhenAll(complexSumTask, complexWordTask);

                Console.WriteLine("Time elapsed 4..." + stopwatch.Elapsed);

                complexSum = complexSumTask.Result;

                Console.WriteLine("Time elapsed 5..." + stopwatch.Elapsed);

                complexWord = complexWordTask.Result;

                Console.WriteLine("Time elapsed 6..." + stopwatch.Elapsed);
            }

            Console.WriteLine($"Result of complex sum = {complexSum.result} @ {complexSum.elapsedTime}");
            Console.WriteLine($"Result of complex letter processing {complexWord.result} @ {complexSum.elapsedTime}");
        }

        private static async Task<TimedResult<int>> SlowAndComplexSumAsync()
        {
            Console.WriteLine($"Starting SlowAndComplexSumAsync");

            TimedResult<int> timedresult = new TimedResult<int>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            int sum = 0;
            foreach (var counter in Enumerable.Range(0, 25))
            {
                sum += counter;
                await Task.Delay(100);
            }

            timedresult.elapsedTime = stopwatch.Elapsed;
            timedresult.result = sum;

            Console.WriteLine($"Ending SlowAndComplexSumAsync");

            return timedresult;
        }

        private static async Task<TimedResult<string>> SlowAndComplexWordAsync()
        {
            Console.WriteLine($"Starting SlowAndComplexWordAsync");

            TimedResult<string> timedresult = new TimedResult<string>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var word = string.Empty;
            foreach (var counter in Enumerable.Range(65, 26))
            {
                word = string.Concat(word, (char)counter);
                await Task.Delay(150);
            }

            timedresult.elapsedTime = stopwatch.Elapsed;
            timedresult.result = word;

            Console.WriteLine($"Ending SlowAndComplexWordAsync");

            return timedresult;
        }
    }
}
