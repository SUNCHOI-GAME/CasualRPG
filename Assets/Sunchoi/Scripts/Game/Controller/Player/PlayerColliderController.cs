using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerColliderController : MonoBehaviour
{
    #region [var]

    #region [01. Reference]
    /// <summary>
    /// UILogController
    /// </summary>
    [SerializeField]
    private UILogController uILogController;
    #endregion

    #endregion   
    
    #region [func]
    /// <summary>
    /// OnTriggerEnter2D
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            // 各種データセット後、ItemLogを表示
            var item = other.transform.parent.GetComponent<ItemController>().Item;
            PlayerStatusManager.Instance.SetCurrentContactingItem(item, other.transform, true);
            this.uILogController.SetItemLog(item, other.transform);
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
            // 各種データを初期化
            PlayerStatusManager.Instance.SetCurrentContactingItem(null, null, false);
        }
    }
    #endregion
}
