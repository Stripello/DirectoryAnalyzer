using DirectoryAnalyzer.Models;

namespace DirectoryAnalyzer.Dal
{
    public class MyFileSystemNodeDaoSelfWrittenDb : IMyFileSystemNodeDao
    {
        readonly string dataBaseLocation;
        public MyFileSystemNodeDaoSelfWrittenDb(string databaseDirectory , string dataBaseName = "MySelfWrittenDb")
        {
            databaseDirectory += "\\" + dataBaseName + ".txt";
            if (!File.Exists(databaseDirectory))
            {
                File.Create(databaseDirectory).Close();
            }
            dataBaseLocation = databaseDirectory;
        }

        public void Add(IList<MyFileSystemNode> nodes)
        {
            var maxId = GetMaxId()+1;
            using (StreamWriter myStreamWriter = File.AppendText(dataBaseLocation))
            {
                foreach (var element in nodes)
                {
                    //bootleg last id incrimentation
                    if (element.Id == 0)
                    {
                        element.Id = maxId;
                        maxId++;
                    }
                    myStreamWriter.WriteLine(element.ToString());
                }
                
            }
        }

        public IList<MyFileSystemNode> Read(IList<string> directoriesToSearch)
        {
            var remainDirectoriesToSearch = new List<string>(directoriesToSearch);
            var allLines = File.ReadLines(dataBaseLocation).ToArray();
            var answer = new List<MyFileSystemNode>();
            var length = allLines.Count();
            for (int i = 0; i < length; i++)
            {
                if (!allLines[i].StartsWith('$'))
                {
                    continue;
                }
                i++;
                var tempId = remainDirectoriesToSearch.IndexOf(allLines[i][1..]);
                if (tempId == -1)
                {
                    continue;
                }
                remainDirectoriesToSearch.RemoveAt(tempId);
                var nodeId = int.Parse(allLines[i - 1][1..]);
                var directoryName = allLines[i][1..];
                var childrenDirectories = new List<string>();
                i++;
                while (i < length && allLines[i].StartsWith('?'))
                {
                    childrenDirectories.Add(allLines[i][1..]);
                    i++;
                }
                var content = new List<MyFileInfo>();
                while (i < length && allLines[i].StartsWith('*'))
                {
                    content.Add(MyFileInfo.Parse(allLines[i]));
                    i++;
                }
                i--;
                answer.Add(new MyFileSystemNode()
                {
                    Id = nodeId,
                    DirectoryName = directoryName,
                    ChildrenDirectories = childrenDirectories,
                    Content = content
                });
            }
            return answer;
        }

        public void UpdateDb(IList<MyFileSystemNode> nodesToUpdate)
        {
            var directoryNames = nodesToUpdate.Select(x => x.DirectoryName).ToList();
            var listOfIdToUpdate = GetDirectoriesId(directoryNames);
            //could be optimize, double open and scan of file not neccesary
            var maxId = GetMaxId();
            //protect id of elements from changing if element was presented in DB
            for (int i = 0; i < nodesToUpdate.Count; i ++)
            {
                if (listOfIdToUpdate[i] == -1)
                {
                    nodesToUpdate[i].Id = maxId;
                    maxId++;
                }
                else
                {
                    nodesToUpdate[i].Id = listOfIdToUpdate[i];
                }
            }
            listOfIdToUpdate = listOfIdToUpdate.Where(x => x != -1).ToList();
            var amountOfIdToUpdate = listOfIdToUpdate.Count();
            var fileContent = File.ReadAllLines(dataBaseLocation).ToList();
            var rowsInFile = fileContent.Count;
            for (int i = 0; i < rowsInFile && amountOfIdToUpdate>0; i++)
            {
                if (fileContent[i].StartsWith('$'))
                {
                    var tempId = int.Parse(fileContent[i][1..]);
                    var tempIndex = listOfIdToUpdate.IndexOf(tempId);
                    if (tempIndex!=-1)
                    {
                        listOfIdToUpdate.RemoveAt(tempIndex);
                        amountOfIdToUpdate--;
                        fileContent.RemoveAt(i);
                        rowsInFile--;
                        while (i < rowsInFile && !fileContent[i].StartsWith('$'))
                        {
                            fileContent.RemoveAt(i);
                            rowsInFile--;
                        }
                        i--;
                    }
                }
            }
            var mySb = new StreamWriter(dataBaseLocation);
            mySb.Write(String.Join("\n", fileContent));
            mySb.Write("\n");
            foreach (var element in nodesToUpdate)
            {
                mySb.Write(String.Join("\n", element.ToString())); //possible issues
            }
            mySb.Close();
        }

        /// <summary>
        /// Returns current maximal id of all elements in DB. If DB empty returns -1;
        /// </summary>
        /// <returns></returns>
        private int GetMaxId()
        {
            var answer = -1;
            var content = File.ReadAllLines(dataBaseLocation);
            //using array instead of list because numbers of lines in db could be really big
            for (int i = content.Length - 1; i >= 0; i--)
            {
                if (content[i].StartsWith('$'))
                {
                    var parseResult = int.Parse(content[i][1..]);
                    answer = answer > parseResult ? answer : parseResult;
                }
            }
            return answer;
        }

        private IList<int> GetDirectoriesId(IList<string> setOfDirectories)
        {
            var answer = Enumerable.Repeat(-1,setOfDirectories.Count).ToArray();
            var fileContent = File.ReadAllLines(dataBaseLocation);
            var tempSetOfDirectories = new List<string>(setOfDirectories);
            for (int i = 1; i < fileContent.Length && tempSetOfDirectories.Count !=0; i++)
            {
                if (!fileContent[i].StartsWith('>'))
                {
                    continue;
                }
                if (tempSetOfDirectories.Contains(fileContent[i][1..]))
                {
                    tempSetOfDirectories.Remove(fileContent[i][1..]);
                    answer[setOfDirectories.IndexOf(fileContent[i][1..])] = int.Parse(fileContent[i - 1][1..]);
                }
            }

            return answer;
        }
    }
}