using System;
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
            var opinion = GetOpinion(message.Content);
            await VoiceOpinion(message, opinion);
        }

        private async Task VoiceOpinion(SocketMessage message, Opinion opinion)
        {
            switch (opinion)
            {
                case Opinion.Based:
                case Opinion.Cringe:
                    _logger.LogInformation("Telling {0} that their opinion was {1}.", message.Author.Username, opinion);
                    await message.Channel.SendMessageAsync(opinion.ToString(), messageReference: new MessageReference(message.Id));
                    break;
                case Opinion.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(opinion));
            }
        }

        private Opinion GetOpinion(string messageContent)
        {
            // TODO: This really should be a query of some sort for performance
            foreach (var thing in _spiceContext.Things)
            {
                foreach (var statement in _spiceContext.Statements)
                {
                    if (messageContent.Contains(string.Format(statement.Format, thing.Name), StringComparison.InvariantCultureIgnoreCase))
                    {
                        return GetOpinion(statement, thing);
                    }
                }
            }

            return Opinion.None;
        }

        private static Opinion GetOpinion(Statement messageContent, Thing thing)
        {
            var isBased = thing.IsBased;
            if (messageContent.Negates) isBased = !isBased;
            return isBased ? Opinion.Based : Opinion.Cringe;
        }
    }
}