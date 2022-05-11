using System;
using DG.Tweening;
using UnityEngine;

public class UIMenuController : MonoBehaviour
{
    #region [var]
    [Header("Menu Objects")]
    /// <summary>
    /// SettingsメニューのGameObject
    /// </summary>
    [SerializeField]
    private GameObject menu_Settings;
    /// <summary>
    /// InventoryメニューのGameObject
    /// </summary>
    [SerializeField]
    private GameObject menu_Inventory;
    [Header("Menu Animation")]
    /// <summary>
    /// カメラ移動時のアニメーションパターン
    /// </summary>
    [SerializeField]
    private Ease menuEase;
    /// <summary>
    /// Close時スケール
    /// </summary>
    private Vector2 closeScale = Vector2.zero;
    /// <summary>
    /// Open時スケール
    /// </summary>
    private Vector2 openScale = Vector2.one;
    /// <summary>
    /// Close時のスピード
    /// </summary>
    private float closeSpeed = 0.5f;
    /// <summary>
    /// Open時のスピード
    /// </summary>
    private float openSpeed = 0.5f;
    #endregion


    #region [func]

    #region [01. コンストラクタ]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Awake()
    {
        // メニュー表示を初期化
        this.menu_Settings.transform.localScale = this.closeScale;
        this.menu_Inventory.transform.localScale = this.closeScale;
    }
    #endregion

    #region [02. メニュー表示/非表示]
    /// <summary>
    /// メニュー表示
    /// </summary>
    /// <param name="tranform"></param>
    private void ShowMenu(Transform tranform)
    {
        // アニメーション
        tranform.DOScale(1.0f, this.openSpeed)
            .From(this.closeScale)
            .SetEase(this.menuEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                // DOTWeenのScale変更アニメーションによって発生するScrollViewの不具合を回避 
                if (tranform.name == "Inventory")
                {
                    InventoryManager.Instance.SetScrollRectOptionState(true);
                }
            });

        // スケール固定
        tranform.localScale = this.openScale;
    }
    
    /// <summary>
    /// メニュー非表示
    /// </summary>
    /// <param name="tranform"></param>
    private void CloseMenu(Transform tranform)
    {
        // アニメーション
        tranform.DOScale(0.0f, this.closeSpeed)
            .SetEase(this.menuEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                // DOTWeenのScale変更アニメーションによって発生するScrollViewの不具合を回避 
                if(tranform.name == "Inventory")
                {
                    InventoryManager.Instance.SetScrollRectOptionState(false);
                    InventoryManager.Instance.SetDescriptionNull();
                }
            });
    }
   #endregion

    #region [03. ボタン押下時の制御]
    /// <summary>
    /// メニュー表示ボタン押下時の処理
    /// </summary>
    public void OnClickShowMenuButton(Transform transform)
    {
        this.ShowMenu(transform);
    }
    /// <summary>
    /// メニュー非表示ボタン押下時の処理
    /// </summary>
    public void OnClickCloseMenuButton(Transform transform)
    {
        this.CloseMenu(transform);
    }
    #endregion
    
    #endregion
}
