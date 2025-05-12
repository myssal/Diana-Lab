using Microsoft.Extensions.Logging;
using System.Text.Json;
namespace BD2Tools;

public partial class AssetLogic
{
    public void DeleteRedundant()
    {
        Logger.LogInformation($"Start cleaning redundant assets");
        List<string> deleteList = File.ReadAllLines("delete.txt").ToList();
        deleteList.Sort();
        int count = 0;
        List<string> file = Directory.GetFiles(config["output"], "*.png*", SearchOption.TopDirectoryOnly).ToList();
        foreach (string fileItem in file)
        {
            if (deleteList.Any(x => fileItem.Contains(x)))
            {
                Logger.LogInformation($"Delete {Path.GetFileName(fileItem)}");
                File.Delete(fileItem);
                count++;
                continue;
            }
        }
        List<string> audioFile = Directory.GetFiles(config["output"], "*.bytes*", SearchOption.TopDirectoryOnly).ToList();
        foreach (string fileItem in audioFile)
        {
            Logger.LogInformation($"Delete {Path.GetFileName(fileItem)}");
            File.Delete(fileItem);
        }
        Logger.LogInformation($"Deleted {count} files.");
    }
    
    public void RenameSpine()
    {
        Logger.LogInformation($"Start renaming spine");
        List<string> spineFile = Directory.GetFiles(config["output"], "*.asset*", SearchOption.TopDirectoryOnly).ToList();
        foreach (string fileItem in spineFile)
        {
            Logger.LogInformation($"Rename spine file: {Path.GetFileName(fileItem)}");
            File.Move(fileItem, fileItem.Replace(".asset", string.Empty), true);
        }
    }
    
    public void SortSpine()
    {
        Logger.LogInformation($"Start sorting spine.");
        string outputPath = config["output"];
        string spineDir = Path.Combine(outputPath, "spine");

        // Create 'spine' directory if it doesn't exist
        if (!Directory.Exists(spineDir))
        {
            Directory.CreateDirectory(spineDir);
        }

        // Find all .atlas files in the output directory
        var atlasFiles = Directory.GetFiles(outputPath, "*.atlas*", SearchOption.TopDirectoryOnly);

        foreach (string atlasPath in atlasFiles)
        {
            string[] atlasContent = File.ReadAllLines(atlasPath);
            var textureFiles = atlasContent.Where(line => line.Contains(".png")).ToArray();
            Logger.LogInformation($"Found {textureFiles.Length} textures in {Path.GetFileName(atlasPath)}.");

            string baseFileName = Path.GetFileNameWithoutExtension(atlasPath);
            string targetSubDir = Path.Combine(spineDir, baseFileName.ToLower());

            Directory.CreateDirectory(targetSubDir);

            try
            {
                // Move .atlas file
                string targetAtlasPath = Path.Combine(targetSubDir, $"{baseFileName}.atlas");
                Logger.LogInformation($"Moving atlas: {atlasPath} -> {targetAtlasPath}");
                File.Move(atlasPath, targetAtlasPath, overwrite: true);

                // Move corresponding .skel file
                string skelPath = Path.Combine(outputPath, $"{baseFileName}.skel");
                string targetSkelPath = Path.Combine(targetSubDir, $"{baseFileName}.skel");
                Logger.LogInformation($"Moving skeleton: {skelPath} -> {targetSkelPath}");
                File.Move(skelPath, targetSkelPath, overwrite: true);

                // Move all associated texture files
                foreach (string textureName in textureFiles)
                {
                    string sourceTexturePath = Path.Combine(outputPath, textureName);
                    string targetTexturePath = Path.Combine(targetSubDir, Path.GetFileName(textureName));
                    Logger.LogInformation($"Moving texture: {sourceTexturePath} -> {targetTexturePath}");
                    File.Move(sourceTexturePath, targetTexturePath, overwrite: true);
                }
            }
            catch (FileNotFoundException ex)
            {
                Logger.LogInformation($"File not found: {ex.FileName}");
            }
            catch (Exception ex)
            {
                Logger.LogInformation($"Unexpected error: {ex.Message}");
            }
        }
    }

    public void SortAsset()
    {
        Logger.LogInformation($"Start sorting assets.");
        string outputPath = config["output"];
        string sortPath = Path.Combine(outputPath, "sort");

        if (!Directory.Exists(sortPath))
        {
            Directory.CreateDirectory(sortPath);

            string jsonContent = File.ReadAllText("path.json");
            var pathMappings = JsonSerializer.Deserialize<List<PathJson>>(jsonContent);

            foreach (var mapping in pathMappings)
            {
                string targetDir = Path.Combine(sortPath, mapping.path);
                if (!Directory.Exists(targetDir))
                {
                    Logger.LogInformation($"Creating directory: {targetDir}");
                    Directory.CreateDirectory(targetDir);
                }
            }

            var pngFiles = Directory.GetFiles(outputPath, "*.png", SearchOption.TopDirectoryOnly);
            int totalFiles = pngFiles.Length;
            int movedFiles = 0;

            foreach (string filePath in pngFiles)
            {
                string fileName = Path.GetFileName(filePath);

                // Try to find a matching path using the keyword
                string targetSubPath = pathMappings
                    .FirstOrDefault(mapping => filePath.Contains(mapping.keyword))?.path;

                if (!string.IsNullOrEmpty(targetSubPath))
                {
                    string targetPath = Path.Combine(sortPath, targetSubPath, fileName);
                    try
                    {
                        Logger.LogInformation($"Moving: {filePath} -> {targetPath}");
                        File.Move(filePath, targetPath, overwrite: true);
                        movedFiles++;
                    }
                    catch (IOException ex)
                    {
                        Logger.LogError($"Failed to move {filePath}: {ex.Message}");
                    }
                }
            }

            Logger.LogInformation($"Moved {movedFiles} files out of {totalFiles}.");
        }
    }
}