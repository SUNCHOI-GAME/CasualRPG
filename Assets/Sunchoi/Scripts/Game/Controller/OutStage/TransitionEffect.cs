using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TransitionEffect : MonoBehaviour
{
    #region [var]

    #region [00. コンストラクタ]
    /// <summary>
    /// インスタンス
    /// </summary>
    public static TransitionEffect Instance { get; private set; }
    #endregion
    
    #region [01. アニメーション]
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
    
    #endregion
    
    #region [func]

    #region [00. コンストラクタ]
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

    #region [01. アニメーション]
    /// <summary>
    /// TransitionEffect再生：In
    /// </summary>
    public void PlayInEffect(Action onFinished)
    {
        // Image表示
        this.transitionEffectObj.SetActive(true);

        this.transitionEffectImage
            .DOFade(1f, this.timeForEffectIn)
            .OnComplete(() =>
            {
                onFinished?.Invoke();
            });
    }
    
    /// <summary>
    /// TransitionEffect再生：Out
    /// </summary>
    public void PlayOutEffect()
    {
        this.transitionEffectImage
            .DOFade(0f, this.timeForEffectOut)
            .OnComplete(() =>
            {
                // Image非表示
                this.transitionEffectObj.SetActive(false);
            });
    }
    #endregion

    #endregion
}
