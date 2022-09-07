using DirectoryAnalyzer.Models;

namespace DirectoryAnalyzer.Providers
{
    internal static class MyFileSystemNodeProvider
    {
        internal static List<MyFileSystemNode> GetFsNodes(IEnumerable<string> directories)
        {
            var answer = new List<MyFileSystemNode>();

            foreach (var directory in directories)
            {
                List<string> children = new();
                List<MyFileInfo> files = new();
                try
                {
                    children.AddRange( Directory.GetDirectories(directory, "*", SearchOption.TopDirectoryOnly));
                }
                catch (UnauthorizedAccessException)
                {
                }
                try
                {
                    foreach (var filename in Directory.GetFiles(directory))
                    {
                        //cleanup on release
                        try
                        {
                            if (string.IsNullOrEmpty(filename))
                            {
                                Console.WriteLine("debug error in GetFsNodes");
                                continue;
                            }
                            files.Add(new MyFileInfo(filename));
                        }
                        catch 
                        {
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                }
                
                answer.Add(new MyFileSystemNode() { DirectoryName = directory, ChildrenDirectories = children , Content = files });
            }
            return answer;
        }
    }
}