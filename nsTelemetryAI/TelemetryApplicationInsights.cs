/* ==============================
** Copyright 2015, 2018 nishy software
**
**      First Author : nishy software
**		Create : 2015/12/07
** ============================== */

namespace NishySoftware.Telemetry.ApplicationInsights
{
#if !NO_APP_INSIGHTS
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
#endif
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Management;
    using System.Net.NetworkInformation;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Security.Principal;
    using System.Reflection;

    internal class SdkVersionUtils
    {
        internal static string GetAssemblyVersion()
        {
            return typeof(Microsoft.ApplicationInsights.Channel.ITelemetry).Assembly.GetCustomAttributes(false).OfType<AssemblyFileVersionAttribute>().First<AssemblyFileVersionAttribute>().Version;
        }
    }

    class TelemetryFactoryApplicationInsights
    {
        #region Constructors / Destructor
        static TelemetryFactoryApplicationInsights()
        {
            var config = TelemetryConfiguration.Active;
#if DEBUG
            config.TelemetryChannel.DeveloperMode = true;
#else
            config.TelemetryChannel.DeveloperMode = System.Diagnostics.Debugger.IsAttached;
#endif
            if (config.TelemetryChannel.DeveloperMode.HasValue
                && config.TelemetryChannel.DeveloperMode.Value == true)
            {
                var devKey = GetDevKeyFromConfigurationXml();
                if (!string.IsNullOrWhiteSpace(devKey))
                {
                    config.InstrumentationKey = devKey;
                }
            }

            config.TelemetryInitializers.Add(new DeviceTelemetryInitializer());
            config.TelemetryInitializers.Add(new UserTelemetryInitializer());
            config.TelemetryInitializers.Add(new SessionTelemetryInitializer());
            config.TelemetryInitializers.Add(new ComponentTelemetryInitializer());
        }
        #endregion Constructors / Destructor

        #region Fields
        static readonly object _defaultGlobalSyncObj = new object();
        static readonly Dictionary<string, string> _defaultGlobalProperties = new Dictionary<string, string>();
        static readonly Dictionary<string, double> _defaultGlobalMetrics = new Dictionary<string, double>();
        static readonly IDictionary<string, string> _defaultGlobalExceptionProperties = new Dictionary<string, string>();
        static readonly IDictionary<string, double> _defaultGlobalExceptionMetrics = new Dictionary<string, double>();
        #endregion Fields

        #region Properties

        #region TelemetryDataFlags
        static Telemetry.TelemetryDataFlag _telemetryDataFlags = 0;
        public static Telemetry.TelemetryDataFlag TelemetryDataFlags
        {
            get
            {
                return _telemetryDataFlags;
            }
            set
            {
                if (_telemetryDataFlags != value)
                {
                    var changed = _telemetryDataFlags ^ value;
                    _telemetryDataFlags = value;

                    if (_telemetryDataFlags.HasFlag(Telemetry.TelemetryDataFlag.NetworkType)
                        || _telemetryDataFlags.HasFlag(Telemetry.TelemetryDataFlag.NetworkSpeed))
                    {
                        // 監視が必要
                        System.Net.NetworkInformation.NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
                        System.Net.NetworkInformation.NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
                    }
                    else
                    {
                        // 監視が不要
                        System.Net.NetworkInformation.NetworkChange.NetworkAvailabilityChanged -= NetworkChange_NetworkAvailabilityChanged;
                        System.Net.NetworkInformation.NetworkChange.NetworkAddressChanged -= NetworkChange_NetworkAddressChanged;
                    }
                    if (changed.HasFlag(Telemetry.TelemetryDataFlag.NetworkType)
                        || changed.HasFlag(Telemetry.TelemetryDataFlag.NetworkSpeed))
                    {
                        UpdateNetworkType();
                    }
                    if (changed.HasFlag(Telemetry.TelemetryDataFlag.DeviceName))
                    {
                        if (_telemetryDataFlags.HasFlag(Telemetry.TelemetryDataFlag.DeviceName))
                        {
                            DeviceContextReader instance = DeviceContextReader.Instance;
                            var oemName = instance.GetOemName();
                            var model = DeviceTelemetryInitializer.AdjustDeviceModel(instance.GetDeviceModel(), oemName);

                            _defaultGlobalProperties["DeviceName"] = model;
                        }
                        else
                        {
                            _defaultGlobalProperties.Remove("DeviceName");
                        }
                    }
                    if (changed.HasFlag(Telemetry.TelemetryDataFlag.DeviceManufacturer))
                    {
                        if (_telemetryDataFlags.HasFlag(Telemetry.TelemetryDataFlag.DeviceManufacturer))
                        {
                            DeviceContextReader instance = DeviceContextReader.Instance;
                            var oemName = instance.GetOemName();
                            _defaultGlobalProperties["DeviceManufacturer"] = oemName;
                        }
                        else
                        {
                            _defaultGlobalProperties.Remove("DeviceManufacturer");
                        }
                    }
                }
            }
        }
        #endregion

        #endregion Properties

        #region public methods

        public static NishySoftware.Telemetry.ITelemetry Create()
        {
            return new TelemetryApplicationInsights(_defaultGlobalSyncObj,
                _defaultGlobalProperties, _defaultGlobalMetrics,
                _defaultGlobalExceptionProperties, _defaultGlobalExceptionMetrics);
        }

        public static void GetGlobalParameters(ref IDictionary<string, string> properties, ref IDictionary<string, double> metrics)
        {
            lock (_defaultGlobalSyncObj)
            {
                foreach (var item in _defaultGlobalProperties)
                {
                    properties[item.Key] = item.Value;
                }
                foreach (var item in _defaultGlobalMetrics)
                {
                    metrics[item.Key] = item.Value;
                }
            }
        }

        public static void GetGlobalExceptionParameters(ref IDictionary<string, string> properties, ref IDictionary<string, double> metrics)
        {
            lock (_defaultGlobalSyncObj)
            {
                foreach (var item in _defaultGlobalExceptionProperties)
                {
                    properties[item.Key] = item.Value;
                }
                foreach (var item in _defaultGlobalExceptionMetrics)
                {
                    metrics[item.Key] = item.Value;
                }
            }
        }

        #endregion public methods

        #region event handlers
        static void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            UpdateNetworkType();
        }

        static void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            UpdateNetworkType();
        }
        #endregion event handlers

        #region private methods
        static void UpdateNetworkType()
        {
            long networkSpeed = 0;
            DeviceContextReader instance = DeviceContextReader.Instance;
            string networkType = null;
            if (_telemetryDataFlags.HasFlag(Telemetry.TelemetryDataFlag.NetworkType)
                || _telemetryDataFlags.HasFlag(Telemetry.TelemetryDataFlag.NetworkSpeed))
            {
                networkType = instance.GetNetworkType(ref networkSpeed, true);
            }
            lock (_defaultGlobalSyncObj)
            {
                if (_telemetryDataFlags.HasFlag(Telemetry.TelemetryDataFlag.NetworkType))
                {
                    _defaultGlobalProperties["NetworkType"] = networkType;
                }
                else
                {
                    _defaultGlobalProperties.Remove("NetworkType");
                }
                if (_telemetryDataFlags.HasFlag(Telemetry.TelemetryDataFlag.NetworkSpeed))
                {
                    _defaultGlobalMetrics["NetworkSpeed"] = networkSpeed;
                }
                else
                {
                    _defaultGlobalMetrics.Remove("NetworkSpeed");
                }
            }
        }

        static string ReadConfigurationXml()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApplicationInsights.config");
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return string.Empty;
        }

        static string GetDevKeyFromConfigurationXml()
        {
            var configXml = ReadConfigurationXml();
            if (!string.IsNullOrWhiteSpace(configXml))
            {
                var startKey = "<InstrumentationDevKey>";
                var start = configXml.IndexOf(startKey);
                var end = configXml.IndexOf("</InstrumentationDevKey>");
                if (start >= 0 && end >= 0)
                {
                    start += startKey.Length;
                    var content = configXml.Substring(start, end - start);
                    return content;
                }
            }
            return string.Empty;
        }

        #endregion private methods
    }

    sealed class TelemetryApplicationInsights : NishySoftware.Telemetry.ITelemetry
    {
        #region Constructors / Destructor
        public TelemetryApplicationInsights(object defaultGlobalSyncObj, IDictionary<string, string> defaultGlobalProperties, IDictionary<string, double> defaultGlobalMetrics, IDictionary<string, string> defaultGlobalExceptionProperties, IDictionary<string, double> defaultGlobalExceptionMetrics, TelemetryConfiguration configuration = null)
        {
            this._defaultGlobalSyncObj = defaultGlobalSyncObj;
            this._defaultGlobalProperties = defaultGlobalProperties;
            this._defaultGlobalMetrics = defaultGlobalMetrics;
            this._defaultGlobalExceptionProperties = defaultGlobalExceptionProperties;
            this._defaultGlobalExceptionMetrics = defaultGlobalExceptionMetrics;
            this._configuration = configuration ?? TelemetryConfiguration.Active;
        }
        #endregion Constructors / Destructor

        #region Fields
        readonly TelemetryConfiguration _configuration;
        readonly object _defaultGlobalSyncObj;
        readonly IDictionary<string, string> _defaultGlobalProperties;
        readonly IDictionary<string, double> _defaultGlobalMetrics;
        readonly IDictionary<string, string> _defaultGlobalExceptionProperties;
        readonly IDictionary<string, double> _defaultGlobalExceptionMetrics;
        readonly Dictionary<string, string> _defaultProperties = new Dictionary<string, string>();
        readonly Dictionary<string, double> _defaultMetrics = new Dictionary<string, double>();
        TelemetryClient _telemetryClient;
        #endregion

        #region ITelemetry interface

        public object GlobalSyncObject { get { return this._defaultGlobalSyncObj; } }

        public IDictionary<string, string> GlobalProperties { get { return this._defaultGlobalProperties; } }

        public IDictionary<string, double> GlobalMetrics { get { return this._defaultGlobalMetrics; } }

        public IDictionary<string, string> GlobalExceptionProperties { get { return this._defaultGlobalExceptionProperties; } }

        public IDictionary<string, double> GlobalExceptionMetrics { get { return this._defaultGlobalExceptionMetrics; } }

        public IDictionary<string, string> Properties { get { return this._defaultProperties; } }

        public IDictionary<string, double> Metrics { get { return this._defaultMetrics; } }

        public void TrackEvent(string eventName, string prop1key, string prop1value, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null)
        {
            this.TrackEvent(eventName, TriggerType.None, prop1key, prop1value, prop2key, prop2value, prop3key, prop3value);
        }

        public void TrackEvent(string eventName, string metric1key, double metric1value, string prop1key = null, string prop1value = null, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null)
        {
            this.TrackEvent(eventName, TriggerType.None, metric1key, metric1value, prop1key, prop1value, prop2key, prop2value, prop3key, prop3value);
        }

        public void TrackEvent(string eventName, TriggerType triggerType, string prop1key, string prop1value, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null)
        {
            var properties = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(prop1key))
            {
                properties.Add(prop1key, prop1value);
            }
            if (!string.IsNullOrEmpty(prop2key))
            {
                properties.Add(prop2key, prop2value);
            }
            if (!string.IsNullOrEmpty(prop3key))
            {
                properties.Add(prop3key, prop3value);
            }
            EventTelemetry telemetry = new EventTelemetry(eventName);
            this.TrackEvent(telemetry, triggerType, properties);
        }

        public void TrackEvent(string eventName, TriggerType triggerType, string metric1key, double metric1value, string prop1key = null, string prop1value = null, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null)
        {
            var properties = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(prop1key))
            {
                properties.Add(prop1key, prop1value);
            }
            if (!string.IsNullOrEmpty(prop2key))
            {
                properties.Add(prop2key, prop2value);
            }
            if (!string.IsNullOrEmpty(prop3key))
            {
                properties.Add(prop3key, prop3value);
            }
            var metrics = new Dictionary<string, double>();
            if (!string.IsNullOrEmpty(metric1key))
            {
                metrics.Add(metric1key, metric1value);
            }

            EventTelemetry telemetry = new EventTelemetry(eventName);
            this.TrackEvent(telemetry, triggerType, properties, metrics);
        }

        public void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            this.TrackEvent(eventName, TriggerType.None, properties, metrics);
        }

        public void TrackEvent(string eventName, TriggerType triggerType, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            EventTelemetry telemetry = new EventTelemetry(eventName);
            this.TrackEvent(telemetry, triggerType, properties, metrics);
        }

        public void TrackPageView(string pageName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            PageViewTelemetry telemetry = new PageViewTelemetry(pageName);
            this.TrackPageView(telemetry, properties, metrics);
        }

        public void TrackPageView(string pageName, TimeSpan duration, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            PageViewTelemetry telemetry = new PageViewTelemetry(pageName)
            {
                Duration = duration
            };
            this.TrackPageView(telemetry, properties, metrics);
        }

        public void TrackPageView(string pageName, TimeSpan duration, string metric1key, double metric1value, string prop1key = null, string prop1value = null, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null)
        {
            var properties = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(prop1key))
            {
                properties.Add(prop1key, prop1value);
            }
            if (!string.IsNullOrEmpty(prop2key))
            {
                properties.Add(prop2key, prop2value);
            }
            if (!string.IsNullOrEmpty(prop3key))
            {
                properties.Add(prop3key, prop3value);
            }
            var metrics = new Dictionary<string, double>();
            if (!string.IsNullOrEmpty(metric1key))
            {
                metrics.Add(metric1key, metric1value);
            }
            PageViewTelemetry telemetry = new PageViewTelemetry(pageName)
            {
                Duration = duration
            };
            this.TrackPageView(telemetry, properties, metrics);
        }

        public void TrackException(Exception exception, Microsoft.ApplicationInsights.DataContracts.SeverityLevel level, string prop1key, string prop1value, string prop2key = null, string prop2value = null, string prop3key = null, string prop3value = null)
        {
            var properties = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(prop1key))
            {
                properties.Add(prop1key, prop1value);
            }
            if (!string.IsNullOrEmpty(prop2key))
            {
                properties.Add(prop2key, prop2value);
            }
            if (!string.IsNullOrEmpty(prop3key))
            {
                properties.Add(prop3key, prop3value);
            }
            this.TrackException(exception, level, properties);
        }

        public void TrackException(Exception exception, Microsoft.ApplicationInsights.DataContracts.SeverityLevel level, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            var exp = new ExceptionTelemetry(exception)
            {
                HandledAt = ExceptionHandledAt.UserCode,
                SeverityLevel = level,
            };

            this.TrackException(exp, properties, metrics);
        }

        public void Flush()
        {
            TelemetryClient telemetryClient = LazyInitializer.EnsureInitialized<TelemetryClient>(ref this._telemetryClient, () => new TelemetryClient(this._configuration));
            try
            {
                telemetryClient.Flush();
            }
            catch (ObjectDisposedException)
            {
                Interlocked.CompareExchange<TelemetryClient>(ref this._telemetryClient, null, telemetryClient);
            }
        }

        #endregion ITelemetry interface

        void TrackEvent(EventTelemetry telemetry, TriggerType triggerType, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            TelemetryClient telemetryClient = LazyInitializer.EnsureInitialized<TelemetryClient>(ref this._telemetryClient, () => new TelemetryClient(this._configuration));
            try
            {
                lock (this._defaultGlobalSyncObj)
                {
                    UpdateTelemetryData(telemetry.Properties, telemetry.Metrics, this._defaultGlobalProperties, this._defaultGlobalMetrics);
                }
                UpdateTelemetryData(telemetry.Properties, telemetry.Metrics, this._defaultProperties, this._defaultMetrics);
                UpdateTelemetryData(telemetry.Properties, telemetry.Metrics, properties, metrics);
                if (triggerType != TriggerType.None)
                {
                    telemetry.Properties["TriggerType"] = triggerType.ToString();
                }
                telemetryClient.TrackEvent(telemetry);
            }
            catch (ObjectDisposedException)
            {
                Interlocked.CompareExchange<TelemetryClient>(ref this._telemetryClient, null, telemetryClient);
            }
        }

        void TrackPageView(PageViewTelemetry telemetry, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            TelemetryClient telemetryClient = LazyInitializer.EnsureInitialized<TelemetryClient>(ref this._telemetryClient, () => new TelemetryClient(this._configuration));
            try
            {
                if (telemetry.Name.IndexOf(':') < 0)
                {
                    telemetry.Name = "application:" + telemetry.Name;
                }
                lock (this._defaultGlobalSyncObj)
                {
                    UpdateTelemetryData(telemetry.Properties, telemetry.Metrics, this._defaultGlobalProperties, this._defaultGlobalMetrics);
                }
                UpdateTelemetryData(telemetry.Properties, telemetry.Metrics, this._defaultProperties, this._defaultMetrics);
                UpdateTelemetryData(telemetry.Properties, telemetry.Metrics, properties, metrics);
                telemetryClient.TrackPageView(telemetry);
            }
            catch (ObjectDisposedException)
            {
                Interlocked.CompareExchange<TelemetryClient>(ref this._telemetryClient, null, telemetryClient);
            }
        }

        void TrackException(ExceptionTelemetry telemetry, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            TelemetryClient telemetryClient = LazyInitializer.EnsureInitialized<TelemetryClient>(ref this._telemetryClient, () => new TelemetryClient(this._configuration));
            try
            {
                try
                {
                    lock (this._defaultGlobalSyncObj)
                    {
                        UpdateTelemetryData(telemetry.Properties, telemetry.Metrics, this._defaultGlobalProperties, this._defaultGlobalMetrics);
                        UpdateTelemetryData(telemetry.Properties, telemetry.Metrics, this._defaultGlobalExceptionProperties, this._defaultGlobalExceptionMetrics);
                    }
                    UpdateTelemetryData(telemetry.Properties, telemetry.Metrics, this._defaultProperties, this._defaultMetrics);
                } catch { }
                try
                {
                    var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                    telemetry.Metrics["HandleCount"] = currentProcess.HandleCount;
                    telemetry.Metrics["ElapaseTime"] = (DateTime.Now - currentProcess.StartTime).TotalSeconds;
                    telemetry.Metrics["ProcessTime"] = currentProcess.TotalProcessorTime.TotalSeconds;
                    telemetry.Metrics["UserTime"] = currentProcess.UserProcessorTime.TotalSeconds;
                    telemetry.Metrics["MemoryPrivateSize"] = currentProcess.PrivateMemorySize64;
                    telemetry.Metrics["MemoryWorkingSet"] = currentProcess.WorkingSet64;
                    telemetry.Metrics["MemoryVirtualSize"] = currentProcess.VirtualMemorySize64;
                    telemetry.Metrics["MemoryPeakWorkingSet"] = currentProcess.PeakWorkingSet64;
                    telemetry.Metrics["MemoryPeakVirtualSize"] = currentProcess.PeakVirtualMemorySize64;
                    telemetry.Metrics["ThreadCount"] = currentProcess.Threads.Count;
                } catch { }
                try
                {
                    telemetry.Metrics["MemoryManagedSize"] = System.GC.GetTotalMemory(false);
                } catch { }
                try
                {
                    telemetry.Properties["IsAggregateException"] = (telemetry.Exception.GetType() == typeof(AggregateException)).ToString();
                } catch { }
                try
                {
                    UpdateTelemetryData(telemetry.Properties, telemetry.Metrics, properties, metrics);
                } catch { }
                telemetryClient.TrackException(telemetry);
            }
            catch (ObjectDisposedException)
            {
                Interlocked.CompareExchange<TelemetryClient>(ref this._telemetryClient, null, telemetryClient);
            }
        }

        void UpdateTelemetryData(IDictionary<string, string> targetProperties, IDictionary<string, double> targetMetrics, IDictionary<string, string> soruceProperties, IDictionary<string, double> sourceMetrics)
        {
            if (targetProperties != null && soruceProperties != null)
            {
                foreach (var prop in soruceProperties)
                {
                    targetProperties[prop.Key] = prop.Value;
                }
            }
            if (targetMetrics != null && sourceMetrics != null)
            {
                foreach (var metric in sourceMetrics)
                {
                    targetMetrics[metric.Key] = metric.Value;
                }
            }
        }
    }


    #region Component Initializer

    class ComponentTelemetryInitializer : ITelemetryInitializer
    {
        /// <summary>
        /// Populates component properties on a telemetry item.
        /// </summary>
        public void Initialize(Microsoft.ApplicationInsights.Channel.ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException("telemetry");
            }
            if (telemetry.Context != null && telemetry.Context.Component != null)
            {
                var instance = ComponentContextReader.Instance;
                telemetry.Context.Component.Version = instance.GetVersion();
            }
        }
    }

    class ComponentContextReader
    {
        private static ComponentContextReader _instance;
        private string _version = "";

        /// <summary>
        /// Gets or sets the singleton instance for our application context reader.
        /// </summary>
        public static ComponentContextReader Instance
        {
            get
            {
                if (ComponentContextReader._instance != null)
                {
                    return ComponentContextReader._instance;
                }
                Interlocked.CompareExchange<ComponentContextReader>(ref ComponentContextReader._instance, new ComponentContextReader(), null);
                return ComponentContextReader._instance;
            }
            internal set
            {
                ComponentContextReader._instance = value;
            }
        }

        /// <summary>
        /// Gets the component version.
        /// </summary>
        /// <returns>The component version.</returns>
        public virtual string GetVersion()
        {
            // 初期値は "" なので、 null ではなく、""で比較する
            if (this._version != "")
            {
                return this._version;
            }

            string version = null;
            try
            {
                var appVer = System.Diagnostics.FileVersionInfo.GetVersionInfo(
                    System.Reflection.Assembly.GetEntryAssembly().Location);

                version = appVer.ProductVersion.ToString();
            }
            catch { }

            return this._version = version;
        }
    }

    #endregion Component Initializer

    #region Session Initializer

    class SessionTelemetryInitializer : ITelemetryInitializer
    {
        /// <summary>
        /// Populates session properties on a telemetry item.
        /// </summary>
        public void Initialize(Microsoft.ApplicationInsights.Channel.ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException("telemetry");
            }
            if (telemetry.Context != null && telemetry.Context.Session != null)
            {
                var instance = SessionContextReader.Instance;
                telemetry.Context.Session.Id = instance.GetSessionId();
                telemetry.Context.Session.IsFirst = instance.IsFirstSession(telemetry.Context.InstrumentationKey);
            }
        }
    }

    class SessionContextReader
    {
        private static SessionContextReader _instance;
        private string _sessionId;
        private bool? _isFirstSession = null;

        /// <summary>
        /// Gets or sets the singleton instance for our application context reader.
        /// </summary>
        public static SessionContextReader Instance
        {
            get
            {
                if (SessionContextReader._instance != null)
                {
                    return SessionContextReader._instance;
                }
                Interlocked.CompareExchange<SessionContextReader>(ref SessionContextReader._instance, new SessionContextReader(), null);
                return SessionContextReader._instance;
            }
            internal set
            {
                SessionContextReader._instance = value;
            }
        }

        /// <summary>
        /// Gets the session id.
        /// </summary>
        /// <returns>The session id.</returns>
        public virtual string GetSessionId()
        {
            if (this._sessionId != null)
            {
                return this._sessionId;
            }

            return this._sessionId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Gets the first session.
        /// </summary>
        /// <returns>The first session flag.</returns>
        public virtual bool IsFirstSession(string instrumentationKey)
        {
            if (this._isFirstSession != null)
            {
                return this._isFirstSession.Value;
            }
            var userRootFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appFoler = System.IO.Path.Combine(userRootFolder, "nishy software", "Telemetry");
            var exeName = Path.GetFileName(Assembly.GetEntryAssembly().Location);
            var name = Assembly.GetEntryAssembly().FullName;
            System.IO.Directory.CreateDirectory(appFoler);
            var aiPath = System.IO.Path.Combine(appFoler, instrumentationKey + "_" + exeName + ".ai");
            bool isFirstSession = false;
            try
            {
                if (System.IO.File.Exists(aiPath))
                {
                    var text = System.IO.File.ReadAllText(aiPath, Encoding.UTF8);
                    isFirstSession = text.IndexOf(name) < 0;
                }
                else
                {
                    isFirstSession = true;
                }
                if (isFirstSession)
                {
                    System.IO.File.AppendAllText(aiPath, name + "\r\n", Encoding.UTF8);
                }

                this._isFirstSession = isFirstSession;
            }
            catch { }

            return isFirstSession;
        }
    }

    #endregion Session Initializer

    #region User Initializer

    class UserTelemetryInitializer : ITelemetryInitializer
    {
        /// <summary>
        /// Populates user properties on a telemetry item.
        /// </summary>
        public void Initialize(Microsoft.ApplicationInsights.Channel.ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException("telemetry");
            }
            if (telemetry.Context != null && telemetry.Context.User != null)
            {
                var instance = UserContextReader.Instance;
                telemetry.Context.User.Id = instance.GetUserUniqueId();
            }
        }
    }

    class UserContextReader
    {
        private static UserContextReader _instance;
        private string _userId = "";

        /// <summary>
        /// Gets or sets the singleton instance for our application context reader.
        /// </summary>
        public static UserContextReader Instance
        {
            get
            {
                if (UserContextReader._instance != null)
                {
                    return UserContextReader._instance;
                }
                Interlocked.CompareExchange<UserContextReader>(ref UserContextReader._instance, new UserContextReader(), null);
                return UserContextReader._instance;
            }
            internal set
            {
                UserContextReader._instance = value;
            }
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <returns>The user id.</returns>
        public virtual string GetUserUniqueId()
        {
            // 初期値は "" なので、 null ではなく、 ""で比較する
            if (this._userId != "")
            {
                return this._userId;
            }

            var userId = GetUserSid();
            if (userId != null)
            {
                // User SIDを使ってUniqueIdを作成するが、少しだけ配置変換して、すぐにはSIDと分からなくする
                userId = userId.Replace("-", "");
                userId = userId.Replace("S", "");
                userId = new String(userId.Reverse().ToArray());
            }
            return this._userId = userId;
        }

        public static string GetUserSid()
        {
            var sid = System.Security.Principal.WindowsIdentity.GetCurrent().User.ToString();
            return sid;
        }
    }

    #endregion User Initializer

    #region Device Initializer

    class DeviceTelemetryInitializer : ITelemetryInitializer
    {
        /// <summary>
        /// Populates device properties on a telemetry item.
        /// </summary>
        public void Initialize(Microsoft.ApplicationInsights.Channel.ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException("telemetry");
            }
            if (telemetry.Context != null && telemetry.Context.Device != null)
            {
                DeviceContextReader instance = DeviceContextReader.Instance;
                telemetry.Context.Device.Type = instance.GetDeviceType();
                telemetry.Context.Device.Id = instance.GetDeviceUniqueId();
                var oemName = instance.GetOemName();
                var model = AdjustDeviceModel(instance.GetDeviceModel(), oemName);
                telemetry.Context.Device.OemName = oemName;
                telemetry.Context.Device.Model = model;
                long networkSpeed = 0;
                telemetry.Context.Device.NetworkType = instance.GetNetworkType(ref networkSpeed);
                telemetry.Context.Device.Language = instance.GetHostSystemLocale();
                telemetry.Context.Device.OperatingSystem = instance.GetOperatingSystem();
            }
        }

        public static string AdjustDeviceModel(string model, string oemName)
        {
            // model名が適切なものが設定されていないものは、区別できるようにメーカー名を追加する
            if (string.IsNullOrWhiteSpace(model))
            {
                model = "";
            }
            if (model == ""
                || model == "System Product Name"
                || model == "To Be Filled By O.E.M."
                || model == "To be filled by O.E.M."
                || model == "All Series"
                )
            {
                model += "(" + oemName + ")";
            }
            return model;
        }
    }

    class DeviceContextReader
    {
        private static DeviceContextReader _instance;
        private string _deviceId = "";
        private string _deviceManufacturer;
        private string _deviceName;
        private string _networkType;
        private long _networkSpeed;
        private string _operatingSystem;

        /// <summary>
        /// Gets or sets the singleton instance for our application context reader.
        /// </summary>
        public static DeviceContextReader Instance
        {
            get
            {
                if (DeviceContextReader._instance != null)
                {
                    return DeviceContextReader._instance;
                }
                Interlocked.CompareExchange<DeviceContextReader>(ref DeviceContextReader._instance, new DeviceContextReader(), null);
                return DeviceContextReader._instance;
            }
            internal set
            {
                DeviceContextReader._instance = value;
            }
        }

        /// <summary>
        /// Gets the host system locale.
        /// </summary>
        /// <returns>The discovered locale.</returns>
        public virtual string GetHostSystemLocale()
        {
            return CultureInfo.CurrentCulture.Name;
        }

        /// <summary>
        /// Gets the type of the device.
        /// </summary>
        /// <returns>The type for this device as a hard-coded string.</returns>
        public virtual string GetDeviceType()
        {
            return "PC";
        }

        /// <summary>
        /// Gets the device unique ID, or uses the fallback if none is available due to application configuration.
        /// </summary>
        /// <returns>
        /// The discovered device identifier.
        /// </returns>
        public virtual string GetDeviceUniqueId()
        {
            // 初期値は "" なので、 null ではなく、 ""で比較する
            if (this._deviceId != "")
            {
                return this._deviceId;
            }

            var deviceId = GetComputerSid();
            if (deviceId != null)
            {
                // Machine SIDを使ってUniqueIdを作成するが、少しだけ配置変換して、すぐにはSIDと分からなくする
                deviceId = deviceId.Replace("-", "");
                deviceId = deviceId.Replace("S", "");
                deviceId = new String(deviceId.Reverse().ToArray());
            }

            return this._deviceId = deviceId;
        }
        public static string GetComputerSid()
        {
            string sid = null;
            // Local AdministratorのSIDを取得して、後半の -500 を取り除いたのをMachineSIDとする
            var sidRef = new System.Security.Principal.NTAccount("Administrator").Translate(typeof(System.Security.Principal.SecurityIdentifier));
            var sidObj = sidRef as SecurityIdentifier;
            if (sidObj != null)
            {
                sid = sidObj.ToString();
                var index = sid.LastIndexOf('-');
                if (index >= 0)
                {
                    sid = sid.Substring(0, index);
                }
            }
            return sid;
        }

        /// <summary>
        /// Gets the device OEM.
        /// </summary>
        /// <returns>The discovered OEM.</returns>
        public virtual string GetOemName()
        {
            if (this._deviceManufacturer != null)
            {
                return this._deviceManufacturer;
            }
            return this._deviceManufacturer = this.RunWmiQuery("Win32_ComputerSystem", "Manufacturer", string.Empty);
        }

        /// <summary>
        /// Gets the device model.
        /// </summary>
        /// <returns>The discovered device model.</returns>
        public virtual string GetDeviceModel()
        {
            if (this._deviceName != null)
            {
                return this._deviceName;
            }
            return this._deviceName = this.RunWmiQuery("Win32_ComputerSystem", "Model", string.Empty);
        }

        /// <summary>
        /// Gets the network type.
        /// </summary>
        /// <returns>The discovered network type.</returns>
        public string GetNetworkType(ref long networkSpeed, bool force = false)
        {
            if (force || string.IsNullOrEmpty(this._networkType))
            {
                NetworkInterfaceType networkType = NetworkInterfaceType.Unknown;
                long speed = 0;
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    bool foundLoopback = false;
                    long loopbackSpeed = 0;
                    NetworkInterfaceType networkTypeWithoutGW = NetworkInterfaceType.Unknown;
                    long speedWithoutGW = 0;
                    NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                    foreach (var networkInterface in allNetworkInterfaces)
                    {
                        if (networkInterface.OperationalStatus == OperationalStatus.Up)
                        {
                            if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                            {
                                foundLoopback = true;
                                loopbackSpeed = networkInterface.Speed;
                            }
                            else
                            {
                                try
                                {
                                    var prop = networkInterface.GetIPProperties();
                                    var gw = prop.GatewayAddresses;
                                    if (gw.Count > 0)
                                    {
                                        if (networkType == NetworkInterfaceType.Unknown)
                                        {
                                            networkType = networkInterface.NetworkInterfaceType;
                                            speed = networkInterface.Speed;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        networkTypeWithoutGW = networkInterface.NetworkInterfaceType;
                                        speedWithoutGW = networkInterface.Speed;
                                    }
                                }
                                catch
                                {
                                    if (networkTypeWithoutGW == NetworkInterfaceType.Unknown)
                                    {
                                        networkTypeWithoutGW = networkInterface.NetworkInterfaceType;
                                        speedWithoutGW = networkInterface.Speed;
                                    }
                                }
                            }
                        }
                    }
                    if (networkType == NetworkInterfaceType.Unknown)
                    {
                        if (networkTypeWithoutGW != NetworkInterfaceType.Unknown)
                        {
                            networkType = networkTypeWithoutGW;
                            speed = speedWithoutGW;
                        }
                        else if (foundLoopback)
                        {
                            networkType = NetworkInterfaceType.Loopback;
                            speed = loopbackSpeed;
                        }
                    }
                }
                this._networkType = networkType.ToString();
                this._networkSpeed = speed;
            }
            networkSpeed = this._networkSpeed;
            return this._networkType;
        }

        /// <summary>
        /// Gets the Operating System.
        /// </summary>
        /// <returns>The discovered operating system.</returns>
        public virtual string GetOperatingSystem()
        {
            if (this._operatingSystem != null)
            {
                return this._operatingSystem;
            }

            var osProps = GetData("Win32_OperatingSystem", "Version,OSType");
            if (osProps != null)
            {
                string version = "";
                try
                {
                    version = osProps["Version"].Value.ToString();
                }
                catch { }
                try
                {
                    var osType = (UInt16)(osProps["OSType"].Value);
                    if (osType == 18)
                    {
                        version = "Windows NT " + version;
                    }
                }
                catch { }

                if (!string.IsNullOrEmpty(version))
                {
                    this._operatingSystem = version;
                }
            }
            return this._operatingSystem;
        }

        /// <summary>
        /// Runs a single WMI query for a property.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="property">The property.</param>
        /// <param name="defaultValue">The default value of the property if WMI fails.</param>
        /// <returns>The value if found, Unknown otherwise.</returns>
        private string RunWmiQuery(string table, string property, string defaultValue)
        {
            try
            {
                using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1}", new object[]
                {
                    property,
                    table
                })))
                {
                    using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectSearcher.Get().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            object obj = ((ManagementObject)enumerator.Current)[property];
                            if (obj != null)
                            {
                                return obj.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //WindowsServerEventSource.Log.DeviceContextWmiFailureWarning(ex.ToString(), "Incorrect");
            }
            return defaultValue;
        }

        static PropertyDataCollection GetData(string category, string properties = "*")
        {
            // Manufacturer (Win32_ComputerSystem)
            // Model        (Win32_ComputerSystem)
            // Version        (Win32_OperatingSystem) Version=10.0.10586
            // Caption=Microsoft Windows 10 Pro
            var scope = new System.Management.ManagementScope("root\\cimv2");
            scope.Connect();

            System.Management.ObjectQuery q = new System.Management.ObjectQuery("select " + properties + " from " + category);
            System.Management.PropertyDataCollection props = null;
            var searcher = new System.Management.ManagementObjectSearcher(scope, q);
            var collection = searcher.Get();

            var sb = new System.Text.StringBuilder();
            foreach (var o in collection)
            {
                if (props == null && o.Properties.Count > 0)
                {
                    props = o.Properties;
                    break;
                }
            }
            return props;
        }
    }

    #endregion Device Initializer
}
