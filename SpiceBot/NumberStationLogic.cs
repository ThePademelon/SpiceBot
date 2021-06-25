using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace SpiceBot
{
    internal class NumberStationLogic : SpiceLogic
    {
        private readonly ILogger<DiscordBotHost> _logger;

        public NumberStationLogic(ILogger<DiscordBotHost> logger)
        {
            _logger = logger;
        }

        public override async Task HandleMessage(SocketMessage message)
        {
            if (!message.Content.StartsWith("!numberStation", StringComparison.InvariantCultureIgnoreCase)) return;
            var guildUser = (IGuildUser) message.Author;
            var voiceChannel = guildUser.VoiceChannel;
            if (voiceChannel is null)
            {
                await message.Channel.SendMessageAsync("You're not in a voice channel :(");
                return;
            }
            var voiceBotUser = await voiceChannel.GetUserAsync(BotId);
            if (voiceBotUser is null)
            {
                _logger.LogInformation($"Joining {voiceChannel.Name} channel.");
                await voiceChannel.ConnectAsync();
            }
            else
            {
                _logger.LogInformation($"Leaving {voiceChannel.Name} channel.");
                await voiceChannel.DisconnectAsync();
            }
        }
    }
}