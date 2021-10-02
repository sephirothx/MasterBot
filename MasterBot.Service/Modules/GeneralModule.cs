using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using MasterBot.Service.Common;
using Microsoft.Extensions.Configuration;

namespace MasterBot.Service.Modules
{
    [Name("General")]
    [Summary("General commands")]
    public class GeneralModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commands;
        private readonly BotActions     _actions;
        private readonly IConfiguration _config;

        public GeneralModule(CommandService commands,
                             BotActions actions,
                             IConfiguration config)
        {
            _commands = commands;
            _config   = config;
            _actions  = actions;
        }

        [Command("help"), Alias("h")]
        [Summary("Posts a summary of the usable commands.")]
        public async Task Help()
        {
            var user = Context.User;

            string prefix = _config["discord:prefix"];
            var builder = new EmbedBuilder
            {
                Color = new Color(114, 137, 218),
                Description = $"Type `{prefix}help command-name` to get the description of a specific command."
            };
            
            foreach (var module in _commands.Modules)
            {
                string description = null;
                foreach (var cmd in module.Commands)
                {
                    var result = await cmd.CheckPreconditionsAsync(Context);
                    if (result.IsSuccess)
                    {
                        description += $"`{prefix}{cmd.Name}";
                        if (cmd.Parameters.Any()) description += $" {string.Join(',', cmd.Parameters.Select(p => p.Name))}";
                        description += $"`{Environment.NewLine}";
                    }
                }
                
                if (!string.IsNullOrWhiteSpace(description))
                {
                    builder.AddField(x =>
                    {
                        x.Name = module.Name;
                        x.Value = description;
                        x.IsInline = false;
                    });
                }
            }

            await user.SendMessageAsync("", false, builder.Build());
        }

        [Command("help"), Alias("h")]
        [Summary("Posts a description of a specific command.")]
        public async Task Help(string command)
        {
            var user   = Context.User;
            var result = _commands.Search(Context, command);

            string prefix = _config["discord:prefix"];
            if (!result.IsSuccess)
            {
                await ReplyAsync($"Sorry, I couldn't find a command like **{command}**.");
                return;
            }
            
            var builder = new EmbedBuilder
            {
                Color = new Color(114, 137, 218)
            };

            foreach (var match in result.Commands)
            {
                var cmd = match.Command;

                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases.Select(c => $"`{prefix}{c}`"));
                    x.Value = $"Parameters: {string.Join(", ", cmd.Parameters.Select(p => $"`{p.Name}`"))}\n" + 
                              $"Summary: {cmd.Summary}";
                    x.IsInline = false;
                });
            }

            await user.SendMessageAsync("", false, builder.Build());
        }

        [Command("noping")]
        [Summary("Removes the ping role for Games Starting")]
        [RequireRole("Masters")]
        public async Task NoPing()
        {
            if (Context.User is IGuildUser user)
            {
                ulong roleId = ulong.Parse(_config["discord:start-role"]);
                var   role   = Context.Guild.Roles.FirstOrDefault(r => r.Id == roleId);

                if (user.RoleIds.Contains(roleId) == false)
                {
                    await _actions.SendDirectMessageAsync(user, "You already have that role removed");
                    return;
                }

                await user.RemoveRoleAsync(role);
                await _actions.SendDirectMessageAsync(user, "Role removed");
            }
        }

        [Command("addping")]
        [Summary("Adds the ping role for Games Starting")]
        [RequireRole("Masters")]
        public async Task AddPing()
        {
            if (Context.User is IGuildUser user)
            {
                ulong roleId = ulong.Parse(_config["discord:start-role"]);
                var   role   = Context.Guild.Roles.FirstOrDefault(r => r.Id == roleId);

                if (user.RoleIds.Contains(roleId))
                {
                    await _actions.SendDirectMessageAsync(user, "You already have that role");
                    return;
                }

                await user.AddRoleAsync(role);
                await _actions.SendDirectMessageAsync(user, "Role granted");
            }
        }
    }
}
