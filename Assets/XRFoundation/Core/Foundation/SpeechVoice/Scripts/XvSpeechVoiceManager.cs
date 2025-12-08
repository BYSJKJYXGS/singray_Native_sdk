using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using UnityEngine.Events;
using XvXR.utils;

namespace XvXR.Foundation
{
    public class result {
        public  string word;
        public int id;
        public int sc;
    }

    public sealed class XvSpeechVoiceManager : MonoBehaviour
    {
        private AndroidJavaObject interfaceObject;

        private AndroidJavaObject InterfaceObject
        {
            get
            {
                if (interfaceObject == null)
                {
                    AndroidJavaClass activityClass = XvAndroidHelper.GetClass("com.unity3d.player.UnityPlayer");
                    AndroidJavaObject activityObject = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                    if (activityObject != null)
                    {
                        interfaceObject = XvAndroidHelper.Create("com.xv.aitalk.UnityInterface", new object[] { activityObject });
                    }
                }
                return interfaceObject;
            }
        }

        private XvSpeechVoiceManager() { }

        private const string ENGINE_TYPE = "local";

        private string LOCAL_BNF = "#BNF+IAT 1.0 UTF-8;\n"
        + "!grammar word;\n"
        + "!slot <words>;\n"
        + "!start <words>;\n";
        private const string LOCAL_GRAMMAR = "word";

        private const string LOCAL_THRESHOLD = "60";
        private UnityAction<RecognizedStatus,string> OnRecognizedStatus;

        private result result1 = new result();

     
        [SerializeField]
        private List<AitalkWord> aitalkWords = new List<AitalkWord>();

      

        public UnityEvent<result> OnCommandRecognized;
     
        public UnityEvent OnSpeechRecognizeWake;

        public UnityEvent OnSpeechRecognizeEnd;


       

        [Tooltip("sc")]
        [Range(0,100)]
        [SerializeField]
        private int sc=20;

       


        void Start()
        {
            this.name = "Aitalk";

            StringBuilder stringBuilder = new StringBuilder("<words>:");

            for (int i = 0; i < aitalkWords.Count; i++)
            {
                if (i == aitalkWords.Count - 1)
                {
                    
                    stringBuilder.Append(string.Format("{0}!id({1});\n", aitalkWords[i].word, aitalkWords[i].id));

                }
                else
                {
                    stringBuilder.Append(string.Format("{0}!id({1})|", aitalkWords[i].word, aitalkWords[i].id));

                }
            }

            LOCAL_BNF += stringBuilder.ToString();

            MyDebugTool.Log(LOCAL_BNF);
#if UNITY_EDITOR
            return;
#endif

            //Invoke("StartAVW", 3);
        }


     
        public void StartASR(int local_VAD_BOS = 5000, int LOCAL_VAD_EOS = 500)
        {
            MyDebugTool.Log("StartASR");

            try
            { 
            
            UpdateText("startASR");
            AndroidHelper.CallObjectMethod(InterfaceObject, "init", new object[] { });
            UpdateText("init");

            AndroidHelper.CallObjectMethod(InterfaceObject, "buildGrammar", new object[] { ENGINE_TYPE, LOCAL_BNF });
            UpdateText("buildGrammar");

            AndroidHelper.CallObjectMethod(InterfaceObject, "setParam", new object[] { ENGINE_TYPE, LOCAL_GRAMMAR, LOCAL_THRESHOLD, local_VAD_BOS.ToString(),LOCAL_VAD_EOS .ToString() });
            UpdateText("setParam");

             AndroidHelper.CallObjectMethod(InterfaceObject, "setUseKeepAlive", new object[] { true });//

                AndroidHelper.CallObjectMethod(InterfaceObject, "startASR", new object[] { });
            UpdateText("startASREnd");
            }catch (Exception e)
            {

                MyDebugTool.Log("aitak_log:Exception  " + e.Message);
            }

        }


    
        public void StopASR() { 
            MyDebugTool.Log("StopASR");

            AndroidHelper.CallObjectMethod(InterfaceObject, "stopASR", new object[] { });

        }

       
        public void StartAVW(int local_VAD_BOS=5000,int LOCAL_VAD_EOS=500)
        {
            MyDebugTool.Log("StartAVW");

            AndroidHelper.CallObjectMethod(InterfaceObject, "init", new object[] { });
            //  AndroidHelper.CallObjectMethod(InterfaceObject, "buildWakeUpGrammar", new object[] { ENGINE_TYPE, LOCAL_WAKE_BNF });
            AndroidHelper.CallObjectMethod(InterfaceObject, "buildGrammar", new object[] { ENGINE_TYPE, LOCAL_BNF });
            AndroidHelper.CallObjectMethod(InterfaceObject, "setParam", new object[] { ENGINE_TYPE, LOCAL_GRAMMAR, LOCAL_THRESHOLD, local_VAD_BOS.ToString() , LOCAL_VAD_EOS .ToString()});
            AndroidHelper.CallObjectMethod(InterfaceObject, "setUseKeepAlive", new object[] { false });//
            AndroidHelper.CallObjectMethod(InterfaceObject, "startAvw", new object[] { false });//
        }

      
        public void StopAVW() {
            MyDebugTool.Log("StopAVW");
            AndroidHelper.CallObjectMethod(InterfaceObject, "stopAvw", new object[] {  });//

        }

      
        public void onAvwResult(string result)
        {
            MyDebugTool.Log("aitak_log:unity:LogInfo:onAvwResult:" + result);
            // awvResult data = SimpleJson.SimpleJson.DeserializeObject<awvResult>(result, new JsonSerializerStrategy());
          
            AndroidHelper.CallObjectMethod(InterfaceObject, "startASR", new object[] { });

            OnSpeechRecognizeWake?.Invoke();
        }

      
        public void onSpeechRecognizeEnd(string msg)
        {
            MyDebugTool.Log("aitak_log:unity:LogInfo:onSpeechRecognizeEnd:" + msg);
            OnSpeechRecognizeEnd?.Invoke();
        }

        
       

        private void UpdateText(string text)
        {
           MyDebugTool.Log(text);
            //statusText.text = text;
        }

  
        public void onInit(string result)
        {
            OnRecognizedStatus?.Invoke(RecognizedStatus.Init, result);

            MyDebugTool.Log("aitak_log:unity:LogInfo:onInit:" + result);
            UpdateText("init" + result);
        }

     
        public void onBuildFinish(string result)
        {

            MyDebugTool.Log("aitak_log:unity:LogInfo:onBuildFinish:" + result);
            string[] strArray = result.Split('|');
            if (strArray.Length > 1)
            {
                UpdateText("onBuildFinish:" + strArray[1]);
                OnRecognizedStatus?.Invoke(RecognizedStatus.BuildSuccess, result);


            }
            else
            {
                UpdateText("onBuildFinish:false");
                OnRecognizedStatus?.Invoke(RecognizedStatus.BuildFail, result);

            }

        }

      
        public void onBeginOfSpeech(string nullstr)
        {
            MyDebugTool.Log("aitak_log:unity:LogInfo:onBeginOfSpeech.....");

            OnRecognizedStatus?.Invoke(RecognizedStatus.BeginOfSpeech,nullstr);
        }

      
        public void onEndOfSpeech(string nullstr)
        {
            MyDebugTool.Log("aitak_log:unity:LogInfo:onEndOfSpeech.....");
            OnRecognizedStatus?.Invoke(RecognizedStatus.EndOfSpeech,nullstr);

        }


        public void onError(string error)
        {
            OnRecognizedStatus?.Invoke(RecognizedStatus.Error, error);

            Debug.LogError("aitak_log:unity:LogError:" + error);
            UpdateText(error);
        }



        public void onResult(string result)
        {
            OnRecognizedStatus?.Invoke(RecognizedStatus.Result, result);

            MyDebugTool.Log("aitak_log:unity:LogInfo:onResult:" + result);
            string[] strArray = result.Split('|');
            if (strArray.Length > 1)
            {

                if ("true" == strArray[1])
                {
                    try
                    {
                        XvAitalkModels.result data = SimpleJson.SimpleJson.DeserializeObject<XvAitalkModels.result>(result, new JsonSerializerStrategy());
                        if (data != null)
                        {
                            if (data.sc > sc)
                            {
                                for (int i = 0; i < aitalkWords.Count; i++)
                                {
                                    if (aitalkWords[i].id == data.ws[0].cw[0].id)
                                    {
                                        aitalkWords[i].action?.Invoke();

                                        result1.id = data.ws[0].cw[0].id;
                                        result1.word = aitalkWords[i].word;
                                        result1.sc = data.sc;

                                        OnCommandRecognized?.Invoke(result1);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        UpdateText("onResult:false");
                    }
                }
                else
                {
                    UpdateText("onResult:false");
                }
            }
            else
            {
                UpdateText("onResult:false");
            }


        }
        private class JsonSerializerStrategy : SimpleJson.PocoJsonSerializerStrategy
        {
            public override object DeserializeObject(object value, Type type)
            {
                if (type == typeof(Int32) && value.GetType() == typeof(string))
                {
                    return Int32.Parse(value.ToString());
                }
                return base.DeserializeObject(value, type);
            }
        }


    }

    [Serializable]
    public class AitalkWord
    {
        public int id;
        public string word;
        public UnityEvent action;
    }


    public enum RecognizedStatus
    {
        None,
        Init,
        BuildSuccess,
        BuildFail,
        BeginOfSpeech,
        EndOfSpeech,
        Error,
        Result,

    }
}