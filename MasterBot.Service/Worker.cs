using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MasterBot.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker>  _logger;
        private readonly IConfiguration   _config;
        private readonly IServiceProvider _services;

        public Worker(ILogger<Worker> logger, IConfiguration config, IServiceProvider services)
        {
            _logger   = logger;
            _config   = config;
            _services = services;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
