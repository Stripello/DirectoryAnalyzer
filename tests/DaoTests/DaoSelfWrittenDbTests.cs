using DirectoryAnalyzer;
using DirectoryAnalyzer.Dal;
using DirectoryAnalyzer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using System.Linq;

namespace DaoTests
{
    public class DaoSelfWrittenDbTests
    {
        [Fact]
        public void ClassConstructor_ValidDirectory_SuccessfulCreation()
        {
            //Arrange
            var directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + "\\TestData";
            var name = "TestSelfWrittenDb";
            var fullName = directory + "\\" + name + ".txt";
            File.Delete(fullName);

            //Act
            var myDb = new MyFileSystemNodeDaoSelfWrittenDb(directory, name);

            //Assert
            Assert.True(File.Exists(fullName));
            File.Delete(fullName);
        }

        [Fact]
        public void ClassConstructor_InvalidDirectory_Exception()
        {
            var realDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + "\\TestData";
            var wrongDirectory = realDirectory+@"\\";
            var randomizer = new Random();
            while (Directory.Exists(wrongDirectory))
            {
                wrongDirectory += (char)randomizer.Next(65,124);
            }
            Assert.Throws<System.IO.DirectoryNotFoundException>(()=>_=new MyFileSystemNodeDaoSelfWrittenDb(wrongDirectory));
        }

        [Fact]
        public void Add_StaticNodes_Succed()
        {
            //Arrange
            var directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + "\\TestData";
            var name = "DbForAddingStaticNodes";
            var fullName = directory + "\\" + name + ".txt";
            File.Delete(fullName);
            var myDb = new MyFileSystemNodeDaoSelfWrittenDb(directory, name);
            var data = new List<MyFileSystemNode>();

            data.Add(new MyFileSystemNode()
            {
                Id = 1,
                DirectoryName = @"C:\repos\try-samples-main\LINQ",
                ChildrenDirectories = new List<string>() { @"C:\repos\try-samples-main\LINQ\docs",
                    @"C:\repos\try-samples-main\LINQ\src" },
                Content = new List<MyFileInfo>() { new MyFileInfo() { Name = @"C:\repos\try-samples-main\LINQ\readme.md",
                    Extension = ".md",Size=1489,Changedate = DateTime.Parse("19.10.2021 3:30:52") } }
            });

            data.Add(new MyFileSystemNode()
            {
                Id = 2,
                DirectoryName = @"C:\repos\try-samples-main\LINQ\docs",
                ChildrenDirectories = new List<string>(),
                Content = new List<MyFileInfo>() {
                    new MyFileInfo() { Name = @"C:\repos\try-samples-main\LINQ\docs\lazy-equation.md",
                    Extension = ".md",Size=3001,Changedate = DateTime.Parse("19.10.2021 3:30:52") },

                    new MyFileInfo(){ Name = @"C:\repos\try-samples-main\LINQ\docs\query-syntax.md",
                    Extension = ".md",Size = 2068,Changedate =DateTime.Parse("19.10.2021 3:30:52")} }
            });

            data.Add(new MyFileSystemNode()
            {
                Id = 3,
                DirectoryName = @"C:\repos\try-samples-main\LINQ\src",
                ChildrenDirectories = new List<string>(),
                Content = new List<MyFileInfo>() {
                    new MyFileInfo() { Name = @"C:\repos\try-samples-main\LINQ\src\LINQ.csproj",
                    Extension = ".csproj",Size=431,Changedate = DateTime.Parse("19.10.2021 3:30:52") },

                    new MyFileInfo(){ Name = @"C:\repos\try-samples-main\LINQ\src\Program.cs",
                    Extension = ".cs",Size = 2900,Changedate =DateTime.Parse("19.10.2021 3:30:52")} }
            });

            var addedNodesNames = data.Select(x => x.DirectoryName).ToList();
            //Act
            myDb.Add(data);
            var actual = myDb.Read(addedNodesNames).Select(x=>x.ToString());
            var expected = data.Select(x=>x.ToString());

            //duct tape using ToString 
            //Assert
            Assert.Equal(expected, actual);
            File.Delete(fullName);
        }

        [Fact]
        public void Add_NullNode_Fail()
        {
            var directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + "\\TestData";
            var name = "DbForNullNodes";
            var fullName = directory + "\\" + name + ".txt";
            File.Delete(fullName);
            var myDb = new MyFileSystemNodeDaoSelfWrittenDb(directory, name);
            var data = new List<MyFileSystemNode>();
            data.Add(null);
            Assert.Throws<System.NullReferenceException>(()=>myDb.Add(data));
            File.Delete(fullName);
        }
        [Fact]
        public void Add_NullList_Fail()
        {
            var directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + "\\TestData";
            var name = "DbForNullList";
            var fullName = directory + "\\" + name + ".txt";
            File.Delete(fullName);
            var myDb = new MyFileSystemNodeDaoSelfWrittenDb(directory, name);
            List<MyFileSystemNode> data = null;
            Assert.Throws<System.NullReferenceException>(() => myDb.Add(data));
        }

        [Fact]
        public void Read_StaticData_Succed()
        {
            //Arrange
            var directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + "\\TestData";
            var name = "DbForReadingStaticNodes";
            var fullName = directory + "\\" + name + ".txt";

            //Act
            var myDb = new MyFileSystemNodeDaoSelfWrittenDb(directory,name);
            var actual = myDb.Read(new List<string>() { @"C:\repos\try-samples-main\LINQ\src" }).Select(x=>x.ToString());
            var expected = new List<MyFileSystemNode>() { new MyFileSystemNode() { Id =3 ,
            DirectoryName = @"C:\repos\try-samples-main\LINQ\src", ChildrenDirectories = new List<string>(),
            Content = new List<MyFileInfo>(){ 
                new MyFileInfo() {Name = @"C:\repos\try-samples-main\LINQ\src\LINQ.csproj",Extension = ".csproj",
                Size = 431, Changedate = DateTime.Parse("19.10.2021 3:30:52")} ,
                new MyFileInfo(){Name = @"C:\repos\try-samples-main\LINQ\src\Program.cs" , Extension = ".cs",
                Size=2900, Changedate = DateTime.Parse("19.10.2021 3:30:52")}
            } } }.Select(x=>x.ToString());

            //ducttape using converting to strings
            //Assert
            Assert.Equal(expected,actual);
        }

        [Fact]
        public void Read_ArrayOfNulls_Succed()
        {
            //Arrange
            var directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + "\\TestData";
            var name = "DbForReadingStaticNodes";
            var myDb = new MyFileSystemNodeDaoSelfWrittenDb(directory, name);
            var arrayOfNulls = new List<string>() { null, null, null };

            //Act
            var actual = myDb.Read(arrayOfNulls);
            var expected = new List<MyFileSystemNode>() { }; 

            //Assert
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void Read_NullArray_Fail()
        {
            var directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + "\\TestData";
            var name = "DbForReadingStaticNodes";
            var myDb = new MyFileSystemNodeDaoSelfWrittenDb(directory, name);

            Assert.Throws<ArgumentNullException>(()=>myDb.Read(null));
        }
        [Fact]
        public void Update_StaticData_Succed()
        {
            //Arrange
            var directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + "\\TestData";
            var name = "DbForUpdate";
            var fullName = directory + "\\" + name + ".txt";
            File.Delete(fullName);
            File.WriteAllLines(fullName, new string[] { "$1",
            @">C:\repos\try-samples-main\LINQ",
            @"?C:\repos\try-samples-main\LINQ\docs",
            @"?C:\repos\try-samples-main\LINQ\src",
            @"*C:\repos\try-samples-main\LINQ\readme.md*.md*1489*19.10.2021 3:30:52",
            "",
            "$2",
            @">C:\repos\try-samples-main\LINQ\docs",
            @"*C:\repos\try-samples-main\LINQ\docs\lazy-equation.md*.md*3001*19.10.2021 3:30:52",
            @"*C:\repos\try-samples-main\LINQ\docs\query-syntax.md*.md*2068*19.10.2021 3:30:52",
            "",
            "$3",
            @">C:\repos\try-samples-main\LINQ\src",
            @"*C:\repos\try-samples-main\LINQ\src\LINQ.csproj*.csproj*431*19.10.2021 3:30:52",
            @"*C:\repos\try-samples-main\LINQ\src\Program.cs*.cs*2900*19.10.2021 3:30:52",
            ""});

            //Act
            var myBd = new MyFileSystemNodeDaoSelfWrittenDb(directory, name);
            var nodesToUpdate = new List<MyFileSystemNode>() { new MyFileSystemNode() {
                DirectoryName = @"C:\repos\try-samples-main\LINQ\docs",
                ChildrenDirectories = new List<string> { @"C:\repos\try-samples-main\LINQ\docs\KindaNewSubdir" },
            Content = new List<MyFileInfo>(){ 
                new MyFileInfo() { Name = @"C:\repos\try-samples-main\LINQ\docs\KindaNewFile.gif" ,
                Extension = ".gif",Size = 2424,Changedate = DateTime.Parse("03.07.2022 19:48:15")},
                new MyFileInfo () { Name = @"C:\repos\try-samples-main\LINQ\docs\lazy-equation.md",
                Extension = ".md", Size = 3001, Changedate = DateTime.Parse("19.10.2021 3:30:52")}
            }
            } };
            myBd.UpdateDb(nodesToUpdate);
            var actual = File.ReadAllLines(fullName);
            var expected = new string[] { "$1",
            @">C:\repos\try-samples-main\LINQ",
            @"?C:\repos\try-samples-main\LINQ\docs",
            @"?C:\repos\try-samples-main\LINQ\src",
            @"*C:\repos\try-samples-main\LINQ\readme.md*.md*1489*19.10.2021 3:30:52",
            "",
            "$3",
            @">C:\repos\try-samples-main\LINQ\src",
            @"*C:\repos\try-samples-main\LINQ\src\LINQ.csproj*.csproj*431*19.10.2021 3:30:52",
            @"*C:\repos\try-samples-main\LINQ\src\Program.cs*.cs*2900*19.10.2021 3:30:52",
            "",
            "$2",
            @">C:\repos\try-samples-main\LINQ\docs",
            @"?C:\repos\try-samples-main\LINQ\docs\KindaNewSubdir",
            @"*C:\repos\try-samples-main\LINQ\docs\KindaNewFile.gif*.gif*2424*03.07.2022 19:48:15",
            @"*C:\repos\try-samples-main\LINQ\docs\lazy-equation.md*.md*3001*19.10.2021 3:30:52",
            ""
            };

            //Assert
            Assert.Equal(expected, actual);
            File.Delete(fullName);
        }
        [Fact]
        public void DebugTest()
        {

            var directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + "\\TestData";
            var name = "DbForUpdate";
            var fullName = directory + "\\" + name + ".txt";
            File.Delete(fullName);
            var myBd = new MyFileSystemNodeDaoSelfWrittenDb(directory, name);
            var nodesToUpdate = new List<MyFileSystemNode>() { new MyFileSystemNode() {
                DirectoryName = @"C:\repos\try-samples-main\LINQ\docs",
                ChildrenDirectories = new List<string> { @"C:\repos\try-samples-main\LINQ\docs\KindaNewSubdir" },
            Content = new List<MyFileInfo>(){
                new MyFileInfo() { Name = @"C:\repos\try-samples-main\LINQ\docs\KindaNewFile.gif" ,
                Extension = ".gif",Size = 2424,Changedate = DateTime.Parse("03.07.2022 19:48:15")},
                new MyFileInfo () { Name = @"C:\repos\try-samples-main\LINQ\docs\lazy-equation.md",
                Extension = ".md", Size = 3001, Changedate = DateTime.Parse("19.10.2021 3:30:52")}
            }
            } };
            var nodesToUpdate2 = new List<MyFileSystemNode>() { new MyFileSystemNode() {
                DirectoryName = @"C:\repos\try-samples-main\LINQ\docs",
                ChildrenDirectories = new List<string> { @"C:\repos\try-samples-main\LINQ\docs\KindaNewSubdir" },
            Content = new List<MyFileInfo>(){
                new MyFileInfo() { Name = @"C:\repos\try-samples-main\LINQ\docs\KindaNewFile.gif" ,
                Extension = ".gif",Size = 2424,Changedate = DateTime.Parse("03.07.2022 19:48:15")},
                new MyFileInfo () { Name = @"C:\repos\try-samples-main\LINQ\docs\lazy-equation.md",
                Extension = ".md", Size = 3001, Changedate = DateTime.Parse("19.10.2021 3:30:52")}
            }
            } };

            var bo = nodesToUpdate[0].Equals( nodesToUpdate2[0]);
            Assert.True(bo);
        }
    }
}