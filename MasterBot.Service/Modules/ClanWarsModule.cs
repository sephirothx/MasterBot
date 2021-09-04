using System.Threading.Tasks;
using Discord.Commands;

namespace MasterBot.Service.Modules
{
    [Name("Clan Wars")]
    [Summary("Clan Wars specific commands")]
    public class ClanWarsModule : ModuleBase<SocketCommandContext>
    {
        [Command("freewin"), Alias("fw")]
        [Summary("Posts an explanation about free wins in Clan War.")]
        public async Task FreeWin()
        {
            var channel = Context.Channel;
            await channel.SendMessageAsync(@"You can only get a free win if all of these requirements are satisfied:
    1. No other player (or team) from your clan got a game
    2. No opponents could be found for you (or your team)");
        }
    }
}
