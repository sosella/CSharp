using System;
using System.Reflection;

namespace CSharp.TestSuites
{
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

            Console.WriteLine($"{num1} + {num2} = {sum}");
        }

        private void OperatorOverloading2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            Note B = new Note(2);
            Note Csharp = B + 2;

            Console.WriteLine($"{B} + 2 = {Csharp}");

            Note n = (Note)554.37;  // explicit
            double x = n;           // implicit

            Console.WriteLine($"{n} = {x}");
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
            return ($"({real} + {imaginary}i)");
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
}
