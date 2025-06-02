using System.Diagnostics;
using BD2Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

class Program
{
    static async Task Main(string[] args)
    {
        long version = 20250521211633; 
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services.AddTransient<AssetLogic>();
                services.AddTransient<CDN>(provider =>
                {
                    var logger = provider.GetRequiredService<ILogger<CDN>>();
                    return new CDN(logger, version);
                });
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Information);
            })
            .Build();

        // Resolve and run the service
        //var assetLogic = host.Services.GetRequiredService<AssetLogic>();
        //assetLogic.ProcessAsset();
        var cdn = host.Services.GetRequiredService<CDN>();
        //await cdn.ProcessCatalog();
        //cdn.ReadUrl();    
        //await cdn.Download(@"F:\FullSetC\Temp\bd2");
        var stopWatch = Stopwatch.StartNew();
        cdn.CalculateHash(@"F:\FullSetC\Temp\bd3", "hash.json");
        stopWatch.Stop();
        Console.WriteLine($"Time elapsed: {stopWatch.ElapsedMilliseconds} ms");
    }
}