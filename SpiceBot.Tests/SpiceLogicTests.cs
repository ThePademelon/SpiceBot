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
        [InlineData("there are too many pot holes on main street.", Opinion.Cringe)]
        public async Task CanDetermineCorrectOpinion(string message, Opinion opinion)
        {
            var logic = new BasedCringeLogic(
                new NullLogger<DiscordBotHost>(),
                new SpiceContext()
            );

            var result = await logic.GetOpinion(message);

            Assert.StrictEqual(opinion, result);
        }
    }
}