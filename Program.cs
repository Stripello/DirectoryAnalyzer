using DirectoryAnalyzer;
using System.Text;

var directory = @"c:" + Path.DirectorySeparatorChar;
//var allFiles = DirectoryOperation.GetAllFiles(directory);

var fs = new FileSystem(directory);
var allFiles = DirectoryOperation.GetAllFiles(fs);

var biggestFiles = DirectoryOperation.GetBiggestFiles(allFiles);
var oldestFiles = DirectoryOperation.GetOldestFiles(allFiles);
var frequientExtension = DirectoryOperation.GetFrequentExtension(allFiles);

var answer = new StringBuilder();
answer.AppendJoin("\n",TableOperator.BuildTable(biggestFiles));
answer.Append("\n");
answer.AppendJoin("\n", TableOperator.BuildTable(oldestFiles));
answer.Append("\n");
answer.AppendJoin("\n", TableOperator.BuildTable(frequientExtension));
answer.Append("\n");

Console.WriteLine(answer);

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
 * forbidden symbols of directory and file names are same for windows,linux and macos systems?
 * directory and file existance check
 * cleanup
 * length check
 * accessability check
 */