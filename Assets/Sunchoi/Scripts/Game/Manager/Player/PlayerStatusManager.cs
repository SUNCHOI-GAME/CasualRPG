using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusManager : MonoBehaviour
{
    #region [01. Instance]

    #region [var]
    /// <summary>
    /// インスタンス
    /// </summary>
    public static PlayerStatusManager Instance { get; private set; }
    #endregion

    #region [func]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Start()
    {
        // インスタンス
        Instance = this;
        // 破棄不可
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion
    
    #endregion
    
    
    
    #region [02. Set Status]

    #region [var]
    /// <summary>
    /// Level
    /// </summary>
    [SerializeField]
    private int currentLevel;
    [SerializeField]
    private int maxLevel = 10;
    /// <summary>
    /// EXP
    /// </summary>
    [SerializeField]
    private int currentExp;
    [SerializeField]
    private int maxExp;
    private List<int> maxExpList = new List<int>(){50, 100, 200, 400, 750, 1500, 2300, 3200, 4400, 10000};
    /// <summary>
    /// HP
    /// </summary>
    [SerializeField]
    private int currentHp;
    [SerializeField]
    private int maxHP;
    /// <summary>
    /// Attack Damage
    /// </summary>
    [SerializeField]
    private int attack;
    /// <summary>
    /// Critical Chance
    /// </summary>
    [SerializeField]
    private int critical;
    /// <summary>
    /// Defence
    /// </summary>
    [SerializeField]
    private int defence;
    /// <summary>
    /// Agility
    /// </summary>
    [SerializeField]
    private int agility;
    /// <summary>
    /// Inventory Count
    /// </summary>
    [SerializeField]
    private int currentInventoryCount;
    [SerializeField]
    private int maxInventoryCount;
    /// <summary>
    /// DoorKey Count
    /// </summary>
    [SerializeField]
    private int currentDoorKeyCount;
    [SerializeField]
    private int maxDoorKeyCount = 1;
    #endregion

    #region [func]
    
    #region [00. Set Status]

    /// <summary>
    /// PlayerStatusをセット
    /// </summary>
    public void SetPlayerStatus()
    {
        // PlayerStatusを初期値に更新
        // TODO:: 
        this.InitPlayerStatus();
    }
    
    /// <summary>
    /// PlayerStatusを初期値に更新
    /// </summary>
    private void InitPlayerStatus()
    {
        // 各種Status更新
        this.SetLevel(1);
        this.SetExp(0);
        this.SetHp(100);
        this.SetMaxHp(100);
        this.SetAttack(10);
        this.SetCritical(10);
        this.SetDefence(5);
        this.SetAgility(20);
        this.SetCurrentInventoryCount(0);
        this.SetMaxInventoryCount(10);
        this.SetCurrentDoorKeyCount(0);
        
        // 各種StatusのTEXTを一斉更新
        this.SetAllStatusTexts();
    }

    /// <summary>
    /// 各種Statusを更新
    /// </summary>
    public void SetLevel(int value) { this.currentLevel = value; }
    public void SetExp(int value) { this.currentExp = value; this.maxExp = this.maxExpList[this.currentLevel - 1]; }
    public void SetHp(int value) { this.currentHp = value; }
    public void SetMaxHp(int value) { this.maxHP = value; }
    public void SetAttack(int value) { this.attack = value; }
    public void SetCritical(int value) { this.critical = value; }
    public void SetDefence(int value) { this.defence = value; }
    public void SetAgility(int value) { this.agility = value; }
    public void SetCurrentInventoryCount(int value) { this.currentInventoryCount = value; }
    public void SetMaxInventoryCount(int value) { this.maxInventoryCount = value; }
    public void SetCurrentDoorKeyCount(int value) { this.currentDoorKeyCount = value; }
    #endregion

    #endregion

    #endregion

    
    
    #region [03. 各種StatusのText更新 関連]

    #region [var]
    [Header(" --- Level TEXT")]
    /// <summary>
    /// Level TEXT
    /// </summary>
    [SerializeField]
    private Text currentLevelText;
    
    [Header(" --- EXP TEXT")]
    /// <summary>
    /// EXP TEXT
    /// </summary>
    [SerializeField]
    private Text expText;
    
    [Header(" --- HP TEXT")]
    /// <summary>
    /// HP TEXT
    /// </summary>
    [SerializeField]
    private Text hpText;
    
    [Header(" --- Attack TEXT")]
    /// <summary>
    /// Attack TEXT
    /// </summary>
    [SerializeField]
    private Text attackText;
    
    [Header(" --- Critical TEXT")]
    /// <summary>
    /// Critical TEXT
    /// </summary>
    [SerializeField]
    private Text critiaclText;
    
    [Header(" --- Defence TEXT")]
    /// <summary>
    /// Defence TEXT
    /// </summary>
    [SerializeField]
    private Text defenceText;
    
    [Header(" --- Agility TEXT")]
    /// <summary>
    /// Agility TEXT
    /// </summary>
    [SerializeField]
    private Text agilityText;
    
    [Header(" --- InventoryCount TEXT")]
    /// <summary>
    /// InventoryCount TEXT
    /// </summary>
    [SerializeField]
    private Text inventoryCountText;
    
    [Header(" --- DoorKeyCount TEXT")]
    /// <summary>
    /// DoorKeyCount TEXT
    /// </summary>
    [SerializeField]
    private Text doorKeyCountText;
    #endregion



    #region [func]
    /// <summary>
    /// 各種StatusのTextを一斉更新
    /// </summary>
    private void SetAllStatusTexts()
    {
        this.currentLevelText.text = this.currentLevel >= this.maxLevel ? "MAX" : this.currentLevel.ToString();
        this.expText.text = this.currentExp.ToString() + " / " + this.maxExp.ToString();
        this.hpText.text = this.currentHp.ToString() + " / " + this.maxHP.ToString();
        this.critiaclText.text = this.critical.ToString()+ " % ";
        this.defenceText.text = this.defence.ToString();
        this.agilityText.text = this.agility.ToString()+ " % ";
        this.inventoryCountText.text = this.currentInventoryCount.ToString() + " / " + this.maxInventoryCount.ToString();
        this.doorKeyCountText.text = this.currentDoorKeyCount.ToString() + " / " + this.maxDoorKeyCount.ToString();
    }
    
    /// <summary>
    /// 各種StatusのTextを更新
    /// </summary>
    private void SetLevelText()
    {
        this.currentLevelText.text = this.currentLevel >= this.maxLevel ? "MAX" : this.currentLevel.ToString();
    }
    private void SetExpText()
    {
        this.expText.text = this.currentExp.ToString() + " / " + this.maxExp.ToString();
    }
    private void SetHpText()
    {
        this.hpText.text = this.currentHp.ToString() + " / " + this.maxHP.ToString();
    }
    private void SetAttackText()
    {
        this.attackText.text = this.attack.ToString();
    }
    private void SetCriticalText()
    {
        this.critiaclText.text = this.critical.ToString()+ " % ";
    }
    private void SetDefenceText()
    {
        this.defenceText.text = this.defence.ToString();
    }
    private void SetAgilityText()
    {
        this.agilityText.text = this.agility.ToString()+ " % ";
    }
    private void SetInventoryCountText()
    {
        this.inventoryCountText.text = this.currentInventoryCount.ToString() + " / " + this.maxInventoryCount.ToString();
    }
    private void SetDoorKeyCountText()
    {
        this.doorKeyCountText.text = this.currentDoorKeyCount.ToString() + " / " + this.maxDoorKeyCount.ToString();
    }
    #endregion

    #endregion
}
