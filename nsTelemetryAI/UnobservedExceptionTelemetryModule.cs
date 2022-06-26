/* ==============================
** Copyright 2015, 2020, 2021 nishy software
**
**      First Author : nishy software
**		Create : 2015/12/07
** ============================== */

namespace NishySoftware.Telemetry.ApplicationInsights
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    /// <summary>
    /// The module subscribed to TaskScheduler.UnobservedTaskException to send exceptions to ApplicationInsights.
    /// <br/>
    /// [ja] ApplicationInsightsに例外の発生を送信するために、TaskScheduler.UnobservedTaskExceptionイベントを購読するためのTelemetryモジュール
    /// </summary>
    public sealed class UnobservedExceptionTelemetryModule : ITelemetryModule, IDisposable
    {
        private readonly Action<EventHandler<UnobservedTaskExceptionEventArgs>> registerAction;
        private readonly Action<EventHandler<UnobservedTaskExceptionEventArgs>> unregisterAction;
        private readonly object lockObject = new object();

        private TelemetryClient telemetryClient;
        private bool isInitialized = false;

        ///// <summary>
        ///// Initializes a new instance of the <see cref="UnobservedExceptionTelemetryModule" /> class.
        ///// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public UnobservedExceptionTelemetryModule() : this(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            action => TaskScheduler.UnobservedTaskException += action,
            action => TaskScheduler.UnobservedTaskException -= action)
        {
        }

        internal UnobservedExceptionTelemetryModule(
            Action<EventHandler<UnobservedTaskExceptionEventArgs>> registerAction,
            Action<EventHandler<UnobservedTaskExceptionEventArgs>> unregisterAction)
        {
            this.registerAction = registerAction;
            this.unregisterAction = unregisterAction;
        }

        ///// <summary>
        ///// Initializes the telemetry module.
        ///// </summary>
        ///// <param name="configuration">Telemetry Configuration used for creating TelemetryClient for sending exceptions to ApplicationInsights.</param>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void Initialize(TelemetryConfiguration configuration)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            // Core SDK creates 1 instance of a module but calls Initialize multiple times
            if (!this.isInitialized)
            {
                lock (this.lockObject)
                {
                    if (!this.isInitialized)
                    {
                        this.isInitialized = true;

                        this.telemetryClient = new TelemetryClient(configuration);
                        this.telemetryClient.Context.GetInternalContext().SdkVersion = "unobs: " + SdkVersionUtils.GetAssemblyVersion();

                        this.registerAction(this.TaskSchedulerOnUnobservedTaskException);
                    }
                }
            }
        }

        ///// <summary>
        ///// Disposing TaskSchedulerOnUnobservedTaskException instance.
        ///// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void Dispose()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            this.unregisterAction(this.TaskSchedulerOnUnobservedTaskException);
        }

        private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            //WindowsServerEventSource.Log.TaskSchedulerOnUnobservedTaskException();

            var exp = new ExceptionTelemetry(unobservedTaskExceptionEventArgs.Exception)
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
                using (var currentProcess = System.Diagnostics.Process.GetCurrentProcess())
                {
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

            this.telemetryClient.TrackException(exp);
        }
    }
}
