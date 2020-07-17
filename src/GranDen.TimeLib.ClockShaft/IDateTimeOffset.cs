using System;

namespace GranDen.TimeLib.ClockShaft
{
    public interface IDateTimeOffset
    {
        DateTimeOffset Now { get; }
        
        DateTimeOffset UtcNow { get; }
    }
}