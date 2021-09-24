using System.IO;
using System.Text;
using GranDen.TimeLib.ClockShaft.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ClockShaftOptionsTests
{
    public static class TestHostHelper
    {
        public static IConfigurationRoot InitConfiguration(string jsonStr)
        {
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonStr));
            return new ConfigurationBuilder().AddJsonStream(ms).Build();
        }

        public static IHost InitDemoTestHost(string configJsonStr, RequestDelegate handler)
        {
            void ConfigurationDelegate(IConfigurationBuilder configurationBuilder)
            {
                configurationBuilder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(configJsonStr)));
            }

            var hostBuilder = new HostBuilder().ConfigureWebHost(webHost =>
            {
                webHost.UseTestServer()
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.ConfigureClockShaftOption(hostContext.Configuration.GetSection("ClockShaft"));
                    })
                    .Configure((context, appBuilder) =>
                    {
                        appBuilder.Run(handler);

                        using var scope = appBuilder.ApplicationServices.CreateScope();
                        var applicationLifeTime = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
                        var clockShaftOptionAccessor = scope.ServiceProvider.GetService<IOptionsMonitor<ClockShaftOptions>>();
                        applicationLifeTime.ConfigureClockShaft(clockShaftOptionAccessor);
                    });
            }).ConfigureHostConfiguration(ConfigurationDelegate);

            return hostBuilder.Start();
        }
    }
}