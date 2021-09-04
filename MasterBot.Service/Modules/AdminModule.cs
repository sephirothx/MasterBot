using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MasterBot.Service.Modules
{
    [Name("Admin")]
    [Summary("Admin specific commands")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Makes the bot say something.")]
        public async Task Say([Remainder] string text)
        {
            var message = Context.Message;
            await message.DeleteAsync();

            await ReplyAsync(text);
        }

        [Command("say")]
        [Summary("Makes the bot say something in a specific channel.")]
        public async Task Say(IMessageChannel channel, [Remainder] string text)
        {
            var message = Context.Message;
            await message.DeleteAsync();

            await channel.SendMessageAsync(text);
        }
    }
}
