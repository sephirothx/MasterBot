using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MasterBot.Service.Services
{
    public class StartupService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService      _commands;
        private readonly IConfigurationRoot  _config;
        private readonly IServiceProvider    _provider;
        private readonly ILogger             _logger;

        public StartupService(DiscordSocketClient discord,
                              CommandService commands,
                              IConfigurationRoot config,
                              IServiceProvider provider,
                              ILogger logger)
        {
            _discord  = discord;
            _commands = commands;
            _config   = config;
            _provider = provider;
            _logger   = logger;
        }

        public void Run()
        {
            _discord.MessageReceived += OnMessageReceivedAsync;

            _discord.Log  += LogAsync;
            _commands.Log += LogAsync;
        }

        private Task LogAsync(LogMessage msg)
        {
            _logger.Log((LogLevel)msg.Severity,
                        msg.Exception,
                        "{source}: {message}", msg.Source, msg.Message);

            return Task.CompletedTask;
        }

        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            if (s is not SocketUserMessage msg || msg.Author.Id == _discord.CurrentUser.Id)
            {
                return;
            }
            
            var context = new SocketCommandContext(_discord, msg);

            int argPos = 0;
            if (msg.HasStringPrefix(_config["discord:prefix"], ref argPos) ||
                msg.HasMentionPrefix(_discord.CurrentUser, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _provider);

                if (!result.IsSuccess)
                {
                    _logger.LogError("msg: {msg}, result: {result}", msg.ToString(), result.ToString());
                }
            }
        }
    }
}
