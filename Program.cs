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
 * fancy крутилко
 * 
 * forbidden symbols of directory and file names are same for windows,linux and macos systems?
 * directory and file existance check
 * cleanup
 * length check
 */