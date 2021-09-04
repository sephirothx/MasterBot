using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MasterBot.Service.Services;
using Microsoft.Extensions.Configuration;

namespace MasterBot.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker>     _logger;
        private readonly IConfiguration      _config;
        private readonly IServiceProvider    _services;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService      _commands;
        private readonly StartupService      _startup;
        private readonly SchedulerService    _scheduler;

        public Worker(ILogger<Worker> logger,
                      IConfiguration config,
                      IServiceProvider services,
                      DiscordSocketClient discord,
                      CommandService commands,
                      StartupService startup,
                      SchedulerService scheduler)
        {
            _logger    = logger;
            _config    = config;
            _services  = services;
            _discord   = discord;
            _commands  = commands;
            _startup   = startup;
            _scheduler = scheduler;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service started at {time}", DateTime.UtcNow);
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service stopped at {time}", DateTime.UtcNow);
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _startup.Run();

            await _discord.LoginAsync(TokenType.Bot, _config["discord:token"]);
            await _discord.StartAsync();

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            await _scheduler.ScheduleJobs();

            try
            {
                await Task.Delay(Timeout.Infinite, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation("Task canceled, closing the service.");
            }
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
                var result = await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess)
                {
                    _logger.LogError("msg: {msg}, result: {result}", msg.ToString(), result.ToString());
                }
            }
        }
    }
}
