using DianaLab.Core.Services;
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
                services.AddTransient<AssetService>();
                services.AddTransient<CDNService>(provider =>
                {
                    var logger = provider.GetRequiredService<ILogger<CDNService>>();
                    return new CDNService(logger, version);
                });
                services.AddTransient<CharacterService>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Information);
            })
            .Build();

        var assetService = host.Services.GetRequiredService<AssetService>();
        assetService.ProcessAsset();
        //assetService.CheckDuplicate(@"F:\FullSetC\Game\Active\BrownDust\BrownDust2\ui\atlas");
        //assetService.NormalizeCostumeIcons(@"F:\FullSetC\Game\Active\BrownDust\BrownDust2\ui\icon\icon_costume");
        //CharacterService.RunAddCLI();
    }
}