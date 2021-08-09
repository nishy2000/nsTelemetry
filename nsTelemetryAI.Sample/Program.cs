/* ==============================
** Copyright 2020, 2021 nishy software
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
                    // [en] Setting TelemetryDataFlags takes time the first time, so set it asynchronously.
                    // [ja] TelemetryDataFlags は、初回は時間がかかるので、非同期で設定する。
                    Task.Run(() =>
                    {
                        // [en] Setup common global properties
                        // [ja] 共通グローバルプロパティをセットアップする
                        NishySoftware.Telemetry.ApplicationInsights.Telemetry.TelemetryDataFlags = NishySoftware.Telemetry.ApplicationInsights.Telemetry.TelemetryDataFlag.All;
                    });

                    // [en] Create an instance of the telemetry interface
                    // [ja] テレメトリーインターフェースのインスタンスを作成する
                    var telemetry = NishySoftware.Telemetry.ApplicationInsights.Telemetry.CreateTelemetry();

                    // [en] Add custom global property if you need
                    // [ja] 必要なカスタムグローバルプロパティを追加する
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
                    // [en] For the debug version, use synchronous transmission.
                    // [ja] デバッグ版のときに、非同期送信を利用する
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

#if DEBUG
            Console.WriteLine("Wait for the debugger to give you a chance to attach.\nType a new line to proceed.");
            var wait = Console.ReadLine();
#endif

            program.TrackEventStartup(args);

            Console.WriteLine("Hello World!");

            program.TrackEventExit(Environment.ExitCode);
        }
    }
}
