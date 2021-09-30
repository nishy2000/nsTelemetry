/* ==============================
** Copyright 2015, 2018, 2020, 2021 nishy software
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
    using System.Net;
    using System.Management;
    using System.Net.NetworkInformation;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Security;
    using System.Security.Cryptography;
    using System.Security.Principal;
    using System.Reflection;
    using System.Runtime.InteropServices;

    internal class SdkVersionUtils
    {
        internal static string GetAssemblyVersion()
        {
            return typeof(Microsoft.ApplicationInsights.Channel.ITelemetry).Assembly.GetCustomAttributes(false).OfType<AssemblyFileVersionAttribute>().First<AssemblyFileVersionAttribute>().Version;
        }
    }

    //
    // Summary:
    //     This enumeration is used by ExceptionTelemetry to identify if and where exception
    //     was handled.
    internal enum ExceptionHandledAt
    {
        //
        // Summary:
        //     Exception was not handled. Application crashed.
        Unhandled = 0,
        //
        // Summary:
        //     Exception was handled in user code.
        UserCode = 1,
        //
        // Summary:
        //     Exception was handled by some platform handlers.
        Platform = 2
    }


    internal sealed class TelemetryGlobalParams
    {
        internal TelemetryGlobalParams()
        {
        }

        internal readonly object _syncObj = new object();
        internal volatile bool _globalEnable = true;
        internal readonly Dictionary<string, string> _defaultGlobalProperties = new Dictionary<string, string>();
        internal readonly Dictionary<string, double> _defaultGlobalMetrics = new Dictionary<string, double>();
        internal readonly IDictionary<string, string> _defaultGlobalExceptionProperties = new Dictionary<string, string>();
        internal readonly IDictionary<string, double> _defaultGlobalExceptionMetrics = new Dictionary<string, double>();
        internal TelemetryConfiguration _config;
    };

    class TelemetryFactoryApplicationInsights : PlatformBase
    {
        #region Constructors / Destructor
        static TelemetryFactoryApplicationInsights()
        {
#if true
            TelemetryConfiguration adjustedConfig = null;

            if (!IsWindowsPlatform)
            {
                // https://github.com/microsoft/ApplicationInsights-dotnet/pull/1860 (SDK 2.15 or later)
                // ServerTelemetryChannel stores telemetry data in default folder during transient errors in non-windows environments] #1860 
                var tcVersionString = typeof(Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.ServerTelemetryChannel).Assembly.GetCustomAttributes(false).OfType<AssemblyFileVersionAttribute>().First<AssemblyFileVersionAttribute>().Version;
                var isVersion = Version.TryParse(tcVersionString, out var tcVersion);

                if (!isVersion || tcVersion < new Version(2, 15))
                {
                    // SDK 2.15 or before

                    var configXml = ReadConfigurationXml();
                    var telemetryFolder = GetTelemetryFolder();
                    var subdirectoryName = GetStorageFolderName();
                    var telemetryStorageFolder = Path.Combine(telemetryFolder, "StorageFolder", subdirectoryName);
                    var adjustedConfigXml = AdjustServerTelemetryChannelStorageFolder(configXml, telemetryStorageFolder, out var adjustedStorageFolder);
                    if (adjustedStorageFolder)
                    {
                        try
                        {
                            if (!Directory.Exists(telemetryStorageFolder))
                            {
                                System.IO.Directory.CreateDirectory(telemetryStorageFolder);
                            }
                            configXml = adjustedConfigXml;
                            adjustedConfig = TelemetryConfiguration.CreateFromConfiguration(configXml);
                        }
                        catch { }
                    }
                }
            }

            // Refer TelemetryConfiguration.Active in order to initialize TelemetryModules in ApplicationInsights.config
            var config = TelemetryConfiguration.Active;

            // Update telemetryChannel
            if (adjustedConfig != null)
            {
                config.TelemetryChannel = adjustedConfig.TelemetryChannel;
            }
#else
            var config = TelemetryConfiguration.Active;
#endif
            _globalParams._config = config;

#if DEBUG
            var isAttached = true;
#else
            var isAttached = System.Diagnostics.Debugger.IsAttached;
#endif

            if (isAttached
                || config.TelemetryChannel.DeveloperMode.HasValue
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
        static internal readonly TelemetryGlobalParams _globalParams = new TelemetryGlobalParams();
        #endregion Fields

        #region Properties

        #region CommonDataKinds
        static TelemetryDataKinds _commonDataKinds = 0;
        public static TelemetryDataKinds CommonDataKinds
        {
            get
            {
                return _commonDataKinds;
            }
            set
            {
                if (_commonDataKinds != value)
                {
                    var changed = _commonDataKinds ^ value;
                    _commonDataKinds = value;

                    if (_commonDataKinds.HasFlag(TelemetryDataKinds.NetworkType)
                        || _commonDataKinds.HasFlag(TelemetryDataKinds.NetworkSpeed))
                    {
                        // [en] Monitoring required
                        // [ja] 監視が必要
                        System.Net.NetworkInformation.NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
                        System.Net.NetworkInformation.NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
                    }
                    else
                    {
                        // [en] No monitoring required
                        // [ja] 監視が不要
                        System.Net.NetworkInformation.NetworkChange.NetworkAvailabilityChanged -= NetworkChange_NetworkAvailabilityChanged;
                        System.Net.NetworkInformation.NetworkChange.NetworkAddressChanged -= NetworkChange_NetworkAddressChanged;
                    }
                    if (changed.HasFlag(TelemetryDataKinds.NetworkType)
                        || changed.HasFlag(TelemetryDataKinds.NetworkSpeed))
                    {
                        UpdateNetworkType();
                    }

                    UpdateTelemetryDataProperty(changed, TelemetryDataKinds.DeviceName, nameof(TelemetryDataKinds.DeviceName),
                        () =>
                        {
                            DeviceContextReader instance = DeviceContextReader.Instance;
                            var oemName = instance.GetOemName();
                            var model = DeviceTelemetryInitializer.AdjustDeviceModel(instance.GetDeviceModel(), oemName);
                            return model;
                        });

                    UpdateTelemetryDataProperty(changed, TelemetryDataKinds.DeviceManufacturer, nameof(TelemetryDataKinds.DeviceManufacturer),
                        () =>
                        {
                            DeviceContextReader instance = DeviceContextReader.Instance;
                            return instance.GetOemName();
                        });

                    if (IsWindowsPlatform)
                    {
                        UpdateTelemetryDataProperty(changed, TelemetryDataKinds.ScreenResolution, nameof(TelemetryDataKinds.ScreenResolution),
                            () =>
                            {
                                DeviceContextReader instance = DeviceContextReader.Instance;
                                return instance.GetScreenResolution();
                            });
                    }

                    UpdateTelemetryDataProperty(changed, TelemetryDataKinds.Language, nameof(TelemetryDataKinds.Language),
                        () =>
                        {
                            DeviceContextReader instance = DeviceContextReader.Instance;
                            return instance.GetHostSystemLocale();
                        });

                    UpdateTelemetryDataProperty(changed, TelemetryDataKinds.ExeName, nameof(TelemetryDataKinds.ExeName),
                        () =>
                        {
                            var asm = System.Reflection.Assembly.GetEntryAssembly();
                            var exeName = Path.GetFileName(asm.Location);
                            return exeName;
                        });

                    UpdateTelemetryDataProperty(changed, TelemetryDataKinds.HostName, nameof(TelemetryDataKinds.HostName),
                        () =>
                        {
                            var hostName = Dns.GetHostName();
                            if (string.IsNullOrWhiteSpace(hostName))
                            {
                                hostName = Environment.MachineName;
                            }
                            return hostName;
                        });

                    UpdateTelemetryDataProperty(changed, TelemetryDataKinds.UserName, nameof(TelemetryDataKinds.UserName),
                        () =>
                        {
                            return Environment.UserName;
                        });
                }
            }
        }

        static void UpdateTelemetryDataProperty(TelemetryDataKinds changed, TelemetryDataKinds flag, string name, Func<string> getValue)
        {
            if (changed.HasFlag(flag))
            {
                lock (_globalParams._syncObj)
                {
                    if (_commonDataKinds.HasFlag(flag))
                    {
                        _globalParams._defaultGlobalProperties[name] = getValue();
                    }
                    else
                    {
                        _globalParams._defaultGlobalProperties.Remove(name);
                    }
                }
            }
        }
        #endregion

        #endregion Properties

        #region public methods

        public static NishySoftware.Telemetry.ITelemetry Create()
        {
            return new TelemetryApplicationInsights(_globalParams);
        }

        public static void GetGlobalParameters(ref IDictionary<string, string> properties, ref IDictionary<string, double> metrics)
        {
            lock (_globalParams._syncObj)
            {
                foreach (var item in _globalParams._defaultGlobalProperties)
                {
                    properties[item.Key] = item.Value;
                }
                foreach (var item in _globalParams._defaultGlobalMetrics)
                {
                    metrics[item.Key] = item.Value;
                }
            }
        }

        public static void GetGlobalExceptionParameters(ref IDictionary<string, string> properties, ref IDictionary<string, double> metrics)
        {
            lock (_globalParams._syncObj)
            {
                foreach (var item in _globalParams._defaultGlobalExceptionProperties)
                {
                    properties[item.Key] = item.Value;
                }
                foreach (var item in _globalParams._defaultGlobalExceptionMetrics)
                {
                    metrics[item.Key] = item.Value;
                }
            }
        }

        public static bool? EnableDeveloperMode(bool enable)
        {
            bool? old = null;
#if true
            var config = _globalParams._config;
#else
            var config = Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active;
#endif
            if (config?.TelemetryChannel != null)
            {
                old = config.TelemetryChannel.DeveloperMode;
                config.TelemetryChannel.DeveloperMode = false;
            }
            return old;
        }

        public static void SetInstrumentationKey(string instrumentationKey)
        {
#if true
            var config = _globalParams._config;
#else
            var config = Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active;
#endif
            if (config != null)
            {
                config.InstrumentationKey = instrumentationKey;
            }
        }

        public static bool IsEnabled()
        {
            return _globalParams._globalEnable;
        }

        public static bool Enable(bool enable)
        {
            bool oldValue = true;
            lock (_globalParams._syncObj)
            {
                oldValue = _globalParams._globalEnable;
                _globalParams._globalEnable = enable;
            }
            return oldValue;
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
            if (_commonDataKinds.HasFlag(TelemetryDataKinds.NetworkType)
                || _commonDataKinds.HasFlag(TelemetryDataKinds.NetworkSpeed))
            {
                networkType = instance.GetNetworkType(ref networkSpeed, true);
            }
            lock (_globalParams._syncObj)
            {
                if (_commonDataKinds.HasFlag(TelemetryDataKinds.NetworkType))
                {
                    _globalParams._defaultGlobalProperties[nameof(TelemetryDataKinds.NetworkType)] = networkType;
                }
                else
                {
                    _globalParams._defaultGlobalProperties.Remove(nameof(TelemetryDataKinds.NetworkType));
                }
                if (_commonDataKinds.HasFlag(TelemetryDataKinds.NetworkSpeed))
                {
                    _globalParams._defaultGlobalMetrics[nameof(TelemetryDataKinds.NetworkSpeed)] = networkSpeed;
                }
                else
                {
                    _globalParams._defaultGlobalMetrics.Remove(nameof(TelemetryDataKinds.NetworkSpeed));
                }
            }
        }

        static string ReadConfigurationXml()
        {
            string configFilePath;
            try
            {
                // Config file should be in the base directory of the app domain
                configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApplicationInsights.config");
            }
            catch (SecurityException)
            {
                // CoreEventSource.Log.ApplicationInsightsConfigNotAccessibleWarning();
                return string.Empty;
            }

            try
            {
                // Ensure config file actually exists
                if (File.Exists(configFilePath))
                {
                    return File.ReadAllText(configFilePath);
                }
            }
            catch (FileNotFoundException)
            {
                // For cases when file was deleted/modified while reading
                // CoreEventSource.Log.ApplicationInsightsConfigNotFoundWarning(configFilePath);
            }
            catch (DirectoryNotFoundException)
            {
                // For cases when file was deleted/modified while reading
                // CoreEventSource.Log.ApplicationInsightsConfigNotFoundWarning(configFilePath);
            }
            catch (IOException)
            {
                // For cases when file was deleted/modified while reading
                // CoreEventSource.Log.ApplicationInsightsConfigNotFoundWarning(configFilePath);
            }
            catch (UnauthorizedAccessException)
            {
                // CoreEventSource.Log.ApplicationInsightsConfigNotFoundWarning(configFilePath);
            }
            catch (SecurityException)
            {
                // CoreEventSource.Log.ApplicationInsightsConfigNotFoundWarning(configFilePath);
            }

            return string.Empty;
        }

        static string GetDevKeyFromConfigurationXml()
        {
            var configXml = ReadConfigurationXml();
            if (!string.IsNullOrWhiteSpace(configXml))
            {
                // Remove <!-- --!>
                var commentStartKey = "<!--";
                var commentEndKey = "-->";
                int commentStart;
                while ((commentStart = configXml.IndexOf(commentStartKey)) >= 0)
                {
                    var commentEnd = configXml.IndexOf(commentEndKey, commentStart + commentStartKey.Length);
                    if (commentEnd >= 0)
                    {
                        configXml = configXml.Remove(commentStart, commentEnd + commentEndKey.Length - commentStart);
                    }
                }

                // find InstrumentationDevKey elements
                var startKey = "<InstrumentationDevKey>";
                var endKey = "</InstrumentationDevKey>";
                var start = configXml.IndexOf(startKey);
                var end = configXml.IndexOf(endKey);
                if (start >= 0 && end >= 0)
                {
                    start += startKey.Length;
                    var content = configXml.Substring(start, end - start);
                    return content;
                }
            }
            return string.Empty;
        }

        static string AdjustServerTelemetryChannelStorageFolder(string configXml, string storageFolderPath, out bool adjustedStorageFolder)
        {
            adjustedStorageFolder = false;
            if (string.IsNullOrWhiteSpace(configXml)) return string.Empty;
            if (string.IsNullOrWhiteSpace(storageFolderPath)) return configXml;

            // Remove <!-- --!>
            var commentStartKey = "<!--";
            var commentEndKey = "-->";
            int commentStart;
            while ((commentStart = configXml.IndexOf(commentStartKey)) >= 0)
            {
                var commentEnd = configXml.IndexOf(commentEndKey, commentStart + commentStartKey.Length);
                if (commentEnd >= 0)
                {
                    configXml = configXml.Remove(commentStart, commentEnd + commentEndKey.Length - commentStart);
                }
            }

            // find TelemetryChannel elements
            var tcStartKey = "<TelemetryChannel";
            var typeKey = "Type";
            var typeClassName = "Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.ServerTelemetryChannel";
            var tcEndBracketKey = ">";
            var tcsfStartKey = "<StorageFolder>";
            var tcEndKey = "</TelemetryChannel>";
            int startIndex = 0;
            int tcStartTag;
            bool done = false;
            while (!done && (tcStartTag = configXml.IndexOf(tcStartKey, startIndex)) >= 0)
            {
                startIndex = tcStartTag + tcStartKey.Length + 1;

                // check TelemetryChannel as a word
                var tcStartValue = configXml.Substring(tcStartTag, tcStartKey.Length + 1);
                if (tcStartValue.Trim() != tcStartKey)
                {
                    continue;
                }

                // check end bracket
                var tcEndBracket = configXml.IndexOf(tcEndBracketKey, startIndex);
                if (tcStartTag < 0 || tcEndBracket < 0)
                {
                    break;
                }
                else
                {
                    var matchType = false;

                    // check type attribute
                    var tcTypeAttr = configXml.IndexOf(typeKey, startIndex);
                    if (tcTypeAttr > 0 && tcTypeAttr < tcEndBracket)
                    {
                        var value = configXml.Substring(tcTypeAttr + typeKey.Length, tcEndBracket - tcTypeAttr - typeKey.Length);
                        value = value.Trim();
                        if (value.Length > 0 && value[0] == '=')
                        {
                            value = value.Substring(1);
                            value = value.Trim();
                            if (value.Length > 0 && (value[0] == '"' || value[0] == '\''))
                            {
                                var quote = value[0];
                                value = value.Substring(1);
                                var quoteEnd = value.IndexOf(quote);
                                if (quoteEnd >= 0)
                                {
                                    value = value.Substring(0, quoteEnd);
                                }

                            }

                            // check class name
                            var typeValues = value.Split(new char[] { ',' }, 2);
                            matchType = typeValues.First().Trim() == typeClassName;
                        }
                    }
                    if (matchType)
                    {
                        startIndex = tcEndBracket + tcEndBracketKey.Length;
                        var escapedStorageFolderPath = storageFolderPath.Replace("&", "&amp;");
                        if (configXml[tcEndBracket - 1] == '/')
                        {
                            // <TelemetryChannel ... />
                            var element = string.Format("><StorageFolder>{0}</StorageFolder></TelemetryChannel>", escapedStorageFolderPath);
                            configXml = configXml.Remove(tcEndBracket - 1, 2);
                            configXml = configXml.Insert(tcEndBracket - 1, element);
                            startIndex += element.Length - 2;

                            adjustedStorageFolder = true;

                            // Set StorageFolder only once.
                            done = true;
                        }
                        else
                        {
                            // <TelemetryChannel ... >...</TelemetryChannel>
                            var tcEndTag = configXml.IndexOf(tcEndKey, startIndex);

                            if (tcEndTag < 0)
                            {
                                continue;
                            }
                            if (configXml.IndexOf(tcsfStartKey, startIndex) >= 0)
                            {
                                // already exist StorageFolder tag in TelemetryChannel tag
                                startIndex = tcEndTag + tcEndKey.Length;

                                done = true;
                            }
                            else
                            {
                                // not exist StorageFolder tag in TelemetryChannel tag
                                var element = string.Format("<StorageFolder>{0}</StorageFolder>", escapedStorageFolderPath);
                                configXml = configXml.Insert(tcEndBracket + 1, element);
                                startIndex = tcEndTag + tcEndKey.Length + element.Length;

                                adjustedStorageFolder = true;

                                // Set StorageFolder only once.
                                done = true;
                            }
                        }
                    }
                    else
                    {
                        startIndex = tcEndBracket + tcEndBracketKey.Length;
                    }
                }
            }
            return configXml;
        }

        #endregion private methods
    }

    sealed class TelemetryApplicationInsights : NishySoftware.Telemetry.ITelemetry
    {
        #region Constructors / Destructor
        public TelemetryApplicationInsights(TelemetryGlobalParams globalParams, TelemetryConfiguration configuration = null)
        {
            this._globalParams = globalParams;
#if true
            this._configuration = configuration ?? globalParams._config;
#else
            this._configuration = configuration ?? TelemetryConfiguration.Active;
#endif
        }
        #endregion Constructors / Destructor

        #region Fields
        readonly TelemetryConfiguration _configuration;
        readonly TelemetryGlobalParams _globalParams;
        readonly Dictionary<string, string> _defaultProperties = new Dictionary<string, string>();
        readonly Dictionary<string, double> _defaultMetrics = new Dictionary<string, double>();
        TelemetryClient _telemetryClient;
        #endregion

        #region ITelemetry interface

        public object GlobalSyncObject { get { return this._globalParams._syncObj; } }

        public IDictionary<string, string> GlobalProperties { get { return this._globalParams._defaultGlobalProperties; } }

        public IDictionary<string, double> GlobalMetrics { get { return this._globalParams._defaultGlobalMetrics; } }

        public IDictionary<string, string> GlobalExceptionProperties { get { return this._globalParams._defaultGlobalExceptionProperties; } }

        public IDictionary<string, double> GlobalExceptionMetrics { get { return this._globalParams._defaultGlobalExceptionMetrics; } }

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
                SeverityLevel = level,
            };

            this.TrackException(exp, ExceptionHandledAt.UserCode, properties, metrics);
        }

        public void Flush()
        {
            if (!this._globalParams._globalEnable) return;

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
            if (!this._globalParams._globalEnable) return;

            TelemetryClient telemetryClient = LazyInitializer.EnsureInitialized<TelemetryClient>(ref this._telemetryClient, () => new TelemetryClient(this._configuration));
            try
            {
                lock (this._globalParams._syncObj)
                {
                    UpdateTelemetryData(telemetry.Properties, telemetry.Metrics, this._globalParams._defaultGlobalProperties, this._globalParams._defaultGlobalMetrics);
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
            if (!this._globalParams._globalEnable) return;

            TelemetryClient telemetryClient = LazyInitializer.EnsureInitialized<TelemetryClient>(ref this._telemetryClient, () => new TelemetryClient(this._configuration));
            try
            {
                if (telemetry.Name.IndexOf(':') < 0)
                {
                    telemetry.Name = "application:" + telemetry.Name;
                }
                lock (this._globalParams._syncObj)
                {
                    UpdateTelemetryData(telemetry.Properties, telemetry.Metrics, this._globalParams._defaultGlobalProperties, this._globalParams._defaultGlobalMetrics);
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

        void TrackException(ExceptionTelemetry telemetry, NishySoftware.Telemetry.ApplicationInsights.ExceptionHandledAt at, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            if (!this._globalParams._globalEnable) return;

            TelemetryClient telemetryClient = LazyInitializer.EnsureInitialized<TelemetryClient>(ref this._telemetryClient, () => new TelemetryClient(this._configuration));
            try
            {
                try
                {
                    lock (this._globalParams._syncObj)
                    {
                        UpdateTelemetryData(telemetry.Properties, telemetry.Metrics, this._globalParams._defaultGlobalProperties, this._globalParams._defaultGlobalMetrics);
                        UpdateTelemetryData(telemetry.Properties, telemetry.Metrics, this._globalParams._defaultGlobalExceptionProperties, this._globalParams._defaultGlobalExceptionMetrics);
                    }
                    UpdateTelemetryData(telemetry.Properties, telemetry.Metrics, this._defaultProperties, this._defaultMetrics);
                }
                catch { }
                try
                {
                    telemetry.Properties[nameof(NishySoftware.Telemetry.ApplicationInsights.ExceptionHandledAt)] = at.ToString();
                }
                catch { }
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
                }
                catch { }
                try
                {
                    telemetry.Metrics["MemoryManagedSize"] = System.GC.GetTotalMemory(false);
                }
                catch { }
                try
                {
                    telemetry.Properties["IsAggregateException"] = (telemetry.Exception.GetType() == typeof(AggregateException)).ToString();
                }
                catch { }
                try
                {
                    UpdateTelemetryData(telemetry.Properties, telemetry.Metrics, properties, metrics);
                }
                catch { }
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

    class PlatformBase
    {
        static internal bool IsLinuxPlatform
        {
            get
            {
#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET470
                return Environment.OSVersion.Platform == PlatformID.Unix;
#else
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
#endif
            }
        }

        static internal bool IsWindowsPlatform
        {
            get
            {
#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET470
                return Environment.OSVersion.Platform == PlatformID.Win32S
                    || Environment.OSVersion.Platform == PlatformID.Win32Windows
                    || Environment.OSVersion.Platform == PlatformID.Win32NT
                    || Environment.OSVersion.Platform == PlatformID.WinCE;
#else
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif
            }
        }

        static readonly string _folderCompany = "nishy software";
        static readonly string _folderProduct = "nsTelemetry";

        static internal string GetTelemetryFolder()
        {
            string appFolder = null;
            if (IsWindowsPlatform)
            {
                var userRootFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                appFolder = System.IO.Path.Combine(userRootFolder, _folderCompany, _folderProduct);
            }
            else if (IsLinuxPlatform)
            {
                var userRootFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (string.IsNullOrWhiteSpace(userRootFolder))
                {
                    // XDG Base Directory Specification
                    //  -               $HOME                   SpecialFolder.Personal
                    //  XDG_CONFIG_HOME $HOME/.config           SpecialFolder.ApplicationData
                    //  XDG_DATA_HOME   $HOME/.local/share      SpecialFolder.LocalApplicationData
                    //  XDG_STATE_HOME  $HOME/.local/state      -
                    userRootFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    userRootFolder = System.IO.Path.Combine(userRootFolder, ".local", ".share");
                }
                appFolder = System.IO.Path.Combine(userRootFolder, _folderCompany, _folderProduct);
            }

            if (string.IsNullOrWhiteSpace(appFolder))
            {
                var userRootFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                appFolder = System.IO.Path.Combine(userRootFolder, _folderCompany, _folderProduct);
            }

            return appFolder;
        }

        static internal string GetStorageFolderName()
        {
#if NETFRAMEWORK
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
#else
            var baseDirectory = AppContext.BaseDirectory;
#endif

            var appIdentity = GetName() + "@" + Path.Combine(baseDirectory, System.Diagnostics.Process.GetCurrentProcess().ProcessName);
            return GetSHA256Hash(appIdentity);
        }

        static internal string GetSHA256Hash(string input)
        {
            var hashString = new StringBuilder();

            byte[] inputBits = Encoding.Unicode.GetBytes(input);
            using (var sha256 = CreateSHA256())
            {
                byte[] hashBits = sha256.ComputeHash(inputBits);
                foreach (byte b in hashBits)
                {
                    hashString.Append(b.ToString("x2", CultureInfo.InvariantCulture));
                }
            }

            return hashString.ToString();
        }

        static SHA256 CreateSHA256()
        {
#if NETSTANDARD
            return SHA256.Create();
#else
            return new SHA256CryptoServiceProvider();
#endif
        }

        static string GetName()
        {
            if (IsWindowsPlatform)
            {
                return WindowsIdentity.GetCurrent().Name;
            }
            else
            {
                return Environment.UserName;
            }
        }
    }

    #region Component Initializer

    class ComponentTelemetryInitializer : ITelemetryInitializer
    {
        ///// <summary>
        ///// Populates component properties on a telemetry item.
        ///// </summary>
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

        ///// <summary>
        ///// Gets or sets the singleton instance for our application context reader.
        ///// </summary>
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

        ///// <summary>
        ///// Gets the component version.
        ///// </summary>
        ///// <returns>The component version.</returns>
        public virtual string GetVersion()
        {
            // [en] default value is "", so compare with "" instead of null
            // [ja] 初期値は "" なので、 null ではなく、 ""で比較する
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
        ///// <summary>
        ///// Populates session properties on a telemetry item.
        ///// </summary>
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

    class SessionContextReader : PlatformBase
    {
        private static SessionContextReader _instance;
        private string _sessionId;
        private bool? _isFirstSession = null;

        ///// <summary>
        ///// Gets or sets the singleton instance for our application context reader.
        ///// </summary>
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

        ///// <summary>
        ///// Gets the session id.
        ///// </summary>
        ///// <returns>The session id.</returns>
        public virtual string GetSessionId()
        {
            if (this._sessionId != null)
            {
                return this._sessionId;
            }

            return this._sessionId = Guid.NewGuid().ToString();
        }

        ///// <summary>
        ///// Gets the first session.
        ///// </summary>
        ///// <returns>The first session flag.</returns>
        public virtual bool IsFirstSession(string instrumentationKey)
        {
            if (this._isFirstSession != null)
            {
                return this._isFirstSession.Value;
            }

            string appFolder = GetTelemetryFolder();
            var exeName = Path.GetFileName(Assembly.GetEntryAssembly().Location);
            var name = Assembly.GetEntryAssembly().FullName;
            System.IO.Directory.CreateDirectory(appFolder);
            var aiPath = System.IO.Path.Combine(appFolder, instrumentationKey + "_" + exeName + ".ai");
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
        ///// <summary>
        ///// Populates user properties on a telemetry item.
        ///// </summary>
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

    class UserContextReader : PlatformBase
    {
        private static UserContextReader _instance;
        private string _userId = "";

        ///// <summary>
        ///// Gets or sets the singleton instance for our application context reader.
        ///// </summary>
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

        ///// <summary>
        ///// Gets the user id.
        ///// </summary>
        ///// <returns>The user id.</returns>
        public virtual string GetUserUniqueId()
        {
            // [en] default value is "", so compare with "" instead of null
            // [ja] 初期値は "" なので、 null ではなく、 ""で比較する
            if (this._userId != "")
            {
                return this._userId;
            }

            string userId;
            if (IsWindowsPlatform)
            {
                userId = GetUserSid();
                if (userId != null)
                {
                    // [en] Create a UniqueId using User SID, but transform the placement a bit.
                    // [ja] User SIDを使ってUniqueIdを作成するが、少しだけ配置を変換する
                    userId = userId.Replace("-", "");
                    userId = userId.Replace("S", "");
                    userId = new String(userId.Reverse().ToArray());
                }
            }
            else
            {
                userId = GetUserGuid();
            }
            return this._userId = userId;
        }

        public static string GetUserSid()
        {
            var sid = System.Security.Principal.WindowsIdentity.GetCurrent().User.ToString();
            return sid;
        }

        public static string GetUserGuid()
        {
            string uid = null;
            string appFolder = GetTelemetryFolder();
            System.IO.Directory.CreateDirectory(appFolder);
            var aiPath = System.IO.Path.Combine(appFolder, "idu.ai");
            try
            {
                if (System.IO.File.Exists(aiPath))
                {
                    var line = System.IO.File.ReadLines(aiPath, Encoding.UTF8)?.FirstOrDefault();
                    line = line?.Trim() ?? "";
                    if (Guid.TryParse(line, out var result))
                    {
                        uid = line;
                    }
                }
                if (string.IsNullOrWhiteSpace(uid))
                {
                    uid = Guid.NewGuid().ToString();
                    System.IO.File.AppendAllLines(aiPath, new List<string> { uid }, Encoding.UTF8);
                }
            }
            catch { }
            return uid;
        }
    }

    #endregion User Initializer

    #region Device Initializer

    class DeviceTelemetryInitializer : ITelemetryInitializer
    {
        ///// <summary>
        ///// Populates device properties on a telemetry item.
        ///// </summary>
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
                telemetry.Context.Device.OperatingSystem = instance.GetOperatingSystem();
            }
        }

        public static string AdjustDeviceModel(string model, string oemName)
        {
            // [en] If the appropriate model name is not set, add the manufacturer's name so that it can be distinguished.
            // [ja] model名が適切なものが設定されていないものは、区別できるようにメーカー名を追加する
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

    class DeviceContextReader : PlatformBase
    {
        private static DeviceContextReader _instance;
        private string _deviceId = "";
        private string _deviceManufacturer;
        private string _deviceName;
        private string _screenResolution;
        private string _networkType;
        private long _networkSpeed;
        private string _operatingSystem;

        ///// <summary>
        ///// Gets or sets the singleton instance for our application context reader.
        ///// </summary>
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

        ///// <summary>
        ///// Gets the host system locale.
        ///// </summary>
        ///// <returns>The discovered locale.</returns>
        public virtual string GetHostSystemLocale()
        {
            return CultureInfo.CurrentCulture.Name;
        }

        ///// <summary>
        ///// Gets the type of the device.
        ///// </summary>
        ///// <returns>The type for this device as a hard-coded string.</returns>
        public virtual string GetDeviceType()
        {
            return "PC";
        }

        ///// <summary>
        ///// Gets the device unique ID, or uses the fallback if none is available due to application configuration.
        ///// </summary>
        ///// <returns>
        ///// The discovered device identifier.
        ///// </returns>
        public virtual string GetDeviceUniqueId()
        {
            // [en] default value is "", so compare with "" instead of null
            // [ja] 初期値は "" なので、 null ではなく、 ""で比較する
            if (this._deviceId != "")
            {
                return this._deviceId;
            }

            string deviceId = null;
            if (IsWindowsPlatform)
            {
                deviceId = GetComputerSid();
                if (deviceId != null)
                {
                    // [en] Create a UniqueId using Machine SID, but transform the placement a bit.
                    // [ja] Machine SIDを使ってUniqueIdを作成するが、少しだけ配置を変換する
                    deviceId = deviceId.Replace("-", "");
                    deviceId = deviceId.Replace("S", "");
                    deviceId = new String(deviceId.Reverse().ToArray());
                }
            }
            else
            {
                string appFolder = GetTelemetryFolder();
                System.IO.Directory.CreateDirectory(appFolder);
                var aiPath = System.IO.Path.Combine(appFolder, "idd.ai");
                try
                {
                    if (System.IO.File.Exists(aiPath))
                    {
                        var line = System.IO.File.ReadLines(aiPath, Encoding.UTF8)?.FirstOrDefault();
                        line = line?.Trim() ?? "";
                        if (Guid.TryParse(line, out var result))
                        {
                            deviceId = line;
                        }
                    }
                }
                catch { }
                if (this._deviceId != "")
                {
                    return this._deviceId;
                }
                if (string.IsNullOrWhiteSpace(deviceId))
                {
                    try
                    {
                        var pathDiskById = "/dev/disk/by-id";
                        if (System.IO.Directory.Exists(pathDiskById))
                        {
                            var files = System.IO.Directory.EnumerateFiles(pathDiskById, "scsi-*", SearchOption.TopDirectoryOnly).ToList();
                            var seed = files.FirstOrDefault(i => i.Contains("-part3"));
                            if (seed == null)
                            {
                                seed = files.FirstOrDefault(i => i.Contains("-part2"));
                            }
                            if (seed == null)
                            {
                                seed = files.FirstOrDefault(i => i.Contains("-part1"));
                            }
                            if (seed == null)
                            {
                                files = System.IO.Directory.EnumerateFiles(pathDiskById, "wwn-*", SearchOption.TopDirectoryOnly).ToList();
                                seed = files.FirstOrDefault(i => i.Contains("-part3"));
                                if (seed == null)
                                {
                                    seed = files.FirstOrDefault(i => i.Contains("-part2"));
                                }
                                if (seed == null)
                                {
                                    seed = files.FirstOrDefault(i => i.Contains("-part1"));
                                }
                            }
                            if (seed != null)
                            {
                                var hashProvider = new SHA256CryptoServiceProvider();
                                var hashed = string.Join("", hashProvider.ComputeHash(Encoding.UTF8.GetBytes(seed)).Select(x => $"{x:x2}"));
                                deviceId = hashed.Substring(0, 32);
                            }
                        }
                    }
                    catch { }
                    if (this._deviceId != "")
                    {
                        return this._deviceId;
                    }
                    if (string.IsNullOrWhiteSpace(deviceId))
                    {
                        try
                        {
                            var bytes = GetNetworkAddress();
                            if ((bytes?.Length ?? 0) >= 4)
                            {
                                var guiBytes = new byte[16];
                                for (int i = 0; i < guiBytes.Length; i++)
                                {
                                    guiBytes[i] = (byte)i;
                                }
                                for (int i = 0; i < bytes.Length && i < guiBytes.Length; i++)
                                {
                                    guiBytes[i] = bytes[i];
                                }
                                deviceId = new Guid(guiBytes).ToString();
                            }
                        }
                        catch { }
                    }
                    if (this._deviceId != "")
                    {
                        return this._deviceId;
                    }
                    if (string.IsNullOrWhiteSpace(deviceId))
                    {
                        deviceId = Guid.NewGuid().ToString();
                    }

                    if (!string.IsNullOrWhiteSpace(deviceId))
                    {
                        deviceId = deviceId.ToLower();
                        System.IO.File.AppendAllLines(aiPath, new List<string> { deviceId }, Encoding.UTF8);
                    }
                }
            }

            if (this._deviceId != "")
            {
                return this._deviceId;
            }

            return this._deviceId = deviceId;
        }

        public static string GetComputerSid()
        {
            string sid = null;
            // [en] Get the SID of the Local Administrator and remove the -500 in the second half to make it the MachineSID.
            // [ja] Local AdministratorのSIDを取得して、後半の -500 を取り除いたのをMachineSIDとする
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

        public byte[] GetNetworkAddress()
        {
            byte[] address = null;

            NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            var ethernets = allNetworkInterfaces.Where(i => i.NetworkInterfaceType == NetworkInterfaceType.Ethernet
            || i.NetworkInterfaceType == NetworkInterfaceType.FastEthernetT
            || i.NetworkInterfaceType == NetworkInterfaceType.GigabitEthernet).OrderBy(i => i.OperationalStatus).ToArray();
            var wirelesses = allNetworkInterfaces.Where(i => i.NetworkInterfaceType == NetworkInterfaceType.Wireless80211).OrderBy(i => i.OperationalStatus).ToArray();
            var physicalAddresses = new List<byte[]>();
            foreach (var item in ethernets.Concat(wirelesses))
            {
                try
                {
                    var physical = item.GetPhysicalAddress();
                    var physicalAddress = physical.GetAddressBytes();
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(string.Join(":", physicalAddress.Select(i => i.ToString("x2"))));
#endif
                    if (!physicalAddress.All(i => i == 0 || i == 0xff))
                    {
                        physicalAddresses.Add(physicalAddress);

                        var sp = item.Speed;    // if exception occurs, skip
                        address = physicalAddress;
                        break;
                    }
                }
                catch { }
            }
            if (address == null)
            {
                foreach (var physicalAddress in physicalAddresses)
                {
                    if (!IsVirtualAddress(physicalAddress))
                    {
                        address = physicalAddress;
                        break;
                    }
                }
            }
            if (address == null)
            {
                address = physicalAddresses.FirstOrDefault();
            }
            return address;
        }

        bool IsVirtualAddress(byte[] physicalAddress)
        {
            // https://macaddress.io/faq/how-to-detect-a-virtual-machine-by-its-mac-address
            return IsHyperVAddress(physicalAddress)
                || IsVirtualBoxAddress(physicalAddress)
                || IsVMWareAddress(physicalAddress)
                || IsXenAddress(physicalAddress)
                || IsParallellsAddress(physicalAddress)
                || IsVirtualIron(physicalAddress);
        }
        bool IsHyperVAddress(byte[] physicalAddress)
        {
            // 00-15-5D(Hyper-V), 00-03-FF(Virtual PC)
            return physicalAddress.Length >= 3
                && (physicalAddress[0] == 0x00
                && physicalAddress[1] == 0x15
                && physicalAddress[2] == 0x5d
                || physicalAddress[0] == 0x00
                && physicalAddress[1] == 0x03
                && physicalAddress[2] == 0xff);
        }

        bool IsVirtualBoxAddress(byte[] physicalAddress)
        {
            // 08-00-27(Oracle VirtualBox 5.2), 00:21:F6(Oracle VirtualBox 3.3), 00:14:4F(Oracle VM Server for SPARC)
            // 52:54:00:C9:C7:04(Oracle VirtualBox 5.2 + Vagrant)
            return (physicalAddress.Length >= 3
                && (physicalAddress[0] == 0x08
                && physicalAddress[1] == 0x00
                && physicalAddress[2] == 0x27
                || physicalAddress[0] == 0x00
                && physicalAddress[1] == 0x21
                && physicalAddress[2] == 0xf6
                || physicalAddress[0] == 0x00
                && physicalAddress[1] == 0x14
                && physicalAddress[2] == 0x4f))
                || physicalAddress.Length >= 6
                && (physicalAddress[0] == 0x52
                && physicalAddress[1] == 0x54
                && physicalAddress[2] == 0x00
                && physicalAddress[3] == 0xc9
                && physicalAddress[4] == 0xc7
                && physicalAddress[5] == 0x04);
        }

        bool IsVMWareAddress(byte[] physicalAddress)
        {
            // 00-50-56, 00-0C-29, 00-05-69, 00:1C:14
            return physicalAddress.Length >= 3
                && (physicalAddress[0] == 0x00
                && physicalAddress[1] == 0x50
                && physicalAddress[2] == 0x56
                || physicalAddress[0] == 0x00
                && physicalAddress[1] == 0x0c
                && physicalAddress[2] == 0x29
                || physicalAddress[0] == 0x00
                && physicalAddress[1] == 0x05
                && physicalAddress[2] == 0x69
                || physicalAddress[0] == 0x00
                && physicalAddress[1] == 0x1c
                && physicalAddress[2] == 0x14);
        }

        bool IsXenAddress(byte[] physicalAddress)
        {
            // 00-16-3E
            return physicalAddress.Length >= 3
                && (physicalAddress[0] == 0x00
                && physicalAddress[1] == 0x16
                && physicalAddress[2] == 0x3e);
        }

        bool IsParallellsAddress(byte[] physicalAddress)
        {
            // 00-1C-42
            return physicalAddress.Length >= 3
                && (physicalAddress[0] == 0x00
                && physicalAddress[1] == 0x1c
                && physicalAddress[2] == 0x42);
        }

        bool IsVirtualIron(byte[] physicalAddress)
        {
            // 00-0F-4B (Oracle Virtual Iron 4)
            return physicalAddress.Length >= 3
                && physicalAddress[0] == 0x00
                && physicalAddress[1] == 0x0f
                && physicalAddress[2] == 0x4b;
        }

        ///// <summary>
        ///// Gets the device OEM.
        ///// </summary>
        ///// <returns>The discovered OEM.</returns>
        public virtual string GetOemName()
        {
            if (this._deviceManufacturer != null)
            {
                return this._deviceManufacturer;
            }
            string deviceManufacturer = string.Empty;
            if (IsWindowsPlatform)
            {
                deviceManufacturer = this.RunWmiQuery("Win32_ComputerSystem", "Manufacturer", string.Empty);
            }
            else
            {
                var pathVender = "/sys/devices/virtual/dmi/id/sys_vendor";
                var pathVersion = "/proc/version";
                if (System.IO.File.Exists(pathVender))
                {
                    try
                    {
                        var line = System.IO.File.ReadLines(pathVender, Encoding.UTF8)?.FirstOrDefault();
                        line = line?.Trim() ?? "";
                        deviceManufacturer = line;
                    }
                    catch { }
                }
                if (deviceManufacturer == string.Empty
                    && System.IO.File.Exists(pathVersion))
                {
                    try
                    {
                        var line = System.IO.File.ReadLines(pathVersion, Encoding.UTF8)?.FirstOrDefault();
                        if (line.Contains("Microsoft") || line.Contains("microsoft"))
                        {
                            deviceManufacturer = "Microsoft Corporation";
                        }
                    }
                    catch { }
                }
            }
            return this._deviceManufacturer = deviceManufacturer;
        }

        ///// <summary>
        ///// Gets the device model.
        ///// </summary>
        ///// <returns>The discovered device model.</returns>
        public virtual string GetDeviceModel()
        {
            if (this._deviceName != null)
            {
                return this._deviceName;
            }
            string deviceName = string.Empty;
            if (IsWindowsPlatform)
            {
                deviceName = this.RunWmiQuery("Win32_ComputerSystem", "Model", string.Empty);
            }
            else
            {
                var pathName = "/sys/devices/virtual/dmi/id/product_name";
                var pathVersion = "/proc/version";
                if (System.IO.File.Exists(pathName))
                {
                    try
                    {
                        var line = System.IO.File.ReadLines(pathName, Encoding.UTF8)?.FirstOrDefault();
                        line = line?.Trim() ?? "";
                        deviceName = line;
                    }
                    catch { }
                }
                if (deviceName == string.Empty
                    && System.IO.File.Exists(pathVersion))
                {
                    try
                    {
                        var line = System.IO.File.ReadLines(pathVersion, Encoding.UTF8)?.FirstOrDefault();
                        if (line.Contains("Microsoft") || line.Contains("microsoft"))
                        {
                            // Linux version 5.10.16.3-microsoft-standard-WSL2 (oe-user@oe-host) (x86_64-msft-linux-gcc (GCC) 9.3.0, GNU ld (GNU Binutils) 2.34.0.20200220) #1 SMP Fri Apr 2 22:23:49 UTC 2021
                            if (line.Contains("WSL2") || line.Contains("wsl2"))
                            {
                                deviceName = "WSL 2";
                            }
                            else if (line.Contains("WSL") || line.Contains("wsl"))
                            {
                                deviceName = "WSL";
                            }
                            // Linux version 4.4.0-19041-Microsoft (Microsoft@Microsoft.com) (gcc version 5.4.0 (GCC) ) #1237-Microsoft Sat Sep 11 14:32:00 PST 2021
                            else if (line.Contains("Microsoft@Microsoft.com") || line.Contains("microsoft@microsoft.com"))
                            {
                                deviceName = "WSL 1";
                            }
                        }
                    }
                    catch { }
                }
            }
            return this._deviceName = deviceName;
        }

        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(SystemMetric smIndex);

        ///// <summary>
        ///// Flags used with the Windows API (User32.dll):GetSystemMetrics(SystemMetric smIndex)
        /////  
        ///// This Enum and declaration signature was written by Gabriel T. Sharp
        ///// ai_productions@verizon.net or osirisgothra@hotmail.com
        ///// Obtained on pinvoke.net, please contribute your code to support the wiki!
        ///// </summary>
        public enum SystemMetric : int
        {
            ///// <summary>
            ///// The width of the screen of the primary display monitor, in pixels. This is the same value obtained by calling 
            ///// GetDeviceCaps as follows: GetDeviceCaps( hdcPrimaryMonitor, HORZRES).
            ///// </summary>
            SM_CXSCREEN = 0,
            ///// <summary>
            ///// The height of the screen of the primary display monitor, in pixels. This is the same value obtained by calling 
            ///// GetDeviceCaps as follows: GetDeviceCaps( hdcPrimaryMonitor, VERTRES).
            ///// </summary>
            SM_CYSCREEN = 1,
        }

        ///// <summary>
        ///// Gets the ScreenResolution.
        ///// </summary>
        ///// <returns>The discovered ScreenResolution.</returns>
        public string GetScreenResolution(bool force = false)
        {
            if (force || string.IsNullOrEmpty(this._screenResolution))
            {
                var width = GetSystemMetrics(SystemMetric.SM_CXSCREEN);
                var height = GetSystemMetrics(SystemMetric.SM_CYSCREEN);
                this._screenResolution = string.Format("{0:D}x{1:D}", width, height);
            }
            return this._screenResolution;
        }

        ///// <summary>
        ///// Gets the network type.
        ///// </summary>
        ///// <returns>The discovered network type.</returns>
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

        ///// <summary>
        ///// Gets the Operating System.
        ///// </summary>
        ///// <returns>The discovered operating system.</returns>
        public virtual string GetOperatingSystem()
        {
            if (this._operatingSystem != null)
            {
                return this._operatingSystem;
            }

            string operatingSystem = string.Empty;
            if (IsWindowsPlatform)
            {
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
                        operatingSystem = version;
                    }
                }
            }
            else
            {
                operatingSystem = "Linux";
                var pathRelease = "/etc/os-release";
                if (System.IO.File.Exists(pathRelease))
                {
                    try
                    {
                        // for WSL2
                        //NAME="Ubuntu"
                        //VERSION="20.04.1 LTS (Focal Fossa)"
                        //ID=ubuntu
                        //ID_LIKE=debian
                        //PRETTY_NAME="Ubuntu 20.04.1 LTS"
                        //VERSION_ID="20.04"
                        //HOME_URL="https://www.ubuntu.com/"
                        //SUPPORT_URL="https://help.ubuntu.com/"
                        //BUG_REPORT_URL="https://bugs.launchpad.net/ubuntu/"
                        //PRIVACY_POLICY_URL="https://www.ubuntu.com/legal/terms-and-policies/privacy-policy"
                        //VERSION_CODENAME=focal
                        //UBUNTU_CODENAME=focal

                        // for debian
                        //PRETTY_NAME="Debian GNU/Linux 10 (buster)"
                        //NAME="Debian GNU/Linux"
                        //VERSION_ID="10"
                        //VERSION="10 (buster)"
                        //VERSION_CODENAME=buster
                        //ID=debian
                        //HOME_URL="https://www.debian.org/"
                        //SUPPORT_URL="https://www.debian.org/support"
                        //BUG_REPORT_URL="https://bugs.debian.org/"

                        var lines = System.IO.File.ReadLines(pathRelease, Encoding.UTF8);
                        var prettyName = lines?.FirstOrDefault(i => i.StartsWith("PRETTY_NAME="))?.Substring(12).Trim('"');
                        var name = lines?.FirstOrDefault(i => i.StartsWith("NAME="))?.Substring(5).Trim('"');
                        var versionId = lines?.FirstOrDefault(i => i.StartsWith("VERSION_ID="))?.Substring(11).Trim('"');
                        if (!string.IsNullOrEmpty(prettyName))
                        {
                            operatingSystem = prettyName;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(name))
                            {
                                operatingSystem = name;
                            }
                            if (!string.IsNullOrEmpty(versionId))
                            {
                                operatingSystem += " " + versionId;
                            }
                        }
                    }
                    catch { }
                }
            }
            return this._operatingSystem = operatingSystem;
        }

        ///// <summary>
        ///// Runs a single WMI query for a property.
        ///// </summary>
        ///// <param name="table">The table.</param>
        ///// <param name="property">The property.</param>
        ///// <param name="defaultValue">The default value of the property if WMI fails.</param>
        ///// <returns>The value if found, Unknown otherwise.</returns>
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
