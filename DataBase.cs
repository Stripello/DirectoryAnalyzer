using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DirectoryAnalyzer
{
    internal class DirectoriesDataBase
    {
        internal List<string> directories;

        /// <summary>
        /// it's debateable 
        /// </summary>
        internal DirectoriesDataBase()
        {
        }

        internal DirectoriesDataBase(string directory)
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
                directories = null;
            }
        }

        internal static DirectoriesDataBase? ReadFromFile()
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
                return new DirectoriesDataBase() {directories = fileContent.ToList() };
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendJoin("\n", directories);
            return sb.ToString();
        }

        /// <summary>
        /// Returns collection of directories represented and not represented in DB.
        /// </summary>
        /// <param name="directoriesToCompare"></param>
        /// <returns></returns>
        internal (List<string> represented, List<string> notRepresented) Comparator(DirectoriesDataBase DirectoryDataBaseToCompare)
        {
            List<string> represented = new();
            List<string> notRepresented = new ();
            foreach (var directory in DirectoryDataBaseToCompare.directories) //must be really slow and definitly C# got standarth method for this
            {
                if (directories.Contains(directory))
                {
                    represented.Add(directory);
                }
                else
                {
                    notRepresented.Add(directory);
                }
            }
            return (represented, notRepresented);
        }

        internal static void AddToFile(List<string> stringsToAdd)
        {
            List<string> directories = new List<string>();
            var DS = Path.DirectorySeparatorChar;
            var DataBaseDirectoryFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + DS +
               $"{DS}DB{DS}DirectoriesDB.txt";
            File.AppendAllLines(DataBaseDirectoryFile, stringsToAdd);
        }
    }

    internal class FilesDataBase
    {
        Dictionary<string, List<DtoFileInfo>> files;

        /// <summary>
        /// it's also debatable
        /// </summary>
        internal FilesDataBase()
        {
        }

        internal FilesDataBase(List<string> listOfDirectories)
        {
            files = new Dictionary<string, List<DtoFileInfo>>();
            foreach (var directory in listOfDirectories)
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

        /// <summary>
        /// Reads information of certain derictories from FilesDataBase.
        /// </summary>
        /// <returns></returns>
        internal static FilesDataBase? ReadFromFile(List<string> requestedDirectories)
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
                    if (fileContent[i].StartsWith("$") && requestedDirectories.Contains(fileContent[i].Substring(1)) )
                    {
                        keyForDictionary = fileContent[i].Substring(1);
                        if (i == fileContent.Length - 1)
                        {
                            break;
                        }
                        i++;
                        while (i < fileContent.Length && !fileContent[i+1].StartsWith("$"))
                        {
                            listTempDTO.Add(DtoFileInfo.Parse(fileContent[i]));
                            i++;
                        }
                    }
                    if (!answer.TryAdd(keyForDictionary, listTempDTO))
                    {
                        Console.WriteLine($"FilesDataBase parsing issues, string {i}");
                    }
                    listTempDTO.Clear();
                }
                return new FilesDataBase() { files = answer };
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

        internal static void AddToFile(Dictionary<string,List<DtoFileInfo>> FilesDataBase)
        {
            List<string> directories = new List<string>();
            var DS = Path.DirectorySeparatorChar;
            var DataBaseDirectoryFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + DS +
               $"{DS}DB{DS}FilesDB.txt";
            File.AppendAllText(DataBaseDirectoryFile, FilesDataBase.ToString());
        }
    }
}
