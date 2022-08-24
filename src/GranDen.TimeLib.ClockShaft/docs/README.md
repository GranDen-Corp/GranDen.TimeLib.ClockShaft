# GranDen.TimeLib.ClockShaft

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
Then when you access the `ClockWork.DateTime.Now` or `ClockWork.DateTimeOffset.Now` static property, it will return the [datetime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime) one hour earlier.

Once you want to clear the drift of the system clock, you can call the `ClockWork.Reset()` static method.