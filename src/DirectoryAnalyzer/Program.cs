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
Console.WriteLine("Processing data, plese wait.");
var neededDirectories = DirectoryProvider.GetAllDirectories(userPickedDirectory);
var dataBaseDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString();
var dao = new MyFileSystemNodeDaoSelfWrittenDb(dataBaseDirectory);
var nodesFromDb = dao.Read(neededDirectories);
var directoriesFromDb = nodesFromDb.Select(x => x.DirectoryName);
var nodesFromProvider = MyFileSystemNodeProvider.GetFsNodes(neededDirectories.Except(directoriesFromDb));

var fileList = DirectoryOperation.GetAllFiles(nodesFromDb).ToList();
fileList.AddRange(DirectoryOperation.GetAllFiles(nodesFromProvider));
TableOperator.BuildTable(DirectoryOperation.GetOldestFiles(fileList), true, false, false, true, "oldest file").ToList().ForEach(x => Console.WriteLine(x));
Console.WriteLine();
TableOperator.BuildTable(DirectoryOperation.GetBiggestFiles(fileList), true, false, true, false, "biggest files").ToList().ForEach(x => Console.WriteLine(x));
Console.WriteLine();
TableOperator.BuildTable(DirectoryOperation.GetFrequentExtension(fileList), "frequient extensions").ToList().ForEach(x => Console.WriteLine(x));
Console.WriteLine();
TableOperator.BuildTable(DirectoryOperation.GetBiggestExtensions(fileList), "biggest extensions").ToList().ForEach(x => Console.WriteLine(x));
var copies = DirectoryOperation.GetCopies(fileList).ToList();
if (copies.Count > 0)
{
    Console.WriteLine("\nPossible duplicate files by groups:");
    Console.WriteLine(new String('-', 50));
    foreach (var subgroup in copies)
    {
        foreach (var element in subgroup)
        {
            Console.WriteLine(element);
        }
        Console.WriteLine(new String('-', 50));
    }
}
else
{
    Console.WriteLine("\nCan't find duplicate of files.");
}
dao.Add(nodesFromProvider);


Console.WriteLine("Data base was successfuly update. Now you can close application.");


/* TODO
  1.2 В базу данных писать только данные, ошибки пишутся в другие места
- SLQ: SqlLite
- web-api
- telegram bot(по желанию)
- обновить данные в БД(пересканировать диск) по запросу
По желанию можно реализовать дополнительно:
- авто пересканирование
- сохранение истории изменений: файлы, подпапки
- получение истории изменений директории
- кешировать ответ последних 100 запросов пользователя
- покрыть тестами логику доступа к данным, хотябы  для одной из 

 * fancy крутилко - in process, parallel invoke syncronized failure
 * directory = tomename exception
 * cleanup
 * length check
 */