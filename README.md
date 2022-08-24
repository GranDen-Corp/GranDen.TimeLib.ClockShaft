# GranDen Time-related testing library

[![Build Status](https://img.shields.io/github/workflow/status/GranDen-Corp/GranDen.TimeLib.ClockShaft/build%20and%20test%20src)](https://github.com/GranDen-Corp/GranDen.TimeLib.ClockShaft/actions)

## GranDen.TimeLib.ClockShaft

[![Nuget](https://img.shields.io/nuget/v/GranDen.TimeLib.ClockShaft)](https://www.nuget.org/packages/GranDen.TimeLib.ClockShaft/)

DateTime utility library for easier testing in unit test or integration test, naming by the shaft mechanical part of clock:

![Clock diagram](./img/Assembly-Diagram-CLEAN.jpg)

Yoo can define a configure lambda function to customize the behavior like following:
```cs
 ClockWork.ShaftConfigurationFunc = shaft =>
            {
                shaft.Backward = true;
                shaft.ShiftTimeSpan = new TimeSpan(1, 0, 0);
                return shaft;
            };
```
Then when you access the `ClockWork.DateTime.Now` static property, it will return the [datetime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime) one hour earlier.

## GranDen.TimeLib.ClockShaft.Options

[![Nuget](https://img.shields.io/nuget/v/GranDen.TimeLib.ClockShaft.Options)](https://www.nuget.org/packages/GranDen.TimeLib.ClockShaft.Options)

Library provides [ASP.NET Core Options pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options) helper method and class for [GranDen.TimeLib.ClockShaft](https://www.nuget.org/packages/GranDen.TimeLib.ClockShaft/) library, see [test_sample](./test_sample) projects to see how to apply on ASP.NET Core Web and Generic Host project.

![Demo Web App](./img/DemoAspNetCore_screenshots01.png)
