using System;

namespace GranDen.TimeLib.ClockShaft
{
    /// <summary>
    /// Mimic interface for <c>DateTime</c>
    /// </summary>
    public interface IDateTime
    {
        /// <summary>
        /// Act like <c>DateTime.Now</c> property
        /// </summary>
        DateTime Now { get; } 
        
        /// <summary>
        /// Act like <c>DateTime.UtcNow</c> property
        /// </summary>
        DateTime UtcNow { get; }
    }
}