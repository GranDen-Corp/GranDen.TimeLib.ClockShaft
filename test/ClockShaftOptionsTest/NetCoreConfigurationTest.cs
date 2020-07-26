using System;
using GranDen.TimeLib.ClockShaft.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace ClockShaftOptionsTest
{
    public class NetCoreConfigurationTest
    {
        [Fact]
        public void TestLoadClockShaftConfiguration()
        {
            //Arrange
            const string clockShaftJsonStr = @"
{
    ""ClockShaft"" : {
        ""Backward"" : true,
        ""ShiftTime"" : ""00:01:00""
    }
}
";
            var services = new ServiceCollection();
            services.ConfigureClockShaftOption(TestHostHelper.InitConfiguration(clockShaftJsonStr).GetSection("ClockShaft"));

            //Act
            var builder = services.BuildServiceProvider();
            var clockShaftOptionsMonitor = builder.GetService<IOptionsMonitor<ClockShaftOptions>>();

            //Assert
            Assert.NotNull(clockShaftOptionsMonitor);
            Assert.True(clockShaftOptionsMonitor.CurrentValue.Backward);
            Assert.Equal(TimeSpan.FromMinutes(1.0), clockShaftOptionsMonitor.CurrentValue.ShiftTime);
        }

    }
}