# GranDen.TimeLib.ClockShaft

[![Nuget](https://img.shields.io/nuget/v/GranDen.TimeLib.ClockShaft)](https://www.nuget.org/packages/GranDen.TimeLib.ClockShaft/)

DateTime utility library for easier testing in unit test or integration test;

Yoo can define a configure lambda function to customize the behavior like following:
```csharp
 ClockWork.ShaftConfigurationFunc = shaft =>
            {
                shaft.Backward = true;
                shaft.ShiftTimeSpan = new TimeSpan(1, 0, 0);
                return shaft;
            };
```
Then when you access the `ClockWork.DateTime.Now` static property, it will return the [datetime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime) one hour earlier.