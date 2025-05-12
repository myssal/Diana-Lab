using Microsoft.Extensions.Logging;

namespace BD2Tools;

public class AssetLogic: LoggedService<AssetLogic>
{
    Dictionary<string, string> config {get; set;}
    private string configPath = "config.txt";
    private List<string> updatedFiles {get; set;}
    
    public AssetLogic(ILogger<AssetLogic> logger) : base(logger)
    {
        config = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        updatedFiles = new List<string>();
        GetConfigureData();
        updatedFiles = Helper.GetFilesBasedOnDate(config["input"], 1, config["date"]);
        if (updatedFiles.Count == 0)
            Logger.LogInformation($"No files modified found in specific time range, consider changing time date in config.txt.");
    }
    
    public void GetConfigureData()
    {
        if (!File.Exists(configPath))
        {
            Logger.LogError($"Config file not found at {AppDomain.CurrentDomain.BaseDirectory}{configPath}.");
            return;
        }
        Logger.LogInformation($"Reading config file from {configPath}");
        try
        {
            using (StreamReader reader = new StreamReader(configPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(": "))
                    {
                        var parts = line.Split(": ");
                        if (parts.Length == 2)
                        {
                            config[parts[0].Trim()] = parts[1].Trim();
                        }
                    }
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
        if (updatedFiles.Count > 0)
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
    }
    public void ExtractAsset()
    {
        if (updatedFiles.Count > 0)
        {
            Logger.LogInformation($"Extracting from {config["temp"]}");
            string parameter = $"{config["temp"]} {config["output"]} --types Texture2D TextAsset --game Normal --unity_version 2022.3.22f1 --group_assets None";
            Helper.RunCommand(config["asset-studio"], parameter);
        }
    }
    
}