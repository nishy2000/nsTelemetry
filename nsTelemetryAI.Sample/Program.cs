/* ==============================
** Copyright 2020, 2021, 2023 nishy software
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
                        NishySoftware.Telemetry.ApplicationInsights.Telemetry.CommonDataKinds = TelemetryDataKinds.All;
                    });

                    // [en] If you do not place the InstrumentationKey in ApplicationInsights.config file, setup the InstrumentationKey using SetInstrumentationKey().
                    // [ja] ApplicationInsights.configファイルにInstrumentationKeyを配置していない場合は、SetInstrumentationKey()を使用してInstrumentationKeyを設定します。
                    // NishySoftware.Telemetry.ApplicationInsights.Telemetry.SetInstrumentationKey("your InstrumentationKey");

                    // [en] Create an instance of the telemetry interface
                    // [ja] テレメトリーインターフェースのインスタンスを作成する
                    var telemetry = NishySoftware.Telemetry.ApplicationInsights.Telemetry.CreateTelemetry();

                    // [en] Add custom global property if you need
                    // [ja] 必要ならカスタムグローバルプロパティを追加する
                    var userDomainName = Environment.UserDomainName;
                    lock (telemetry.GlobalSyncObject)
                    {
                        var prop = telemetry.GlobalProperties;
                        if (!prop.ContainsKey("UserDomainName"))
                        {
                            prop.Add("UserDomainName", userDomainName);
                        }
                    }

                    // [en] When the debugger is attached to the process, the default of developer mode is true 
                    //      Otherwise, the developer mode default is false.
                    //      When developer mode is true, transmission mode default is synchronous mode.
                    //      Otherwise, transmission mode default is asynchronous mode.
                    //      For console apps where the process terminates in a short period of time, asynchronous communication is not enough to send the data by the time the app terminates, so true is recommended.
                    // [ja] デバッガーがアタッチされているとき、開発者モードの既定値はtrueです。
                    //      デバッガーがアタッチされていないとき、開発者モードの既定値はfalseです。
                    //      開発者モードがtrueのとき、送信モードの既定値は同期送信です。
                    //      開発者モードがfalseのとき、送信モードの既定値は非同期送信です。
                    //      短時間でプロセスが終了するようなコンソールアプリでは、非同期通信を使うとアプリの終了時までに送信しきれないため、trueを推奨します。
#if DEBUG
                    // [en] For the debug version, always use asynchronous transmission if you need.
                    // [ja] 必要なら、デバッグ版のときに、常に非同期送信を利用する
                    //NishySoftware.Telemetry.ApplicationInsights.Telemetry.EnableDeveloperMode(false);
#endif
#if !DEBUG
                    // [en] For the release version, always use synchronous transmission if you need.
                    // [ja] 必要なら、リリース版のときに、常に同期送信を利用する。
                    NishySoftware.Telemetry.ApplicationInsights.Telemetry.EnableDeveloperMode(true);
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
                // [en] Wait a bit after calling Telemetry.Flush()
                // [ja] Telemetry.Flush()を呼び出した後は少し待ちます。
                Thread.Sleep(1000);
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

#if DEBUG
            Console.WriteLine("Wait for the debugger to give you a chance to attach.\nType a new line to proceed.");
            var wait2 = Console.ReadLine();
#endif
            program.TrackEventExit(Environment.ExitCode);
        }
    }
}
