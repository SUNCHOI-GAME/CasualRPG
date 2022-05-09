using System.Collections;
using System.Collections.Generic;
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
            obj.GetComponent<SlotIconInfo>().SetItemInfo(item.itemName, item.itemSprite);
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
    
    #endregion
}
