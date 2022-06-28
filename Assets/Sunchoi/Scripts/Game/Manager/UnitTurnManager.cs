using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UnitTurnManager : MonoBehaviour
{
    #region [var]

    #region [00. コンストラクタ]

    /// <summary>
    /// インスタンス
    /// </summary>
    public static UnitTurnManager Instance { get; private set; }
   
    [Header(" --- Reference")]
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
    
    #endregion
    
    #region [02. Turn メイン制御]
   
    [Header(" --- 移動処理が終了したEnemy数")]
    /// <summary>
    /// 移動処理が終了したEnemy数
    /// </summary>
    [SerializeField]
    private int finishedMovementEnemyCount;

    [Header(" --- トリガー")]
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
    private bool isPlayerCheckEventPhaseOn = false;

    [Header(" --- コルーチン")]
    /// <summary>
    /// 一時停止および再開用のIEnumerator
    /// </summary>
    private IEnumerator coroutine;
    
    [Header(" --- Map Info")]
    /// <summary>
    /// Playerが止まったMapのMapInfo
    /// </summary>
    private MapInfo mapInfo = null;
    
    [Header(" --- EnemeyTurnのターン終了関連")]
    /// <summary>
    /// ターン開始時に記録するEnemy数
    /// </summary>
    [SerializeField]
    private int turnActionStartingEnemyCount = 0;
    /// <summary>
    /// 各Enemyの行動終了毎に記録するEnemy数
    /// </summary>
    [SerializeField]
    private int turnActionFinishedEnemyCount = 0;
    #endregion

    #region [03. Map Info 取得関連]
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
    
    #endregion
    
    
    #region [func]

    #region [00. コンストラクタ]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Start()
    {
        // インスタンス
        Instance = this;

        // データ初期化
        this.InitData();
    }
    #endregion
    
    #region [01. 初期化]
    /// <summary>
    /// 各種データを初期化
    /// </summary>
    private void InitData()
    {
        this.turnActionStartingEnemyCount = 0;
        this.turnActionFinishedEnemyCount = 0;
        
        this.isPlayerAttackPhaseOn = false;
        this.isEnemyAttackPhaseOn = false;
        this.isPlayerCheckEventPhaseOn = false;

        this.didPlayerContactEnemy = false;
        this.didEnemyContactPlayer = false;
        
        this.mapInfo = null;
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
    public void SetPlayerCheckEventPhaseTrigger(bool state)
    {
        this.isPlayerCheckEventPhaseOn = state;
    }
    #endregion
    
    #region [03. Turn メイン制御]

    #region [001. Player Turn]
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
        if (this.coroutine != null)
            this.coroutine = null;
        coroutine = this.PlayerTurn(directionStr);
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// PlayerTurnコルーチン
    /// </summary>
    /// <param name="directionStr"></param>
    /// <returns></returns>
    IEnumerator PlayerTurn(string directionStr)
    {
        Debug.LogFormat($"Coroutine [PlayerTurn] Activated", DColor.white);

        // 各種データを初期化
        this.InitData();

        // Loopトリガーをセット
        this.isPlayerAttackPhaseOn = true;
        
        // ボタンタッチ無効
        this.uIbuttonController.DisableButtonTouch();
        
        // TurnDialog表示：Player
        this.uIDialogController.ShowTurnDialog(this.uIDialogController.PlayerTurnDialog, () =>
        {
            // Player移動開始
            this.playerMovementController.PlayerMove(directionStr, () =>
            {
                // Playerの現在座標
                var playerPos = this.playerMovementController.transform.position;
                // Player座標と一致するMapを検索
                foreach (var map in MapCollector.Instance.collectedMapList)
                {
                    // 一致した場合
                    if (map.transform.position == playerPos)
                    {
                        // MapのMapInfoを更新
                        this.mapInfo = map.GetComponent<MapInfo>();
                    }
                }
                
                // MapEventを消化したMapをOpenStateに変更
                this.mapInfo.SetMapSpriteToOpenState();
            });
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
            // PlayerTurnコルーチン停止
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
            StopCoroutine(this.coroutine);
            this.coroutine = null;
            
            // EnemyTurnコルーチン開始
            this.EnemyTurnAsync();
        });
    }

    /// <summary>
    /// 再生中PlayerTurnコルーチンの一時停止
    /// </summary>
    public void StopPlayerTurnCoroutineAtMoment()
    {
        StopCoroutine(this.coroutine);
    }

    /// <summary>
    /// 一時停止中のPlayerTurnコルーチンの再開
    /// </summary>
    public void StartPlayerTurnCoroutineAgain()
    {
        StartCoroutine(this.coroutine);
    }
    #endregion
    
    #region [002. Enemy Turn]
    /// <summary>
    /// EnemyTurnコルーチン開始
    /// </summary>
    public void EnemyTurnAsync()
    {
        // Map上にEnemyが存在している場合
        if (EnemyManager.Instance.EnemyMovementControllerList.Count != 0)
        {
            // コルーチンスタート
            if (this.coroutine != null)
                this.coroutine = null;
            coroutine = this.EnemyTurn();
            StartCoroutine(coroutine);
        }
        // Map上にEnemyが存在していない場合
        else
        {
            //　CheckEventコルーチン開始
            this.CheckEventAsync();
        }
    }
    
    /// <summary>
    /// 行動終了のEnemy数をカウント
    /// </summary>
    public void SetTurnActionFinishedEnemyCount()
    {
        this.turnActionFinishedEnemyCount += 1;
    }

    /// <summary>
    /// EnemyTurnコルーチン
    /// </summary>
    public IEnumerator EnemyTurn()
    {
        Debug.LogFormat($"Coroutine [EnemyTurn] Activated", DColor.white);

        // 全Enemy数を記録
        this.turnActionStartingEnemyCount = EnemyManager.Instance.EnemyMovementControllerList.Count;
        
        // Loopトリガーをセット
        this.isEnemyAttackPhaseOn = true;
        
        // TurnDialog表示：Enemy
        this.uIDialogController.ShowTurnDialog(this.uIDialogController.EnemyTurnDialog, () =>
        {
            // Enemy移動開始
            EnemyManager.Instance.EnemyMoveEachAsync();
        });
        
        // Enemy移動終了まで待機
        yield return new WaitForSeconds(2f);
        
        // Loop処理
        while (this.isEnemyAttackPhaseOn)
        {
            // 行動終了のEnemy数が全体Enemy数に達すればLoopを終了
            if (this.turnActionFinishedEnemyCount == this.turnActionStartingEnemyCount)
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
            StopCoroutine(this.coroutine);
            this.coroutine = null;
         
            // 一時保存状態のEnemyを破棄
            EnemyManager.Instance.DestroyTempEnemy();
            
            // CheckEventコルーチン開始
            this.CheckEventAsync();
        });
    }

    /// <summary>
    /// 再生中EnemyTurnコルーチンの一時停止
    /// </summary>
    public void StopEnemyTurnCoroutineAtMoment()
    {
        StopCoroutine(this.coroutine);
    }

    /// <summary>
    /// 一時停止中のEnemyTurnコルーチンの再開
    /// </summary>
    public void StartEnemyTurnCoroutineAgain()
    {
        StartCoroutine(this.coroutine);
    }
    #endregion
    
    #region [003. Check Event Turn]
    /// <summary>
    /// CheckEventコルーチン開始
    /// </summary>
    public void CheckEventAsync()
    {
        // コルーチンスタート
        // コルーチンスタート
        if (this.coroutine != null)
            this.coroutine = null;
        coroutine = this.CheckEvent();
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// CheckEventコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckEvent()
    {
        Debug.LogFormat($"Coroutine [CheckEvent] Activated", DColor.white);

        // Loopトリガーをセット
        this.isPlayerCheckEventPhaseOn = true;
        
        // ItemDialogを1回のみ表示するためのトリガー
        bool isEventDialogOpened = false;

        // Loop処理
        while (this.isPlayerCheckEventPhaseOn)
        {
            // Event発生ありの場合、EventDialogを表示
            // 処理後、Loopを終了
            if (this.mapInfo != null && this.mapInfo.IsMapEventFinished == false)
            {
                if (!isEventDialogOpened)
                {
                    // ItemDialogを表示（初回のみ）
                    this.uIDialogController.ShowEventDialog(
                        this.uIDialogController.Dialog_Event.transform
                        , this.mapInfo
                        , () =>
                        {
                            // 消化したMapEventをFinishedStateに変更
                            this.mapInfo.SetMapEventToFinishedState();
                        });
                    
                    isEventDialogOpened = true;
                }
            }

            // Event発生なしの場合、即Loopを終了
            if (this.mapInfo != null && this.mapInfo.IsMapEventFinished == true)
            {
                this.isPlayerCheckEventPhaseOn = false;
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
            StopCoroutine(this.coroutine);
            this.coroutine = null;

            DOVirtual.DelayedCall(.2f, () =>
            {
                // ボタンタッチ有効
                this.uIbuttonController.EnableButtonTouchExpectMovementButton();
            });
        });
    }
    #endregion
    
    #endregion
    
    #region [04. MapInfo取得]
    
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
    /// </summary>s
    private void SetMovementButton()
    {
        this.uIbuttonController.SetEachMovementButtonEnableState(this.uIbuttonController.NorthButton, this.canMoveToNorth);
        this.uIbuttonController.SetEachMovementButtonEnableState(this.uIbuttonController.EastButton, this.canMoveToEast);
        this.uIbuttonController.SetEachMovementButtonEnableState(this.uIbuttonController.SouthButton, this.canMoveToSouth);
        this.uIbuttonController.SetEachMovementButtonEnableState(this.uIbuttonController.WestButton, this.canMoveToWest);
    }
    
    #endregion
    
    #endregion
}