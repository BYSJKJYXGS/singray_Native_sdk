using Newtonsoft.Json.Linq;
using UnityEngine;
using XvXR.utils;

public sealed class MyDebugTool
{
    private static string TAG = "wuxh:";
    
    public static bool logEnable = true;


    public static void Log(object message)
    {
        XvXRLog.LogEnable = logEnable;
        if (!logEnable)
        {
            return;
        }

        Debug.Log(TAG + message);
    }

    public static void Log(object message, Object context)
    {

        if (!logEnable)
        {
            return;
        }

        Debug.Log(TAG + message, context);
    }

    public static void LogError(object message)
    {
        if (!logEnable)
        {
            return;
        }

        Debug.LogError(TAG + message);


    }

    public static void LogError(object message, Object context)
    {
        if (!logEnable)
        {
            return;
        }

        Debug.LogError(TAG + message, context);
    }

    public static void LogWarning(object message)
    {
        if (!logEnable)
        {
            return;
        }

        Debug.LogWarning(TAG + message);
    }
    public static void LogWarning(object message, Object context)
    {
        if (!logEnable)
        {
            return;
        }

        Debug.LogWarning(TAG + message, context);
    }
}
