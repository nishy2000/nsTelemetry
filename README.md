# nsTelemetry (NishySoftware.Telemetry.ApplicationInsights)

[Click here](https://nishy-software.com/ja/nstelemetry/) for Japanese page (日本語ページは[こちら](https://nishy-software.com/ja/nstelemetry/))

## Development status

[![Build Status (develop)](https://nishy-software.visualstudio.com/nsTelemetry/_apis/build/status/nishy2000.nsTelemetry?branchName=develop&label=develop)](https://nishy-software.visualstudio.com/nsTelemetry/_build/latest?definitionId=6&branchName=develop)
[![Build Status (master)](https://nishy-software.visualstudio.com/nsTelemetry/_apis/build/status/nishy2000.nsTelemetry?branchName=master&label=master)](https://nishy-software.visualstudio.com/nsTelemetry/_build/latest?definitionId=6&branchName=master)

[![Downloads](https://img.shields.io/nuget/dt/NishySoftware.Telemetry.ApplicationInsights.svg?style=flat-square&label=downloads)](https://www.nuget.org/packages/NishySoftware.Telemetry.ApplicationInsights/)
[![NuGet](https://img.shields.io/nuget/v/NishySoftware.Telemetry.ApplicationInsights.svg?style=flat-square)](https://www.nuget.org/packages/NishySoftware.Telemetry.ApplicationInsights/)
[![NuGet (pre)](https://img.shields.io/nuget/vpre/NishySoftware.Telemetry.ApplicationInsights.svg?style=flat-square&label=nuget-pre)](https://www.nuget.org/packages/NishySoftware.Telemetry.ApplicationInsights/)
[![Release](https://img.shields.io/github/release/nishy2000/nsTelemetry.svg?style=flat-square)](https://github.com/nishy2000/nsTelemetry/releases)
[![License](https://img.shields.io/github/license/nishy2000/nsTelemetry.svg?style=flat-square)](https://github.com/nishy2000/nsTelemetry/blob/master/LICENSE)

[![Issues](https://img.shields.io/github/issues/nishy2000/nsTelemetry.svg?style=flat-square)](https://github.com/nishy2000/nsTelemetry/issues)
[![Issues](https://img.shields.io/github/issues-closed/nishy2000/nsTelemetry.svg?style=flat-square)](https://github.com/nishy2000/nsTelemetry/issues?q=is%3Aissue+is%3Aclosed)
[![Pull Requests](https://img.shields.io/github/issues-pr/nishy2000/nsTelemetry.svg?style=flat-square)](https://github.com/nishy2000/nsTelemetry/pulls)
[![Pull Requests](https://img.shields.io/github/issues-pr-closed/nishy2000/nsTelemetry.svg?style=flat-square)](https://github.com/nishy2000/nsTelemetry/pulls?q=is%3Apr+is%3Aclosed)

## About

This library provides PC apps with the ability to send telemetry information to measure PC app usage and exceptions.
The destination for the telemetry information in this library is Azure Application Insight.
The library wraps the API of ApplicationInsights to make it easy to use from PC apps.
The PC apps are intended for Windows and Linux.

![Telemetry Graph example](images/nsTelemetry-AppInsight-sample-en.png "Example graph of the app usage timeline in Azure Application Insights")
*Example graph of the app usage timeline in Azure Application Insights*

## Installation

Install NuGet package(s).

```powershell
PM> Install-Package NishySoftware.Telemetry.ApplicationInsights
```

* [NishySoftware.Telemetry.ApplicationInsights](https://www.nuget.org/packages/NishySoftware.Telemetry.ApplicationInsights/) - nsTelemetry library.

## How to use

### nsTelemetry (NishySoftware.Telemetry.ApplicationInsights)

This library provides a static Telemetry class and an ITelemetry interface.
All public methods of the Telemetry class are exposed as static methods.
The Telemetry class allows for overall configuration and instantiation of the ITelemetry interface.
The ITelemetry interface can be used to configure telemetry data and send telemetry.

### Prepare

1. Get the InstrumentationKey from Azure Portal
   1. If you don't have an Azure account yet, go to the Azure portal and create an account with an active subscription. [Create an account for free](https://azure.microsoft.com/free/dotnet).
   2. Create an Application Insights resource in the [Azure portal](https://portal.azure.com/). For more information, please refer to [Microsoft's website](https://docs.microsoft.com/azure/azure-monitor/app/create-new-resource).
   3. Get the InstrumentationKey displayed in the "Overview" pane of the newly created resource page.
2. Install this nuget library ([`NisySoftware.Telemetry.ApplicationInsights`](https://www.nuget.org/packages/NishySoftware.Telemetry.ApplicationInsights/)) in the target app project.
3. Once the project is built, the `ApplicationInsights.config` file will be added to the project.
4. Set the InstrumentKey obtained from the Azure portal to the content of the `InstrumentKey` tag in the `ApplicationInsights.config` file.
   If you do not want to set the InstrumentationKey in the `ApplicationInsights.config` file, you can specify it in the source code.

### Implementation

1. Setup the global common telemetry data (properties/metrics) if necessary.
2. If the InstrumentationKey has not been set in the `ApplicationInsights.config` file, set the InstrumentationKey using `SetInstrumentationKey()`.
3. Create an instance of `ITelemetry`.
4. Setup the global custom telemetry data (properties/metrics) if necessary.
5. Set the DeveloperMode with `EnableDeveloperMode()` if necessary.
6. Send telemetry using the `TrackEvent()` / `TrackPageView()` / `TrackException()` methods of the `ITelemetry` interface.

## Example

```csharp
namespace nsTelemetryAI.Sample
{
    using NishySoftware.Telemetry;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        #region Fields
        DateTime _startupDateTime = DateTime.UtcNow;
        bool _sentEventExit = false;
        #endregion Fields

        #region Properties

        #region Telemetry
        static ITelemetry _telemetry;
        public static ITelemetry Telemetry
        {
            get
            {
                return LazyInitializer.EnsureInitialized<ITelemetry>(ref _telemetry, () =>
                {
                    // [en] Setting TelemetryDataFlags takes time the first time, so set it asynchronously.
                    Task.Run(() =>
                    {
                        // [en] Setup common global properties
                        NishySoftware.Telemetry.ApplicationInsights.Telemetry.CommonDataKinds = TelemetryDataKinds.All;
                    });

                    // [en] If you do not place the InstrumentationKey in ApplicationInsights.config file,
                    // setup the InstrumentationKey using SetInstrumentationKey().

                    // NishySoftware.Telemetry.ApplicationInsights.Telemetry.SetInstrumentationKey("your InstrumentationKey");

                    // [en] Create an instance of the telemetry interface
                    var telemetry = NishySoftware.Telemetry.ApplicationInsights.Telemetry.CreateTelemetry();

                    // [en] Add custom global property if you need
                    var userDomainName = Environment.UserDomainName;
                    lock (telemetry.GlobalSyncObject)
                    {
                        var prop = telemetry.GlobalProperties;
                        if (!prop.ContainsKey("UserDomainName"))
                        {
                            prop.Add("UserDomainName", userDomainName);
                        }
                    }

#if DEBUG
                    // [en] For the debug version, use synchronous transmission if you need.
                    NishySoftware.Telemetry.ApplicationInsights.Telemetry.EnableDeveloperMode(false);
#endif

                    return telemetry;
                });
            }
        }
        #endregion

        #endregion Properties

        #region Methods
        void TrackEventStartup(string[] startupArgs)
        {
            _startupDateTime = DateTime.UtcNow;
            Telemetry.TrackEvent("App_" + "General" + "Startup",
                "Command", startupArgs.Length > 0 ? startupArgs[0] : null);
        }

        void TrackEventExit(int exitCode)
        {
            if (!this._sentEventExit)
            {
                this._sentEventExit = true;
                var exitDateTime = DateTime.UtcNow;
                double duration = (exitDateTime - _startupDateTime).TotalSeconds;
                Telemetry.TrackEvent("App_" + "General" + "Exit",
                    "Duration", duration,
                    "ExitCode", exitCode.ToString());
                Telemetry.Flush();
                // [en] Wait a bit after calling Telemetry.Flush()
                Thread.Sleep(1000);
            }
        }
        #endregion Methods


        static void Main(string[] args)
        {
            var program = new Program();

            program.TrackEventStartup(args);

            Console.WriteLine("Hello World!");

            program.TrackEventExit(Environment.ExitCode);
        }
    }
}
```

## nsTelemetry API Reference

The API reference for nsTelemetryAI can be found in the [nsTelemetryAI.md](nsTelemetryAI.md) file.
This nsTelemetryAI.md is written in both English [en] and Japanese [ja].

namespace : NishySoftware.Telemetry

- [ITelemetry](nsTelemetryAI.md#T-NishySoftware-Telemetry-ITelemetry) inteface
- [TelemetryDataKinds](nsTelemetryAI.md#T-NishySoftware-Telemetry-TelemetryDataKinds) enum
- [TriggerType](nsTelemetryAI.md#T-NishySoftware-Telemetry-TriggerType) enum

namespace : NishySoftware.Telemetry.ApplicationInsights
- [Telemetry](nsTelemetryAI.md#T-NishySoftware-Telemetry-ApplicationInsights-Telemetry) class

## License

This library is under [the MIT License (MIT)](LICENSE).