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
                    var hash = Helper.ComputeXXHash32(file);
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

    public bool CheckSimilarity(string input1, string input2) => Helper.ComputeXXHash32(input1) == Helper.ComputeXXHash32
        (input2);
    
    
}

public class FileHash
{
    public string Filename { get; set; }
    public string Hash { get; set; }
}
