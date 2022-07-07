using DirectoryAnalyzer;
using DirectoryAnalyzer.Dal;
using DirectoryAnalyzer.Models;
using DirectoryAnalyzer.Providers;
using DirectoryOperationServices;


#region debug unit test for dummies zone
/*
var directoryName = @"C:\repos\Git repos\DirectoryAnalyzer\tests\DaoTests\TestData";
var fileName = "DbForUpdate";
var myDb = new MyFileSystemNodeDaoSelfWrittenDb(directoryName, fileName);
var fullName = directoryName + "\\" + fileName + ".txt";
var nodesToUpdate = new List<MyFileSystemNode>() { new MyFileSystemNode() {
                DirectoryName = @"C:\repos\try-samples-main\LINQ\docs",
                ChildrenDirectories = new List<string> { @"C:\repos\try-samples-main\LINQ\docs\KindaNewSubdir" },
            Content = new List<MyFileInfo>(){
                new MyFileInfo() { Name = @"C:\repos\try-samples-main\LINQ\docs\KindaNewFile.gif" ,
                Extension = ".gif",Size = 2424,Changedate = DateTime.Parse("03.07.2022 19:48:15")},
                new MyFileInfo () { Name = @"C:\repos\try-samples-main\LINQ\docs\lazy-equation.md",
                Extension = ".md", Size = 3001, Changedate = DateTime.Parse("19.10.2021 3:30:52")}
                  }
            } };
myDb.UpdateDb(nodesToUpdate);

var actual = File.ReadAllLines(fullName);
var expected = new string[] { "$1",
            @">C:\repos\try-samples-main\LINQ",
            @"?C:\repos\try-samples-main\LINQ\docs",
            @"?C:\repos\try-samples-main\LINQ\src",
            @"*C:\repos\try-samples-main\LINQ\readme.md*.md*1489*19.10.2021 3:30:52",
            "",
            "$3",
            @">C:\repos\try-samples-main\LINQ\src",
            @"*C:\repos\try-samples-main\LINQ\src\LINQ.csproj*.csproj*431*19.10.2021 3:30:52",
            @"*C:\repos\try-samples-main\LINQ\src\Program.cs*.cs*2900*19.10.2021 3:30:52",
            "",
            "$2",
            @">C:\repos\try-samples-main\LINQ\docs",
            @"?C:\repos\try-samples-main\LINQ\docs\KindaNewSubdir",
            @"*C:\repos\try-samples-main\LINQ\docs\KindaNewFile.gif*.gif*2424*03.07.2022 19:48:15",
            @"*C:\repos\try-samples-main\LINQ\docs\lazy-equation.md*.md*3001*19.10.2021 3:30:52",
            ""
            };
Console.WriteLine(expected == actual);
*/
#endregion


var userPickedDirectory = DirectoryProvider.AskDirectory();
if (userPickedDirectory == null)
{
    return;
}
Console.WriteLine("Processing data, plese wait.");
var neededDirectories = DirectoryProvider.GetAllDirectories(userPickedDirectory);
var dataBaseDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString();
var dao = new MyFileSystemNodeDaoSQLite(dataBaseDirectory);
var nodesFromDb = dao.Read(neededDirectories);
var directoriesFromDb = nodesFromDb.Select(x => x.DirectoryName);
var nodesFromProvider = MyFileSystemNodeProvider.GetFsNodes(neededDirectories.Except(directoriesFromDb));
var allNodes = nodesFromDb.Concat(nodesFromProvider);

var allFiles = DirectoryOperation.GetAllFiles(allNodes).ToList();
TableOperator.BuildTable(DirectoryOperation.GetOldestFiles(allFiles), true, false, false, true, "oldest file").ToList().ForEach(x => Console.WriteLine(x));
Console.WriteLine();
TableOperator.BuildTable(DirectoryOperation.GetBiggestFiles(allFiles), true, false, true, false, "biggest files").ToList().ForEach(x => Console.WriteLine(x));
Console.WriteLine();
TableOperator.BuildTable(DirectoryOperation.GetFrequentExtension(allFiles), "frequient extensions").ToList().ForEach(x => Console.WriteLine(x));
Console.WriteLine();
TableOperator.BuildTable(DirectoryOperation.GetBiggestExtensions(allFiles), "biggest extensions").ToList().ForEach(x => Console.WriteLine(x));
Console.WriteLine();
TableOperator.BuildTable(DirectoryOperation.GetBiggestDirectories(allNodes), "biggest directories").ToList().ForEach(x => Console.WriteLine(x));
var copies = DirectoryOperation.GetCopies(allFiles).ToList();
if (copies.Count > 0)
{
    //cleanup
    Console.WriteLine("\nPossible duplicate files by groups:");
    var divider = new String('-', 50);
    Console.WriteLine(divider);
    foreach (var subgroup in copies)
    {
        foreach (var element in subgroup)
        {
            Console.WriteLine(element);
        }
        Console.WriteLine(divider);
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

 * threading
 * directory = tomename exception
 * cleanup
 * length check
 * auto delatate console output
 */