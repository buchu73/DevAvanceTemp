using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfiniteLoop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int numberOfThreads = Environment.ProcessorCount * 8; // Use one thread per CPU core

            Console.WriteLine($"Starting {numberOfThreads} threads to maximize CPU usage.");

            for (int i = 0; i < numberOfThreads; i++)
            {
                Thread thread = new Thread(ComputePrimes);
                thread.Start();
            }

            Console.WriteLine("Press Enter to stop...");
            Console.ReadLine();
        }

        static void ComputePrimes()
        {
            int primeCount = 0;
            int number = 2; // Start checking for primes from number 2

            while (true)
            {
                while (primeCount < 10000) // Limit to finding 10,000 prime numbers
                {
                    if (IsPrime(number))
                    {
                        primeCount++;
                        //Console.WriteLine(number);
                    }
                    number++;
                }
            }

            Console.WriteLine("Finished computing prime numbers.");
        }

        static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;

            // Check divisibility from 2 to the square root of the number
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0) return false;
            }

            return true;
        }
    }
}
