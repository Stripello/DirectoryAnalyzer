namespace DirectoryAnalyzer
{
    internal static class DirectoryOperation
    {
        internal static string? AskDirectory()
        {
            string directory;
            do
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
            while (true);
        }

        internal static List<DtoFileInfo> GetAllFiles(string directory)
        {
            var answer = new List<DtoFileInfo>();
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
                answer.Add(new DtoFileInfo(file));
            }

            return answer;
        }

        internal static string[,] GetBiggestFiles(List<DtoFileInfo> incomingFiles)
        {
            var maxSampleSize = 10; //according to task
            var answerSize = Math.Min(incomingFiles.Count, maxSampleSize);

            var tenBiggestFiles = Enumerable.Repeat(new DtoFileInfo(),answerSize).ToArray();
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
                answer[i+1, 0] = tenBiggestFiles[i].name;
                var currentItemSize = tenBiggestFiles[i].size;

                switch (currentItemSize)
                {
                    case > 1099511627776: //terabyte size 
                        answer[i + 1, 1] = (currentItemSize / 1099511627776).ToString() + " Tb";
                        break;
                    case > 1073741824: //gigabyte size 
                        answer[i + 1, 1] = (currentItemSize / 1073741824).ToString() + " Gb";
                        break;
                    case > 1048576: //megabyte size 
                        answer[i + 1, 1] = (currentItemSize / 1048576).ToString() + " Kb";
                        break;
                    case > 1024: //kilobyte size 
                        answer[i + 1, 1] = (currentItemSize / 1048576).ToString() + " Kb";
                        break;

                    default:
                        answer[i + 1, 1] = (currentItemSize).ToString() + " bytes";// (1+...)shift for headder
                        break;

                }
            }
            return answer;
        }

        internal static string[,] GetOldestFiles(List<DtoFileInfo> incomingFiles)
        {
            var maxSampleSize = 10; //according to task
            var answerSize = Math.Min(incomingFiles.Count, maxSampleSize);

            var tenOldestFiles = Enumerable.Repeat(new DtoFileInfo(), answerSize).ToArray();
            var newestFileDate = DateTime.MaxValue;
            int newestFileId = 0;
            foreach (var file in incomingFiles)
            {
                var currentFileChangeDate = file.changedate;
                if (DateTime.Compare(currentFileChangeDate, newestFileDate) < 0)
                {
                    tenOldestFiles[newestFileId] = file;
                    newestFileDate = tenOldestFiles.Max(x => x.changedate);
                    var smallestItem = tenOldestFiles.Last(x => x.changedate == newestFileDate);
                    newestFileId = Array.LastIndexOf(tenOldestFiles, smallestItem);
                }
            }

            tenOldestFiles = tenOldestFiles.OrderBy(x => x.changedate).ToArray();

            var amountOfColumns = 2;
            var answer = new string[answerSize + 1, amountOfColumns];
            answer[0, 0] = "file name"; //header of output table
            answer[0, 1] = "file changedate";
            for (int i = 0; i < answerSize; i++)
            {
                answer[i + 1, 0] = Path.GetFileNameWithoutExtension(tenOldestFiles[i].name);
                answer[i + 1, 1] = tenOldestFiles[i].changedate.ToString();
            }
            return answer;
        }

        internal static string[,] GetFrequentExtension(List<DtoFileInfo> incomingFiles)
        {
            var listOfExtensions = new List<string>();
            var listOfAmountOfExtensions = new List<int>();

            foreach (var file in incomingFiles)
            {
                string currentFileExtension = file.extension;
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
                        (listOfAmountOfExtensions[i + 1], listOfAmountOfExtensions[i]) =
                            (listOfAmountOfExtensions[i], listOfAmountOfExtensions[i + 1]);

                        (listOfExtensions[i + 1], listOfExtensions[i]) = 
                            (listOfExtensions[i], listOfExtensions[i + 1]);

                        gotRelocateThisTurn = true;
                    }
                }
            }
            while (gotRelocateThisTurn == true);

            var maxSampleSize = 10;
            var answerSize = Math.Min(maxSampleSize, amountOfExtensions);
            var answer = new string[answerSize + 1, 2]; // ( + 1 to inseart headder of the table)
            answer[0, 0] = "extension";
            answer[0, 1] = "number of appearances";
            for (int i = 0; i < answerSize; i++)
            {
                answer[i + 1, 0] = listOfExtensions[i];
                answer[i + 1, 1] = listOfAmountOfExtensions[i].ToString();
            }
            return answer;
        }
    }
}
