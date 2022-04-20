using System.Text;

namespace DirectoryAnalyzer
{
    internal class DirectoriesDataBase
    {
        internal List<string> directories = new List<string> { "" };

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
                directories = new List<string>();
                directories = Directory.EnumerateDirectories(directory, "*", new EnumerationOptions
                {
                    IgnoreInaccessible = true,
                    RecurseSubdirectories = true
                }).ToList();
                directories.Add(directory);
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
            var directoryDb = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName
                + $"{DS}LocalDB";
            if (!Directory.Exists(directoryDb))
            {
                Directory.CreateDirectory(directoryDb);
            }
            var DataBaseDirectoryFile = directoryDb + $"{DS}DirectoriesDB.txt";
            if (!File.Exists(DataBaseDirectoryFile))
            {
                File.Create(DataBaseDirectoryFile).Close();
                return null;
            }
            else
            {
                var fileContent = File.ReadAllLines(DataBaseDirectoryFile);
                return new DirectoriesDataBase() { directories = fileContent.ToList() };
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
        internal static (List<string>? represented, List<string>? notRepresented) Comparator
            (DirectoriesDataBase whatWeGot, DirectoriesDataBase whatWeNeed)
        {
            if (whatWeGot == null)
            {
                return (null, whatWeNeed.directories);
            }
            if (whatWeNeed == null)
            {
                return (whatWeGot.directories, null);
            }

            var directories = whatWeGot.directories;

            List<string> represented = new();
            List<string> notRepresented = new();
            foreach (var directory in whatWeNeed.directories) 
            //^must be really slow and definitly C# got standarth method for this
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
            var DataBaseDirectoryFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.
                FullName + $"{DS}LocalDB{DS}DirectoriesDB.txt";
            File.AppendAllLines(DataBaseDirectoryFile, stringsToAdd);
        }
    }
}
