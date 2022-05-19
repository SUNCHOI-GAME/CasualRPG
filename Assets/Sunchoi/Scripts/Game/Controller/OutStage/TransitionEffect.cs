using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TransitionEffect : MonoBehaviour
{
    #region [01. コンストラクタ]

    #region [var]
    /// <summary>
    /// インスタンス
    /// </summary>
    public static TransitionEffect Instance { get; private set; }
    #endregion
    
    
    #region [func]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Start()
    {
        // インスタンス
        Instance = this;

        // 初期化
        this.transitionEffectObj.SetActive(false);
    }
    #endregion

    
    
    #endregion
    
    #region [02. アニメーション]

    #region [var]
    /// <summary>
    /// TransitionEffectのGameObject
    /// </summary>
    [SerializeField]
    private GameObject transitionEffectObj;
    /// <summary>
    /// TransitionEffectのImage
    /// </summary>
    [SerializeField]
    private Image transitionEffectImage;
    /// <summary>
    /// Fade時間
    /// </summary>
    [SerializeField]
    private float timeForEffectIn = 0.8f;
    [SerializeField]
    private float timeForEffectOut = 0.5f;
    
    #endregion


    #region [func]
    /// <summary>
    /// TransitionEffect再生
    /// </summary>
    public void PlayEffectIn(Action OnFinished)
    {
        // Image表示
        this.transitionEffectObj.SetActive(true);

        this.transitionEffectImage
            .DOFade(1f, this.timeForEffectIn)
            .OnComplete(() =>
            {
                OnFinished?.Invoke();
            });
    }
    public void PlayEffectOut(Action OnFinished)
    {
        this.transitionEffectImage
            .DOFade(0f, this.timeForEffectOut)
            .OnComplete(() =>
            {
                // Image非表示
                this.transitionEffectObj.SetActive(false);
                
                OnFinished?.Invoke();
            });
    }
    #endregion

    #endregion
}
