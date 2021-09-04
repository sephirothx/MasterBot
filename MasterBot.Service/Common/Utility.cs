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
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Error    => LogLevel.Error,
                LogSeverity.Warning  => LogLevel.Warning,
                LogSeverity.Info     => LogLevel.Information,
                LogSeverity.Debug    => LogLevel.Information,
                LogSeverity.Verbose  => LogLevel.Information,
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
