using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    /// <summary>
    /// ItemID
    /// </summary>
    public int itemID;
    /// <summary>
    /// ItemSprite
    /// </summary>
    public Sprite itemSprite;
    /// <summary>
    /// ItemName
    /// </summary>
    public string itemName;
    /// <summary>
    /// ItemDescription
    /// </summary>
    [TextArea(10,10)]
    public string itemDescription;
    /// <summary>
    /// 使用できるアイテムかどうか
    /// </summary>
    public bool isUsable = false;
}
