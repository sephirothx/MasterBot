using System.Threading.Tasks;
using Discord;

namespace MasterBot.Service.Common
{
    public class BotActions
    {
        public async Task SendDirectMessageAsync(IUser user, string message = null, Embed embed = null)
        {
            var channel = await user.GetOrCreateDMChannelAsync();

            await channel.SendMessageAsync(message, embed: embed);
            await channel.CloseAsync();
        }
    }
}
