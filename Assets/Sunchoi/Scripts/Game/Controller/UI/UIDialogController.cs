using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogController : MonoBehaviour
{
    
    #region [var]

    #region [00. Instance]
    /// <summary>
    /// UIButtonController
    /// </summary>
    [SerializeField]
    private UIButtonController uIButtonController;
    #endregion

    #region [01. Base]
    [Header(" --- Dialog Objects")]
    /// <summary>
    /// ItemDialogのGameObject
    /// </summary>
    [SerializeField]
    private GameObject dialog_Item;
    public GameObject Dialog_Item { get => this.dialog_Item; }
    /// <summary>
    /// StatusInfoDialogのGameObject
    /// </summary>
    [SerializeField]
    private GameObject dialog_StatusInfo;
    public GameObject Dialog_StatusInfo { get => this.dialog_StatusInfo; }
    
    [Header(" --- Dialog Animation")]
    /// <summary>
    /// Dialogアニメーションパターン
    /// </summary>
    [SerializeField]
    private Ease diallogEase;
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
    [SerializeField]
    private float closeSpeed_LongDialog = 0.2f;
    [SerializeField]
    private float closeSpeed_ShortDialog = 0.2f;
    /// <summary>
    /// Open時のスピード
    /// </summary>
    [SerializeField]
    private float openSpeed_LongDialog = 0.2f;
    [SerializeField]
    private float openSpeed_ShortDialog = 0.2f;
    #endregion

    #region [02. Item Dialog]
    [Header(" --- Item Dialog")]
    /// <summary>
    /// トリガー
    /// </summary>
    [SerializeField]
    private bool isItemDialog = false;
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
    /// Game画面タッチ不可にするための暗幕
    /// </summary>
    [SerializeField]
    private GameObject curtain;
    
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
        this.dialog_Item.transform.localScale = this.closeScale;
        this.dialog_StatusInfo.transform.localScale = this.closeScale;
    }
    #endregion

    #region [02. Log表示/非表示]
    /// <summary>
    /// メニュー表示
    /// </summary>
    /// <param name="tranform"></param>
    public void ShowDialog(Transform dialogTransform, int size)
    {
        float startYPos = 0;
        startYPos = size == 0 ? -345f : -150f;
        startYPos = size == 0 ? -345f : -150f;

        float speed = 0;
        speed = size == 0 ? this.openSpeed_LongDialog : this.openSpeed_ShortDialog;

        // ボタン押下無効
        this.uIButtonController.DisableButtonTouch();
        
        // 暗幕表示
        this.curtain.SetActive(true);
        
        // スケール変更
        dialogTransform.localScale = this.openScale;
        
        // アニメーション
        dialogTransform.DOLocalMove(new Vector3(0f, 0f, 0f), speed)
            .From(new Vector3(0f, startYPos, 0f))
            .SetEase(this.diallogEase)
            .SetAutoKill(true)
            .SetUpdate(true);

        // スケール固定
        dialogTransform.localScale = this.openScale;
    }
    
    /// <summary>
    /// メニュー非表示
    /// </summary>
    /// <param name="tranform"></param>
    public void CloseDialog(Transform dialogTransform, int size)
    {
        float startYPos = 0;
        startYPos = size == 0 ? -345f : -150f;

        float speed = 0;
        speed = size == 0 ? this.closeSpeed_LongDialog : this.closeSpeed_ShortDialog;
        
        // アニメーション
        dialogTransform.DOLocalMove(new Vector3(0f, startYPos, 0f), speed)
            .From(new Vector3(0f, 0f, 0f))
            .SetEase(this.diallogEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                // ボタン押下有効
                this.uIButtonController.EnableButtonTouch();

                // 暗幕表示
                this.curtain.SetActive(false);
                
                // スケール変更
                dialogTransform.localScale = this.closeScale;
                
                // 初期化
                if(dialogTransform.name == "ItemLog")
                {
                    this.SetItemLogNull();
                }
            });
    }
    #endregion
    
    #region [03. Item Dialog]
    /// <summary>
    /// Item LogのItem画像および名前をセット
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="name"></param>
    public void SetItemDialog(Item item, Transform logTransform)
    {
        // ItemLog上の各種データをセット
        this.itemImage.sprite = item.itemSprite;
        this.itemName.text = item.itemName;

        // 同じItemの対象に再度ItemLogを表示する際のためのデータセット
        this.currentItem = item;
        this.currentItemTransform = logTransform;
        
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

        // トリガーセット
        this.isItemDialog = true;
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
        if(this.isItemDialog)
        {
            // Inventoryに該当アイテムを追加
            this.currentItemTransform.parent.GetComponent<ItemController>().AddToInventory();
            InventoryManager.Instance.ListItemsOnInventory();
            
            // Turn制御のトリガーをセット
            UnitTurnManager.Instance.SetPlayerCheckObjectPhaseTrigger(false);
            
            // Log非表示
            this.CloseDialog(this.dialog_Item.transform, 1);
        }
    }

    /// <summary>
    /// NoButton押下時の処理
    /// </summary>
    public void OnClickNoButton()
    {
        if(this.isItemDialog)
        {
            // Turn制御のトリガーをセット
            UnitTurnManager.Instance.SetPlayerCheckObjectPhaseTrigger(false);
            
            // Log非表示
            this.CloseDialog(this.dialog_Item.transform, 1);
        }
    }
    #endregion
    
    #endregion
}
