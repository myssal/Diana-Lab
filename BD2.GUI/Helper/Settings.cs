using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2.GUI
{
	public class LoadUpSettings
	{
		string settingsPath = @"F:\FullSetC\Tool\Game\Brown Dust\ExtractAsset\BD2.GUI\settings.json";
		public bool isSettingsJsonExist { get; set; }
		public Root settings { get; set; }
		public LoadUpSettings()
		{
			settings = new Root();
			GetSettings();
		}

		public bool CheckSettingsExistence() => isSettingsJsonExist = File.Exists(settingsPath);

		public void GetSettings()
		{
			if (CheckSettingsExistence())
			{
				string json = File.ReadAllText(settingsPath);
				settings = JsonConvert.DeserializeObject<Root>(json);
			}
		}

		public void SaveSettings()
		{
			string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
			File.WriteAllText(settingsPath, json);
		}
		public class AssetTypes
		{
			public bool Texture2D { get; set; }

			[JsonProperty("Text Asset")]
			public bool TextAsset { get; set; }
			public bool Audio { get; set; }
		}

		public class Options
		{
			[JsonProperty("Asset types")]
			public AssetTypes Assettypes { get; set; }
			public Process Process { get; set; }
		}

		public class Paths
		{
			public string Data { get; set; }
			public string Output { get; set; }
		}

		public class Process
		{
			[JsonProperty("Export asset")]
			public bool Exportasset { get; set; }

			[JsonProperty("Sort asset")]
			public bool Sortasset { get; set; }

			[JsonProperty("Export l2d bgs")]
			public bool Exportl2dbgs { get; set; }

			[JsonProperty("Copy to repo")]
			public bool Copytorepo { get; set; }

			[JsonProperty("Delete data")]
			public bool Deletedata { get; set; }
		}

		public class Root
		{
			public Options options { get; set; }
			public Paths paths { get; set; }

			public Root()
			{
				options = new Options();
				paths = new Paths();
			}
		}
	}
}