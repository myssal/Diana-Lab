using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BD2Logic
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //string loc = args[0];
            string og = @"F:\FullSetC\Game\Active\BrownDust\BrownDust2\spine\cutscenes";
            string bg = @"F:\FullSetC\Game\Active\BrownDust\out";
            //SortCutsceneBGs(og, bg);
            //FilterAtlas(@"F:\FullSetC\Game\Active\BrownDust\BrownDust2\ui\atlas", @"F:\FullSetC\Temp\output.txt");
            DeleteAtlas(@"F:\FullSetC\Game\Active\BrownDust\BrownDust2\ui\atlas");
			//DeleteUnnecessary(loc);
   //         RenameSpine(loc);
   //         SortSpine(loc);
   //         SortAsset(loc);
            Console.ReadLine();
            
        }
        public static void DeleteUnnecessary(string path)
        {
            List<string> deleteList = File.ReadAllLines("delete.txt").ToList();
            deleteList.Sort();
            int count = 0;
            List<string> file = Directory.GetFiles(path, "*.png*", SearchOption.TopDirectoryOnly).ToList();
            foreach (string fileItem in file)
            {
                if (deleteList.Any(x => fileItem.Contains(x)))
                {
                    Console.WriteLine($"Delete {Path.GetFileName(fileItem)}");
                    File.Delete(fileItem);
                    count++;
                    continue;
                }
            }
            List<string> audioFile = Directory.GetFiles(path, "*.bytes*", SearchOption.TopDirectoryOnly).ToList();
            foreach (string fileItem in audioFile)
            {
                Console.WriteLine($"Delete {Path.GetFileName(fileItem)}");
                File.Delete(fileItem);
            }
            Console.WriteLine($"Deleted {count} files.");
        }

        public static void RenameSpine(string path)
        {
            List<string> spineFile = Directory.GetFiles(path, "*.asset*", SearchOption.TopDirectoryOnly).ToList();
            foreach (string fileItem in spineFile)
            {
                Console.WriteLine($"Rename extension: {Path.GetFileName(fileItem)}");
                File.Move(fileItem, fileItem.Replace(".asset", string.Empty), true);
            }
        }

        public static void SortSpine(string path)
        {
            //create subdir spine for all spine files
            if (!Directory.Exists(Path.Combine(path, "spine"))) Directory.CreateDirectory(Path.Combine(path, "spine"));
            List<string> atlasFile = Directory.GetFiles(path, "*.atlas*", SearchOption.TopDirectoryOnly).ToList();
            foreach(string fileItem in atlasFile)
            {
                string[] content = File.ReadAllLines(fileItem);
                string[] texture = content.Where(x => x.Contains(".png")).ToArray();
                Console.WriteLine($"Found {texture.Length} textures.");
                string subFolderName = Path.GetFileNameWithoutExtension(fileItem);
                Directory.CreateDirectory(Path.Combine(path, "spine", subFolderName.ToLower()));
                try
                {
                    Console.WriteLine($"Move {fileItem}");
                    File.Move(fileItem, $"{Path.Combine(path, "spine", subFolderName.ToLower())}/{subFolderName}.atlas", true);
                    Console.WriteLine($"Move {fileItem.Replace(".atlas", ".skel")}");
                    File.Move(fileItem.Replace(".atlas", ".skel"), $"{Path.Combine(path, "spine", subFolderName.ToLower())}/{subFolderName}.skel", true);
                    foreach (string fileItem2 in texture)
                    {
                        Console.WriteLine($"Move {fileItem2}");
                        File.Move($"{path}/{fileItem2}", $"{Path.Combine(path, "spine", subFolderName.ToLower())}/{Path.GetFileName(fileItem2)}", true);
                    }
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine(e);
                }                
            }
        }

        public static void SortAsset(string path)
        {
            if (!Directory.Exists(Path.Combine(path, "sort"))) Directory.CreateDirectory(Path.Combine(path, "sort"));
            string content = File.ReadAllText("path.json");
            List<PathJson> list = JsonSerializer.Deserialize<List<PathJson>>(content);
            foreach (var item in list)
            {
                if (!Directory.Exists(Path.Combine(path, "sort", item.path)))
                {
                    Console.WriteLine($"Creating {Path.Combine(path, "sort", item.path)}");
                    Directory.CreateDirectory(Path.Combine(path, "sort", item.path));
                }

            }
            List<string> fileList = Directory.GetFiles(path, "*.png*", SearchOption.TopDirectoryOnly).ToList();
            int total = 0;
            int check = 0;
            foreach (string file in fileList)
            {
                total++;
                string subPath = list.Where(x => file.Contains(x.keyword)).Select(x => x.path).FirstOrDefault();
                if (subPath != null)
                {
                    check++;
                    Console.WriteLine($"Move {file}");
                    File.Move(file, $"{Path.Combine(path, "sort", subPath)}/{Path.GetFileName(file)}");
                }
            }
            Console.WriteLine($"Move {check} files in total of {total}");
        }

        public static void SortCutsceneBGs(string ogPath, string assetPath)
        {
            // assuming there's no number before id in path
            var idList = Directory.GetDirectories(ogPath, "*cutscene_char*");
            int[] id = idList.Select(x => int.Parse(Regex.Match(Path.GetFileNameWithoutExtension(x), @"\d+").Value)).ToArray();
			Regex bgCheck = new Regex(@"^((cha)r?)?[0-9]{5,6}");

			var bgFirstFilter = Directory.GetFiles(assetPath, "*.png", SearchOption.AllDirectories)
				                         .Where(x => bgCheck.IsMatch(Path.GetFileNameWithoutExtension(x).ToLower()))
				                         .ToList();
			Console.WriteLine("--------------------");
			Console.WriteLine(bgFirstFilter.Count);
            foreach (var bg in bgFirstFilter)
            {
				string expectedSubfolder = $"cutscene_char{int.Parse(Regex.Match(Path.GetFileNameWithoutExtension(bg), @"\d+").Value).ToString("D6")}";
				if (Directory.Exists(Path.Combine(ogPath, expectedSubfolder)))
                {
					Console.WriteLine($"Move {bg} -> {Path.Combine(ogPath, expectedSubfolder)}");   
                    File.Copy(bg, Path.Combine(ogPath, expectedSubfolder, Path.GetFileName(bg)), true);
                }
			}
		}
        
        public static void FilterAtlas(string filePath, string outputPath)
        {
            var files = Directory.GetFiles(filePath, "*.png", SearchOption.AllDirectories).ToList();
            var groupedFiles = new Dictionary<string, List<string>>();
            foreach (var file in files)
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
                int lastDashIndex = fileNameWithoutExt.LastIndexOf('-');

                if (lastDashIndex > 0)
                {
                    string prefix = fileNameWithoutExt.Substring(0, lastDashIndex);
                    string suffix = fileNameWithoutExt.Substring(lastDashIndex + 1);

                    if (!groupedFiles.ContainsKey(prefix))
                    {
                        groupedFiles[prefix] = new List<string>();
                    }

                    groupedFiles[prefix].Add(suffix);
                }
            }
            
            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                foreach (var group in groupedFiles)
                {
                    writer.WriteLine($"Prefix: {group.Key}");
                    foreach (var suffix in group.Value)
                    {
                        writer.WriteLine($"  Suffix: {suffix}");
                    }
                }
            }
        }

        public static void DeleteAtlas(string directoryPath)
        {
            var allFiles = Directory.GetFiles(directoryPath, "*.png");
            var groupedFiles = new Dictionary<string, List<FileInfo>>();

            foreach (var filePath in allFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                int lastDashIndex = fileName.LastIndexOf('-');

                if (lastDashIndex > 0)
                {
                    string prefix = fileName.Substring(0, lastDashIndex);
                    FileInfo fileInfo = new FileInfo(filePath);

                    if (!groupedFiles.ContainsKey(prefix))
                    {
                        groupedFiles[prefix] = new List<FileInfo>();
                    }

                    groupedFiles[prefix].Add(fileInfo);
                }
            }

            // Delete all files in each group except the most recently modified
            foreach (var group in groupedFiles)
            {
                var filesByDate = group.Value.OrderByDescending(f => f.LastWriteTime).ToList();

                for (int i = 1; i < filesByDate.Count; i++) // Keep the first (most recent)
                {
                    try
                    {
                        filesByDate[i].Delete();
                        Console.WriteLine($"Deleted: {filesByDate[i].FullName}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to delete {filesByDate[i].FullName}: {ex.Message}");
                    }
                }
            }

            Console.WriteLine("Cleanup complete (kept most recent file per group).");
        }
    }


}
