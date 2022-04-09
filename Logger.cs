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

        internal static void ModifySearchLog(string searchedDirectory, )
        {
            var pathSearchLog = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Log\\SearchLog.txt";
            File.WriteAllTextAsync(pathSearchLog, searchedDirectory);

            var 
        }
    }
}
