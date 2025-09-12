using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Drawing;
using System.Text.RegularExpressions;
using DianaLab.Core.Model;
using DianaLab.Core.Utils;

namespace DianaLab.Core.Services;

public class AssetService : LoggedService<AssetService>
{
    private readonly Dictionary<string, string> config;
    private const string ConfigPath = "config.txt";
    public static string deleteTxt = @"Data\delete.txt";
    public static string atlasJson = @"Data\atlas.json";
    public static string pathJson = @"Data\path.json";
    private readonly List<string> updatedFiles;

    public AssetService(ILogger<AssetService> logger) : base(logger)
    {
        config = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        updatedFiles = new List<string>();
        GetConfigureData();
        if (config.Count > 0)
        {
            updatedFiles = Helper.GetFilesBasedOnDate(config["input"], int.Parse(config["check_option"]), config["date"],
                int.Parse(config["fallback_date"]));
            if (updatedFiles.Count == 0)
                Logger.LogInformation("No files modified found in specific time range, consider changing time date in config.txt.");
        }
    }

    public void GetConfigureData()
    {
        if (!File.Exists(ConfigPath))
        {
            Logger.LogError($"Config file not found at {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigPath)}.");
            return;
        }

        Logger.LogInformation($"Reading config file from {ConfigPath}");

        try
        {
            foreach (var line in File.ReadLines(ConfigPath))
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
            string relativePath = Path.GetRelativePath(config["input"], file);
            string destinationPath = Path.Combine(config["temp"], relativePath);
            string? destinationDir = Path.GetDirectoryName(destinationPath);
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir!);
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

    public void ProcessAsset()
    {
        if (updatedFiles.Count > 0)
        {
            MoveFiles();
            ExtractAsset();
            DeleteRedundant();
            RenameSpine();
            SortAsset();
            SortSpine();
            OrganizeSpine();
            ResizeSpineImages();
            SortAtlas();
            NormalizeCostumeIcons();
            Logger.LogInformation("Done!");
        }
    }

    public void DeleteRedundant(string inputPath = "")
    {
        if (string.IsNullOrEmpty(inputPath)) inputPath = config["output"];
        Logger.LogInformation("Start cleaning redundant assets");
        List<string> deleteList = File.ReadAllLines(deleteTxt).ToList();
        deleteList.Sort();
        int count = 0;
        List<string> file = Directory.GetFiles(inputPath, "*.png*", SearchOption.TopDirectoryOnly).ToList();
        foreach (string fileItem in file)
        {
            if (deleteList.Any(x => fileItem.Contains(x)))
            {
                Logger.LogInformation($"Delete {Path.GetFileName(fileItem)}");
                File.Delete(fileItem);
                count++;
            }
        }
        List<string> audioFile = Directory.GetFiles(inputPath, "*.bytes*", SearchOption.TopDirectoryOnly).ToList();
        foreach (string fileItem in audioFile)
        {
            Logger.LogInformation($"Delete {Path.GetFileName(fileItem)}");
            File.Delete(fileItem);
        }
        Logger.LogInformation($"Deleted {count} files.");
    }

    public void RenameSpine(string inputPath = "")
    {
        if (string.IsNullOrEmpty(inputPath)) inputPath = config["output"];
        Logger.LogInformation("Start renaming spine");
        List<string> spineFile = Directory.GetFiles(inputPath, "*.asset*", SearchOption.TopDirectoryOnly).ToList();
        spineFile.AddRange(Directory.GetFiles(inputPath, "*.prefab*", SearchOption.TopDirectoryOnly).ToList());
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
        
        // dirty workaround for this certain spine
        var atlasFiles = Directory.GetFiles(outputPath, "*.atlas*", SearchOption.TopDirectoryOnly)
            .Where(f => !Path.GetFileName(f).Equals("char000402.skel.atlas", StringComparison.OrdinalIgnoreCase) && 
                        !Path.GetFileName(f).Equals("cutscene_char066402_camera", StringComparison.OrdinalIgnoreCase));

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
                string targetAtlasPath = Path.Combine(targetSubDir, $"{baseFileName}.atlas");
                Logger.LogInformation($"Moving atlas: {atlasPath} -> {targetAtlasPath}");
                File.Move(atlasPath, targetAtlasPath, overwrite: true);

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
                }
                catch (Exception ex)
                {
                    Logger.LogWarning($"Error moving texture {textureName}: {ex.Message}");
                }
            }
        }
    }

    public void SortAsset()
    {
        Logger.LogInformation("Start sorting assets.");
        string outputPath = config["output"];
        string sortPath = Path.Combine(outputPath, "sort");
        if (!Directory.Exists(sortPath))
            Directory.CreateDirectory(sortPath);

        string jsonContent = File.ReadAllText(pathJson);
        var pathMappings = JsonSerializer.Deserialize<List<PathJson>>(jsonContent);

        foreach (var mapping in pathMappings!)
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
            string? targetSubPath = pathMappings
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

        return "misc";
    }

    public void SortAtlas(string atlasPath = "Data/atlas.json")
    {
        Logger.LogInformation("Start sorting atlas.");
        string basePath = Path.Combine(config["output"], "sort", "ui", "atlas");

        List<string> atlasContents = Directory.GetFiles(basePath, "*.png*", SearchOption.TopDirectoryOnly).ToList();
        Logger.LogInformation($"Found {atlasContents.Count} atlas files to sort in {basePath}."); ;
        string jsonContent = File.ReadAllText(atlasPath);
        var atlasMappings = JsonSerializer.Deserialize<List<PathJson>>(jsonContent);

        foreach (var file in atlasContents)
        {
            string fileName = Path.GetFileName(file);

            foreach (var mapping in atlasMappings!)
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

        Logger.LogInformation("Finished sorting atlas.");
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

            File.Move(file, destinationPath, true);
            Logger.LogInformation($"Moved '{relativePath}' to root.");
        }

        var subDirs = Directory.GetDirectories(atlasPath, "*", SearchOption.AllDirectories);
        foreach (var dir in subDirs.OrderByDescending(d => d.Length))
        {
            if (Directory.GetFiles(dir).Length == 0 && Directory.GetDirectories(dir).Length == 0)
            {
                Directory.Delete(dir, recursive: false);
                Logger.LogInformation($"Deleted empty folder: {dir}");
            }
        }

        Logger.LogInformation("Finished flattening atlas directory.");
    }

    public void CheckDuplicate(string inputPath)
    {
        List<string> lowestLevelFolders = Helper.GetLowestLevelSubfolders(inputPath);
        Logger.LogInformation($"Found {lowestLevelFolders.Count} lowest level.");
        foreach (var subfolder in lowestLevelFolders)
        {
            Logger.LogInformation($"Scanning duplicate in {subfolder}");
            foreach (var file in FindDuplicateImages(subfolder))
            {
                Logger.LogInformation($"Delete duplicate image: {Path.GetFileName(file)}");
                File.Delete(file);
            }
        }
    }
    public List<string> FindDuplicateImages(string folderPath)
    {
        var pngFiles = Directory.GetFiles(folderPath, "*.png", SearchOption.TopDirectoryOnly);

        var fileInfos = pngFiles.Select(file => new
        {
            FilePath = file,
            Name = GetBaseName(Path.GetFileNameWithoutExtension(file)),
            Hash = Helper.ComputeXXHash32(file),
            Modified = File.GetLastWriteTimeUtc(file)
        }).ToList();

        var duplicatesToReport = new List<string>();

        var groupedByName = fileInfos.GroupBy(f => f.Name);

        foreach (var group in groupedByName)
        {
            var hashGroups = group.GroupBy(f => f.Hash);

            foreach (var hashGroup in hashGroups)
            {
                if (hashGroup.Count() > 1)
                {
                    var sorted = hashGroup.OrderByDescending(f => f.Modified).ToList();
                    duplicatesToReport.AddRange(sorted.Skip(1).Select(f => f.FilePath));
                }
            }
        }

        return duplicatesToReport;
    }

    public string GetBaseName(string filename)
    {
        int lastDash = filename.LastIndexOf('-');
        return lastDash > 0 ? filename.Substring(0, lastDash) : filename;
    }

    public void ResizeSpineImages()
    {
        string basePath = Path.Combine(config["output"], "spine");
        if (!Directory.Exists(basePath))
        {
            Logger.LogWarning($"Spine directory not found: {basePath}");
            return;
        }

        var atlasFiles = Directory.GetFiles(basePath, "*.atlas", SearchOption.AllDirectories);
        int resizedFilesCount = 0;
        var resizedFiles = new List<string>();

        foreach (var atlasFile in atlasFiles)
        {
            Logger.LogInformation($"Processing atlas: {atlasFile}");
            var imageDimensions = new Dictionary<string, Size>();
            string[] lines = File.ReadAllLines(atlasFile);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(".png"))
                {
                    string imageName = lines[i].Trim();
                    if (i + 1 < lines.Length)
                    {
                        string sizeLine = lines[i + 1];
                        var match = System.Text.RegularExpressions.Regex.Match(sizeLine, @"size:\s*(\d+),(\d+)");
                        if (match.Success)
                        {
                            int width = int.Parse(match.Groups[1].Value);
                            int height = int.Parse(match.Groups[2].Value);
                            imageDimensions[imageName] = new Size(width, height);
                        }
                    }
                }
            }

            string directory = Path.GetDirectoryName(atlasFile)!;
            foreach (var entry in imageDimensions)
            {
                string imagePath = Path.Combine(directory, entry.Key);
                if (File.Exists(imagePath))
                {
                    using (var image = new Bitmap(imagePath))
                    {
                        if (image.Width != entry.Value.Width || image.Height != entry.Value.Height)
                        {
                            Logger.LogInformation($"Resizing {imagePath} from {image.Width}x{image.Height} to {entry.Value.Width}x{entry.Value.Height}");
                            using (var newBitmap = new Bitmap(entry.Value.Width, entry.Value.Height))
                            {
                                using (var graphics = Graphics.FromImage(newBitmap))
                                {
                                    graphics.DrawImage(image, 0, 0, entry.Value.Width, entry.Value.Height);
                                }
                                image.Dispose();
                                newBitmap.Save(imagePath);
                            }
                            resizedFilesCount++;
                            resizedFiles.Add(Path.GetRelativePath(basePath, imagePath));
                        }
                    }
                }
                else
                {
                    Logger.LogWarning($"Image not found: {imagePath}");
                }
            }
        }

        Logger.LogInformation($"Resized {resizedFilesCount} files.");
        foreach (var file in resizedFiles)
        {
            Logger.LogInformation(file);
        }
    }
    
    public void NormalizeCostumeIcons(string inputPath = "")
    {
        if (String.IsNullOrEmpty(inputPath))
            inputPath = Path.Combine(config["output"], "sort", "ui", "icon", "costume");
    
        Logger.LogInformation($"Normalizing costume name: {inputPath}");
    
        if (!Directory.Exists(inputPath))
        {
            Logger.LogWarning("Directory not found: " + inputPath);
            return;
        }

        // Regex to match files like: icon_costume123_45.ext
        Regex regex = new Regex(@"^icon_costume(\d+)_(\d+)(\..+)?$", RegexOptions.IgnoreCase);

        int renamedCount = 0;
        int skippedCount = 0;

        foreach (var file in Directory.GetFiles(inputPath, "icon_costume*"))
        {
            var fileName = Path.GetFileName(file);
            var match = regex.Match(fileName);

            if (match.Success)
            {
                string firstNum = match.Groups[1].Value; 
                string secondNum = match.Groups[2].Value;
                string extension = match.Groups[3].Value ?? "";
            
                string normalizedFirst = int.Parse(firstNum).ToString("D6");
                string newFileName = $"icon_costume{normalizedFirst}_{secondNum}{extension}";

                string newPath = Path.Combine(inputPath, newFileName);

                if (!File.Exists(newPath))
                {
                    File.Move(file, newPath);
                    Logger.LogInformation($"Renamed: {fileName} -> {newFileName}");
                    renamedCount++;
                }
                else
                {
                    Logger.LogWarning($"Skipped (target exists): {newFileName}");
                    skippedCount++;
                }
            }
        }

        Logger.LogInformation($"Normalization complete. Renamed: {renamedCount}, Skipped: {skippedCount}, Total: {renamedCount + skippedCount}");
    }
}