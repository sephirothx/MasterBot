﻿using System;
using System.Threading.Tasks;
using Discord.Commands;
using MasterBot.Service.Common;
using WarZone.WebClient;

namespace MasterBot.Service.Modules
{
    [Name("Clan Wars")]
    [Summary("Clan Wars specific commands")]
    [RequireRole("Masters")]
    public class ClanWarsModule : ModuleBase<SocketCommandContext>
    {
        private readonly ITimeslotsData _timeslots;
        private readonly Utility        _utility;

        public ClanWarsModule(ITimeslotsData timeslots, Utility utility)
        {
            _timeslots = timeslots;
            _utility   = utility;
        }

        [Command("freewin", true), Alias("fw")]
        [Summary("Posts an explanation about free wins in Clan War.")]
        public async Task FreeWin()
        {
            await ReplyAsync(Strings.FREE_WIN);
        }

        [Command("swarm", true)]
        [Summary("Posts an explanation about swarming templates in Clan War.")]
        public async Task Swarm()
        {
            await ReplyAsync(Strings.SWARM);
        }

        [Command("clanwars", true), Alias("clanwar", "cw")]
        [Summary("Posts useful information about Clan Wars")]
        public async Task ClanWarsInfo()
        {
            await ReplyAsync(Strings.CLAN_WAR);
        }

        [Command("jebait", true), Alias("bait", "jb")]
        [Summary("Posts an explanation about jebaiting in Clan War.")]
        public async Task Jebait()
        {
            await ReplyAsync(Strings.JEBAIT);
        }

        [Command("timeslot"), Alias("lastslot", "ts", "ls")]
        [Summary("Posts a link to the latest timeslot.")]
        public async Task Timeslot()
        {
            await ReplyAsync(_utility.GetTimeslotLink());
        }

        [Command("timeslot"), Alias("ts")]
        [Summary("Posts a link to a previous timeslot.")]
        public async Task Timeslot(int n)
        {
            try
            {
                await ReplyAsync(_utility.GetTimeslotLink(n));
            }
            catch (ArgumentException)
            {
                await ReplyAsync($"Were you even born back then {Context.User.Mention}?");
            }
        }

        [Command("templates")]
        [Summary("Posts the list of templates of the latest timeslot.")]
        public async Task Templates()
        {
            int timeslot  = _utility.GetLastTimeslotNumber();
            var templates = _timeslots.GetTimeslotTemplates(timeslot);

            await ReplyAsync(string.Join($"{Environment.NewLine}", templates));
        }

        [Command("templates")]
        [Summary("Posts the list of templates of a previous timeslot.")]
        public async Task Templates(int n)
        {
            int timeslot  = _utility.GetLastTimeslotNumber() - Math.Abs(n);
            var templates = _timeslots.GetTimeslotTemplates(timeslot);

            await ReplyAsync(string.Join($"{Environment.NewLine}", templates));
        }

        [Command("incrementtimeslot"), Alias("inc")]
        [Summary("Increases the timeslot number by 1")]
        public async Task IncrementTimeslot()
        {
            await _utility.IncrementTimeslotNumber();
        }

        [Command("decrementtimeslot"), Alias("dec")]
        [Summary("Decreases the timeslot number by 1")]
        public async Task DecrementTimeslot()
        {
            await _utility.IncrementTimeslotNumber(-1);
        }

        [Command("incrementseason"), Alias("incs")]
        [Summary("Increases the season number by 1")]
        public async Task IncrementSeason()
        {
            await _utility.IncrementSeasonNumber();
        }

        [Command("decrementseason"), Alias("decs")]
        [Summary("Decreases the season number by 1")]
        public async Task DecrementSeason()
        {
            await _utility.IncrementSeasonNumber(-1);
        }
    }
}
