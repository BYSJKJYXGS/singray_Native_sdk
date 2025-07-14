using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XvXR.Engine;
using XvXR.MixedReality.Toolkit.XvXR.Input;
using XvXR.UI.Input;


namespace XvXR.Foundation.SampleScenes
{

    public class XvJoystickDemo : MonoBehaviour
    {
        public Text headsixdof;
        public Text headsixdof_rot;

        public Text realhandle;
        public Text realhandle_rot;

        public Text keyA;
        public Text keyB;
        public Text keyTrigger;
        public Text keySlide;
       // public Text keyRocker;
        public Text keyRockerValue;
       

        public Text confidence;



        public static bool ifHideJoystick = true;
        public GameObject connectHint;



        public GameObject joyStick;
        public GameObject blueTeechBtn;
        public GameObject blueTeechContent;


        private bool gazeShow = true;
        private bool handRayShow = true;
        private bool joystickRayShow = true;




        // Start is called before the first frame update
        void Start()
        {
          

            for (int i = 0; i < 20; i++)
            {
                GameObject btn = Instantiate(blueTeechBtn, Vector3.zero, Quaternion.identity);
                btn.transform.parent = blueTeechContent.transform;
                btn.transform.localPosition = new Vector3(0, 0, 0);
                btn.transform.localScale = new Vector3(1, 1, 1);
                btn.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                btn.transform.Find("id").GetComponent<Text>().text = "";
                btn.transform.Find("mac").GetComponent<Text>().text = "";
                btn.name = "Bt_" + i;
                btn.SetActive(false);
            }
        }



        public void btnClick(GameObject btn)
        {
            switch (btn.name)
            {
                case "HandleRestartBtn":
                    API.xslam_reset_slam();
                    break;

                case "ShellBtn":
                   

                    joystickRayShow = !joystickRayShow;

                    if (joystickRayShow)
                    {
                        btn.transform.GetComponentInChildren<Text>().text = "�����ֱ�";
                        PointerUtils.SetMotionControllerRayPointerBehavior(PointerBehavior.AlwaysOn);
                    }
                    else { 
                        PointerUtils.SetMotionControllerRayPointerBehavior(PointerBehavior.AlwaysOff);
                        btn.transform.GetComponentInChildren<Text>().text = "�����ֱ�";
                    }

                    MixedRealityControllerVisualizer[] mixedRealityControllerVisualizers = GameObject.FindObjectsOfType<MixedRealityControllerVisualizer>(true);

                    foreach (var item in mixedRealityControllerVisualizers)
                    {
                        item.gameObject.SetActive(joystickRayShow);
                    }

                    break;

                case "HideGaze":
                    gazeShow = !gazeShow;
                    if (XvHeadGazeInputController.Instance) {

                        if (gazeShow)
                        {
                            btn.transform.GetComponentInChildren<Text>().text = "����ͷ��";

                        }
                        else { 
                            btn.transform.GetComponentInChildren<Text>().text = "����ͷ��";

                        }


                        XvHeadGazeInputController.Instance.ShowOrHidePointer(gazeShow);
                    }
                    break;

                case "HandsRay":
                    handRayShow = !handRayShow;

                    if (handRayShow)
                    {
                        btn.transform.GetComponentInChildren<Text>().text="��������";
                        PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOn);
                    }
                    else {
                        btn.transform.GetComponentInChildren<Text>().text = "��������";

                        PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);
                    }

                    MixedRealityInputSystemProfile inputSystemProfile = CoreServices.InputSystem?.InputSystemProfile;
                    if (inputSystemProfile == null)
                    {
                        return;
                    }

                    MixedRealityHandTrackingProfile handTrackingProfile = inputSystemProfile.HandTrackingProfile;
                    if (handTrackingProfile != null)
                    {
                        handTrackingProfile.EnableHandMeshVisualization = handRayShow;
                    }

                    XvDeviceManager.Manager.ChangeGetureStatus(handRayShow);

                    break;

                    

            }
        }

     

        private void clearContain()
        {
            int childCount = blueTeechContent.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                blueTeechContent.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        private void updateBlueTeech()
        {
            clearContain();
            int index = 0;
            List<bleInfo> bleInfoList = XvJoystickManager.Instance.GetBleInfo(TrackerType.Right);

          string serialNumber= XvJoystickManager.Instance.GetSerialNumber(TrackerType.Right);
            for (int i = 0; i < bleInfoList.Count; i++)
            {

                GameObject btn = blueTeechContent.transform.GetChild(index).gameObject;
                btn.SetActive(true);
                btn.transform.Find("id").GetComponent<Text>().text = bleInfoList[i].id;
                btn.transform.Find("mac").GetComponent<Text>().text = bleInfoList[i].mac;
                btn.GetComponent<BlueTeethControl>().id = bleInfoList[i].id;
                btn.GetComponent<BlueTeethControl>().mac = bleInfoList[i].mac;
                btn.name = "Bt_" + i;

                if (serialNumber== bleInfoList[i].serialNumber) {
                    if (bleInfoList[i].status == 0)
                    {
                        MyDebugTool.Log("�Զ����ӣ�" + serialNumber);
                        XvJoystickManager.Instance.ConnectXvBle(TrackerType.Right, bleInfoList[i].id, bleInfoList[i].mac);
                    }
                }

                if (bleInfoList[i].status == 1)
                {
                    btn.transform.Find("mac").GetComponent<Text>().text = "<color=red>�����Ӵ��ֱ�</color>";
                    btn.GetComponent<BlueTeethControl>().state = 1;
                }
                index++;
            }

            //for (int i = 0; i < bleInfoList.Count; i++)
            //{

            //    GameObject btn = blueTeechContent.transform.GetChild(index).gameObject;
            //    btn.SetActive(true);
            //    btn.transform.Find("id").GetComponent<Text>().text = bleInfoList[i].id;
            //    btn.transform.Find("mac").GetComponent<Text>().text = bleInfoList[i].mac;
            //    btn.GetComponent<BlueTeethControl>().id = bleInfoList[i].id;
            //    btn.GetComponent<BlueTeethControl>().mac = bleInfoList[i].mac;
            //    btn.name = "Bt_" + i;

            //    if (bleInfoList[i].status == 1)
            //    {
            //        btn.transform.Find("mac").GetComponent<Text>().text = "<color=red>�����Ӵ��ֱ�</color>";
            //        btn.GetComponent<BlueTeethControl>().state = 1;
            //    }
            //    index++;
            //}

        }

        public void blueTeechConnect(GameObject btn)
        {

            string id = btn.transform.GetComponent<BlueTeethControl>().id;
            string mac = btn.transform.GetComponent<BlueTeethControl>().mac;

            MyDebugTool.Log("blueTeechConnect" + id + "   " + mac);

            if (btn.GetComponent<BlueTeethControl>().state == 0)
            {
               XvJoystickManager.Instance.ConnectXvBle(TrackerType.Right,id, mac);
            }
            else
            {
                XvJoystickManager.Instance.DisConnectXvBle(TrackerType.Right, id, mac);
            }

        }

        

        // Update is called once per frame
        void Update()
        {

            if (headsixdof != null)
            {
                headsixdof.text = $"�۾�pos: {Math.Round(XvXRManager.SDK.HeadPose.Position.x, 5)} , {Math.Round(XvXRManager.SDK.HeadPose.Position.y, 5)} , {Math.Round(XvXRManager.SDK.HeadPose.Position.z, 5)}";
            }
            if (headsixdof_rot != null)
            {
                headsixdof_rot.text = $"�۾�rot: {XvXRManager.SDK.HeadPose.Orientation.eulerAngles}";
            }

            Vector3 pos = XvJoystickManager.Instance.GetPosition(TrackerType.Right);
            Quaternion rot = XvJoystickManager.Instance.GetRotation(TrackerType.Right);


            if (realhandle != null)
            {
                realhandle.text = $"��ʵ�ֱ�pos: {Math.Round(pos.x, 5)} , {Math.Round(pos.y, 5)} , {Math.Round(pos.z, 5)}";
            }
            if (realhandle_rot != null)
            {
                realhandle_rot.text = $"��ʵ�ֱ�rot: {rot}";
            }


            if (keyTrigger != null)
            {
                keyTrigger.text = $"keyTrigger: {XvJoystickManager.Instance.GetKey(JoystickButton.Button_Trigger,TrackerType.Right)}";
            }
            if (keyA != null)
            {
                keyA.text = $"keyA: {XvJoystickManager.Instance.GetKey(JoystickButton.Button_A,TrackerType.Right)}";
            }
            if (keyB != null)
            {
                keyB.text = $"keyB: {XvJoystickManager.Instance.GetKey(JoystickButton.Button_B,TrackerType.Right)}";
            }
            if (keySlide != null)
            {
                keySlide.text = $"keySlide: {XvJoystickManager.Instance.GetKey(JoystickButton.Button_Grip,TrackerType.Right)}";
            }

            //if (keyRocker != null)
            //{
            //    keyRocker.text = $"keyRocker: {XvJoystickManager.Instance.GetJoystickData(TrackerType.Right).keyRocker}";
            //}

            if (keyRockerValue != null)
            {
                keyRockerValue.text = $"keyRockerValue: {XvJoystickManager.Instance.GetRockerVector2(TrackerType.Right)}";
            }
            if (confidence != null)
            {
                confidence.text = $"confidence: {XvJoystickManager.Instance.GetConfidence(TrackerType.Right)}";
            }
           


            if (XvJoystickManager.Instance.IsConnected(TrackerType.Right))
            {
                if (connectHint != null)
                {
                    connectHint.SetActive(true);
                }
            }
            else
            {
                if (connectHint != null)
                    connectHint.SetActive(false);
            }

            if (Time.frameCount%30==0) {
                updateBlueTeech();
            }

          
        }

    }

}