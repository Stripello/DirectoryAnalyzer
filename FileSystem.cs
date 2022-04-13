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
      
        internal FileSystem(string directory) //Creates FS class of chosen directory
        {
            fileSystem = new Dictionary<string, FileSystemNode>();
            var tempNode = new FileSystemNode(directory);
            AddNode(tempNode);
            foreach (var childNodeAdress in tempNode.child)
            {
                var subFileSystem =new FileSystem(childNodeAdress);
                foreach (var element in subFileSystem.fileSystem.Keys)
                {
                    AddNode(subFileSystem.fileSystem[element]);
                }
            }
        }

        internal FileSystem(bool fromLog) //creates FS class from log
        {
            fileSystem = new Dictionary<string, FileSystemNode>();
            if (!fromLog)
            {
                return;
            }
            var logFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                "\\Log\\Log.txt";
            
            if (!File.Exists(logFile))
            {
                File.Create(logFile);
                return;
            }
            var Log = File.ReadAllLines(logFile);


        }

        internal string[] ToString(FileSystem fileSystem)
        {
            var sb = new StringBuilder();
            foreach (var elemet in fileSystem.fileSystem)
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

        private FileSystem Parse(string[] incomingData)
        {
            var answer = new FileSystem(false);
            


            return answer;
        }

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

        internal void LeftOnlyChildNodes(string directory)
        {

        }
    }
}
