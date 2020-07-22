using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace GranDen.TimeLib.ClockShaft.Options
{
    /// <summary>
    /// Extension methods for facilitating <c>ClockWork.ShaftConfigurationFunc</c> configuration
    /// </summary>
    public static class ClockShaftConfigureAppExtension
    {
        /// <summary>
        /// Configure <c>ClockWork.ShaftConfigurationFunc</c> to apply at host application startup and reset it when stopped
        /// </summary>
        /// <param name="hostApplicationLifetime"></param>
        /// <param name="optionsAccessor"></param>
        /// <returns></returns>
        public static IHostApplicationLifetime ConfigureClockShaft(this IHostApplicationLifetime hostApplicationLifetime, IOptionsMonitor<ClockShaftOptions> optionsAccessor)
        {
            var clockShiftOptions = optionsAccessor.CurrentValue;
            
            return hostApplicationLifetime.ConfigureClockShaft(clockShiftOptions);
        }

        /// <summary>
        /// Configure <c>ClockWork.ShaftConfigurationFunc</c> to apply at host application startup and reset it when stopped
        /// </summary>
        /// <param name="hostApplicationLifetime"></param>
        /// <param name="clockShaftOptions"></param>
        /// <returns></returns>
        public static IHostApplicationLifetime ConfigureClockShaft(this IHostApplicationLifetime hostApplicationLifetime, ClockShaftOptions clockShaftOptions)
        {
            hostApplicationLifetime.ApplicationStarted.Register(() =>
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
            });

            hostApplicationLifetime.ApplicationStopped.Register(ClockWork.Reset);
            
            return hostApplicationLifetime;
        }
    }
}