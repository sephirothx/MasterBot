using System;
using System.Threading.Tasks;
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
        private readonly ILogger<Worker>     _logger;
        private readonly Utility             _utility;

        public SchedulerService(ILogger<Worker> logger,
                                Utility utility)
        {
            _logger  = logger;
            _utility = utility;
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
                                        .WithSimpleSchedule(x => x.WithIntervalInHours(_utility.GetTimeslotIntervalH())
                                                                  .RepeatForever())
                                        .StartAt(_utility.GetNextTimeslotTime().AddMilliseconds(200))
                                        .Build();

            var next = await scheduler.ScheduleJob(job, trigger);
            _logger.LogInformation("CW ping scheduled for {datetime}", next);
        }

        private async Task ScheduleGamesStartingPing(IScheduler scheduler)
        {
            var job = ActionJob.Create(GamesStartingPing)
                               .WithIdentity("Games Starting job")
                               .Build();

            var trigger = TriggerBuilder.Create()
                                        .WithIdentity("Games Starting trigger")
                                        .WithSimpleSchedule(x => x.WithIntervalInHours(_utility.GetTimeslotIntervalH())
                                                                  .RepeatForever())
                                        .StartAt(_utility.GetNextTimeslotTime().AddMinutes(10).AddMilliseconds(200))
                                        .Build();

            var next = await scheduler.ScheduleJob(job, trigger);
            _logger.LogInformation("Games Starting ping scheduled for {datetime}", next);
        }

        private void TimeslotPing()
        {
            try
            {
                _logger.LogInformation("Sending Timeslot ping");

                var chan = _utility.GetChannelFromConfig();
                var role = _utility.GetRoleIdFromConfig();

                chan.SendMessageAsync($"<@&{role}>").Wait();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Sending Timeslot ping failed");
            }
        }

        private void GamesStartingPing()
        {
            try
            {
                _logger.LogInformation("Sending Games Starting ping");

                var chan = _utility.GetChannelFromConfig();
                var role = _utility.GetRoleIdFromConfig();

                chan.SendMessageAsync($"<@&{role}> Games are up, don't get booted!").Wait();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Sending Games Starting ping failed");
            }
        }
    }
}
