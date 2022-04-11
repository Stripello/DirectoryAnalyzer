namespace DirectoryAnalyzer
{
    internal class Logger
    {
        internal static string GetCurrentDirectory()
        {
            var fi = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Log\\ShortLog.txt";
            var an = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Log\\LongLog.txt";
            return fi;
        }

        internal static string[]? GetSearchLog()
        {
            var path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Log\\ShortLog.txt";
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

        internal static void AddToShortLog(string searchedDirectory)
        {
            var shortLog = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Log\\ShortLog.txt";
            if (!File.Exists(shortLog))
            {
                File.Create(shortLog);
            }
            File.AppendAllText(shortLog, searchedDirectory);
        }
        internal static void AddToLongLog(string searchedResult)
        {
            var shortLog = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Log\\LongLog.txt";
            if (!File.Exists(shortLog))
            {
                File.Create(shortLog);
            }
            File.AppendAllText(shortLog, searchedResult);
        }
    }
}
