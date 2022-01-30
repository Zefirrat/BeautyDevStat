using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BeautyDevStat.Git.Wrapper;

public static class CommandExecuter
{
    public static Task<string> ExecuteCmdAsync(string command, CancellationToken cancellationToken = default)
    {
        return ExecuteCmdFromDirectoryAsync(command, string.Empty, cancellationToken);
    }

    public static async Task<string> ExecuteCmdFromDirectoryAsync(string command, string startupPath,
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
            RedirectStandardOutput = true,
            UseShellExecute = false
        };

        if (!string.IsNullOrEmpty(startupPath))
        {
            startInfo.WorkingDirectory = startupPath;
        }

        process.StartInfo = startInfo;
        
        process.Start();

        var result = await ReadOutput(process, cancellationToken);

        return result;
    }

    private static async Task<string> ReadOutput(Process process, CancellationToken cancellationToken = default)
    {
        var result = string.Empty;
        
        async Task AppendResult()
        {
            result += await process.StandardOutput.ReadToEndAsync();
        }

        if (cancellationToken.IsCancellationRequested)
        {
            return result;
        }

        await AppendResult();

        while (!process.StandardOutput.EndOfStream)
        {
            await AppendResult();
        }

        await process.WaitForExitAsync(cancellationToken);
        return result;
    }
}