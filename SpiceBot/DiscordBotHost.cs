using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SpiceBot
{
    public class DiscordBotHost : IHostedService
    {
        private static DiscordSocketClient _client;
        private readonly SpiceLogic _logic;

        public DiscordBotHost(IConfiguration config, SpiceLogic basedCringeLogic)
        {
            _client = new DiscordSocketClient();
            _client.LoggedIn += ClientOnLoggedIn;
            _client.MessageReceived += ClientOnMessageReceived;
            _client.LoginAsync(TokenType.Bot, config["Token"]);
            _logic = basedCringeLogic;
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