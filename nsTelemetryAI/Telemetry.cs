/* ==============================
** Copyright 2015, 2020 nishy software
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
    /// ITelemetryインターフェースを作成するAPIを公開するクラス
    /// </summary>
    public class Telemetry
    {
        /// <summary>
        /// 全体に適用するTelemetry Dataを指定するためのフラグ
        /// </summary>
        [Flags]
        public enum TelemetryDataFlag : ulong
        {
            /// <summary>
            /// NetworkType をGlobalPropertiesに追加します
            /// </summary>
            NetworkType = 1,
            /// <summary>
            /// NetworkType をGlobalMetricsに追加します
            /// </summary>
            NetworkSpeed = 2,
            /// <summary>
            /// DeviceName をGlobalPropertiesに追加します
            /// </summary>
            DeviceName = 4,
            /// <summary>
            /// Manufacture をGlobalPropertiesに追加します
            /// </summary>
            DeviceManufacturer = 8,
            /// <summary>
            /// ScreenResolution をGlobalPropertiesに追加します
            /// </summary>
            ScreenResolution = 16,
            /// <summary>
            /// Language をGlobalPropertiesに追加します
            /// </summary>
            Language = 32,
            /// <summary>
            /// ExeName をGlobalPropertiesに追加します
            /// </summary>
            ExeName = 64,
            /// <summary>
            /// HostName をGlobalPropertiesに追加します
            /// </summary>
            HostName = 128,
            /// <summary>
            /// UserName をGlobalPropertiesに追加します
            /// </summary>
            UserName = 256,

            // All / Default
            All = 0x00ffffff,
            Default = NetworkType | NetworkSpeed | DeviceName | DeviceManufacturer | ScreenResolution | Language | ExeName,
        };

        /// <summary>
        /// ITelemetryインターフェースを作成する静的関数
        /// <returns>ITelemetryインターフェース</returns>
        /// </summary>
        public static ITelemetry CreateTelemetry()
        {
            return TelemetryFactoryApplicationInsights.Create();
        }

        /// <summary>
        /// 自動的に取得するTelemetryデータを取得・設定するプロパティ
        /// </summary>
        public static TelemetryDataFlag TelemetryDataFlags
        {
            get
            {
                return TelemetryFactoryApplicationInsights.TelemetryDataFlags;
            }
            set
            {
                TelemetryFactoryApplicationInsights.TelemetryDataFlags = value;
            }
        }

        /// <summary>
        /// 強制的にDeveloperModeを設定・解除する
        /// 通常はDebuggerがアッタッチされていたら、DeveloperModeモードに設定される。
        /// DeveloperModeが有効な場合は、TrackXXX関数を呼び出したときに常に同期送信が行われる。
        /// 同期送信では、TrackXXX関数を呼び出したとき待ちが発生するため、アプリの動作が遅くなる。
        /// アプリの動作が遅くならないようにするためには、DeveloperModeを解除するとよい。
        /// <returns>old value</returns>
        /// </summary>
        public static bool? EnableDeveloperMode(bool enable)
        {
            return TelemetryFactoryApplicationInsights.EnableDeveloperMode(enable);
        }

        /// <summary>
        /// グローバルにTelemetryが有効化されているか確認する静的関数
        /// 無効化されている場合は、TrackXXXX()関数を呼び出しても、テレメトリー送信されません
        /// <returns>current value</returns>
        /// </summary>
        public static bool IsEnabled()
        {
            return TelemetryFactoryApplicationInsights.IsEnabled();
        }

        /// <summary>
        /// グローバルにTelemetryを有効化・無効化する静的関数
        /// 無効化されている場合は、TrackXXXX()関数を呼び出しても、テレメトリー送信されません
        /// <returns>old value</returns>
        /// </summary>
        public static bool Enable(bool enable)
        {
            return TelemetryFactoryApplicationInsights.Enable(enable);
        }
    }
}
