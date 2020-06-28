/* ==============================
** Copyright 2015 nishy software
**
**      First Author : nishy software
**		Create : 2015/12/07
** ============================== */

namespace NishySoftware.Telemetry
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// TriggerType
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
    /// Telemetry用のインターフェース
    /// </summary>
    [ContractClass(typeof(ITelemetryContract))]
    public interface ITelemetry
    {
        /// <summary>
        /// GlobalProperties/GlobalMetricsを操作・参照するときに使用するLockObject
        /// GlobalProperties/GlobalMetricsは、Globalで共有されるため、どのスレッドで使用されているかわかりません。
        /// そのため、このオブジェクトを使ってLockしてから、操作・参照します。
        /// </summary>
        object GlobalSyncObject { get; }

        /// <summary>
        /// デフォルトで送信するプロパティ。
        /// このプロパティはGlobalで共有されるため、このプロパティを操作するときは、GlobalSyncObjectでLockする必要があります。
        /// Interfaceのインスタンスにかかわらず、すべてのTrackXXXXでこのプロパティは送信されます。
        /// InterfaceのインスタンスやTrackXXX()でプロパティが指定された場合は、同じ名前の値は引数、Interfaceの順で指定されたほうが優先されます
        /// </summary>
        IDictionary<string, string> GlobalProperties { get; }

        /// <summary>
        /// デフォルトで送信するメトリック。
        /// このプロパティはGlobalで共有されるため、このプロパティを操作するときは、GlobalSyncObjectでLockする必要があります。
        /// このInterfaceのインスタンスから発行する、すべてのTrackXXXXでこのメトリックは送信されます。
        /// InterfaceのインスタンスやTrackXXX()でメトリックが指定された場合は、同じ名前の値は引数、Interfaceの順で指定されたほうが優先されます
        /// </summary>
        IDictionary<string, double> GlobalMetrics { get; }

        /// <summary>
        /// Exceptionのときにデフォルトで送信するプロパティ。
        /// このプロパティはGlobalで共有されるため、このプロパティを操作するときは、GlobalSyncObjectでLockする必要があります。
        /// Interfaceのインスタンスにかかわらず、すべてのTrackExceptionでこのプロパティは送信されます。
        /// InterfaceのインスタンスやTrackException()でプロパティが指定された場合は、同じ名前の値は引数、Interfaceの順で指定されたほうが優先されます
        /// </summary>
        IDictionary<string, string> GlobalExceptionProperties { get; }

        /// <summary>
        /// Exceptionのときにデフォルトで送信するメトリック。
        /// このプロパティはGlobalで共有されるため、このプロパティを操作するときは、GlobalSyncObjectでLockする必要があります。
        /// このInterfaceのインスタンスから発行する、すべてのTrackExceptionでこのメトリックは送信されます。
        /// InterfaceのインスタンスやTrackException()でメトリックが指定された場合は、同じ名前の値は引数、Interfaceの順で指定されたほうが優先されます
        /// </summary>
        IDictionary<string, double> GlobalExceptionMetrics { get; }

        /// <summary>
        /// デフォルトで送信するプロパティ。
        /// このInterfaceのインスタンスから発行する、すべてのTrackXXXXでこのプロパティは送信されます。
        /// TrackXXX()でプロパティが指定された場合は、同じ名前の値は引数で指定されたほうが優先されます
        /// </summary>
        IDictionary<string, string> Properties { get; }

        /// <summary>
        /// デフォルトで送信するメトリック。
        /// このInterfaceのインスタンスから発行する、すべてのTrackXXXXでこのメトリックは送信されます。
        /// TrackXXX()でメトリックが指定された場合は、同じ名前の値は引数で指定されたほうが優先されます
        /// </summary>
        IDictionary<string, double> Metrics { get; }

        /// <summary>
        /// CustomEventを送信する
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="prop1key"></param>
        /// <param name="prop1value"></param>
        /// <param name="prop2key"></param>
        /// <param name="prop2value"></param>
        /// <param name="prop3key"></param>
        /// <param name="prop3value"></param>
        void TrackEvent(string eventName, string prop1key, string prop1value, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null);

        /// <summary>
        /// CustomEventを送信する
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="metric1key"></param>
        /// <param name="metric1value"></param>
        /// <param name="prop1key"></param>
        /// <param name="prop1value"></param>
        /// <param name="prop2key"></param>
        /// <param name="prop2value"></param>
        /// <param name="prop3key"></param>
        /// <param name="prop3value"></param>
        void TrackEvent(string eventName, string metric1key, double metric1value, string prop1key = null, string prop1value = null, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null);

        /// <summary>
        /// CustomEventを送信する
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="triggerType"></param>
        /// <param name="prop1key"></param>
        /// <param name="prop1value"></param>
        /// <param name="prop2key"></param>
        /// <param name="prop2value"></param>
        /// <param name="prop3key"></param>
        /// <param name="prop3value"></param>
        void TrackEvent(string eventName, TriggerType triggerType, string prop1key, string prop1value, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null);

        /// <summary>
        /// CustomEventを送信する
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="triggerType"></param>
        /// <param name="metric1key"></param>
        /// <param name="metric1value"></param>
        /// <param name="prop1key"></param>
        /// <param name="prop1value"></param>
        /// <param name="prop2key"></param>
        /// <param name="prop2value"></param>
        /// <param name="prop3key"></param>
        /// <param name="prop3value"></param>
        void TrackEvent(string eventName, TriggerType triggerType, string metric1key, double metric1value, string prop1key = null, string prop1value = null, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null);

        /// <summary>
        /// CustomEventを送信する
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="properties"></param>
        /// <param name="metrics"></param>
        void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

        /// <summary>
        /// CustomEventを送信する
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="triggerType"></param>
        /// <param name="properties"></param>
        /// <param name="metrics"></param>
        void TrackEvent(string eventName, TriggerType triggerType, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

        /// <summary>
        /// PageViewを送信する
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="properties"></param>
        /// <param name="metrics"></param>
        void TrackPageView(string pageName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

        /// <summary>
        /// PageViewを送信する
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="duration"></param>
        /// <param name="properties"></param>
        /// <param name="metrics"></param>
        void TrackPageView(string pageName, TimeSpan duration, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);


        /// <summary>
        /// PageViewを送信する
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="duration"></param>
        /// <param name="metric1key"></param>
        /// <param name="metric1value"></param>
        /// <param name="prop1key"></param>
        /// <param name="prop1value"></param>
        /// <param name="prop2key"></param>
        /// <param name="prop2value"></param>
        /// <param name="prop3key"></param>
        /// <param name="prop3value"></param>
        void TrackPageView(string pageName, TimeSpan duration, string metric1key, double metric1value, string prop1key = null, string prop1value = null, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null);

        /// <summary>
        /// Exceptionを送信する
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="level"></param>
        /// <param name="prop1key"></param>
        /// <param name="prop1value"></param>
        /// <param name="prop2key"></param>
        /// <param name="prop2value"></param>
        /// <param name="prop3key"></param>
        /// <param name="prop3value"></param>
        void TrackException(Exception exception, Microsoft.ApplicationInsights.DataContracts.SeverityLevel level, string prop1key, string prop1value, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null);

        /// <summary>
        /// Exceptionを送信する
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="level"></param>
        /// <param name="properties"></param>
        /// <param name="metrics"></param>
        void TrackException(Exception exception, Microsoft.ApplicationInsights.DataContracts.SeverityLevel level, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

        /// <summary>
        /// 未送信のTelemetryデータを送信する
        /// 非同期関数なので、アプリの終了時に確実にデータ保護(送信またはファイル保存)をしたい場合は、呼び出し後、少し待ってから、アプリを終了すること。
        /// </summary>
        void Flush();
    }

    [ContractClassFor(typeof(ITelemetry))]
    abstract class ITelemetryContract : ITelemetry
    {
        public object GlobalSyncObject
        {
            get
            {
                Contract.Ensures(Contract.Result<object>() != null);
                return default(object);
            }
        }

        public IDictionary<string, string> GlobalProperties
        {
            get
            {
                Contract.Ensures(Contract.Result<IDictionary<string, string>>() != null);
                return default(IDictionary<string, string>);
            }
        }

        public IDictionary<string, double> GlobalMetrics
        {
            get
            {
                Contract.Ensures(Contract.Result<IDictionary<string, double>>() != null);
                return default(IDictionary<string, double>);
            }
        }

        public IDictionary<string, string> GlobalExceptionProperties
        {
            get
            {
                Contract.Ensures(Contract.Result<IDictionary<string, string>>() != null);
                return default(IDictionary<string, string>);
            }
        }

        public IDictionary<string, double> GlobalExceptionMetrics
        {
            get
            {
                Contract.Ensures(Contract.Result<IDictionary<string, double>>() != null);
                return default(IDictionary<string, double>);
            }
        }

        public IDictionary<string, string> Properties
        {
            get
            {
                Contract.Ensures(Contract.Result<IDictionary<string, string>>() != null);
                return default(IDictionary<string, string>);
            }
        }

        public IDictionary<string, double> Metrics
        {
            get
            {
                Contract.Ensures(Contract.Result<IDictionary<string, double>>() != null);
                return default(IDictionary<string, double>);
            }
        }

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

        public void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(eventName));
        }

        public void TrackEvent(string eventName, TriggerType triggerType, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(eventName));
        }

        public void TrackPageView(string pageName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(pageName));
        }

        public void TrackPageView(string pageName, TimeSpan duration, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(pageName));
        }

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

        public void TrackException(Exception exception, Microsoft.ApplicationInsights.DataContracts.SeverityLevel level, string prop1key, string prop1value, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null)
        {
            Contract.Requires(exception != null);
            Contract.Requires(prop1key == null || prop1key != "");
            Contract.Requires(string.IsNullOrEmpty(prop1key) || prop1value != null);
            Contract.Requires(prop2key == null || prop2key != "");
            Contract.Requires(string.IsNullOrEmpty(prop2key) || prop2value != null);
            Contract.Requires(prop3key == null || prop3key != "");
            Contract.Requires(string.IsNullOrEmpty(prop3key) || prop3value != null);
        }

        public void TrackException(Exception exception, Microsoft.ApplicationInsights.DataContracts.SeverityLevel level, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            Contract.Requires(exception != null);
        }

        public void Flush()
        {
        }
    }
}
