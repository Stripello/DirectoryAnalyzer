using DirectoryAnalyzer;
using DirectoryAnalyzer.Models;

namespace DirectoryOperationServices
{
    
    public static class DirectoryOperation
    {
        internal static IList<MyFileInfo> GetAllFiles(IList<MyFileSystemNode> incomingFS)
        {
            var answer = new List<MyFileInfo>();
            foreach (var element in incomingFS)
            {
                answer.AddRange(element.Content);
            }
            return answer;
        }

        public static IList<MyFileInfo> GetBiggestFiles(IList<MyFileInfo> incomingFiles)
        {
            var maxSampleSize = 10; //according to task
            var answerSize = Math.Min(incomingFiles.Count, maxSampleSize);
            return incomingFiles.OrderBy(x => x.Size).Reverse().Take(answerSize).ToList();
        }

        internal static IList<MyFileInfo> GetOldestFiles(IList<MyFileInfo> incomingFiles)
        {
            var maxSampleSize = 10; //according to task
            var answerSize = Math.Min(incomingFiles.Count, maxSampleSize);
            return incomingFiles.OrderBy(x => x.Changedate).Take(answerSize).ToArray();
        }

        //need to be completely rebuild for purpose of unit tests
        internal static string[,] GetFrequentExtension(IList<MyFileInfo> incomingFiles)
        {
            var listOfExtensions = new List<string>();
            var listOfAmountOfExtensions = new List<int>();

            foreach (var file in incomingFiles)
            {
                string currentFileExtension = file.Extension;
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
                    if (listOfAmountOfExtensions[i] < listOfAmountOfExtensions[i + 1])
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
        //need to be completely rebuild for purpose of unit tests
        internal static string[,] GetBiggestExtensions(IList<MyFileInfo> myFileInfos)
        {
            var resultOfAnalys = new List<(string, long)>();
            foreach (var fi in myFileInfos)
            {
                var id = resultOfAnalys.FindIndex(x => x.Item1 == fi.Extension);
                if (id == -1)
                {
                    resultOfAnalys.Add((fi.Extension, fi.Size));
                }
                else
                {
                    resultOfAnalys[id] = (resultOfAnalys[id].Item1, resultOfAnalys[id].Item2 + fi.Size);
                }
            }
            resultOfAnalys.Sort((x, y) => y.Item2.CompareTo(x.Item2));
            var answerLength = resultOfAnalys.Count() < 10 ? resultOfAnalys.Count() : 10;
            var answer = new string[11, 2];
            answer[0, 0] = "Extension";
            answer[0, 1] = "Summarized size";
            for (int i = 0; i < answerLength; i++)
            {
                answer[i + 1, 0] = resultOfAnalys[i].Item1;
                answer[i + 1, 1] = resultOfAnalys[i].Item2.ToString();
            }
            return answer;
        }
        //need to be completely rebuild for purpose of unit tests
        internal static string[,] GetBiggestDirectories(IEnumerable<MyFileSystemNode> allNodes)
        {
            var answer = new List<(string, long)>();

            foreach (var node in allNodes)
            {
                answer.Add(new(node.DirectoryName, node.Content.Sum(x => x.Size)));
            }
            var returnTableSize = allNodes.Count() < 10 ? allNodes.Count() + 1 : 11;
            answer.Sort((x, y) => x.Item2.CompareTo(y.Item2)); //possible issues
            var tableToReturn = new string[returnTableSize, 2];
            tableToReturn[0, 0] = "directory name";
            tableToReturn[0, 1] = "summ size";
            for (int i = 1; i < returnTableSize; i++)
            {
                tableToReturn[i, 0] = answer[i].Item1;
                tableToReturn[i, 1] = answer[i].Item2.ToString();
            }
            return tableToReturn;
        }
        //possible to make complieance throgh checksum, but there is issues with performance on big files
        //and different checksum for file with same data and different metadata
        public static IList<IList<MyFileInfo>> GetCopies(IList<MyFileInfo> incomingFiles)
        {
            const int minimalSize = 1024;
            incomingFiles = incomingFiles.Where(x => x.Size > minimalSize).OrderBy(x => x.Size).ToList();
            var answer = new List<IList<MyFileInfo>>();
            for (int i = 0; i < incomingFiles.Count()-1;)
            {
                if (incomingFiles[i].Size == incomingFiles[i + 1].Size)
                {
                    answer.Add(new List<MyFileInfo> { incomingFiles[i], incomingFiles[i + 1] });
                    i++;
                    while (i < incomingFiles.Count()-1 && incomingFiles[i].Size == incomingFiles[i+1].Size)
                    {
                        answer.Last().Add(incomingFiles[i + 1]);
                        i++;
                    }
                }
                else
                {
                    i++;
                }
            }
            var amountOfGroupsInAnswer = answer.Count();
            const int sliceSize = 8;
            for (int i = 0; i < amountOfGroupsInAnswer; i++)
            {
                var currentList = answer[0];
                var currentListFilesSize = currentList[0].Size;
                var currentCheckSummArray = new List<long>();
                foreach (var element in currentList)
                {
                    long iterations = currentListFilesSize / sliceSize;
                    iterations = currentListFilesSize%sliceSize ==0 ? iterations : iterations + 1;
                    long checkSum = 0;

                    using (FileStream fs2 =File.OpenRead(element.Name))
                    {
                        byte[] currentSlice = new byte[sliceSize];

                        for (int j = 0; j < iterations; j++)
                        {
                            fs2.Read(currentSlice, 0, sliceSize);
                            checkSum += BitConverter.ToInt64(currentSlice, 0);
                        }
                    }
                    currentCheckSummArray.Add(checkSum);
                }
                if (currentCheckSummArray.All(x => x == currentCheckSummArray[0]))
                {
                    continue;
                }
                else
                {
                    answer.RemoveAt(0);
                    var auxArray = new List<(long, int)>();
                    for (int k = 0; k < currentCheckSummArray.Count(); k++)
                    {
                        auxArray.Add(new (currentCheckSummArray[k],k));
                    }
                    auxArray=auxArray.OrderBy(x=>x.Item1).ToList();
                    var counter = 0;
                    var tempCheckSum = auxArray[0].Item1;
                    var tempList = new List<MyFileInfo>();
                    while (counter <auxArray.Count())
                    {
                        counter++;
                        if (tempCheckSum != auxArray[counter].Item1)
                        {
                            tempCheckSum = auxArray[counter].Item1;
                            if (tempList.Count > 1)
                            {
                                answer.Add(tempList);
                            }
                            tempList.Clear();
                        }
                        else
                        {
                            tempList.Add(currentList[auxArray[counter].Item2]);
                        }
                    }
                }
            }
            

            return answer;
        }
    }
}
