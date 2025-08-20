using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BD2Tools.Core
{
    public partial class CharInfo
    {
        private static List<CharacterInfo> LoadJson(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<CharacterInfo>>(json, options) ?? new List<CharacterInfo>();
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
            return PromptInt(message, defaultValue); // retry on invalid input
        }

        public static void RunAddCLI()
        {
            Console.WriteLine("Enter the path to the JSON file:");
            string filePath = Console.ReadLine()?.Trim() ?? "";
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
                string choice = Console.ReadLine()?.Trim() ?? "";

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

                SaveJson(GetOutputPath(filePath, overwrite), characters);
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
}
