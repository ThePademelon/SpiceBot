using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpiceBot.Data;

namespace SpiceBot
{
    internal class DiscordBotHost : IHostedService
    {
        private readonly ILogger<DiscordBotHost> _logger;
        private static DiscordSocketClient _client;
        private readonly SpiceLogic _logic;

        public DiscordBotHost(IConfiguration config, ILogger<DiscordBotHost> logger, SpiceContext spiceContext)
        {
            _logger = logger;
            _client = new DiscordSocketClient();
            _client.LoggedIn += ClientOnLoggedIn;
            _client.MessageReceived += ClientOnMessageReceived;
            _client.LoginAsync(TokenType.Bot, config["Token"]);
            _logic = new SpiceLogic(logger, spiceContext);
        }

        private async Task ClientOnMessageReceived(SocketMessage message)
        {
            if (Equals(message.Author.Id, _client.CurrentUser.Id))
            {
                // It's myself                
            }
            else
            {
                await _logic.HandleMessage(message);
            }
        }

        private static Task ClientOnLoggedIn() => _client.StartAsync();

        public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    }
}