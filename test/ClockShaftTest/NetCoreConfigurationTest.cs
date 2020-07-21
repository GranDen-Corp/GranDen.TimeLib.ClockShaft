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

        private const string ClockShaftJsonStr = @"
{
    ""ClockShaft"" : {
        ""Backward"" : true,
        ""ShiftTimeSpan"" : ""00:00:00""
    }
}
";
        #endregion

        [Fact]
        public void TestLoadClockShaftConfiguration()
        {
            //Arrange
            var services = new ServiceCollection();
            services.ConfigureClockShaftOption(InitConfiguration(ClockShaftJsonStr).GetSection("ClockShaft"));
            
             //Act
             var builder = services.BuildServiceProvider();
             var clockShaftOptionsMonitor = builder.GetService<IOptionsMonitor<ClockShaftOptions>>();
             
             //Assert
             Assert.NotNull(clockShaftOptionsMonitor);
             Assert.True(clockShaftOptionsMonitor.CurrentValue.Backward);
             Assert.Equal(TimeSpan.Zero, clockShaftOptionsMonitor.CurrentValue.ShiftTimeSpan);

        }
        
        private static IConfigurationRoot InitConfiguration(string jsonStr)
        {
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonStr));
            return new ConfigurationBuilder().AddJsonStream(ms).Build();
        }
    }
}