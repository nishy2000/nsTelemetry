/* ==============================
** Copyright 2015, 2020 nishy software
**
**      First Author : nishy software
**		Create : 2015/12/07
** ============================== */

namespace NishySoftware.Telemetry.ApplicationInsights
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    /// <summary>
    /// The module subscribed to AppDomain.CurrentDomain.UnhandledException to send exceptions to ApplicationInsights.
    /// </summary>
    public sealed class UnhandledExceptionTelemetryModule : ITelemetryModule, IDisposable
    {
        private readonly ITelemetryChannel channel;
        private readonly Action<UnhandledExceptionEventHandler> unregisterAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledExceptionTelemetryModule"/> class.
        /// </summary>
        public UnhandledExceptionTelemetryModule() : this(
            action => AppDomain.CurrentDomain.UnhandledException += action,
            action => AppDomain.CurrentDomain.UnhandledException -= action,
            new InMemoryChannel())
        {
        }

        internal UnhandledExceptionTelemetryModule(
            Action<UnhandledExceptionEventHandler> registerAction,
            Action<UnhandledExceptionEventHandler> unregisterAction,
            ITelemetryChannel channel)
        {
            this.unregisterAction = unregisterAction;
            this.channel = channel;

            registerAction(this.CurrentDomainOnUnhandledException);
        }

        /// <summary>
        /// Initializes the telemetry module.
        /// </summary>
        public void Initialize(TelemetryConfiguration configuration)
        {
        }

        /// <summary>
        /// Disposing UnhandledExceptionTelemetryModule instance.
        /// </summary>
        public void Dispose()
        {
            this.unregisterAction(this.CurrentDomainOnUnhandledException);

            if (this.channel != null)
            {
                this.channel.Dispose();
            }
        }

        private static void CopyConfiguration(TelemetryConfiguration source, TelemetryConfiguration target)
        {
            target.InstrumentationKey = source.InstrumentationKey;

            foreach (var telemetryInitializer in source.TelemetryInitializers)
            {
                target.TelemetryInitializers.Add(telemetryInitializer);
            }
        }

        private TelemetryClient GetTelemetryClient(TelemetryConfiguration sourceConfiguration)
        {
            this.channel.EndpointAddress = sourceConfiguration.TelemetryChannel.EndpointAddress;

            var newConfiguration = new TelemetryConfiguration
            {
                TelemetryChannel = this.channel
            };

            CopyConfiguration(sourceConfiguration, newConfiguration);

            var telemetryClient = new TelemetryClient(newConfiguration);
            telemetryClient.Context.GetInternalContext().SdkVersion = "unhnd: " + SdkVersionUtils.GetAssemblyVersion();

            return telemetryClient;
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            //WindowsServerEventSource.Log.CurrentDomainOnUnhandledException();

            var telemetryClient = this.GetTelemetryClient(TelemetryConfiguration.Active);

            var exp = new ExceptionTelemetry(unhandledExceptionEventArgs.ExceptionObject as Exception)
            {
                SeverityLevel = SeverityLevel.Critical,
            };
            try
            {
                exp.Properties[nameof(NishySoftware.Telemetry.ApplicationInsights.ExceptionHandledAt)] = NishySoftware.Telemetry.ApplicationInsights.ExceptionHandledAt.Unhandled.ToString();
            }
            catch { }

            var properties = exp.Properties;
            var metrics = exp.Metrics;
            try
            {
                TelemetryFactoryApplicationInsights.GetGlobalParameters(ref properties, ref metrics);
            }
            catch { }
            try
            {
                TelemetryFactoryApplicationInsights.GetGlobalExceptionParameters(ref properties, ref metrics);
            }
            catch { }
            try
            {
                var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                exp.Metrics["HandleCount"] = currentProcess.HandleCount;
                exp.Metrics["ElapaseTime"] = (DateTime.Now - currentProcess.StartTime).TotalSeconds;
                exp.Metrics["ProcessTime"] = currentProcess.TotalProcessorTime.TotalSeconds;
                exp.Metrics["UserTime"] = currentProcess.UserProcessorTime.TotalSeconds;
                exp.Metrics["MemoryPrivateSize"] = currentProcess.PrivateMemorySize64;
                exp.Metrics["MemoryWorkingSet"] = currentProcess.WorkingSet64;
                exp.Metrics["MemoryVirtualSize"] = currentProcess.VirtualMemorySize64;
                exp.Metrics["MemoryPeakWorkingSet"] = currentProcess.PeakWorkingSet64;
                exp.Metrics["MemoryPeakVirtualSize"] = currentProcess.PeakVirtualMemorySize64;
                exp.Metrics["ThreadCount"] = currentProcess.Threads.Count;
            }
            catch { }
            try
            {
                exp.Metrics["MemoryManagedSize"] = System.GC.GetTotalMemory(false);
            }
            catch { }
            try
            {
                exp.Properties["IsAggregateException"] = (exp.Exception.GetType() == typeof(AggregateException)).ToString();
            }
            catch { }

            telemetryClient.TrackException(exp);
            telemetryClient.Flush();
        }
    }
}
