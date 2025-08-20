using System.Text.Json;
using System.Text.Json.Serialization;

namespace BD2Tools.Core;

public partial class CharInfo
{
    public static void ConvertOldJson(string inputPath, string outputPath)
    {
        // Read the old JSON file
        string json = File.ReadAllText(inputPath);
        var oldChars = JsonSerializer.Deserialize<List<OldCharacter>>(json);

        // Map to new CharacterInfo structure
        var newChars = new List<CharacterInfo>();
        foreach (var oldChar in oldChars)
        {
            var newChar = new CharacterInfo
            {
                charId = oldChar.idChar,
                charName = oldChar.charName ?? "",
                rarity = SafeToInt(oldChar.rarity),
                costumes = new List<CostumeInfo>(),
                guest = null,
                prestigeSkin = null
            };

            if (oldChar.costume != null)
            {
                foreach (var oldCostume in oldChar.costume)
                {
                    newChar.costumes.Add(new CostumeInfo
                    {
                        costumeId =oldCostume.idCostume,
                        costumeName = oldCostume.costumeName ?? "",
                        releaseDate = oldCostume.releaseDate ?? "",
                        spine = oldCostume.spine ?? "",
                        cutscene = oldCostume.cutscene ?? ""
                    });
                }
            }

            newChars.Add(newChar);
        }
        
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        File.WriteAllText(outputPath, JsonSerializer.Serialize(newChars, options));
    }

    private static int SafeToInt(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            return 0;
        if (int.TryParse(s, out int result))
            return result;
        return 0;
    }

    
    private class OldCharacter
    {
        public string idChar { get; set; }
        public string charName { get; set; }
        public string rarity { get; set; }
        public List<OldCostume> costume { get; set; }
    }

    private class OldCostume
    {
        public string idCostume { get; set; }
        public string costumeName { get; set; }
        public string releaseDate { get; set; }
        public string spine { get; set; }
        public string cutscene { get; set; }
    }
}