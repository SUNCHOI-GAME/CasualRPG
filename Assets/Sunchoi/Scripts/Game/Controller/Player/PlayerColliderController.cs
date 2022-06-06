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
                    // Turn制御のトリガーをセット:Player Contact Enemy
                    UnitTurnManager.Instance.SetPlayerContactEnemyTrigger(true);
                    
                    Debug.LogFormat("Player Battle 開始", DColor.cyan);
                    // ゲーム再生を再開
                    Time.timeScale = 0f;
                    
                    // BattleDialog表示：PlayerBattleDialog
                    this.uIDialogController.
                        ShowBattleDialog(this.uIDialogController.Dialog_PlayerBattle.transform, () =>
                        {
                            // TODO:: Battle開始処理を追加
                            
                            // TODO:: 臨時処理、該当Enemyを破棄する、本来はBattleの結果によって廃棄を決める予定
                            EnemyManager.Instance.
                                DestroySpecificEnemy(this.enemyCollider.transform.parent
                                    .GetComponent<EnemyMovementController>());
                        });
                }
                // EnemyTurn時
                if(UnitTurnManager.Instance.IsEnemyAttackPhaseOn)
                {
                    // Turn制御のトリガーをセット:Enemy Contact Player
                    UnitTurnManager.Instance.SetEnemyContactPlayerTrigger(true);
                    
                    Debug.LogFormat("Enemy Battle 開始", DColor.cyan);
                    // ゲーム再生を再開
                    Time.timeScale = 0f;
                    
                    // 該当Enemyの移動コルーチン再生を一時停止
                    EnemyManager.Instance.StopEnemyMoveEachCoroutineAtMoment();
                    // BattleDialog表示：EnemyBattleDialog
                    this.uIDialogController.
                        ShowBattleDialog(this.uIDialogController.Dialog_EnemyBattle.transform, () =>
                        {
                            // TODO:: Battle開始処理を追加
                            
                            // TODO:: 臨時処理、該当Enemyを破棄する、本来はBattleの結果によって廃棄を決める予定
                            EnemyManager.Instance.
                                ExcludeEnemyTemporarily(this.enemyCollider.transform.parent
                                    .GetComponent<EnemyMovementController>());
                        });
                }
            }
            
            if (other.CompareTag("Item"))
            {
                // 各種データセット後、ItemLogを表示
                var item = other.transform.parent.GetComponent<ItemController>().Item;
                PlayerStatusManager.Instance.SetCurrentContactingItem(item, other.transform, true);
                this.uIDialogController.SetItemDialog(item, other.transform);
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
