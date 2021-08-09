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
    public class Telemetry
    {
        /// <summary>
        /// [en] Flags to specify the Telemetry Data to be applied globally.
        /// <br/>
        /// [ja] 全体に適用するTelemetry Dataを指定するためのフラグ
        /// </summary>
        [Flags]
        public enum TelemetryDataFlag : ulong
        {
            /// <summary>
            /// [en] Add NetworkType to GlobalProperties
            /// <br/>
            /// [ja] NetworkType をGlobalPropertiesに追加します
            /// </summary>
            NetworkType = 1,

            /// <summary>
            /// [en] Add NetworkType to GlobalProperties
            /// <br/>
            /// [ja] NetworkType をGlobalMetricsに追加します
            /// </summary>
            NetworkSpeed = 2,

            /// <summary>
            /// [en] Add DeviceName to GlobalProperties
            /// <br/>
            /// [ja] DeviceName をGlobalPropertiesに追加します
            /// </summary>
            DeviceName = 4,

            /// <summary>
            /// [en] Add Manufacture to GlobalProperties
            /// <br/>
            /// [ja] Manufacture をGlobalPropertiesに追加します
            /// </summary>
            DeviceManufacturer = 8,

            /// <summary>
            /// [en] Add ScreenResolution to GlobalProperties
            /// <br/>
            /// [ja] ScreenResolution をGlobalPropertiesに追加します
            /// </summary>
            ScreenResolution = 16,

            /// <summary>
            /// [en] Add Language to GlobalProperties
            /// <br/>
            /// [ja] Language をGlobalPropertiesに追加します
            /// </summary>
            Language = 32,

            /// <summary>
            /// [en] Add ExeName to GlobalProperties
            /// <br/>
            /// [ja] ExeName をGlobalPropertiesに追加します
            /// </summary>
            ExeName = 64,

            /// <summary>
            /// [en] Add HostName to GlobalProperties
            /// <br/>
            /// [ja] HostName をGlobalPropertiesに追加します
            /// </summary>
            HostName = 128,

            /// <summary>
            /// [en] Add UserName to GlobalProperties
            /// <br/>
            /// [ja] UserName をGlobalPropertiesに追加します
            /// </summary>
            UserName = 256,

            /// <summary>
            /// [en] Add all common global properties to GlobalProperties
            /// <br/>
            /// [ja] すべての共通グローバルプロパティをGlobalPropertiesに追加します
            /// </summary>
            All = 0x00ffffff,

            /// <summary>
            /// [en] Add default common global properties to GlobalProperties
            /// <br/>
            /// [ja] 既定の共通グローバルプロパティをGlobalPropertiesに追加します
            /// </summary>
            Default = NetworkType | NetworkSpeed | DeviceName | DeviceManufacturer | ScreenResolution | Language | ExeName,
        };

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
        /// <returns>
        /// [en] old value
        /// <br/>
        /// [ja] 変更前の値
        /// </returns>
        /// </summary>
        public static bool Enable(bool enable)
        {
            return TelemetryFactoryApplicationInsights.Enable(enable);
        }
    }
}
