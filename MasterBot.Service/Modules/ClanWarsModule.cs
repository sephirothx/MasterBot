﻿using System.Threading.Tasks;
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
            await ReplyAsync(@"Wait Private you don't know what a free win is and how to get it?
Then give me your ATTENTION and listen up while i explain.

You can only get a free win if all of these requirements are satisfied:
    1. No other player (or team) from your clan got a game on this template
    2. No opponents could be found for you (or your team)");
        }

        [Command("swarm")]
        [Summary("Posts an explanation about swarming templates in Clan War.")]
        public async Task Swarm()
        {
            await ReplyAsync(@"Swarming happens when multiple Masters join the same template.
It has several benefits:
    1. It ensures that, if some M'Hunters are queued, we're not just filtering the hardest opponents from them
    2. It makes us get direct matchups against M'Hunters
    3. It gives us a chance of getting 'easier' opponents on average");
        }
    }
}