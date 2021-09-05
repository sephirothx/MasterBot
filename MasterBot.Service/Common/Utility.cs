using System;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MasterBot.Service.Common
{
    public class Utility
    {
        private readonly IConfiguration      _config;
        private readonly DiscordSocketClient _discord;

        public Utility(IConfiguration config,
                       DiscordSocketClient discord)
        {
            _config  = config;
            _discord = discord;
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

        public DateTimeOffset GetLastTimeslotTime()
        {
            int timeslot_interval_h = int.Parse(_config["warlight:timeslot:interval_h"]);

            var time  = DateTimeOffset.Now.ToUniversalTime();
            var start = new DateTimeOffset(time.Year, time.Month, time.Day, time.Hour, 0, 0, TimeSpan.Zero);

            return start.AddHours(-(time.Hour % timeslot_interval_h));
        }

        public DateTimeOffset GetNextTimeslotTime()
        {
            int timeslot_interval_h = int.Parse(_config["warlight:timeslot:interval_h"]);

            return GetLastTimeslotTime().AddHours(timeslot_interval_h);
        }

        public SocketTextChannel GetChannelFromConfig()
        {
            var id = ulong.Parse(_config["discord:ping-channel"]);
            var ch = _discord.GetChannel(id) as SocketTextChannel ?? throw new Exception($"Channel {id} is not valid.");

            return ch;
        }

        public ulong GetRoleIdFromConfig()
        {
            var id = ulong.Parse(_config["discord:ping-role"]);

            return id;
        }
    }
}
