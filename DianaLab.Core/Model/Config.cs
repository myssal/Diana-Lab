using Newtonsoft.Json;

namespace DianaLab.Core.Model;

public class Config
{
    [JsonProperty("input")]
    public string Input { get; set; }
    [JsonProperty("temp")]
    public string Temp { get; set; }
    [JsonProperty("output")]
    public string Output { get; set; }
    [JsonProperty("startDate")]
    public string StartDate { get; set; }
    [JsonProperty("endDate")]
    public string EndDate { get; set; }
    [JsonProperty("assetStudio")]
    public string AssetStudio { get; set; }
    [JsonProperty("unityVersion")]
    public string UnityVersion { get; set; }
    [JsonProperty("isCopyToTemp")]
    public bool IsCopyToTemp { get; set; }
    [JsonProperty("isWriteUpdateFilesList")]
    public bool IsWriteUpdateFilesList { get; set; }
    [JsonProperty("types")]
    public List<string> Types { get; set; }
    
    [JsonProperty("extractAsset")]
    public bool ExtractAsset { get; set; }
    [JsonProperty("deleteRedundant")]
    public bool DeleteRedundant { get; set; }
    [JsonProperty("renameSpine")]
    public bool RenameSpine { get; set; }
    [JsonProperty("sortAsset")]
    public bool SortAsset { get; set; }
    [JsonProperty("sortSpine")]
    public bool SortSpine { get; set; }
    [JsonProperty("organizeSpine")]
    public bool OrganizeSpine { get; set; }
    [JsonProperty("resizeSpineTextures")]
    public bool ResizeSpineTextures { get; set; }
    [JsonProperty("sortAtlas")]
    public bool SortAtlas { get; set; }
    [JsonProperty("normalizeCostumeName")]
    public bool NormalizeCostumeName { get; set; }
    [JsonProperty("charInfoPath")]
    public string CharInfoPath { get; set; }
    [JsonProperty("loadLocale")]
    public bool LoadLocale { get; set; }

    public Config(string input, string temp, string output, string startDate, string endDate,
        string assetStudio, string unityVersion, bool isCopyToTemp,
        bool isWriteUpdateFilesList, bool extractAsset, bool deleteRedundant,
        bool renameSpine, bool sortAsset, bool sortSpine, bool organizeSpine,
        bool resizeSpineTextures, bool sortAtlas, bool normalizeCostumeName,
        List<string> types, string charInfoPath, bool loadLocale)
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
        CharInfoPath = charInfoPath;
        LoadLocale = loadLocale;
    }

    public Config()
    {
        
    }
}
