using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BD2Tools;

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
    
    public static void SortCutsceneBGs(string ogPath, string assetPath)
    {
        // assuming there's no number before id in path
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
}