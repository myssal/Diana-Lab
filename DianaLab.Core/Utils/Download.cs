using System.Net;

namespace DianaLab.Core.Utils;

public class Download
{
    private static readonly HttpClient _httpClient = new HttpClient();
    
    public static bool DownloadFromURL(string url, string destinationPath)
    {
        try
        {
            using HttpResponseMessage response = _httpClient.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();

            using FileStream fs = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);
            response.Content.CopyToAsync(fs).Wait();
            return true;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error downloading file: {e.Message}");
            return false;
        }
    }
}