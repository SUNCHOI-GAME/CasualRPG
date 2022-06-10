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

        while (true)
        {
            // 比較ターゲットのPosition
            Vector2 toPos = this.toTargetTransform.position;
            Vector2 fromPos = this.fromTargetTransform.position;

            // Indicatorの角度を更新
            {
                // 両ターゲットの座標の差分
                Vector2 diference = toPos - fromPos;
                // 角度の向き（正か反か）を指定
                float sign = (toPos.y < fromPos.y) ? -1.0f : 1.0f;
                // 角度
                float angle = Vector2.Angle(Vector2.right, diference) * sign;

                // Indicatorに角度を適用
                this.indicatorRectTransform.localEulerAngles = new Vector3(0f, 0f, angle);
                // Indicator内のExitDoorスプライトを常に正しい角度で表示するようにangle値を相殺
                this.ExitDoorImage.localEulerAngles = new Vector3(0f, 0f, - angle - 90f);
            }

            // Indicatorの表示是非を更新
            {
                // MainCamera
                var playerCamera = fromTargetTransform.GetComponent<PlayerMovementController>().MainCamera;
                // Indicatorが表示される範囲の４点を指定
                Rect rect = new Rect(0f, 0.3f, 1f, 1f);
                
                // Viewportで見たターゲットの座標
                Vector2 targetPosViewportPoint = playerCamera.WorldToViewportPoint(toPos);
                // ターゲットの座標がRectの範囲に収まっているかどうか
                bool isOffScreen = rect.Contains(targetPosViewportPoint);

                // 表示切り替え
                if (isOffScreen)
                    this.indicatorRectTransform.gameObject.SetActive(false);
                else
                    this.indicatorRectTransform.gameObject.SetActive(true);
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
        // Indicator非表示
        this.SetIndicatorState(false);
        
        this.toTargetTransform = null;
        this.fromTargetTransform = null;

        this.StopCoroutines();
    }
    
    /// <summary>
    /// コルーチン停止
    /// </summary>
    private void StopCoroutines()
    {
        StopCoroutine(DisplayIndicator());
    }
    #endregion
    
    #endregion
}
