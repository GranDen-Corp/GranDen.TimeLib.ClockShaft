# GranDen.TimeLib.ClockShaft.Options

[![Build Status](https://dev.azure.com/GranDen-Corp/GranDen.TimeLib.ClockShaft/_apis/build/status/GranDen-Corp.GranDen.TimeLib.ClockShaft?branchName=dev)](https://dev.azure.com/GranDen-Corp/GranDen.TimeLib.ClockShaft/_build/latest?definitionId=36&branchName=dev)&nbsp;&nbsp;
[![Nuget](https://img.shields.io/nuget/v/GranDen.TimeLib.ClockShaft.Options)](https://www.nuget.org/packages/GranDen.TimeLib.ClockShaft.Options)

Library provides [ASP.NET Core Options pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options) helper method and class for [GranDen.TimeLib.ClockShaft](https://www.nuget.org/packages/GranDen.TimeLib.ClockShaft/) library,

To apply on ASP.NET Core Web and Generic Host project:

1. Define addition setting like following:
    
    ```json
    "ClockShaft" : {
        "Backward": true,
        "ShiftTime": "01:30:00"
    }
    ```
2. Bind that setting to .NET Core's option using `ConfigureClockShaftOption()` extension method:
    ```cs
   services.ConfigureClockShaftOption(Configuration.GetSection("ClockShaft"));
    ```

Then you should be able to use `ClockWork.DateTime.Now` property to get the datetime one and half hour earlier.

Besides, if you want to apply setting when app/service startup, then reset after app/service shutdown, you can apply following code on initialization:
```csharp
using (var scope = appBuilder.ApplicationServices.CreateScope())
{
   var applicationLifeTime = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
   var clockShaftOptionAccessor = scope.ServiceProvider.GetService<IOptionsMonitor<ClockShaftOptions>>();
   applicationLifeTime.ConfigureClockShaft(clockShaftOptionAccessor);
}
```
