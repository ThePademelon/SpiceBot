using System.Threading.Tasks;
using Discord.WebSocket;

namespace SpiceBot
{
    public class SpiceLogic
    {
        public virtual Task HandleMessage(SocketMessage message)
        {
            return Task.CompletedTask;
        }
    }
}