using UnityEngine;

public enum DColor
{
    white,
    grey,
    black,
    red,
    green,
    blue,
    yellow,
    cyan,
    brown,
}

public static class Debug
{
    public static bool isDebugBuild { get { return UnityEngine.Debug.isDebugBuild; } }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Log(object message, UnityEngine.Object context) => UnityEngine.Debug.Log(message, context);
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Log(object message) => UnityEngine.Debug.Log(message);
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogFormat(string format, params object[] args) => UnityEngine.Debug.LogFormat(format, args);
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogFormat(string text, DColor color) => UnityEngine.Debug.LogFormat("<color={0}>{1}</color>",color.ToString(), text);
    //public static void LogFormat(string text, Color)
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogFormat(Object context, string format, params object[] args) => UnityEngine.Debug.LogFormat(context, format, args);
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogError(object message) => UnityEngine.Debug.LogError(message);
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogErrorFormat(string format, params object[] args) => UnityEngine.Debug.LogErrorFormat(format, args);
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogWarning(object message) => UnityEngine.Debug.LogWarning(message);
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogWarning(object message, UnityEngine.Object context) => UnityEngine.Debug.LogWarning(message, context);
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogWarningFormat(string format, params object[] args) => UnityEngine.Debug.LogWarningFormat(format, args);
}
