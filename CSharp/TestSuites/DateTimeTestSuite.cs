using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CSharp.TestSuites
{
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

            Console.WriteLine($"Date Time:          {dt}");
        }

        private void DateTime2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            DateTimeOffset localTime = DateTimeOffset.Now;  // set to the current date and time, with the offset set to the local time's offset from Coordinated Universal Time (UTC)

            Console.WriteLine($"Local Time:          {localTime}");
            Console.WriteLine($"Difference from UTC: {localTime.Offset}");

            DateTimeOffset utcTime = DateTimeOffset.UtcNow; // set to the current Coordinated Universal Time (UTC) date and time and whose offset is TimeSpan.Zero

            Console.WriteLine($"UTC:                 {utcTime}");
            Console.WriteLine($"Difference from UTC: {utcTime.Offset}");
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
                Console.WriteLine($"Current Time in TZ: {dto}");
                Console.WriteLine();
            }
        }
    }
}
