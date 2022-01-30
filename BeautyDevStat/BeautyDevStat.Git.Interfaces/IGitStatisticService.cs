using System.Threading;
using System.Threading.Tasks;

namespace BeautyDevStat.Git.Interfaces;

public interface IGitStatisticService
{
    Task<int> GetCommittedLinesCountAsync(string path, CancellationToken cancellationToken = default);
}