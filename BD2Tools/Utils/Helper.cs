
using System.Diagnostics;
using System.Text.RegularExpressions;
using Extensions.Data;

namespace BD2Tools.Utils;

public class Helper
{
    public static List<string> GetFilesBasedOnDate(string input, int option = 2, string specificDate = @"2025-05-01",
        int dateBack = 1)
    {
        var result = new List<string>();

        if (!Directory.Exists(input))
            return result;

        DateTime fromDate;

        if (option == 1)
        {
            if (!DateTime.TryParse(specificDate, out fromDate))
                throw new ArgumentException("Invalid specificDate format. Use yyyy-MM-dd or similar.");
        }
        else if (option == 2)
            fromDate = DateTime.Now.AddDays(-dateBack);
        else
        {
            throw new ArgumentException("Invalid option. Must be 1 or 2.");
        }

        var allFiles = Directory.GetFiles(input, "*__data", SearchOption.AllDirectories);

        foreach (var file in allFiles)
        {
            var lastWrite = File.GetLastWriteTime(file);
            if (lastWrite >= fromDate)
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

    public static string ComputeXXHash32(string filePath)
    {
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4 * 1024 * 1024);
        XXHash.State32 state = XXHash.CreateState32();
        XXHash.UpdateState32(state, stream);
        return XXHash.DigestState32(state).ToString();
    }

    public static void SortCutsceneBGs(string ogPath, string assetPath)
    {
        var idList = Directory.GetDirectories(ogPath, "*cutscene_char*");
        int[] id = idList.Select(x => int.Parse(Regex.Match(Path.GetFileNameWithoutExtension(x), @"\d+").Value)).ToArray();
        Regex bgCheck = new Regex(@"^((cha)r?)?[0-9]{5,6}");

        var bgFirstFilter = Directory.GetFiles(assetPath, "*.png", SearchOption.AllDirectories)
            .Where(x => bgCheck.IsMatch(Path.GetFileNameWithoutExtension(x).ToLower()))
            .ToList();
        Console.WriteLine("--------------------");
        Console.WriteLine(bgFirstFilter.Count);
        foreach (var bg in bgFirstFilter)
        {
            string expectedSubfolder = $"cutscene_char{int.Parse(Regex.Match(Path.GetFileNameWithoutExtension(bg), @"\d+").Value).ToString("D6")}";
            if (Directory.Exists(Path.Combine(ogPath, expectedSubfolder)))
            {
                Console.WriteLine($"Move {bg} -> {Path.Combine(ogPath, expectedSubfolder)}");
                File.Copy(bg, Path.Combine(ogPath, expectedSubfolder, Path.GetFileName(bg)), true);
            }
        }
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
}
