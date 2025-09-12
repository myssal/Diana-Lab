
using System.Collections.Concurrent;
using Extensions.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DianaLab.Core.Model;
using DianaLab.Core.Utils;

namespace DianaLab.Core.Services;

public class CDNService : LoggedService<CDNService>
{
    private const string Host = "https://bd2-cdn.akamaized.net/ServerData";
    private const string Resolution = "HD";
    private readonly long version;
    private readonly string catalogUrl;
    private readonly string baseUrl;
    private List<string> downloadList;
    private readonly List<string> failedDownloads;

    public CDNService(ILogger<CDNService> logger, long version) : base(logger)
    {
        this.version = version;
        baseUrl = $"{Host}/StandaloneWindows64/{Resolution}/{version}";
        catalogUrl = $"{baseUrl}/catalog_alpha.json";
        downloadList = new List<string>();
        failedDownloads = new List<string>();
    }

    public static async Task<string?> DownloadFileAsync(string url, string? outputPath = null)
    {
        HttpClient client = new HttpClient();
        Console.WriteLine($"Downloading file from {url}");
        string data = await client.GetStringAsync(url);

        if (!string.IsNullOrEmpty(outputPath))
        {
            await File.WriteAllTextAsync(outputPath, data);
            return null;
        }

        return data;
    }

    public async Task ProcessCatalog()
    {
        Logger.LogInformation("Processing catalog");
        var data = await DownloadFileAsync(catalogUrl);
        if (data == null)
        {
            Logger.LogError("Failed to download catalog.");
            return;
        }

        Logger.LogInformation("Download complete. Filtering url...");
        string filter =
            @"{BDNetwork.CdnInfo.Info}\StandaloneWindows64\{BDNetwork.CdnInfo.Resolution}\{BDNetwork.CdnInfo.Version}";
        JObject? obj = JObject.Parse(data);
        JArray? internalIds = (JArray?)obj?["m_InternalIds"];
        downloadList = internalIds?.ToObject<List<string>>() ?? new List<string>();
        downloadList = downloadList.Where(x => x.StartsWith(filter)).ToList();

        for (int i = 0; i < downloadList.Count; i++)
        {
            downloadList[i] = downloadList[i].Replace("{BDNetwork.CdnInfo.Info}", Host)
                .Replace("{BDNetwork.CdnInfo.Resolution}", Resolution)
                .Replace("{BDNetwork.CdnInfo.Version}", version.ToString());
        }

        Logger.LogInformation($"Writing to file {AppDomain.CurrentDomain.BaseDirectory}/cdn.txt");
        await File.WriteAllLinesAsync("cdn.txt", downloadList);
    }

    public void ReadUrl()
    {
        string expectedCdn = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cdn.txt");
        downloadList = File.ReadAllLines(expectedCdn).ToList();
    }

    public async Task Download(string input)
    {
        Logger.LogInformation("Downloading assets...");
        if (!Directory.Exists(input))
        {
            Logger.LogInformation("Download folder does not exist, creating it.");
            Directory.CreateDirectory(input);
        }
        int count = downloadList.Count;
        for (int i = 0; i < downloadList.Count; i++)
        {
            try
            {
                Logger.LogInformation($"[{i + 1}/{count}] Downloading {downloadList[i]}");
                await DownloadFileAsync(downloadList[i], Path.Combine(input, Path.GetFileName(downloadList[i])));
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to download {downloadList[i]}: {ex.Message}");
                failedDownloads.Add(downloadList[i]);
            }
        }
        Logger.LogInformation($"Downloaded {count} assets, with {count - failedDownloads.Count} success and {failedDownloads.Count} failed downloads.");
    }

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

    public bool CheckSimilarity(string input1, string input2) => Helper.ComputeXXHash32(input1) == Helper.ComputeXXHash32(input2);
}

public class FileHash
{
    public string? Filename { get; set; }
    public string? Hash { get; set; }
}
