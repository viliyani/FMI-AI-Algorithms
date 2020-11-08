using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstSteps
{
    public static class InputReader
    {
        public static int ReadBlocksNumber()
        {
            int n = 0;

            List<int> validNumbers = new List<int>() { 8, 15, 24 };

            while (!validNumbers.Contains(n))
            {
                Console.WriteLine("Please enter the number of blocks (8, 15 or 24):");
                n = int.Parse(Console.ReadLine());
            }

            return n;
        }

        public static int ReadZeroPosition()
        {
            Console.WriteLine("Please enter the position of zero:");
            return int.Parse(Console.ReadLine());
        }

        public static int[,] ReadMatrix(int n)
        {
            Console.WriteLine("Please enter the matrix:");

            int rows = (int)Math.Sqrt(n + 1);
            int cols = rows;

            int[,] matrix = new int[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                int[] numbers = Console.ReadLine().Split().Select(int.Parse).ToArray();
                for (int col = 0; col < cols; col++)
                {
                    matrix[row, col] = numbers[col];
                }
            }

            return matrix;
        }
    }
}
