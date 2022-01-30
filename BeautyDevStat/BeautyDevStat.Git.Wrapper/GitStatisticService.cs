using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BeautyDevStat.Git.Interfaces;

namespace BeautyDevStat.Git.Wrapper;

public class GitStatisticService : IGitStatisticService
{
    public async Task<int> GetCommittedLinesCountAsync(string path, CancellationToken cancellationToken = default)
    {
        var author = "Zefirrat";
        var countCommand =
            $"git log --numstat --pretty=\"%H\" --author=\"{author}\"";
        var result =
            await CommandExecuter.ExecuteCmdFromDirectoryAsync($"{countCommand}", path, cancellationToken);

        var parsed = ParseAddedAndDeleted(result);
        return parsed.Item1 + parsed.Item2;
    }

    public static (int, int) ParseAddedAndDeleted(string input)
    {
        var addedKey = "added";
        var deletedKey = "deleted";
        var regexPattern = @$"(^(?<{addedKey}>[\d]+)[\s]{{1}}(?<{deletedKey}>[\d]+)[\s]{{1}}.+$)";
        var added = 0;
        var deleted = 0;


        var regex = new Regex(regexPattern);

        foreach (var inputLine in input.Split("\n"))
        {
            var match = regex.Match(inputLine);

            if (match.Groups.TryGetValue(addedKey, out _))
            {
                added += int.Parse(match.Groups[addedKey].Value);
                deleted += int.Parse(match.Groups[deletedKey].Value);
            }
        }


        return (added, deleted);
    }
}