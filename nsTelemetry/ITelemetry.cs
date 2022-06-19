/* ==============================
** Copyright 2015, 2021 nishy software
**
**      First Author : nishy software
**		Create : 2015/12/07
** ============================== */

namespace NishySoftware.Telemetry
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// [en] TriggerType
    /// <br/>
    /// [ja] TriggerType
    /// </summary>
    public enum TriggerType
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// key operation
        /// </summary>
        Key,
        /// <summary>
        /// context menu operation
        /// </summary>
        ContextMenu,
        /// <summary>
        /// click operation on mouse
        /// </summary>
        Click,
        /// <summary>
        /// tap operation on touch panel
        /// </summary>
        Tap,
        /// <summary>
        /// flick operation on touch panel
        /// </summary>
        Flick,
        /// <summary>
        /// other operation
        /// </summary>
        Other
    }

    /// <summary>
    /// [en] Flags to specify the Telemetry Data to be applied globally.
    /// <br/>
    /// [ja] 全体に適用するTelemetry Dataを指定するためのフラグ
    /// </summary>
    [Flags]
    public enum TelemetryDataKinds : ulong
    {
        /// <summary>
        /// [en] Add NetworkType to GlobalProperties
        /// <br/>
        /// [ja] NetworkType をGlobalPropertiesに追加します
        /// </summary>
        NetworkType = 1,

        /// <summary>
        /// [en] Add NetworkType to GlobalProperties
        /// <br/>
        /// [ja] NetworkType をGlobalMetricsに追加します
        /// </summary>
        NetworkSpeed = 2,

        /// <summary>
        /// [en] Add DeviceName to GlobalProperties
        /// <br/>
        /// [ja] DeviceName をGlobalPropertiesに追加します
        /// </summary>
        DeviceName = 4,

        /// <summary>
        /// [en] Add Manufacture to GlobalProperties
        /// <br/>
        /// [ja] Manufacture をGlobalPropertiesに追加します
        /// </summary>
        DeviceManufacturer = 8,

        /// <summary>
        /// [en] Add ScreenResolution to GlobalProperties
        /// <br/>
        /// [ja] ScreenResolution をGlobalPropertiesに追加します
        /// </summary>
        ScreenResolution = 16,

        /// <summary>
        /// [en] Add Language to GlobalProperties
        /// <br/>
        /// [ja] Language をGlobalPropertiesに追加します
        /// </summary>
        Language = 32,

        /// <summary>
        /// [en] Add ExeName to GlobalProperties
        /// <br/>
        /// [ja] ExeName をGlobalPropertiesに追加します
        /// </summary>
        ExeName = 64,

        /// <summary>
        /// [en] Add HostName to GlobalProperties
        /// <br/>
        /// [ja] HostName をGlobalPropertiesに追加します
        /// </summary>
        HostName = 128,

        /// <summary>
        /// [en] Add UserName to GlobalProperties
        /// <br/>
        /// [ja] UserName をGlobalPropertiesに追加します
        /// </summary>
        UserName = 256,

        /// <summary>
        /// [en] Add all common global properties to GlobalProperties
        /// <br/>
        /// [ja] すべての共通グローバルプロパティをGlobalPropertiesに追加します
        /// </summary>
        All = 0x00ffffff,

        /// <summary>
        /// [en] Add default common global properties to GlobalProperties
        /// <br/>
        /// [ja] 既定の共通グローバルプロパティをGlobalPropertiesに追加します
        /// </summary>
        Default = NetworkType | NetworkSpeed | DeviceName | DeviceManufacturer | ScreenResolution | Language | ExeName,
    };

    /// <summary>
    /// [en] Enumeration values used by ExceptionTelemetry (TrackException) and TraceTelemetry to identify severity levels
    /// <br/>
    /// [ja] ExceptionTelemetry (TrackException) および TraceTelemetry が重大度レベルを識別するために使用する列挙値
    /// </summary>
    public enum SeverityLevel
    {
        /// <summary>
        /// [en] Verbose severity level
        /// <br/>
        /// [ja] 冗長の重要度
        /// </summary>
        Verbose = 0,
        /// <summary>
        /// [en] Information severity level
        /// <br/>
        /// [ja] 情報の重要度
        /// </summary>
        Information = 1,
        /// <summary>
        /// [en] Warning severity level
        /// <br/>
        /// [ja] 警告の重要度
        /// </summary>
        Warning = 2,
        /// <summary>
        /// [en] Error severity level
        /// <br/>
        /// [ja] エラーの重要度
        /// </summary>
        Error = 3,
        /// <summary>
        /// [en] Critical severity level
        /// <br/>
        /// [ja] 致命的な重要度
        /// </summary>
        Critical = 4
    }

    /// <summary>
    /// [en] Interface for Telemetry
    /// <br/>
    /// [ja] Telemetry用のインターフェース
    /// </summary>
    [ContractClass(typeof(ITelemetryContract))]
    public interface ITelemetry
    {
        /// <summary>
        /// [en] LockObject to use when manipulating and referencing <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalProperties"/> / <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalMetrics"/> / <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalExceptionProperties"/> / <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalExceptionMetrics"/>.
        /// <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalProperties"/> / <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalMetrics"/> / <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalExceptionProperties"/> / <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalExceptionMetrics"/> are shared globally, so we don't know which thread is using them.
        /// Therefore, we can use this object to Lock and then manipulate and reference it.
        /// <br/>
        /// [ja] <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalProperties"/> / <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalMetrics"/> / <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalExceptionProperties"/> / <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalExceptionMetrics"/> を操作・参照するときに使用するLockObject。
        /// <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalProperties"/> / <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalMetrics"/> / <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalExceptionProperties"/> / <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalExceptionMetrics"/> は、グローバルで共有されるため、どのスレッドで使用されているかわかりません。
        /// そのため、このオブジェクトを使ってLockしてから、操作・参照します。
        /// </summary>
        object GlobalSyncObject { get; }

        /// <summary>
        /// [en] The property that holds the properties to be sent by default.
        /// Since this property is shared globally, you need to lock the <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalSyncObject"/> when manipulating or referencing this property.
        /// <br/>
        /// [ja] 既定で送信するプロパティを保持するプロパティ。
        /// このプロパティはグローバルで共有されるため、このプロパティを操作・参照するときは、<see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalSyncObject"/> でLockする必要があります。
        /// </summary>
        /// <remarks>
        /// [en] The properties held by this property will be sent by all <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX(), regardless of the Interface instance.
        /// If a property with the same name is specified in an instance of Interface or in an argument of <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX(),
        /// the value with the same name will take precedence over the value in the order of argument and Interface
        /// <br/>
        /// [ja] このプロパティが保持するプロパティはInterfaceのインスタンスによらず、すべての <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX() で送信されます。
        /// Interfaceのインスタンスや <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX() の引数で同じ名前のプロパティが指定された場合は、同じ名前の値は、引数、Interfaceの順で優先されます。
        /// </remarks>
        IDictionary<string, string> GlobalProperties { get; }

        /// <summary>
        /// [en] The property that holds the metrics to be sent by default.
        /// Since this property is shared globally, you need to lock the <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalSyncObject"/> when manipulating or referencing this property.
        /// <br/>
        /// [ja] 既定で送信するメトリックを保持するプロパティ。
        /// このプロパティはグローバルで共有されるため、このプロパティを操作・参照するときは、<see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalSyncObject"/> でLockする必要があります。
        /// </summary>
        /// <remarks>
        /// [en] The metrics held by this property will be sent by all <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX(), regardless of the Interface instance.
        /// If a metric with the same name is specified in an instance of Interface or in an argument of <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX(),
        /// the value with the same name will take precedence over the value in the order of argument and Interface
        /// <br/>
        /// [ja] このプロパティが保持するメトリックはInterfaceのインスタンスによらず、すべての <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX() で送信されます。
        /// Interfaceのインスタンスや <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX() の引数で同じ名前のメトリックが指定された場合は、同じ名前の値は、引数、Interfaceの順で優先されます。
        /// </remarks>
        IDictionary<string, double> GlobalMetrics { get; }

        /// <summary>
        /// [en] The property that holds the properties to be sent by default when using ITelemetry.TrackException().
        /// Since this property is shared globally, you need to lock the <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalSyncObject"/> when manipulating or referencing this property.
        /// <br/>
        /// [ja] ITelemetry.TrackException() を使用するときに既定で送信するプロパティを保持するプロパティ。
        /// このプロパティはグローバルで共有されるため、このプロパティを操作・参照するときは、<see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalSyncObject"/> でLockする必要があります。
        /// </summary>
        /// <remarks>
        /// [en] The properties held by this property will be sent by all ITelemetry.TrackException(), regardless of the Interface instance.
        /// If a property with the same name is specified in an instance of Interface or in an argument of ITelemetry.TrackException(),
        /// the value with the same name will take precedence over the value in the order of argument and Interface
        /// <br/>
        /// [ja] このプロパティが保持するプロパティはInterfaceのインスタンスによらず、すべての ITelemetry.TrackException() で送信されます。
        /// Interfaceのインスタンスや ITelemetry.TrackException() の引数で同じ名前のプロパティが指定された場合は、同じ名前の値は、引数、Interfaceの順で優先されます。
        /// </remarks>
        IDictionary<string, string> GlobalExceptionProperties { get; }

        /// <summary>
        /// [en] The property that holds the metrics to be sent by default when using ITelemetry.TrackException().
        /// Since this property is shared globally, you need to lock the <see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalSyncObject"/> when manipulating or referencing this property.
        /// <br/>
        /// [ja] ITelemetry.TrackException() を使用するときに既定で送信するメトリックを保持するプロパティ。
        /// このプロパティはグローバルで共有されるため、このプロパティを操作・参照するときは、<see cref="P:NishySoftware.Telemetry.ITelemetry.GlobalSyncObject"/> でLockする必要があります。
        /// </summary>
        /// <remarks>
        /// [en] The metrics held by this property will be sent by all ITelemetry.TrackException(), regardless of the Interface instance.
        /// If a metric with the same name is specified in an instance of Interface or in an argument of ITelemetry.TrackException(),
        /// the value with the same name will take precedence over the value in the order of argument and Interface
        /// <br/>
        /// [ja] このプロパティが保持するメトリックはInterfaceのインスタンスによらず、すべての ITelemetry.TrackException() で送信されます。
        /// Interfaceのインスタンスや ITelemetry.TrackException() の引数で同じ名前のメトリックが指定された場合は、同じ名前の値は、引数、Interfaceの順で優先されます。
        /// </remarks>
        IDictionary<string, double> GlobalExceptionMetrics { get; }

        /// <summary>
        /// [en] The property that holds the properties that an instance of this interface sends by default.
        /// <br/>
        /// [ja] このインターフェースのインスタンスが既定で送信するプロパティを保持するプロパティ。
        /// </summary>
        /// <remarks>
        /// [en] The properties held by this property will be sent in all <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX() issued by instances of this Interface.
        /// If a property with the same name is specified in the argument of <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX(),
        /// the value of the argument will take precedence over the value of the same name
        /// <br/>
        /// [ja] このプロパティが保持するプロパティはこのInterfaceのインスタンスから発行するすべての <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX() で送信されます。
        /// <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX() の引数で同じ名前のプロパティが指定された場合は、同じ名前の値は、引数の値が優先されます。
        /// </remarks>
        IDictionary<string, string> Properties { get; }

        /// <summary>
        /// [en] The property that holds the metrics that an instance of this interface sends by default.
        /// <br/>
        /// [ja] このインターフェースのインスタンスが既定で送信するメトリックを保持するプロパティ。
        /// </summary>
        /// <remarks>
        /// [en] The metrics held by this property will be sent in all <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX() issued by instances of this Interface.
        /// If a metric with the same name is specified in the argument of <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX(),
        /// the value of the argument will take precedence over the value of the same name
        /// <br/>
        /// [ja] このプロパティが保持するメトリックはこのInterfaceのインスタンスから発行するすべての <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX() で送信されます。
        /// <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX() の引数で同じ名前のメトリックが指定された場合は、同じ名前の値は、引数の値が優先されます。
        /// </remarks>
        IDictionary<string, double> Metrics { get; }

        /// <summary>
        /// [en] Send information about the custom event (such as usage of features and commands) in the application.
        /// For ApplicationInsight, send an <see cref="T:Microsoft.ApplicationInsights.DataContracts.EventTelemetry"/> for display in Diagnostic Search and in the Analytics Portal.
        /// <br/>
        /// [ja] アプリのカスタムイベント(機能やコマンドの利用状況など)についての情報を送信する。
        /// ApplicationInsightにおいては、解析ポータルや診断検索で表示する <see cref="T:Microsoft.ApplicationInsights.DataContracts.EventTelemetry"/> を送信します。
        /// </summary>
        /// <remarks>
        /// [en]To send more than four string properties, use <see cref="M:NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})"/> instead of this method.
        /// <br/>
        /// [ja] 4個以上の文字列プロパティを送信する場合は、このメソッドの代わりに <see cref="M:NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})"/> を使用します。
        /// </remarks>
        /// <param name="eventName">[en] A name for the event.<br/> [ja]イベントの名前。</param>
        /// <param name="prop1key">[en] Key name of string property value #1.<br/> [ja] 文字列プロパティ値1のキー名。</param>
        /// <param name="prop1value">[en] String property value #1.<br/> [ja] 文字列プロパティ値1。</param>
        /// <param name="prop2key">[en] Key name of string property value #2.<br/> [ja] 文字列プロパティ値2のキー名。</param>
        /// <param name="prop2value">[en] String property value #2.<br/> [ja] 文字列プロパティ値2。</param>
        /// <param name="prop3key">[en] Key name of string property value #3.<br/> [ja] 文字列プロパティ値3のキー名。</param>
        /// <param name="prop3value">[en] String property value #3.<br/> [ja] 文字列プロパティ値3。</param>
        void TrackEvent(string eventName, string prop1key, string prop1value, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null);

        /// <summary>
        /// [en] Send information about the custom event (such as usage of features and commands) in the application.
        /// For ApplicationInsight, send an <see cref="T:Microsoft.ApplicationInsights.DataContracts.EventTelemetry"/> for display in Diagnostic Search and in the Analytics Portal.
        /// <br/>
        /// [ja] アプリのカスタムイベント(機能やコマンドの利用状況など)についての情報を送信する。
        /// ApplicationInsightにおいては、解析ポータルや診断検索で表示する <see cref="T:Microsoft.ApplicationInsights.DataContracts.EventTelemetry"/> を送信します。
        /// </summary>
        /// <remarks>
        /// [en]To more than two metrics or send more than four string properties, use <see cref="M:NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})"/> instead of this method.
        /// <br/>
        /// [ja] 2個以上のメトリック、または、4個以上の文字列プロパティを送信する場合は、このメソッドの代わりに <see cref="M:NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})"/> を使用します。
        /// </remarks>
        /// <param name="eventName">[en] A name for the event. <br/> [ja]イベントの名前。</param>
        /// <param name="metric1key">[en] Key name of measurement value #1. <br/> [ja] 計測値1のキー名。</param>
        /// <param name="metric1value">[en] Measurement value #1. <br/> [ja] 計測値1。</param>
        /// <param name="prop1key">[en] Key name of string property value #1. <br/> [ja] 文字列プロパティ値1のキー名。</param>
        /// <param name="prop1value">[en] String property value #1. <br/> [ja] 文字列プロパティ値1。</param>
        /// <param name="prop2key">[en] Key name of string property value #2. <br/> [ja] 文字列プロパティ値2のキー名。</param>
        /// <param name="prop2value">[en] String property value #2. <br/> [ja] 文字列プロパティ値2。</param>
        /// <param name="prop3key">[en] Key name of string property value #3. <br/> [ja] 文字列プロパティ値3のキー名。</param>
        /// <param name="prop3value">[en] String property value #3. <br/> [ja] 文字列プロパティ値3。</param>
        void TrackEvent(string eventName, string metric1key, double metric1value, string prop1key = null, string prop1value = null, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null);

        /// <summary>
        /// [en] Send information about the custom event (such as usage of features and commands) in the application.
        /// For ApplicationInsight, send an <see cref="T:Microsoft.ApplicationInsights.DataContracts.EventTelemetry"/> for display in Diagnostic Search and in the Analytics Portal.
        /// <br/>
        /// [ja] アプリのカスタムイベント(機能やコマンドの利用状況など)についての情報を送信する。
        /// ApplicationInsightにおいては、解析ポータルや診断検索で表示する <see cref="T:Microsoft.ApplicationInsights.DataContracts.EventTelemetry"/> を送信します。
        /// </summary>
        /// <remarks>
        /// [en]To send more than four string properties, use <see cref="M:NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})"/> instead of this method.
        /// <br/>
        /// [ja] 4個以上の文字列プロパティを送信する場合は、このメソッドの代わりに <see cref="M:NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})"/> を使用します。
        /// </remarks>
        /// <param name="eventName">[en] A name for the event. <br/> [ja]イベントの名前。</param>
        /// <param name="triggerType">[en] Trigger type for the event. <br/> [ja]イベントのトリガーの種類。</param>
        /// <param name="prop1key">[en] Key name of string property value #1. <br/> [ja] 文字列プロパティ値1のキー名。</param>
        /// <param name="prop1value">[en] String property value #1. <br/> [ja] 文字列プロパティ値1。</param>
        /// <param name="prop2key">[en] Key name of string property value #2. <br/> [ja] 文字列プロパティ値2のキー名。</param>
        /// <param name="prop2value">[en] String property value #2. <br/> [ja] 文字列プロパティ値2。</param>
        /// <param name="prop3key">[en] Key name of string property value #3. <br/> [ja] 文字列プロパティ値3のキー名。</param>
        /// <param name="prop3value">[en] String property value #3. <br/> [ja] 文字列プロパティ値3。</param>
        void TrackEvent(string eventName, TriggerType triggerType, string prop1key, string prop1value, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null);

        /// <summary>
        /// [en] Send information about the custom event (such as usage of features and commands) in the application.
        /// For ApplicationInsight, send an <see cref="T:Microsoft.ApplicationInsights.DataContracts.EventTelemetry"/> for display in Diagnostic Search and in the Analytics Portal.
        /// <br/>
        /// [ja] アプリのカスタムイベント(機能やコマンドの利用状況など)についての情報を送信する。
        /// ApplicationInsightにおいては、解析ポータルや診断検索で表示する <see cref="T:Microsoft.ApplicationInsights.DataContracts.EventTelemetry"/> を送信します。
        /// </summary>
        /// <remarks>
        /// [en]To more than two metrics or send more than four string properties, use <see cref="M:NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})"/> instead of this method.
        /// <br/>
        /// [ja] 2個以上のメトリック、または、4個以上の文字列プロパティを送信する場合は、このメソッドの代わりに <see cref="M:NishySoftware.Telemetry.ITelemetry.TrackEvent(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})"/> を使用します。
        /// </remarks>
        /// <param name="eventName">[en] A name for the event. <br/> [ja]イベントの名前。</param>
        /// <param name="triggerType">[en] Trigger type for the event. <br/> [ja]イベントのトリガーの種類。</param>
        /// <param name="metric1key">[en] Key name of measurement value #1. <br/> [ja] 計測値1のキー名。</param>
        /// <param name="metric1value">[en] Measurement value #1. <br/> [ja] 計測値1。</param>
        /// <param name="prop1key">[en] Key name of string property value #1. <br/> [ja] 文字列プロパティ値1のキー名。</param>
        /// <param name="prop1value">[en] String property value #1. <br/> [ja] 文字列プロパティ値1。</param>
        /// <param name="prop2key">[en] Key name of string property value #2. <br/> [ja] 文字列プロパティ値2のキー名。</param>
        /// <param name="prop2value">[en] String property value #2. <br/> [ja] 文字列プロパティ値2。</param>
        /// <param name="prop3key">[en] Key name of string property value #3. <br/> [ja] 文字列プロパティ値3のキー名。</param>
        /// <param name="prop3value">[en] String property value #3. <br/> [ja] 文字列プロパティ値3。</param>
        void TrackEvent(string eventName, TriggerType triggerType, string metric1key, double metric1value, string prop1key = null, string prop1value = null, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null);

        /// <summary>
        /// [en] Send information about the custom event (such as usage of features and commands) in the application.
        /// For ApplicationInsight, send an <see cref="T:Microsoft.ApplicationInsights.DataContracts.EventTelemetry"/> for display in Diagnostic Search and in the Analytics Portal.
        /// <br/>
        /// [ja] アプリのカスタムイベント(機能やコマンドの利用状況など)についての情報を送信する。
        /// ApplicationInsightにおいては、解析ポータルや診断検索で表示する <see cref="T:Microsoft.ApplicationInsights.DataContracts.EventTelemetry"/> を送信します。
        /// </summary>
        /// <param name="eventName">[en] A name for the event. <br/> [ja]イベントの名前。</param>
        /// <param name="properties">[en] Named string values you can use to search and classify events. <br/> [ja] イベントの検索や分類に使用できる名前付きの文字列値。</param>
        /// <param name="metrics">[en] Named measurements associated with this event. <br/> [ja] このイベントに関連する名前付きの測定値。</param>
        void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

        /// <summary>
        /// [en] Send information about the custom event (such as usage of features and commands) in the application.
        /// For ApplicationInsight, send an <see cref="T:Microsoft.ApplicationInsights.DataContracts.EventTelemetry"/> for display in Diagnostic Search and in the Analytics Portal.
        /// <br/>
        /// [ja] アプリのカスタムイベント(機能やコマンドの利用状況など)についての情報を送信する。
        /// ApplicationInsightにおいては、解析ポータルや診断検索で表示する <see cref="T:Microsoft.ApplicationInsights.DataContracts.EventTelemetry"/> を送信します。
        /// </summary>
        /// <param name="eventName">[en] A name for the event. <br/> [ja]イベントの名前。</param>
        /// <param name="triggerType">[en] Trigger type for the event. <br/> [ja]イベントのトリガーの種類。</param>
        /// <param name="properties">[en] Named string values you can use to search and classify events. <br/> [ja] イベントの検索や分類に使用できる名前付きの文字列値。</param>
        /// <param name="metrics">[en] Named measurements associated with this event. <br/> [ja] このイベントに関連する名前付きの測定値。</param>
        void TrackEvent(string eventName, TriggerType triggerType, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

        /// <summary>
        /// [en] Send information about the pages, windows, and dialogs displayed by the app.
        /// For ApplicationInsight, send a separate <see cref="T:Microsoft.ApplicationInsights.DataContracts.PageViewTelemetry"/> for each call to this method.
        /// <br/>
        /// [ja] アプリで表示されたページ、ウィンドウ、ダイアログについての情報を送信する。
        /// ApplicationInsightにおいては、<see cref="T:Microsoft.ApplicationInsights.DataContracts.PageViewTelemetry"/> をこのメソッドの呼び出しごとに個別に送信します。
        /// </summary>
        /// <param name="pageName">[en] A name for the page. <br/> [ja]ページの名前。</param>
        /// <param name="properties">[en] Named string values you can use to search and classify events. <br/> [ja] イベントの検索や分類に使用できる名前付きの文字列値。</param>
        /// <param name="metrics">[en] Named measurements associated with this event. <br/> [ja] このイベントに関連する名前付きの測定値。</param>
        void TrackPageView(string pageName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

        /// <summary>
        /// [en] Send information about the pages, windows, and dialogs displayed by the app.
        /// For ApplicationInsight, send a separate <see cref="T:Microsoft.ApplicationInsights.DataContracts.PageViewTelemetry"/> for each call to this method.
        /// <br/>
        /// [ja] アプリで表示されたページ、ウィンドウ、ダイアログについての情報を送信する。
        /// ApplicationInsightにおいては、<see cref="T:Microsoft.ApplicationInsights.DataContracts.PageViewTelemetry"/> をこのメソッドの呼び出しごとに個別に送信します。
        /// </summary>
        /// <param name="pageName">[en] A name for the page. <br/> [ja]ページの名前。</param>
        /// <param name="duration">[en] Page view duration. <br/> [ja] ページ閲覧時間。</param>
        /// <param name="properties">[en] Named string values you can use to search and classify events. <br/> [ja] イベントの検索や分類に使用できる名前付きの文字列値。</param>
        /// <param name="metrics">[en] Named measurements associated with this event. <br/> [ja] このイベントに関連する名前付きの測定値。</param>
        void TrackPageView(string pageName, TimeSpan duration, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);


        /// <summary>
        /// [en] Send information about the pages, windows, and dialogs displayed by the app.
        /// For ApplicationInsight, send a separate <see cref="T:Microsoft.ApplicationInsights.DataContracts.PageViewTelemetry"/> for each call to this method.
        /// <br/>
        /// [ja] アプリで表示されたページ、ウィンドウ、ダイアログについての情報を送信する。
        /// ApplicationInsightにおいては、<see cref="T:Microsoft.ApplicationInsights.DataContracts.PageViewTelemetry"/> をこのメソッドの呼び出しごとに個別に送信します。
        /// </summary>
        /// <remarks>
        /// [en]To more than two metrics or send more than four string properties, use <see cref="M:NishySoftware.Telemetry.ITelemetry.TrackPageView(System.String,System.TimeSpan,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})"/> instead of this method.
        /// <br/>
        /// [ja] 2個以上のメトリック、または、4個以上の文字列プロパティを送信する場合は、このメソッドの代わりに <see cref="M:NishySoftware.Telemetry.ITelemetry.TrackPageView(System.String,System.TimeSpan,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})"/> を使用します。
        /// </remarks>
        /// <param name="pageName">[en] A name for the page. <br/> [ja]ページの名前。</param>
        /// <param name="duration">[en] Page view duration. <br/> [ja] ページ閲覧時間。</param>
        /// <param name="metric1key">[en] Key name of measurement value #1. <br/> [ja] 計測値1のキー名。</param>
        /// <param name="metric1value">[en] Measurement value #1. <br/> [ja] 計測値1。</param>
        /// <param name="prop1key">[en] Key name of string property value #1. <br/> [ja] 文字列プロパティ値1のキー名。</param>
        /// <param name="prop1value">[en] String property value #1. <br/> [ja] 文字列プロパティ値1。</param>
        /// <param name="prop2key">[en] Key name of string property value #2. <br/> [ja] 文字列プロパティ値2のキー名。</param>
        /// <param name="prop2value">[en] String property value #2. <br/> [ja] 文字列プロパティ値2。</param>
        /// <param name="prop3key">[en] Key name of string property value #3. <br/> [ja] 文字列プロパティ値3のキー名。</param>
        /// <param name="prop3value">[en] String property value #3. <br/> [ja] 文字列プロパティ値3。</param>
        void TrackPageView(string pageName, TimeSpan duration, string metric1key, double metric1value, string prop1key = null, string prop1value = null, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null);

        /// <summary>
        /// [en] Send information about the exception event in the application.
        /// For ApplicationInsight, send a separate <see cref="T:Microsoft.ApplicationInsights.DataContracts.ExceptionTelemetry"/> for display in Diagnostic Search for each call to this method.
        /// <br/>
        /// [ja] アプリのExceptionイベントについての情報を送信する。
        /// ApplicationInsightにおいては、診断検索で表示するための <see cref="T:Microsoft.ApplicationInsights.DataContracts.ExceptionTelemetry"/> をこのメソッドの呼び出しごとに個別に送信します。
        /// </summary>
        /// <remarks>
        /// [en]To send more than four string properties, use <see cref="M:NishySoftware.Telemetry.ITelemetry.TrackException(System.Exception,Microsoft.ApplicationInsights.DataContracts.SeverityLevel,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})"/> instead of this method.
        /// <br/>
        /// [ja] 4個以上の文字列プロパティを送信する場合は、このメソッドの代わりに <see cref="M:NishySoftware.Telemetry.ITelemetry.TrackException(System.Exception,Microsoft.ApplicationInsights.DataContracts.SeverityLevel,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IDictionary{System.String,System.Double})"/> を使用します。
        /// </remarks>
        /// <param name="exception">[en] The exception to log. <br/> [ja] 記録するException</param>
        /// <param name="level">[en] Severity level. <br/> [ja] 深刻度</param>
        /// <param name="prop1key">[en] Key name of string property value #1. <br/> [ja] 文字列プロパティ値1のキー名。</param>
        /// <param name="prop1value">[en] String property value #1. <br/> [ja] 文字列プロパティ値1。</param>
        /// <param name="prop2key">[en] Key name of string property value #2. <br/> [ja] 文字列プロパティ値2のキー名。</param>
        /// <param name="prop2value">[en] String property value #2. <br/> [ja] 文字列プロパティ値2。</param>
        /// <param name="prop3key">[en] Key name of string property value #3. <br/> [ja] 文字列プロパティ値3のキー名。</param>
        /// <param name="prop3value">[en] String property value #3. <br/> [ja] 文字列プロパティ値3。</param>
        void TrackException(Exception exception, SeverityLevel level, string prop1key, string prop1value, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null);

        /// <summary>
        /// [en] Send information about the exception event in the application.
        /// For ApplicationInsight, send a separate <see cref="T:Microsoft.ApplicationInsights.DataContracts.ExceptionTelemetry"/> for display in Diagnostic Search for each call to this method.
        /// <br/>
        /// [ja] アプリのExceptionイベントについての情報を送信する。
        /// ApplicationInsightにおいては、診断検索で表示するための <see cref="T:Microsoft.ApplicationInsights.DataContracts.ExceptionTelemetry"/> をこのメソッドの呼び出しごとに個別に送信します。
        /// </summary>
        /// <param name="exception">[en] The exception to log. <br/> [ja] 記録するException</param>
        /// <param name="level">[en] Severity level. <br/> [ja] 深刻度</param>
        /// <param name="properties">[en] Named string values you can use to search and classify events. <br/> [ja] イベントの検索や分類に使用できる名前付きの文字列値。</param>
        /// <param name="metrics">[en] Named measurements associated with this event. <br/> [ja] このイベントに関連する名前付きの測定値。</param>
        void TrackException(Exception exception, SeverityLevel level, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

        /// <summary>
        /// [en] Flushes the unsent telemetry data in the in-memory buffer.
        /// This method is an asynchronous function, so if you want to ensure data protection (sending or saving files) when the application is closed,
        /// wait a little while after the call before closing the application.
        /// <br/>
        /// [ja] メモリ内バッファにある未送信のTelemetryデータを送信をフラッシュする
        /// このメソッドは非同期関数なので、アプリの終了時に確実にデータ保護(送信またはファイル保存)をしたい場合は、呼び出し後、少し待ってから、アプリを終了すること。
        /// </summary>
        void Flush();
    }

    [ContractClassFor(typeof(ITelemetry))]
    abstract class ITelemetryContract : ITelemetry
    {
        /// <inheritdoc/>
        public object GlobalSyncObject
        {
            get
            {
                Contract.Ensures(Contract.Result<object>() != null);
                return default(object);
            }
        }

        /// <inheritdoc/>
        public IDictionary<string, string> GlobalProperties
        {
            get
            {
                Contract.Ensures(Contract.Result<IDictionary<string, string>>() != null);
                return default(IDictionary<string, string>);
            }
        }

        /// <inheritdoc/>
        public IDictionary<string, double> GlobalMetrics
        {
            get
            {
                Contract.Ensures(Contract.Result<IDictionary<string, double>>() != null);
                return default(IDictionary<string, double>);
            }
        }

        /// <inheritdoc/>
        public IDictionary<string, string> GlobalExceptionProperties
        {
            get
            {
                Contract.Ensures(Contract.Result<IDictionary<string, string>>() != null);
                return default(IDictionary<string, string>);
            }
        }

        /// <inheritdoc/>
        public IDictionary<string, double> GlobalExceptionMetrics
        {
            get
            {
                Contract.Ensures(Contract.Result<IDictionary<string, double>>() != null);
                return default(IDictionary<string, double>);
            }
        }

        /// <inheritdoc/>
        public IDictionary<string, string> Properties
        {
            get
            {
                Contract.Ensures(Contract.Result<IDictionary<string, string>>() != null);
                return default(IDictionary<string, string>);
            }
        }

        /// <inheritdoc/>
        public IDictionary<string, double> Metrics
        {
            get
            {
                Contract.Ensures(Contract.Result<IDictionary<string, double>>() != null);
                return default(IDictionary<string, double>);
            }
        }

        /// <inheritdoc/>
        public void TrackEvent(string eventName, string prop1key, string prop1value, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(eventName));
            Contract.Requires(prop1key == null || prop1key != "");
            Contract.Requires(string.IsNullOrEmpty(prop1key) || prop1value != null);
            Contract.Requires(prop2key == null || prop2key != "");
            Contract.Requires(string.IsNullOrEmpty(prop2key) || prop2value != null);
            Contract.Requires(prop3key == null || prop3key != "");
            Contract.Requires(string.IsNullOrEmpty(prop3key) || prop3value != null);
        }

        /// <inheritdoc/>
        public void TrackEvent(string eventName, string metric1key, double metric1value, string prop1key = null, string prop1value = null, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(eventName));
            Contract.Requires(!string.IsNullOrEmpty(metric1key));
            Contract.Requires(prop1key == null || prop1key != "");
            Contract.Requires(string.IsNullOrEmpty(prop1key) || prop1value != null);
            Contract.Requires(prop2key == null || prop2key != "");
            Contract.Requires(string.IsNullOrEmpty(prop2key) || prop2value != null);
            Contract.Requires(prop3key == null || prop3key != "");
            Contract.Requires(string.IsNullOrEmpty(prop3key) || prop3value != null);
        }

        /// <inheritdoc/>
        public void TrackEvent(string eventName, TriggerType triggerType, string prop1key, string prop1value, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(eventName));
            Contract.Requires(prop1key == null || prop1key != "");
            Contract.Requires(string.IsNullOrEmpty(prop1key) || prop1value != null);
            Contract.Requires(prop2key == null || prop2key != "");
            Contract.Requires(string.IsNullOrEmpty(prop2key) || prop2value != null);
            Contract.Requires(prop3key == null || prop3key != "");
            Contract.Requires(string.IsNullOrEmpty(prop3key) || prop3value != null);
        }

        /// <inheritdoc/>
        public void TrackEvent(string eventName, TriggerType triggerType, string metric1key, double metric1value, string prop1key = null, string prop1value = null, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(eventName));
            Contract.Requires(!string.IsNullOrEmpty(metric1key));
            Contract.Requires(prop1key == null || prop1key != "");
            Contract.Requires(string.IsNullOrEmpty(prop1key) || prop1value != null);
            Contract.Requires(prop2key == null || prop2key != "");
            Contract.Requires(string.IsNullOrEmpty(prop2key) || prop2value != null);
            Contract.Requires(prop3key == null || prop3key != "");
            Contract.Requires(string.IsNullOrEmpty(prop3key) || prop3value != null);
        }

        /// <inheritdoc/>
        public void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(eventName));
        }

        /// <inheritdoc/>
        public void TrackEvent(string eventName, TriggerType triggerType, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(eventName));
        }

        /// <inheritdoc/>
        public void TrackPageView(string pageName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(pageName));
        }

        /// <inheritdoc/>
        public void TrackPageView(string pageName, TimeSpan duration, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(pageName));
        }

        /// <inheritdoc/>
        public void TrackPageView(string pageName, TimeSpan duration, string metric1key, double metric1value, string prop1key = null, string prop1value = null, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(pageName));
            Contract.Requires(!string.IsNullOrEmpty(metric1key));
            Contract.Requires(prop1key == null || prop1key != "");
            Contract.Requires(string.IsNullOrEmpty(prop1key) || prop1value != null);
            Contract.Requires(prop2key == null || prop2key != "");
            Contract.Requires(string.IsNullOrEmpty(prop2key) || prop2value != null);
            Contract.Requires(prop3key == null || prop3key != "");
            Contract.Requires(string.IsNullOrEmpty(prop3key) || prop3value != null);
        }

        /// <inheritdoc/>
        public void TrackException(Exception exception, SeverityLevel level, string prop1key, string prop1value, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null)
        {
            Contract.Requires(exception != null);
            Contract.Requires(prop1key == null || prop1key != "");
            Contract.Requires(string.IsNullOrEmpty(prop1key) || prop1value != null);
            Contract.Requires(prop2key == null || prop2key != "");
            Contract.Requires(string.IsNullOrEmpty(prop2key) || prop2value != null);
            Contract.Requires(prop3key == null || prop3key != "");
            Contract.Requires(string.IsNullOrEmpty(prop3key) || prop3value != null);
        }

        /// <inheritdoc/>
        public void TrackException(Exception exception, SeverityLevel level, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            Contract.Requires(exception != null);
        }

        /// <inheritdoc/>
        public void Flush()
        {
        }
    }
}
