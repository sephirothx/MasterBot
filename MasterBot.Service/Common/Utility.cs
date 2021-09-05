using System;
using Discord;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MasterBot.Service.Common
{
    public class Utility
    {
        private readonly IConfiguration _config;

        public Utility(IConfiguration config)
        {
            _config = config;
        }

        public static LogLevel GetLogLevel(LogSeverity severity)
        {
            return severity switch
            {
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Error    => LogLevel.Error,
                LogSeverity.Warning  => LogLevel.Warning,
                LogSeverity.Info     => LogLevel.Information,
                LogSeverity.Debug    => LogLevel.Information,
                LogSeverity.Verbose  => LogLevel.Information,
                _                    => LogLevel.None
            };
        }

        public DateTimeOffset GetNextTimeslotTime()
        {
            int timeslot_interval_h = int.Parse(_config["warlight:timeslot:interval_h"]);

            var time  = DateTimeOffset.Now.ToUniversalTime();
            var start = new DateTimeOffset(time.Year, time.Month, time.Day, time.Hour, 0, 0, TimeSpan.Zero);

            return start.AddHours(timeslot_interval_h - time.Hour % timeslot_interval_h);
        }
    }
}
