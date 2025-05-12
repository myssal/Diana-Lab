using Microsoft.Extensions.Logging;

namespace BD2Tools;

public class AssetLogic: LoggedService<AssetLogic>
{
    string inputPath { get; set; }
    string outputPath { get; set; }
    private string configPath = "config.txt";
    
    public AssetLogic(ILogger<AssetLogic> logger) : base(logger)
    {
        
    }
    
    public void GetConfigureData()
    {
        if (!File.Exists(configPath))
        {
            Logger.LogError($"Config file not found at {AppDomain.CurrentDomain.BaseDirectory}{configPath}.");
            return;
        }
        Logger.LogInformation($"Reading config file from {configPath}");
    }
}