namespace DirectoryAnalyzer.Models;
public class MyFileInfo
{
    public string Name { get; set; }
    public string Extension { get; set; }
    public long Size { get; set; }
    public DateTime Changedate { get; set; }

    public MyFileInfo(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Console.WriteLine("Error in MyFileInfo constructor");
            Name = String.Empty;
            Extension = String.Empty;
            Size = 0;
            Changedate = DateTime.MaxValue;
            return;
        }
        var temp = new FileInfo(path);
        Name = temp.FullName;
        Extension = temp.Extension;
        Size = temp.Length;
        Changedate = temp.LastWriteTime;
    }

    public MyFileInfo()
    {
        Name = String.Empty;
        Extension = String.Empty;
        Size = 0;
        Changedate = DateTime.MaxValue;
    }

    public override string ToString()
    {
        return ($"*{Name}*{Extension}*{Size}*{Changedate}");
    }

    public static MyFileInfo Parse(string stringToParse)
    {
        var tempArray = stringToParse[1..].Split('*');
        return new MyFileInfo() { Name = tempArray[0], Extension = tempArray[1], Size = long.Parse(tempArray[2]), 
            Changedate = DateTime.Parse(tempArray[3]) };
    }
}