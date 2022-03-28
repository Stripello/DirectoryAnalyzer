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
            var amountOfFiles = incomingFiles.Count();
            var sampleSize = 10; //according to task
            var amountOfColumns = 2;

            long smallestSizeInSample = 0;
            var tenBiggestFiles = new DTOFileInfo[sampleSize];
            foreach (var file in incomingFiles)
            {
                var currentFileSize = file.size;
                if (currentFileSize > smallestSizeInSample)
                {
                    smallestSizeInSample = Math.Min(currentFileSize, tenBiggestFiles[8].size);
                    tenBiggestFiles[9] = file;

                    for (int i = 9; i > 1; i--)
                    {
                        if (tenBiggestFiles[i] < tenBiggestFiles[i-1])
                        {
                            new DTOF
                        }
                    }
                }



            }

            var answer = new string[sampleSize+1, amountOfColumns];
            answer[0, 0] = "file name"; //header of output table
            answer[0, 1] = "file size";
            for (int i = 0; i < amountOfFiles && i < 10; i++)
            {
                answer[i+1, 0] = Path.GetFileNameWithoutExtension(listOfFilesAndSizes[i].Item1); 
                answer[i+1, 1] = listOfFilesAndSizes[i].Item2.ToString(); // (1+...)shift for headder
            }
            
            return answer;
        }

        internal static string[,] GetOldestFiles(string directory)
        {
            string[] listOfFiles = Directory.GetFiles(directory);
            int amountOfFiles = listOfFiles.Length;
            var listOfFilesAndDates = new (string, DateTime)[amountOfFiles];
            for (int i = 0; i < amountOfFiles; i++)
            {
                listOfFilesAndDates[i] = (listOfFiles[i], new FileInfo(listOfFiles[i]).LastWriteTime);
            }

            bool gotRelocateThisTurn;
            do
            {
                gotRelocateThisTurn = false;
                for (int i = 0; i < amountOfFiles - 1; i++)
                {
                    if (DateTime.Compare(listOfFilesAndDates[i].Item2 , listOfFilesAndDates[i+1].Item2) >0)
                    {
                        (string, DateTime) tmp = listOfFilesAndDates[i];
                        listOfFilesAndDates[i] = listOfFilesAndDates[i + 1];
                        listOfFilesAndDates[i + 1] = tmp;
                        gotRelocateThisTurn = true;
                    }
                }
            }
            while (gotRelocateThisTurn == true);

            var answer = new string[11, 2];
            answer[0, 0] = "file name";
            answer[0, 1] = "latest change time";
            for (int i = 0; i < amountOfFiles && i < 10; i++)
            {
                answer[i + 1, 0] = Path.GetFileNameWithoutExtension(listOfFilesAndDates[i].Item1);
                answer[i + 1, 1] = listOfFilesAndDates[i].Item2.ToString();
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
                        var tempInt = listOfAmountOfExtensions[i];
                        listOfAmountOfExtensions[i] = listOfAmountOfExtensions[i + 1];
                        listOfAmountOfExtensions[i+1] = tempInt;

                        var tempString = listOfExtensions[i];
                        listOfExtensions[i] = listOfExtensions[i + 1];
                        listOfExtensions[i + 1] = tempString;

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
