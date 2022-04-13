using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryAnalyzer
{
    internal class FileSystemNode
    {
        internal string name;
        internal string parent;
        internal List<string> children = new();
        internal List<DtoFileInfo> content = new();

        internal static FileSystemNode CreateFromDirectory(string directory)
        {
            var result = new FileSystemNode();
            result.name = directory;
            var files = Array.Empty<string>();

            try
            {
                if (directory == Directory.GetDirectoryRoot(directory))
                {
                    result.parent = null;
                }
                else
                {
                    result.parent = Directory.GetParent(directory).FullName;
                }
                result.children = Directory.GetDirectories(directory).ToList();
                files = Directory.GetFiles(directory);
            }
            catch
            {
            }
            
            result.content = new List<DtoFileInfo>();
            foreach (var file in files)
            {
                result.content.Add(new DtoFileInfo(file));
            }

            return result;
        }
    }
}
