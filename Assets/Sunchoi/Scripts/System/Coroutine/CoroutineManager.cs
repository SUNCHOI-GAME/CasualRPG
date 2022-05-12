using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class CoroutineManager : MonoBehaviour
{
    #region [var]
    /// <summary>
    /// 再生済みコルーチンの格納Dictionary
    /// </summary>
    private Dictionary<string, Coroutine> dictionary = new Dictionary<string, Coroutine>();
    /// <summary>
    /// 再生中コルーチンを格納するリスト
    /// </summary>
    [NonReorderable, SerializeField]
    private List<string> playingCoroutineList = new List<string>();
    #endregion

    #region [func]
    /// <summary>
    /// コルーチンの一時的格納関数
    /// </summary>
    /// <param name="e"></param>
    /// <param name="onCompleted"></param>
    /// <returns></returns>
    private IEnumerator TempCoroutine(IEnumerator e, Action onCompleted = null)
    {
        yield return e;
        onCompleted?.Invoke();
    }
    
    /// <summary>
    /// コルーチン再生
    /// </summary>
    /// <param name="e"></param>
    /// <param name="onCompleted"></param>
    /// <param name="key"></param>
    internal void Play(IEnumerator e, string key = "", Action onCompleted = null)
    {
        // Keyチェック
        var isCache = !string.IsNullOrEmpty(key) && !this.dictionary.ContainsKey(key);
        if (isCache)
        {
            onCompleted += () =>
            {
                if (this.dictionary.ContainsKey(key))
                    this.dictionary.Remove(key);
            };
        }

        // コルーチン再生
        var coroutine = StartCoroutine(TempCoroutine(e, onCompleted));
        if (isCache)
        {
            // 再生中のコルーチンをDictionaryに追加
            this.dictionary.Add(key, coroutine);
            // 再生中コルーチンのKeyをリストに追加
            this.playingCoroutineList.Add(key);
        }
            
    }
    
    /// <summary>
    /// コルーチン停止
    /// </summary>
    /// <param name="key"></param>
    internal void Stop(string key)
    {
        // Keyチェック
        if (!this.dictionary.ContainsKey(key))
        {
            if (this.playingCoroutineList.Contains(key))
            {
                Debug.LogFormat($"List Has Key,  [ {key} ]. It will be removed Automatically.", DColor.yellow);
            
                // Keyをリストから除去
                this.playingCoroutineList.Remove(key);
                return;
            }
            
            Debug.LogFormat($"Coroutine Key [ {key} ] is not found", DColor.yellow);
            return;
        }
        
        // コルーチン停止
        StopCoroutine(this.dictionary[key]);
        
        // 再生中のコルーチンをDictionaryから除去
        this.dictionary[key] = null;
        this.dictionary.Remove(key);

        // 再生中コルーチンのKeyをリストから除去
        this.playingCoroutineList.Remove(key);
    }
    #endregion
}