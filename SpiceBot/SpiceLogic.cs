using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace SpiceBot
{
    internal class SpiceLogic
    {
        private readonly ILogger<DiscordBotHost> _logger;

        public SpiceLogic(ILogger<DiscordBotHost> logger)
        {
            _logger = logger;
        }

        public async Task HandleMessage(SocketMessage message)
        {
            switch (GetOpinion(message.Content))
            {
                case Opinion.Based:
                    await message.Channel.SendMessageAsync("Based", messageReference: message.Reference);
                    break;
                case Opinion.Cringe:
                    await message.Channel.SendMessageAsync("Cringe", messageReference: message.Reference);
                    break;
                case Opinion.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static Opinion GetOpinion(string messageContent)
        {
            if (messageContent.Contains("I like choccy milk", StringComparison.InvariantCultureIgnoreCase))
            {
                return Opinion.Based;
            }

            if (messageContent.Contains("I like weed", StringComparison.InvariantCultureIgnoreCase))
            {
                return Opinion.Cringe;
            }

            return Opinion.None;
        }
    }
}