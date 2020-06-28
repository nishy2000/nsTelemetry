/* ==============================
** Copyright 2020 nishy software
**
**      First Author : nishy software
**		Create : 2020/06/29
** ============================== */

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
                    // TelemetryDataFlags は、初回は時間がかかるので、非同期で設定する。
                    Task.Run(() =>
                    {
                        NishySoftware.Telemetry.ApplicationInsights.Telemetry.TelemetryDataFlags = NishySoftware.Telemetry.ApplicationInsights.Telemetry.TelemetryDataFlag.All;
                    });

                    var userDomainName = Environment.UserDomainName;
                    var telemetry = NishySoftware.Telemetry.ApplicationInsights.Telemetry.CreateTelemetry();
                    lock (telemetry.GlobalSyncObject)
                    {
                        var prop = telemetry.GlobalProperties;
                        if (!prop.ContainsKey("UserDomainName"))
                        {
                            prop.Add("UserDomainName", userDomainName);
                        }
                    }

#if DEBUG
                    // デバッグ版のときの、同期送信やめる
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
