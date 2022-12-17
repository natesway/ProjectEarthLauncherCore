using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemPlus.Extensions;

namespace ProjectEarthLauncherCore
{
    public static class Util
    {
        public static string[] Split(this string s, string separator, StringSplitOptions options = StringSplitOptions.None)
            => s.Split(new string[] { separator }, options);

        public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            DirectoryInfo dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles()) {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive) {
                foreach (DirectoryInfo subDir in dirs) {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        public static void HandleBack()
        {
            ConsoleExtensions.WriteLine("Back", true);
            getKey:
            if (Console.ReadKey(true).Key != ConsoleKey.Enter)
                goto getKey;
            Console.Clear();
        }

        public static DialogResult FolderDialog(string title, out string path)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[Input_Folder] ");
            Console.ResetColor();
            title = title.Replace("Select", "Input");
            Console.Write(title + " (Empty to Cancel): ");
            path = Console.ReadLine();

            if (path == string.Empty)
                return DialogResult.Cancel;
            else if (Directory.Exists(path)) {
                char lastChar = path[path.Length - 1];
                if (lastChar == '/' || lastChar == '\\')
                    path = path.Substring(0, path.Length - 1);
                return DialogResult.OK;
            }
            else
                return DialogResult.DoesntExist;
        }
    }
}
