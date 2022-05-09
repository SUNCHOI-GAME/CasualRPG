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
    /// <summary>
    /// ItemDescripction
    /// </summary>
    [SerializeField, TextArea(10,10)]
    private string itemDescripction;
    #endregion


    #region [func]
    /// <summary>
    /// Item情報を登録
    /// </summary>
    /// <param name="name"></param>
    /// <param name="sprite"></param>
    public void SetItemInfo(string name, Sprite sprite, string description)
    {
        this.itemName = name;
        this.itemSprite.sprite = sprite;
        this.itemDescripction = description;
    }

    /// <summary>
    /// SlotIcon押下時の処理
    /// </summary>
    public void OnClickSlotIcon()
    {
        InventoryManager.Instance.SetDecsription(this.itemName, this.itemSprite.sprite, this.itemDescripction);
    }
    #endregion
}
