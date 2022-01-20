using System;

namespace MenuLibrary
{
    public class MenuItem
    {
        public string Title { get; set; }
        public string Shortcut { get; set; }

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

        public override string ToString()
        {
            return Shortcut + ") " + Title;
        }
    }
}