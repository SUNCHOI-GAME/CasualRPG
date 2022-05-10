using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderController : MonoBehaviour
{
    #region [func]
    /// <summary>
    /// OnTriggerEnter2D
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            // Item格納数のステータスによって挙動を変更
            if(InventoryManager.Instance.InventoryCurrentStorageNum < InventoryManager.Instance.InventoryMaxStorageNum)
            {
                other.transform.parent.GetComponent<ItemController>().AddToInventory();
                InventoryManager.Instance.ListItemsOnInventory();
            }
            else
            {
                Debug.LogFormat("Inventory Stage is Max, Cannot Get Item Anymore.", DColor.yellow);
            }
        }
    }
    
    /// <summary>
    /// OnTriggerExit2D
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            
        }
    }
    #endregion
}
