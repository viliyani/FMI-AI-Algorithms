using System;
using System.Collections.Generic;

namespace FirstSteps
{
    public static class MyHelper
    {
        // Calculate Inversion
        public static int CalculateInversions(List<int> data)
        {
            int count = 0;

            for (int i = 0; i < data.Count - 1; i++)
            {
                for (int j = i + 1; j < data.Count; j++)
                {
                    if (data[i] != 0 && data[j] != 0 && data[i] > data[j])
                    {
                        count += 1;
                    }
                }
            }

            return count;
        }

        // Check if two lists are equal
        public static bool CheckEqualLists(List<int> list1, List<int> list2)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }

            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] != list2[i])
                {
                    return false;
                }
            }

            return true;
        }

        // Generate Goal State Data List
        public static List<int> GenerateGoalStateData(int blocksCount, int zeroPosition)
        {
            List<int> data = new List<int>();

            int number = 1;

            if (zeroPosition == -1)
            {
                zeroPosition = blocksCount;
            }

            for (int i = 0; i < blocksCount + 1; i++)
            {
                if (i == zeroPosition)
                {
                    data.Add(0);
                }
                else
                {
                    data.Add(number++);
                }
            }

            return data;
        }

        // Convert Matrix To List
        public static List<int> ConvertMatrixToList(int[,] matrix)
        {
            List<int> list = new List<int>();

            int rows = matrix.GetLength(0);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < rows; col++)
                {
                    list.Add(matrix[row, col]);
                }
            }

            return list;
        }

        // Convert List To Matrix
        public static int[,] ConvertListToMatrix(List<int> list)
        {
            int rows = list.Count / (int)Math.Sqrt(list.Count);

            int[,] resultMatrix = new int[rows, rows];

            int listIdx = 0;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < rows; col++)
                {
                    resultMatrix[row, col] = list[listIdx++];
                }
            }

            return resultMatrix;
        }

        // Calculate Manhattan Distance
        public static int CalculateManhattanDistance(List<int> data, List<int> goalData)
        {
            int distance = 0;
            int rowsCount = data.Count / (int)Math.Sqrt(data.Count);

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] == 0)
                {
                    continue;
                }

                for (int j = 0; j < goalData.Count; j++)
                {
                    if (data[i] == goalData[j])
                    {
                        int row1 = i / rowsCount;
                        int col1 = i % rowsCount;
                        int row2 = j / rowsCount;
                        int col2 = j % rowsCount;

                        distance += Math.Abs(row1 - row2) + Math.Abs(col1 - col2);
                    }
                }
            }

            return distance;
        }

        // Copy content from one list to another
        public static void CopyList(List<int> from, List<int> to)
        {
            for (int i = 0; i < from.Count; i++)
            {
                to.Add(from[i]);
            }
        }

        public static Dictionary<string, int> FindZeroPosition(List<int> data)
        {
            int[,] matrix = ConvertListToMatrix(data);

            var result = new Dictionary<string, int>();

            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    if (matrix[row, col] == 0)
                    {
                        result.Add("zeroRow", row);
                        result.Add("zeroCol", col);
                    }
                }
            }

            return result;
        }

        public static void Swap(List<int> list, int idx1, int idx2)
        {
            int temp = list[idx1];
            list[idx1] = list[idx2];
            list[idx2] = temp;
        }

        public static bool IsValid(int row, int col, int[,] matrix)
        {
            if (row < 0 || col < 0)
            {
                return false;
            }

            if (row >= 0 && row < matrix.GetLength(0) && col >= 0 && col < matrix.GetLength(0))
            {
                return true;
            }

            return false;
        }

        // Print Matrix
        public static void PrintMatrix(int[,] currentMatrix)
        {
            for (int row = 0; row < currentMatrix.GetLength(0); row++)
            {
                for (int col = 0; col < currentMatrix.GetLength(1); col++)
                {
                    Console.Write(currentMatrix[row, col] + " ");
                }
                Console.WriteLine();
            }
        }

    }
}
