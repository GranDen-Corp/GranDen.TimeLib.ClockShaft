using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GranDen.TimeLib.ClockShaft.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DemoWorkerService
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
                    services.ConfigureClockShaftOption(hostContext.Configuration.GetSection("ClockShaft"));
                    services.AddHostedService<Worker>();
                });
    }
}