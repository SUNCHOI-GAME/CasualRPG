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

    /// <summary>
    /// UsableItemTrigger
    /// </summary>
    [SerializeField]
    private bool isUsable;
    #endregion


    #region [func]
    /// <summary>
    /// Item情報を登録
    /// </summary>
    /// <param name="name"></param>
    /// <param name="sprite"></param>
    public void SetItemInfo(string name, Sprite sprite, string description, bool state)
    {
        this.itemName = name;
        this.itemSprite.sprite = sprite;
        this.itemDescripction = description;
        this.isUsable = state;
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
        InventoryManager.Instance.SetDecsription(this.itemName, this.itemSprite.sprite, this.itemDescripction, this.isUsable);
        InventoryManager.Instance.SetSelectedItemInfo(this);
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
