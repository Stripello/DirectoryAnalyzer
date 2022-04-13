using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryAnalyzer
{
    enum FsNodeStatus
    {
        orphan,        //core of all file system, like tome itself
        unknownFather, //father node unknown by FileSystem, but exist
        fledgling      //no child nodes 
    }

    internal class FileSystemNode
    {
        internal string name;
        internal string? parent;
        internal List<string>? child;
        internal List<DtoFileInfo>? content;

        internal FileSystemNode(string directory)
        {
            name = directory;
            if (directory == Directory.GetDirectoryRoot(directory))
            {
                parent = null;
            }
            else
            {
                parent = Directory.GetParent(directory).FullName;
            }
            child = Directory.GetDirectories(directory).ToList();
            content = new List<DtoFileInfo>();
            foreach (var file in Directory.GetFiles(directory))
            {
                content.Add(new DtoFileInfo(file));
            }
        }

        
    }
}
