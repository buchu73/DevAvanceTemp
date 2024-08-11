using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiMonteCarloMono
{
    internal class Program
    {
        private const long NumPoints = 10000000000; // Nombre total de points
        private static readonly Random Random = new Random();

        static void Main()
        {
            long insideCircle = 0;

            Stopwatch stopwatch = Stopwatch.StartNew();

            // Génération des points et comptage de ceux à l'intérieur du cercle
            for (long i = 0; i < NumPoints; i++)
            {
                double x = Random.NextDouble();
                double y = Random.NextDouble();
                if (x * x + y * y <= 1.0)
                {
                    insideCircle++;
                }
            }

            // Estimation de Pi
            double piEstimate = 4.0 * insideCircle / NumPoints;

            stopwatch.Stop();

            Console.WriteLine($"Estimated Pi: {piEstimate}, computed in {stopwatch.Elapsed.TotalSeconds} seconds");
        }
    }
}
