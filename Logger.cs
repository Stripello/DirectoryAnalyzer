using System.IO;

namespace DirectoryAnalyzer
{
    internal class Logger
    {
        internal static string GetCurrentDirectory()
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Log\\log.txt";
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            return path;
        }

        internal static string[] GetCurrentLog()
        {
            var filePath = GetCurrentDirectory();
            return File.ReadAllLines(filePath);
        }

        internal static void UpdateLog(string [] modifier)
        {
            var filePath = GetCurrentDirectory();
            File.WriteAllLines(filePath, modifier);
        }


    }
}
