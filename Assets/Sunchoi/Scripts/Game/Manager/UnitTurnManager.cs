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
    /// UILogController
    /// </summary>
    [SerializeField]
    private UILogController uILogController;

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
    private bool didPlayerContactEnemy = false;

    /// <summary>
    /// Enemy攻撃フェーズのトリガー
    /// </summary>
    [SerializeField]
    private bool isEnemyAttackPhaseOn = false;
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
        this.PlayerMoveAsync(directionStr);
    }

    /// <summary>
    /// TurnManagementコルーチンの開始
    /// </summary>
    /// <param name="directionStr"></param>
    public void PlayerMoveAsync(string directionStr)
    {
        // コルーチンスタート
        GlobalCoroutine.Play(this.TurnManaging(directionStr), "TurnManaging", null);
    }

    /// <summary>
    /// TurnManagementコルーチン
    /// </summary>
    /// <param name="directionStr"></param>
    /// <returns></returns>
    IEnumerator TurnManaging(string directionStr)
    {
        Debug.LogFormat($"【Coroutine】  Turn Managing Activated", DColor.white);

        // 各種データを初期化
        this.InitData();

        // ボタンタッチ無効
        this.uIbuttonController.DisableButtonTouch();

        // Player移動開始
        this.playerMovementController.PlayerMove(directionStr);

        yield return new WaitForSeconds(1f);

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

        // プレイヤーの移動と同期して敵移動開始
        EnemyManager.Instance.SetEnemyMovement(directionStr, () =>
        {
            DOVirtual.DelayedCall(1.2f, () =>
            {
                // Enemy移動終了後、現在位置での移動可能方向を検索
                EnemyManager.Instance.SetEnemyMovableDirection();
            });
        });

        yield return new WaitForSeconds(1f);

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

        // ItemLogを1回のみ表示するためのトリガー
        bool isItemLogOpened = false;
        
        // Loop処理
        while (this.isPlayerCheckObjectPhaseOn)
        {
            // トリガー次第でLoopを終了
            if (!this.didPlayerContactObject)
            {
                this.isPlayerCheckObjectPhaseOn = false;
            }

            if (!isItemLogOpened)
            {
                if (PlayerStatusManager.Instance.IsSourceItem)
                {
                    // ItemLogを表示（初回のみ）
                    this.uILogController.ShowLog(this.uILogController.Log_Item.transform);
                    isItemLogOpened = true;
                }
            }

            yield return null;
        }

        // コルーチン停止
        this.StopTurnManagingCoroutine();
    }

    /// <summary>
    /// コルーチン停止
    /// </summary>
    private void StopTurnManagingCoroutine()
    {
        DOVirtual.DelayedCall(.1f, () =>
        {
            GlobalCoroutine.Stop("TurnManaging");

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