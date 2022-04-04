using DirectoryAnalyzer;

var directory = DirectoryOperation.AskDirectory();
if (directory == null)
{
    return;
}
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
 * big directories
 * access problem
 * instead of 
 * fancy крутилко
 * 
 * cleanup
 * length check
 */