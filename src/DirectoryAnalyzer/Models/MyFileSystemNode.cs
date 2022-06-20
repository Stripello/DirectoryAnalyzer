using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectoryAnalyzer.Models;

namespace DirectoryAnalyzer
{
    public class MyFileSystemNode
    {

        public int Id { get; set; }
        public string DirectoryName { get; set; }
        public List<string> ChildrenDirectories { get; set; }
        public List<MyFileInfo> Content { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"${Id}");
            sb.AppendLine($">{DirectoryName}") ;
            foreach (var child in ChildrenDirectories)
            {
                sb.AppendLine($"?{child.ToString()}");
            }
            foreach (var fileInfos in Content)
            {
                sb.AppendLine($"{fileInfos.ToString()}");
            }
            return sb.ToString();
        }

        public static MyFileSystemNode Parse(string[] incomingStrings)
        {
            var id = int.Parse(incomingStrings[0][1..]);
            var directoryName = incomingStrings[1][1..];
            var childrenDirectories = new List<string>();
            int i = 2;
            for (; i < incomingStrings.Length && incomingStrings[i].StartsWith('?'); i++)
            {
                childrenDirectories.Add(incomingStrings[i][1..]);
            }
            var content = new List<MyFileInfo>();
            for (;i<incomingStrings.Length; i++)
            {
                content.Add(MyFileInfo.Parse(incomingStrings[i][1..]));
            }
            return new MyFileSystemNode() { Id = id, DirectoryName = directoryName, ChildrenDirectories = childrenDirectories, Content = content};
        }
    }
}
