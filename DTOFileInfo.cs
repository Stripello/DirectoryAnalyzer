using System.Text;

namespace DirectoryAnalyzer;
internal class DtoFileInfo
{
    internal string name;
    internal string extension;
    internal long size;
    internal DateTime changedate;

    internal DtoFileInfo()
    {
        name = "stub_name";
        extension = "stub_extension";
        size = 0;
        changedate = DateTime.MaxValue;
    }
    internal DtoFileInfo(FileInfo incomingFileInfo)
    {
        name = incomingFileInfo.Name;
        extension = incomingFileInfo.Extension;
        size = incomingFileInfo.Length;
        changedate = incomingFileInfo.LastWriteTime;
    }
    internal DtoFileInfo(string path)
    {
        if (File.Exists(path))
        {
            var fileInfo = new FileInfo(path);
            name = fileInfo.Name;
            extension = fileInfo.Extension;
            size = fileInfo.Length;
            changedate = fileInfo.LastWriteTime;
        }
        else
        {
            name = "error_stub_name";
            extension = "error_stub_extension";
            size = 0;
            changedate = DateTime.MaxValue;
        }
    }
    public override string ToString()
    {
        var sb = new StringBuilder(name.ToString());
        sb.Append('*');
        sb.Append(extension.ToString());
        sb.Append('*');
        sb.Append(size.ToString());
        sb.Append('*');
        sb.Append(changedate.ToString());
        return sb.ToString();
    }
    internal static DtoFileInfo Parse(string stringToParse)
    {
        var temp = stringToParse.Split('*');
        var answer = new DtoFileInfo();
        answer.name = temp[0];
        answer.extension = temp[1];
        answer.size = long.Parse(temp[2]);
        answer.changedate = DateTime.Parse(temp[3]);
        return answer;
    }


}