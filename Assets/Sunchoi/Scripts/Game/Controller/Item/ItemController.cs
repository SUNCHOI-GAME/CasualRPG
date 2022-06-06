using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    /// <summary>
    /// ScriptableIbject上のItem情報 
    /// </summary>
    [SerializeField]
    private Item item;
    public Item Item { get => this.item; }

    /// <summary>
    /// Item取得時、該当するItemをInventoryListに追加
    /// </summary>
    public void AddToInventory()
    {
        // InventoryListに追加
        InventoryManager.Instance.AddList(this.item);
        // Map上のItemを破棄
        Destroy(this.gameObject);
    }
}
