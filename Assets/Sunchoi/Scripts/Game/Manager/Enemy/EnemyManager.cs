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

        // TODO:: テスト時のみの処理
        this.SetEnemyOnEnemyPoint(new Vector2(-15f, 10f));
    }

    #endregion

    #endregion
    
    
    #region [02. Setting Enemy List]

    #region [var]

    [Header(" --- Prefab")]
    /// <summary>
    /// Enemy Prefab
    /// </summary>
    [SerializeField]
    private GameObject enemyPrafab;
    /// <summary>
    /// EnemyRoot Transform
    /// </summary>
    [SerializeField]
    private Transform enemyRoot;
    [Header(" --- リスト")]
    /// <summary>
    /// Map上のすべての敵のEnemyMovementControllerリスト
    /// </summary>
    [SerializeField]
    private List<EnemyMovementController> enemyMovementControllerList = new List<EnemyMovementController>();
    #endregion


    #region [func]
    /// <summary>
    /// Enemyを生成、各種データをセット
    /// </summary>
    public void SetEnemyOnEnemyPoint(Vector2 position)
    {
        // 生成
        var enemyObj = Instantiate(this.enemyPrafab, this.enemyRoot);
        
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
    public void SetEnemyMovement(string directionStr)
    {
        foreach (var enemyMovementController in this.enemyMovementControllerList)
        {
            enemyMovementController.MoveEnemy(directionStr);
        }
    }
    #endregion

    #endregion
}
