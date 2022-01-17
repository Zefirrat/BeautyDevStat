namespace BeautyDevStat.Git.Interfaces;

public interface IGitStatisticService
{
    Task<int> GetCommittedLinesCountAsync(string path, CancellationToken cancellationToken = default);
}