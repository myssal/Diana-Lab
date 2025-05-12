using System.Diagnostics;

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
}