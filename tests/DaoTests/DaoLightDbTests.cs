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

namespace DaoTests
{
    public class DaoLightDbTests
    {
        [Fact]
        public void TryMock()
        {
            var mock = new Mock<LiteDatabase>();
            mock.SetReturnsDefault<MyFileSystemNode>(new MyFileSystemNode());
            MyFileSystemNodeDaoLightDb db = mock;
            Assert.
        }
    }
}
