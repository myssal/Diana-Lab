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
}
