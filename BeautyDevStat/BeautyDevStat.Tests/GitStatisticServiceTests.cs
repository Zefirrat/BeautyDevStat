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
}