using System.Threading.Tasks;
using BeautyDevStat.Git.Wrapper;
using NUnit.Framework;

namespace BeautyDevStat.Tests;

public class GitStatisticServiceTests
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public async Task Test_GitStatisticService_ShouldExecuteSuccess()
    {
        var configuration = ConfigurationLoader.GetConfiguration(nameof(GitStatisticServiceTests));
        
        var service = new GitStatisticService();
        var result = await service.GetCommittedLinesCountAsync(configuration.GetSection("TestGitFolderPath").Value);
    }

    [TestCase("1 1 Test1", 1, 1)]
    [TestCase("1 2 Test2", 1, 2)]
    [TestCase("Test3", 0, 0)]
    [TestCase("25 Test4", 0, 0)]
    [TestCase("1 2 Test5\n3 4 Test5", 4, 6)]
    public async Task Test_GitStatisticService_ParseAddedAndDeleted_ShouldParse(string inputString, int addedExpected, int deletedExpected)
    {
        var (actualAdded, actualDeleted) = GitStatisticService.ParseAddedAndDeleted(inputString);

        Assert.AreEqual(addedExpected, actualAdded);
        Assert.AreEqual(deletedExpected, actualDeleted);
    }
}