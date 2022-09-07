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
			testData.Add(new MyFileInfo() { Name = $"plugName{i}", Size = temp });
		}

		// Act
		var actual = DirectoryOperation.GetBiggestFiles(testData);
		var actualIsOrdered = true;
		for (var i = 0; i < actual.Count-1 ; i++)
        {
			if (actual[i].Size < actual[i + 1].Size)
            {
				actualIsOrdered = false;
				break;
            }
        }

		// Assert
		Assert.True(actualIsOrdered);

	}

	[Fact]
	public void GetBiggestFiles_EmptyFiles_Succeed()
	{
		// Arrange
		var testData = new List<MyFileInfo>();

		// Act
		var actual = DirectoryOperation.GetBiggestFiles(testData);

		//Assert
		Assert.Equal(0, actual.Count);
	}

	[Fact]
	public void GetBiggestFiles_InvalidData_Failed()
	{
		Assert.Throws<ArgumentNullException>(() => DirectoryOperation.GetBiggestFiles(null));
	}
	#endregion

	#region GetCopiesTests
	[Fact]
	//cleanup
	public void GetCopies_threeAndTwoCopies_Succeed()
	{
		//Arrange
		var directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + "\\TestData\\";
		var testData = new List<MyFileInfo>();
		testData.Add(new MyFileInfo(directory + "0.txt"));
		testData.Add(new MyFileInfo(directory + "1.txt"));
		testData.Add(new MyFileInfo(directory + "2.txt"));
		testData.Add(new MyFileInfo(directory + "3.txt"));
		testData.Add(new MyFileInfo(directory + "4.txt"));
		testData.Add(new MyFileInfo(directory + "5.txt"));
		testData.Add(new MyFileInfo(directory + "6.txt"));
		testData.Add(new MyFileInfo(directory + "7.txt"));
		//Act
		var actual = DirectoryOperation.GetCopies(testData, 0);
		//Assert
		Assert.Equal(actual, new List<List<MyFileInfo>>() { new List<MyFileInfo> { testData[5],testData[6]}
		, new List<MyFileInfo> { testData[1],testData[2],testData[4]} });
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
		Assert.Equal(actual, new List<List<MyFileInfo>>() { });
	}

	[Fact]
	public void GetCopies_EmptyInput_Succeed()
	{
		//Arrange
		var testData = new List<MyFileInfo>();
		//Act
		var actual = DirectoryOperation.GetCopies(testData);
		//Assert
		Assert.Equal(actual, new List<List<MyFileInfo>>() { });
	}
	[Fact]
	public void GetCopies_invalidData_Failed()
	{
		Assert.Throws<ArgumentNullException>(() => DirectoryOperation.GetCopies(null));
	}
	#endregion

	#region GetOldestFilesTests
	[Fact]
	public void GetOldestFiles_RandomFiles_Succeed()
	{
		//Arrange
		var random = new Random();
		var testData = new List<MyFileInfo>();
		const int testCases = 10000;
		for (int i = 0; i < testCases; i++)
		{
			var randomDate = new DateTime(random.Next(1980, 2023), random.Next(1, 13), random.Next(1, 28));
			testData.Add(new MyFileInfo() { Changedate = randomDate });
		}

		//Act
		var actual = DirectoryOperation.GetOldestFiles(testData);
		var actualIsOrdered = true;
		for (int i = 0; i < actual.Count - 1; i++)
		{
			if (actual[i].Changedate > actual[i + 1].Changedate)
			{
				actualIsOrdered = false;
				break;
			}
		}

		//Assert
		Assert.True(actualIsOrdered);
	}
	[Fact]
	public void GetOldestFiles_EmptyData_Succeed()
	{
		// Arrange
		var testData = new List<MyFileInfo>();

		// Act
		var actual = DirectoryOperation.GetOldestFiles(testData);

		//Assert
		Assert.Equal(0, actual.Count);
	}
	[Fact]
	public void GetOldestFiles_InvalidData_Fail()
	{
		Assert.Throws<ArgumentNullException>(() => DirectoryOperation.GetOldestFiles(null));
	}
	#endregion

	#region GetFrequentExtension
	[Fact]
    public void GetFrequentExtension_RandomExtensions_Succed()
    {
		//Arrange
		var randomizer = new Random();
		const int amountOfTestObjects = 100000;
		//according to ASCII2 table min max numeric values relevant to lowcase english letters
		const int asciiMin = 97;
		const int asciiMax = 122;
		var testData = new List<MyFileInfo>();
		for (int i = 0; i < amountOfTestObjects; i++)
        {
			testData.Add(new MyFileInfo() { Extension = "." + (char)randomizer.Next(asciiMin, asciiMax + 1) +
				(char)randomizer.Next(asciiMin, asciiMax + 1) + (char)randomizer.Next(asciiMin, asciiMax + 1) });
        }

		//Act
		var actual = DirectoryOperation.GetFrequentExtension(testData);
        var actualIsOrdered = true;
		for (int i = 0; i < actual.Count - 1; i++)
        {
			if (actual[i].Item2 < actual[i + 1].Item2)
            {
				actualIsOrdered = false;
				break;
            }
        }

        //Assert
        Assert.True(actualIsOrdered);
    }

	[Fact]
	public void GetFrequentExtension_EmptyData_Succed()
    {
		//Arrange
		var data = new List<MyFileInfo>();

		//Act
		var actual = DirectoryOperation.GetFrequentExtension(data);

		//Assert
		Assert.Empty(actual);
    }

	[Fact]
    public void GetFrequentExtension_InvalidData_Failed()
    {
		Assert.Throws<ArgumentNullException>(() => DirectoryOperation.GetFrequentExtension(null));
	}
	#endregion

	#region GetBiggestDerictories
	[Fact]
	public void GetBiggestDirectories_RandomFileSystemNode_Succed()
	{
		//Arrange
		var randomizer = new Random();
		const int amountOfTestObjects = 100;
		const int maxRandomFiles = 20;
		const int maxFileSize = int.MaxValue;
		var random = new Random();
		var testData = new List<MyFileSystemNode>();
		for (int i = 0; i < amountOfTestObjects; i++)
        {
			testData.Add(new MyFileSystemNode() { Content = new List<MyFileInfo>()});
			var currentAmountOfFiles = random.Next(maxRandomFiles);
			for (int j = 0; j < currentAmountOfFiles; j++)
            {
				testData[i].Content.Add(new MyFileInfo() {Size = random.Next(maxFileSize) });
            }
        }

		//Act
		var actual = DirectoryOperation.GetBiggestDirectories(testData);
		var actualIsOrdered = true;
		for (int i = 0; i < actual.Count-1; i++)
        {
			if (actual[i].Item2 < actual[i + 1].Item2)
            {
				actualIsOrdered = false;
				break;
            }
        }

		//Assert
		Assert.True(actualIsOrdered);
	}

	[Fact]
	public void GetBiggestDirectories_EmptyData_Succed()
	{
		//Arrange
		var data = new List<MyFileSystemNode>();

		//Act
		var actual = DirectoryOperation.GetBiggestDirectories(data);

		//Assert
		Assert.Empty(actual);
	}

	[Fact]
	public void GetBiggestDirectories_InvalidData_Failed()
	{
        Assert.Throws<NullReferenceException>(() => DirectoryOperation.GetBiggestDirectories(null));
	}
	#endregion
}