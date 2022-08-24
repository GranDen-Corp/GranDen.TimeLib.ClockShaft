using System;

namespace GranDen.TimeLib.ClockShaft.Options
{
    /// <summary>
    /// Option class to facilitate <c>ClockWork.ShaftConfigurationFunc</c> configuration.
    /// </summary>
    public class ClockShaftOptions
    {
        /// <summary>
        /// Make clock shaft roll counter clockwise (forward) or clockwise (backward)
        /// </summary>
        public bool Backward { get; set; } = false;

        /// <summary>
        /// Clock shaft movement amount
        /// </summary>
        public TimeSpan ShiftTime { get; set; } = TimeSpan.Zero;
    }
}