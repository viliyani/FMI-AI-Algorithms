namespace FirstSteps
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int N = InputReader.ReadBlocksNumber();
            int I = InputReader.ReadZeroPosition();
            int[,] matrix = InputReader.ReadMatrix(N);

            // # Solution with A* algorithm
            //var astarSolver = new AstarSolver(N, I, matrix);
            //astarSolver.Solve();
            //astarSolver.PrintSolution();

            // # Solution with IDA* algorithm
            var IDAstarSolver = new IDAstarSolver(N, I, matrix);
            IDAstarSolver.Solve();
            IDAstarSolver.PrintSolution();

        }
    }
}
