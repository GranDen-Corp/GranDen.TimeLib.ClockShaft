using System;
using System.IO;
using System.Text;
using GranDen.TimeLib.ClockShaft.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace ClockShaftTest
{
    public class NetCoreConfigurationTest
    {
        #region Constant Definition

        #endregion

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
            services.ConfigureClockShaftOption(InitConfiguration(clockShaftJsonStr).GetSection("ClockShaft"));

            //Act
            var builder = services.BuildServiceProvider();
            var clockShaftOptionsMonitor = builder.GetService<IOptionsMonitor<ClockShaftOptions>>();

            //Assert
            Assert.NotNull(clockShaftOptionsMonitor);
            Assert.True(clockShaftOptionsMonitor.CurrentValue.Backward);
            Assert.Equal(TimeSpan.FromMinutes(1.0), clockShaftOptionsMonitor.CurrentValue.ShiftTime);
        }

        private static IConfigurationRoot InitConfiguration(string jsonStr)
        {
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonStr));
            return new ConfigurationBuilder().AddJsonStream(ms).Build();
        }
    }
}