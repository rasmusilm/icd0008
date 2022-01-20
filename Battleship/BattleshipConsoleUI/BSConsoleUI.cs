using System;
using BattleshipBasicTypes;

namespace BattleshipConsoleUI
{
    public static class BSConsoleUI
    {
        public static void DrawBoardSimple(BoardSquareState[,] board)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                for (int x = 0; x < board.GetLength(0); x++)
                {
                    Console.Write(board[x, y]);
                }

                Console.WriteLine();
            }
        }
        
        
        public static void DrawBoardSimpleBorders(BoardSquareState[,] board)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                for (int x = 0; x < board.GetLength(0); x++)
                {
                    Console.Write("|" + board[x, y]);
                }

                Console.WriteLine("|");
            }
        }

        public static void DrawBoard(BoardSquareState[,] board)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";

            Console.Write(new string(' ', board.GetLength(1).ToString().Length));

            for (int i = 0; i < board.GetLength(1); i++)
            {
                var text = " " + alphabet[i % alphabet.Length];
                Console.Write(text.ToUpper());
            }

            Console.WriteLine(" ");

            int buffer = board.GetLength(1).ToString().Length;
            
            for (int y = 0; y < board.GetLength(1); y++)
            {
                Console.Write(new string(' ', buffer - (y + 1).ToString().Length));
                Console.Write(y + 1);
                for (int x = 0; x < board.GetLength(0); x++)
                {
                    Console.Write("|" + board[x, y]);
                }

                Console.WriteLine("|");
            }
        }
        
        public static void DrawBoardsSideBySide(BoardSquareState[,] board, BoardSquareState[,] board2)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";

            Console.Write(new string(' ', board.GetLength(1).ToString().Length));

            for (int i = 0; i < board.GetLength(1); i++)
            {
                var text = " " + alphabet[i % alphabet.Length];
                Console.Write(text.ToUpper());
            }
            
            Console.Write("     ");
            
            for (int i = 0; i < board2.GetLength(1); i++)
            {
                var text = " " + alphabet[i % alphabet.Length];
                Console.Write(text.ToUpper());
            }

            Console.WriteLine(" ");

            int buffer = board.GetLength(1).ToString().Length;
            
            for (int y = 0; y < board.GetLength(1); y++)
            {
                Console.Write(new string(' ', buffer - (y + 1).ToString().Length));
                Console.Write(y + 1);
                for (int x = 0; x < board.GetLength(0); x++)
                {
                    Console.Write("|" + board[x, y]);
                }

                Console.Write("|");
                
                Console.Write(" ");
                
                for (int x = 0; x < board2.GetLength(0); x++)
                {
                    Console.Write("|" + board2[x, y]);
                }

                Console.WriteLine("|");
            }
        }
    }
}