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
            other.transform.parent.GetComponent<ItemController>().AddToInventory();
            InventoryManager.Instance.ListItemsOnInventory();
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
