using System;

namespace IS_HW_04
{
    public class TTTGame
    {
        // Marks
        private char player = 'X';
        private char opponent = 'O';

        public TTTGame()
        {

        }

        // Function that print board
        public void PrintBoard(char[,] board)
        {
            Console.WriteLine("-----------");
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Console.Write(board[row, col] + " | ");
                }
                Console.WriteLine();
                Console.WriteLine("-----------");
            }
        }

        // Function that checks if it is a leaf board
        public bool IsLeafBoard(char[,] board)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row, col] == ' ')
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // Score Function
        public int Score(char[,] board)
        {
            // Check the rows
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                {
                    if (board[i, 0] == player)
                    {
                        return +10;
                    }
                    else if (board[i, 0] == opponent)
                    {
                        return -10;
                    }
                }
            }

            // Check the columns
            for (int i = 0; i < 3; i++)
            {
                if (board[0, i] == board[1, i] && board[1, i] == board[2, i])
                {
                    if (board[0, i] == player)
                    {
                        return +10;
                    }
                    else if (board[0, i] == opponent)
                    {
                        return -10;
                    }
                }
            }

            // Check the diagonals
            if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
            {
                if (board[0, 0] == player)
                {
                    return +10;
                }
                else if (board[0, 0] == opponent)
                {
                    return -10;
                }
            }
            if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
            {
                if (board[0, 2] == player)
                {
                    return +10;
                }
                else if (board[0, 2] == opponent)
                {
                    return -10;
                }
            }

            // Score is returned as 0 if there's a tie.

            if (board[0, 0] != ' ' && board[0, 1] != ' ' && board[0, 2] != ' ' && board[1, 0] != ' ' && board[1, 1] != ' ' && board[1, 2] != ' ' && board[2, 0] != ' ' && board[2, 1] != ' ' && board[2, 2] != ' ')
            {
                return 0;
            }

            // Exit condition
            return -1;
        }

        // Recursive function to find the optimal move which gives the min score for the minimizing player.
        public int MinSearch(char[,] board, int depth, int alpha, int beta)
        {
            // If terminal nodes are reached. i.e, the game has a clear winner.
            int scoreBoard = Score(board);

            if (scoreBoard == 10)
            {
                return 10;
            }
            if (scoreBoard == -10)
            {
                return -10;
            }
            if (scoreBoard == 0)
            {
                return 0;
            }

            int score = int.MaxValue;

            // Finds the best move i.e, min score amongst the available moves.
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == ' ')
                    {
                        // alpha-beta pruning
                        board[i, j] = opponent;
                        score = Math.Min(score, MaxSearch(board, depth + 1, alpha, beta) + depth);
                        alpha = Math.Min(alpha, score);
                        board[i, j] = ' ';

                        if (beta <= alpha)
                        {
                            return alpha;
                        }
                    }
                }
            }

            return score;
        }

        // Recursive function to find the optimal move which gives the max score for the maximizing player.
        public int MaxSearch(char[,] board, int depth, int alpha, int beta)
        {
            // If terminal nodes are reached. i.e, the game has a clear winner.
            int scoreBoard = Score(board);

            if (scoreBoard == 10)
            {
                return 10;
            }
            if (scoreBoard == -10)
            {
                return -10;
            }
            if (scoreBoard == 0)
            {
                return 0;
            }

            int score = int.MinValue;

            // Finds the best move i.e, max score amongst the available moves.
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == ' ')
                    {
                        // alpha - beta pruning
                        board[i, j] = player;
                        score = Math.Max(score, MinSearch(board, depth + 1, alpha, beta) - depth);
                        alpha = Math.Max(alpha, score);
                        board[i, j] = ' ';

                        if (beta <= alpha)
                        {
                            return alpha;
                        }
                    }
                }
            }

            return score;
        }

        // Implementation of the minimax algorithm
        public Move Minimax(char[,] board)
        {
            int score = int.MaxValue;

            Move move = new Move();
            int depth = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == ' ')
                    {
                        board[i, j] = opponent; // here the the computer is considered as the maximizing player.

                        int temp = MaxSearch(board, depth, int.MinValue, int.MaxValue);

                        if (temp < score)
                        {
                            score = temp;
                            move.X = i;
                            move.Y = j;
                        }
                        board[i, j] = ' ';
                    }
                }
            }

            return move;
        }

    }
}
