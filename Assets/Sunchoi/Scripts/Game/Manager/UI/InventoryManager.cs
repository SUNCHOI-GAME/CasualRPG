using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
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
    /// <summary>
    /// 空のSlotIconのPrefab
    /// </summary>
    [SerializeField]
    private GameObject inventoryItem_Empty;
    
    /// <summary>
    /// InventoryMaxStorageNum
    /// </summary>
    [SerializeField]
    private int inventoryMaxStorageNum;
    public int InventoryMaxStorageNum { get => this.inventoryMaxStorageNum; }
    /// <summary>
    /// InventoryCurrentStorageNum
    /// </summary>
    [SerializeField]
    private int inventoryCurrentStorageNum;
    public int InventoryCurrentStorageNum { get => this.inventoryCurrentStorageNum; }
    #endregion
    
    #region [04. Description表示関連]
    /// <summary>
    /// DescriptionのTransform
    /// </summary>
    [SerializeField]
    private Transform descriptionTransform;
    /// <summary>
    /// DescriptionのTransform
    /// </summary>
    private bool isDescriptionShown = false;
    public bool IsDescriptionShown { get => this.isDescriptionShown; }
    /// <summary>
    /// DescriptionClose時のスピード
    /// </summary>
    private float closeSpeed = 0.2f;
    /// <summary>
    /// DescriptionOpen時のスピード
    /// </summary>
    private float openSpeed = 0.1f;
    /// <summary>
    /// Description表示時のアニメーションパターン
    /// </summary>
    [SerializeField]
    private Ease menuEase;
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
    /// RemoveButton
    /// </summary>
    [SerializeField]
    private Button removeButton;
    /// <summary>
    /// Inventoryで選択されたItemのInfo
    /// </summary>
    [SerializeField]
    private SlotIconInfo selectedItemInfo;
    #endregion
    
    #region [05. Status表示関連]
    /// <summary>
    /// InventoryCurrentStorageValue
    /// </summary>
    [SerializeField]
    private Text inventoryCurrentStorageValue;
    /// <summary>
    /// InventoryMaxStorageValue
    /// </summary>
    [SerializeField]
    private Text inventoryMaxStorageValue;
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
        // アイテム格納数表示を更新
        this.SetStorageValue();
        // 空のSlotIconを生成
        this.InitiateEmptySlotIcon();
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

        // Item格納数の現在値を更新
        this.inventoryCurrentStorageNum += 1;
    }
    
    /// <summary>
    /// Item放棄
    /// </summary>
    /// <param name="item"></param>
    public void RemoveList(Item item)
    {
        // リストから排除
        this.itemList.Remove(item);
        
        // Item格納数の現在値を更新
        this.inventoryCurrentStorageNum -= 1;
    }
    public void RemoveList(string itemName)
    {
        Item targetItem = null;
        
        // 名前で検索し、一致する最初の要素を指定
        foreach (var item in this.itemList)
        {
            if (item.name == itemName)
            {
                targetItem = item;
                continue;
            }
        }
        
        // リストから排除
        this.itemList.Remove(targetItem);
        
        // Item格納数の現在値を更新
        this.inventoryCurrentStorageNum -= 1;
        
        // アイテム格納数表示を更新
        this.SetStorageValue();
        
        // 空のSlotIconを生成
        GameObject obj;
        obj = Instantiate(this.inventoryItem_Empty, this.itemContent);
    }

    /// <summary>
    /// 選択済みStateを解除
    /// </summary>
    public void ResetSelectedIcon()
    {
        foreach (Transform item in this.itemContent)
        {
            item.gameObject.GetComponent<SlotIconInfo>()?.SetSelectedState(false);
        }
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

        // 重複チェックのためのリスト
        // ⓵リストで重複しない　→　生成　→　リストに追加
        // ⓶リストで重複している　→　カウントを増加
        List<GameObject> checkList = new List<GameObject>();

        // 新規で表示項目を生成
        foreach (var item in this.itemList)
        {
            // 重複判定トリガー
            bool isItemAleadyExist = false;            
            
            // 重複チェック
            foreach (var element in checkList)
            {
                // 重複している場合
                if (element.name == item.name)
                {
                    isItemAleadyExist = true;
                    // カウント増加
                    element.GetComponent<SlotIconInfo>().AddItemCount();
                }
            }

            // 重複しない場合
            if (!isItemAleadyExist)
            {
                // 生成
                var obj = Instantiate(this.inventoryItem, this.itemContent);
                // 命名変更
                obj.name = item.name;
                
                // 重複チェックの対象リストに追加
                checkList.Add(obj);

                // Item情報を登録
                obj.GetComponent<SlotIconInfo>().SetItemInfo(item.itemName, item.itemSprite, item.itemDescription, item.isUsable);
            }
        }

        // アイテム格納数表示を更新
        this.SetStorageValue();

        // 空のSlotIconを生成
        this.InitiateEmptySlotIcon();
    }

    /// <summary>
    /// 空のSlotIconを生成
    /// </summary>
    private void InitiateEmptySlotIcon()
    {
        var differ = this.inventoryMaxStorageNum - this.inventoryCurrentStorageNum;
        
        if(differ > 0)
        {
            GameObject obj;

            for (int num = differ; num > 0; num--)
                obj = Instantiate(this.inventoryItem_Empty, this.itemContent);
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
    public void SetDescription(string name, Sprite sprite, string description, bool isUsable)
    {
        this.itemName.text = name;
        this.itemImage.enabled = true;
        this.itemImage.sprite = sprite;
        this.itemImageBackground.enabled = true;
        this.itemDescription.text = description;
        
        this.useButton.gameObject.SetActive(true);
        this.removeButton.gameObject.SetActive(true);

        // TODO :: ボタン無効化処理実装時変更 
        if (!isUsable)
        {
            this.useButton.image.color = Color.gray;
            this.useButton.enabled = false;
        }
        else
        {
            this.useButton.image.color = Color.white;
            this.useButton.enabled = true;
        }
        
        if(!this.isDescriptionShown)
            // Description表示
            this.ShowDescription();
    }

    /// <summary>
    /// Description表示
    /// </summary>
    private void ShowDescription()
    {
        // Description表示のトリガー
        this.isDescriptionShown = true;
        
        this.descriptionTransform.DOLocalMove(new Vector3(0f, 17.45f, 0f), openSpeed)
            .From(new Vector3(0f, -130f, 0f))
            .SetEase(this.menuEase)
            .SetAutoKill(true)
            .SetUpdate(true);
    }
    
    /// <summary>
    /// Description非表示
    /// </summary>
    public void CloseDescription()
    {
        // Description表示のトリガー
        this.isDescriptionShown = false;
        
        this.descriptionTransform.DOLocalMove(new Vector3(0f, -130f, 0f), closeSpeed)
            .From(new Vector3(0f, 17f, 0f))
            .SetEase(this.menuEase)
            .SetAutoKill(true)
            .SetUpdate(true);
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
        this.removeButton.gameObject.SetActive(false);
        
        // TODO :: ボタン無効化処理実装時変更
        this.useButton.image.color = Color.white;
        this.useButton.enabled = false;
        
        this.selectedItemInfo = null;
    }

    /// <summary>
    /// Inventoryで選択中のItemのInfo
    /// </summary>
    /// <param name="itemInfo"></param>
    public void SetSelectedItemInfo(SlotIconInfo itemInfo)
    {
        this.selectedItemInfo = itemInfo;
    }

    /// <summary>
    /// RemoveButton押下時の処理
    /// </summary>
    public void OnClickRemoveItemButton()
    {
        // ItemListから削除
        this.RemoveList(this.selectedItemInfo.ItemName);
        // Inventory上から削除
        this.selectedItemInfo.RemoveItem();
    }
    #endregion

    #region [04.Item格納]
    /// <summary>
    /// Itemの最大格納数の増加処理
    /// </summary>
    /// <param name="num"></param>
    public void SetInventoryMaxStorageNum(int num)
    {
        this.inventoryMaxStorageNum = num;
    }
    public void AddInventoryMaxStorageNum(int num)
    {
        this.inventoryMaxStorageNum += num;
    }

    /// <summary>
    /// アイテム格納数表示を更新
    /// </summary>
    private void SetStorageValue()
    {
        this.inventoryCurrentStorageValue.text = this.inventoryCurrentStorageNum.ToString();
        this.inventoryMaxStorageValue.text = this.inventoryMaxStorageNum.ToString();
        
        // Item格納数が現在値と最大値の差よって現在値のテキストの色を変更
        if(this.inventoryCurrentStorageNum < this.inventoryMaxStorageNum - 3)
            this.inventoryCurrentStorageValue.color = Color.white;
        if(this.inventoryCurrentStorageNum >= this.inventoryMaxStorageNum - 3)
            this.inventoryCurrentStorageValue.color = Color.yellow;
        if(this.inventoryCurrentStorageNum == this.inventoryMaxStorageNum)
            this.inventoryCurrentStorageValue.color = Color.red;
    }

    #endregion
    #endregion
}
