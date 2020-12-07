using System;
using System.Collections.Generic;
using System.Linq;

namespace IS_HW_04
{
    class Program
    {
        public static void Main()
        {
            var game = new TTTGame();

            char[,] board = new char[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = ' ';
                }
            }

            int player = 1;
            int flag = -1; // to check if the game is over or not
            char symbol;

            Console.WriteLine("Would you like to play first? (y/n):");
            char c = char.Parse(Console.ReadLine());

            if (c == 'n')
            {
                player = 2;
            }

            do
            {
                game.PrintBoard(board);
                Console.WriteLine();

                if (player % 2 == 1)
                {
                    player = 1;
                }
                else
                {
                    player = 2;
                }

                if (player == 1)
                {
                    symbol = 'X';
                }
                else
                {
                    symbol = 'O';
                }

                bool fail = true;
                int x = -1;
                int y = -1;

                if (player == 1)
                {
                    // Input
                    do
                    {
                        Console.WriteLine($"It's your turn. Enter row and col:");

                        try
                        {
                            int[] options = Console.ReadLine().Split().Select(int.Parse).ToArray();

                            x = options[0];
                            y = options[1];
                        }
                        catch (Exception)
                        {

                        }

                        if (x >= 0 & x <= 2 && y >= 0 && y <= 2 && board[x, y] == ' ')
                        {
                            fail = false;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Try again!");
                            Console.WriteLine();
                        }

                    } while (fail);

                    board[x, y] = symbol;
                }
                else if (player == 2)
                {
                    Console.WriteLine("Computer's Move:");

                    Move computerMove = game.Minimax(board);

                    board[computerMove.X, computerMove.Y] = symbol;
                }

                flag = game.Score(board);
                player++;

            } while (flag == -1);

            game.PrintBoard(board);

            Console.WriteLine();

            if (flag == 10)
            {
                Console.WriteLine("Winner: YOU");
            }
            else if (flag == -10)
            {
                Console.WriteLine("Winner: COMPUTER");
            }
            else
            {
                Console.WriteLine("Result: TIE");
            }

            Console.WriteLine();
        }
    }
}