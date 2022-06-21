using UnityEngine;
using UnityEngine.Serialization;

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
    /// StatusBonus
    /// </summary>
    public int maxHp;
    public int attack;
    public int critical;
    public int defence;
    public int agility;
    public int maxInventoryCount;
}
