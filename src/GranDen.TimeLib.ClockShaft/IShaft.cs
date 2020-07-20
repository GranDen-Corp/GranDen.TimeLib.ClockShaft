using System;

namespace GranDen.TimeLib.ClockShaft
{
    /// <summary>
    /// The Shaft (aka. Clock winding) property interface
    /// </summary>
    public interface IShaft
    {
        /// <summary>
        /// Move clock backward or forward, default null value means moving forward
        /// </summary>
        bool? Backward { get; set; }
        
        /// <summary>
        /// Clock drift amount, default null value means no drift
        /// </summary>
        TimeSpan? ShiftTimeSpan { get; set; } 
    }
}