using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace SpiceBot
{
    internal class SpiceLogic
    {
        private readonly ILogger<DiscordBotHost> _logger;
        private readonly SpiceContext _spiceContext;

        public SpiceLogic(ILogger<DiscordBotHost> logger, SpiceContext spiceContext)
        {
            _logger = logger;
            _spiceContext = spiceContext;

            spiceContext.Database.EnsureCreated();
        }

        public async Task HandleMessage(SocketMessage message)
        {
            switch (await GetOpinion(message.Content))
            {
                case Opinion.Based:
                    await message.Channel.SendMessageAsync("Based", messageReference: new MessageReference(message.Id));
                    break;
                case Opinion.Cringe:
                    await message.Channel.SendMessageAsync("Cringe", messageReference: new MessageReference(message.Id));
                    break;
                case Opinion.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task<Opinion> GetOpinion(string messageContent)
        {
            var firstOrDefault = (await _spiceContext.Nouns.ToListAsync()).FirstOrDefault(x => messageContent.Contains($"I like {x.Name}", StringComparison.InvariantCultureIgnoreCase));
            if (firstOrDefault is null) return Opinion.None;
            return firstOrDefault.IsBased ? Opinion.Based : Opinion.Cringe;
        }
    }
}