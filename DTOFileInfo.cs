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
        size = incomingFileInfo.Length;
        changedate = incomingFileInfo.LastWriteTime;
    }
    public DTOFileInfo()
    {
        this.name = "stub_name";
        this.extension = "stub_extension";
        this.size = 0;
        this.changedate = DateTime.MaxValue;
    }
}