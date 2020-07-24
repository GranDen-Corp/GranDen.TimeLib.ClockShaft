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
        private class TestDateTimeOffsetDto
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

            //Act
            var host = TestHostHelper.InitDemoTestHost(clockShaftJsonStr, async ctx =>
            {
                await ctx.Response.WriteAsync(
                    $"{{ now : \"{DateTimeOffset.Now}\", shift : \"{ClockWork.DateTimeOffset.Now}\"}}");
            });
            var client = host.GetTestClient();
            using var response = await client.GetAsync("/");

            //Assert
            response.EnsureSuccessStatusCode();

            var resultStr = await response.Content.ReadAsStringAsync();
            Assert.NotNull(resultStr);
            var resultObj = JsonConvert.DeserializeObject<TestDateTimeOffsetDto>(resultStr);

            Assert.Equal(resultObj.Now, resultObj.Shift.Add(TimeSpan.FromHours(1.0)), new DateTimeOffsetComparator(10.0));
        }

        private class TestDateTimeDto
        {
            public DateTime Now { get; set; }
            public DateTime Shift { get; set; }
        }

        [Fact]
        public async Task TestNoConfigurationWillJustLikeNormalDateTime()
        {
            //Arrange
            const string clockShaftJsonStr = @"{}";

            //Act
            var host = TestHostHelper.InitDemoTestHost(clockShaftJsonStr, async ctx =>
            {
                await ctx.Response.WriteAsync(
                    $"{{ now : \"{DateTime.Now}\", shift : \"{ClockWork.DateTime.Now}\"}}");
            });
            var client = host.GetTestClient();
            using var response = await client.GetAsync("/");

            //Assert
            response.EnsureSuccessStatusCode();

            var resultStr = await response.Content.ReadAsStringAsync();
            Assert.NotNull(resultStr);
            var resultObj = JsonConvert.DeserializeObject<TestDateTimeDto>(resultStr);

            Assert.Equal(resultObj.Now, resultObj.Shift);
        }
    }
}