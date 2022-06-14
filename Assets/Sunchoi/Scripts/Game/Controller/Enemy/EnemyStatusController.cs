using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusController : MonoBehaviour
{
    #region [01. コンストラクタ]

    #region [var]
    [Header(" --- Info")]
    /// <summary>
    /// ScriptableIbject上のEnemy情報 
    /// </summary>
    [SerializeField]
    private Enemy enemy;
    public Enemy Enemy { get => this.enemy; }
    #endregion

    #region [func]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Start()
    {
        // Enemyのデータをセット
        this.SetEnemyData();
    }
    #endregion
    
    #endregion
    
    
    
    #region [01. Set Enemy Data]

    #region [var]
    [Header(" --- Status")]
    /// <summary>
    /// Name
    /// </summary>
    [SerializeField]
    private string name = "";
    /// <summary>
    /// Level
    /// </summary>
    [SerializeField]
    private int level = 0;
    /// <summary>
    /// HP
    /// </summary>
    [SerializeField]
    private int hp = 0;
    /// <summary>
    /// Attack Damage
    /// </summary>
    [SerializeField]
    private int attack = 0;
    /// <summary>
    /// Critical Chance
    /// </summary>
    [SerializeField]
    private int critical = 0;
    /// <summary>
    /// Defence
    /// </summary>
    [SerializeField]
    private int defence = 0;

    [Header(" --- Status Offset")]
    /// <summary>
    /// HPOffest
    /// </summary>
    [SerializeField]
    private int hpOffest = 10;
    /// <summary>
    /// Attack Damage Offest
    /// </summary>
    [SerializeField]
    private int atkOffest = 3;
    /// <summary>
    /// Critical Chance Offest
    /// </summary>
    [SerializeField]
    private int criOffest = 2;
    /// <summary>
    /// Defence Offest
    /// </summary>
    [SerializeField]
    private int defOffest = 2;
    #endregion

    #region [func]
    /// <summary>
    /// Enemyのデータをセット
    /// </summary>
    private void SetEnemyData()
    {
        this.name = enemy.name;
        this.level = UnityEngine.Random.Range(enemy.minLevel, enemy.maxLevel + 1);
        this.hp = UnityEngine.Random.Range(enemy.minHp, enemy.maxHp + 1)
                  + (this.hpOffest * this.level);
        this.attack = UnityEngine.Random.Range(enemy.minAttack, enemy.maxAttack + 1)
                      + (this.atkOffest * this.level);
        this.critical = UnityEngine.Random.Range(enemy.minCritical, enemy.maxCritical + 1)
                        + (this.criOffest * this.level);
        this.defence = UnityEngine.Random.Range(enemy.minDefence, enemy.maxDefence + 1)
                       + (this.defOffest * this.level);
    }
    #endregion
    
    #endregion
    
    
    
    
    
    

    
}
