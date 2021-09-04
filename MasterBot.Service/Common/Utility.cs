using System;
using Discord;
using Microsoft.Extensions.Logging;

namespace MasterBot.Service.Common
{
    public static class Utility
    {
        public static LogLevel GetLogLevel(LogSeverity severity)
        {
            return severity switch
            {
                LogSeverity.Verbose  => LogLevel.Trace,
                LogSeverity.Debug    => LogLevel.Debug,
                LogSeverity.Info     => LogLevel.Information,
                LogSeverity.Warning  => LogLevel.Warning,
                LogSeverity.Error    => LogLevel.Error,
                LogSeverity.Critical => LogLevel.Critical,
                _                    => LogLevel.None
            };
        }

        public static DateTimeOffset GetNextTimeslotTime()
        {
            var time = DateTimeOffset.Now.ToUniversalTime();

            var start = new DateTimeOffset(time.Year, time.Month, time.Day, time.Hour, 0, 0, TimeSpan.Zero);
            return start.AddHours(4 - time.Hour % 4);
        }
    }
}
