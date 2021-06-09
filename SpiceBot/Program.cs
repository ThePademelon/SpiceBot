using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SpiceBot
{
    internal class Program
    {
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
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