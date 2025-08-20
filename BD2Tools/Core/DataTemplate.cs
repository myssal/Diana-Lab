namespace BD2Tools.Core;

public class CharacterInfo
{
    public string charId { get; set; }
    public string charName { get; set; }
    public int rarity { get; set; }
    public List<CostumeInfo> costumes { get; set; }
    public SpecialGuestInfo guest { get; set; }
    public PrestigeSkinInfo prestigeSkin { get; set; }
}

public class CostumeInfo
{
    public string costumeId { get; set; }
    public string costumeName { get; set; }
    public string releaseDate { get; set; }
    public string spine { get; set; }
    public string cutscene { get; set; }
}

public class SpecialGuestInfo
{
    public string releaseDate { get; set; }
    public string interact { get; set; }
}

public class PrestigeSkinInfo
{
    public string prestigeSkinName { get; set; }
    public string releaseDate { get; set; }
    public string spine { get; set; }
    public string interact { get; set; }
}