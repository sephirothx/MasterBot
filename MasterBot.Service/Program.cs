using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MasterBot.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MasterBot.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                 {
                     ConfigureServices(services);
                     services.AddHostedService<Worker>();
                 });

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
                    .AddSingleton<StartupService>();
        }
    }
}
