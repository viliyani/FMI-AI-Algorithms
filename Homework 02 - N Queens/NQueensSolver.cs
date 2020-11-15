using System;

namespace NQueens
{
    public class NQueensSolver
    {
        // Number of queens
        private int N;

        // Position of the queens on the board
        private int[] queens; // index = col, value = row

        // Total queens for each row
        private int[] rowQueens; // index = row, value = number of queens at the current row

        // Total queens for each main diagonal
        private int[] diagonal1Queens; // index = diagonal num, value = queens count

        // Total queens for each secondary diagonal
        private int[] diagonal2Queens; // index = diagonal num, value = queens count

        // Helper for equal possible positions
        private int[] freePositions;

        // Index for the freePositions array
        private int freePosIdx;

        // Random helper
        private Random rand;

        // Has Conflicts
        private bool hasConflicts;

        public NQueensSolver(int n)
        {
            N = n;
            queens = new int[N];
            for (int i = 0; i < N; i++)
            {
                queens[i] = -1;
            }
            rowQueens = new int[N];
            diagonal1Queens = new int[2 * N - 1];
            diagonal2Queens = new int[2 * N - 1];
            freePositions = new int[N];
            freePosIdx = 0;
            rand = new Random();
            hasConflicts = true;
        }

        public void Solve()
        {
            // 1. Initialize queens positions
            InitFirstState();

            if (N < 21)
            {
                Console.WriteLine("Init board:");
                PrintBoard();
            }

            int br = 0; // Anti infinite loop

            while (br++ < 1000)
            {
                // 2. Find Max Queen's Conflicts
                int bestCol = GetColWithQueenWithMaxConf();

                if (hasConflicts == false)
                {
                    // break -> The solution has been found.
                    break;
                }

                // 3. Find Min Row's Conflicts for the found Queen
                int bestRow = GetMinConflictsForCol(bestCol);

                // 4. Decrease Old Queen Cell Conflicts
                DecreaseCellConflicts(queens[bestCol], bestCol);

                // 5. Increase New Queen Cell Conflicts
                IncreaseCellConflicts(bestRow, bestCol);

                // 6. Move The Queen
                queens[bestCol] = bestRow;
            }

        }

        public int GetColWithQueenWithMaxConf()
        {
            int maxConflicts = GetConflictsForCell(queens[0], 0);
            freePositions[0] = 0;
            freePosIdx = 1;

            for (int col = 1; col < N; col++)
            {
                int currentConflicts = GetConflictsForCell(queens[col], col);

                if (currentConflicts > maxConflicts)
                {
                    maxConflicts = currentConflicts;
                    freePositions[0] = col;
                    freePosIdx = 1;
                }
                else if (currentConflicts == maxConflicts)
                {
                    freePositions[freePosIdx] = col;
                    freePosIdx++;
                }
            }

            if (maxConflicts == 0)
            {
                hasConflicts = false;
            }

            // Random get col from possible cols (cols that are with the same max conflicts value)
            int randomPossibleCol = freePositions[rand.Next(0, freePosIdx)];

            return randomPossibleCol;
        }

        public void InitFirstState()
        {
            // First queen go to the first cell
            queens[0] = 0;
            IncreaseCellConflicts(0, 0);

            for (int col = 1; col < N; col++)
            {
                // Find row with min conflicts for given col
                int bestRow = GetMinConflictsForCol(col);

                // Set queen at position with most little conflicts
                queens[col] = bestRow;

                // Update conflicts information
                IncreaseCellConflicts(bestRow, col);
            }
        }

        public int GetMinConflictsForCol(int col)
        {
            // Find row with min conflicts
            int minConflicts = GetConflictsForCell(0, col);
            freePositions[0] = 0;
            freePosIdx = 1;

            for (int row = 1; row < N; row++)
            {
                int currentConflicts = GetConflictsForCell(row, col);

                if (currentConflicts < minConflicts)
                {
                    minConflicts = currentConflicts;
                    freePositions[0] = row;
                    freePosIdx = 1;
                }
                else if (currentConflicts == minConflicts)
                {
                    freePositions[freePosIdx] = row;
                    freePosIdx++;
                }
            }

            // Random get row from possible rows (rows that are with the same min conflicts value)
            int randomPossibleRow = freePositions[rand.Next(0, freePosIdx)];

            return randomPossibleRow;
        }

        public int GetConflictsForCell(int row, int col)
        {
            int remove = 0;

            if (queens[col] == row)
            {
                // There is a queen in this cell
                remove = 3; // remove 3 queens in the count for conflicts
            }

            return rowQueens[row] + diagonal1Queens[GetDiagonalIdx(row, col)] + diagonal2Queens[row + col] - remove;
        }

        public void IncreaseCellConflicts(int row, int col)
        {
            // Update conflicts for row
            rowQueens[row]++;

            // Update conflicts for diagonal 1
            diagonal1Queens[GetDiagonalIdx(row, col)]++;

            // Update conflicts for diagonal 2
            diagonal2Queens[row + col]++;
        }

        public void DecreaseCellConflicts(int row, int col)
        {
            // Update conflicts for row
            rowQueens[row]--;

            // Update conflicts for diagonal 1
            diagonal1Queens[GetDiagonalIdx(row, col)]--;

            // Update conflicts for diagonal 2
            diagonal2Queens[row + col]--;
        }

        // Calculate Main Diagonal Idx by given row and col
        public int GetDiagonalIdx(int row, int col)
        {
            int center = (2 * N - 1) / 2;

            int x = row - col;

            return x + center;
        }

        public void PrintSolution()
        {
            // If N < 21 -> Print The Board
            if (N < 21)
            {
                Console.WriteLine("Final Board:");
                PrintBoard();
            }

            // Print result
            Console.WriteLine("-> Result!");
        }

        public void PrintBoard()
        {
            Console.WriteLine();
            for (int row = 0; row < N; row++)
            {
                for (int col = 0; col < N; col++)
                {
                    if (queens[col] == row)
                    {
                        // This is a queen
                        Console.Write("Q ");
                    }
                    else
                    {
                        Console.Write(". ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
