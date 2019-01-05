using System;
using System.Collections.Generic;
using System.Text;

namespace Nozzibot.Helpers
{
    class CConsole
    {
        public static void WriteLine(string text, ConsoleColor color)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = originalColor;
        }
    }
}
