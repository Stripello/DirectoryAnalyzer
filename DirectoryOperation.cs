using System.Linq;

namespace DirectoryAnalyzer
{
    internal static class DirectoryOperation
    {
        internal static string AskDirectory()
        {
            string directory;
            do
            {
                Console.WriteLine("Please, enter directory.");
                directory = Console.ReadLine();
                if (Directory.Exists(directory))
                {
                    return directory;
                }
                Console.WriteLine("Can't find such directory.");
            }
            while (true);
        }

        internal static List<DTOFileInfo> GetAllFiles(string directory)
        {
            var answer = new List<DTOFileInfo>();
            var allSubdirectories = Directory.GetDirectories(directory);
            
            if (allSubdirectories.Length != 0)
            {
                foreach (var subdirectory in allSubdirectories)
                {
                    try
                    {
                        answer.AddRange(GetAllFiles(subdirectory));
                    }
                    catch
                    {
                    }
                }
            }
            var directoryInfo = new DirectoryInfo(directory);
            FileInfo[] files = directoryInfo.GetFiles();
            foreach (var file in files)
            {
                answer.Add(new DTOFileInfo(file));
            }

            return answer;
        }

        internal static string[,] GetBiggestFiles(List<DTOFileInfo> incomingFiles)
        {
            var maxSampleSize = 10; //according to task
            var answerSize = Math.Min(incomingFiles.Count, maxSampleSize);

            var tenBiggestFiles = Enumerable.Repeat(new DTOFileInfo(),answerSize).ToArray();
            long smallestSizeInSample = 0;
            int smallestItemID = 0;
            foreach (var file in incomingFiles)
            {
                var currentFileSize = file.size;
                if (currentFileSize > smallestSizeInSample)
                {
                    tenBiggestFiles[smallestItemID] = file;
                    smallestSizeInSample = tenBiggestFiles.Min(x => x.size);
                    var smallestItem = tenBiggestFiles.Last(x => x.size == smallestSizeInSample);
                    smallestItemID = Array.LastIndexOf(tenBiggestFiles, smallestItem);
                }
            }

            tenBiggestFiles = tenBiggestFiles.OrderBy(x => x.size).ToArray();
            Array.Reverse(tenBiggestFiles);

            var amountOfColumns = 2;
            var answer = new string[answerSize +1, amountOfColumns];
            answer[0, 0] = "file name"; //header of output table
            answer[0, 1] = "file size";
            for (int i = 0; i < answerSize; i++)
            {
                answer[i+1, 0] = Path.GetFileNameWithoutExtension(tenBiggestFiles[i].name);
                var currentItemSize = tenBiggestFiles[i].size;

                var teraByte = 1099511627776;
                if (currentItemSize > teraByte)
                {
                    answer[i + 1, 1] = (currentItemSize/teraByte).ToString() + " Tb"; // (1+...)shift for headder
                }
                var gigaByte = 1073741824;
                if (currentItemSize > gigaByte)
                {
                    answer[i + 1, 1] = (currentItemSize / gigaByte).ToString() + " Gb";
                }
                var megaByte = 1048576;
                if (currentItemSize > megaByte)
                {
                    answer[i + 1, 1] = (currentItemSize / megaByte).ToString() + " Mb";
                }
                var kiloByte = 1024;
                if (currentItemSize > kiloByte)
                {
                    answer[i + 1, 1] = (currentItemSize / kiloByte).ToString() + " Kb";
                }
                else
                {
                    answer[i + 1, 1] = (currentItemSize).ToString() + " bytes";
                }
            }
            return answer;
        }

        internal static string[,] GetOldestFiles(List<DTOFileInfo> incomingFiles)
        {
            var maxSampleSize = 10; //according to task
            var answerSize = Math.Min(incomingFiles.Count, maxSampleSize);

            var tenOldestFiles = Enumerable.Repeat(new DTOFileInfo(), answerSize).ToArray();
            var oldestFileDate = DateTime.MaxValue;
            int oldestItemId = 0;
            foreach (var file in incomingFiles)
            {
                var currentFileChangeDate = file.changedate;
                if (DateTime.Compare(oldestFileDate , currentFileChangeDate) > 0)
                {
                    tenOldestFiles[oldestItemId] = file;
                    smallestSizeInSample = tenOldestFiles.Min(x => x.size);
                    var smallestItem = tenOldestFiles.Last(x => x.size == smallestSizeInSample);
                    oldestItemId = Array.LastIndexOf(tenOldestFiles, smallestItem);
                }
            }

            tenOldestFiles = tenOldestFiles.OrderBy(x => x.size).ToArray();
            Array.Reverse(tenOldestFiles);

            var amountOfColumns = 2;
            var answer = new string[answerSize + 1, amountOfColumns];
            answer[0, 0] = "file name"; //header of output table
            answer[0, 1] = "file size";
            for (int i = 0; i < answerSize; i++)
            {
                answer[i + 1, 0] = Path.GetFileNameWithoutExtension(tenOldestFiles[i].name);
                var currentItemSize = tenOldestFiles[i].size;

                var teraByte = 1099511627776;
                if (currentItemSize > teraByte)
                {
                    answer[i + 1, 1] = (currentItemSize / teraByte).ToString() + " Tb"; // (1+...)shift for headder
                }
                var gigaByte = 1073741824;
                if (currentItemSize > gigaByte)
                {
                    answer[i + 1, 1] = (currentItemSize / gigaByte).ToString() + " Gb";
                }
                var megaByte = 1048576;
                if (currentItemSize > megaByte)
                {
                    answer[i + 1, 1] = (currentItemSize / megaByte).ToString() + " Mb";
                }
                var kiloByte = 1024;
                if (currentItemSize > kiloByte)
                {
                    answer[i + 1, 1] = (currentItemSize / kiloByte).ToString() + " Kb";
                }
                else
                {
                    answer[i + 1, 1] = (currentItemSize).ToString() + " bytes";
                }
            }
            return answer;
        }

        internal static string[,] GetFrequentExtension(string directory)
        {
            string[] listOfFiles = Directory.GetFiles(directory);
            
            var listOfExtensions = new List<string>();
            var listOfAmountOfExtensions = new List<int>();

            foreach (var file in listOfFiles)
            {
                string currentFileExtension = Path.GetExtension(file);
                var searchedElement = listOfExtensions.IndexOf(currentFileExtension);
                
                if (searchedElement == -1)
                {
                    listOfExtensions.Add(currentFileExtension);
                    listOfAmountOfExtensions.Add(1);
                }
                else
                {
                    listOfAmountOfExtensions[searchedElement]++;
                }
            }

            int amountOfExtensions = listOfExtensions.Count;
            bool gotRelocateThisTurn;
            do
            {
                gotRelocateThisTurn = false;
                for (int i = 0; i < amountOfExtensions - 1; i++)
                {
                    if (listOfAmountOfExtensions[i] < listOfAmountOfExtensions[i+1])
                    {
                        (listOfAmountOfExtensions[i + 1], listOfAmountOfExtensions[i]) = (listOfAmountOfExtensions[i], listOfAmountOfExtensions[i + 1]);
                        (listOfExtensions[i + 1], listOfExtensions[i]) = (listOfExtensions[i], listOfExtensions[i + 1]);
                        gotRelocateThisTurn = true;
                    }
                }
            }
            while (gotRelocateThisTurn == true);

            var answer = new string[11,2];
            answer[0, 0] = "extension";
            answer[0, 1] = "appearance number";
            for (int i = 0; i < amountOfExtensions && i < 10; i++)
            {
                answer[i + 1, 0] = listOfExtensions[i];
                answer[i + 1, 1] = listOfAmountOfExtensions[i].ToString();
            }
            return answer;
        }

        
    }
}
