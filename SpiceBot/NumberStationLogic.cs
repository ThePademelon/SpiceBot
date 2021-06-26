using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace SpiceBot
{
    internal class NumberStationLogic : SpiceLogic, IDisposable
    {
        private readonly ILogger<DiscordBotHost> _logger;
        private readonly List<NumberStation> _runningStations = new();

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
                var audioClient = await voiceChannel.ConnectAsync();
                _runningStations.Add(new NumberStation(audioClient));
            }
            else
            {
                _logger.LogInformation($"Leaving {voiceChannel.Name} channel.");
                await voiceChannel.DisconnectAsync();
            }
        }

        public void Dispose()
        {
            foreach (var station in _runningStations)
            {
                station.Dispose();
            }
        }
    }
}