using System.Text;

namespace DirectoryAnalyzer;
internal class DTOFileInfo
{
    internal string name;
    internal string extension;
    internal long size;
    internal DateTime changedate;

    internal DTOFileInfo(FileInfo incomingFileInfo)
    {
        name = incomingFileInfo.FullName;
        extension = incomingFileInfo.Extension;
        size = incomingFileInfo.Length;
        changedate = incomingFileInfo.LastWriteTime;
    }
    internal DTOFileInfo()
    {
        this.name = "stub_name";
        this.extension = "stub_extension";
        this.size = 0;
        this.changedate = DateTime.MaxValue;
    }

    public override string ToString()
    {
        var sb = new StringBuilder(name.ToString());
        sb.Append("*");
        sb.Append(extension.ToString());
        sb.Append("*");
        sb.Append(size.ToString());
        sb.Append("*");
        sb.Append(changedate.ToString());
        return sb.ToString();
    }

    internal DTOFileInfo Parse(string stringToParse)
    {
        stringToParse.First('*');


    }


}