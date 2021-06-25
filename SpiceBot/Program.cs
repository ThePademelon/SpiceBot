using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpiceBot.Data;

namespace SpiceBot
{
    internal class Program
    {
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // TODO: Figure out how to switch logic based on config
                    // services.AddSingleton<SpiceLogic, BasedCringeLogic>();
                    services.AddSingleton<SpiceLogic, NumberStationLogic>();
                    services.AddHostedService<DiscordBotHost>();
                    services.AddEntityFrameworkSqlite();
                    services.AddDbContext<SpiceContext>();
                })
                .ConfigureAppConfiguration(app =>
                {
                    app.AddJsonFile("appsettings.json");
                    app.AddUserSecrets<DiscordBotHost>();
                });
        }

        private static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
    }
}