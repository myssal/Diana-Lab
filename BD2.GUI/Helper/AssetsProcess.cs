using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2.GUI
{
	public class AssetsProcess
	{
		public static bool CheckFileExist(string path) => File.Exists(path);
		public static void DeleteUnnecessary(string path, bool deleteAudio = true)
		{
			if (CheckFileExist("delete.txt"))
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
		}
	}
}
