using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using SpiceBot.Data;
using Xunit;

namespace SpiceBot.Tests
{
    public class SpiceLogicTests
    {
        [Theory]
        [InlineData("i hate math", Opinion.Cringe)]
        [InlineData("choccy milk", Opinion.Based)]
        [InlineData("tweed", Opinion.None)]
        public async Task CanDetermineCorrectOpinion(string message, Opinion opinion)
        {
            var logic = new SpiceLogic(
                new NullLogger<DiscordBotHost>(),
                new SpiceContext()
            );

            var result = await logic.GetOpinion(message);

            Assert.StrictEqual(opinion, result);
        }
    }
}