using System;

namespace GranDen.TimeLib.ClockShaft
{
    public interface IShaft
    {
        bool? Backward { get; set; }
        
        TimeSpan? ShiftTimeSpan { get; set; } 
    }
}