using UnityEngine;

[CreateAssetMenu(fileName ="New Enemy", menuName = "Enemy/Create New Enemy")]
public class Enemy : ScriptableObject
{
    /// <summary>
    /// Name
    /// </summary>
    public string name;
    /// <summary>
    /// Sprite
    /// </summary>
    public Sprite sprite;
    
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
}
