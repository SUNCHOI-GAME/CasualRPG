using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UILogController : MonoBehaviour
{
    
    #region [var]

    #region [00. Instance]
    /// <summary>
    /// UIButtonController
    /// </summary>
    [SerializeField]
    private UIButtonController uIButtoncontroller;
    #endregion

    #region [01. Base]
    [Header("Log Objects")]
    /// <summary>
    /// ItemLogのGameObject
    /// </summary>
    [SerializeField]
    private GameObject log_Item;
    [Header("Log Animation")]
    /// <summary>
    /// カメラ移動時のアニメーションパターン
    /// </summary>
    [SerializeField]
    private Ease logEase;
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

    #region [02. Item Log]
    /// <summary>
    /// トリガー
    /// </summary>
    [SerializeField]
    private bool isItemLog = false;
    /// <summary>
    /// Item
    /// </summary>
    [SerializeField]
    private Item currentItem;
    [SerializeField]
    private Transform currentItemTransform;
    /// <summary>
    /// Item Image
    /// </summary>
    [SerializeField]
    private Image itemImage;
    /// <summary>
    /// Item Name
    /// </summary>
    [SerializeField]
    private Text itemName;
    
    /// <summary>
    /// Yes Button
    /// </summary>
    [SerializeField]
    private Button button_yes;
    #endregion
    
    #endregion


    #region [func]

    #region [01. コンストラクタ]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Awake()
    {
        // Log表示を初期化
        this.log_Item.transform.localScale = this.closeScale;
    }
    #endregion

    #region [02. Log表示/非表示]
    /// <summary>
    /// メニュー表示
    /// </summary>
    /// <param name="tranform"></param>
    public void ShowLog(Transform tranform)
    {
        // ボタン押下無効
        this.uIButtoncontroller.DisableButtonTouch();
        
        // アニメーション
        tranform.DOScale(1.0f, this.openSpeed)
            .From(this.closeScale)
            .SetEase(this.logEase)
            .SetAutoKill(true)
            .SetUpdate(true);

        // スケール固定
        tranform.localScale = this.openScale;
    }
    
    /// <summary>
    /// メニュー非表示
    /// </summary>
    /// <param name="tranform"></param>
    public void CloseLog(Transform tranform)
    {
        // アニメーション
        tranform.DOScale(0.0f, this.closeSpeed)
            .SetEase(this.logEase)
            .SetAutoKill(true)
            .SetUpdate(true).OnComplete(() =>
            {
                // ボタン押下有効
                this.uIButtoncontroller.EnableButtonTouch();
                
                // 初期化
                if(tranform.name == "ItelmLog")
                {
                    this.SetItemLogNull();
                }
            });
    }
    #endregion
    
    #region [03. Item Log]
    /// <summary>
    /// Item LogのItem画像および名前をセット
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="name"></param>
    public void SetItemLog(Item item, Transform transform)
    {
        // ItemLog上の各種データをセット
        this.itemImage.sprite = item.itemSprite;
        this.itemName.text = item.itemName;

        // 同じItemの対象に再度ItemLogを表示する際のためのデータセット
        this.currentItem = item;
        this.currentItemTransform = transform;
        
        // Item格納数のステータスによって挙動を変更
        if(InventoryManager.Instance.InventoryCurrentStorageNum < InventoryManager.Instance.InventoryMaxStorageNum)
        {
            this.button_yes.enabled = true;
            this.button_yes.image.color = Color.white;
        }
        else
        {
            this.button_yes.enabled = false;
            this.button_yes.image.color = Color.gray;
        }
        
        // ItemLog表示
        this.ShowLog(this.log_Item.transform);

        // トリガーセット
        this.isItemLog = true;
    }
    
    /// <summary>
    /// ItemLog初期化
    /// </summary>
    public void SetItemLogNull()
    {
        this.itemImage.sprite = null;
        this.itemName.text = null;

        this.currentItem = null;
        this.currentItemTransform = null;
    }

    /// <summary>
    /// YesButton押下時の処理
    /// </summary>
    public void OnClickYesButton()
    {
        if(this.isItemLog)
        {
            // Inventoryに該当アイテムを追加
            this.currentItemTransform.parent.GetComponent<ItemController>().AddToInventory();
            InventoryManager.Instance.ListItemsOnInventory();

            // Log非表示
            this.CloseLog(this.log_Item.transform);
        }
    }

    /// <summary>
    /// NoButton押下時の処理
    /// </summary>
    public void OnClickNoButton()
    {
        if(this.isItemLog)
        {
            // Log非表示
            this.CloseLog(this.log_Item.transform);
        }
    }
    #endregion
    
    #endregion
}
