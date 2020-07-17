using System;

namespace GranDen.TimeLib.ClockShaft
{
    /// <summary>
    /// Mimic interface for <c>DateTimeOffset</c>
    /// </summary>
    public interface IDateTimeOffset
    {
        /// <summary>
        /// Act like <c>DateTimeOffset.Now</c>
        /// </summary>
        DateTimeOffset Now { get; }
        
        /// <summary>
        /// Act like <c>DateTimeOffset.UtcNow</c>
        /// </summary>
        DateTimeOffset UtcNow { get; }
    }
}