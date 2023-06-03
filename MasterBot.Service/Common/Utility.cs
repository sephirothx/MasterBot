using System;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
            var time  = DateTimeOffset.Now.ToUniversalTime();
            var start = new DateTimeOffset(time.Year, time.Month, time.Day, time.Hour, 0, 0, TimeSpan.Zero);

            return start.AddHours(-(time.Hour % GetTimeslotIntervalH()));
        }

        public DateTimeOffset GetNextTimeslotTime()
        {
            return GetLastTimeslotTime().AddHours(GetTimeslotIntervalH());
        }

        public int GetLastTimeslotNumber()
        {
            int key_number = int.Parse(_config["warzone:timeslot:keynumber"]);

            var last_time = GetLastTimeslotTime();
            var key_time  = DateTimeOffset.Parse(_config["warzone:timeslot:key-datetime"]).UtcDateTime;

            var diff = (int)(last_time - key_time).TotalHours / 4;

            return key_number + diff;
        }

        public string GetTimeslotLink(int n = 0)
        {
            int timeslot = GetLastTimeslotNumber() - Math.Abs(n);
            if (timeslot < 0) throw new ArgumentException("Timeslot not found");

            return "https://www.warzone.com/Clans/War" +
                   $"?ID={Getseason()}"         +
                   $"&Timeslot={timeslot}";
        }

        public bool IsLastTimeslotOfTheDay()
        {
            return GetNextTimeslotTime().Day != DateTime.UtcNow.Day;
        }

        public SocketTextChannel GetChannelFromConfig()
        {
            var id = ulong.Parse(_config["discord:ping-channel"]);
            var ch = _discord.GetChannel(id) as SocketTextChannel ?? throw new Exception($"Channel {id} is not valid.");

            return ch;
        }

        public ulong GetPingRoleIdFromConfig()
        {
            var id = ulong.Parse(_config["discord:ping-role"]);

            return id;
        }

        public ulong GetStartRoleIdFromConfig()
        {
            var id = ulong.Parse(_config["discord:start-role"]);

            return id;
        }

        public int Getseason()
        {
            int id = int.Parse(_config["warzone:season"]);

            return id;
        }

        public int GetTimeslotIntervalH()
        {
            int h = int.Parse(_config["warzone:timeslot:interval-h"]);

            return h;
        }

        public async Task IncrementTimeslotNumber(int increment = 1)
        {
            await UpdateAppsettings(o =>
                {
                    int key = int.Parse(o.warzone.timeslot.keynumber);
                    key += increment;
                    o.warzone.timeslot.keynumber = key.ToString();
                });
        }

        public async Task IncrementSeasonNumber(int increment = 1)
        {
            await UpdateAppsettings(o =>
            {
                int season = int.Parse(o.warzone.season);
                season += increment;
                o.warzone.season = season.ToString();
            });
        }

        public static async Task UpdateAppsettings(Action<dynamic> action)
        {
            var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            var json = await File.ReadAllTextAsync(appSettingsPath);

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.Converters.Add(new ExpandoObjectConverter());

            dynamic config = JsonConvert.DeserializeObject<ExpandoObject>(json, jsonSettings);

            action(config);

            var newJson = JsonConvert.SerializeObject(config, Formatting.Indented, jsonSettings);
            await File.WriteAllTextAsync(appSettingsPath, newJson);
        }
    }
}
