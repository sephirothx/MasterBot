using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using MasterBot.Service.Common;
using MasterBot.Service.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;

namespace MasterBot.Service.Services
{
    public class SchedulerService
    {
        
        private readonly DiscordSocketClient _discord;
        private readonly CommandService      _commands;
        private readonly IConfiguration      _config;
        private readonly IServiceProvider    _provider;
        private readonly ILogger<Worker>     _logger;

        public SchedulerService(DiscordSocketClient discord,
                                CommandService commands,
                                IConfiguration config,
                                IServiceProvider provider,
                                ILogger<Worker> logger)
        {
            _discord  = discord;
            _commands = commands;
            _config   = config;
            _provider = provider;
            _logger   = logger;
        }

        public async Task ScheduleJobs()
        {
            var factory = new StdSchedulerFactory();

            var scheduler = await factory.GetScheduler();
            await scheduler.Start();

            await ScheduleNewTimeslotPing(scheduler);
            await ScheduleGamesStartingPing(scheduler);
        }

        private async Task ScheduleNewTimeslotPing(IScheduler scheduler)
        {
            var job = ActionJob.Create(TimeslotPing)
                               .WithIdentity("CW ping job")
                               .Build();

            var trigger = TriggerBuilder.Create()
                                        .WithIdentity("CW ping trigger")
                                        .WithSimpleSchedule(x => x.WithIntervalInHours(4).RepeatForever())
                                        .StartAt(Utility.GetNextTimeslotTime())
                                        .Build();

            await scheduler.ScheduleJob(job, trigger);
        }

        private async Task ScheduleGamesStartingPing(IScheduler scheduler)
        {
            var job = ActionJob.Create(GamesStartingPing)
                               .WithIdentity("Games Starting job")
                               .Build();

            var trigger = TriggerBuilder.Create()
                                        .WithIdentity("Games Starting trigger")
                                        .WithSimpleSchedule(x => x.WithIntervalInHours(4).RepeatForever())
                                        .StartAt(Utility.GetNextTimeslotTime().AddMinutes(10))
                                        .Build();

            await scheduler.ScheduleJob(job, trigger);
        }

        private void TimeslotPing()
        {
            var chan = GetChannelFromConfig();
            var role = GetRoleIdFromConfig();

            chan.SendMessageAsync($"<@&{role}>").Wait();
        }

        private void GamesStartingPing()
        {
            var chan = GetChannelFromConfig();
            var role = GetRoleIdFromConfig();

            chan.SendMessageAsync($"<@&{role}> Games are up, don't get booted!").Wait();
        }

        private SocketTextChannel GetChannelFromConfig()
        {
            var id = ulong.Parse(_config["discord:ping-channel"]);
            var ch = _discord.GetChannel(id) as SocketTextChannel ?? throw new Exception($"Channel {id} is not valid.");

            return ch;
        }

        private ulong GetRoleIdFromConfig()
        {
            var id = ulong.Parse(_config["discord:ping-role"]);

            return id;
        }
    }
}
