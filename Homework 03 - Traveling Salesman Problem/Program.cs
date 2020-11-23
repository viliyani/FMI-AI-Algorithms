using System;

namespace FirstSteps
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());

            var solver = new TSPSolver(n);
            solver.Solve();

            Console.WriteLine("===========   Iteration: 0   ===========");
            solver.PrintInfo();

            // Evolve 100 times
            for (int i = 1; i <= 100; i++)
            {
                solver.EvolvePopulation();

                // Print info
                if (i == 10 || i == 20 || i == 40 || i == 70 || i == 100)
                {
                    Console.WriteLine($"===========   Iteration: {i}   ===========");
                    solver.PrintInfo();
                }
            }
            
        }
    }
}
