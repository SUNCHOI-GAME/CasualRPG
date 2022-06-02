using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region [00. Common]

    #region [var]

    

    #endregion

    #endregion
    
    #region [01. コンストラクタ]

    #region [var]
    /// <summary>
    /// インスタンス
    /// </summary>
    public static EnemyManager Instance { get; private set; }
    #endregion
    
    
    #region [func]

    private void Start()
    {
        // インスタンス
        Instance = this;
        // 破棄不可
        DontDestroyOnLoad(this.gameObject);
    }

    #endregion

    #endregion
    
    
    #region [02. Setting Enemy List]

    #region [var]

    [Header(" --- リスト")]
    /// <summary>
    /// Map上のすべての敵のEnemyMovementControllerリスト
    /// </summary>
    [SerializeField]
    private List<EnemyMovementController> enemyMovementControllerList = new List<EnemyMovementController>();
    public List<EnemyMovementController> EnemyMovementControllerList { get => this.enemyMovementControllerList; }
    #endregion


    #region [func]
    /// <summary>
    /// Enemyを生成、各種データをセット
    /// </summary>
    public void SetEnemyOnMap(GameObject enemyPrafab, Transform enemyRoot, Vector2 position)
    {
        // 生成
        var enemyObj = Instantiate(enemyPrafab, enemyRoot);
        
        var enemyScript = enemyObj.GetComponent<EnemyScriptController>();
        // 初期座標をセット
        enemyScript.EnemyMovementController.transform.position = position;
        // 敵が持つEnemyMovementControllerをリストに追加
        this.enemyMovementControllerList.Add(enemyScript.EnemyMovementController);
    }
    #endregion

    #endregion

    #region [03. EnemyDestroy]

    #region [var]
    /// <summary>
    /// Enemyを臨時保存
    /// </summary>
    private EnemyMovementController tempEnemyMovementController;
    #endregion

    #region [func]
    /// <summary>
    /// 該当するEnemyを破棄
    /// </summary>
    public void DestroySpecificEnemy(EnemyMovementController targetEnemyMovementController)
    {
        // リストから排除
        this.enemyMovementControllerList.Remove(targetEnemyMovementController);
        // 破棄
        Destroy(targetEnemyMovementController.transform.parent.gameObject);
    }
    
    /// <summary>
    /// 破棄予定のEnemyを臨時的にゲームから除外
    /// （EnemyTurn終了後破棄）
    /// </summary>
    /// <param name="targetEnemyMovementController"></param>
    public void ExcludeEnemyTemporarily(EnemyMovementController targetEnemyMovementController)
    {
        this.tempEnemyMovementController = targetEnemyMovementController;
        
        targetEnemyMovementController.transform.localScale = Vector3.zero;
    }

    /// <summary>
    /// 臨時保存中のEnemyを破棄
    /// </summary>
    public void DestroyTempEnemy()
    {
        if(tempEnemyMovementController != null)
            DestroySpecificEnemy(tempEnemyMovementController);
    }
    #endregion
    

    #endregion
    
    
    #region [04. Enemy Movement]

    #region [var]
    /// <summary>
    /// 一時停止および再開用のIEnumerator
    /// </summary>
    private IEnumerator coroutine;
    /// <summary>
    /// コルーチン内Loop用変数
    /// </summary>
    private int finishedEnemyMovementCount = 0;
    #endregion


    #region [func]
    /// <summary>
    /// SetEnemyMovement
    /// </summary>
    public void SetEnemyMovement(string directionStr, Action onFinished)
    {
        // Enemyリストに存在するすべてのEnemyを移動
        foreach (var enemyMovementController in this.enemyMovementControllerList)
        {
            enemyMovementController.SetNextDirection();
        }

        onFinished?.Invoke();
    }

    /// <summary>
    /// EnemyMoveEachコルーチン開始
    /// </summary>
    public void EnemyMoveEachAsync()
    {
        // EnemyTurnコルーチンを一時停止
        UnitTurnManager.Instance.StopEnemyTurnCoroutineAtMoment();
        
        // 初期化
        this.finishedEnemyMovementCount = 0;
        
        // コルーチンスタート
        if (this.coroutine != null)
            this.coroutine = null;
        this.coroutine = this.EnemyMoveEach();
        StartCoroutine(this.coroutine);
    }
    
    /// <summary>
    /// EnemyMoveEachコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyMoveEach()
    {
        // Loop処理
        while (this.finishedEnemyMovementCount < this.enemyMovementControllerList.Count)
        {
            this.enemyMovementControllerList[finishedEnemyMovementCount].SetNextDirection();

            Debug.LogFormat($"Move No.{finishedEnemyMovementCount + 1} Enemy is Moving", DColor.cyan);
            
            yield return new WaitForSeconds(1f);
            
            this.finishedEnemyMovementCount += 1;
        }
        
        Debug.LogFormat($"Every Enemy Movement is Finished", DColor.cyan);

        // Enemy移動終了後、現在位置での移動可能方向を検索
        this.SetEnemyMovableDirection();
        
        // 初期化
        this.finishedEnemyMovementCount = 0;
        
        // EnemyMoveEachコルーチン停止
        this.StopEnemyMoveEachCoroutine();
    }
    
    /// <summary>
    /// EnemyMoveEachコルーチン停止
    /// </summary>
    private void StopEnemyMoveEachCoroutine()
    {
        DOVirtual.DelayedCall(.1f, () =>
        {
            StopCoroutine(coroutine);
            
            // EnemyTurnコルーチンを再開
            UnitTurnManager.Instance.StartEnemyTurnCoroutineAgain();
        });
    }
    
    /// <summary>
    /// 再生中EnemyMoveEachコルーチンの一時停止
    /// </summary>
    public void StopEnemyMoveEachCoroutineAtMoment()
    {
        StopCoroutine(coroutine);
    }
    
    /// <summary>
    /// 一時停止中のEnemyMoveEachコルーチンの再開
    /// </summary>
    public void StartEnemyMoveEachCoroutineAgain()
    {
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// 移動可能方向をセット
    /// </summary>
    public void SetEnemyMovableDirection()
    {
        // Enemyリストに存在するすべてのEnemyにセット
        foreach (var enemyMovementController in this.enemyMovementControllerList)
        {
            enemyMovementController.GetMapInfo();
        }
    }
    
    #endregion

    #endregion
}
