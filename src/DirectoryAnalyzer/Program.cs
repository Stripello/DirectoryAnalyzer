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
    var answer = TableOperator.BuildTable(DirectoryOperation.GetOldestFiles(fileList), true, false, false, true, "oldest file");
    answer = answer.Concat(TableOperator.BuildTable(DirectoryOperation.GetBiggestFiles(fileList), true, false, true, false, "biggest files")).ToList();
    answer = answer.Concat(TableOperator.BuildTable(DirectoryOperation.GetFrequentExtension(fileList), "frequient extensions")).ToList();
    answer = answer.Concat(TableOperator.BuildTable(DirectoryOperation.GetBiggestExtensions(fileList), "biggest extensions")).ToList();
        
    var possibleCopies = DirectoryOperation.GetCopies(fileList);
    
    foreach (var el in answer)
    {
        Console.WriteLine(el);
    }
    
    foreach (var sublist in possibleCopies)
    {
        foreach (var element in sublist)
        {
            global::System.Console.WriteLine($"{element.Name} {element.Extension} {element.Changedate} {element.Size} ");
        }
        global::System.Console.WriteLine("----------------------------------");
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
- покрыть тестами бизнеслогику - in process, rework of DirectoryOperationService is needed

 * change DTOFileInfo - delete all info about directory
 * FSNode by lambda sentence
 * fancy крутилко - in process, parallel invoke syncronized failure
 * directory = tomename exception
 * cleanup
 * length check
 */