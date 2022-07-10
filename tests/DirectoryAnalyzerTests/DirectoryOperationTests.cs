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
		const int testObjects = 10000;
		var testData = Enumerable.Repeat(new MyFileInfo(),testObjects).ToList();
		for (int i = 0; i < testObjects; i++)
        {
			var currentRandomValue = randomizer.Next(31);
            switch (currentRandomValue)
            {
                case 0:
					testData[i].Extension = ".zip";
					break;
				case 1:
					testData[i].Extension = ".xps";
					break;
				case 2:
					testData[i].Extension = ".bin";
					break;
				case 3:
					testData[i].Extension = ".xltm";
					break;
				case 4:
					testData[i].Extension = ".xlt";
					break;
				case 5:
					testData[i].Extension = ".xlsx";
					break;
				case 6:
					testData[i].Extension = ".xls";
					break;
				case 7:
					testData[i].Extension = ".wms";
					break;
				case 8:
					testData[i].Extension = ".wmv";
					break;
				case 9:
					testData[i].Extension = ".wmd";
					break;
				case 10:
					testData[i].Extension = ".wav";
					break;
				case 11:
					testData[i].Extension = ".txt";
					break;
				case 12:
					testData[i].Extension = ".tmp";
					break;
				case 13:
					testData[i].Extension = ".tif";
					break;
				case 14:
					testData[i].Extension = ".sys";
					break;
				case 15:
					testData[i].Extension = ".rtf";
					break;
				case 16:
					testData[i].Extension = ".rar";
					break;
				case 17:
					testData[i].Extension = ".pst";
					break;
				case 18:
					testData[i].Extension = ".psd";
					break;
				case 19:
					testData[i].Extension = ".pptx";
					break;
				case 20:
					testData[i].Extension = ".png";
					break;
				case 21:
					testData[i].Extension = ".pdf";
					break;
				case 22:
					testData[i].Extension = ".mpeg";
					break;
				case 23:
					testData[i].Extension = ".mp4";
					break;
				case 24:
					testData[i].Extension = ".mp3";
					break;
				case 25:
					testData[i].Extension = ".mov";
					break;
				case 26:
					testData[i].Extension = ".midi";
					break;
				case 27:
					testData[i].Extension = ".jpg";
					break;
				case 28:
					testData[i].Extension = ".doc";
					break;
				case 29:
					testData[i].Extension = ".dll";
                    break;
                default:
					testData[i].Extension = ".exe";
					break;
            }
        }

		//Act
		var actual = DirectoryOperation.GetFrequentExtension(testData);
		var expected = actual.OrderBy(x => x).ToArray();

		//Assert
		Assert.Equal(expected, actual);
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
		const int maxFileSize = 2 ^ 32;
		var random = new Random();
		var testData = Enumerable.Repeat(new MyFileSystemNode() { Content = new List<MyFileInfo>()}, amountOfTestObjects).ToList();
		for (int i = 0; i < amountOfTestObjects; i++)
        {
			var currentAmountOfFiles = random.Next(maxRandomFiles);
			for (int j = 0; j < currentAmountOfFiles; j++)
            {
				testData[i].Content.Add(new MyFileInfo() {Size = random.Next(maxFileSize) });
            }
        }

		//Act
		var actual = DirectoryOperation.GetBiggestDirectories(testData);
		var expected = actual.OrderBy(x => x).ToList();

		//Assert
		Assert.Equal(expected, actual);
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