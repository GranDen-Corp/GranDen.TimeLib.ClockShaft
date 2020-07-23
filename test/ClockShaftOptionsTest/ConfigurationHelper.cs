using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ClockShaftOptionsTest
{
    public static class ConfigurationHelper
    {
        public static IConfigurationRoot InitConfiguration(string jsonStr)
        {
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonStr));
            return new ConfigurationBuilder().AddJsonStream(ms).Build();
        }
    }
}