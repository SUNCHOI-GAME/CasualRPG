using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITargetIndicatorController : MonoBehaviour
{
    #region [var]

    #region [01. instance]
    /// <summary>
    /// インスタンス
    /// </summary>
    public static UITargetIndicatorController Instance { get; private set; }
    #endregion
    
    #region [02. Indicator]
    /// <summary>
    /// ToTargetのTransform
    /// </summary>
    [SerializeField]
    private Transform toTargetTransform;
    /// <summary>
    /// FromTargetのTransform
    /// </summary>
    [SerializeField]
    private Transform fromTargetTransform;
    /// <summary>
    /// IndicatorのRectTransform
    /// </summary>
    [SerializeField]
    private RectTransform indicatorRectTransform;
    /// <summary>
    /// ExitDoorのImage
    /// </summary>
    [SerializeField]
    private RectTransform ExitDoorImage;
    
    /// <summary>
    /// Indicatorが表示される範囲を指定
    /// </summary>
    [SerializeField]
    Rect rect = new Rect(0f, 0f, 0.95f, 1f);
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

    #region [01.Set Target]
    /// <summary>
    /// ToTarget登録
    /// </summary>
    /// <param name="targetTransform"></param>
    public void SetToTarget(Transform targetTransform)
    {
        this.toTargetTransform = targetTransform;
    }
    /// <summary>
    /// FromTarget登録
    /// </summary>
    /// <param name="targetTransform"></param>
    public void SetFromTarget(Transform targetTransform)
    {
        this.fromTargetTransform = targetTransform;
    }
    #endregion

    #region [02.Display Indicator]
    /// <summary>
    /// DisplayIndicatorコルーチン開始
    /// </summary>
    public void DisplayIndicatorAsync()
    {
        StartCoroutine(DisplayIndicator());
    }
    
    /// <summary>
    /// DisplayIndicatorコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator DisplayIndicator()
    {
        // Indicator表示
        this.SetIndicatorState(true);
        // MainCamera
        var mainCamera = fromTargetTransform.GetComponent<PlayerMovementController>().MainCamera;

        while (true)
        {
            // ターゲット(ExitDoor)のPositionをViewportPoint座標に変換
            Vector2 toPos = mainCamera.WorldToViewportPoint(this.toTargetTransform.position);
            // 基準点(Player)のPositionをViewportPoint座標に変換
            Vector2 fromPos = mainCamera.WorldToViewportPoint(this.fromTargetTransform.position);
            // 基準点から見たターゲットまでの距離
            var difference = toPos - fromPos;
            
            // 距離によってIndicatorの表示ステートを更新
            {
                bool isOffGameScreen = Mathf.Abs(difference.x) > 0.457f || Mathf.Abs(difference.y) > 0.307f;
                if (isOffGameScreen) this.indicatorRectTransform.gameObject.SetActive(true);
                else this.indicatorRectTransform.gameObject.SetActive(false);
            }
            
            // Indicatorの角度を更新
            {
                // 角度の向き（正か反か）を指定
                float sign = (toPos.y < fromPos.y) ? -1.0f : 1.0f;
                // 角度
                float angle = Vector2.Angle(Vector2.right, difference) * sign;
                float offset = 0;
                
                
                // if (sign < 0)
                // {
                //     if (difference.x < 0 && difference.y < 0) offset = 20;
                //     else if (difference.x > 0 && difference.y < 0) offset = -20;
                // }
                // else if (sign > 0)
                // {
                //     if (difference.x < 0 && difference.y > 0) offset = -20;
                //     else if (difference.x > 0 && difference.y > 0) offset = 20;
                // }
                
                // Debug.LogFormat($" ", DColor.yellow);
                // Debug.LogFormat($"{sign}", DColor.cyan);
                // Debug.LogFormat($"{difference.x}", DColor.cyan);
                // Debug.LogFormat($"{offset}", DColor.yellow);
                // Debug.LogFormat($"{angle + offset}", DColor.yellow);
                
                // Indicatorに角度を適用
                this.indicatorRectTransform.localEulerAngles = new Vector3(0f, 0f, angle + offset);
                
                // Indicator内のExitDoorスプライトを常に正しい角度で表示するようにangle値を相殺
                this.ExitDoorImage.localEulerAngles = new Vector3(0f, 0f, -angle - 90f);
            }
             
            yield return null;
        }
    }
    #endregion
    
    #region [03.Indicator 表示関連]
    /// <summary>
    /// Indicatorの表示切り替え
    /// </summary>
    /// <param name="state"></param>
    public void SetIndicatorState(bool state)
    {
        this.indicatorRectTransform.gameObject.SetActive(state);
    }
    
    /// <summary>
    /// Indicatorの処理全般を停止
    /// </summary>
    public void StopDisplayingIndicator()
    {
        StopCoroutine(DisplayIndicator());
        
        // Indicator非表示
        this.SetIndicatorState(false);
        
        this.toTargetTransform = null;
        this.fromTargetTransform = null;
    }
    
    /// <summary>
    /// コルーチン停止
    /// </summary>
    private void StopCoroutineAtTheMoment()
    {
        StopCoroutine(DisplayIndicator());
    }
    
    /// <summary>
    /// コルーチン再生
    /// </summary>
    private void StopCoroutineAgain()
    {
        StartCoroutine(DisplayIndicator());
    }
    #endregion
    
    #endregion
}
