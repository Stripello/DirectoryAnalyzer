using DirectoryAnalyzer;

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

/* TODO
 * 
 * empty directories
 * big directories
 * less then 10 files directories
 * access problem
 * fancy крутилко
 * 
 * cleanup
 * length check
 */