using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    #region [var]

    #region [00. コンストラクタ]
    /// <summary>
    /// インスタンス
    /// </summary>
    public static TitleController Instance { get; private set; }
    #endregion

    #region [01. UI表示]
    [Header(" --- Common")]
    /// <summary>
    /// BackgroundのGameObject
    /// </summary>
    [SerializeField]
    private GameObject background;
    /// <summary>
    /// CurtainのGameObject
    /// </summary>
    [SerializeField]
    private GameObject curtain;
    
    [Header(" --- Title Screen")]
    /// <summary>
    /// TitleScreenのGameObject
    /// </summary>
    [SerializeField]
    private GameObject titleScreen;
    
    [Header(" --- Stage Info")]
    /// <summary>
    /// StageInfoのGameObject
    /// </summary>
    [SerializeField]
    private GameObject stageInfo;
    /// <summary>
    /// StageGourpのGameObject
    /// </summary>
    [SerializeField]
    private GameObject stageGourp;
    /// <summary>
    /// Stageのリスト
    /// </summary>
    [SerializeField]
    private List<GameObject> stageList = new List<GameObject>();
    /// <summary>
    /// StagePopUp時のアニメーションパターン
    /// </summary>
    [SerializeField]
    private Ease stagePopupEase;
    /// <summary>
    /// StageSlide時のアニメーションパターン
    /// </summary>
    [SerializeField]
    private Ease stageSlideEase;

    /// <summary>
    /// スキップボタン押下是非のトリガー
    /// </summary>
    private bool isSkipped = false;
    
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
    }

    #endregion

    #region [01. UI表示]
    /// <summary>
    /// Title表示
    /// </summary>
    public void ShowTitle()
    {
        this.background.SetActive(true);
        this.curtain.SetActive(true);
        
        this.SetTitleScreen(true);

        DOVirtual.DelayedCall(1f, () =>
        {
            // TitleCurtainのフェードイン
            this.TitleCurtainOff(() => { });
        });
    }

    /// <summary>
    /// Backgroundの表示切り替え
    /// </summary>
    /// <param name="state"></param>
    public void SetBackground(bool state)
    {
        this.background.SetActive(state);
    }
    
    /// <summary>
    /// Titleの表示切り替え
    /// </summary>
    /// <param name="state"></param>
    public void SetTitleScreen(bool state)
    {
        this.titleScreen.SetActive(state);
    }
    
    /// <summary>
    /// StageInfoの表示切り替え
    /// </summary>
    /// <param name="state"></param>
    public void SetStageInfo(bool state)
    {
         this.stageInfo.SetActive(state);
    }

    /// <summary>
    /// TitleCurtainのフェードイン
    /// </summary>
    /// <param name="onFinished"></param>
    private void TitleCurtainOff(Action onFinished)
    {
        this.curtain.GetComponent<Image>()
            .DOFade(0f, 1.5f)
            .OnComplete(() =>
            {
                this.curtain.SetActive(false);
                
                onFinished?.Invoke();
            });
    }
    
    /// <summary>
    /// TitleCurtainのフェードアウト
    /// </summary>
    /// <param name="onFinished"></param>
    private void TitleCurtainOn(Action onFinished)
    {
        this.curtain.SetActive(true);
        
        this.curtain.GetComponent<Image>()
            .DOFade(1f, 1.5f)
            .OnComplete(() =>
            {
                onFinished?.Invoke();
            });
    }
    
    /// <summary>
    /// StageInfoのPopupアニメーションコルーチン開始
    /// </summary>
    private void StageInfoPopupAnimAsync()
    {
        StartCoroutine(this.StageInfoPopupAnim());
    }
    
    /// <summary>
    /// StageInfoのPopupアニメーションコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator StageInfoPopupAnim()
    {
        // 順にPopup
        for (int num = 0; num < this.stageList.Count; num++)
        {
            this.stageList[num].transform.DOScale(1.0f, 0.5f)
                .From(Vector3.zero)
                .SetEase(this.stagePopupEase)
                .SetAutoKill(true)
                .SetUpdate(true);
            
            yield return new WaitForSeconds(0.5f);
            
            if (num == 0) { StageInfoSlideAnimAsync(); }
        }
    }
    
    /// <summary>
    /// StageInfoのSlideアニメーションコルーチン開始
    /// </summary>
    private void StageInfoSlideAnimAsync()
    {
        StartCoroutine(this.StageInfoSlideAnim());
    }
    
    /// <summary>
    /// StageInfoのSlideアニメーションコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator StageInfoSlideAnim()
    {
        // Slide
        stageGourp.transform.DOLocalMoveX(-900f, 4.5f)
            .From(0f)
            .SetEase(this.stageSlideEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(1f, () =>
                {
                    // Slide Backward to Present Stage
                    stageGourp.transform.DOLocalMoveX(0, 2.5f)
                        .From(-900f)
                        .SetEase(this.stageSlideEase)
                        .SetAutoKill(true)
                        .SetUpdate(true)
                        .OnComplete(() =>
                        {
                            // コルーチン停止
                            this.StopCoroutines();
                        });
                });
            });

        yield return null;
    }

    /// <summary>
    /// コルーチン停止
    /// </summary>
    private void StopCoroutines()
    {
        StopCoroutine(this.StageInfoPopupAnim());
        StopCoroutine(this.StageInfoSlideAnim());
        
        DOVirtual.DelayedCall(2f, () =>
        {
            // スケール初期化
            foreach (var stage in this.stageList)
            {
                stage.transform.localScale = Vector3.zero;
            }
            
            // スキップボタン押下有無で分岐
            if (!this.isSkipped)
                // ゲームスタート
                this.StartGame();
            else
                // トリガー初期化
                this.isSkipped = false;
        });
    }

    /// <summary>
    /// ゲームスタート
    /// </summary>
    private void StartGame()
    {
        GameManager.Instance.TransitionEffectOnTitleToStage();
    }
    #endregion
    
    #region [02. ボタン押下時]
    /// <summary>
    /// Startボタン押下時の処理
    /// </summary>
    public void OnClickStartButton()
    {
        // TitleScreenフェードアウト
        this.TitleCurtainOn(() =>
        {
            // TitleScreen非表示
            this.SetTitleScreen(false);
            // StageInfo表示
            this.SetStageInfo(true);
            
            // TitleScreenフェードイン
            this.TitleCurtainOff(() =>
            {
                // アニメーション開始
                this.StageInfoPopupAnimAsync();
            });
        });
    }
    
    /// <summary>
    /// Skipボタン押下時の処理
    /// </summary>
    public void OnClickSkipButton()
    {
        // スキップボタン押下是非のトリガー
        this.isSkipped = true;
        
        // ゲームスタート
        GameManager.Instance.TransitionEffectOnTitleToStage();
    }
    #endregion
    
    #endregion
}
