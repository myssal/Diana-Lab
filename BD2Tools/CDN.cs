using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace BD2Tools;

public partial class CDN: LoggedService<CDN>
{
    const string host = "https://bd2-cdn.akamaized.net/ServerData";
    const string resolution = "HD";
    private long version;
    private string catalogUrl;
    private string baseUrl;
    private List<string> downloadList;
    private List<string> failedDownloads;
    public CDN(ILogger<CDN> logger, long version): base(logger)
    {
        //{BDNetwork.CdnInfo.Info}\\StandaloneWindows64\\{BDNetwork.CdnInfo.Resolution}\\{BDNetwork.CdnInfo.Version}\\common-bgmalbum_1_assets_all.bundle
        //https://bd2-cdn.akamaized.net/ServerData/StandaloneWindows64/HD/20250521211633/catalog_alpha.json
        this.version = version;
        baseUrl = $"{host}/StandaloneWindows64/{resolution}/{version}";
        catalogUrl = $"{baseUrl}/catalog_alpha.json";
        downloadList = new List<string>();
        failedDownloads = new List<string>();
    }

    public static async Task<string> DownloadFileAsync(string url, string outputPath = null)
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
        Logger.LogInformation($"Processing catalog");
        var data = await DownloadFileAsync(catalogUrl);
        if (data == null)
        {
            Logger.LogError("Failed to download catalog.");
        }
        
        Logger.LogInformation($"Download complete. Filtering url...");
        string filter =
            @"{BDNetwork.CdnInfo.Info}\StandaloneWindows64\{BDNetwork.CdnInfo.Resolution}\{BDNetwork.CdnInfo.Version}";
        JObject obj = JObject.Parse(data);
        JArray internalIds = (JArray)obj["m_InternalIds"];
        downloadList = internalIds.ToObject<List<string>>();
        downloadList = downloadList.Where(x => x.StartsWith(filter)).ToList();

        for (int i = 0; i < downloadList.Count; i++)
        {
            downloadList[i] = downloadList[i].Replace("{BDNetwork.CdnInfo.Info}", host)
                           .Replace("{BDNetwork.CdnInfo.Resolution}", resolution)
                           .Replace("{BDNetwork.CdnInfo.Version}", version.ToString());
        }
        
        Logger.LogInformation($"Writing to file {AppDomain.CurrentDomain.BaseDirectory}/cdn.txt");
        // Write to cdn.txt (each item on its own line)
        File.WriteAllLines("cdn.txt", downloadList);
    }

    public void ReadUrl()
    {
        string expectedCdn = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cdn.txt");
        downloadList = File.ReadAllLines(expectedCdn).ToList();
    }
    public async Task Download(string input)
    {
        Logger.LogInformation($"Downloading assets...");
        if (!Directory.Exists(input) )
        {
            Logger.LogInformation("Donwload folder does not exist, creating it.");
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

    
    
}