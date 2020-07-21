using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace GranDen.TimeLib.ClockShaft.Options
{
    /// <summary>
    /// Extension methods for facilitating <c>ClockWork.ShaftConfigurationFunc</c> configuration
    /// </summary>
    public static class ClockShaftConfigureAppExtension
    {
        /// <summary>
        /// Configure <c>ClockWork.ShaftConfigurationFunc</c> 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="optionsAccessor"></param>
        /// <returns></returns>
        public static IApplicationBuilder ConfigureClockShaft(this IApplicationBuilder builder,
            IOptionsMonitor<ClockShaftOptions> optionsAccessor)
        {
            var clockShiftOptions = optionsAccessor.CurrentValue;

            return builder.ConfigureClockShaft(clockShiftOptions);
        }

        /// <summary>
        /// Configure <c>ClockWork.ShaftConfigurationFunc</c>
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="clockShaftOptions"></param>
        /// <returns></returns>
        public static IApplicationBuilder ConfigureClockShaft(this IApplicationBuilder builder,
            ClockShaftOptions clockShaftOptions)
        {
            
            ClockWork.ShaftConfigurationFunc = instance =>
            {
                var configTimeSpan = clockShaftOptions.ShiftTimeSpan;

                if (configTimeSpan <= TimeSpan.Zero)
                {
                    return instance;
                }

                instance.ShiftTimeSpan = configTimeSpan;

                if (clockShaftOptions.Backward)
                {
                    instance.Backward = true;
                }

                return instance;
            };

            return builder;
        }
    }
}