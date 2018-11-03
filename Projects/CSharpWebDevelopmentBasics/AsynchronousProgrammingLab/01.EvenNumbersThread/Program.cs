using System;
using System.Linq;
using System.Threading;

namespace _01.EvenNumbersThread
{
    class Program
    {
        static void Main(string[] args)
        {
            var range = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray(); ;
            var start = range[0];
            var end = range[1];
            Thread evens = new Thread(() => PrintEvenNumbers(start, end));

            evens.Start();
            evens.Join();
            Console.WriteLine("Thread finished work");
            
        }
        private static void PrintEvenNumbers(int start,int end)
        {
            for (int i = start; i <= end; i++)
            {
                if (i%2 == 0)
                {
                     Console.WriteLine(i);
                }
            }
        }
    }
}
