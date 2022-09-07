using System.Diagnostics.CodeAnalysis;

namespace DirectoryAnalyzer.Models;
public class MyFileInfo : IEqualityComparer<MyFileInfo>
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
    public override int GetHashCode()
    {
        return ($"{ Name}{Extension}{Size}{Changedate}").GetHashCode();
    }

    public static MyFileInfo Parse(string stringToParse)
    {
        var tempArray = stringToParse[1..].Split('*');
        return new MyFileInfo() { Name = tempArray[0], Extension = tempArray[1], Size = long.Parse(tempArray[2]), 
            Changedate = DateTime.Parse(tempArray[3]) };
    }
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            if (this == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        var castObj = obj as MyFileInfo;
        if (castObj.Name != this.Name || castObj.Extension != this.Extension ||
            castObj.Size != this.Size || castObj.Changedate != this.Changedate)
        {
            return false;
        }
        return true;
    }

    bool IEqualityComparer<MyFileInfo>.Equals(MyFileInfo? x, MyFileInfo? y)
    {
        return x.Equals(y);
    }

    int IEqualityComparer<MyFileInfo>.GetHashCode([DisallowNull] MyFileInfo obj)
    {
        return ($"{ obj.Name}{obj.Extension}{obj.Size}{obj.Changedate}").GetHashCode();
    }
}