using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace SpiceBot
{
    internal class Program
    {
        private static DiscordSocketClient _botClient;

        private static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                _botClient = new DiscordSocketClient();
                _botClient.LoggedIn += BotClientOnLoggedIn;
                _botClient.Disconnected += BotClientOnDisconnected;
                await _botClient.LoginAsync(TokenType.Bot, "token lol");
                await Task.Delay(int.MaxValue);
            }).GetAwaiter().GetResult();
        }

        private static Task BotClientOnDisconnected(Exception arg)
        {
            throw new NotImplementedException();
        }

        private static async Task BotClientOnLoggedIn()
        {
            await _botClient.StartAsync();
            Console.Write("Input channel:");
            var channelString = Console.ReadLine();

            var channel = _botClient.Guilds.SelectMany(x => x.Channels.Where(y => y.Name.Equals(channelString, StringComparison.InvariantCultureIgnoreCase))).Single();

            var textIn = string.Empty;
            while (!textIn.Equals("exit"))
            {
                var @in = Console.ReadLine();
                await ((SocketTextChannel) channel).SendMessageAsync(@in);
            }
        }
    }
}