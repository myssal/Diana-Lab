namespace DianaLab.Core.Model;

public class Config
{
    public string Input { get; set; }
    public string Temp { get; set; }
    public string Output { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string AssetStudio { get; set; }
    public string UnityVersion { get; set; }
    public bool IsCopyToTemp { get; set; }
    public bool IsWriteUpdateFilesList { get; set; }
    public List<string> Types { get; set; }
    
    public bool ExtractAsset { get; set; }
    public bool DeleteRedundant { get; set; }
    public bool RenameSpine { get; set; }
    public bool SortAsset { get; set; }
    public bool SortSpine { get; set; }
    public bool OrganizeSpine { get; set; }
    public bool ResizeSpineTextures { get; set; }
    public bool SortAtlas { get; set; }
    public bool NormalizeCostumeName { get; set; }

    public Config(string input, string temp, string output, string startDate, string endDate,
        string assetStudio, string unityVersion, bool isCopyToTemp,
        bool isWriteUpdateFilesList, bool extractAsset, bool deleteRedundant,
        bool renameSpine, bool sortAsset, bool sortSpine, bool organizeSpine,
        bool resizeSpineTextures, bool sortAtlas, bool normalizeCostumeName,
        List<string> types)
    {
        Input = input;
        Temp = temp;
        Output = output;
        StartDate = startDate;
        EndDate = endDate;
        AssetStudio = assetStudio;
        UnityVersion = unityVersion;
        IsCopyToTemp = isCopyToTemp;
        IsWriteUpdateFilesList = isWriteUpdateFilesList;
        ExtractAsset = extractAsset;
        DeleteRedundant = deleteRedundant;  
        RenameSpine = renameSpine;
        SortAsset = sortAsset;
        SortSpine = sortSpine;
        OrganizeSpine = organizeSpine;
        ResizeSpineTextures = resizeSpineTextures;
        SortAtlas = sortAtlas;
        NormalizeCostumeName = normalizeCostumeName;
        Types = types ?? new List<string>();
    }

    public Config()
    {
        
    }
}
