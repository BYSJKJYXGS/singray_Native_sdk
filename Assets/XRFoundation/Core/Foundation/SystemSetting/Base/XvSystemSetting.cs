using System.Runtime.InteropServices;

namespace XvXR.Foundation
{
    /// <summary>
    //key = 2 ,state = 0 �۾�ժ��״̬
    //key = 2 ,state = 1 �۾�����״̬
    //key = 6 ,state = 0 ���
    //key = 14 1 13 3 ,state = 254 ѹ�� 255 ̧�� 
    //key = 17 18 ,state = 101 ��ת+ 99 ��ת-
    /// </summary>
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
        /// �����۾�����
        /// </summary>
        /// <param name="level">0~9���ȼ�</param>
        [DllImport("xslam-unity-wrapper")]
        public static extern void xslam_display_set_brightnesslevel(int level); //levelΪ���ȵȼ�

 
    }
}
