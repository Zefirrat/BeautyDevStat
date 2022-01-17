using System.Diagnostics;

namespace BeautyDevStat.Git.Wrapper;

public static class CommandExecuter
{
    public static Task<string> ExecuteCmdAsync(string command, CancellationToken cancellationToken = default)
    {
        return ExecuteCmdFromDirectoryAsync(command, string.Empty, cancellationToken);
    }

    public static Task<string> ExecuteCmdFromDirectoryAsync(string command, string startupPath,
        CancellationToken cancellationToken = default)
    {
        // source: https://stackoverflow.com/questions/1469764/run-command-prompt-commands
        var process = new System.Diagnostics.Process();
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
            FileName = "cmd.exe",
            Arguments = $"/C {command}",
            // source: https://stackoverflow.com/questions/37694532/standardout-has-not-been-redirected-or-the-process-hasnt-started-yet-when-rea
            RedirectStandardOutput = true
        };

        if (!string.IsNullOrEmpty(startupPath))
        {
            startInfo.WorkingDirectory = startupPath;
        }

        process.StartInfo = startInfo;
        process.Start();

        var result = ReadOutput(process, cancellationToken);

        return result;
    }

    private static async Task<string> ReadOutput(Process process, CancellationToken cancellationToken = default)
    {
        var result = string.Empty;
        while (!process.StandardOutput.EndOfStream)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return result;
            }

            result += await process.StandardOutput.ReadLineAsync();
        }

        return result;
    }
}