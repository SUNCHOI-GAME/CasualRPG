using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    /// <summary>
    /// ItemID
    /// </summary>
    public int itemID;
    /// <summary>
    /// ItemName
    /// </summary>
    public string itemName;
    /// <summary>
    /// ItemSprite
    /// </summary>
    public Sprite itemSprite;
}
