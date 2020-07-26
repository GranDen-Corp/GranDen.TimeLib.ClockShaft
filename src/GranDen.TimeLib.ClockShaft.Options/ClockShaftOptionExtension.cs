using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GranDen.TimeLib.ClockShaft.Options
{
    /// <summary>
    /// Extension methods to configure <c>ClockShaftOption</c> options
    /// </summary>
    public static class ClockShaftOptionExtension
    {
        /// <summary>
        /// Configure <c>ClockShaftOption</c> service registration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="postConfigureAction"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureClockShaftOption(this IServiceCollection services,
            IConfiguration configuration, Action<ClockShaftOptions> postConfigureAction = null)
        {
            services.AddOptions<ClockShaftOptions>().Bind(configuration);
            if (postConfigureAction != null)
            {
                services.PostConfigure(postConfigureAction);
            }

            return services;
        }
    }
}