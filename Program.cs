using DirectoryAnalyzer;

var directory = DirectoryOperation.AskDirectory();

var biggestFiles = DirectoryOperation.GetBiggestFiles(directory);
foreach (var tableElement in TableOperator.BuildTable(biggestFiles))
{
    Console.WriteLine(tableElement);
}

var oldestFiles = DirectoryOperation.GetOldestFiles(directory);
foreach (var tableElement in TableOperator.BuildTable(oldestFiles))
{
    Console.WriteLine(tableElement);
}

var frequentExtensons = DirectoryOperation.GetFrequentExtension(directory);
foreach (var tableElement in TableOperator.BuildTable(frequentExtensons))
{
    Console.WriteLine(tableElement);
}
