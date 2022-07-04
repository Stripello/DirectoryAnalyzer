using DirectoryAnalyzer;
using DirectoryAnalyzer.Models;

namespace DirectoryOperationServices
{

    public static class DirectoryOperation
    {
        internal static IList<MyFileInfo> GetAllFiles(IEnumerable<MyFileSystemNode> incomingFS)
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
            const int maxSampleSize = 10; //according to task
            return incomingFiles.OrderBy(x => x.Size).Reverse().Take(maxSampleSize).ToList();
        }

        public static IList<MyFileInfo> GetOldestFiles(IList<MyFileInfo> incomingFiles)
        {
            const int maxSampleSize = 10; //according to task
            return incomingFiles.OrderBy(x => x.Changedate).Take(maxSampleSize).ToArray();
        }

        public static string[,] GetFrequentExtension(IEnumerable<MyFileInfo> incomingFiles)
        {
            const int sampleSize = 10;
            var auxList = (from file in incomingFiles
                           group file by file.Extension ?? "" into g
                           let amount = g.Count()
                           orderby amount descending
                           select new { Extension = g.Key, Amount = amount })
                          .Take(sampleSize).ToList();
            var answer = new string[auxList.Count() + 1, 2];
            answer[0, 0] = "Extension";
            answer[0, 1] = "Amount of files";
            var i = 1;
            foreach (var el in auxList)
            {
                answer[i, 0] = el.Extension.ToString() ?? "";
                answer[i, 1] = el.Amount.ToString();
                i++;
            }
            return answer;
        }
        //bad returning type
        public static string[,] GetBiggestExtensions(IList<MyFileInfo> myFileInfos)
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
        //bad returning type
        public static string[,] GetBiggestDirectories(IEnumerable<MyFileSystemNode> allNodes)
        {
            var answer = new List<(string, long)>();

            foreach (var node in allNodes)
            {
                answer.Add(new(node.DirectoryName, node.Content.Sum(x => x.Size)));
            }
            var returnTableSize = allNodes.Count() < 10 ? allNodes.Count() + 1 : 11;
            answer = answer.OrderBy(x => x.Item2).Reverse().ToList();
            var tableToReturn = new string[returnTableSize, 2];
            tableToReturn[0, 0] = "Directory name";
            tableToReturn[0, 1] = "Summ size";
            for (int i = 1; i < returnTableSize; i++)
            {
                tableToReturn[i, 0] = answer[i-1].Item1;
                tableToReturn[i, 1] = answer[i-1].Item2.ToString();
            }
            return tableToReturn;
        }
        //need to take into account difference in counting checksum for different extensions
        public static IList<IList<MyFileInfo>> GetCopies(IList<MyFileInfo> incomingFiles, long minimalSize = 67108864)
        {
            incomingFiles = incomingFiles.Where(x => x.Size > minimalSize).OrderBy(x => x.Size).ToList();
            //#linq
            var preliminaryAnswer = new List<IList<MyFileInfo>>();
            for (int i = 0; i < incomingFiles.Count() - 1;)
            {
                if (incomingFiles[i].Size == incomingFiles[i + 1].Size)
                {
                    preliminaryAnswer.Add(new List<MyFileInfo> { incomingFiles[i], incomingFiles[i + 1] });
                    i++;
                    while (i < incomingFiles.Count() - 1 && incomingFiles[i].Size == incomingFiles[i + 1].Size)
                    {
                        preliminaryAnswer.Last().Add(incomingFiles[i + 1]);
                        i++;
                    }
                }
                else
                {
                    i++;
                }
            }
            var length = preliminaryAnswer.Count;
            var answer = new List<IList<MyFileInfo>>();
            for (int i = 0; i < length; i++)
            {
                answer.AddRange(CheckSumGrouping(preliminaryAnswer[i]));
            }
            return answer;
        }

        private static IList<IList<MyFileInfo>> CheckSumGrouping(IList<MyFileInfo> myFileInfos)
        {
            const int sliceSize = 8;
            var auxList = new List<(MyFileInfo, long)>();
            foreach (var element in myFileInfos)
            {
                long iterations = element.Size / sliceSize;
                iterations = element.Size % sliceSize == 0 ? iterations : iterations + 1;
                long checkSum = 0;
                using (FileStream fs2 = File.OpenRead(element.Name))
                {
                    byte[] currentSlice = new byte[sliceSize];
                    for (int j = 0; j < iterations; j++)
                    {
                        fs2.Read(currentSlice, 0, sliceSize);
                        checkSum += BitConverter.ToInt64(currentSlice, 0);
                    }
                }
                auxList.Add((element, checkSum));
            }
            var answer = from element in auxList
                         group element by element.Item2;

            var finalAnswer = new List<IList<MyFileInfo>>() { };
            foreach (var element in answer)
            {
                if (element.Count() > 1)
                {
                    var tempList = new List<MyFileInfo>();
                    foreach (var subel in element)
                    {
                        tempList.Add(subel.Item1);
                    }
                    finalAnswer.Add(tempList);
                }
            }
            return finalAnswer;
        }
    }
}
