using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace DianaLab.Core.Services;

public class CharacterService
{
    public static void ConvertOldJson(string inputPath, string outputPath)
    {
        string json = File.ReadAllText(inputPath);
        var oldChars = JsonSerializer.Deserialize<List<OldCharacter>>(json);

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
                        costumeId = oldCostume.idCostume,
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
        public string? idChar { get; set; }
        public string? charName { get; set; }
        public string? rarity { get; set; }
        public List<OldCostume>? costume { get; set; }
    }

    private class OldCostume
    {
        public string? idCostume { get; set; }
        public string? costumeName { get; set; }
        public string? releaseDate { get; set; }
        public string? spine { get; set; }
        public string? cutscene { get; set; }
    }

    private static List<CharacterInfo> LoadJson(string filePath)
    {
        string json = File.ReadAllText(filePath);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<List<CharacterInfo>>(json, options) ?? new List<CharacterInfo>();
    }

    public static List<CharacterInfo> LoadCharacters(string filePath)
    {
        return LoadJson(filePath);
    }

    private static void SaveJson(string filePath, List<CharacterInfo> characters)
    {
        var saveOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        File.WriteAllText(filePath, JsonSerializer.Serialize(characters, saveOptions));
    }

    private static string GetOutputPath(string filePath, bool overwrite)
    {
        if (overwrite) return filePath;
        string folder = Path.GetDirectoryName(filePath) ?? "";
        string name = Path.GetFileNameWithoutExtension(filePath);
        string ext = Path.GetExtension(filePath);
        return Path.Combine(folder, $"{name}_new{ext}");
    }

    private static string Prompt(string message, string defaultValue = "")
    {
        Console.Write($"{message}{(string.IsNullOrEmpty(defaultValue) ? "" : $" [{defaultValue}]")}: ");
        string? input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? defaultValue : input.Trim();
    }

    private static int PromptInt(string message, int defaultValue = 0)
    {
        Console.Write($"{message}{(defaultValue != 0 ? $" [{defaultValue}]" : "")}: ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
            return defaultValue;

        if (int.TryParse(input.Trim(), out int result))
            return result;

        Console.WriteLine("Invalid input. Please enter a valid integer.");
        return PromptInt(message, defaultValue);
    }

    public static void RunAddCLI()
    {
        Console.WriteLine("Enter the path to the JSON file:");
        string? filePath = Console.ReadLine()?.Trim() ?? "";
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found.");
            return;
        }

        var characters = LoadJson(filePath);

        while (true)
        {
            Console.WriteLine("\nSelect an option:");
            Console.WriteLine("0 - Exit");
            Console.WriteLine("1 - Add new character");
            Console.WriteLine("2 - Add new costume");
            Console.WriteLine("3 - Add special guest");
            Console.WriteLine("4 - Add prestige skin");
            Console.Write("Choice: ");
            string? choice = Console.ReadLine()?.Trim() ?? "";

            if (choice == "0")
            {
                Console.WriteLine("Exiting...");
                break;
            }

            bool overwrite = Prompt("Overwrite existing file? (y/n)", "n").ToLower() == "y";

            switch (choice)
            {
                case "1":
                    AddNewCharacter(characters);
                    break;
                case "2":
                    AddNewCostume(characters);
                    break;
                case "3":
                    AddSpecialGuest(characters);
                    break;
                case "4":
                    AddPrestigeSkin(characters);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    continue;
            }
            
            // sort based on id before rewriting into the file
            var sorted = characters
                .OrderBy(c =>
                {
                    if (int.TryParse(c.charId, out int result))
                        return result;
                    return int.MaxValue;
                })
                .ToList();
            
            SaveJson(GetOutputPath(filePath, overwrite), sorted);
            Console.WriteLine("Changes saved successfully.");
        }
    }

    private static void AddNewCharacter(List<CharacterInfo> characters)
    {
        string charId = Prompt("Enter Character ID");
        if (characters.Any(c => c.charId == charId))
        {
            Console.WriteLine("Character ID already exists.");
            return;
        }

        var newChar = new CharacterInfo
        {
            charId = charId,
            charName = Prompt("Enter Character Name"),
            rarity = PromptInt("Enter Rarity"),
            costumes = new List<CostumeInfo>(),
            guest = null,
            prestigeSkin = null
        };

        characters.Add(newChar);
        Console.WriteLine("New character added.");
    }

    private static void AddNewCostume(List<CharacterInfo> characters)
    {
        string charId = Prompt("Enter Character ID");
        var character = characters.FirstOrDefault(c => c.charId == charId);

        if (character == null)
        {
            Console.WriteLine("Character not found. Creating new one...");
            character = new CharacterInfo
            {
                charId = charId,
                charName = Prompt("Enter Character Name"),
                rarity = PromptInt("Enter Rarity"),
                costumes = new List<CostumeInfo>(),
                guest = null,
                prestigeSkin = null
            };
            characters.Add(character);
        }

        string tempPrompt = Prompt("Enter Release Date (YYYY-MM-DD or 0 if UNRELEASED)");

        var costume = new CostumeInfo
        {
            costumeId = Prompt("Enter Costume ID"),
            costumeName = Prompt("Enter Costume Name"),
            releaseDate = tempPrompt == "0" ? "UNRELEASED" : tempPrompt,
            spine = Prompt("Enter Spine"),
            cutscene = Prompt("Enter Cutscene")
        };

        if (character.costumes == null)
            character.costumes = new List<CostumeInfo>();

        var existing = character.costumes.FirstOrDefault(c => c.costumeId == costume.costumeId);
        if (existing != null)
            character.costumes[character.costumes.IndexOf(existing)] = costume;
        else
            character.costumes.Add(costume);

        Console.WriteLine("Costume added/updated.");
    }

    private static void AddSpecialGuest(List<CharacterInfo> characters)
    {
        string charId = Prompt("Enter Character ID");
        var character = characters.FirstOrDefault(c => c.charId == charId);

        if (character == null)
        {
            Console.WriteLine("Character not found. Creating new one...");
            character = new CharacterInfo
            {
                charId = charId,
                charName = Prompt("Enter Character Name"),
                rarity = PromptInt("Enter Rarity"),
                costumes = new List<CostumeInfo>(),
                guest = null,
                prestigeSkin = null
            };
            characters.Add(character);
        }

        string tempPrompt = Prompt("Enter Guest Release Date (YYYY-MM-DD or 0 if UNRELEASED)");

        character.guest = new SpecialGuestInfo
        {
            releaseDate = tempPrompt == "0" ? "UNRELEASED" : tempPrompt,
            interact = Prompt("Enter Guest Interact")
        };

        Console.WriteLine("Special guest added/updated.");
    }

    private static void AddPrestigeSkin(List<CharacterInfo> characters)
    {
        string charId = Prompt("Enter Character ID");
        var character = characters.FirstOrDefault(c => c.charId == charId);

        if (character == null)
        {
            Console.WriteLine("Character not found. Creating new one...");
            character = new CharacterInfo
            {
                charId = charId,
                charName = Prompt("Enter Character Name"),
                rarity = PromptInt("Enter Rarity"),
                costumes = new List<CostumeInfo>(),
                guest = null,
                prestigeSkin = null
            };
            characters.Add(character);
        }

        string tempPrompt = Prompt("Enter Prestige Skin Release Date (YYYY-MM-DD or 0 if UNRELEASED)");

        character.prestigeSkin = new PrestigeSkinInfo
        {
            prestigeSkinName = Prompt("Enter Prestige Skin Name"),
            releaseDate = tempPrompt == "0" ? "UNRELEASED" : tempPrompt,
            spine = Prompt("Enter Prestige Skin Spine"),
            interact = Prompt("Enter Prestige Skin Interact")
        };

        Console.WriteLine("Prestige skin added/updated.");
    }
}

public class CharacterInfo
{
    public string? charId { get; set; }
    public string? charName { get; set; }
    public int rarity { get; set; }
    public List<CostumeInfo>? costumes { get; set; }
    public SpecialGuestInfo? guest { get; set; }
    public PrestigeSkinInfo? prestigeSkin { get; set; }
}

public static class L2DManager
{
    public static List<L2DInfo> GetL2DAssets(List<CharacterInfo> characters)
    {
        var l2dAssets = new List<L2DInfo>();

        foreach (var character in characters)
        {
            if (character.costumes != null)
            {
                foreach (var costume in character.costumes)
                {
                    if (!string.IsNullOrEmpty(costume.spine))
                    {
                        l2dAssets.Add(new L2DInfo
                        {
                            id = costume.costumeId,
                            name = $"{character.charName}: {costume.costumeName}",
                            l2d = costume.spine,
                            l2dTags = new List<L2DTag> { L2DTag.Costume_Idle, L2DTag.Costume },
                            releaseDate = costume.releaseDate
                        });
                    }

                    if (!string.IsNullOrEmpty(costume.cutscene))
                    {
                        l2dAssets.Add(new L2DInfo
                        {
                            id = costume.costumeId,
                            name = $"{character.charName}: {costume.costumeName}",
                            l2d = costume.cutscene,
                            l2dTags = new List<L2DTag> { L2DTag.Costume_Cutscene, L2DTag.Costume },
                            releaseDate = costume.releaseDate
                        });
                    }
                }
            }

            if (character.guest != null)
            {
                l2dAssets.Add(new L2DInfo
                {
                    id = character.charId,
                    name = $"Special guest: {character.charName}",
                    l2d = character.guest.interact,
                    l2dTags = new List<L2DTag> { L2DTag.Dating },
                    releaseDate = character.guest.releaseDate
                });
            }

            if (character.prestigeSkin != null)
            {
                if (!string.IsNullOrEmpty(character.prestigeSkin.spine))
                {
                    l2dAssets.Add(new L2DInfo
                    {
                        id = character.prestigeSkin.spine.Replace("char", ""),
                        name = $"{character.charName}: {character.prestigeSkin.prestigeSkinName}",
                        l2d = character.prestigeSkin.spine,
                        l2dTags = new List<L2DTag> { L2DTag.Prestige_Idle, L2DTag.Prestige_Skin },
                        releaseDate = character.prestigeSkin.releaseDate
                    });
                }

                if (!string.IsNullOrEmpty(character.prestigeSkin.interact))
                {
                    l2dAssets.Add(new L2DInfo
                    {
                        id = character.prestigeSkin.spine?.Replace("char", ""),
                        name = $"{character.charName}: {character.prestigeSkin.prestigeSkinName}",
                        l2d = character.prestigeSkin.interact,
                        l2dTags = new List<L2DTag> { L2DTag.Prestige_Interact, L2DTag.Prestige_Skin },
                        releaseDate = character.prestigeSkin.releaseDate
                    });
                }
            }
        }

        return l2dAssets;
    }
}

public class CostumeInfo
{
    public string? costumeId { get; set; }
    public string? costumeName { get; set; }
    public string? releaseDate { get; set; }
    public string? spine { get; set; }
    public string? cutscene { get; set; }
}

public class SpecialGuestInfo
{
    public string? releaseDate { get; set; }
    public string? interact { get; set; }
}

public class PrestigeSkinInfo
{
    public string? prestigeSkinName { get; set; }
    public string? releaseDate { get; set; }
    public string? spine { get; set; }
    public string? interact { get; set; }
}

public class L2DInfo
{
    public string id { get; set; }
    public string name { get; set; }
    public string l2d { get; set; }
    public List<L2DTag> l2dTags { get; set; }
    public string releaseDate { get; set; }
}

public enum L2DTag
{
    Prestige_Interact,
    Prestige_Skin,
    Dating,
    Prestige_Idle,
    Costume,
    Costume_Idle,
    Costume_Cutscene,
    SpecialIllust
}