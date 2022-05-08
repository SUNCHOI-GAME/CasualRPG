using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

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
    #endregion


    #region [func]

    #region [01. コンストラクタ]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Awake()
    {
        // メニュー表示を初期化
        this.menu_Settings.SetActive(false);
        this.menu_Inventory.SetActive(false);
    }
    #endregion

    #region [02. メニュー表示切り替え]
    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    public void SetSettingsMenuActivationState(bool state)
    {
        this.menu_Settings.SetActive(state);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    public void SetInventoryMenuActivationState(bool state)
    {
        this.menu_Inventory.SetActive(state);
    }
    #endregion

    #region [03. ボタン押下時の制御]
    /// <summary>
    /// 
    /// </summary>
    public void OnClickCloseSettingsMenuButton()
    {
        // 該当メニューを非表示に切り替え
        this.menu_Settings.SetActive(false);
    }
    /// <summary>
    /// 
    /// </summary>
    public void OnClickCloseInventoryMenuButton()
    {
        // 該当メニューを非表示に切り替え
        this.menu_Inventory.SetActive(false);
    }
    #endregion
    
    #endregion
}
