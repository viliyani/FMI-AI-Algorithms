using System;

namespace NQueens
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Input
            Console.WriteLine("Enter N:");
            int n = int.Parse(Console.ReadLine());

            // Start Timer
            var watch = System.Diagnostics.Stopwatch.StartNew();

            // N Queens Solver
            var solver = new NQueensSolver(n);
            solver.Solve();
            solver.PrintSolution();

            // Stop Timer
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            // Print Time Result
            Console.WriteLine();
            Console.WriteLine($"--> Total time: {elapsedMs} ms.");
            Console.WriteLine();
        }
    }
}
