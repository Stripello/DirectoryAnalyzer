using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryAnalyzer
{
    internal class DataBaseDirectories
    {
        internal List<string> directories;

        /// <summary>
        /// it's debateable 
        /// </summary>
        internal DataBaseDirectories()
        {
        }

        internal DataBaseDirectories(string directory)
        {
            if (Directory.Exists(directory))
            {
                directories = Directory.EnumerateDirectories(directory, "*", new EnumerationOptions
                {
                    IgnoreInaccessible = true,
                    RecurseSubdirectories = true
                }).ToList();
            }
            else
            {
                Console.WriteLine($"Can't find directory {directory}"); 
            }
        }

        internal static DataBaseDirectories? ReadFromFile()
        {
            List<string> directories = new List<string>();
            var DS = Path.DirectorySeparatorChar;
            var DataBaseDirectoryFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + DS +
               $"{DS}DB{DS}DirectoriesDB.txt";
            if (!File.Exists(DataBaseDirectoryFile))
            {
                File.Create(DataBaseDirectoryFile);
                return null;
            }
            else
            {
                var fileContent = File.ReadAllLines(DataBaseDirectoryFile);
                return new DataBaseDirectories() {directories = fileContent.ToList() };
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendJoin("\n", directories);
            return sb.ToString();
        }
    }

    internal class DataBaseFiles
    {
        Dictionary<string, List<DtoFileInfo>> files;

        /// <summary>
        /// it's also debatable
        /// </summary>
        internal DataBaseFiles()
        {
        }

        internal DataBaseFiles(DataBaseDirectories listOfDirectories)
        {
            files = new Dictionary<string, List<DtoFileInfo>>();
            foreach (var directory in listOfDirectories.directories)
            {
                
                var fileEnum = Directory.EnumerateFiles(directory, "*", new EnumerationOptions 
                { 
                    IgnoreInaccessible = true,
                    RecurseSubdirectories = false
                });
                var dtoList = new List<DtoFileInfo>() { };
                foreach (var filePath in fileEnum)
                {
                    dtoList.Add(new DtoFileInfo(filePath) );
                }

                files.TryAdd(directory, dtoList);
            }
        }

        internal static DataBaseFiles? ReadFromFile()
        {
            var answer = new Dictionary<string,List<DtoFileInfo>>();
            var DS = Path.DirectorySeparatorChar;
            var DataBaseDirectoryFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
               $"{DS}DB{DS}FilesDB.txt";
            if (!File.Exists(DataBaseDirectoryFile))
            {
                File.Create(DataBaseDirectoryFile);
                return null;
            }
            else
            {
                var fileContent = File.ReadAllLines(DataBaseDirectoryFile);
                string keyForDictionary = String.Empty;        //correct init empty string??
                var listTempDTO = new List<DtoFileInfo>();
                for (int i = 0; i < fileContent.Length; i++)
                {
                    if (fileContent[i].StartsWith("$"))
                    {
                        keyForDictionary = fileContent[i].Substring(1);
                    }
                    else
                    {
                        listTempDTO.Add( DtoFileInfo.Parse(fileContent[i]) );
                    }

                    if ((i + 1 < fileContent.Length && fileContent[i + 1].StartsWith("$")) || i == fileContent.Length-1)
                    {
                        if (!answer.TryAdd(keyForDictionary, listTempDTO))
                        {
                            Console.WriteLine($"Some file parsing issues during create FileDataBase.{i} string.");
                        }
                        listTempDTO.Clear();
                    }
                }
                return new DataBaseFiles() { files = answer };
            }
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var element in files)
            {
                sb.Append("$" + element.Key + "\n");
                foreach (var file in element.Value)
                {
                    sb.Append(file.ToString() + "\n");
                }
            }
            return sb.ToString();
        }
    }
}
