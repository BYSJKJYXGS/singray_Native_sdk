using System.Runtime.InteropServices;
namespace XvXR.Foundation
{
    public class XvEyeTracking 
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public float x;
        public float y;
        public float z;
    };
    [StructLayout(LayoutKind.Sequential)]
    public struct GazePoint
    {

        public uint gazeBitMask;
        public Point gazePoint;
        public Point rawPoint;
        public Point smoothPoint;
        public Point gazeOrigin;
        public Point gazeDirection;
        public float re;
        public uint exDataBitMask;
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct XV_ET_EYE_DATA_EX
    {
        public ulong timestamp;   //!<timestamp.
        public int recommend;    //!<whether if there has the recommend point. 0-no recommend point, 1-use left eye as recommend point, 2-use right eye as recomment point.
        public XV_ET_GAZE_POINT recomGaze; //!<recommend gaze data
        public XV_ET_GAZE_POINT leftGaze;  //!<left eye gaze data
        public XV_ET_GAZE_POINT rightGaze; //!<right eye gaze data

        public XV_ET_PUPIL_INFO leftPupil;  //!<left eye pupil data
        public XV_ET_PUPIL_INFO rightPupil; //!<right eye pupil data

        public XV_ET_EYE_EXDATA leftExData;  //!<left eye extend data(include blink and eyelid data)
        public XV_ET_EYE_EXDATA rightExData; //!<right eye extend data(include blink and eyelid data)

        public int leftEyeMove;//!<0-Eye movement type is no-eye detected. 1-Eye movement type is blink. 2-Eye movement type is noraml.
        public int rightEyeMove;//!<0-Eye movement type is no-eye detected. 1-Eye movement type is blink. 2-Eye movement type is noraml.
        public float ipd; //ͫ�����ݣ����۶�У׼�����ݲŻ���Ч
    };
    [StructLayout(LayoutKind.Sequential)]
    public struct XV_ET_GAZE_POINT
    {
        public uint gazeBitMask;               //!<gaze bit mask, identify the six data below are valid or invalid.
        public XV_ETPoint3D gazePoint;     //!<gaze point, x and y are valid, z default value is 0, x and y scope are related to the input calibration point, not fixed.
        public XV_ETPoint3D rawPoint;      //!<gaze point before smooth, x and y are valid, z default value is 0, x and y scope are as above.
        public XV_ETPoint3D smoothPoint;   //!<gaze point after smooth, x and y are valid, z default value is 0, x and y scope are as above.
        public XV_ETPoint3D gazeOrigin;    //!<origin gaze center coordinate.
        public XV_ETPoint3D gazeDirection; //!<gaze direction.
        public float re;                       //!<gaze re value, confidence level.
        public uint exDataBitMask;             //!<reserved data.
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.R4)]
        public float[] exData;                 //!<reserved data. 32
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct XV_ETPoint3D
    {
        public float x;
        public float y;
        public float z;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct XV_ET_PUPIL_INFO
    {
        public uint pupilBitMask;            //!<pupil bit mask, identify the six data below are valid or invalid.
        public xv_ETPoint2D pupilCenter; //!<pupil center(0-1), the coordinate value of pupil center in the image, normalization value, image height and width is 1.
        public float pupilDistance;          //!<the distance between pupil and camera(mm)
        public float pupilDiameter;          //!<pupil diameter, pupil long axis value(0-1), the ratio of the pixel value of the long axis size of the pupil ellipse to the image width, normalization value.
        public float pupilDiameterMM;        //!<pupil diameter, pupil long axis value(mm).
        public float pupilMinorAxis;         //!<pupil diameter, pupil minor axis value(0-1), the ratio of the pixel value of the minor axis size of the pupil ellipse to the image width, normalization value.
        public float pupilMinorAxisMM;       //!<pupil diameter, pupil minor axis value(mm).
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct xv_ETPoint2D
    {
        public float x;
        public float y;
    };



    [StructLayout(LayoutKind.Sequential)]
    public struct XV_ET_EYE_EXDATA
    {
        public uint eyeDataExBitMask;         //!<eye extend data bit mask, identify the four data below are valid or invalid.
        public int blink;                     //!<blink data, 0-no blink, 1-start blinking, 2-closing process, 3-close eyes, 4-opening process, 5-finish blinking.
        public float openness;                //!<eye openness(0-100), 0-cloing, 100-opening normally, >100-opening on purpose.
        public float eyelidUp;                //!<up eyelid data(0-1), up eyelid's vertical position in the image, normalization value, image height is 1.
        public float eyelidDown;              //!<down eyelid data(0-1), down eyelid's vertical position in the image, normalization value, image height is 1.
    };

    public delegate void fn_gaze_callback(XV_ET_EYE_DATA_EX gazedata);



        //�۶��ӿ�
        [DllImport("xslam-unity-wrapper")]
        public static extern void pub_set_usr_eye_ready();

    [DllImport("xslam-unity-wrapper")]
    public static extern void xslam_set_gaze_configs(int width, int height);

    [DllImport("xslam-unity-wrapper", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void xslam_gaze_set_config_path(string coe_path);

    [DllImport("xslam-unity-wrapper", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int xslam_gaze_calibration_apply(string file);//���ڸ���֮ǰ�����ɵ�ָ��У׼�ļ�

    [DllImport("xslam-unity-wrapper")]
    public static extern bool xslam_start_gaze();
    [DllImport("xslam-unity-wrapper")]
    public static extern bool xslam_stop_gaze();
    [DllImport("xslam-unity-wrapper")]
    public static extern bool xslam_set_exposure(int leftGain, float leftTimeMs, int rightGain, float rightTimeMs);
    [DllImport("xslam-unity-wrapper")]
    public static extern int xslam_set_gaze_callback(fn_gaze_callback cb);
    [DllImport("xslam-unity-wrapper")]
    public static extern bool xslam_unset_gaze_callback();

    [DllImport("xslam-unity-wrapper")]
    public static extern bool xslam_set_bright(int eye, int led, int brightness);



    //�ۿ���ؽӿ�
    [StructLayout(LayoutKind.Sequential)]
    public struct GazeCalibStatus
    {
        /** The status of API CalibrationEnter */
        public int enter_status;

        /** The status of API CalibrationCollect */
        public int collect_status;

        /** The status of API CalibrationSetup */
        public int setup_status;

        /** The status of API CalibrationComputeApply */
        public int compute_apply_status;

        /** The status of API CalibrationLeave */
        public int leave_status;

        /** The status of API CalibrationReset */
        public int reset_status;
    };

    [DllImport("xslam-unity-wrapper")]
    public static extern void xslam_gaze_calibration_enter();//����У׼ģʽ

    [DllImport("xslam-unity-wrapper")]
    public static extern int xslam_gaze_calibration_leave();//�뿪У׼ģʽ��У׼�������ʱ����

    [DllImport("xslam-unity-wrapper")]
    public static extern int xslam_gaze_calibration_collect(float x, float y, float z, int index);//У׼5��У׼��λ

    [DllImport("xslam-unity-wrapper", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int xslam_gaze_calibration_retrieve(string file);//��ȡУ׼������ݲ��洢���Զ����ļ�



    [DllImport("xslam-unity-wrapper")]
    public static extern int xslam_gaze_calibration_reset();//������вɼ�����У׼�㣬��������У׼����

    [DllImport("xslam-unity-wrapper")]
    public static extern int xslam_gaze_calibration_compute_apply();//����У׼������Ӧ�����豸

    [DllImport("xslam-unity-wrapper")]
    public static extern int xslam_gaze_calibration_setup();//У׼��������

    [DllImport("xslam-unity-wrapper")]
    public static extern int xslam_gaze_calibration_query_status(ref GazeCalibStatus status);//��ѯУ׼�����ڲ�״̬



        //��ȡ�۶�ͼ��ӿ�
        [DllImport("xslam-unity-wrapper")]
        public static extern bool xv_eyetracking_start();

        [DllImport("xslam-unity-wrapper")]
        public static extern bool xv_eyetracking_stop();

        [DllImport("xslam-unity-wrapper")]
        public static extern bool xv_eyetracking_get_rgba(System.IntPtr left, System.IntPtr right, ref int width, ref int height);
    }
}