using DirectoryAnalyzer;
using DirectoryAnalyzer.Dal;
using DirectoryAnalyzer.Models;
using DirectoryAnalyzer.Providers;
using DirectoryOperationServices;
using System.Configuration;


ConnectionStringSettingsCollection settings =
            ConfigurationManager.ConnectionStrings;

if (settings != null)
{
    foreach (ConnectionStringSettings cs in settings)
    {
        Console.WriteLine(cs.Name);
        Console.WriteLine(cs.ProviderName);
        Console.WriteLine(cs.ConnectionString);
    }
}

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
TableOperator.BuildTable(DirectoryOperation.GetOldestFiles(allFiles), true, false, false, true, "oldest files")
    .ToList().ForEach(x => Console.WriteLine(x));
Console.WriteLine();
TableOperator.BuildTable(DirectoryOperation.GetBiggestFiles(allFiles), true, false, true, false, "biggest files").
    ToList().ForEach(x => Console.WriteLine(x));
Console.WriteLine();
TableOperator.BuildTable(DirectoryOperation.GetFrequentExtension(allFiles), head:"frequient extensions",
    firstColumnName:"extension",secondColumnName:"repetition rate").ToList().ForEach(x => Console.WriteLine(x));
Console.WriteLine();
TableOperator.BuildTable(DirectoryOperation.GetBiggestExtensions(allFiles),head: "biggest extensions",
    firstColumnName:"extendion",secondColumnName:"summarized size").ToList().ForEach(x => Console.WriteLine(x));
Console.WriteLine();
TableOperator.BuildTable(DirectoryOperation.GetBiggestDirectories(allNodes), 
    head:"biggest directories (child derictories size doesn't count)",firstColumnName:"name of directory"
    ,secondColumnName:"summarized content size").ToList().ForEach(x => Console.WriteLine(x));
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