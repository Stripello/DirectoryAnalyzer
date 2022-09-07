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

        public static IList<(string,int)> GetFrequentExtension(IEnumerable<MyFileInfo> incomingFiles)
        {
            const int maxAnswerSize = 10;
            var answer = (from file in incomingFiles
                           group file by file.Extension ?? "" into g
                           let amount = g.Count()
                           orderby amount descending
                           select new { Extension = g.Key, Amount = amount })
                          .Take(maxAnswerSize).Select(x => (x.Extension,x.Amount)).ToList();
            
            return answer;
        }
        public static IList<(string,long)> GetBiggestExtensions(IList<MyFileInfo> myFileInfos)
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
            const int maxAnswerLength = 10;
            
            return resultOfAnalys.Take(maxAnswerLength).ToList();
        }
        
        public static IList<(string,long)> GetBiggestDirectories(IEnumerable<MyFileSystemNode> allNodes)
        {
            const int maxAnswerLength = 10;
            var answer = new List<(string, long)>();
            foreach (var node in allNodes)
            {
                answer.Add(new(node.DirectoryName, node.Content.Sum(x => x.Size)));
            }
            var returnTableSize = allNodes.Count() < 10 ? allNodes.Count() + 1 : 11;
            answer = answer.OrderBy(x => x.Item2).Reverse().Take(maxAnswerLength).ToList();
            
            return answer;
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