using Microsoft.Extensions.Logging;

namespace BD2Tools;

public partial class AssetLogic: LoggedService<AssetLogic>
{
    Dictionary<string, string> config {get; set;}
    private string configPath = "config.txt";
    private List<string> updatedFiles {get; set;}
    
    public AssetLogic(ILogger<AssetLogic> logger) : base(logger)
    {
        config = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        updatedFiles = new List<string>();
        GetConfigureData();
        updatedFiles = Helper.GetFilesBasedOnDate(config["input"], int.Parse(config["check_option"]), config["date"],
            int.Parse(config["fallback_date"]));
        if (updatedFiles.Count == 0)
            Logger.LogInformation($"No files modified found in specific time range, consider changing time date in config.txt.");
    }
    
    public void GetConfigureData()
    {
        if (!File.Exists(configPath))
        {
            Logger.LogError($"Config file not found at {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configPath)}.");
            return;
        }

        Logger.LogInformation($"Reading config file from {configPath}");

        try
        {
            foreach (var line in File.ReadLines(configPath))
            {
                int separatorIndex = line.IndexOf(": ");
                if (separatorIndex > 0)
                {
                    var key = line.Substring(0, separatorIndex).Trim();
                    var value = line.Substring(separatorIndex + 2).Trim();
                    config[key] = value;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error reading config file: {ex.Message}");
        }
    }

    public void MoveFiles()
    {
        foreach (var file in updatedFiles)
        {
            string relativePath = Path.GetRelativePath(config["input"],file);
                
            string destinationPath = Path.Combine(config["temp"], relativePath);
                
            string destinationDir = Path.GetDirectoryName(destinationPath);
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }
            Logger.LogInformation($"Copying {file} -> {destinationDir}");
            File.Copy(file, destinationPath, overwrite: true);
        }
    }
    public void ExtractAsset()
    {
        Logger.LogInformation($"Extracting from {config["temp"]}");
        string parameter = $"{config["temp"]} {config["output"]} --types Texture2D TextAsset --game Normal --unity_version 2022.3.22f1 --group_assets None";
        Helper.RunCommand(config["asset-studio"], parameter);
    }

    public void SactxSort(string input)
    {
        Logger.LogInformation("Sorting ");
    }
    
    public void ProcessAsset()
    {
        if (updatedFiles.Count > 0)
        {
            // MoveFiles();
            // ExtractAsset();
            // DeleteRedundant();
            // RenameSpine();
            // SortAsset();
            // SortSpine();
            // OrganizeSpine();
            SortAtlas();
            Logger.LogInformation("Done!");
        }
    }
    
}