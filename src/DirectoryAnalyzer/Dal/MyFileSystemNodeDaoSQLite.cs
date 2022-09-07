using Microsoft.Data.Sqlite;
using DirectoryAnalyzer.Models;

namespace DirectoryAnalyzer.Dal
{
    internal class MyFileSystemNodeDaoSQLite : IMyFileSystemNodeDao
    {
        readonly string dataBaseLocation;

        public MyFileSystemNodeDaoSQLite(string dataBaseDirectory, string dataBaseName = "MySQLiteDb")
        {
            dataBaseLocation = dataBaseDirectory + "\\" + dataBaseName + ".db";
        }
        public void Add(IList<MyFileSystemNode> nodesToStore)
        {
            using (var connection = new SqliteConnection($"Data Source={dataBaseLocation};Mode=ReadWriteCreate;"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = "CREATE TABLE IF NOT EXISTS FileSystemNodes" +
                    "(Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                    "DirectoryName TEXT NOT NULL," +
                    "ChildrenDirectories TEXT," +
                    "Content TEXT)";
                command.ExecuteNonQuery();
                foreach (var node in nodesToStore)
                {
                    command.CommandText = "INSERT INTO FileSystemNodes (DirectoryName,ChildrenDirectories,Content)" +
                        $"VALUES ('{node.DirectoryName}'," +
                        $"'{string.Join('\n', node.ChildrenDirectories)}'," +
                        $"'{string.Join('\n', node.Content.Select(x => x.ToString()))}')";
                    command.ExecuteNonQuery();
                    //Ducttape using joined strings instead of arrays
                }
            }
        }

        public IList<MyFileSystemNode> Read(IList<string> directoriesToSearch)
        {
            var dbContent = new List<MyFileSystemNode>();
            using (var connection = new SqliteConnection($"Data Source={dataBaseLocation};Mode=ReadWriteCreate;"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='FileSystemNodes'";
                using (var checkFortableExist = command.ExecuteReader())
                {
                    if (!checkFortableExist.HasRows)
                    {
                        return dbContent;
                    }
                }
                command.CommandText = "SELECT * FROM FileSystemNodes";
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //ducttape using upcast (long) then downcast (int)
                            var Id = (long)reader.GetValue(0);
                            var DirectoryName = (string)reader.GetValue(1);
                            var ChildrenDirectories = reader.GetValue(2).ToString().Split('\n').ToList();
                            var Content = reader.GetValue(3).ToString().Split('\n').ToList().Where(x=>!string.IsNullOrEmpty(x)).Select(x=> MyFileInfo.Parse(x)).ToList();
                            dbContent.Add(new MyFileSystemNode() { Id = (int)Id, DirectoryName = DirectoryName, ChildrenDirectories = ChildrenDirectories , Content = Content});
                        }
                    }
                }


            }

            //probably it's better to order both of arrays then only start to intersect
           var answer = (from directory in directoriesToSearch
                        join node in dbContent on directory equals node.DirectoryName
                        select node).ToList();

            return answer ?? new List<MyFileSystemNode>();
        }

        public void UpdateDb(IList<MyFileSystemNode> nodesToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}