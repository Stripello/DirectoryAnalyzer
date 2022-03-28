namespace DirectoryAnalyzer;
internal class DTOFileInfo
{
    internal string name;
    internal string extension;
    internal long size;
    internal DateTime changedate;

    public DTOFileInfo(FileInfo incomingFileInfo)
    {
        name = incomingFileInfo.FullName;
        extension = incomingFileInfo.Extension;
        size = incomingFileInfo.Length / 1024;
        changedate = incomingFileInfo.LastWriteTime;
    }
}