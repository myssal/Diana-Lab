using System.Text.Json;
using Microsoft.Extensions.Logging;
namespace BD2Tools;

public partial class AssetLogic
{
    public void SortAtlas(string atlasPath = "data/atlas.json")
    {
        Logger.LogInformation($"Start sorting atlas.");
        string basePath = Path.Combine(config["output"], "sort", "ui", "atlas");
        //string basePath = atlasPath;
        
        List<string> atlasContents = Directory.GetFiles(basePath, "*.png*", SearchOption.TopDirectoryOnly).ToList();
        Logger.LogInformation($"Found {atlasContents.Count} atlas files to sort in {basePath}.");;
        string jsonContent = File.ReadAllText(atlasPath);
        var atlasMappings = JsonSerializer.Deserialize<List<PathJson>>(jsonContent);
        
        foreach (var file in atlasContents)
        {
            string fileName = Path.GetFileName(file);

            foreach (var mapping in atlasMappings)
            {
                if (fileName.Contains(mapping.keyword, StringComparison.OrdinalIgnoreCase))
                {
                    string targetDir = Path.Combine(basePath, mapping.path);
                    if (!Directory.Exists(targetDir))
                    {
                        Directory.CreateDirectory(targetDir);
                    }

                    string targetPath = Path.Combine(targetDir, fileName);

                    File.Move(file, targetPath, true);
                    Logger.LogInformation($"Moved '{fileName}' to '{targetDir}'");
                    break;
                }
            }
        }

        Logger.LogInformation($"Finished sorting atlas.");
    }
    
    public void FlattenAtlasDirectory(string atlasPath)
    {
        Logger.LogInformation($"Flattening atlas directory: {atlasPath}");

        var allFiles = Directory.GetFiles(atlasPath, "*.*", SearchOption.AllDirectories);

        foreach (var file in allFiles)
        {
            string relativePath = Path.GetRelativePath(atlasPath, file);
            string fileName = Path.GetFileName(file);
            string destinationPath = Path.Combine(atlasPath, fileName);

            if (Path.GetDirectoryName(file) == atlasPath)
                continue;

            File.Move(file, destinationPath,true);
            Logger.LogInformation($"Moved '{relativePath}' to root.");
        }
        
        var subDirs = Directory.GetDirectories(atlasPath, "*", SearchOption.AllDirectories);
        foreach (var dir in subDirs.OrderByDescending(d => d.Length)) // Ensure deeper folders are deleted first
        {
            if (Directory.GetFiles(dir).Length == 0 && Directory.GetDirectories(dir).Length == 0)
            {
                Directory.Delete(dir, recursive: false);
                Logger.LogInformation($"Deleted empty folder: {dir}");
            }
        }

        Logger.LogInformation("Finished flattening atlas directory.");
    }

    public void CheckDuplicate(string input)
    {
        
    }
}