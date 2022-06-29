using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyStatusController : MonoBehaviour
{
    #region [var]

    #region [00. コンストラクタ]
    [Header(" --- Info")]
    /// <summary>
    /// ScriptableIbject上のEnemy情報 
    /// </summary>
    [SerializeField]
    private Enemy enemy;
    public Enemy Enemy { get => this.enemy; }
    #endregion
    
    #region [01. Set Enemy Data]
    [Header(" --- Status")]
    /// <summary>
    /// Name
    /// </summary>
    private string enemyName = "";
    public string Name { get => this.enemyName; }
    /// <summary>
    /// Level
    /// </summary>
    private int level = 0;
    public int Level { get => this.level; }
    /// <summary>
    /// HP
    /// </summary>
    private int currentHp = 0;
    public int CurrentHp { get => this.currentHp; }
    /// <summary>
    /// Max HP
    /// </summary>
    private int maxHp = 0;
    public int MaxHp { get => this.maxHp; }
    /// <summary>
    /// Attack Damage
    /// </summary>
    private int attack = 0;
    public int Attack { get => this.attack; }
    /// <summary>
    /// Critical Chance
    /// </summary>
    private int critical = 0;
    public int Critical { get => this.critical; }
    /// <summary>
    /// Defence
    /// </summary>
    private int defence = 0;
    public int Defence { get => this.defence; }
    /// <summary>
    /// Agility
    /// </summary>
    private int agility = 0;
    public int Agility { get => this.agility; }
    /// <summary>
    /// ExpValue
    /// </summary>
    private int expValue = 0;
    public int ExpValue { get => this.expValue; }

    [Header(" --- Status Offset")]
    /// <summary>
    /// HPOffest
    /// </summary>
    [SerializeField]
    private int hpOffset = 10;
    /// <summary>
    /// Attack Damage Offest
    /// </summary>
    [SerializeField]
    private int atkOffset = 3;
    /// <summary>
    /// Critical Chance Offest
    /// </summary>
    [SerializeField]
    private int criOffset = 2;
    /// <summary>
    /// Defence Offest
    /// </summary>
    [SerializeField]
    private int defOffset = 2;
    /// <summary>
    /// Agility Offest
    /// </summary>
    [SerializeField]
    private int agiOffset = 2;
    /// <summary>
    /// Exp Offest
    /// </summary>
    [SerializeField]
    private int expOffset = 10
        ;
    #endregion
    
    #endregion
    
    
    #region [func]

    #region [00. コンストラクタ]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Start()
    {
        // Enemyのデータをセット
        this.SetEnemyData();
    }
    #endregion
    
    #region [01. Data Set]

    /// <summary>
    /// Enemyのデータをセット
    /// </summary>
    private void SetEnemyData()
    {
        this.enemyName = enemy.enemyName;
        this.level = UnityEngine.Random.Range(enemy.minLevel, enemy.maxLevel + 1);
        this.maxHp = UnityEngine.Random.Range(enemy.minHp, enemy.maxHp + 1)
                     + (this.hpOffset * this.level);
        this.currentHp = maxHp;
        this.attack = UnityEngine.Random.Range(enemy.minAttack, enemy.maxAttack + 1)
                      + (this.atkOffset * this.level);
        this.critical = UnityEngine.Random.Range(enemy.minCritical, enemy.maxCritical + 1)
                        + (this.criOffset * this.level);
        this.defence = UnityEngine.Random.Range(enemy.minDefence, enemy.maxDefence + 1)
                       + (this.defOffset * this.level);
        this.agility = UnityEngine.Random.Range(enemy.minAgility, enemy.maxAgility + 1)
                       + (this.agiOffset * this.level);
        this.expValue = enemy.expValue + (this.expOffset * this.level);
    }

    #endregion

    #region [02. Damgaed 処理]

    /// <summary>
    /// Enemyがダメージを負った場合
    /// </summary>
    /// <param name="damageValue"></param>
    public void EnemyDamaged(int damageValue, Action onFinished)
    {
        // ダメージ - 防御力
        var calculatedDamage = damageValue - this.defence;
        if (calculatedDamage <= 0) calculatedDamage = 0;
        
        Debug.LogFormat($" Enemy Damaged as {calculatedDamage} ", DColor.yellow);
        
        // Damage Log Animation再生
        BattleManager.Instance.PlayUnitDamageAnim(
            BattleManager.Instance.EnemyDamageLogAnimator,
            BattleManager.Instance.EnemyDamageLogText, calculatedDamage);
        
        // UnitActionLogを表示
        //BattleManager.Instance.UnitActionLog($"{this.enemyName}は\n{calculatedDamage}のダメージを受けた", () => { });
        
        // HPのStatus更新
        var newHp = this.currentHp - calculatedDamage;
        if (newHp <= 0) newHp = 0;
        this.currentHp = newHp;
        
        onFinished?.Invoke();
    }

    #endregion
    
    #endregion
}
