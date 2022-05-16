using System;
using System.Collections;
using System.Collections.Generic;
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
    
    
    #region [03. Enemy Movement]

    #region [var]
    
    
    
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
            enemyMovementController.MoveEnemy(directionStr);
        }

        onFinished?.Invoke();
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