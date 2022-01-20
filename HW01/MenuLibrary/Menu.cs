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
        private Func<string> ShowMemory;

        public ConsoleColor Background;
        public ConsoleColor TextColor;

        public Menu( EMenuLevel menuLevel, string title)
        {
            _menuLevel = menuLevel;
            _menuTitle = title;

            Background = Console.BackgroundColor;
            TextColor = Console.ForegroundColor;

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
                    _specialItems.Add(menuItemMain);
                    break;
                case EMenuLevel.srcondary:
                    _menuSpecialShortcuts.Add(menuItemExit.Shortcut);
                    _specialItems.Add(menuItemExit);
                    _menuSpecialShortcuts.Add(menuItemReturn.Shortcut);
                    _specialItems.Add(menuItemReturn);
                    _menuSpecialShortcuts.Add(menuItemMain.Shortcut);
                    _specialItems.Add(menuItemMain);
                    break;
            }
        }

        public Menu(EMenuLevel menuLevel, string title, Func<string> showMemory)
        {
            DisplayMemory = true;
            ShowMemory = showMemory;
            
            _menuLevel = menuLevel;
            _menuTitle = title;

            Background = Console.BackgroundColor;
            TextColor = Console.ForegroundColor;

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
                    _specialItems.Add(menuItemMain);
                    break;
                case EMenuLevel.srcondary:
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

        public string Run()
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
                    // do sth
                    /*for (int i = 0; i < _menuItems.Count; i++)
                    {
                        if (_menuItems[i].Shortcut == input)
                        {
                            result = _menuItems[i].methodToRun();
                        }
                    }*/
                    var item = _menuItems.FirstOrDefault(t => t.Shortcut == input);
                    input = item?.methodToRun == null ? input : item.methodToRun();
                    CorrectColors();
                }
                
                /*
                 else
                {
                    isInputValid = _menuSpecialShortcuts.Contains(input);

                    if (isInputValid)
                    {
                        var toRun = _specialItems.Find((item => item.Shortcut == input));
                        result = toRun.methodToRun();
                    }
                }
                
                if (result == "e")
                {
                    runDone = true;
                }
                */

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

        private void OutputMenu()
        {
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
                Console.WriteLine(item);
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
                case EMenuLevel.srcondary:
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