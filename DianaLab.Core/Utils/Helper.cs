
using System.Diagnostics;
using System.Text.RegularExpressions;
using Extensions.Data;

namespace DianaLab.Core.Utils;

public class Helper
{
    public static List<string> GetFilesBasedOnDate(string input, string startDate, string endDate)
    {
        var result = new List<string>();

        if (!Directory.Exists(input))
            return result;

        if (!DateTime.TryParse(startDate, out DateTime fromDate) || !DateTime.TryParse(endDate, out DateTime toDate))
        {
            throw new ArgumentException("Invalid date format. Use yyyy-MM-dd or similar.");
        }

        fromDate = fromDate.Date;
        
        toDate = toDate.Date.AddDays(1).AddTicks(-1);

        Console.WriteLine($"Filter files from {fromDate} to {toDate}.");

        var allFiles = Directory.GetFiles(input, "*__data", SearchOption.AllDirectories);

        foreach (var file in allFiles)
        {
            var lastWrite = File.GetLastWriteTime(file);
            if (lastWrite >= fromDate && lastWrite <= toDate)
            {
                result.Add(file);
            }
        }

        return result;
    }

    public static void RunCommand(string exePath, string arguments)
    {
        ProcessStartInfo info = new ProcessStartInfo(exePath);
        info.Arguments = arguments;
        info.UseShellExecute = false;
        var process = new Process();
        process.StartInfo = info;
        process.Start();
        process.WaitForExit();
    }

    public static async Task RunCommandAsync(string exePath, string arguments)
    {
        ProcessStartInfo info = new ProcessStartInfo(exePath);
        info.Arguments = arguments;
        info.UseShellExecute = false;
        var process = new Process();
        process.StartInfo = info;
        process.Start();
        await process.WaitForExitAsync();
    }

    public static string ComputeXXHash32(string filePath)
    {
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4 * 1024 * 1024);
        XXHash.State32 state = XXHash.CreateState32();
        XXHash.UpdateState32(state, stream);
        return XXHash.DigestState32(state).ToString();
    }

    public static List<string> GetLowestLevelSubfolders(string rootFolder)
    {
        var allDirs = Directory.GetDirectories(rootFolder, "*", SearchOption.AllDirectories);
        var lowestLevelDirs = allDirs
            .Where(dir => Directory.GetDirectories(dir).Length == 0)
            .ToList();

        return lowestLevelDirs;
    }

    public static void CopyAllFiles(string sourceDir, string destinationDir)
    {
        if (!Directory.Exists(sourceDir))
        {
            throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");
        }

        foreach (string filePath in Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories))
        {
            string relativePath = Path.GetRelativePath(sourceDir, filePath);
            string destPath = Path.Combine(destinationDir, relativePath);
            string destDir = Path.GetDirectoryName(destPath);
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }
            File.Copy(filePath, destPath, overwrite: true);
        }
    }
    
    public static void CleanDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            Console.WriteLine($"Cleaning directory: {path}");
            var directory = new DirectoryInfo(path);
            foreach (var file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (var subdirectory in directory.GetDirectories())
            {
                subdirectory.Delete(true);
            }
        }
        else
        {
            Directory.CreateDirectory(path);
        }
    }
    
    public static void LogAppend(string text, string filePath) => File.AppendAllText(filePath, text + Environment
        .NewLine);

    public static bool CheckValidFolder(string path) => Directory.Exists(path);
}
