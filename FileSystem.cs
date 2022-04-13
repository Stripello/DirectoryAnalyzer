using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryAnalyzer
{
    internal class FileSystem
    {
        internal Dictionary<string, FileSystemNode> fileSystem;

        internal FileSystem(string directory) //Creates FS class example of chosen directory
        {
            fileSystem = new Dictionary<string, FileSystemNode>();
            var tempNode = new FileSystemNode(directory);
            AddNode(tempNode);
            foreach (var childNodeAdress in tempNode.child)
            {
                var subFileSystem = new FileSystem(childNodeAdress);
                foreach (var element in subFileSystem.fileSystem.Keys)
                {
                    AddNode(subFileSystem.fileSystem[element]);
                }
            }
        }

        internal FileSystem(bool fromLog) //creates FS class example from log or empty one
        {
            if (!fromLog)
            {
                fileSystem = new Dictionary<string, FileSystemNode>();
                return ;
            }
            var logFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                "\\Log\\Log.txt";

            if (!File.Exists(logFile))
            {
                File.Create(logFile);
                fileSystem = new Dictionary<string, FileSystemNode>();
                return;
            }
            fileSystem = Parse(File.ReadAllLines(logFile)).fileSystem;
        }

        private bool AddNode(FileSystemNode incomingNode)
        {
            if (fileSystem != null && fileSystem.ContainsKey(incomingNode.name))
            {
                return false;
            }
            else
            {
                fileSystem.Add(incomingNode.name, incomingNode);
                return true;
            }
        }
        internal string[] ToStringArray()
        {
            var sb = new StringBuilder();
            foreach (var elemet in fileSystem)
            {
                sb.Append("\n$" + elemet.Key);
                sb.Append("\n?" + elemet.Value.parent);
                foreach (var childNode in elemet.Value.child)
                {
                    sb.Append("\n>"+childNode);
                }
                foreach (var file in elemet.Value.content)
                {
                    sb.Append("\n*"+file.ToString());
                }
            }
            return sb.ToString().Split("\n");
        }

        private static FileSystem Parse(string[] incomingText)
        {
            var answer = new FileSystem(false);

            var listOfNodeHeads = new List<long>();
            for (long i = 0; i < incomingText.Length; i++)
            {
                if (incomingText[i].StartsWith("$"))
                {
                    listOfNodeHeads.Add(i);
                }
            }

            for (int i =0; i < listOfNodeHeads.Count; i++) 
            {
                var beginIndex = listOfNodeHeads[i];
                long endIndex;
                if (i == listOfNodeHeads.Count)
                {
                    endIndex = incomingText.Length - 1;
                }
                else
                {
                    endIndex = listOfNodeHeads[i + 1];
                }

                var name = incomingText[beginIndex].Remove(0, 1);
                var parent = incomingText[beginIndex + 1].Remove(0, 1);

                var child = new List<string> { };
                var content = new List<DtoFileInfo> { };
                for (long j = beginIndex + 2; j <= endIndex; j++)
                {
                    if (incomingText[j].StartsWith(">"))
                    {
                        child.Add(incomingText[j].Remove(0, 1));
                    }
                    if (incomingText[j].StartsWith("*"))
                    {
                        var tempDTO = DtoFileInfo.Parse(incomingText[j].Remove(0,1));
                        content.Add(tempDTO);
                    }
                    else
                    {
                        throw new Exception("Parser error.");
                    }
                }
                answer.AddNode(new FileSystemNode(name, parent, child, content));
            }
            return answer;
        }

        internal static FileSystem GetOnlyChild(string directory, FileSystem incomingFS) //bad naming
        {
            var answer = new FileSystem(false);
            if (!incomingFS.fileSystem.ContainsKey(directory))
            {
                return answer;
            }
            var parentNode = incomingFS.fileSystem[directory];
            answer.AddNode(parentNode);

            var currentGeneration = parentNode.child;
            var futureGeneration = new List<string>{ };

            while (true)
            {
                foreach (var elementOfCurrentGeneration in currentGeneration)
                {
                    answer.AddNode(incomingFS.fileSystem[elementOfCurrentGeneration]);
                    futureGeneration.AddRange(incomingFS.fileSystem[elementOfCurrentGeneration].child);
                }
                if (futureGeneration.Count == 0)
                {
                    break;
                }
                else
                {
                    currentGeneration = futureGeneration;
                }
            }

            return answer;
        }
    }
}
