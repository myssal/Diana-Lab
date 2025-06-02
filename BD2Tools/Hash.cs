using System.Collections.Concurrent;
using System.Diagnostics;
using Extensions.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BD2Tools;

public partial class CDN
{
    public void CalculateHash(string input, string output)
    {
        Logger.LogInformation("Starting xxHash64 calculation...");

        var fileHashList = new ConcurrentBag<FileHash>();
        var files = Directory.GetFiles(input, "*.bundle", SearchOption.AllDirectories);
        int processorCount = Environment.ProcessorCount;

        Parallel.ForEach(
            files,
            new ParallelOptions { MaxDegreeOfParallelism = processorCount },
            file =>
            {
                try
                {
                    Logger.LogInformation($"Calculating hash for file: {file}");
                    var hash = ComputeXXHash64(file);
                    var fileHash = new FileHash
                    {
                        Filename = Path.GetFileName(file),
                        Hash = hash
                    };
                    fileHashList.Add(fileHash);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Failed to process file: {file}");
                }
            });

        Logger.LogInformation("Writing hash values to JSON file...");

        var sortedList = fileHashList.ToList();
        sortedList.Sort((a, b) => string.Compare(a.Filename, b.Filename, StringComparison.Ordinal));

        string jsonOutput = JsonConvert.SerializeObject(sortedList, Formatting.Indented);
        File.WriteAllText(output, jsonOutput);
        Logger.LogInformation("Output written to {OutputFile}", output);
    }

    private string ComputeXXHash64(string filePath)
    {
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4 * 1024 * 1024);
        XXHash.State32 state = XXHash.CreateState32();  
        XXHash.UpdateState32(state, stream);  
        return XXHash.DigestState32(state).ToString(); 
    }
}

public class FileHash
{
    public string Filename { get; set; }
    public string Hash { get; set; }
}
