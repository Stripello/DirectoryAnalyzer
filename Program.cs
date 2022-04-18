using DirectoryAnalyzer;
using System.Text;


var dir = @"E:\games\BOA";

Console.WriteLine("Creating list of all required directories.");
var requestedDirectoryDB = new DirectoriesDataBase(dir);

Console.WriteLine("Uploading searchlog.");
var storedDirectoryDB = DirectoriesDataBase.ReadFromFile();
var compareResults = storedDirectoryDB.Comparator(requestedDirectoryDB);

Console.WriteLine("Uploading data of files represented in data base.");
var knownFiles = FilesDataBase.ReadFromFile(compareResults.represented);

Console.WriteLine("Collecting data from files not represented in data base.");
var unknownFiles = new FilesDataBase(compareResults.notRepresented);

Console.WriteLine("Analysing files.");
var sb = new StringBuilder();

Console.WriteLine("Modifying data base.");

Console.WriteLine("Modifying succeed. Now you can close process.");

/* TODO
 * change DTOFileInfo - delete all info about directory
 * FSNode by lambda sentence
 * change FsNode
 * change merging two dictionaries
 * big directories
 * fancy крутилко
 * directory = tomename exception
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