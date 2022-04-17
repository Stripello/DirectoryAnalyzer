using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryAnalyzer
{
    internal class DataBaseDirectories
    {
        List<string> directories;

        public DataBaseDirectories(string directory)
        {
            if (Directory.Exists(directory))
            {
                directories = Directory.GetDirectories(directory, "*.*", searchOption: SearchOption.AllDirectories).ToList();
            }
            else
            {
                throw new Exception($"Can't find directory {directory}");
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
    }
}
