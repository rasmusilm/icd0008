using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using CalculatorBrain;
using MenuLibrary;

namespace Demo1
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello calc!");

            var mainMenu = new Menu(EMenuLevel.root, "Main menu", ShowCurrentValue);
            mainMenu.Background = ConsoleColor.DarkBlue;
            mainMenu.TextColor = ConsoleColor.Gray;
            
            mainMenu.AddMenuItems(new[]
            {
                new MenuItem("B", "Binary operations", BinaryMenu),
                new MenuItem("U", "Unary operations", UnaryMenu),
                new MenuItem("C", "Reset current value", Reset),
                new MenuItem("S", "Set current value", SetValue)
            });
            
            mainMenu.Run();
        }

        static double CurrentValue;

        private static readonly List<string> _menuSpecialShortcuts = new List<string>()
        {
            Menu.menuItemExit.Shortcut, 
            Menu.menuItemMain.Shortcut
        };

        static string BinaryMenu()
        {
            var menu1 = new Menu(EMenuLevel.primary, "Binary operations", ShowCurrentValue);
            menu1.AddMenuItems(new []
            {
                new MenuItem("+", "Adding", Add), 
                new MenuItem("-", "Subtraction", Subtract),
                new MenuItem("*", "Multiplication", Multiply),
                new MenuItem("/", "Division", Divide),
                new MenuItem("p", "Power", PowerOfX)
            });
            var res = menu1.Run();

            return res;
        }

        static string UnaryMenu()
        {
            var menu1 = new Menu(EMenuLevel.primary, "Unary operations", ShowCurrentValue);
            menu1.AddMenuItems(new []
            {
                new MenuItem("-", "Negate", Negate), 
                new MenuItem("/", "Square root", SquareRoot),
                new MenuItem("2", "Square", Square),
                new MenuItem("A", "Absolute value", Absolute)
            });
            
            var res = menu1.Run();

            return res;
        }

        static string Add()
        {
            Console.WriteLine("Current value: " + CurrentValue);
            Console.WriteLine("plus");
            Console.Write("number: ");
            
            var n = Console.ReadLine()?.Trim();
            
            if (_menuSpecialShortcuts.Contains(n.ToUpper())) { return n.ToUpper(); }
            
            double.TryParse(n, out var converted);

            CurrentValue += converted;

            return "";
        }

        static string Subtract()
        {
            Console.WriteLine("Current value: " + CurrentValue);
            Console.WriteLine("minus");
            Console.Write("number: ");
            
            var n = Console.ReadLine()?.Trim();
            
            if (_menuSpecialShortcuts.Contains(n.ToUpper())) { return n.ToUpper(); }
            
            double.TryParse(n, out var converted);

            CurrentValue -= converted;
            
            return "";
        }

        static string Multiply()
        {
            Console.WriteLine("Current value: " + CurrentValue);
            Console.WriteLine("times");
            Console.Write("number: ");
            
            var n = Console.ReadLine()?.Trim();
            
            if (_menuSpecialShortcuts.Contains(n.ToUpper())) { return n.ToUpper(); }
            
            double.TryParse(n, out var converted);

            CurrentValue *= converted;
            
            return "";
        }
        
        static string Divide()
        {
            Console.WriteLine("Current value: " + CurrentValue);
            Console.WriteLine("by");
            Console.Write("number: ");
            
            var n = Console.ReadLine()?.Trim();

            if (_menuSpecialShortcuts.Contains(n.ToUpper())) { return n.ToUpper(); }
            
            double.TryParse(n, out var converted);

            CurrentValue /= converted;
            
            return "";
        }

        static string PowerOfX()
        {
            Console.WriteLine("Current value: " + CurrentValue);
            Console.WriteLine("to");
            Console.Write("number: ");
            
            var n = Console.ReadLine()?.Trim();

            if (_menuSpecialShortcuts.Contains(n.ToUpper())) { return n.ToUpper(); }
            
            double.TryParse(n, out var converted);

            CurrentValue = Math.Pow(CurrentValue, converted);

            return "";
        }

        static string Negate()
        {
            CurrentValue = -CurrentValue;
            return "";
        }

        static string Absolute()
        {
            CurrentValue = Math.Abs(CurrentValue);
            return "";
        }

        static string Square()
        {
            CurrentValue *= CurrentValue;
            return "";
        }

        static string SquareRoot()
        {
            if (CurrentValue >= 0)
            {
                CurrentValue = Math.Sqrt(CurrentValue);
            }
            else
            {
                Console.WriteLine("Can't take square root from negative value: " + CurrentValue);
            }
            return "";
        }

        static string ShowCurrentValue()
        {
            return "Current value: " + CurrentValue;
        }

        static string Reset()
        {
            CurrentValue = 0;
            return "";
        }

        static string SetValue()
        {
            Console.WriteLine("Current value: " + CurrentValue);
            Console.WriteLine("__________________________");
            Console.Write("New value: ");
            
            var n = Console.ReadLine()?.Trim();

            if (_menuSpecialShortcuts.Contains(n.ToUpper())) { return n.ToUpper(); }
            
            double.TryParse(n, out var converted);

            CurrentValue = converted;

            return "";
        }

        static string MainMenu()
        {
            var userChoice = "";
            var done = false;
            do
            {
                done = false;
            
                Console.WriteLine("-------------------");
                Console.WriteLine("Current value: 0");
                Console.WriteLine("C) Classical ops");
                Console.WriteLine("W) Weird stuff");
                Console.WriteLine("N) Enter number");
                Console.WriteLine("-------------------");
                Console.WriteLine("E) Exit");
                Console.WriteLine("===================");
                Console.Write("Your choice:>");
            
                userChoice = Console
                                     .ReadLine()?
                                     .Trim()
                                     .ToUpper()
                                 ?? "";

                switch (userChoice)
                {
                    case "C":
                        done = true;
                        break;
                    case "W":
                    case "N":
                        break;
                }
            } while (!done);

            return userChoice;
        }
    }
}