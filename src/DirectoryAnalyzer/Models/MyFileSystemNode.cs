using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectoryAnalyzer.Models;

namespace DirectoryAnalyzer
{
    public class MyFileSystemNode : IEqualityComparer<MyFileSystemNode>
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

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                if (this == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            var incomingObject = obj as MyFileSystemNode;
            if (this.Id != incomingObject.Id || this.DirectoryName != incomingObject.DirectoryName)
            {
                return false;
            }
            if (this.ChildrenDirectories.Except(incomingObject.ChildrenDirectories).Count() != 0 ||
                incomingObject.ChildrenDirectories.Except(this.ChildrenDirectories).Count() != 0)
            {
                return false;
            }
            if (this.Content.Except(incomingObject.Content,(IEqualityComparer<MyFileInfo>)new MyFileInfo()).Count() != 0 ||
                incomingObject.Content.Except(this.Content, (IEqualityComparer<MyFileInfo>)new MyFileInfo()).Count() != 0)
            {
                return false;
            }

            return true;
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

        public bool Equals(MyFileSystemNode? x, MyFileSystemNode? y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] MyFileSystemNode obj)
        {
            var answer = Id;
            answer += DirectoryName.GetHashCode();
            foreach (var subdir in ChildrenDirectories)
            {
                answer += subdir.GetHashCode();
            }
            foreach (var file in Content)
            {
                answer += file.GetHashCode();
            }
            return answer;
        }
    }
}