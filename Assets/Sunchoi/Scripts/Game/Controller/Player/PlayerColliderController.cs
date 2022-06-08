using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerColliderController : MonoBehaviour
{
    #region [var]

    #region [01. Reference]
    /// <summary>
    /// PlayerMovementController
    /// </summary>
    [SerializeField]
    private PlayerMovementController playerMovementController;
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
                
                // PlayerTurn時
                if(UnitTurnManager.Instance.IsPlayerAttackPhaseOn)
                {
                    // BattleDialog表示：PlayerBattleDialog
                    this.uIDialogController.
                        ShowBattleDialog(this.uIDialogController.Dialog_Battle.transform, () =>
                        {
                            // Battle開始アニメーションの再生
                            BattleManager.Instance.StartBattleAnim(this.enemyCollider.transform, 0);
                        });

                    DOVirtual.DelayedCall(0.15f, () =>
                    {
                        // カメラアニメーションを再生
                        playerMovementController.PlayCameraAnimOnBattleBegin(() => { });
                    });
                    
                    Debug.LogFormat("Player Battle 開始", DColor.cyan);
                    
                    DOVirtual.DelayedCall(0.15f, () =>
                    {
                        // ゲーム再生を停止
                        Time.timeScale = 0f;
                    
                        // Turn制御のトリガーをセット:Player Contact Enemy
                        UnitTurnManager.Instance.SetPlayerContactEnemyTrigger(true);
                    });
                }
                // EnemyTurn時
                if(UnitTurnManager.Instance.IsEnemyAttackPhaseOn)
                {
                    // BattleDialog表示：EnemyBattleDialog
                    this.uIDialogController.
                        ShowBattleDialog(this.uIDialogController.Dialog_Battle.transform, () =>
                        {
                            // Battle開始アニメーションの再生
                            BattleManager.Instance.StartBattleAnim(this.enemyCollider.transform, 1);
                        });
                    
                    DOVirtual.DelayedCall(0.15f, () =>
                    {
                        // カメラアニメーションを再生
                        playerMovementController.PlayCameraAnimOnBattleBegin(() => { });
                    });
                    
                    Debug.LogFormat("Enemy Battle 開始", DColor.cyan);
                    
                    DOVirtual.DelayedCall(0.15f, () =>
                    {
                        // ゲーム再生を停止
                        Time.timeScale = 0f;
                    });
                    
                    // 該当Enemyの移動コルーチン再生を一時停止
                    EnemyManager.Instance.StopEnemyMoveEachCoroutineAtMoment();
                    
                    // Turn制御のトリガーをセット:Enemy Contact Player
                    UnitTurnManager.Instance.SetEnemyContactPlayerTrigger(true);
                }
            }
            
            // if (other.CompareTag("Item"))
            // {
            //     // 各種データセット後、ItemLogを表示
            //     var item = other.transform.parent.GetComponent<ItemController>().Item;
            //     PlayerStatusManager.Instance.SetCurrentContactingItem(item, other.transform, true);
            //     this.uIDialogController.SetItemDialog(item, other.transform);
            // }
            //
            // if (other.CompareTag("ExitDoor"))
            // {
            //     
            //     // Turn制御のトリガーをセット
            //     //UnitTurnManager.Instance.SetPlayerContactObjectTrigger(true);
            // }
            //
            // if (other.CompareTag("LootBox"))
            // {
            //     
            //     // Turn制御のトリガーをセット
            //     //UnitTurnManager.Instance.SetPlayerContactObjectTrigger(true);
            // }

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
