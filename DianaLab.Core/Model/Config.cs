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

    public Config(string input, string temp, string output, string startDate, string endDate, string assetStudio, string unityVersion, bool isCopyToTemp, bool isWriteUpdateFilesList, List<string> types)
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
        Types = types ?? new List<string>();
    }

    public Config()
    {
        
    }
}
