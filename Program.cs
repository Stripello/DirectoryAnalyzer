using DirectoryAnalyzer;
using System.Text;


var dir = @"E:\games\BOA";
var dbD = new DataBaseDirectories(dir);
var dbF = new DataBaseFiles(dbD);

Console.WriteLine(dbF);





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