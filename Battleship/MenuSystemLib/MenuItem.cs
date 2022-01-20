using System;

namespace MenuLibrary
{
    public class MenuItem
    {
        public string Title { get; set; }
        public string Shortcut { get; set; }

        public ConsoleColor[] TitleColors = new[]
        {
            Console.BackgroundColor,
            Console.ForegroundColor
        };
        
        public ConsoleColor[] ShortcutColors = new[]
        {
            Console.BackgroundColor,
            Console.ForegroundColor
        };

        private bool colorChanged = false;

        public Func<string> methodToRun;

        public MenuItem(string shortcut, string title, Func<string> toRun)
        {
            methodToRun = toRun;
            
            if (string.IsNullOrEmpty(shortcut))
            {
                throw new ArgumentException("shortcut cannot be empty");
            }

            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("title cannot be empty");
            }

            Shortcut = shortcut.Trim().ToUpper();
            Title = title.Trim();
        }

        public void SetTitleColoring(ConsoleColor background, ConsoleColor textColor)
        {
            TitleColors[0] = background;
            TitleColors[1] = textColor;
        }

        public void SetShortCutColoring(ConsoleColor background, ConsoleColor textColor)
        {
            ShortcutColors[0] = background;
            ShortcutColors[1] = textColor;
            
        }

        public override string ToString()
        {
            return Shortcut + ") " + Title;
        }

        public void printItem()
        {
            if (colorChanged)
            {
                Console.BackgroundColor = ShortcutColors[0];
                Console.ForegroundColor = ShortcutColors[1];
                Console.Write(Shortcut + ")");
                Console.BackgroundColor = TitleColors[0];
                Console.ForegroundColor = TitleColors[1];
                Console.WriteLine(Title);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine(ToString());
            }
        }
    }
}