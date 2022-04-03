using DirectoryAnalyzer;
/*
var directory = DirectoryOperation.AskDirectory();
var files = DirectoryOperation.GetAllFiles(directory);

var table = TableOperator.BuildTable(DirectoryOperation.GetBiggestFiles(files));
foreach (var element in table)
{
    Console.WriteLine(element);
}

table = TableOperator.BuildTable(DirectoryOperation.GetOldestFiles(files));

foreach (var element in table)
{
    Console.WriteLine(element);
}
table = TableOperator.BuildTable(DirectoryOperation.GetFrequentExtension(files));

foreach (var element in table)
{
    Console.WriteLine(element);
}
*/

foreach (var a in Logger.GetCurrentLog())
{
    Console.WriteLine(a);
}

Logger.UpdateLog(new string[] { });


/* TODO
 * 
 * empty directories
 * big directories
 * less then 10 files directories
 * access problem
 * instead of 
 * fancy крутилко
 * 
 * cleanup
 * length check
 */