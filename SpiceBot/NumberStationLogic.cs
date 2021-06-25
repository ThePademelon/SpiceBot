using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace SpiceBot
{
    internal class NumberStationLogic : SpiceLogic
    {
        public override Task HandleMessage(SocketMessage message)
        {
            if (!message.Content.Equals("!numberStation", StringComparison.InvariantCultureIgnoreCase)) return Task.CompletedTask;
            var guildUser = (IGuildUser) message.Author;
            return guildUser.VoiceChannel.ConnectAsync();
        }
    }
}