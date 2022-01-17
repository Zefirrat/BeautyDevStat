using BeautyDevStat.Git.Interfaces;

namespace BeautyDevStat.Git.Wrapper;

public class GitStatisticService : IGitStatisticService
{
    public async Task<int> GetCommittedLinesCountAsync(string path, CancellationToken cancellationToken = default)
    {
        var author = "Zefirrat";
        var countCommand =
            $"git log --numstat --pretty=\"%H\" --author=\"{author}\" | awk 'NF==3 {{plus+=$1; minus+=$2}} END {{printf(\"+%d, -%d\n\", plus, minus)}}'";
        var result =
            await CommandExecuter.ExecuteCmdFromDirectoryAsync($"{countCommand} {author}", path, cancellationToken);
        return int.Parse(result);
    }
}