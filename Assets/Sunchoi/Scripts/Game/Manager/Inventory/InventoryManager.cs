using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    #region [var]

    #region [01. Instance]
    /// <summary>
    /// インスタンス
    /// </summary>
    public static InventoryManager Instance { get; private set; }
    #endregion
    
    #region [02. List]
    /// <summary>
    /// ItemのList
    /// </summary>
    [SerializeField]
    private List<Item> itemList = new List<Item>();
    #endregion
    
    #region [03. Inventory表示関連]
    /// <summary>
    /// Inventoryで使用しているScrollViewのScrollRect
    /// </summary>
    [SerializeField]
    private ScrollRect inventoryScrollRect;
    /// <summary>
    /// Inventory上のItem格納個所
    /// </summary>
    [SerializeField]
    private Transform itemContent;
    /// <summary>
    /// SlotIconのPrefab
    /// </summary>
    [SerializeField]
    private GameObject inventoryItem;

    #region [04. Description表示関連]
    /// <summary>
    /// ItemImage
    /// </summary>
    [SerializeField]
    private Image itemImage;
    /// <summary>
    /// ItemBackground
    /// </summary>
    [SerializeField]
    private Image itemImageBackground;
    /// <summary>
    /// ItemName
    /// </summary>
    [SerializeField]
    private Text itemName;
    /// <summary>
    /// ItemDescription
    /// </summary>
    [SerializeField]
    private Text itemDescription;
    /// <summary>
    /// UseButton
    /// </summary>
    [SerializeField]
    private Button useButton;
    /// <summary>
    /// DropButton
    /// </summary>
    [SerializeField]
    private Button dropButton;

    #endregion
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
        // 破棄不可
        DontDestroyOnLoad(this.gameObject);
        
        // 初期化
        this.SetDescriptionNull();
    }
    #endregion

    #region [02. Managing Item List]
    /// <summary>
    /// Item取得
    /// </summary>
    /// <param name="item"></param>
    public void AddList(Item item)
    {
        // リストに追加
        this.itemList.Add(item);
    }
    
    /// <summary>
    /// Item放棄
    /// </summary>
    /// <param name="item"></param>
    public void RemoveList(Item item)
    {
        // リストから排除
        this.itemList.Remove(item);
    }

    /// <summary>
    /// Inventoryの並びを更新
    /// </summary>
    public void ListItemsOnInventory()
    {
        // 以前の表示項目を削除
        foreach (Transform item in this.itemContent)
        {
            Destroy(item.gameObject);
        }
        
        // 新規で表示項目を生成
        foreach (var item in this.itemList)
        {
            // 生成
            var obj = Instantiate(this.inventoryItem, this.itemContent);
            // Item情報を登録
            obj.GetComponent<SlotIconInfo>().SetItemInfo(item.itemName, item.itemSprite, item.itemDescription);
        }
    }

    /// <summary>
    /// DOTWeenのScale変更アニメーションによって発生するScrollViewの不具合を回避のためのSet関数
    /// </summary>
    /// <param name="state"></param>
    public void SetScrollRectOptionState(bool state)
    {
        this.inventoryScrollRect.vertical = state;
        this.itemContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        this.itemContent.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
    }
    
    #endregion

    #region [03. Description表示]
    /// <summary>
    /// 該当ItemのDescriptionを表示
    /// </summary>
    /// <param name="name"></param>
    /// <param name="sprite"></param>
    /// <param name="description"></param>
    public void SetDecsription(string name, Sprite sprite, string description)
    {
        this.itemName.text = name;
        this.itemImage.enabled = true;
        this.itemImage.sprite = sprite;
        this.itemImageBackground.enabled = true;
        this.itemDescription.text = description;
        this.useButton.gameObject.SetActive(true);
        this.dropButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Description表示状態を初期化
    /// </summary>
    public void SetDescriptionNull()
    {
        this.itemName.text = null;
        this.itemImage.enabled = false;
        this.itemImage.sprite = null;
        this.itemImageBackground.enabled = false;
        this.itemDescription.text = null;
        this.useButton.gameObject.SetActive(false);
        this.dropButton.gameObject.SetActive(false);
    }
    #endregion
    #endregion
}
