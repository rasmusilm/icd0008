using System;
using System.Text;

namespace BattleShipConsoleApp
{
    public class ConsoleNavigator
    {
        public static int MultipleChoice(bool canCancel, params string[] options)
        {
            const int startX = 0;
            const int startY = 0;
            const int optionsPerLine = 1;
            const int spacingPerLine = 0;

            int currentSelection = 0;

            ConsoleKey key;

            Console.CursorVisible = false;

            do
            {
                Console.Clear();

                for (int i = 0; i < options.Length; i++)
                {
                    Console.SetCursorPosition(startX + (i % optionsPerLine) * spacingPerLine, startY + i);

                    var atm = Console.ForegroundColor;

                    if (i == currentSelection)
                        Console.ForegroundColor = ConsoleColor.Red;

                    Console.Write(options[i]);

                    Console.ForegroundColor = atm;
                }

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                    {
                        if (currentSelection % optionsPerLine > 0)
                            currentSelection--;
                        break;
                    }
                    case ConsoleKey.RightArrow:
                    {
                        if (currentSelection % optionsPerLine < optionsPerLine - 1)
                            currentSelection++;
                        break;
                    }
                    case ConsoleKey.UpArrow:
                    {
                        if (currentSelection >= optionsPerLine)
                            currentSelection -= optionsPerLine;
                        break;
                    }
                    case ConsoleKey.DownArrow:
                    {
                        if (currentSelection + optionsPerLine < options.Length)
                            currentSelection += optionsPerLine;
                        break;
                    }
                    case ConsoleKey.Escape:
                    {
                        if (canCancel)
                            return -1;
                        break;
                    }
                }
            } while (key != ConsoleKey.Enter);

            Console.CursorVisible = true;

            return currentSelection;
        }
        
        public static int MultipleChoiceWithTitle(bool canCancel, string title, params string[] options)
        {

            int currentSelection = 0;

            ConsoleKey key;

            Console.CursorVisible = false;

            do
            {
                Console.Clear();
                
                var line = new StringBuilder();
                
                for (int i = 0; i < 8 + title.Length; i++) { line.Append('-');}

                Console.WriteLine(line);
                Console.WriteLine("===>" + title + "<===");
                Console.WriteLine(line);

                for (int i = 0; i < options.Length; i++)
                {
                    var atm = Console.ForegroundColor;

                    if (i == currentSelection)
                        Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine(options[i]);

                    Console.ForegroundColor = atm;
                }

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                    {
                        if (currentSelection >= 1)
                            currentSelection --;
                        break;
                    }
                    case ConsoleKey.DownArrow:
                    {
                        if (currentSelection < options.Length - 1)
                            currentSelection ++;
                        break;
                    }
                    case ConsoleKey.Escape:
                    {
                        if (canCancel)
                            return -1;
                        break;
                    }
                }
            } while (key != ConsoleKey.Enter);

            Console.CursorVisible = true;

            return currentSelection;
        }
    }
}