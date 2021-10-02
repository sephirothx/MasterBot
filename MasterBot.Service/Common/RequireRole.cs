using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace MasterBot.Service.Common
{
    public class RequireRoleAttribute : PreconditionAttribute
    {
        private readonly string _name;

        public RequireRoleAttribute(string name) => _name = name;

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
                                                                       CommandInfo command,
                                                                       IServiceProvider services)
        {
            if (context.User is SocketGuildUser user)
            {
                return Task.FromResult(user.Roles.Any(r => r.Name == _name)
                                           ? PreconditionResult.FromSuccess()
                                           : PreconditionResult.FromError($"You must have the role {_name} to run this command."));
            }

            return Task.FromResult(PreconditionResult.FromError("You must be in a guild to run this command."));
        }
    }
}
