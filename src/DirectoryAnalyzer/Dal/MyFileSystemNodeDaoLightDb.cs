using DirectoryAnalyzer.Models;
using LiteDB;

namespace DirectoryAnalyzer.Dal
{
    public class MyFileSystemNodeDaoLightDb : IMyFileSystemNodeDao
    {
        public string DbLocation;

        public void Add(IList<MyFileSystemNode> nodesToStore)
        {
            using (var db = new LiteDatabase(DbLocation))
            {
                var storedFileSystemNodes = db.GetCollection<MyFileSystemNode>("FileSystemNodes");
                foreach (var node in nodesToStore)
                {
                    storedFileSystemNodes.Insert(node);
                    //try bulkInsert
                }
            }
        }
        public IList<MyFileSystemNode> Read(IList<string> directoriesToSearch)
        {
            var answer = new List<MyFileSystemNode>();
            using (var db = new LiteDatabase(DbLocation))
            {
                var storedFileSystemNodes = db.GetCollection<MyFileSystemNode>("FileSystemNodes");
                answer.AddRange( storedFileSystemNodes.Query().Where(x=> directoriesToSearch.Contains(x.DirectoryName)).ToList() );
            }
            return answer;
        }
        public void UpdateDb(IList<MyFileSystemNode> nodesToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
