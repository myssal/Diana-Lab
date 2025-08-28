
namespace BD2Tools.Model;

public class PathJson
{
    public string keyword { get; set; }
    public string path { get; set; }

    public PathJson(string keyword, string path)
    {
        this.keyword = keyword;
        this.path = path;
    }
}
