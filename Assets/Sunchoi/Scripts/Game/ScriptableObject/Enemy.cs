using UnityEngine;

[CreateAssetMenu(fileName ="New Enemy", menuName = "Enemy/Create New Enemy")]
public class Enemy : ScriptableObject
{
    /// <summary>
    /// Enemyの名前
    /// </summary>
    public string enemyName;
    /// <summary>
    /// EnemyのSprite
    /// </summary>
    public Sprite sprite;
    /// <summary>
    /// Battle時、Enemy表示やアニメーションのために生成するPrefab
    /// </summary>
    public GameObject battlePrefab;
    
    /// <summary>
    /// Status
    /// </summary>
    public int minLevel;
    public int maxLevel;
    public int minHp;
    public int maxHp;
    public int minAttack;
    public int maxAttack;
    public int minCritical;
    public int maxCritical;
    public int minDefence;
    public int maxDefence;
    public int minAgility;
    public int maxAgility;

    /// <summary>
    /// 倒した際、Playerが得る経験値
    /// </summary>
    public int expValue;
}
