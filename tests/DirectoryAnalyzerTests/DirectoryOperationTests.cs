using DirectoryAnalyzer;
using DirectoryAnalyzer.Models;
using DirectoryOperationServices;

namespace DirectoryAnalyzerTests;

public class DirectoryOperationTests
{
    #region GetBiggestFilesTests
    [Fact]
	public void GetBiggestFiles_RandomFiles_Succeed()
	{
		// Arrange
		var testData = new List<MyFileInfo>();
		var random = new Random();
		int temp;
		var testDataLength = 200;
		for (var i = 0; i < testDataLength; i++)
		{
			temp = random.Next(1024 * 1024 * 3);
			testData.Add(new MyFileInfo() { Name = $"plugName{i}",Size = temp});
		}
		
		// Act
		var actual = DirectoryOperation.GetBiggestFiles(testData);

		// Assert
		for (var i = 0; i < actual.Count - 1; i++)
        {
			Assert.True(actual[i].Size >= actual[i + 1].Size);
		}
			
	}
	
	[Fact]
	public void GetBiggestFiles_EmptyFiles_Succeed()
	{
		// Arrange
		var testData = new List<MyFileInfo>();
		
		// Act
		var actual = DirectoryOperation.GetBiggestFiles(testData);

		Assert.Equal(0, actual.Count);
	}
	
	[Fact]
	public void GetBiggestFiles_InvalidData_Failed()
	{
		Assert.Throws<NullReferenceException>(() => DirectoryOperation.GetBiggestFiles(null));
	}
    #endregion

    #region GetCopies
	[Fact]
    public void GetCopies_threeCopies_Succeed()
    {
		//Arrange
		var testData = new List<MyFileInfo>();
		testData.Add(new MyFileInfo() { Name = "copy", Extension = ".bin", Size = 42*1024 });
		testData.Add(new MyFileInfo() { Size = 1025 });
		testData.Add(new MyFileInfo() { Size = 20000 });
		testData.Add(new MyFileInfo() { Size = 0 });
		testData.Add(new MyFileInfo() { Size = 0 });
		testData.Add(new MyFileInfo() { Size = 1026 });
		testData.Add(new MyFileInfo() { Size = 1027 });
		testData.Add(new MyFileInfo() { Size = 1028 });
		testData.Add(new MyFileInfo() { Name = "copy", Extension = ".bin", Size = 42 * 1024 });
		testData.Add(new MyFileInfo() { Size = 3*256*256 });
		testData.Add(new MyFileInfo() { Size = 4 * 256 * 256 });
		testData.Add(new MyFileInfo() { Size = 5 * 256 * 256 });
		testData.Add(new MyFileInfo() { Size = 6 * 256 * 256 });
		testData.Add(new MyFileInfo() { Size = 7* 256 * 256 });
		testData.Add(new MyFileInfo() { Name = "copy", Extension = ".bin", Size = 42 * 1024 });
		//Act
		var actual = DirectoryOperation.GetCopies(testData);
		//Assert
		Assert.Equal(actual,new List<List<MyFileInfo>>() { new List<MyFileInfo> { testData[0],testData[8],testData[14]} });
	}

	[Fact]
	public void GetCopies_noCopies_Succeed()
	{
		//Arrange
		var testData = new List<MyFileInfo>();
		testData.Add(new MyFileInfo() { Size = 1025 });
		testData.Add(new MyFileInfo() { Size = 20000 });
		testData.Add(new MyFileInfo() { Size = 1 });
		testData.Add(new MyFileInfo() { Size = 0 });
		testData.Add(new MyFileInfo() { Size = 1026 });
		testData.Add(new MyFileInfo() { Size = 1027 });
		testData.Add(new MyFileInfo() { Size = 1028 });
		testData.Add(new MyFileInfo() { Size = 3 * 256 * 256 });
		testData.Add(new MyFileInfo() { Size = 4 * 256 * 256 });
		testData.Add(new MyFileInfo() { Size = 5 * 256 * 256 });
		testData.Add(new MyFileInfo() { Size = 6 * 256 * 256 });
		testData.Add(new MyFileInfo() { Size = 7 * 256 * 256 });
		testData.Add(new MyFileInfo() { Name = "copy", Extension = ".bin", Size = 42 * 1024 });
		//Act
		var actual = DirectoryOperation.GetCopies(testData);
		//Assert
		Assert.Equal(actual, new List<List<MyFileInfo>>() {});
	}
	[Fact]
	public void GetCopies_invalidData_Failed()
    {
		Assert.Throws<ArgumentNullException>(() => DirectoryOperation.GetCopies(null));
    }
	#endregion


}