using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SpiceBot
{
    public class DiscordBotHost : IHostedService
    {
        private static DiscordSocketClient _client;
        private readonly SpiceLogic _logic;
        private readonly ILogger<DiscordBotHost> _logger;

        public DiscordBotHost(IConfiguration config, SpiceLogic logic, ILogger<DiscordBotHost> logger)
        {
            _logger = logger;
            _client = new DiscordSocketClient();
            _client.LoggedIn += ClientOnLoggedIn;
            _client.MessageReceived += ClientOnMessageReceived;
            _client.Log += ClientOnLog;
            _client.Connected += ClientOnConnected;
            _client.LoginAsync(TokenType.Bot, config["Token"]);
            _logic = logic;
        }

        private Task ClientOnConnected()
        {
            _logic.BotId = _client.CurrentUser.Id;
            return Task.CompletedTask;
        }

        private Task ClientOnLog(LogMessage arg)
        {
            _logger.Log(ToLogLevel(arg.Severity), arg.Exception, arg.Message);
            return Task.CompletedTask;
        }

        private static LogLevel ToLogLevel(LogSeverity logSeverity)
        {
            return logSeverity switch
            {
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Debug => LogLevel.Debug,
                LogSeverity.Error => LogLevel.Error,
                LogSeverity.Info => LogLevel.Information,
                LogSeverity.Verbose => LogLevel.Trace,
                LogSeverity.Warning => LogLevel.Warning,
                _ => throw new ArgumentOutOfRangeException(nameof(logSeverity), logSeverity, "There is no matching log level for the provided log severity.")
            };
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