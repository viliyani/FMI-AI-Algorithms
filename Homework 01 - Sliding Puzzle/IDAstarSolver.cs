using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstSteps
{
    public class IDAstarSolver
    {
        private int blocksCount;
        private int rowsCount;
        private int zeroPosition;
        private List<int> initStateData;
        private List<int> goalStateData;
        private List<string> directions;
        private int totalStates;
        private State foundState;
        private SortedSet<int> thresholdsData;
        private bool isSolvable;

        public IDAstarSolver(int blocksCountPar, int zeroPositionPar, int[,] matrix)
        {
            blocksCount = blocksCountPar;
            rowsCount = matrix.GetLength(0);
            zeroPosition = zeroPositionPar;
            initStateData = MyHelper.ConvertMatrixToList(matrix);
            goalStateData = MyHelper.GenerateGoalStateData(blocksCount, zeroPosition);
            foundState = null;
            isSolvable = true;
            directions = new List<string>();
            totalStates = 0;
            thresholdsData = new SortedSet<int>();
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

        public void Solve()
        {
            State firstState = InitStartState();

            // Check if the init state is solvable
            isSolvable = IsSolvable(firstState.ZeroRow, firstState.ZeroCol);

            if (isSolvable)
            {
                thresholdsData.Add(1);

                int count = 1;

                while (foundState == null)
                {
                    // breaker for anti infinite loop
                    if (count > 100)
                    {
                        break;
                    }

                    int threshold = thresholdsData.First();
                    thresholdsData.Remove(threshold);

                    //Console.WriteLine($"-> Threshold: {threshold}");
                    foundState = DFS(firstState, threshold);

                    count++;
                }
            }
        }

        public State DFS(State currentState, int threshold)
        {
            if (currentState.ManhattanDistance == 0)
            {
                return currentState;
            }

            if (currentState.Fvalue > threshold)
            {
                thresholdsData.Add(currentState.Fvalue);
                return null;
            }

            if (currentState.Neighbours.Count == 0)
            {
                // Generate children states of current state because it has not
                GenerateChildren(currentState);
            }

            foreach (var child in currentState.Neighbours)
            {
                State checkChildState = DFS(child, threshold);

                if (checkChildState != null)
                {
                    return checkChildState;
                }
            }

            return null;
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

            if (state.Parent != null && MyHelper.CheckEqualLists(state.Parent.Data, childData))
            {
                isChildAsGrandpa = true;
            }

            if (isChildAsGrandpa == false)
            {
                // This child is different than the grandparent

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
                    Parent = state,
                    Neighbours = new List<State>()
                };

                state.Neighbours.Add(childState);

                totalStates += 1;

                // Check if the goal is reached
                if (childDistance == 0)
                {
                    directions.Add(direction);
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

        public State InitStartState()
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
                Parent = null,
                Neighbours = new List<State>()
            };

            return startState;
        }

        public void PrintSolution()
        {
            Console.WriteLine();

            if (isSolvable)
            {
                if (foundState != null)
                {
                    fillDirections(); // generate directions list

                    directions.RemoveAt(directions.Count - 1); // remove init state direction
                    Console.WriteLine($"The solution is with {directions.Count} moves:");
                    Console.WriteLine(String.Join(Environment.NewLine, directions));
                }
                else
                {
                    Console.WriteLine("No solution has been found!");
                }

                //Console.WriteLine($"---> TOTAL STATES:  {totalStates}");
            }
            else
            {
                Console.WriteLine($"The puzzle is not solvable!");
            }
        }

        public void fillDirections()
        {
            State tempState = foundState;

            while (tempState.Parent != null)
            {
                directions.Add(tempState.Direction);

                tempState = tempState.Parent;
            }

            directions.Reverse();
        }
    }
}
