using DirectoryAnalyzer;
using System.Text;

var dir = DirectoryOperation.AskDirectory();
if (dir == null)
{
    return;
}
Console.WriteLine("Creating list of all required directories.");
var requestedDirectoryDB = new DirectoriesDataBase(dir);

Console.WriteLine("Uploading searchlog.");
var storedDirectoryDB = DirectoriesDataBase.ReadFromFile();
var compareResults = DirectoriesDataBase.Comparator(storedDirectoryDB,requestedDirectoryDB);

Console.WriteLine("Uploading data of files represented in data base.");
var knownFiles = FilesDataBase.ReadFromFile(compareResults.represented);

Console.WriteLine("Collecting data from files not represented in data base.");
var unknownFiles = new FilesDataBase(compareResults.notRepresented);

Console.WriteLine("Concatinate necesary data from stored and unstored elements.");
var DtoToAnalyze = FilesDataBase.GetDtoFileInfos(knownFiles, unknownFiles);

Console.WriteLine("Analysing files.");
var sb = new StringBuilder();
sb.AppendJoin("\n",TableOperator.BuildTable(DirectoryOperation.GetBiggestFiles(DtoToAnalyze)));
sb.Append("\n");
sb.AppendJoin("\n", TableOperator.BuildTable(DirectoryOperation.GetOldestFiles(DtoToAnalyze)));
sb.Append("\n");
sb.AppendJoin("\n", TableOperator.BuildTable(DirectoryOperation.GetFrequentExtension(DtoToAnalyze)));
sb.Append("\n");
Console.WriteLine(sb);

Console.WriteLine("Modifying data base.");
DirectoriesDataBase.AddToFile(compareResults.notRepresented);
FilesDataBase.AddToFile(unknownFiles);
Console.WriteLine("Modifying succeed. Now you can close process.");

/* TODO
 * change DTOFileInfo - delete all info about directory
 * FSNode by lambda sentence
 * change FsNode
 * change merging two dictionaries
 * big directories
 * fancy крутилко
 * directory = volumename exception
 * FileSystem.Parse add divider by parts 
 * FileSystem.GetOnlyChild could be optimized
 * ModifyLog - finish
 * 
 * var check
 * directory and file existance check
 * cleanup
 * length check
 * accessability check
 */