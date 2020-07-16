using System;

namespace GranDen.TimeLib.ClockShaft
{
    public interface IDateTime
    {
        DateTime Now { get; } 
        DateTime UtcNow { get; }
    }
}