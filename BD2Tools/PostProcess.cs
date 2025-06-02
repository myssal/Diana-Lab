using Microsoft.Extensions.Logging;
using System.Text.Json;
namespace BD2Tools;

public partial class AssetLogic
{
    public void DeleteRedundant()
    {
        Logger.LogInformation($"Start cleaning redundant assets");
        List<string> deleteList = File.ReadAllLines(deleteTxt).ToList();
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
        spineFile.AddRange(Directory.GetFiles(config["output"], "*.prefab*", SearchOption.TopDirectoryOnly).ToList());
        foreach (string fileItem in spineFile)
        {
            Logger.LogInformation($"Rename spine file: {Path.GetFileName(fileItem)}");
            File.Move(fileItem, fileItem.Replace(".asset", string.Empty), true);
        }
    }
    
    public void SortSpine()
    {
        Logger.LogInformation("Start sorting spine.");
        string outputPath = config["output"];
        string spineDir = Path.Combine(outputPath, "spine");
        
        if (!Directory.Exists(spineDir))
        {
            Directory.CreateDirectory(spineDir);
        }

        // Get all .atlas files
        var atlasFiles = Directory.GetFiles(outputPath, "*.atlas*", SearchOption.TopDirectoryOnly);

        foreach (string atlasPath in atlasFiles)
        {
            string[] atlasContent = File.ReadAllLines(atlasPath);
            var textureFiles = atlasContent.Where(line => line.Contains(".png")).ToList();
            var textureBg = Directory.GetFiles(outputPath, $"*{Path.GetFileNameWithoutExtension(atlasPath)}_back*", 
                SearchOption.TopDirectoryOnly).ToList();
            textureFiles.AddRange(textureBg);
            Logger.LogInformation($"Found {textureFiles.Count} textures in {Path.GetFileName(atlasPath)}.");

            string baseFileName = Path.GetFileNameWithoutExtension(atlasPath);
            string targetSubDir = Path.Combine(spineDir, baseFileName.ToLower());

            Directory.CreateDirectory(targetSubDir);

            try
            {
                // Move .atlas file
                string targetAtlasPath = Path.Combine(targetSubDir, $"{baseFileName}.atlas");
                Logger.LogInformation($"Moving atlas: {atlasPath} -> {targetAtlasPath}");
                File.Move(atlasPath, targetAtlasPath, overwrite: true);

                // Move .skel file
                string skelPath = Path.Combine(outputPath, $"{baseFileName}.skel");
                string targetSkelPath = Path.Combine(targetSubDir, $"{baseFileName}.skel");
                Logger.LogInformation($"Moving skeleton: {skelPath} -> {targetSkelPath}");
                File.Move(skelPath, targetSkelPath, overwrite: true);
            }
            catch (FileNotFoundException ex)
            {
                Logger.LogWarning($"Missing core file: {ex.FileName}");
                continue; 
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Unexpected error during atlas/skel move: {ex.Message}");
                continue;
            }

            // Move texture files (resilient per texture)
            foreach (string textureName in textureFiles)
            {
                try
                {
                    string sourceTexturePath = Path.Combine(outputPath, textureName);
                    string targetTexturePath = Path.Combine(targetSubDir, Path.GetFileName(textureName));
                    Logger.LogInformation($"Moving texture: {sourceTexturePath} -> {targetTexturePath}");
                    File.Move(sourceTexturePath, targetTexturePath, overwrite: true);
                }
                catch (FileNotFoundException)
                {
                    Logger.LogWarning($"Texture file not found: {textureName}");
                    continue;
                }
                catch (Exception ex)
                {
                    Logger.LogWarning($"Error moving texture {textureName}: {ex.Message}");
                    continue;
                }
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

            string jsonContent = File.ReadAllText(pathJson);
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
    
    public void OrganizeSpine()
    {
        string basePath = Path.Combine(config["output"], "spine");

        if (!Directory.Exists(basePath))
        {
            Console.WriteLine($"Base path does not exist: {basePath}");
            return;
        }

        string[] items;
        try
        {
            items = Directory.GetDirectories(basePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to read directories: {ex.Message}");
            return;
        }

        foreach (string itemPath in items)
        {
            string folderName = Path.GetFileName(itemPath);
            string destination = GetDestinationPath(folderName);
            string destFullPath = Path.Combine(basePath, destination, folderName);

            try
            {
                string? parentDir = Path.GetDirectoryName(destFullPath);
                if (!string.IsNullOrEmpty(parentDir))
                {
                    Directory.CreateDirectory(parentDir);
                }

                if (Directory.Exists(destFullPath))
                {
                    Console.WriteLine($"Warning: Destination already exists, skipping: {destFullPath}");
                    continue;
                }

                Directory.Move(itemPath, destFullPath);
                Console.WriteLine($"Moved: {folderName} -> {destination}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error moving '{folderName}' to '{destination}': {ex.Message}");
            }
        }
    }

    public string GetDestinationPath(string name)
    {
        if (name.StartsWith("char") && !name.StartsWith("cutscene_char"))
            return "char";
        if (name.StartsWith("cutscene_char"))
            return "cutscenes";
        if (name.StartsWith("npc"))
            return "npc";
        if (name.StartsWith("illust_dating"))
            return Path.Combine("illust", "illust_dating");
        if (name.StartsWith("illust_special") || name.StartsWith("specialillust"))
            return Path.Combine("illust", "illust_special");
        if (name.StartsWith("illust_talk"))
            return Path.Combine("illust", "illust_talk");

        // fallback
        return "misc";  
    }
}