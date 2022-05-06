using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class GlobalCoroutine
{
    private static readonly CoroutineManager coroutineManager;
    
    /// <summary>
    /// コンストラクタ
    /// </summary>
    static GlobalCoroutine()
    {
        var obj = new GameObject("CoroutineManager");
        coroutineManager = obj.AddComponent<CoroutineManager>();
        GameObject.DontDestroyOnLoad(coroutineManager);
    }

    /// <summary>
    /// コルーチン再生
    /// </summary>
    /// <param name="e"></param>
    /// <param name="key"></param>
    /// <param name="onCompleted"></param>
    public static void Play(IEnumerator e, string key = "", Action onCompleted = null)
    {
        coroutineManager.Play(e, key, onCompleted);
    }

    /// <summary>
    /// コルーチン停止
    /// </summary>
    /// <param name="key"></param>
    public static void Stop(string key)
    {
        coroutineManager.Stop(key);
    }
}
