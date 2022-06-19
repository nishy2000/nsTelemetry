<a name='assembly'></a>
# nsTelemetryAI

## Contents

- [DeveloperModeWithDebuggerAttachedTelemetryModule](#T-NishySoftware-Telemetry-ApplicationInsights-DeveloperModeWithDebuggerAttachedTelemetryModule 'NishySoftware.Telemetry.ApplicationInsights.DeveloperModeWithDebuggerAttachedTelemetryModule')
- [Telemetry](#T-NishySoftware-Telemetry-ApplicationInsights-Telemetry 'NishySoftware.Telemetry.ApplicationInsights.Telemetry')
  - [CommonDataKinds](#P-NishySoftware-Telemetry-ApplicationInsights-Telemetry-CommonDataKinds 'NishySoftware.Telemetry.ApplicationInsights.Telemetry.CommonDataKinds')
  - [CreateTelemetry()](#M-NishySoftware-Telemetry-ApplicationInsights-Telemetry-CreateTelemetry 'NishySoftware.Telemetry.ApplicationInsights.Telemetry.CreateTelemetry')
  - [Enable(enable)](#M-NishySoftware-Telemetry-ApplicationInsights-Telemetry-Enable-System-Boolean- 'NishySoftware.Telemetry.ApplicationInsights.Telemetry.Enable(System.Boolean)')
  - [EnableDeveloperMode(enable)](#M-NishySoftware-Telemetry-ApplicationInsights-Telemetry-EnableDeveloperMode-System-Boolean- 'NishySoftware.Telemetry.ApplicationInsights.Telemetry.EnableDeveloperMode(System.Boolean)')
  - [IsEnabled()](#M-NishySoftware-Telemetry-ApplicationInsights-Telemetry-IsEnabled 'NishySoftware.Telemetry.ApplicationInsights.Telemetry.IsEnabled')
  - [SetInstrumentationKey(instrumentationKey)](#M-NishySoftware-Telemetry-ApplicationInsights-Telemetry-SetInstrumentationKey-System-String- 'NishySoftware.Telemetry.ApplicationInsights.Telemetry.SetInstrumentationKey(System.String)')
- [UnhandledExceptionTelemetryModule](#T-NishySoftware-Telemetry-ApplicationInsights-UnhandledExceptionTelemetryModule 'NishySoftware.Telemetry.ApplicationInsights.UnhandledExceptionTelemetryModule')
- [UnobservedExceptionTelemetryModule](#T-NishySoftware-Telemetry-ApplicationInsights-UnobservedExceptionTelemetryModule 'NishySoftware.Telemetry.ApplicationInsights.UnobservedExceptionTelemetryModule')

<a name='T-NishySoftware-Telemetry-ApplicationInsights-DeveloperModeWithDebuggerAttachedTelemetryModule'></a>
## DeveloperModeWithDebuggerAttachedTelemetryModule `type`

##### Namespace

NishySoftware.Telemetry.ApplicationInsights

##### Summary

[en] Telemetry module that sets developer mode to true when is not already set AND managed debugger is attached.

[ja] デベロッパーモードが設定されていなくて、デバッガーがアタッチされているときに、デベロッパーモードをセットするTelemetryモジュール

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
### Enable(enable) `method`

##### Summary

[en] Static function to enable/disable Telemetry
If disabled, calling the [ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX() function will not send telemetry.

[ja] Telemetryを有効化・無効化する静的関数
無効化されている場合は、[ITelemetry](#T-NishySoftware-Telemetry-ITelemetry 'NishySoftware.Telemetry.ITelemetry').TrackXXXX()関数を呼び出しても、テレメトリー送信されません

##### Returns

[en] old value

[ja] 変更前の値

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| enable | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | [en] new value. [ja]新しい値。 |

<a name='M-NishySoftware-Telemetry-ApplicationInsights-Telemetry-EnableDeveloperMode-System-Boolean-'></a>
### EnableDeveloperMode(enable) `method`

##### Summary

[en] Set and reset DeveloperMode.

[ja] DeveloperModeを設定・解除する

##### Returns

[en] old value.

[ja]変更前の値。

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| enable | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | [en] new value. [ja]新しい値。 |

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
ApplicationInsights.configファイルにInstrumentationKeyを設定していない場合は、このメソッドでInstrumentationKeyを設定します。

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| instrumentationKey | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | [en] InstrumentationKey of Application Insights resource [ja] Application InsightsリソースのInstrumentationKey |

<a name='T-NishySoftware-Telemetry-ApplicationInsights-UnhandledExceptionTelemetryModule'></a>
## UnhandledExceptionTelemetryModule `type`

##### Namespace

NishySoftware.Telemetry.ApplicationInsights

##### Summary

The module subscribed to AppDomain.CurrentDomain.UnhandledException to send exceptions to ApplicationInsights.

[ja] ApplicationInsightsに例外の発生を送信するために、AppDomain.CurrentDomain.UnhandledExceptionイベントを購読するためのTelemetryモジュール

<a name='T-NishySoftware-Telemetry-ApplicationInsights-UnobservedExceptionTelemetryModule'></a>
## UnobservedExceptionTelemetryModule `type`

##### Namespace

NishySoftware.Telemetry.ApplicationInsights

##### Summary

The module subscribed to TaskScheduler.UnobservedTaskException to send exceptions to ApplicationInsights.

[ja] ApplicationInsightsに例外の発生を送信するために、TaskScheduler.UnobservedTaskExceptionイベントを購読するためのTelemetryモジュール
