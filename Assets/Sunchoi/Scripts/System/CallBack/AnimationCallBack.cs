using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallBack : MonoBehaviour
{
    private Action callEventOnPlaying;
    private Action onCompleted;
    private Action onCompletedForSecondTime;
    
    /// <summary>
    /// Animation再生中のEventトリガー
    /// </summary>
    public void EventOnPlayingAnimationCallBack()
    {
        this.callEventOnPlaying?.Invoke();
    }
    
    public void EventOnPlayingAnimation(Action onFinished)
    {
        this.callEventOnPlaying = onFinished;
    }
    
    
    /// <summary>
    /// Animation再生終了トリガー
    /// </summary>
    public void EndAnimationCallBack()
    {
        this.onCompleted?.Invoke();
    }
    public void EndAnimationCallBackForSecondTime()
    {
        this.onCompletedForSecondTime?.Invoke();
    }
    
    /// <summary>
    /// Animation再生終了コールバック
    /// </summary>
    /// <param name="onCompleted"></param>
    public void EndAnimation(Action onFinished)
    {
        this.onCompleted = onFinished;
    }
    public void EndAnimationForSecondTime(Action onFinished)
    {
        this.onCompletedForSecondTime = onFinished;
    }
}
