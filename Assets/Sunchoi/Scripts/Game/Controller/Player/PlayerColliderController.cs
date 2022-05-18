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
    private UIDialogController uIDialogController;
    #endregion
    
    #region [02. Data Set]
    /// <summary>
    /// EnemyCollider
    /// </summary>
    [SerializeField]
    private Collider2D enemyCollider;
    /// <summary>
    /// ItemCollider
    /// </summary>
    [SerializeField]
    private Collider2D itemCollider;
    #endregion

    #endregion   
    
    #region [func]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="dialogController"></param>
    public void SetData(UIDialogController dialogController)
    {
        this.uIDialogController = dialogController;
    }
    
    /// <summary>
    /// OnTriggerEnter2D
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            if (other.CompareTag("Enemy"))
            {
                // TODO:: 臨時データセット
                this.enemyCollider = other;
                
                // Turn制御のトリガーをセット
                UnitTurnManager.Instance.SetPlayerContactEnemyTrigger(true);
            }
            
            if (other.CompareTag("Item"))
            {
                // 各種データセット後、ItemLogを表示
                var item = other.transform.parent.GetComponent<ItemController>().Item;
                PlayerStatusManager.Instance.SetCurrentContactingItem(item, other.transform, true);
                this.uIDialogController.SetItemDialog(item, other.transform);
                
                // Turn制御のトリガーをセット
                UnitTurnManager.Instance.SetPlayerContactObjectTrigger(true);
            }
            
            if (other.CompareTag("ExitDoor"))
            {
                
                // Turn制御のトリガーをセット
                //UnitTurnManager.Instance.SetPlayerContactObjectTrigger(true);
            }
            
            if (other.CompareTag("LootBox"))
            {
                
                // Turn制御のトリガーをセット
                //UnitTurnManager.Instance.SetPlayerContactObjectTrigger(true);
            }

            other = null;
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
