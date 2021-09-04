using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MasterBot.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace MasterBot.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                Log.Logger.Fatal(e, "Couldn't start the service");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                 {
                     Log.Logger = new LoggerConfiguration()
                                 .ReadFrom.Configuration(hostContext.Configuration)
                                 .CreateLogger();
                     ConfigureServices(services);
                     services.AddHostedService<Worker>();
                 }).UseSerilog();

        public static void ConfigureServices(IServiceCollection services)
        {
            var client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel            = LogSeverity.Verbose,
                MessageCacheSize    = 100,
                ExclusiveBulkDelete = true
            });

            var commands = new CommandService(new CommandServiceConfig
            {
                LogLevel       = LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async
            });

            services.AddSingleton(client)
                    .AddSingleton(commands)
                    .AddSingleton<StartupService>()
                    .AddSingleton<SchedulerService>();
        }
    }
}
