using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GranDen.TimeLib.ClockShaft;
using GranDen.TimeLib.ClockShaft.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TestUtil;
using Xunit;

namespace ClockShaftOptionsTest
{
    public class GenericHostTest
    {
        class TestDto
        {
            public DateTimeOffset Now { get; set; }
            public DateTimeOffset Shift { get; set; }
        }

        [Fact]
        public async Task TestConfigureClockShaftExtensionMethod()
        {
            //Arrange
            const string clockShaftJsonStr = @"
{
    ""ClockShaft"" : {
        ""Backward"" : true,
        ""ShiftTime"" : ""01:00:00""
    }
}
";
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(clockShaftJsonStr));

            var hostBuilder = new HostBuilder().ConfigureWebHost(webHost =>
            {
                webHost.UseTestServer()
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.ConfigureClockShaftOption(hostContext.Configuration.GetSection("ClockShaft"));
                    })
                    .Configure((context, appBuilder) =>
                    {
                        appBuilder.Run(async ctx =>
                        {
                            await ctx.Response.WriteAsync(
                                $"{{ now : \"{DateTimeOffset.Now}\", shift : \"{ClockWork.DateTimeOffset.Now}\"}}");
                        });

                        using var scope = appBuilder.ApplicationServices.CreateScope();
                        var applicationLifeTime = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
                        var clockShaftOptionAccessor = scope.ServiceProvider.GetService<IOptionsMonitor<ClockShaftOptions>>();
                        applicationLifeTime.ConfigureClockShaft(clockShaftOptionAccessor);
                    });
            }).ConfigureHostConfiguration(configurationBuilder => { configurationBuilder.AddJsonStream(ms); });

            //Act
            var host = await hostBuilder.StartAsync();
            await ms.DisposeAsync();
            var client = host.GetTestClient();

            using var response = await client.GetAsync("/");

            //Assert
            response.EnsureSuccessStatusCode();

            var resultStr = await response.Content.ReadAsStringAsync();
            Assert.NotNull(resultStr);
            var resultObj = JsonConvert.DeserializeObject<TestDto>(resultStr);

            Assert.Equal(resultObj.Now, resultObj.Shift.Add(TimeSpan.FromHours(1.0)), new DateTimeOffsetComparator(10.0));
        }
    }
}