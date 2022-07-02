using DirectoryAnalyzer;
using DirectoryAnalyzer.Dal;
using DirectoryAnalyzer.Providers;
using DirectoryOperationServices;



//TestDirectoryOperationServices.TestGetBiggestFiles();
var userPickedDirectory = DirectoryProvider.AskDirectory();
if (userPickedDirectory == null)
{
    return;
}

var neededDirectories = DirectoryProvider.GetAllDirectories(userPickedDirectory);
var ligtSqlLoc = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + "\\MyDB.db";
var dao = new MyFileSystemNodeDaoLightDb() { DbLocation = ligtSqlLoc };
var nodesFromDb = dao.Read(neededDirectories);
var directoriesFromDb = nodesFromDb.Select(x => x.DirectoryName);
var nodesFromProvider = MyFileSystemNodeProvider.GetFsNodes(neededDirectories.Except(directoriesFromDb));

Parallel.Invoke(
    () =>
{
    var fileList = DirectoryOperation.GetAllFiles(nodesFromDb).ToList();
    fileList.AddRange(DirectoryOperation.GetAllFiles(nodesFromProvider));
    TableOperator.BuildTable(DirectoryOperation.GetOldestFiles(fileList), true, false, false, true, "oldest file").ToList().ForEach(x=>Console.WriteLine(x));
    Console.WriteLine();
    TableOperator.BuildTable(DirectoryOperation.GetBiggestFiles(fileList), true, false, true, false, "biggest files").ToList().ForEach(x => Console.WriteLine(x));
    Console.WriteLine();
    TableOperator.BuildTable(DirectoryOperation.GetFrequentExtension(fileList), "frequient extensions").ToList().ForEach(x => Console.WriteLine(x));
    Console.WriteLine();
    TableOperator.BuildTable(DirectoryOperation.GetBiggestExtensions(fileList), "biggest extensions").ToList().ForEach(x => Console.WriteLine(x));
    var copies = DirectoryOperation.GetCopies(fileList).ToList();
    Console.WriteLine("\npossible copies of files by groups");
    Console.WriteLine(new String('-',50));
    foreach (var subgroup in copies)
    {
        foreach (var element in subgroup)
        {
            Console.WriteLine(element);
        }
        Console.WriteLine(new String('-', 50));
    }

},
() =>
dao.Add(nodesFromProvider)
  );

Console.WriteLine("Data base was successfuly update. Now you can close application.");


/* TODO
  1.2 В базу данных писать только данные, ошибки пишутся в другие места
- SLQ: SqlLite
- web-api
- telegram bot(по желанию)
- определять "дубли" больших файлов
- получить информацию о контенте директории
- обновить данные в БД(пересканировать диск) по запросу
По желанию можно реализовать дополнительно:
- авто пересканирование
- сохранение истории изменений: файлы, подпапки
- получение истории изменений директории
- кешировать ответ последних 100 запросов пользователя
- покрыть тестами логику доступа к данным, хотябы  для одной из 

 * change DTOFileInfo - delete all info about directory
 * FSNode by lambda sentence
 * fancy крутилко - in process, parallel invoke syncronized failure
 * directory = tomename exception
 * cleanup
 * length check
 */