/* ==============================
** Copyright 2015 nishy software
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
            DeviceManufacturer = 8
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
    }
}
