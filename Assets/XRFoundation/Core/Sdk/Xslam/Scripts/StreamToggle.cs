using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class StreamToggle : MonoBehaviour {
    public Toggle toggleRgb;
    public Toggle toggleTof;
    public Toggle toggleStereo;
    public Toggle toggleGesture;

    void Start()
    {
     
        //toggleRgb.isOn = XvXR.Engine.XvDeviceManager.Manager.needStartRgb;
        //toggleTof.isOn = XvXR.Engine.XvDeviceManager.Manager.needStartTof;
        //toggleStereo.isOn = XvXR.Engine.XvDeviceManager.Manager.needStartStereo;
        toggleGesture.isOn = XvXR.Engine.XvDeviceManager.Manager.needStartGesture;
        toggleRgb.onValueChanged.AddListener(OnValueChangedRgb);
        toggleTof.onValueChanged.AddListener(OnValueChangedTof);
        toggleStereo.onValueChanged.AddListener(OnValueChangedStereo);
        toggleGesture.onValueChanged.AddListener(OnValueChangedGesture);
    }

    void OnValueChangedRgb(bool check)
    {
        MyDebugTool.Log("OnValueChangedRgb " + check);
        //bool res = XvXR.Engine.XvDeviceManager.Manager.ChangeRgbStatus(check);
        //if(!res&&check){
        //    toggleRgb.isOn = false;
        //}
    }

    void OnValueChangedTof(bool check)
    {
        MyDebugTool.Log("OnValueChangedTof " + check);
        //bool res = XvXR.Engine.XvDeviceManager.Manager.ChangeTofStatus(check);
        //if(!res&&check){
        //    toggleTof.isOn = false;
        //}
    }

    void OnValueChangedStereo(bool check)
    {
        MyDebugTool.Log("OnValueChangedStereo " + check);
        // bool res = XvXR.Engine.XvDeviceManager.Manager.ChangeStereoStatus(check);
        //if(!res&&check){
        //    toggleStereo.isOn = false;
        //}
    }

    void OnValueChangedGesture(bool check)
    {
        //Debug.Log("OnValueChangedGesture " + check);
        // bool res = XvXR.Engine.XvDeviceManager.Manager.ChangeRgbStatus(check);
        //if(!res&&check){
        //    toggleGesture.isOn = false;
        //}
       
    }

 

    // public bool RgbOn()
    // {
    //     return toggleRgb.isOn;
    // }

    // public bool TofOn()
    // {
    //     return toggleTof.isOn;
    // }

    // public bool StereoOn()
    // {
    //     return toggleStereo.isOn;
    // }

}
