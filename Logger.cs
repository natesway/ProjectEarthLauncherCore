using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEarthLauncherCore
{
    public static class Logger
    {
        private static int lastLineWrite = 0;

        public static bool YNInput(string message)
        {
            lastLineWrite = Console.CursorTop;

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[Input_YN] ");
            Console.ResetColor();
            Console.Write(message + " (Y/N): ");
            char typed = Console.ReadKey().KeyChar;
            Console.WriteLine();
            return typed == 'Y' || typed == 'y';
        }

        public static string Input(string message)
        {
            lastLineWrite = Console.CursorTop;

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[Input] ");
            Console.ResetColor();
            Console.Write(message + ": ");
            return Console.ReadLine();
        }

        public static bool YNInputWarning(string message)
        {
            lastLineWrite = Console.CursorTop;

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[Input_YN] ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(message);
            Console.ResetColor();
            Console.Write(" (Y/N): ");
            char typed = Console.ReadKey().KeyChar;
            Console.WriteLine();
            return typed == 'Y' || typed == 'y';
        }

        public static string InputWarning(string message)
        {
            lastLineWrite = Console.CursorTop;

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[Input] ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(message);
            Console.ResetColor();
            Console.Write(": ");
            return Console.ReadLine();
        }

        public static void DebugOverwrite(string message)
        {
            Console.SetCursorPosition(0, lastLineWrite);

            string s = $"[Debug] {message}";

            Console.WriteLine(s + new string(' ', Console.WindowWidth - s.Length - 1));
        }

        public static void LogOverwrite(string message)
        {
            Console.SetCursorPosition(0, lastLineWrite);

            string s = $"[Log] {message}";

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[Log] ");
            Console.ResetColor();
            Console.WriteLine(message + new string(' ', Console.WindowWidth - s.Length - 1));
        }

        public static void Debug(string message)
        {
            lastLineWrite = Console.CursorTop;

            Console.WriteLine($"[Debug] {message}");
        }

        public static void Log(string message)
        {
            lastLineWrite = Console.CursorTop;

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[Log] ");
            Console.ResetColor();
            Console.WriteLine(message);
        }

        public static void Exception(Exception e, bool exit = true)
        {
            lastLineWrite = Console.CursorTop;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[Exception] ");
            Console.ResetColor();
            Console.WriteLine(e.Message);
            Console.WriteLine($"Type:\n{e.GetType()}");
            Console.WriteLine($"Stack trace:\n{e.StackTrace}");
            if (exit) {
                PAKX(string.Empty);
                Environment.Exit(2);
            }
        }

        public static void FatalError(string message, bool exit = true)
        {
            lastLineWrite = Console.CursorTop;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[Error] ");
            Console.ResetColor();
            Console.WriteLine(message);
            if (exit) {
                PAKX(string.Empty);
                Environment.Exit(2);
            }
        }

        public static void Warning(string message)
        {
            lastLineWrite = Console.CursorTop;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[Warning] ");
            Console.ResetColor();
            Console.WriteLine(message);
        }

        public static void PAKX(string message) // press any key exit
            => PAK(message, "exit");

        public static void PAKC(string message) // press any key continue
            => PAK(message, "continue");

        public static void PAK(string mess1, string mess2)
        {
            if (mess1 == string.Empty)
                Log($"Press any key to {mess2}...");
            else
                Log($"{mess1}, press any key to {mess2}...");

            Console.ReadKey(true);
        }
    }
}
