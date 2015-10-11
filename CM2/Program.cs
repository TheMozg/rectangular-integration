using System;
using System.Collections.Generic;

namespace CM2
{
    class Integral
    {
        public int N { get; set; }
        public double D { get; set; }
        public double R { get; set; }
        public double Offset { get; set; }
        private double CalculateForN(Func<double, double> function, double a, double b, int n)
        {
            double result = 0;
            double h = (b - a) / n;
            for (int i = 0; i < n; i++)
            {
                result += function(a + h * (i + Offset));
            }
            result *= h;
            return result;
        }
        public void Calculate(Func<double, double> function, double a, double b, double e)
        {
            if (a > b)
            {
                double tmp = a;
                a = b;
                b = tmp;
            }
            int n = 1;
            double lastResult = CalculateForN(function, a, b, n);
            n *= 2;
            double result = CalculateForN(function, a, b, n);
            while (Math.Abs(result - lastResult) > e)
            {
                lastResult = result;
                n *= 2;
                result = CalculateForN(function, a, b, n);
            }
            D = Math.Abs(result - lastResult);
            R = result;
            N = n;
        }
    }
    class Function
    {
        public Function(string s, Func<double, double> f)
        {
            Text = s;
            Func = f;
        }
        public string Text { get; set; }
        public Func<double, double> Func { get; set; }
    }
    class CLI
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Integration (rectangle method)");
            Console.WriteLine("Fedor Kalugin, P3210");
            Console.WriteLine();

            List<Function> functions = new List<Function>();
            //Add functions here:
            functions.Add(new Function("sin(x)", x => Math.Sin(x)));
            functions.Add(new Function("x", x => x));
            functions.Add(new Function("x^2", x => x*x));
            functions.Add(new Function("ln(x)", x => Math.Log(x)));
            functions.Add(new Function("1/x", x => 1/x));

            Console.WriteLine("Enter the number of action and press [Enter]. Then follow instructions.");
            while (true)
            {
                try
                {
                    Console.WriteLine("Menu:");
                    Console.WriteLine("0. Exit");
                    for(int i = 0; i < functions.Count; i++)
                    {
                        Console.WriteLine("{0}. Calculate function: y={1}", i+1, functions[i].Text);
                    }
                    int choice = getValidatedInt(0,functions.Count);
                    if (choice == 0)
                        break;
                    else
                    {
                        Integrate(functions[choice - 1]);
                    }
                }
                catch (AggregateException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void Integrate(Function function)
        {
            Console.Write("Enter lower bound: ");
            int a;
            while (!int.TryParse(Console.ReadLine(), out a))
                Console.WriteLine("Please enter valid NUMBER.");
            Console.Write("Enter upper bound: ");
            int b;
            while (!int.TryParse(Console.ReadLine(), out b))
                Console.WriteLine("Please enter valid NUMBER.");
            Console.Write("Enter epsilon: ");
            double e;
            while (!double.TryParse(Console.ReadLine(), out e))
                Console.WriteLine("Please enter valid floating-point NUMBER.");
            Integral integral = new Integral();
            Console.WriteLine("Integrating function (left): y={0}", function.Text);
            integral.Offset = 0;
            integral.Calculate(function.Func, a, b, e);
            Console.WriteLine("Result: {0}", integral.R);
            Console.WriteLine("Error: {0}", integral.D);
            Console.WriteLine("Strips: {0}", integral.N);
            Console.WriteLine("Integrating function (mid-point): y={0}", function.Text);
            integral.Offset = 0.5;
            integral.Calculate(function.Func, a, b, e);
            Console.WriteLine("Result: {0}", integral.R);
            Console.WriteLine("Error: {0}", integral.D);
            Console.WriteLine("Strips: {0}", integral.N);
            Console.WriteLine("Integrating function (right): y={0}", function.Text);
            integral.Offset = 1;
            integral.Calculate(function.Func, a, b, e);
            Console.WriteLine("Result: {0}", integral.R);
            Console.WriteLine("Error: {0}", integral.D);
            Console.WriteLine("Strips: {0}", integral.N);
            Console.WriteLine();
        }

        static int getValidatedInt(int min, int max)
        {
            int res = 0;
            for (;;)
            {
                Console.Write("> ");
                if (int.TryParse(Console.ReadLine(), out res))
                {
                    if (res >= min && res <= max)
                        break;
                }
                Console.WriteLine("Please enter valid NUMBER.");
            }
            return res;
        }
    }
}
