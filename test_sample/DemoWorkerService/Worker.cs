using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GranDen.TimeLib.ClockShaft;
using GranDen.TimeLib.ClockShaft.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DemoWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IOptionsMonitor<ClockShaftOptions> _optionsMonitor;

        public Worker(ILogger<Worker> logger,
            IOptionsMonitor<ClockShaftOptions> optionsMonitor,
            IHostApplicationLifetime applicationLifetime)
        {
            _logger = logger;
            _optionsMonitor = optionsMonitor;
            _applicationLifetime = applicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _applicationLifetime.ConfigureClockShaft(_optionsMonitor);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: \"{time}\", And the ClockWork.DateTimeOffset.Now is \"{now}\"",
                    DateTimeOffset.Now, ClockWork.DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}