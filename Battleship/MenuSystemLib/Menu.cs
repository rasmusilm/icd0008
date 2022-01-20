using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MenuLibrary
{
    public class Menu
    {

        private readonly EMenuLevel _menuLevel;
        private readonly string _menuTitle;
        private bool DisplayMemory = false;
        private Func<string>? ShowMemory;

        public ConsoleColor Background;
        public ConsoleColor TextColor;

        public Menu( EMenuLevel menuLevel, string title)
        {
            _menuLevel = menuLevel;
            _menuTitle = title;

            Background = Console.BackgroundColor;
            TextColor = Console.ForegroundColor;

            ApplyMenuLevel(menuLevel);
        }

        public Menu(EMenuLevel menuLevel, string title, Func<string> showMemory)
        {
            DisplayMemory = true;
            ShowMemory = showMemory;
            
            _menuLevel = menuLevel;
            _menuTitle = title;

            Background = Console.BackgroundColor;
            TextColor = Console.ForegroundColor;
            
            ApplyMenuLevel(menuLevel);
        }

        private void ApplyMenuLevel(EMenuLevel menuLevel)
        {
            switch (menuLevel)
            {
                case EMenuLevel.root:
                    _menuSpecialShortcuts.Add(menuItemExit.Shortcut);
                    _specialItems.Add(menuItemExit);
                    break;
                case EMenuLevel.primary:
                    _menuSpecialShortcuts.Add(menuItemExit.Shortcut);
                    _specialItems.Add(menuItemExit);
                    _menuSpecialShortcuts.Add(menuItemReturn.Shortcut);
                    _specialItems.Add(menuItemReturn);
                    _menuSpecialShortcuts.Add(menuItemMain.Shortcut);
                    break;
                case EMenuLevel.secondary:
                    _menuSpecialShortcuts.Add(menuItemExit.Shortcut);
                    _specialItems.Add(menuItemExit);
                    _menuSpecialShortcuts.Add(menuItemReturn.Shortcut);
                    _specialItems.Add(menuItemReturn);
                    _menuSpecialShortcuts.Add(menuItemMain.Shortcut);
                    _specialItems.Add(menuItemMain);
                    _menuSpecialShortcuts.Add(menuItemSpecial.Shortcut);
                    _specialItems.Add(menuItemSpecial);
                    break;
                case EMenuLevel.special:
                    _menuSpecialShortcuts.Add(menuItemExit.Shortcut);
                    _specialItems.Add(menuItemExit);
                    _menuSpecialShortcuts.Add(menuItemReturn.Shortcut);
                    _specialItems.Add(menuItemReturn);
                    _menuSpecialShortcuts.Add(menuItemMain.Shortcut);
                    _specialItems.Add(menuItemMain);
                    break;
            }
        }

        public static MenuItem menuItemReturn = new ("R", "Return", (() => "R"));
        public static MenuItem menuItemExit = new ("X", "Exit", (() => "X"));
        public static MenuItem menuItemMain = new ("M", "Main", (() => "M"));
        public static MenuItem menuItemSpecial = new ("SP", "Back", (() => "SP"));
        private readonly HashSet<string> _menuShortcuts = new();
        private readonly HashSet<string> _menuSpecialShortcuts = new();

        private readonly List<MenuItem> _menuItems = new();
        private readonly List<MenuItem> _specialItems = new();

        public void AddMenuItem(MenuItem item, int position = -1)
        {
            if (!_menuSpecialShortcuts.Add(item.Shortcut.ToUpper()))
            {
                throw new ApplicationException(item.Shortcut + " already exists");
            }
            if (!_menuShortcuts.Add(item.Shortcut.ToUpper()))
            {
                throw new ApplicationException(item.Shortcut + " already exists");
            }

            if (position == -1)
            {
                _menuItems.Add(item);
            }
            else
            {
                _menuItems.Insert(position, item);
            }
        }

        public void DeleteMenuItem(int position = 0)
        {
            _menuItems.RemoveAt(position);
        }

        public void AddMenuItems(MenuItem[] items)
        {
            foreach (var menuItem in items)
            {
                AddMenuItem(menuItem);
            }
        }

        public string RunOld()
        {
            var input = "";
            var runDone = false;

            do
            {
                OutputMenu();
                Console.Write("Your choice: ");
                input = Console
                            .ReadLine()?
                            .Trim()
                            .ToUpper()
                        ?? "";
                
                var isInputValid = _menuShortcuts.Contains(input);

                if (isInputValid)
                {
                    var item = _menuItems.FirstOrDefault(t => t.Shortcut == input);
                    input = item?.methodToRun == null ? input : item.methodToRun();
                    CorrectColors();
                }

                runDone = _menuSpecialShortcuts.Contains(input);
                
                

                if (!runDone && !isInputValid)
                {
                    Console.WriteLine("Unknown shortcut: " + input);
                }

            } while (!runDone) ;

            if (input == menuItemReturn.Shortcut)
            {
                return "";
            }
            
            resetColors();
            return input;
        }

        public string Run()
        {
            var input = "";
            var runDone = false;

            do
            {
                var selection = OutputMenuNew();

                var isSelectionMenuItem = selection < _menuItems.Count;

                if (isSelectionMenuItem)
                {
                    var item = _menuItems[selection];
                    input = item.methodToRun == null ? input : item.methodToRun();
                    CorrectColors();
                }
                else
                {
                    var item = _specialItems[selection - _menuItems.Count];
                    input = item.methodToRun == null ? input : item.methodToRun();
                }

                runDone = _menuSpecialShortcuts.Contains(input);

            } while (!runDone) ;

            if (input == menuItemReturn.Shortcut)
            {
                return "";
            }
            
            resetColors();
            return input;
        }

        private int OutputMenuNew()
        {
            Console.CursorVisible = false;
            
            int currentSelection = 0;

            ConsoleKey key;
            
            do
            {
                var menuLength = _menuItems.Count + _specialItems.Count;
                
                Console.Clear();
                
                CorrectColors();
            
                var line = new StringBuilder();

                for (int i = 0; i < 8 + _menuTitle.Length; i++) { line.Append('-');}

                Console.WriteLine(line);
                Console.WriteLine("===>" + _menuTitle + "<===");

                if (ShowMemory != null)
                {
                    Console.WriteLine(ShowMemory());
                }

                Console.WriteLine(line);

                for (int i = 0; i < menuLength; i++)
                {
                    
                    var atm = Console.ForegroundColor;
                    if (i == _menuItems.Count)
                    {
                      Console.WriteLine(line);
                    }

                    if (i == currentSelection)
                        Console.ForegroundColor = ConsoleColor.Red;
                    if (i < _menuItems.Count)
                    {
                        Console.WriteLine(_menuItems[i].ToString());
                    }
                    else
                    {
                        Console.WriteLine(_specialItems[i - _menuItems.Count]);
                    }
                    
                    Console.ForegroundColor = atm;
                }
                
                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.Escape:
                        Console.CursorVisible = true;
                        resetColors();
                        return _menuItems.Count;
                    case ConsoleKey.UpArrow:
                        if (currentSelection >= 1)
                        {
                            currentSelection -= 1;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (currentSelection + 1 < menuLength)
                        {
                            currentSelection += 1;
                        }
                        break;
                    case ConsoleKey.Enter:
                        return currentSelection;
                }

            } while (true) ;
        }

        private void OutputMenu()
        {
            Console.Clear();
            CorrectColors();
            
            var line = new StringBuilder();

            for (int i = 0; i < 8 + _menuTitle.Length; i++) { line.Append('-');}

            Console.WriteLine(line);
            Console.WriteLine("===>" + _menuTitle + "<===");

            if (ShowMemory != null)
            {
                Console.WriteLine(ShowMemory());
            }

            Console.WriteLine(line);

            foreach (var item in _menuItems)
            {
                item.printItem();
            }

            Console.WriteLine(line);

            switch (_menuLevel)
            {
                case EMenuLevel.root:
                    Console.WriteLine(menuItemExit);
                    break;
                case EMenuLevel.primary:
                    Console.WriteLine(menuItemReturn);
                    Console.WriteLine(menuItemExit);
                    break;
                case EMenuLevel.special:
                case EMenuLevel.secondary:
                    Console.WriteLine(menuItemMain);
                    Console.WriteLine(menuItemReturn);
                    Console.WriteLine(menuItemExit);
                    break;
            }
        }

        private void CorrectColors()
        {
            Console.BackgroundColor = Background;
            Console.ForegroundColor = TextColor;
        }

        private void resetColors()
        {
            Console.ResetColor();
        }
    }
}