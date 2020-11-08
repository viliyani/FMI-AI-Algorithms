using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstSteps
{
    public class AstarSolver
    {
        private int blocksCount;
        private int rowsCount;
        private int zeroPosition;
        private List<int> initStateData;
        private List<int> goalStateData;
        private PriorityQueue priorityQueue;
        private bool isFound;
        private bool isSolvable;
        private List<string> directions;
        private State firstState;

        public AstarSolver(int blocksCountPar, int zeroPositionPar, int[,] matrix)
        {
            blocksCount = blocksCountPar;
            rowsCount = matrix.GetLength(0);
            zeroPosition = zeroPositionPar;
            initStateData = MyHelper.ConvertMatrixToList(matrix);
            goalStateData = MyHelper.GenerateGoalStateData(blocksCount, zeroPosition);
            priorityQueue = new PriorityQueue();
            isFound = false;
            isSolvable = true;
            directions = new List<string>();
        }

        public void Solve()
        {
            InitStartState();

            // Check if the init state is solvable
            isSolvable = IsSolvable(firstState.ZeroRow, firstState.ZeroCol);

            if (isSolvable)
            {
                AstarAlgorithm();
            }
        }

        public bool IsSolvable(int zeroRow, int zeroCol)
        {
            int inversionsCount = MyHelper.CalculateInversions(initStateData);

            if (rowsCount % 2 != 0)
            {
                // Grid is odd => return true if inversion count is even
                return inversionsCount % 2 == 0;
            }
            else
            {
                // Grid is even
                int sum = inversionsCount + zeroRow;

                return sum % 2 != 0;
            }
        }

        public void AstarAlgorithm()
        {
            for (int i = 1; i <= 100; i++)
            {
                State bestState = priorityQueue.Dequeue();

                // Save direction
                directions.Add(bestState.Direction);

                // Generate the children of the best state
                GenerateChildren(bestState);

                // If solution is found -> break the loop
                if (this.isFound == true)
                {
                    break;
                }
            }
        }

        public void GenerateChildren(State state)
        {
            int[,] stateMatrix = MyHelper.ConvertListToMatrix(state.Data);

            int nextZeroRow = -1;
            int nextZeroCol = -1;

            // up
            if (MyHelper.IsValid(state.ZeroRow - 1, state.ZeroCol, stateMatrix))
            {
                nextZeroRow = state.ZeroRow - 1;
                nextZeroCol = state.ZeroCol;

                AddChildToStates(state, nextZeroRow, nextZeroCol, "down");
            }

            // down
            if (MyHelper.IsValid(state.ZeroRow + 1, state.ZeroCol, stateMatrix))
            {
                nextZeroRow = state.ZeroRow + 1;
                nextZeroCol = state.ZeroCol;

                AddChildToStates(state, nextZeroRow, nextZeroCol, "up");
            }

            // right
            if (MyHelper.IsValid(state.ZeroRow, state.ZeroCol + 1, stateMatrix))
            {
                nextZeroRow = state.ZeroRow;
                nextZeroCol = state.ZeroCol + 1;

                AddChildToStates(state, nextZeroRow, nextZeroCol, "left");
            }

            // left
            if (MyHelper.IsValid(state.ZeroRow, state.ZeroCol - 1, stateMatrix))
            {
                nextZeroRow = state.ZeroRow;
                nextZeroCol = state.ZeroCol - 1;

                AddChildToStates(state, nextZeroRow, nextZeroCol, "right");
            }
        }

        public void AddChildToStates(State state, int nextZeroRow, int nextZeroCol, string direction)
        {
            List<int> childData = new List<int>();
            MyHelper.CopyList(state.Data, childData);

            MyHelper.Swap(childData, GetIndexByRowAndCol(state.ZeroRow, state.ZeroCol), GetIndexByRowAndCol(nextZeroRow, nextZeroCol));

            // Check if this child is the same as its grandparent

            bool isChildAsGrandpa = false;

            if (priorityQueue.States.Exists(x => MyHelper.CheckEqualLists(x.Data, childData)))
            {
                isChildAsGrandpa = true;
            }

            if (isChildAsGrandpa == false)
            {
                // This child is different than the grandparent => add it to the queue

                int childDepth = state.Depth + 1;
                int childDistance = MyHelper.CalculateManhattanDistance(childData, goalStateData);

                State childState = new State
                {
                    Depth = childDepth,
                    ManhattanDistance = childDistance,
                    Fvalue = childDepth + childDistance,
                    ZeroRow = nextZeroRow,
                    ZeroCol = nextZeroCol,
                    Direction = direction,
                    Data = childData,
                    Parent = state
                };

                priorityQueue.Enqueue(childState);

                // Check if the goal is reached
                if (childDistance == 0)
                {
                    directions.Add(direction);
                    isFound = true;
                }
            }

        }

        public List<int> GenerateChildData(List<int> parrentData)
        {
            List<int> result = new List<int>();

            for (int i = 0; i < parrentData.Count; i++)
            {
                result.Add(parrentData[i]);
            }

            return result;
        }

        public int GetIndexByRowAndCol(int row, int col)
        {
            return (row * rowsCount) + col;
        }

        public void InitStartState()
        {
            int distance = MyHelper.CalculateManhattanDistance(initStateData, goalStateData);

            var zeroInfo = MyHelper.FindZeroPosition(initStateData);

            State startState = new State
            {
                Depth = 0,
                ManhattanDistance = distance,
                Fvalue = distance,
                ZeroRow = zeroInfo["zeroRow"],
                ZeroCol = zeroInfo["zeroCol"],
                Data = initStateData,
                Parent = null
            };

            firstState = startState;

            priorityQueue.Enqueue(startState);
        }

        public void PrintSolution()
        {
            if (isSolvable)
            {
                if (isFound)
                {
                    directions.RemoveAt(0); // remove init state direction
                    Console.WriteLine();
                    Console.WriteLine($"The solution is with {directions.Count} moves:");
                    Console.WriteLine(String.Join(Environment.NewLine, directions));
                }
                else
                {
                    Console.WriteLine("No solution has been found!");
                }

                //Console.WriteLine($"---> TOTAL in QUEUE:  {priorityQueue.Count}");
            }
            else
            {
                Console.WriteLine($"The puzzle is not solvable!");
            }
        }
    }
}
