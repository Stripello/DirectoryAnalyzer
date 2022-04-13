using System.Collections.Generic;

namespace DirectoryAnalyzer
{
    internal class Logger
    {
        internal static bool CheckMetadata(string directory) //check ability of log to provide data for definite directory
        {
            var metaDataFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                "\\Log\\Meta.txt";
            if (!File.Exists(metaDataFile))
            {
                File.Create(metaDataFile);
                return false;
            }
            var metaData = File.ReadAllLines(metaDataFile);
            while (true)
            {
                if (Array.IndexOf(metaData, directory) != -1)
                {
                    return true;
                }
                if (directory != Directory.GetDirectoryRoot(directory))
                {
                    directory = Directory.GetDirectoryRoot(directory);
                }
                else
                {
                    return false;
                }
            }
        }

        internal static void UpdateLog(string directory, FileSystem incomingFileSystem)
        {
            var metaDataFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                "\\Log\\Meta.txt";
            var logDataFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                "\\Log\\Log.txt";


        }

    }
}
