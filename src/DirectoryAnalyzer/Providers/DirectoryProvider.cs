namespace DirectoryAnalyzer.Providers;

public class DirectoryProvider
{
    /// <summary>
    /// Asks user for directory to operate.
    /// </summary>
    /// <returns>Full name of picked directory. Null if user wants to quit.</returns>
    internal static string? AskDirectory()
    {
        string directory;
        while (true)
        {
            Console.WriteLine("Please, enter directory or type \"Exit\" to terminate process.");
            directory = Console.ReadLine();
            if (directory.ToLower() == "exit")
            {
                return null;
            }
            if (Directory.Exists(directory))
            {
                return directory;
            }
            Console.WriteLine("Can't find such directory.");
        }
    }

    internal static List<string> GetAllDirectories(string directory)
    {
        var answer = new List<string>();
        var dirStack = new Stack<string>();
        dirStack.Push(directory);
        var tempDir = string.Empty;
        do
        {
            if (!dirStack.TryPop(out tempDir))
            {
                break;
            }
            answer.Add(tempDir);
            try
            {
                foreach (var element in Directory.GetDirectories(tempDir, "*", SearchOption.TopDirectoryOnly))
                {
                    dirStack.Push(element);
                }
            }
            catch(UnauthorizedAccessException)
            {
            }
        }
        while (true);
        return answer;
    }
}