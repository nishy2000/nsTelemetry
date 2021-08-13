<a name='assembly'></a>
# nsTelemetryAI

## Contents

- [ComponentContextReader](#T-NishySoftware-Telemetry-ApplicationInsights-ComponentContextReader 'NishySoftware.Telemetry.ApplicationInsights.ComponentContextReader')
  - [Instance](#P-NishySoftware-Telemetry-ApplicationInsights-ComponentContextReader-Instance 'NishySoftware.Telemetry.ApplicationInsights.ComponentContextReader.Instance')
  - [GetVersion()](#M-NishySoftware-Telemetry-ApplicationInsights-ComponentContextReader-GetVersion 'NishySoftware.Telemetry.ApplicationInsights.ComponentContextReader.GetVersion')
- [ComponentTelemetryInitializer](#T-NishySoftware-Telemetry-ApplicationInsights-ComponentTelemetryInitializer 'NishySoftware.Telemetry.ApplicationInsights.ComponentTelemetryInitializer')
  - [Initialize()](#M-NishySoftware-Telemetry-ApplicationInsights-ComponentTelemetryInitializer-Initialize-Microsoft-ApplicationInsights-Channel-ITelemetry- 'NishySoftware.Telemetry.ApplicationInsights.ComponentTelemetryInitializer.Initialize(Microsoft.ApplicationInsights.Channel.ITelemetry)')
- [DeveloperModeWithDebuggerAttachedTelemetryModule](#T-NishySoftware-Telemetry-ApplicationInsights-DeveloperModeWithDebuggerAttachedTelemetryModule 'NishySoftware.Telemetry.ApplicationInsights.DeveloperModeWithDebuggerAttachedTelemetryModule')
  - [IsDebuggerAttached](#F-NishySoftware-Telemetry-ApplicationInsights-DeveloperModeWithDebuggerAttachedTelemetryModule-IsDebuggerAttached 'NishySoftware.Telemetry.ApplicationInsights.DeveloperModeWithDebuggerAttachedTelemetryModule.IsDebuggerAttached')
  - [Initialize(configuration)](#M-NishySoftware-Telemetry-ApplicationInsights-DeveloperModeWithDebuggerAttachedTelemetryModule-Initialize-Microsoft-ApplicationInsights-Extensibility-TelemetryConfiguration- 'NishySoftware.Telemetry.ApplicationInsights.DeveloperModeWithDebuggerAttachedTelemetryModule.Initialize(Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration)')
- [DeviceContextReader](#T-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader 'NishySoftware.Telemetry.ApplicationInsights.DeviceContextReader')
  - [Instance](#P-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-Instance 'NishySoftware.Telemetry.ApplicationInsights.DeviceContextReader.Instance')
  - [GetDeviceModel()](#M-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-GetDeviceModel 'NishySoftware.Telemetry.ApplicationInsights.DeviceContextReader.GetDeviceModel')
  - [GetDeviceType()](#M-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-GetDeviceType 'NishySoftware.Telemetry.ApplicationInsights.DeviceContextReader.GetDeviceType')
  - [GetDeviceUniqueId()](#M-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-GetDeviceUniqueId 'NishySoftware.Telemetry.ApplicationInsights.DeviceContextReader.GetDeviceUniqueId')
  - [GetHostSystemLocale()](#M-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-GetHostSystemLocale 'NishySoftware.Telemetry.ApplicationInsights.DeviceContextReader.GetHostSystemLocale')
  - [GetNetworkType()](#M-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-GetNetworkType-System-Int64@,System-Boolean- 'NishySoftware.Telemetry.ApplicationInsights.DeviceContextReader.GetNetworkType(System.Int64@,System.Boolean)')
  - [GetOemName()](#M-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-GetOemName 'NishySoftware.Telemetry.ApplicationInsights.DeviceContextReader.GetOemName')
  - [GetOperatingSystem()](#M-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-GetOperatingSystem 'NishySoftware.Telemetry.ApplicationInsights.DeviceContextReader.GetOperatingSystem')
  - [GetScreenResolution()](#M-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-GetScreenResolution-System-Boolean- 'NishySoftware.Telemetry.ApplicationInsights.DeviceContextReader.GetScreenResolution(System.Boolean)')
  - [RunWmiQuery(table,property,defaultValue)](#M-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-RunWmiQuery-System-String,System-String,System-String- 'NishySoftware.Telemetry.ApplicationInsights.DeviceContextReader.RunWmiQuery(System.String,System.String,System.String)')
- [DeviceTelemetryInitializer](#T-NishySoftware-Telemetry-ApplicationInsights-DeviceTelemetryInitializer 'NishySoftware.Telemetry.ApplicationInsights.DeviceTelemetryInitializer')
  - [Initialize()](#M-NishySoftware-Telemetry-ApplicationInsights-DeviceTelemetryInitializer-Initialize-Microsoft-ApplicationInsights-Channel-ITelemetry- 'NishySoftware.Telemetry.ApplicationInsights.DeviceTelemetryInitializer.Initialize(Microsoft.ApplicationInsights.Channel.ITelemetry)')
- [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry')
  - [GlobalExceptionMetrics](#P-NishySoftware-Telemetry-ITelemetry-GlobalExceptionMetrics 'NishySoftware.Telemetry.ITelemetry.GlobalExceptionMetrics')
  - [GlobalExceptionProperties](#P-NishySoftware-Telemetry-ITelemetry-GlobalExceptionProperties 'NishySoftware.Telemetry.ITelemetry.GlobalExceptionProperties')
  - [GlobalMetrics](#P-NishySoftware-Telemetry-ITelemetry-GlobalMetrics 'NishySoftware.Telemetry.ITelemetry.GlobalMetrics')
  - [GlobalProperties](#P-NishySoftware-Telemetry-ITelemetry-GlobalProperties 'NishySoftware.Telemetry.ITelemetry.GlobalProperties')
  - [GlobalSyncObject](#P-NishySoftware-Telemetry-ITelemetry-GlobalSyncObject 'NishySoftware.Telemetry.ITelemetry.GlobalSyncObject')
  - [Metrics](#P-NishySoftware-Telemetry-ITelemetry-Metrics 'NishySoftware.Telemetry.ITelemetry.Metrics')
  - [Properties](#P-NishySoftware-Telemetry-ITelemetry-Properties 'NishySoftware.Telemetry.ITelemetry.Properties')
  - [Flush()](#M-NishySoftware-Telemetry-ITelemetry-Flush 'NishySoftware.Telemetry.ITelemetry.Flush')
  - [TrackEvent(eventName,prop1key,prop1value,prop2key,prop2value,prop3key,prop3value)](#M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,System-String,System-String,System-String,System-String,System-String,System-String- 'NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,System.String,System.String,System.String,System.String,System.String,System.String)')
  - [TrackEvent(eventName,metric1key,metric1value,prop1key,prop1value,prop2key,prop2value,prop3key,prop3value)](#M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,System-String,System-Double,System-String,System-String,System-String,System-String,System-String,System-String- 'NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,System.String,System.Double,System.String,System.String,System.String,System.String,System.String,System.String)')
  - [TrackEvent(eventName,triggerType,prop1key,prop1value,prop2key,prop2value,prop3key,prop3value)](#M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,NishySoftware-Telemetry-TriggerType,System-String,System-String,System-String,System-String,System-String,System-String- 'NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,NishySoftware.Telemetry.TriggerType,System.String,System.String,System.String,System.String,System.String,System.String)')
  - [TrackEvent(eventName,triggerType,metric1key,metric1value,prop1key,prop1value,prop2key,prop2value,prop3key,prop3value)](#M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,NishySoftware-Telemetry-TriggerType,System-String,System-Double,System-String,System-String,System-String,System-String,System-String,System-String- 'NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,NishySoftware.Telemetry.TriggerType,System.String,System.Double,System.String,System.String,System.String,System.String,System.String,System.String)')
  - [TrackEvent(eventName,properties,metrics)](#M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})')
  - [TrackEvent(eventName,triggerType,properties,metrics)](#M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,NishySoftware-Telemetry-TriggerType,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,NishySoftware.Telemetry.TriggerType,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})')
  - [TrackException(exception,level,prop1key,prop1value,prop2key,prop2value,prop3key,prop3value)](#M-NishySoftware-Telemetry-ITelemetry-TrackException-System-Exception,Microsoft-ApplicationInsights-DataContracts-SeverityLevel,System-String,System-String,System-String,System-String,System-String,System-String- 'NishySoftware.Telemetry.ITelemetry.TrackException(System.Exception,Microsoft.ApplicationInsights.DataContracts.SeverityLevel,System.String,System.String,System.String,System.String,System.String,System.String)')
  - [TrackException(exception,level,properties,metrics)](#M-NishySoftware-Telemetry-ITelemetry-TrackException-System-Exception,Microsoft-ApplicationInsights-DataContracts-SeverityLevel,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetry.TrackException(System.Exception,Microsoft.ApplicationInsights.DataContracts.SeverityLevel,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})')
  - [TrackPageView(pageName,properties,metrics)](#M-NishySoftware-Telemetry-ITelemetry-TrackPageView-System-String,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetry.TrackPageView(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})')
  - [TrackPageView(pageName,duration,properties,metrics)](#M-NishySoftware-Telemetry-ITelemetry-TrackPageView-System-String,System-TimeSpan,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetry.TrackPageView(System.String,System.TimeSpan,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})')
  - [TrackPageView(pageName,duration,metric1key,metric1value,prop1key,prop1value,prop2key,prop2value,prop3key,prop3value)](#M-NishySoftware-Telemetry-ITelemetry-TrackPageView-System-String,System-TimeSpan,System-String,System-Double,System-String,System-String,System-String,System-String,System-String,System-String- 'NishySoftware.Telemetry.ITelemetry.TrackPageView(System.String,System.TimeSpan,System.String,System.Double,System.String,System.String,System.String,System.String,System.String,System.String)')
- [ITelemetryContract](#T-NishySoftware-Telemetry-ITelemetryContract 'NishySoftware.Telemetry.ITelemetryContract')
  - [GlobalExceptionMetrics](#P-NishySoftware-Telemetry-ITelemetryContract-GlobalExceptionMetrics 'NishySoftware.Telemetry.ITelemetryContract.GlobalExceptionMetrics')
  - [GlobalExceptionProperties](#P-NishySoftware-Telemetry-ITelemetryContract-GlobalExceptionProperties 'NishySoftware.Telemetry.ITelemetryContract.GlobalExceptionProperties')
  - [GlobalMetrics](#P-NishySoftware-Telemetry-ITelemetryContract-GlobalMetrics 'NishySoftware.Telemetry.ITelemetryContract.GlobalMetrics')
  - [GlobalProperties](#P-NishySoftware-Telemetry-ITelemetryContract-GlobalProperties 'NishySoftware.Telemetry.ITelemetryContract.GlobalProperties')
  - [GlobalSyncObject](#P-NishySoftware-Telemetry-ITelemetryContract-GlobalSyncObject 'NishySoftware.Telemetry.ITelemetryContract.GlobalSyncObject')
  - [Metrics](#P-NishySoftware-Telemetry-ITelemetryContract-Metrics 'NishySoftware.Telemetry.ITelemetryContract.Metrics')
  - [Properties](#P-NishySoftware-Telemetry-ITelemetryContract-Properties 'NishySoftware.Telemetry.ITelemetryContract.Properties')
  - [Flush()](#M-NishySoftware-Telemetry-ITelemetryContract-Flush 'NishySoftware.Telemetry.ITelemetryContract.Flush')
  - [TrackEvent()](#M-NishySoftware-Telemetry-ITelemetryContract-TrackEvent-System-String,System-String,System-String,System-String,System-String,System-String,System-String- 'NishySoftware.Telemetry.ITelemetryContract.TrackEvent(System.String,System.String,System.String,System.String,System.String,System.String,System.String)')
  - [TrackEvent()](#M-NishySoftware-Telemetry-ITelemetryContract-TrackEvent-System-String,System-String,System-Double,System-String,System-String,System-String,System-String,System-String,System-String- 'NishySoftware.Telemetry.ITelemetryContract.TrackEvent(System.String,System.String,System.Double,System.String,System.String,System.String,System.String,System.String,System.String)')
  - [TrackEvent()](#M-NishySoftware-Telemetry-ITelemetryContract-TrackEvent-System-String,NishySoftware-Telemetry-TriggerType,System-String,System-String,System-String,System-String,System-String,System-String- 'NishySoftware.Telemetry.ITelemetryContract.TrackEvent(System.String,NishySoftware.Telemetry.TriggerType,System.String,System.String,System.String,System.String,System.String,System.String)')
  - [TrackEvent()](#M-NishySoftware-Telemetry-ITelemetryContract-TrackEvent-System-String,NishySoftware-Telemetry-TriggerType,System-String,System-Double,System-String,System-String,System-String,System-String,System-String,System-String- 'NishySoftware.Telemetry.ITelemetryContract.TrackEvent(System.String,NishySoftware.Telemetry.TriggerType,System.String,System.Double,System.String,System.String,System.String,System.String,System.String,System.String)')
  - [TrackEvent()](#M-NishySoftware-Telemetry-ITelemetryContract-TrackEvent-System-String,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetryContract.TrackEvent(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})')
  - [TrackEvent()](#M-NishySoftware-Telemetry-ITelemetryContract-TrackEvent-System-String,NishySoftware-Telemetry-TriggerType,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetryContract.TrackEvent(System.String,NishySoftware.Telemetry.TriggerType,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})')
  - [TrackException()](#M-NishySoftware-Telemetry-ITelemetryContract-TrackException-System-Exception,Microsoft-ApplicationInsights-DataContracts-SeverityLevel,System-String,System-String,System-String,System-String,System-String,System-String- 'NishySoftware.Telemetry.ITelemetryContract.TrackException(System.Exception,Microsoft.ApplicationInsights.DataContracts.SeverityLevel,System.String,System.String,System.String,System.String,System.String,System.String)')
  - [TrackException()](#M-NishySoftware-Telemetry-ITelemetryContract-TrackException-System-Exception,Microsoft-ApplicationInsights-DataContracts-SeverityLevel,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetryContract.TrackException(System.Exception,Microsoft.ApplicationInsights.DataContracts.SeverityLevel,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})')
  - [TrackPageView()](#M-NishySoftware-Telemetry-ITelemetryContract-TrackPageView-System-String,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetryContract.TrackPageView(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})')
  - [TrackPageView()](#M-NishySoftware-Telemetry-ITelemetryContract-TrackPageView-System-String,System-TimeSpan,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetryContract.TrackPageView(System.String,System.TimeSpan,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})')
  - [TrackPageView()](#M-NishySoftware-Telemetry-ITelemetryContract-TrackPageView-System-String,System-TimeSpan,System-String,System-Double,System-String,System-String,System-String,System-String,System-String,System-String- 'NishySoftware.Telemetry.ITelemetryContract.TrackPageView(System.String,System.TimeSpan,System.String,System.Double,System.String,System.String,System.String,System.String,System.String,System.String)')
- [SessionContextReader](#T-NishySoftware-Telemetry-ApplicationInsights-SessionContextReader 'NishySoftware.Telemetry.ApplicationInsights.SessionContextReader')
  - [Instance](#P-NishySoftware-Telemetry-ApplicationInsights-SessionContextReader-Instance 'NishySoftware.Telemetry.ApplicationInsights.SessionContextReader.Instance')
  - [GetSessionId()](#M-NishySoftware-Telemetry-ApplicationInsights-SessionContextReader-GetSessionId 'NishySoftware.Telemetry.ApplicationInsights.SessionContextReader.GetSessionId')
  - [IsFirstSession()](#M-NishySoftware-Telemetry-ApplicationInsights-SessionContextReader-IsFirstSession-System-String- 'NishySoftware.Telemetry.ApplicationInsights.SessionContextReader.IsFirstSession(System.String)')
- [SessionTelemetryInitializer](#T-NishySoftware-Telemetry-ApplicationInsights-SessionTelemetryInitializer 'NishySoftware.Telemetry.ApplicationInsights.SessionTelemetryInitializer')
  - [Initialize()](#M-NishySoftware-Telemetry-ApplicationInsights-SessionTelemetryInitializer-Initialize-Microsoft-ApplicationInsights-Channel-ITelemetry- 'NishySoftware.Telemetry.ApplicationInsights.SessionTelemetryInitializer.Initialize(Microsoft.ApplicationInsights.Channel.ITelemetry)')
- [SystemMetric](#T-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-SystemMetric 'NishySoftware.Telemetry.ApplicationInsights.DeviceContextReader.SystemMetric')
  - [SM_CXSCREEN](#F-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-SystemMetric-SM_CXSCREEN 'NishySoftware.Telemetry.ApplicationInsights.DeviceContextReader.SystemMetric.SM_CXSCREEN')
  - [SM_CYSCREEN](#F-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-SystemMetric-SM_CYSCREEN 'NishySoftware.Telemetry.ApplicationInsights.DeviceContextReader.SystemMetric.SM_CYSCREEN')
- [Telemetry](#T-NishySoftware-Telemetry-ApplicationInsights-Telemetry 'NishySoftware.Telemetry.ApplicationInsights.Telemetry')
  - [CommonDataKinds](#P-NishySoftware-Telemetry-ApplicationInsights-Telemetry-CommonDataKinds 'NishySoftware.Telemetry.ApplicationInsights.Telemetry.CommonDataKinds')
  - [CreateTelemetry()](#M-NishySoftware-Telemetry-ApplicationInsights-Telemetry-CreateTelemetry 'NishySoftware.Telemetry.ApplicationInsights.Telemetry.CreateTelemetry')
  - [Enable()](#M-NishySoftware-Telemetry-ApplicationInsights-Telemetry-Enable-System-Boolean- 'NishySoftware.Telemetry.ApplicationInsights.Telemetry.Enable(System.Boolean)')
  - [EnableDeveloperMode()](#M-NishySoftware-Telemetry-ApplicationInsights-Telemetry-EnableDeveloperMode-System-Boolean- 'NishySoftware.Telemetry.ApplicationInsights.Telemetry.EnableDeveloperMode(System.Boolean)')
  - [IsEnabled()](#M-NishySoftware-Telemetry-ApplicationInsights-Telemetry-IsEnabled 'NishySoftware.Telemetry.ApplicationInsights.Telemetry.IsEnabled')
  - [SetInstrumentationKey(instrumentationKey)](#M-NishySoftware-Telemetry-ApplicationInsights-Telemetry-SetInstrumentationKey-System-String- 'NishySoftware.Telemetry.ApplicationInsights.Telemetry.SetInstrumentationKey(System.String)')
- [TelemetryDataKinds](#T-NishySoftware-Telemetry-TelemetryDataKinds 'NishySoftware.Telemetry.TelemetryDataKinds')
  - [All](#F-NishySoftware-Telemetry-TelemetryDataKinds-All 'NishySoftware.Telemetry.TelemetryDataKinds.All')
  - [Default](#F-NishySoftware-Telemetry-TelemetryDataKinds-Default 'NishySoftware.Telemetry.TelemetryDataKinds.Default')
  - [DeviceManufacturer](#F-NishySoftware-Telemetry-TelemetryDataKinds-DeviceManufacturer 'NishySoftware.Telemetry.TelemetryDataKinds.DeviceManufacturer')
  - [DeviceName](#F-NishySoftware-Telemetry-TelemetryDataKinds-DeviceName 'NishySoftware.Telemetry.TelemetryDataKinds.DeviceName')
  - [ExeName](#F-NishySoftware-Telemetry-TelemetryDataKinds-ExeName 'NishySoftware.Telemetry.TelemetryDataKinds.ExeName')
  - [HostName](#F-NishySoftware-Telemetry-TelemetryDataKinds-HostName 'NishySoftware.Telemetry.TelemetryDataKinds.HostName')
  - [Language](#F-NishySoftware-Telemetry-TelemetryDataKinds-Language 'NishySoftware.Telemetry.TelemetryDataKinds.Language')
  - [NetworkSpeed](#F-NishySoftware-Telemetry-TelemetryDataKinds-NetworkSpeed 'NishySoftware.Telemetry.TelemetryDataKinds.NetworkSpeed')
  - [NetworkType](#F-NishySoftware-Telemetry-TelemetryDataKinds-NetworkType 'NishySoftware.Telemetry.TelemetryDataKinds.NetworkType')
  - [ScreenResolution](#F-NishySoftware-Telemetry-TelemetryDataKinds-ScreenResolution 'NishySoftware.Telemetry.TelemetryDataKinds.ScreenResolution')
  - [UserName](#F-NishySoftware-Telemetry-TelemetryDataKinds-UserName 'NishySoftware.Telemetry.TelemetryDataKinds.UserName')
- [TriggerType](#T-NishySoftware-Telemetry-TriggerType 'NishySoftware.Telemetry.TriggerType')
  - [Click](#F-NishySoftware-Telemetry-TriggerType-Click 'NishySoftware.Telemetry.TriggerType.Click')
  - [ContextMenu](#F-NishySoftware-Telemetry-TriggerType-ContextMenu 'NishySoftware.Telemetry.TriggerType.ContextMenu')
  - [Flick](#F-NishySoftware-Telemetry-TriggerType-Flick 'NishySoftware.Telemetry.TriggerType.Flick')
  - [Key](#F-NishySoftware-Telemetry-TriggerType-Key 'NishySoftware.Telemetry.TriggerType.Key')
  - [None](#F-NishySoftware-Telemetry-TriggerType-None 'NishySoftware.Telemetry.TriggerType.None')
  - [Other](#F-NishySoftware-Telemetry-TriggerType-Other 'NishySoftware.Telemetry.TriggerType.Other')
  - [Tap](#F-NishySoftware-Telemetry-TriggerType-Tap 'NishySoftware.Telemetry.TriggerType.Tap')
- [UnhandledExceptionTelemetryModule](#T-NishySoftware-Telemetry-ApplicationInsights-UnhandledExceptionTelemetryModule 'NishySoftware.Telemetry.ApplicationInsights.UnhandledExceptionTelemetryModule')
  - [#ctor()](#M-NishySoftware-Telemetry-ApplicationInsights-UnhandledExceptionTelemetryModule-#ctor 'NishySoftware.Telemetry.ApplicationInsights.UnhandledExceptionTelemetryModule.#ctor')
  - [Dispose()](#M-NishySoftware-Telemetry-ApplicationInsights-UnhandledExceptionTelemetryModule-Dispose 'NishySoftware.Telemetry.ApplicationInsights.UnhandledExceptionTelemetryModule.Dispose')
  - [Initialize()](#M-NishySoftware-Telemetry-ApplicationInsights-UnhandledExceptionTelemetryModule-Initialize-Microsoft-ApplicationInsights-Extensibility-TelemetryConfiguration- 'NishySoftware.Telemetry.ApplicationInsights.UnhandledExceptionTelemetryModule.Initialize(Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration)')
- [UnobservedExceptionTelemetryModule](#T-NishySoftware-Telemetry-ApplicationInsights-UnobservedExceptionTelemetryModule 'NishySoftware.Telemetry.ApplicationInsights.UnobservedExceptionTelemetryModule')
  - [#ctor()](#M-NishySoftware-Telemetry-ApplicationInsights-UnobservedExceptionTelemetryModule-#ctor 'NishySoftware.Telemetry.ApplicationInsights.UnobservedExceptionTelemetryModule.#ctor')
  - [Dispose()](#M-NishySoftware-Telemetry-ApplicationInsights-UnobservedExceptionTelemetryModule-Dispose 'NishySoftware.Telemetry.ApplicationInsights.UnobservedExceptionTelemetryModule.Dispose')
  - [Initialize(configuration)](#M-NishySoftware-Telemetry-ApplicationInsights-UnobservedExceptionTelemetryModule-Initialize-Microsoft-ApplicationInsights-Extensibility-TelemetryConfiguration- 'NishySoftware.Telemetry.ApplicationInsights.UnobservedExceptionTelemetryModule.Initialize(Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration)')
- [UserContextReader](#T-NishySoftware-Telemetry-ApplicationInsights-UserContextReader 'NishySoftware.Telemetry.ApplicationInsights.UserContextReader')
  - [Instance](#P-NishySoftware-Telemetry-ApplicationInsights-UserContextReader-Instance 'NishySoftware.Telemetry.ApplicationInsights.UserContextReader.Instance')
  - [GetUserUniqueId()](#M-NishySoftware-Telemetry-ApplicationInsights-UserContextReader-GetUserUniqueId 'NishySoftware.Telemetry.ApplicationInsights.UserContextReader.GetUserUniqueId')
- [UserTelemetryInitializer](#T-NishySoftware-Telemetry-ApplicationInsights-UserTelemetryInitializer 'NishySoftware.Telemetry.ApplicationInsights.UserTelemetryInitializer')
  - [Initialize()](#M-NishySoftware-Telemetry-ApplicationInsights-UserTelemetryInitializer-Initialize-Microsoft-ApplicationInsights-Channel-ITelemetry- 'NishySoftware.Telemetry.ApplicationInsights.UserTelemetryInitializer.Initialize(Microsoft.ApplicationInsights.Channel.ITelemetry)')

<a name='T-NishySoftware-Telemetry-ApplicationInsights-ComponentContextReader'></a>
## ComponentContextReader `type`

##### Namespace

NishySoftware.Telemetry.ApplicationInsights

<a name='P-NishySoftware-Telemetry-ApplicationInsights-ComponentContextReader-Instance'></a>
### Instance `property`

##### Summary

Gets or sets the singleton instance for our application context reader.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-ComponentContextReader-GetVersion'></a>
### GetVersion() `method`

##### Summary

Gets the component version.

##### Returns

The component version.

##### Parameters

This method has no parameters.

<a name='T-NishySoftware-Telemetry-ApplicationInsights-ComponentTelemetryInitializer'></a>
## ComponentTelemetryInitializer `type`

##### Namespace

NishySoftware.Telemetry.ApplicationInsights

<a name='M-NishySoftware-Telemetry-ApplicationInsights-ComponentTelemetryInitializer-Initialize-Microsoft-ApplicationInsights-Channel-ITelemetry-'></a>
### Initialize() `method`

##### Summary

Populates component properties on a telemetry item.

##### Parameters

This method has no parameters.

<a name='T-NishySoftware-Telemetry-ApplicationInsights-DeveloperModeWithDebuggerAttachedTelemetryModule'></a>
## DeveloperModeWithDebuggerAttachedTelemetryModule `type`

##### Namespace

NishySoftware.Telemetry.ApplicationInsights

##### Summary

Telemetry module that sets developer mode to true when is not already set AND managed debugger is attached.

<a name='F-NishySoftware-Telemetry-ApplicationInsights-DeveloperModeWithDebuggerAttachedTelemetryModule-IsDebuggerAttached'></a>
### IsDebuggerAttached `constants`

##### Summary

Function that checks whether debugger is attached with implementation that can be replaced by unit test code.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-DeveloperModeWithDebuggerAttachedTelemetryModule-Initialize-Microsoft-ApplicationInsights-Extensibility-TelemetryConfiguration-'></a>
### Initialize(configuration) `method`

##### Summary

Gives the opportunity for this telemetry module to initialize configuration object that is passed to it.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| configuration | [Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration](#T-Microsoft-ApplicationInsights-Extensibility-TelemetryConfiguration 'Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration') | Configuration object. |

<a name='T-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader'></a>
## DeviceContextReader `type`

##### Namespace

NishySoftware.Telemetry.ApplicationInsights

<a name='P-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-Instance'></a>
### Instance `property`

##### Summary

Gets or sets the singleton instance for our application context reader.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-GetDeviceModel'></a>
### GetDeviceModel() `method`

##### Summary

Gets the device model.

##### Returns

The discovered device model.

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-GetDeviceType'></a>
### GetDeviceType() `method`

##### Summary

Gets the type of the device.

##### Returns

The type for this device as a hard-coded string.

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-GetDeviceUniqueId'></a>
### GetDeviceUniqueId() `method`

##### Summary

Gets the device unique ID, or uses the fallback if none is available due to application configuration.

##### Returns

The discovered device identifier.

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-GetHostSystemLocale'></a>
### GetHostSystemLocale() `method`

##### Summary

Gets the host system locale.

##### Returns

The discovered locale.

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-GetNetworkType-System-Int64@,System-Boolean-'></a>
### GetNetworkType() `method`

##### Summary

Gets the network type.

##### Returns

The discovered network type.

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-GetOemName'></a>
### GetOemName() `method`

##### Summary

Gets the device OEM.

##### Returns

The discovered OEM.

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-GetOperatingSystem'></a>
### GetOperatingSystem() `method`

##### Summary

Gets the Operating System.

##### Returns

The discovered operating system.

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-GetScreenResolution-System-Boolean-'></a>
### GetScreenResolution() `method`

##### Summary

Gets the ScreenResolution.

##### Returns

The discovered ScreenResolution.

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-RunWmiQuery-System-String,System-String,System-String-'></a>
### RunWmiQuery(table,property,defaultValue) `method`

##### Summary

Runs a single WMI query for a property.

##### Returns

The value if found, Unknown otherwise.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| table | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The table. |
| property | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The property. |
| defaultValue | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The default value of the property if WMI fails. |

<a name='T-NishySoftware-Telemetry-ApplicationInsights-DeviceTelemetryInitializer'></a>
## DeviceTelemetryInitializer `type`

##### Namespace

NishySoftware.Telemetry.ApplicationInsights

<a name='M-NishySoftware-Telemetry-ApplicationInsights-DeviceTelemetryInitializer-Initialize-Microsoft-ApplicationInsights-Channel-ITelemetry-'></a>
### Initialize() `method`

##### Summary

Populates device properties on a telemetry item.

##### Parameters

This method has no parameters.

<a name='T-NishySoftware-Telemetry-ITelemetry'></a>
## ITelemetry `type`

##### Namespace

NishySoftware.Telemetry

##### Summary

[en] Interface for Telemetry

[ja] Telemetry用のインターフェース

<a name='P-NishySoftware-Telemetry-ITelemetry-GlobalExceptionMetrics'></a>
### GlobalExceptionMetrics `property`

##### Summary

[en] The property that holds the metrics to be sent by default when using ITelemetry.TrackException().
Since this property is shared globally, you need to lock the [GlobalSyncObject](#P-NishySoftware-Telemetry-ITelemetry-GlobalSyncObject 'NishySoftware.Telemetry.ITelemetry.GlobalSyncObject') when manipulating or referencing this property.

[ja] ITelemetry.TrackException() を使用するときに既定で送信するメトリックを保持するプロパティ。
このプロパティはグローバルで共有されるため、このプロパティを操作・参照するときは、[GlobalSyncObject](#P-NishySoftware-Telemetry-ITelemetry-GlobalSyncObject 'NishySoftware.Telemetry.ITelemetry.GlobalSyncObject') でLockする必要があります。

##### Remarks

[en] The metrics held by this property will be sent by all ITelemetry.TrackException(), regardless of the Interface instance.
If a metric with the same name is specified in an instance of Interface or in an argument of ITelemetry.TrackException(),
the value with the same name will take precedence over the value in the order of argument and Interface

[ja] このプロパティが保持するメトリックはInterfaceのインスタンスによらず、すべての ITelemetry.TrackException() で送信されます。
Interfaceのインスタンスや ITelemetry.TrackException() の引数で同じ名前のメトリックが指定された場合は、同じ名前の値は、引数、Interfaceの順で優先されます。

<a name='P-NishySoftware-Telemetry-ITelemetry-GlobalExceptionProperties'></a>
### GlobalExceptionProperties `property`

##### Summary

[en] The property that holds the properties to be sent by default when using ITelemetry.TrackException().
Since this property is shared globally, you need to lock the [GlobalSyncObject](#P-NishySoftware-Telemetry-ITelemetry-GlobalSyncObject 'NishySoftware.Telemetry.ITelemetry.GlobalSyncObject') when manipulating or referencing this property.

[ja] ITelemetry.TrackException() を使用するときに既定で送信するプロパティを保持するプロパティ。
このプロパティはグローバルで共有されるため、このプロパティを操作・参照するときは、[GlobalSyncObject](#P-NishySoftware-Telemetry-ITelemetry-GlobalSyncObject 'NishySoftware.Telemetry.ITelemetry.GlobalSyncObject') でLockする必要があります。

##### Remarks

[en] The properties held by this property will be sent by all ITelemetry.TrackException(), regardless of the Interface instance.
If a property with the same name is specified in an instance of Interface or in an argument of ITelemetry.TrackException(),
the value with the same name will take precedence over the value in the order of argument and Interface

[ja] このプロパティが保持するプロパティはInterfaceのインスタンスによらず、すべての ITelemetry.TrackException() で送信されます。
Interfaceのインスタンスや ITelemetry.TrackException() の引数で同じ名前のプロパティが指定された場合は、同じ名前の値は、引数、Interfaceの順で優先されます。

<a name='P-NishySoftware-Telemetry-ITelemetry-GlobalMetrics'></a>
### GlobalMetrics `property`

##### Summary

[en] The property that holds the metrics to be sent by default.
Since this property is shared globally, you need to lock the [GlobalSyncObject](#P-NishySoftware-Telemetry-ITelemetry-GlobalSyncObject 'NishySoftware.Telemetry.ITelemetry.GlobalSyncObject') when manipulating or referencing this property.

[ja] 既定で送信するメトリックを保持するプロパティ。
このプロパティはグローバルで共有されるため、このプロパティを操作・参照するときは、[GlobalSyncObject](#P-NishySoftware-Telemetry-ITelemetry-GlobalSyncObject 'NishySoftware.Telemetry.ITelemetry.GlobalSyncObject') でLockする必要があります。

##### Remarks

[en] The metrics held by this property will be sent by all [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX(), regardless of the Interface instance.
If a metric with the same name is specified in an instance of Interface or in an argument of [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX(),
the value with the same name will take precedence over the value in the order of argument and Interface

[ja] このプロパティが保持するメトリックはInterfaceのインスタンスによらず、すべての [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX() で送信されます。
Interfaceのインスタンスや [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX() の引数で同じ名前のメトリックが指定された場合は、同じ名前の値は、引数、Interfaceの順で優先されます。

<a name='P-NishySoftware-Telemetry-ITelemetry-GlobalProperties'></a>
### GlobalProperties `property`

##### Summary

[en] The property that holds the properties to be sent by default.
Since this property is shared globally, you need to lock the [GlobalSyncObject](#P-NishySoftware-Telemetry-ITelemetry-GlobalSyncObject 'NishySoftware.Telemetry.ITelemetry.GlobalSyncObject') when manipulating or referencing this property.

[ja] 既定で送信するプロパティを保持するプロパティ。
このプロパティはグローバルで共有されるため、このプロパティを操作・参照するときは、[GlobalSyncObject](#P-NishySoftware-Telemetry-ITelemetry-GlobalSyncObject 'NishySoftware.Telemetry.ITelemetry.GlobalSyncObject') でLockする必要があります。

##### Remarks

[en] The properties held by this property will be sent by all [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX(), regardless of the Interface instance.
If a property with the same name is specified in an instance of Interface or in an argument of [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX(),
the value with the same name will take precedence over the value in the order of argument and Interface

[ja] このプロパティが保持するプロパティはInterfaceのインスタンスによらず、すべての [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX() で送信されます。
Interfaceのインスタンスや [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX() の引数で同じ名前のプロパティが指定された場合は、同じ名前の値は、引数、Interfaceの順で優先されます。

<a name='P-NishySoftware-Telemetry-ITelemetry-GlobalSyncObject'></a>
### GlobalSyncObject `property`

##### Summary

[en] LockObject to use when manipulating and referencing [GlobalProperties](#P-NishySoftware-Telemetry-ITelemetry-GlobalProperties 'NishySoftware.Telemetry.ITelemetry.GlobalProperties') / [GlobalMetrics](#P-NishySoftware-Telemetry-ITelemetry-GlobalMetrics 'NishySoftware.Telemetry.ITelemetry.GlobalMetrics') / [GlobalExceptionProperties](#P-NishySoftware-Telemetry-ITelemetry-GlobalExceptionProperties 'NishySoftware.Telemetry.ITelemetry.GlobalExceptionProperties') / [GlobalExceptionMetrics](#P-NishySoftware-Telemetry-ITelemetry-GlobalExceptionMetrics 'NishySoftware.Telemetry.ITelemetry.GlobalExceptionMetrics').
[GlobalProperties](#P-NishySoftware-Telemetry-ITelemetry-GlobalProperties 'NishySoftware.Telemetry.ITelemetry.GlobalProperties') / [GlobalMetrics](#P-NishySoftware-Telemetry-ITelemetry-GlobalMetrics 'NishySoftware.Telemetry.ITelemetry.GlobalMetrics') / [GlobalExceptionProperties](#P-NishySoftware-Telemetry-ITelemetry-GlobalExceptionProperties 'NishySoftware.Telemetry.ITelemetry.GlobalExceptionProperties') / [GlobalExceptionMetrics](#P-NishySoftware-Telemetry-ITelemetry-GlobalExceptionMetrics 'NishySoftware.Telemetry.ITelemetry.GlobalExceptionMetrics') are shared globally, so we don't know which thread is using them.
Therefore, we can use this object to Lock and then manipulate and reference it.

[ja] [GlobalProperties](#P-NishySoftware-Telemetry-ITelemetry-GlobalProperties 'NishySoftware.Telemetry.ITelemetry.GlobalProperties') / [GlobalMetrics](#P-NishySoftware-Telemetry-ITelemetry-GlobalMetrics 'NishySoftware.Telemetry.ITelemetry.GlobalMetrics') / [GlobalExceptionProperties](#P-NishySoftware-Telemetry-ITelemetry-GlobalExceptionProperties 'NishySoftware.Telemetry.ITelemetry.GlobalExceptionProperties') / [GlobalExceptionMetrics](#P-NishySoftware-Telemetry-ITelemetry-GlobalExceptionMetrics 'NishySoftware.Telemetry.ITelemetry.GlobalExceptionMetrics') を操作・参照するときに使用するLockObject。
[GlobalProperties](#P-NishySoftware-Telemetry-ITelemetry-GlobalProperties 'NishySoftware.Telemetry.ITelemetry.GlobalProperties') / [GlobalMetrics](#P-NishySoftware-Telemetry-ITelemetry-GlobalMetrics 'NishySoftware.Telemetry.ITelemetry.GlobalMetrics') / [GlobalExceptionProperties](#P-NishySoftware-Telemetry-ITelemetry-GlobalExceptionProperties 'NishySoftware.Telemetry.ITelemetry.GlobalExceptionProperties') / [GlobalExceptionMetrics](#P-NishySoftware-Telemetry-ITelemetry-GlobalExceptionMetrics 'NishySoftware.Telemetry.ITelemetry.GlobalExceptionMetrics') は、グローバルで共有されるため、どのスレッドで使用されているかわかりません。
そのため、このオブジェクトを使ってLockしてから、操作・参照します。

<a name='P-NishySoftware-Telemetry-ITelemetry-Metrics'></a>
### Metrics `property`

##### Summary

[en] The property that holds the metrics that an instance of this interface sends by default.

[ja] このインターフェースのインスタンスが既定で送信するメトリックを保持するプロパティ。

##### Remarks

[en] The metrics held by this property will be sent in all [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX() issued by instances of this Interface.
If a metric with the same name is specified in the argument of [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX(),
the value of the argument will take precedence over the value of the same name

[ja] このプロパティが保持するメトリックはこのInterfaceのインスタンスから発行するすべての [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX() で送信されます。
[ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX() の引数で同じ名前のメトリックが指定された場合は、同じ名前の値は、引数の値が優先されます。

<a name='P-NishySoftware-Telemetry-ITelemetry-Properties'></a>
### Properties `property`

##### Summary

[en] The property that holds the properties that an instance of this interface sends by default.

[ja] このインターフェースのインスタンスが既定で送信するプロパティを保持するプロパティ。

##### Remarks

[en] The properties held by this property will be sent in all [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX() issued by instances of this Interface.
If a property with the same name is specified in the argument of [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX(),
the value of the argument will take precedence over the value of the same name

[ja] このプロパティが保持するプロパティはこのInterfaceのインスタンスから発行するすべての [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX() で送信されます。
[ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX() の引数で同じ名前のプロパティが指定された場合は、同じ名前の値は、引数の値が優先されます。

<a name='M-NishySoftware-Telemetry-ITelemetry-Flush'></a>
### Flush() `method`

##### Summary

[en] Flushes the unsent telemetry data in the in-memory buffer.
This method is an asynchronous function, so if you want to ensure data protection (sending or saving files) when the application is closed,
wait a little while after the call before closing the application.

[ja] メモリ内バッファにある未送信のTelemetryデータを送信をフラッシュする
このメソッドは非同期関数なので、アプリの終了時に確実にデータ保護(送信またはファイル保存)をしたい場合は、呼び出し後、少し待ってから、アプリを終了すること。

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,System-String,System-String,System-String,System-String,System-String,System-String-'></a>
### TrackEvent(eventName,prop1key,prop1value,prop2key,prop2value,prop3key,prop3value) `method`

##### Summary

[en] Send information about the custom event (such as usage of features and commands) in the application.
For ApplicationInsight, send an [EventTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-EventTelemetry 'Microsoft.ApplicationInsights.DataContracts.EventTelemetry') for display in Diagnostic Search and in the Analytics Portal.

[ja] アプリのカスタムイベント(機能やコマンドの利用状況など)についての情報を送信する。
ApplicationInsightにおいては、解析ポータルや診断検索で表示する [EventTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-EventTelemetry 'Microsoft.ApplicationInsights.DataContracts.EventTelemetry') を送信します。

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| eventName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] A name for the event.[ja]イベントの名前。 |
| prop1key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of string property value #1.[ja] 文字列プロパティ値1のキー名。 |
| prop1value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] String property value #1.[ja] 文字列プロパティ値1。 |
| prop2key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of string property value #2.[ja] 文字列プロパティ値2のキー名。 |
| prop2value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] String property value #2.[ja] 文字列プロパティ値2。 |
| prop3key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of string property value #3.[ja] 文字列プロパティ値3のキー名。 |
| prop3value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] String property value #3.[ja] 文字列プロパティ値3。 |

##### Remarks

[en]To send more than four string properties, use [TrackEvent](#M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})') instead of this method.

[ja] 4個以上の文字列プロパティを送信する場合は、このメソッドの代わりに [TrackEvent](#M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})') を使用します。

<a name='M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,System-String,System-Double,System-String,System-String,System-String,System-String,System-String,System-String-'></a>
### TrackEvent(eventName,metric1key,metric1value,prop1key,prop1value,prop2key,prop2value,prop3key,prop3value) `method`

##### Summary

[en] Send information about the custom event (such as usage of features and commands) in the application.
For ApplicationInsight, send an [EventTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-EventTelemetry 'Microsoft.ApplicationInsights.DataContracts.EventTelemetry') for display in Diagnostic Search and in the Analytics Portal.

[ja] アプリのカスタムイベント(機能やコマンドの利用状況など)についての情報を送信する。
ApplicationInsightにおいては、解析ポータルや診断検索で表示する [EventTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-EventTelemetry 'Microsoft.ApplicationInsights.DataContracts.EventTelemetry') を送信します。

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| eventName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] A name for the event. [ja]イベントの名前。 |
| metric1key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of measurement value #1. [ja] 計測値1のキー名。 |
| metric1value | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | [en] Measurement value #1. [ja] 計測値1。 |
| prop1key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of string property value #1. [ja] 文字列プロパティ値1のキー名。 |
| prop1value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] String property value #1. [ja] 文字列プロパティ値1。 |
| prop2key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of string property value #2. [ja] 文字列プロパティ値2のキー名。 |
| prop2value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] String property value #2. [ja] 文字列プロパティ値2。 |
| prop3key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of string property value #3. [ja] 文字列プロパティ値3のキー名。 |
| prop3value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] String property value #3. [ja] 文字列プロパティ値3。 |

##### Remarks

[en]To more than two metrics or send more than four string properties, use [TrackEvent](#M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})') instead of this method.

[ja] 2個以上のメトリック、または、4個以上の文字列プロパティを送信する場合は、このメソッドの代わりに [TrackEvent](#M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})') を使用します。

<a name='M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,NishySoftware-Telemetry-TriggerType,System-String,System-String,System-String,System-String,System-String,System-String-'></a>
### TrackEvent(eventName,triggerType,prop1key,prop1value,prop2key,prop2value,prop3key,prop3value) `method`

##### Summary

[en] Send information about the custom event (such as usage of features and commands) in the application.
For ApplicationInsight, send an [EventTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-EventTelemetry 'Microsoft.ApplicationInsights.DataContracts.EventTelemetry') for display in Diagnostic Search and in the Analytics Portal.

[ja] アプリのカスタムイベント(機能やコマンドの利用状況など)についての情報を送信する。
ApplicationInsightにおいては、解析ポータルや診断検索で表示する [EventTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-EventTelemetry 'Microsoft.ApplicationInsights.DataContracts.EventTelemetry') を送信します。

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| eventName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] A name for the event. [ja]イベントの名前。 |
| triggerType | [NishySoftware.Telemetry.TriggerType](#T-NishySoftware-Telemetry-TriggerType 'NishySoftware.Telemetry.TriggerType') | [en] Trigger type for the event. [ja]イベントのトリガーの種類。 |
| prop1key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of string property value #1. [ja] 文字列プロパティ値1のキー名。 |
| prop1value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] String property value #1. [ja] 文字列プロパティ値1。 |
| prop2key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of string property value #2. [ja] 文字列プロパティ値2のキー名。 |
| prop2value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] String property value #2. [ja] 文字列プロパティ値2。 |
| prop3key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of string property value #3. [ja] 文字列プロパティ値3のキー名。 |
| prop3value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] String property value #3. [ja] 文字列プロパティ値3。 |

##### Remarks

[en]To send more than four string properties, use [TrackEvent](#M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})') instead of this method.

[ja] 4個以上の文字列プロパティを送信する場合は、このメソッドの代わりに [TrackEvent](#M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})') を使用します。

<a name='M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,NishySoftware-Telemetry-TriggerType,System-String,System-Double,System-String,System-String,System-String,System-String,System-String,System-String-'></a>
### TrackEvent(eventName,triggerType,metric1key,metric1value,prop1key,prop1value,prop2key,prop2value,prop3key,prop3value) `method`

##### Summary

[en] Send information about the custom event (such as usage of features and commands) in the application.
For ApplicationInsight, send an [EventTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-EventTelemetry 'Microsoft.ApplicationInsights.DataContracts.EventTelemetry') for display in Diagnostic Search and in the Analytics Portal.

[ja] アプリのカスタムイベント(機能やコマンドの利用状況など)についての情報を送信する。
ApplicationInsightにおいては、解析ポータルや診断検索で表示する [EventTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-EventTelemetry 'Microsoft.ApplicationInsights.DataContracts.EventTelemetry') を送信します。

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| eventName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] A name for the event. [ja]イベントの名前。 |
| triggerType | [NishySoftware.Telemetry.TriggerType](#T-NishySoftware-Telemetry-TriggerType 'NishySoftware.Telemetry.TriggerType') | [en] Trigger type for the event. [ja]イベントのトリガーの種類。 |
| metric1key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of measurement value #1. [ja] 計測値1のキー名。 |
| metric1value | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | [en] Measurement value #1. [ja] 計測値1。 |
| prop1key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of string property value #1. [ja] 文字列プロパティ値1のキー名。 |
| prop1value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] String property value #1. [ja] 文字列プロパティ値1。 |
| prop2key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of string property value #2. [ja] 文字列プロパティ値2のキー名。 |
| prop2value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] String property value #2. [ja] 文字列プロパティ値2。 |
| prop3key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of string property value #3. [ja] 文字列プロパティ値3のキー名。 |
| prop3value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] String property value #3. [ja] 文字列プロパティ値3。 |

##### Remarks

[en]To more than two metrics or send more than four string properties, use [TrackEvent](#M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})') instead of this method.

[ja] 2個以上のメトリック、または、4個以上の文字列プロパティを送信する場合は、このメソッドの代わりに [TrackEvent](#M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})') を使用します。

<a name='M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}-'></a>
### TrackEvent(eventName,properties,metrics) `method`

##### Summary

[en] Send information about the custom event (such as usage of features and commands) in the application.
For ApplicationInsight, send an [EventTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-EventTelemetry 'Microsoft.ApplicationInsights.DataContracts.EventTelemetry') for display in Diagnostic Search and in the Analytics Portal.

[ja] アプリのカスタムイベント(機能やコマンドの利用状況など)についての情報を送信する。
ApplicationInsightにおいては、解析ポータルや診断検索で表示する [EventTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-EventTelemetry 'Microsoft.ApplicationInsights.DataContracts.EventTelemetry') を送信します。

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| eventName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] A name for the event. [ja]イベントの名前。 |
| properties | [System.Collections.Generic.IDictionary{System.String,System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IDictionary 'System.Collections.Generic.IDictionary{System.String,System.String}') | [en] Named string values you can use to search and classify events. [ja] イベントの検索や分類に使用できる名前付きの文字列値。 |
| metrics | [System.Collections.Generic.IDictionary{System.String,System.Double}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IDictionary 'System.Collections.Generic.IDictionary{System.String,System.Double}') | [en] Named measurements associated with this event. [ja] このイベントに関連する名前付きの測定値。 |

<a name='M-NishySoftware-Telemetry-ITelemetry-TrackEvent-System-String,NishySoftware-Telemetry-TriggerType,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}-'></a>
### TrackEvent(eventName,triggerType,properties,metrics) `method`

##### Summary

[en] Send information about the custom event (such as usage of features and commands) in the application.
For ApplicationInsight, send an [EventTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-EventTelemetry 'Microsoft.ApplicationInsights.DataContracts.EventTelemetry') for display in Diagnostic Search and in the Analytics Portal.

[ja] アプリのカスタムイベント(機能やコマンドの利用状況など)についての情報を送信する。
ApplicationInsightにおいては、解析ポータルや診断検索で表示する [EventTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-EventTelemetry 'Microsoft.ApplicationInsights.DataContracts.EventTelemetry') を送信します。

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| eventName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] A name for the event. [ja]イベントの名前。 |
| triggerType | [NishySoftware.Telemetry.TriggerType](#T-NishySoftware-Telemetry-TriggerType 'NishySoftware.Telemetry.TriggerType') | [en] Trigger type for the event. [ja]イベントのトリガーの種類。 |
| properties | [System.Collections.Generic.IDictionary{System.String,System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IDictionary 'System.Collections.Generic.IDictionary{System.String,System.String}') | [en] Named string values you can use to search and classify events. [ja] イベントの検索や分類に使用できる名前付きの文字列値。 |
| metrics | [System.Collections.Generic.IDictionary{System.String,System.Double}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IDictionary 'System.Collections.Generic.IDictionary{System.String,System.Double}') | [en] Named measurements associated with this event. [ja] このイベントに関連する名前付きの測定値。 |

<a name='M-NishySoftware-Telemetry-ITelemetry-TrackException-System-Exception,Microsoft-ApplicationInsights-DataContracts-SeverityLevel,System-String,System-String,System-String,System-String,System-String,System-String-'></a>
### TrackException(exception,level,prop1key,prop1value,prop2key,prop2value,prop3key,prop3value) `method`

##### Summary

[en] Send information about the exception event in the application.
For ApplicationInsight, send a separate [ExceptionTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-ExceptionTelemetry 'Microsoft.ApplicationInsights.DataContracts.ExceptionTelemetry') for display in Diagnostic Search for each call to this method.

[ja] アプリのExceptionイベントについての情報を送信する。
ApplicationInsightにおいては、診断検索で表示するための [ExceptionTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-ExceptionTelemetry 'Microsoft.ApplicationInsights.DataContracts.ExceptionTelemetry') をこのメソッドの呼び出しごとに個別に送信します。

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| exception | [System.Exception](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Exception 'System.Exception') | [en] The exception to log. [ja] 記録するException |
| level | [Microsoft.ApplicationInsights.DataContracts.SeverityLevel](#T-Microsoft-ApplicationInsights-DataContracts-SeverityLevel 'Microsoft.ApplicationInsights.DataContracts.SeverityLevel') | [en] Severity level. [ja] 深刻度 |
| prop1key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of string property value #1. [ja] 文字列プロパティ値1のキー名。 |
| prop1value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] String property value #1. [ja] 文字列プロパティ値1。 |
| prop2key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of string property value #2. [ja] 文字列プロパティ値2のキー名。 |
| prop2value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] String property value #2. [ja] 文字列プロパティ値2。 |
| prop3key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of string property value #3. [ja] 文字列プロパティ値3のキー名。 |
| prop3value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] String property value #3. [ja] 文字列プロパティ値3。 |

##### Remarks

[en]To send more than four string properties, use [TrackException](#M-NishySoftware-Telemetry-ITelemetry-TrackException-System-Exception,Microsoft-ApplicationInsights-DataContracts-SeverityLevel,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetry.TrackException(System.Exception,Microsoft.ApplicationInsights.DataContracts.SeverityLevel,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})') instead of this method.

[ja] 4個以上の文字列プロパティを送信する場合は、このメソッドの代わりに [TrackException](#M-NishySoftware-Telemetry-ITelemetry-TrackException-System-Exception,Microsoft-ApplicationInsights-DataContracts-SeverityLevel,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetry.TrackException(System.Exception,Microsoft.ApplicationInsights.DataContracts.SeverityLevel,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})') を使用します。

<a name='M-NishySoftware-Telemetry-ITelemetry-TrackException-System-Exception,Microsoft-ApplicationInsights-DataContracts-SeverityLevel,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}-'></a>
### TrackException(exception,level,properties,metrics) `method`

##### Summary

[en] Send information about the exception event in the application.
For ApplicationInsight, send a separate [ExceptionTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-ExceptionTelemetry 'Microsoft.ApplicationInsights.DataContracts.ExceptionTelemetry') for display in Diagnostic Search for each call to this method.

[ja] アプリのExceptionイベントについての情報を送信する。
ApplicationInsightにおいては、診断検索で表示するための [ExceptionTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-ExceptionTelemetry 'Microsoft.ApplicationInsights.DataContracts.ExceptionTelemetry') をこのメソッドの呼び出しごとに個別に送信します。

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| exception | [System.Exception](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Exception 'System.Exception') | [en] The exception to log. [ja] 記録するException |
| level | [Microsoft.ApplicationInsights.DataContracts.SeverityLevel](#T-Microsoft-ApplicationInsights-DataContracts-SeverityLevel 'Microsoft.ApplicationInsights.DataContracts.SeverityLevel') | [en] Severity level. [ja] 深刻度 |
| properties | [System.Collections.Generic.IDictionary{System.String,System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IDictionary 'System.Collections.Generic.IDictionary{System.String,System.String}') | [en] Named string values you can use to search and classify events. [ja] イベントの検索や分類に使用できる名前付きの文字列値。 |
| metrics | [System.Collections.Generic.IDictionary{System.String,System.Double}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IDictionary 'System.Collections.Generic.IDictionary{System.String,System.Double}') | [en] Named measurements associated with this event. [ja] このイベントに関連する名前付きの測定値。 |

<a name='M-NishySoftware-Telemetry-ITelemetry-TrackPageView-System-String,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}-'></a>
### TrackPageView(pageName,properties,metrics) `method`

##### Summary

[en] Send information about the pages, windows, and dialogs displayed by the app.
For ApplicationInsight, send a separate [PageViewTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-PageViewTelemetry 'Microsoft.ApplicationInsights.DataContracts.PageViewTelemetry') for each call to this method.

[ja] アプリで表示されたページ、ウィンドウ、ダイアログについての情報を送信する。
ApplicationInsightにおいては、[PageViewTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-PageViewTelemetry 'Microsoft.ApplicationInsights.DataContracts.PageViewTelemetry') をこのメソッドの呼び出しごとに個別に送信します。

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| pageName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] A name for the page. [ja]ページの名前。 |
| properties | [System.Collections.Generic.IDictionary{System.String,System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IDictionary 'System.Collections.Generic.IDictionary{System.String,System.String}') | [en] Named string values you can use to search and classify events. [ja] イベントの検索や分類に使用できる名前付きの文字列値。 |
| metrics | [System.Collections.Generic.IDictionary{System.String,System.Double}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IDictionary 'System.Collections.Generic.IDictionary{System.String,System.Double}') | [en] Named measurements associated with this event. [ja] このイベントに関連する名前付きの測定値。 |

<a name='M-NishySoftware-Telemetry-ITelemetry-TrackPageView-System-String,System-TimeSpan,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}-'></a>
### TrackPageView(pageName,duration,properties,metrics) `method`

##### Summary

[en] Send information about the pages, windows, and dialogs displayed by the app.
For ApplicationInsight, send a separate [PageViewTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-PageViewTelemetry 'Microsoft.ApplicationInsights.DataContracts.PageViewTelemetry') for each call to this method.

[ja] アプリで表示されたページ、ウィンドウ、ダイアログについての情報を送信する。
ApplicationInsightにおいては、[PageViewTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-PageViewTelemetry 'Microsoft.ApplicationInsights.DataContracts.PageViewTelemetry') をこのメソッドの呼び出しごとに個別に送信します。

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| pageName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] A name for the page. [ja]ページの名前。 |
| duration | [System.TimeSpan](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.TimeSpan 'System.TimeSpan') | [en] Page view duration. [ja] ページ閲覧時間。 |
| properties | [System.Collections.Generic.IDictionary{System.String,System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IDictionary 'System.Collections.Generic.IDictionary{System.String,System.String}') | [en] Named string values you can use to search and classify events. [ja] イベントの検索や分類に使用できる名前付きの文字列値。 |
| metrics | [System.Collections.Generic.IDictionary{System.String,System.Double}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IDictionary 'System.Collections.Generic.IDictionary{System.String,System.Double}') | [en] Named measurements associated with this event. [ja] このイベントに関連する名前付きの測定値。 |

<a name='M-NishySoftware-Telemetry-ITelemetry-TrackPageView-System-String,System-TimeSpan,System-String,System-Double,System-String,System-String,System-String,System-String,System-String,System-String-'></a>
### TrackPageView(pageName,duration,metric1key,metric1value,prop1key,prop1value,prop2key,prop2value,prop3key,prop3value) `method`

##### Summary

[en] Send information about the pages, windows, and dialogs displayed by the app.
For ApplicationInsight, send a separate [PageViewTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-PageViewTelemetry 'Microsoft.ApplicationInsights.DataContracts.PageViewTelemetry') for each call to this method.

[ja] アプリで表示されたページ、ウィンドウ、ダイアログについての情報を送信する。
ApplicationInsightにおいては、[PageViewTelemetry](#T-Microsoft-ApplicationInsights-DataContracts-PageViewTelemetry 'Microsoft.ApplicationInsights.DataContracts.PageViewTelemetry') をこのメソッドの呼び出しごとに個別に送信します。

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| pageName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] A name for the page. [ja]ページの名前。 |
| duration | [System.TimeSpan](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.TimeSpan 'System.TimeSpan') | [en] Page view duration. [ja] ページ閲覧時間。 |
| metric1key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of measurement value #1. [ja] 計測値1のキー名。 |
| metric1value | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | [en] Measurement value #1. [ja] 計測値1。 |
| prop1key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of string property value #1. [ja] 文字列プロパティ値1のキー名。 |
| prop1value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] String property value #1. [ja] 文字列プロパティ値1。 |
| prop2key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of string property value #2. [ja] 文字列プロパティ値2のキー名。 |
| prop2value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] String property value #2. [ja] 文字列プロパティ値2。 |
| prop3key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] Key name of string property value #3. [ja] 文字列プロパティ値3のキー名。 |
| prop3value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] String property value #3. [ja] 文字列プロパティ値3。 |

##### Remarks

[en]To more than two metrics or send more than four string properties, use [TrackPageView](#M-NishySoftware-Telemetry-ITelemetry-TrackPageView-System-String,System-TimeSpan,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetry.TrackPageView(System.String,System.TimeSpan,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})') instead of this method.

[ja] 2個以上のメトリック、または、4個以上の文字列プロパティを送信する場合は、このメソッドの代わりに [TrackPageView](#M-NishySoftware-Telemetry-ITelemetry-TrackPageView-System-String,System-TimeSpan,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}- 'NishySoftware.Telemetry.ITelemetry.TrackPageView(System.String,System.TimeSpan,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})') を使用します。

<a name='T-NishySoftware-Telemetry-ITelemetryContract'></a>
## ITelemetryContract `type`

##### Namespace

NishySoftware.Telemetry

<a name='P-NishySoftware-Telemetry-ITelemetryContract-GlobalExceptionMetrics'></a>
### GlobalExceptionMetrics `property`

##### Summary

*Inherit from parent.*

<a name='P-NishySoftware-Telemetry-ITelemetryContract-GlobalExceptionProperties'></a>
### GlobalExceptionProperties `property`

##### Summary

*Inherit from parent.*

<a name='P-NishySoftware-Telemetry-ITelemetryContract-GlobalMetrics'></a>
### GlobalMetrics `property`

##### Summary

*Inherit from parent.*

<a name='P-NishySoftware-Telemetry-ITelemetryContract-GlobalProperties'></a>
### GlobalProperties `property`

##### Summary

*Inherit from parent.*

<a name='P-NishySoftware-Telemetry-ITelemetryContract-GlobalSyncObject'></a>
### GlobalSyncObject `property`

##### Summary

*Inherit from parent.*

<a name='P-NishySoftware-Telemetry-ITelemetryContract-Metrics'></a>
### Metrics `property`

##### Summary

*Inherit from parent.*

<a name='P-NishySoftware-Telemetry-ITelemetryContract-Properties'></a>
### Properties `property`

##### Summary

*Inherit from parent.*

<a name='M-NishySoftware-Telemetry-ITelemetryContract-Flush'></a>
### Flush() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ITelemetryContract-TrackEvent-System-String,System-String,System-String,System-String,System-String,System-String,System-String-'></a>
### TrackEvent() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ITelemetryContract-TrackEvent-System-String,System-String,System-Double,System-String,System-String,System-String,System-String,System-String,System-String-'></a>
### TrackEvent() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ITelemetryContract-TrackEvent-System-String,NishySoftware-Telemetry-TriggerType,System-String,System-String,System-String,System-String,System-String,System-String-'></a>
### TrackEvent() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ITelemetryContract-TrackEvent-System-String,NishySoftware-Telemetry-TriggerType,System-String,System-Double,System-String,System-String,System-String,System-String,System-String,System-String-'></a>
### TrackEvent() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ITelemetryContract-TrackEvent-System-String,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}-'></a>
### TrackEvent() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ITelemetryContract-TrackEvent-System-String,NishySoftware-Telemetry-TriggerType,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}-'></a>
### TrackEvent() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ITelemetryContract-TrackException-System-Exception,Microsoft-ApplicationInsights-DataContracts-SeverityLevel,System-String,System-String,System-String,System-String,System-String,System-String-'></a>
### TrackException() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ITelemetryContract-TrackException-System-Exception,Microsoft-ApplicationInsights-DataContracts-SeverityLevel,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}-'></a>
### TrackException() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ITelemetryContract-TrackPageView-System-String,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}-'></a>
### TrackPageView() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ITelemetryContract-TrackPageView-System-String,System-TimeSpan,System-Collections-Generic-IDictionary{System-String,System-String},System-Collections-Generic-IDictionary{System-String,System-Double}-'></a>
### TrackPageView() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ITelemetryContract-TrackPageView-System-String,System-TimeSpan,System-String,System-Double,System-String,System-String,System-String,System-String,System-String,System-String-'></a>
### TrackPageView() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='T-NishySoftware-Telemetry-ApplicationInsights-SessionContextReader'></a>
## SessionContextReader `type`

##### Namespace

NishySoftware.Telemetry.ApplicationInsights

<a name='P-NishySoftware-Telemetry-ApplicationInsights-SessionContextReader-Instance'></a>
### Instance `property`

##### Summary

Gets or sets the singleton instance for our application context reader.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-SessionContextReader-GetSessionId'></a>
### GetSessionId() `method`

##### Summary

Gets the session id.

##### Returns

The session id.

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-SessionContextReader-IsFirstSession-System-String-'></a>
### IsFirstSession() `method`

##### Summary

Gets the first session.

##### Returns

The first session flag.

##### Parameters

This method has no parameters.

<a name='T-NishySoftware-Telemetry-ApplicationInsights-SessionTelemetryInitializer'></a>
## SessionTelemetryInitializer `type`

##### Namespace

NishySoftware.Telemetry.ApplicationInsights

<a name='M-NishySoftware-Telemetry-ApplicationInsights-SessionTelemetryInitializer-Initialize-Microsoft-ApplicationInsights-Channel-ITelemetry-'></a>
### Initialize() `method`

##### Summary

Populates session properties on a telemetry item.

##### Parameters

This method has no parameters.

<a name='T-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-SystemMetric'></a>
## SystemMetric `type`

##### Namespace

NishySoftware.Telemetry.ApplicationInsights.DeviceContextReader

##### Summary

Flags used with the Windows API (User32.dll):GetSystemMetrics(SystemMetric smIndex)
 
This Enum and declaration signature was written by Gabriel T. Sharp
ai_productions@verizon.net or osirisgothra@hotmail.com
Obtained on pinvoke.net, please contribute your code to support the wiki!

<a name='F-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-SystemMetric-SM_CXSCREEN'></a>
### SM_CXSCREEN `constants`

##### Summary

The width of the screen of the primary display monitor, in pixels. This is the same value obtained by calling 
GetDeviceCaps as follows: GetDeviceCaps( hdcPrimaryMonitor, HORZRES).

<a name='F-NishySoftware-Telemetry-ApplicationInsights-DeviceContextReader-SystemMetric-SM_CYSCREEN'></a>
### SM_CYSCREEN `constants`

##### Summary

The height of the screen of the primary display monitor, in pixels. This is the same value obtained by calling 
GetDeviceCaps as follows: GetDeviceCaps( hdcPrimaryMonitor, VERTRES).

<a name='T-NishySoftware-Telemetry-ApplicationInsights-Telemetry'></a>
## Telemetry `type`

##### Namespace

NishySoftware.Telemetry.ApplicationInsights

##### Summary

[en] A class that exposes an API to create an instance of the [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry') interface.

[ja] [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry') インターフェースのインスタンスを作成するAPIを公開するクラス

<a name='P-NishySoftware-Telemetry-ApplicationInsights-Telemetry-CommonDataKinds'></a>
### CommonDataKinds `property`

##### Summary

[en] Property that specifies the common telemetry data to be applied to all telemetry transmissions.

[ja] すべてのテレメトリー送信に適用する共通Telemetryデータを指定するプロパティ

<a name='M-NishySoftware-Telemetry-ApplicationInsights-Telemetry-CreateTelemetry'></a>
### CreateTelemetry() `method`

##### Summary

[en] Static function to create the [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry') interface

[ja] [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry') インターフェースを作成する静的関数

##### Returns

[en] [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry') interface

[ja]  インターフェース

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-Telemetry-Enable-System-Boolean-'></a>
### Enable() `method`

##### Summary

[en] Static function to enable/disable Telemetry
If disabled, calling the [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX() function will not send telemetry.

[ja] Telemetryを有効化・無効化する静的関数
無効化されている場合は、[ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX()関数を呼び出しても、テレメトリー送信されません

##### Returns

[en] old value

[ja] 変更前の値

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-Telemetry-EnableDeveloperMode-System-Boolean-'></a>
### EnableDeveloperMode() `method`

##### Summary

[en] Set and reset DeveloperMode.

[ja] DeveloperModeを設定・解除する

##### Returns

old value

##### Parameters

This method has no parameters.

##### Remarks

[en] This library is automatically set to DeveloperMode mode when the Debugger is attached.
When DeveloperMode is enabled, synchronous transmission is always performed when the [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX() function is called.
Synchronous sending slows down the application because there is a wait until the sending is completed when the [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX() function is called.
To prevent the app from running slowly during debugging, disable DeveloperMode.

[ja] このライブラリーはDebuggerがアッタッチされていたら、自動的にDeveloperModeモードに設定します。
DeveloperModeが有効な場合は、[ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX()関数を呼び出したときに常に同期送信が行われます。
同期送信では、[ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX()関数を呼び出したときに送信完了するまでの待ちが発生するため、アプリの動作が遅くなります。
デバッグ時にアプリの動作が遅くならないようにするためには、DeveloperModeを解除します。

<a name='M-NishySoftware-Telemetry-ApplicationInsights-Telemetry-IsEnabled'></a>
### IsEnabled() `method`

##### Summary

[en] Static function to check if Telemetry is enabled
If disabled, calling the [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX() function will not send telemetry.

[ja] Telemetryが有効化されているか確認する静的関数
無効化されている場合は、[ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX()関数を呼び出しても、テレメトリー送信されません

##### Returns

[en] current value

[ja] 現在の値

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-Telemetry-SetInstrumentationKey-System-String-'></a>
### SetInstrumentationKey(instrumentationKey) `method`

##### Summary

[en] Setup the InstrumentationKey.
Setup your InstrumentationKey using this method if you do not place it in the ApplicationInsights.config file.

[ja] InstrumentationKeyを設定します。
ApplicationInsights.configファイルにInstrumentationKeyを設定ていない場合は、このメソッドでInstrumentationKeyを設定します。

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| instrumentationKey | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] InstrumentationKey of Application Insights resource[ja] Application InsightsリソースのInstrumentationKey |

<a name='T-NishySoftware-Telemetry-TelemetryDataKinds'></a>
## TelemetryDataKinds `type`

##### Namespace

NishySoftware.Telemetry

##### Summary

[en] Flags to specify the Telemetry Data to be applied globally.

[ja] 全体に適用するTelemetry Dataを指定するためのフラグ

<a name='F-NishySoftware-Telemetry-TelemetryDataKinds-All'></a>
### All `constants`

##### Summary

[en] Add all common global properties to GlobalProperties

[ja] すべての共通グローバルプロパティをGlobalPropertiesに追加します

<a name='F-NishySoftware-Telemetry-TelemetryDataKinds-Default'></a>
### Default `constants`

##### Summary

[en] Add default common global properties to GlobalProperties

[ja] 既定の共通グローバルプロパティをGlobalPropertiesに追加します

<a name='F-NishySoftware-Telemetry-TelemetryDataKinds-DeviceManufacturer'></a>
### DeviceManufacturer `constants`

##### Summary

[en] Add Manufacture to GlobalProperties

[ja] Manufacture をGlobalPropertiesに追加します

<a name='F-NishySoftware-Telemetry-TelemetryDataKinds-DeviceName'></a>
### DeviceName `constants`

##### Summary

[en] Add DeviceName to GlobalProperties

[ja] DeviceName をGlobalPropertiesに追加します

<a name='F-NishySoftware-Telemetry-TelemetryDataKinds-ExeName'></a>
### ExeName `constants`

##### Summary

[en] Add ExeName to GlobalProperties

[ja] ExeName をGlobalPropertiesに追加します

<a name='F-NishySoftware-Telemetry-TelemetryDataKinds-HostName'></a>
### HostName `constants`

##### Summary

[en] Add HostName to GlobalProperties

[ja] HostName をGlobalPropertiesに追加します

<a name='F-NishySoftware-Telemetry-TelemetryDataKinds-Language'></a>
### Language `constants`

##### Summary

[en] Add Language to GlobalProperties

[ja] Language をGlobalPropertiesに追加します

<a name='F-NishySoftware-Telemetry-TelemetryDataKinds-NetworkSpeed'></a>
### NetworkSpeed `constants`

##### Summary

[en] Add NetworkType to GlobalProperties

[ja] NetworkType をGlobalMetricsに追加します

<a name='F-NishySoftware-Telemetry-TelemetryDataKinds-NetworkType'></a>
### NetworkType `constants`

##### Summary

[en] Add NetworkType to GlobalProperties

[ja] NetworkType をGlobalPropertiesに追加します

<a name='F-NishySoftware-Telemetry-TelemetryDataKinds-ScreenResolution'></a>
### ScreenResolution `constants`

##### Summary

[en] Add ScreenResolution to GlobalProperties

[ja] ScreenResolution をGlobalPropertiesに追加します

<a name='F-NishySoftware-Telemetry-TelemetryDataKinds-UserName'></a>
### UserName `constants`

##### Summary

[en] Add UserName to GlobalProperties

[ja] UserName をGlobalPropertiesに追加します

<a name='T-NishySoftware-Telemetry-TriggerType'></a>
## TriggerType `type`

##### Namespace

NishySoftware.Telemetry

##### Summary

[en] TriggerType

[ja] TriggerType

<a name='F-NishySoftware-Telemetry-TriggerType-Click'></a>
### Click `constants`

##### Summary

click operation on mouse

<a name='F-NishySoftware-Telemetry-TriggerType-ContextMenu'></a>
### ContextMenu `constants`

##### Summary

context menu operation

<a name='F-NishySoftware-Telemetry-TriggerType-Flick'></a>
### Flick `constants`

##### Summary

flick operation on touch panel

<a name='F-NishySoftware-Telemetry-TriggerType-Key'></a>
### Key `constants`

##### Summary

key operation

<a name='F-NishySoftware-Telemetry-TriggerType-None'></a>
### None `constants`

##### Summary

None

<a name='F-NishySoftware-Telemetry-TriggerType-Other'></a>
### Other `constants`

##### Summary

other operation

<a name='F-NishySoftware-Telemetry-TriggerType-Tap'></a>
### Tap `constants`

##### Summary

tap operation on touch panel

<a name='T-NishySoftware-Telemetry-ApplicationInsights-UnhandledExceptionTelemetryModule'></a>
## UnhandledExceptionTelemetryModule `type`

##### Namespace

NishySoftware.Telemetry.ApplicationInsights

##### Summary

The module subscribed to AppDomain.CurrentDomain.UnhandledException to send exceptions to ApplicationInsights.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-UnhandledExceptionTelemetryModule-#ctor'></a>
### #ctor() `constructor`

##### Summary

Initializes a new instance of the [UnhandledExceptionTelemetryModule](#T-NishySoftware-Telemetry-ApplicationInsights-UnhandledExceptionTelemetryModule 'NishySoftware.Telemetry.ApplicationInsights.UnhandledExceptionTelemetryModule') class.

##### Parameters

This constructor has no parameters.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-UnhandledExceptionTelemetryModule-Dispose'></a>
### Dispose() `method`

##### Summary

Disposing UnhandledExceptionTelemetryModule instance.

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-UnhandledExceptionTelemetryModule-Initialize-Microsoft-ApplicationInsights-Extensibility-TelemetryConfiguration-'></a>
### Initialize() `method`

##### Summary

Initializes the telemetry module.

##### Parameters

This method has no parameters.

<a name='T-NishySoftware-Telemetry-ApplicationInsights-UnobservedExceptionTelemetryModule'></a>
## UnobservedExceptionTelemetryModule `type`

##### Namespace

NishySoftware.Telemetry.ApplicationInsights

##### Summary

The module subscribed to TaskScheduler.UnobservedTaskException to send exceptions to ApplicationInsights.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-UnobservedExceptionTelemetryModule-#ctor'></a>
### #ctor() `constructor`

##### Summary

Initializes a new instance of the [UnobservedExceptionTelemetryModule](#T-NishySoftware-Telemetry-ApplicationInsights-UnobservedExceptionTelemetryModule 'NishySoftware.Telemetry.ApplicationInsights.UnobservedExceptionTelemetryModule') class.

##### Parameters

This constructor has no parameters.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-UnobservedExceptionTelemetryModule-Dispose'></a>
### Dispose() `method`

##### Summary

Disposing TaskSchedulerOnUnobservedTaskException instance.

##### Parameters

This method has no parameters.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-UnobservedExceptionTelemetryModule-Initialize-Microsoft-ApplicationInsights-Extensibility-TelemetryConfiguration-'></a>
### Initialize(configuration) `method`

##### Summary

Initializes the telemetry module.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| configuration | [Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration](#T-Microsoft-ApplicationInsights-Extensibility-TelemetryConfiguration 'Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration') | Telemetry Configuration used for creating TelemetryClient for sending exceptions to ApplicationInsights. |

<a name='T-NishySoftware-Telemetry-ApplicationInsights-UserContextReader'></a>
## UserContextReader `type`

##### Namespace

NishySoftware.Telemetry.ApplicationInsights

<a name='P-NishySoftware-Telemetry-ApplicationInsights-UserContextReader-Instance'></a>
### Instance `property`

##### Summary

Gets or sets the singleton instance for our application context reader.

<a name='M-NishySoftware-Telemetry-ApplicationInsights-UserContextReader-GetUserUniqueId'></a>
### GetUserUniqueId() `method`

##### Summary

Gets the user id.

##### Returns

The user id.

##### Parameters

This method has no parameters.

<a name='T-NishySoftware-Telemetry-ApplicationInsights-UserTelemetryInitializer'></a>
## UserTelemetryInitializer `type`

##### Namespace

NishySoftware.Telemetry.ApplicationInsights

<a name='M-NishySoftware-Telemetry-ApplicationInsights-UserTelemetryInitializer-Initialize-Microsoft-ApplicationInsights-Channel-ITelemetry-'></a>
### Initialize() `method`

##### Summary

Populates user properties on a telemetry item.

##### Parameters

This method has no parameters.
