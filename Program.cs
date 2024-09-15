using System.Collections.Concurrent;
using System.Linq;
using System.Text.Json;

namespace DeleteUn
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string loc = @"F:\FullSetC\Game\Active\BrownDust\export";
            DeleteUnnecessary(loc);
            //RenameSpine(loc);
            //SortSpine(loc);
            //SortAsset(loc);
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
    }


}
