using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryAnalyzer
{
    internal class FileSystem
    {
        internal Dictionary<string, FileSystemNode> fileSystem = new();

        /// <summary>
        /// Creates FS class example of chosen directory
        /// </summary>
        /// <param name="directory">chosen directory</param>
        internal FileSystem(string directory)
        {
            var tempNode = FileSystemNode.CreateFromDirectory(directory);
            fileSystem.TryAdd(tempNode.name, tempNode);
            foreach (var childNodeAddress in tempNode.children)
            {
                var subFileSystem = new FileSystem(childNodeAddress);
                foreach (var element in subFileSystem.fileSystem.Keys)
                {
                    fileSystem.TryAdd(subFileSystem.fileSystem[element].name, subFileSystem.fileSystem[element]);
                }
            }
        }

        /// <summary>
        /// Creates FS class example from log or empty one
        /// </summary>
        internal FileSystem(bool fromLog)
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

        internal string[] ToStringArray()
        {
            var sb = new StringBuilder();
            foreach (var elemet in fileSystem)
            {
                sb.Append("\n$" + elemet.Key);
                sb.Append("\n?" + elemet.Value.parent);
                foreach (var childNode in elemet.Value.children)
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
                //             new FileSystemNode(name, parent, child, content)
                answer.fileSystem.TryAdd(name, new FileSystemNode {name = name, children = child, content = content, parent = parent});
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
            answer.fileSystem.TryAdd(parentNode.name, parentNode);

            var currentGeneration = parentNode.children;
            var futureGeneration = new List<string>{ };

            while (true)
            {
                foreach (var elementOfCurrentGeneration in currentGeneration)
                {
                    answer.fileSystem.TryAdd(incomingFS.fileSystem[elementOfCurrentGeneration].name, incomingFS.fileSystem[elementOfCurrentGeneration]);
                    futureGeneration.AddRange(incomingFS.fileSystem[elementOfCurrentGeneration].children);
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
