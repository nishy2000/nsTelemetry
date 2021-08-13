/* ==============================
** Copyright 2015, 2020, 2021 nishy software
**
**      First Author : nishy software
**		Create : 2015/12/07
** ============================== */

namespace NishySoftware.Telemetry.ApplicationInsights
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// [en] A class that exposes an API to create an instance of the <see cref="T:NishySoftware.Telemetry.ITelemetry"/> interface.
    /// <br/>
    /// [ja] <see cref="T:NishySoftware.Telemetry.ITelemetry"/> インターフェースのインスタンスを作成するAPIを公開するクラス
    /// </summary>
    public sealed class Telemetry
    {
        /// <summary>
        /// [en] Static function to create the <see cref="T:NishySoftware.Telemetry.ITelemetry"/> interface
        /// <br/>
        /// [ja] <see cref="T:NishySoftware.Telemetry.ITelemetry"/> インターフェースを作成する静的関数
        /// </summary>
        /// <returns>
        /// [en] <see cref="T:NishySoftware.Telemetry.ITelemetry"/> interface
        /// <br/>
        /// [ja] <see creaf="T:NishySoftware.Telemetry.ITelemetry"/> インターフェース
        /// </returns>
        public static ITelemetry CreateTelemetry()
        {
            return TelemetryFactoryApplicationInsights.Create();
        }

        /// <summary>
        /// [en] Property that specifies the common telemetry data to be applied to all telemetry transmissions.
        /// <br/>
        /// [ja] すべてのテレメトリー送信に適用する共通Telemetryデータを指定するプロパティ
        /// </summary>
        public static TelemetryDataKinds CommonDataKinds
        {
            get
            {
                return TelemetryFactoryApplicationInsights.CommonDataKinds;
            }
            set
            {
                TelemetryFactoryApplicationInsights.CommonDataKinds = value;
            }
        }

        /// <summary>
        /// [en] Set and reset DeveloperMode.
        /// <br/>
        /// [ja] DeveloperModeを設定・解除する
        /// </summary>
        /// <remarks>
        /// [en] This library is automatically set to DeveloperMode mode when the Debugger is attached.
        /// When DeveloperMode is enabled, synchronous transmission is always performed when the <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX() function is called.
        /// Synchronous sending slows down the application because there is a wait until the sending is completed when the <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX() function is called.
        /// To prevent the app from running slowly during debugging, disable DeveloperMode.
        /// <br/>
        /// [ja] このライブラリーはDebuggerがアッタッチされていたら、自動的にDeveloperModeモードに設定します。
        /// DeveloperModeが有効な場合は、<see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX()関数を呼び出したときに常に同期送信が行われます。
        /// 同期送信では、<see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX()関数を呼び出したときに送信完了するまでの待ちが発生するため、アプリの動作が遅くなります。
        /// デバッグ時にアプリの動作が遅くならないようにするためには、DeveloperModeを解除します。
        /// </remarks>
        /// <returns>old value</returns>
        public static bool? EnableDeveloperMode(bool enable)
        {
            return TelemetryFactoryApplicationInsights.EnableDeveloperMode(enable);
        }

        /// <summary>
        /// [en] Static function to check if Telemetry is enabled
        /// If disabled, calling the <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX() function will not send telemetry.
        /// <br/>
        /// [ja] Telemetryが有効化されているか確認する静的関数
        /// 無効化されている場合は、<see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX()関数を呼び出しても、テレメトリー送信されません
        /// </summary>
        /// <returns>
        /// [en] current value
        /// <br/>
        /// [ja] 現在の値
        /// </returns>
        public static bool IsEnabled()
        {
            return TelemetryFactoryApplicationInsights.IsEnabled();
        }

        /// <summary>
        /// [en] Static function to enable/disable Telemetry
        /// If disabled, calling the <see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX() function will not send telemetry.
        /// <br/>
        /// [ja] Telemetryを有効化・無効化する静的関数
        /// 無効化されている場合は、<see cref="T:NishySoftware.Telemetry.ITelemetry"/>.TrackXXXX()関数を呼び出しても、テレメトリー送信されません
        /// </summary>
        /// <returns>
        /// [en] old value
        /// <br/>
        /// [ja] 変更前の値
        /// </returns>
        public static bool Enable(bool enable)
        {
            return TelemetryFactoryApplicationInsights.Enable(enable);
        }

        /// <summary>
        /// [en] Setup the InstrumentationKey.
        /// Setup your InstrumentationKey using this method if you do not place it in the ApplicationInsights.config file.
        /// <br/>
        /// [ja] InstrumentationKeyを設定します。
        /// ApplicationInsights.configファイルにInstrumentationKeyを設定ていない場合は、このメソッドでInstrumentationKeyを設定します。
        /// </summary>
        /// <param name="instrumentationKey">[en] InstrumentationKey of Application Insights resource<br/> [ja] Application InsightsリソースのInstrumentationKey</param>
        public static void SetInstrumentationKey(string instrumentationKey)
        {
            TelemetryFactoryApplicationInsights.SetInstrumentationKey(instrumentationKey);
        }
    }
}
