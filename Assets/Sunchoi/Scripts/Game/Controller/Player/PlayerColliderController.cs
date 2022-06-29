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
    private Collider2D enemyCollider;
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
                    // EnemyColliderのTransform
                    var tempEnemyColliderTransform = this.enemyCollider.transform;
                    // EnemyのScriptableObject上のデータ
                    var tempEnemyInfo = tempEnemyColliderTransform.parent.GetComponent<EnemyStatusController>().Enemy; 
                    
                    // Enemyのデータをセット
                    BattleManager.Instance.SetEnemyInfo(tempEnemyColliderTransform, tempEnemyInfo);
                    
                    // BattleDialog表示：PlayerBattleDialog
                    this.uIDialogController.
                        ShowBattleDialog(this.uIDialogController.Dialog_Battle.transform, () =>
                        {
                            // Battle開始アニメーションの再生
                            BattleManager.Instance.StartBattleAnim(0);
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
                    // EnemyColliderのTransform
                    var tempEnemyColliderTransform = this.enemyCollider.transform;
                    // EnemyのScriptableObject上のデータ
                    var tempEnemyInfo = tempEnemyColliderTransform.parent.GetComponent<EnemyStatusController>().Enemy; 
                    
                    // Enemyのデータをセット
                    BattleManager.Instance.SetEnemyInfo(tempEnemyColliderTransform, tempEnemyInfo);
                    
                    // BattleDialog表示：EnemyBattleDialog
                    this.uIDialogController.
                        ShowBattleDialog(this.uIDialogController.Dialog_Battle.transform, () =>
                        {
                            // Battle開始アニメーションの再生
                            BattleManager.Instance.StartBattleAnim(1);
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
            
            other = null;
        }
    }
    #endregion
}
