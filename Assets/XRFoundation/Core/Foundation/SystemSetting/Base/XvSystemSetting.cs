using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
namespace XvXR.Foundation
{
    /// <summary>/// Key = 2:/// - State = 0: Glasses removed state/// - State = 1: Glasses worn state/// Key = 6:/// - State = 0: Light sensor (triggered)/// Keys = 14, 1, 13, 3:/// - State = 254: Pressed down/// - State = 255: Released/// Keys = 17, 18:/// - State = 101: Rotate clockwise (+)/// - State = 99: Rotate counterclockwise (-)/// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct XvEvent
    {
        public double hostTimestamp;
        public long edgeTimestampUs;
        public int type;
        public int state;
    };
    public class XvSystemSetting 
    {
        /// <summary>
        /// xslam_display_set_brightnesslevel
        /// </summary>
        /// <param name="level">0~9 level</param>
        [DllImport("xslam-unity-wrapper")]
        public static extern void xslam_display_set_brightnesslevel(int level); 

        [DllImport("xslam-unity-wrapper")]
        public static extern int xslam_start_event_stream(device_stream_callback cb);

        [DllImport("xslam-unity-wrapper")]
        public static extern void xslam_stop_event_stream();

        public delegate void device_stream_callback(XvEvent xvEvent);
    }
}
