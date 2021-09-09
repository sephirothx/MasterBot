using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MasterBot.Service.Modules
{
    [Name("Owner")]
    [Summary("Owner specific commands")]
    [RequireOwner]
    public class OwnerModule : ModuleBase<SocketCommandContext>
    {
        [Command("setstatus")]
        [Summary("Sets the status of the bot.")]
        public async Task SetStatus(ActivityType activity, [Remainder] string text)
        {
            await Context.Client.SetGameAsync(text, type: activity);
        }

        [Command("removestatus")]
        [Summary("Removes the status of the bot.")]
        public async Task RemoveStatus()
        {
            await Context.Client.SetActivityAsync(new Game("", ActivityType.CustomStatus));
        }
    }
}
