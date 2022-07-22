using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DirectoryAnalyzer;
using DirectoryAnalyzer.Dal;
using LiteDB;
using DirectoryAnalyzer.Models;
using System.IO;
using LiteDB;

namespace DaoTests
{
    public class DaoLightDbTests
    {
        #region Add
        [Fact]
        public void Add_StaticElements_Succed()
        {
            //Arrange
            var directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + "\\TestData";
            var name = "LiteDbForAddingNodes";
            var fullName = $"{directory}\\{name}.db";
            File.Delete(fullName);
            var testDb = new MyFileSystemNodeDaoLightDb(directory,name);
            var dataToAdd = new List<MyFileSystemNode> {
                new MyFileSystemNode{Id =1 ,DirectoryName = @"C:\repos\try-samples-main\LINQ",
                    ChildrenDirectories = new List<string> { @"C:\repos\try-samples - main\LINQ\docs",
                        @"C:\repos\try-samples-main\LINQ\readme.md*.md*1489*19.10.2021 3:30:52" } ,
                Content = new List<MyFileInfo>{ new MyFileInfo(){Name = @"C:\repos\try-samples-main\LINQ\readme.md",
                    Extension =".md",Size =1489 , Changedate = DateTime.Parse("19.10.2021 3:30:52") } } },

                new MyFileSystemNode{Id =2 ,DirectoryName =@"C:\repos\try-samples-main\LINQ\docs",
                ChildrenDirectories = new List<string>{ },
                Content = new List<MyFileInfo>{ new MyFileInfo() { Name = @"C:\repos\try-samples-main\LINQ\docs\lazy-equation.md",
                Extension = ".md",Size = 3001, Changedate = DateTime.Parse("19.10.2021 3:30:52")},
                new MyFileInfo(){ Name = @"C:\repos\try-samples-main\LINQ\docs\query-syntax.md",Extension = ".md",
                Size = 2068,Changedate = DateTime.Parse("19.10.2021 3:30:52")} } },

                new MyFileSystemNode{Id =3 ,DirectoryName =@"C:\repos\try-samples-main\LINQ\src",
                ChildrenDirectories = new List<string>{ },
                Content = new List<MyFileInfo>{ new MyFileInfo { Name = @"C:\repos\try-samples-main\LINQ\src\LINQ.csproj" ,
                Extension = ".csproj", Size = 431, Changedate = DateTime.Parse("19.10.2021 3:30:52")} ,
                new MyFileInfo{Name = @"C:\repos\try-samples-main\LINQ\src\Program.cs" ,Extension = ".cs",
                Size =2900, Changedate = DateTime.Parse("19.10.2021 3:30:52")} } }
                };
            var directoriesToAdd = dataToAdd.Select(d => d.DirectoryName).ToList();

            //Act
            testDb.Add(dataToAdd);
            var actual = testDb.Read(directoriesToAdd);

            //Assert
            Assert.Equal(dataToAdd, actual);
            File.Delete(fullName);
        }

        [Fact]
        public void Add_EmptyNode_Succed()
        {
            //Arrange
            var directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + "\\TestData";
            var name = "LiteDbForAddingNodes";
            var fullName = $"{directory}\\{name}.db";
            File.Delete(fullName);
            //ducttape
            using (_ = new LiteDatabase(fullName))
            {
            }
            var expected = File.ReadAllBytes(fullName);

            var dataToAdd = new List<MyFileSystemNode> { };
            var testDb = new MyFileSystemNodeDaoLightDb(directory, name);

            //Act
            testDb.Add(dataToAdd);

            //Assert
            var actual = File.ReadAllBytes(fullName);
            Assert.Equal(expected,actual);
            File.Delete(fullName);
        }

        [Fact]
        public void Add_NullNode_Fail()
        {
            //Arrange
            var directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + "\\TestData";
            var name = "LiteDbForAddingNodes";
            var fullName = $"{directory}\\{name}.db";
            File.Delete(fullName);
            //ducttape
            using (_ = new LiteDatabase(fullName))
            {
            }
            List<MyFileSystemNode> dataToAdd = null;
            var testDb = new MyFileSystemNodeDaoLightDb(directory, name);

            //Assert
            Assert.Throws<NullReferenceException>(()=> testDb.Add(dataToAdd));
            File.Delete(fullName);
        }
        #endregion

        #region Read
        [Fact]
        public void Read_StaticDb_Succed()
        {
            //Arrange
            var directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + "\\TestData";
            var name = "LiteDbForReading";
            var testDb = new MyFileSystemNodeDaoLightDb(directory, name);
            var expected = new List<MyFileSystemNode> {

                new MyFileSystemNode{Id =2 ,DirectoryName =@"C:\repos\try-samples-main\LINQ\docs",
                ChildrenDirectories = new List<string>{ },
                Content = new List<MyFileInfo>{ new MyFileInfo() { Name = @"C:\repos\try-samples-main\LINQ\docs\lazy-equation.md",
                Extension = ".md",Size = 3001, Changedate = DateTime.Parse("19.10.2021 3:30:52")},
                new MyFileInfo(){ Name = @"C:\repos\try-samples-main\LINQ\docs\query-syntax.md",Extension = ".md",
                Size = 2068,Changedate = DateTime.Parse("19.10.2021 3:30:52")} } }

                };
            var nameOfDir = expected.Select(x => x.DirectoryName).ToList();

            //Act
            var actual = testDb.Read(nameOfDir);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Read_NoMatchInDb_Succed()
        {
            //Arrange
            var directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + "\\TestData";
            var name = "LiteDbForReading";
            var testDb = new MyFileSystemNodeDaoLightDb(directory, name);

            //Act
            var actual = testDb.Read(new List<string> { "/"});
            var expected = new List<MyFileSystemNode> { };

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Read_EmtyInput_Succed()
        {
            //Arrange
            var directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + "\\TestData";
            var name = "LiteDbForReading";
            var testDb = new MyFileSystemNodeDaoLightDb(directory, name);

            //Act
            var actual = testDb.Read(new List<string> { });
            var expected = new List<MyFileSystemNode> { };

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Read_NullInput_Succed()
        {
            //Arrange
            var directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + "\\TestData";
            var name = "LiteDbForReading";
            var testDb = new MyFileSystemNodeDaoLightDb(directory, name);

            //Act
            var actual = testDb.Read(null);
            var expected = new List<MyFileSystemNode> { };

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}
