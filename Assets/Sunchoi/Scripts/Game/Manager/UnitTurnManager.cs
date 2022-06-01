using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UnitTurnManager : MonoBehaviour
{
    #region [01. コンストラクタ]

    #region [var]
    /// <summary>
    /// インスタンス
    /// </summary>
    public static UnitTurnManager Instance { get; private set; }
    #endregion


    #region [func]

    private void Start()
    {
        // インスタンス
        Instance = this;

        // データ初期化
        this.InitData();
    }

    #endregion

    #endregion


    #region [02. Turn Base]

    #region [var]
    /// <summary>
    /// UIButtonController
    /// </summary>
    [SerializeField]
    private UIButtonController uIbuttonController;
    /// <summary>
    /// UIDialogController
    /// </summary>
    [SerializeField]
    private UIDialogController uIDialogController;
    /// <summary>
    /// PlayerMovementController
    /// </summary>
    [SerializeField]
    private PlayerMovementController playerMovementController;

    [Header(" --- 移動処理が終了したEnemy数")]
    /// <summary>
    /// 移動処理が終了したEnemy数
    /// </summary>
    [SerializeField]
    private int finishedMovementEnemyCount;

    [Header(" --- Test")]
    /// <summary>
    /// Player攻撃フェーズのトリガー
    /// </summary>
    [SerializeField]
    private bool isPlayerAttackPhaseOn = false;
    public bool IsPlayerAttackPhaseOn { get => this.isPlayerAttackPhaseOn; }
    private bool didPlayerContactEnemy = false;
    /// <summary>
    /// Enemy攻撃フェーズのトリガー
    /// </summary>
    [SerializeField]
    private bool isEnemyAttackPhaseOn = false;
    public bool IsEnemyAttackPhaseOn { get => this.isEnemyAttackPhaseOn; }
    private bool didEnemyContactPlayer = false;
    /// <summary>
    /// Playerオブジェクトチェックフェーズのトリガー
    /// </summary>
    [SerializeField]
    private bool isPlayerCheckObjectPhaseOn = false;
    private bool didPlayerContactObject = false;

    #endregion


    #region [func]

    #region [01. 初期化]
    /// <summary>
    /// 各種データを初期化
    /// </summary>
    private void InitData()
    {
        this.isPlayerAttackPhaseOn = true;
        this.isEnemyAttackPhaseOn = true;
        this.isPlayerCheckObjectPhaseOn = true;

        this.didPlayerContactEnemy = false;
        this.didEnemyContactPlayer = false;
        this.didPlayerContactObject = false;
    }
    #endregion
    
    #region [02. トリガーセット]
    /// <summary>
    /// Player攻撃フェーズのトリガーをセット
    /// </summary>
    /// <param name="state"></param>
    public void SetPlayerAttackPhaseTrigger(bool state)
    {
        this.isPlayerAttackPhaseOn = state;
    }
    public void SetPlayerContactEnemyTrigger(bool state)
    {
        this.didPlayerContactEnemy = state;
    }

    /// <summary>
    /// Enemy攻撃フェーズのトリガーをセット
    /// </summary>
    /// <param name="state"></param>
    public void SetEnemyAttackPhaseTrigger(bool state)
    {
        this.isEnemyAttackPhaseOn = state;
    }
    public void SetEnemyContactPlayerTrigger(bool state)
    {
        this.didEnemyContactPlayer = state;
    }

    /// <summary>
    /// Playerオブジェクトチェックフェーズのトリガーをセット
    /// </summary>
    /// <param name="state"></param>
    public void SetPlayerCheckObjectPhaseTrigger(bool state)
    {
        this.isPlayerCheckObjectPhaseOn = state;
    }
    public void SetPlayerContactObjectTrigger(bool state)
    {
        this.didPlayerContactObject = state;
    }
    #endregion

    #region [03. Turn Managing 制御全般]
    /// <summary>
    /// 移動ボタン押下時の処理
    /// </summary>
    /// <param name="directionStr"></param>
    public void OnClickMoveButton(string directionStr)
    {
        // PlayerTurnコルーチン開始
        this.PlayerTurnAsync(directionStr);
    }

    /// <summary>
    /// PlayerTurnコルーチン開始
    /// </summary>
    /// <param name="directionStr"></param>
    public void PlayerTurnAsync(string directionStr)
    {
        // コルーチンスタート
        GlobalCoroutine.Play(this.PlayerTurn(directionStr), "PlayerTurn", null);
    }

    /// <summary>
    /// PlayerTurnコルーチン
    /// </summary>
    /// <param name="directionStr"></param>
    /// <returns></returns>
    IEnumerator PlayerTurn(string directionStr)
    {
        Debug.LogFormat($"【Coroutine】  Player Turn Activated", DColor.white);

        // 各種データを初期化
        this.InitData();
        
        // ボタンタッチ無効
        this.uIbuttonController.DisableButtonTouch();
        
        // TurnDialog表示：Player
        this.uIDialogController.ShowTurnDialog(this.uIDialogController.PlayerTurnDialog, () =>
        {
            // Player移動開始
            this.playerMovementController.PlayerMove(directionStr);
        });
        
        // Player移動終了まで待機
        yield return new WaitForSeconds(2f);

        // Loop処理
        while (this.isPlayerAttackPhaseOn)
        {
            // トリガー次第でLoopを終了
            if (!this.didPlayerContactEnemy)
            {
                this.isPlayerAttackPhaseOn = false;
            }

            yield return null;
        }
        
        // TurnDialog非表示：Player
        this.uIDialogController.CloseTurnDialog(this.uIDialogController.PlayerTurnDialog, () =>
        {
            // コルーチン停止
            this.StopPlayerTurnCoroutine(directionStr);
        });
    }

    /// <summary>
    /// PlayerTurnコルーチン停止
    /// </summary>
    private void StopPlayerTurnCoroutine(string directionStr)
    {
        DOVirtual.DelayedCall(.1f, () =>
        {
            GlobalCoroutine.Stop("PlayerTurn");
            
            // EnemyTurnコルーチン開始
            this.EnemyTurnAsync(directionStr);
        });
    }
    
    /// <summary>
    /// EnemyTurnコルーチン開始
    /// </summary>
    /// <param name="directionStr"></param>
    public void EnemyTurnAsync(string directionStr)
    {
        // コルーチンスタート
        GlobalCoroutine.Play(this.EnemyTurn(directionStr), "EnemyTurn", null);
    }

    /// <summary>
    /// EnemyTurnコルーチン
    /// </summary>
    /// <param name="directionStr"></param>
    /// <returns></returns>
    IEnumerator EnemyTurn(string directionStr)
    {
        Debug.LogFormat($"【Coroutine】  Enemy Turn Activated", DColor.white);
        
        // TurnDialog表示：Enemy
        this.uIDialogController.ShowTurnDialog(this.uIDialogController.EnemyTurnDialog, () =>
        {
            // プレイヤーの移動と同期して敵移動開始
            EnemyManager.Instance.SetEnemyMovement(directionStr, () =>
            {
                DOVirtual.DelayedCall(1.2f, () =>
                {
                    // Enemy移動終了後、現在位置での移動可能方向を検索
                    EnemyManager.Instance.SetEnemyMovableDirection();
                });
            });
        });
        
        // Enemy移動終了まで待機
        yield return new WaitForSeconds(2f);

        // Loop処理
        while (this.isEnemyAttackPhaseOn)
        {
            // トリガー次第でLoopを終了
            if (!this.didEnemyContactPlayer)
            {
                this.isEnemyAttackPhaseOn = false;
            }

            yield return null;
        }
        
        // TurnDialog非表示：Player
        this.uIDialogController.CloseTurnDialog(this.uIDialogController.EnemyTurnDialog, () =>
        {
            // EnemyTurnコルーチン停止
            this.StopEnemyTurnCoroutine();
        });
    }
    
    /// <summary>
    /// EnemyTurnコルーチン停止
    /// </summary>
    private void StopEnemyTurnCoroutine()
    {
        DOVirtual.DelayedCall(.1f, () =>
        {
            GlobalCoroutine.Stop("EnemyTurn");
         
            // CheckEventコルーチン開始
            this.CheckEventAsync();
        });
    }
    
    /// <summary>
    /// CheckEventコルーチン開始
    /// </summary>
    public void CheckEventAsync()
    {
        // コルーチンスタート
        GlobalCoroutine.Play(this.CheckEvent(), "CheckEvent", null);
    }

    /// <summary>
    /// CheckEventコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckEvent()
    {
        Debug.LogFormat($"【Coroutine】  Check Event Activated", DColor.white);
        
        // ItemDialogを1回のみ表示するためのトリガー
        bool isItemDialogOpened = false;
        
        // Loop処理
        while (this.isPlayerCheckObjectPhaseOn)
        {
            // トリガー次第でLoopを終了
            if (!this.didPlayerContactObject)
            {
                this.isPlayerCheckObjectPhaseOn = false;
            }

            if (!isItemDialogOpened)
            {
                if (PlayerStatusManager.Instance.IsSourceItem)
                {
                    // ItemDialogを表示（初回のみ）
                    this.uIDialogController.ShowDialog(this.uIDialogController.Dialog_Item.transform, 1);
                    isItemDialogOpened = true;
                }
            }

            yield return null;
        }

        // CheckEventコルーチン停止
        this.StopCheckEventCoroutine();
    }
    
    /// <summary>
    /// CheckEventコルーチン停止
    /// </summary>
    void StopCheckEventCoroutine()
    {
        DOVirtual.DelayedCall(.1f, () =>
        {
            GlobalCoroutine.Stop("CheckEvent");

            DOVirtual.DelayedCall(.2f, () =>
            {
                // ボタンタッチ有効
                this.uIbuttonController.EnableButtonTouchExpectMovementButton();
            });
        });
    }
    #endregion

    #region [04. MapInfo取得]

    #region [var]
    /// <summary>
    /// 移動可能方向のトリガー
    /// </summary>
    [Header(" --- Player移動関連")]
    [SerializeField] 
    private bool canMoveToNorth = false;
    public bool CanMoveToNorth { get => canMoveToNorth; }   
    [SerializeField] 
    private bool canMoveToEast = false;
    public bool CanMoveToEast { get => canMoveToEast; }   
    [SerializeField] 
    private bool canMoveToSouth = false;
    public bool CanMoveToSouth { get => canMoveToSouth; }   
    [SerializeField] 
    private bool canMoveToWest = false;
    public bool CanMoveToWest { get => canMoveToWest; }
    #endregion
    
    
    #region [func]
    /// <summary>
    /// 現在座標のMapInfoを取得
    /// </summary>
    public void GetMapInfo(PlayerScriptController playerScriptController)
    {
        // Playerの現在座標
        this.playerMovementController = playerScriptController.PlayerMovementController;
        var playerPos = this.playerMovementController.transform.position;
        
        // 生成済みMapリストと比較
        this.CompareWithMapInfo(playerPos);
    }
    public void GetMapInfo()
    {
        // Playerの現在座標
        var playerPos = this.playerMovementController.transform.position;
        
        // 生成済みMapリストと比較
        this.CompareWithMapInfo(playerPos);
    }
    
    /// <summary>
    /// 生成済みMapリストと比較
    /// </summary>
    /// <param name="playerPos"></param>
    private void CompareWithMapInfo(Vector3 playerPos)
    {
        foreach (var map in MapCollector.Instance.collectedMapList)
        {
            if (map.transform.position == playerPos)
            {
                // 各トリガーをセット
                var info = map.gameObject.GetComponent<MapInfo>();
                this.canMoveToNorth = info.CanMoveToNorth;
                this.canMoveToEast = info.CanMoveToEast;
                this.canMoveToSouth = info.CanMoveToSouth;
                this.canMoveToWest = info.CanMoveToWest;

                // トリガーによって各MovementButtonの表示切り替え
                this.SetMovementButton();
                
                return;
            }
        }
    }

    /// <summary>
    /// トリガーによって各MovementButtonの表示切り替え
    /// </summary>
    private void SetMovementButton()
    {
        this.uIbuttonController.SetEachMovementButtonEnableState(this.uIbuttonController.NorthButton, this.canMoveToNorth);
        this.uIbuttonController.SetEachMovementButtonEnableState(this.uIbuttonController.EastButton, this.canMoveToEast);
        this.uIbuttonController.SetEachMovementButtonEnableState(this.uIbuttonController.SouthButton, this.canMoveToSouth);
        this.uIbuttonController.SetEachMovementButtonEnableState(this.uIbuttonController.WestButton, this.canMoveToWest);
    }
    #endregion
    
    #endregion
    
   
    
    #endregion
    
    #endregion
}