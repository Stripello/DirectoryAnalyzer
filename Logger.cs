using System.Collections.Generic;

namespace DirectoryAnalyzer
{
    internal class Logger
    {
        internal Dictionary<long, FileSystemNode> knownFileSystem = new Dictionary<long, FileSystemNode> { };
        internal static string[]? GetLog()
        {
            var path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                "\\Log\\Log.txt";
            if (!File.Exists(path))
            {
                File.Create(path);
                return null;
            }
            else
            {
                return File.ReadAllLines(path);
            }
        }
    }
}
