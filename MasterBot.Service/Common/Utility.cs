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
    }
}
