using System.Collections.Generic;

namespace DirectoryAnalyzer
{
    internal class Logger
    {
        internal static string[]? GetShortLog()
        {
            var path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                "\\Log\\ShortLog.txt";
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

        internal static List<DtoFileInfo>? GetLongLog(string directoryName)
        {
            var path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Log\\LongLog.txt";
            var answer = new List<DtoFileInfo> { };
            if (!File.Exists(path))
            {
                File.Create(path);
                return null;
            }
            else
            {
                var fileContent = File.ReadAllLines(path);
                int maximalLogForSingleFile = 30;
                for (int i=0; i < fileContent.Length; i++)
                {
                    if (fileContent[i] == "$"+directoryName)
                    {
                        for (int j = 1; j < maximalLogForSingleFile; j++)
                        {
                            if (fileContent[i + j] == "$$" + directoryName)
                            {
                                return answer;
                            }
                            else
                            {
                                answer.Add(DtoFileInfo.Parse(fileContent[i + j]));
                            }
                        }
                    }
                }
            }
            return null;
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
