namespace DirectoryAnalyzer
{
    internal class Logger
    {
        internal static string GetCurrentDirectory()
        {
            var fi = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Log\\fileLog.txt";
            var an = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Log\\answerLog.txt";
            return fi;
        }

        internal static void ModifyLog(string path)
        {
            var pathy = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Log\\fileLog.txt";
            File.AppendAllTextAsync(pathy, "a1");
        }
    }
}
