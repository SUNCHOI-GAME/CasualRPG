using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SlotIconInfo : MonoBehaviour
{
    #region [var]
    /// <summary>
    /// ItemName
    /// </summary>
    [SerializeField]
    private int itemID;
    public int ItemID { get => this.itemID; }
    /// <summary>
    /// ItemName
    /// </summary>
    [SerializeField]
    private string itemName;
    public string ItemName { get => this.itemName; }
    /// <summary>
    /// ItemSprite
    /// </summary>
    [SerializeField]
    private Image itemSprite;
    /// <summary>
    /// ItemCount
    /// </summary>
    [SerializeField]
    private Text itemCount;
    [SerializeField]
    private int count = 0;
    /// <summary>
    /// ItemDescripction
    /// </summary>
    [SerializeField, TextArea(10,10)]
    private string itemDescripction;
    
    [Header("Slot Icon 押下時")]
    /// <summary>
    /// 選択済み表示アイコン
    /// </summary>
    [SerializeField]
    private GameObject selectedIcon;
    /// <summary>
    /// 選択済みトリガー
    /// </summary>
    [SerializeField]
    private bool isSelected = false;
    #endregion


    #region [func]
    /// <summary>
    /// Item情報を登録
    /// </summary>
    /// <param name="name"></param>
    /// <param name="sprite"></param>
    public void SetItemInfo(int id, string name, Sprite sprite, string description)
    {
        this.itemID = id;
        this.itemName = name;
        this.itemSprite.sprite = sprite;
        this.itemDescripction = description;
        this.AddItemCount();
    }

    /// <summary>
    /// Itemの所持カウントの増加処理
    /// </summary>
    public void AddItemCount()
    {
        this.count += 1;
        this.itemCount.text = this.count.ToString();
    }
    
    /// <summary>
    /// Itemの所持カウントの増加処理
    /// </summary>
    public void ReduceItemCount()
    {
        this.count -= 1;
        this.itemCount.text = this.count.ToString();
    }

    /// <summary>
    /// SlotIcon押下時の処理
    /// </summary>
    public void OnClickSlotIcon()
    {
        // 既に選択されたSlotIconがない場合
        if (!InventoryManager.Instance.IsItemSelected)
        {
            // SelectedIcon表示
            this.SetSelectedState(true);
            
            // ItemInfoの提供およびDescriptionView表示
            InventoryManager.Instance.SetDescription(this.itemName, this.itemSprite.sprite, this.itemDescripction);
            InventoryManager.Instance.SetSelectedItemInfo(this);
        }
        // 既に選択されたSlotIconがあった場合
        else
        {
            // 既に選択されたSlotIconが、現在選択したものだった場合
            if(this == InventoryManager.Instance.SelectedItemInfo)
            {
                // SelectedIcon非表示
                this.SetSelectedState(false);
            
                // DescriptionView非表示
                InventoryManager.Instance.CloseDescription();
            }
            // 既に選択されたSlotIconが、現在選択したものと違う場合
            else
            {
                // 
                InventoryManager.Instance.SelectedItemInfo.SetSelectedState(false);
                
                // SelectedIcon表示
                this.SetSelectedState(true);
            
                // ItemInfoの提供およびDescriptionView表示
                InventoryManager.Instance.SetDescription(this.itemName, this.itemSprite.sprite, this.itemDescripction);
                InventoryManager.Instance.SetSelectedItemInfo(this);
            }
        }
    }

    /// <summary>
    /// SelectedIconの表示切り替え
    /// </summary>
    /// <param name="state"></param>
    public void SetSelectedState(bool state)
    {
        InventoryManager.Instance.SetItemSelectedTriggerState(state);
        this.selectedIcon.SetActive(state);
    }

    /// <summary>
    /// Item破棄時の処理
    /// </summary>
    public void RemoveItem()
    {
        if (this.count > 1)
        {
            // Itemの現在数が2以上の場合、カウントだけを減らす。
            this.ReduceItemCount();
        }
        else if (this.count == 1)
        {
            // Itemの現在数が1つのみの場合、格納しているGameObject自体を破棄
            Destroy(this.gameObject);
            // 開いていたItemDescriptionを初期化
            InventoryManager.Instance.SetDescriptionNull();
        }
    }
    #endregion
}
