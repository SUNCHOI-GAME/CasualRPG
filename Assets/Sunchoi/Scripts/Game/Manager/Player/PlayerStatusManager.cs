using System;
using System.Collections.Generic;
using DG.Tweening;
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
    private int maxHp;
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
        this.SetMaxExp();
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
    public void SetExp(int value) { this.currentExp = value; }
    public void SetMaxExp() { this.maxExp = this.maxExpList[this.currentLevel - 1]; }
    public void SetHp(int value) { this.currentHp = value; }
    public void SetMaxHp(int value) { this.maxHp = value; }
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
        this.hpText.text = this.currentHp.ToString() + " / " + this.maxHp.ToString();
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
        this.hpText.text = this.currentHp.ToString() + " / " + this.maxHp.ToString();
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



    #region [04. 戦闘 関連]

    #region [var]

    

    #endregion
    


    #region [func]
    /// <summary>
    /// Playerがダメージを負った場合
    /// </summary>
    /// <param name="damageValue"></param>
    public void PlayerDamaged(int damageValue)
    {
        // ダメージ - 防御力
        var calculatedDamage = damageValue - this.defence;
        if (calculatedDamage <= 0) calculatedDamage = 0;
        
        // HPのStatusおよびTEXT更新
        var newHp = this.currentHp - calculatedDamage;
        if (newHp <= 0) newHp = 0;
        this.SetHp(newHp);
        this.SetHpText();
    }

    /// <summary>
    /// EXP増加
    /// </summary>
    /// <param name="expValue"></param>
    public void IncreaseExp(int expValue)
    {
        // 既存のEXP + 新しいEXP
        var newExp = this.currentExp + expValue;
        // 最大EXP値とnewEXPの差分
        var difference = this.maxExp - newExp;

        // newEXPが最大EXP値と同じか、上回った場合：EXP増加後、レベルアップ
        if (difference <= 0)
        {
            // differenceを絶対値に変換
            var leftValue = Mathf.Abs(difference);
            // EXPが最大値になった表示に更新
            this.SetExp(maxExp);
            this.SetExpText();
            // レベルアップ演出開始
            this.LevelUpAnim(() =>
            {
                // Level +1 
                this.SetLevel(this.currentLevel + 1);
                // レベルアップ後、残ったEXP値を現在EXPに設定
                this.SetExp(leftValue);
                // LevelUpに合わせてMaxEXP更新
                this.SetMaxExp();
                
                // 各種TEXT表示を更新
                this.SetLevelText();
                this.SetExpText();
            });
        }
        // newEXPが最大EXP値より下回った場合：EXP増加のみ
        else
        {
            // EXP値を更新
            this.SetExp(newExp);
            // TEXT表示を更新
            this.SetExpText();
        }
    }
    #endregion
    
    #endregion

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            this.IncreaseDoorKeyCount();
        }
    }

    #region [05. MapEvent 関連]

    #region [var]
    
    

    #endregion



    #region [func]
    /// <summary>
    /// Heal Hp
    /// </summary>
    /// <param name="healValue"></param>
    public void HealHp(int healValue)
    {
        // 既存HP + 回復値
        var newHp = this.currentHp + healValue;
        if (newHp >= this.maxHp) newHp = this.maxHp;
        
        // StatusおよびTEXTを更新
        this.SetHp(newHp);
        this.SetHpText();
    }
    
    /// <summary>
    /// Increase MaxHp
    /// </summary>
    /// <param name="addValue"></param>
    public void IncreaseMaxHp(int addValue)
    {
        // 既存MaxHP + 増加値
        var newMaxHp = this.maxHp + addValue;
        
        // StatusおよびTEXTを更新
        this.SetMaxHp(newMaxHp);
        this.SetHpText();
    }

    /// <summary>
    /// Decrease MaxHp
    /// </summary>
    /// <param name="subValue"></param>
    public void DecreaseMaxHp(int subValue)
    {
        // 既存MaxHP - 減少値
        var newMaxHp = this.maxHp - subValue;
        if (newMaxHp <= 0) newMaxHp = 0;
        
        // StatusおよびTEXTを更新
        this.SetMaxHp(newMaxHp);
        this.SetHpText();
    }

    /// <summary>
    /// Increase Critical
    /// </summary>
    /// <param name="addValue"></param>
    public void IncreaseCritical(int addValue)
    {
        // 既存CriticalChance + 増加値
        var newCritical = this.critical + addValue;
        
        // StatusおよびTEXTを更新
        this.SetCritical(newCritical);
        this.SetCriticalText();
    }

    /// <summary>
    /// Decrease Critical
    /// </summary>
    /// <param name="subValue"></param>
    public void DecreaseCritical(int subValue)
    {
        // 既存CriticalChance - 減少値
        var newCritical = this.critical - subValue;
        if (newCritical <= 0) newCritical = 0;
        
        // StatusおよびTEXTを更新
        this.SetCritical(newCritical);
        this.SetCriticalText();
    }

    /// <summary>
    /// Increase Defence
    /// </summary>
    /// <param name="addValue"></param>
    public void IncreaseDefence(int addValue)
    {
        // 既存Defence + 増加値
        var newDefence = this.defence + addValue;
        
        // StatusおよびTEXTを更新
        this.SetDefence(newDefence);
        this.SetDefenceText();
    }

    /// <summary>
    /// Decrease Defence
    /// </summary>
    /// <param name="subValue"></param>
    public void DecreaseDefence(int subValue)
    {
        // 既存Defence - 減少値
        var newDefence = this.defence - subValue;
        if (newDefence <= 0) newDefence = 0;
        
        // StatusおよびTEXTを更新
        this.SetDefence(newDefence);
        this.SetDefenceText();
    }

    /// <summary>
    /// Increase Agility
    /// </summary>
    /// <param name="addValue"></param>
    public void IncreaseAgility(int addValue)
    {
        // 既存Agility + 増加値
        var newAgility = this.agility + addValue;
        
        // StatusおよびTEXTを更新
        this.SetAgility(newAgility);
        this.SetAgilityText();
    }

    /// <summary>
    /// Decrease Agility
    /// </summary>
    /// <param name="subValue"></param>
    public void DecreaseAgility(int subValue)
    {
        // 既存Agility - 減少値
        var newAgility = this.agility - subValue;
        if (newAgility <= 0) newAgility = 0;
        
        // StatusおよびTEXTを更新
        this.SetAgility(newAgility);
        this.SetAgilityText();
    }

    /// <summary>
    /// Increase InventoryCount
    /// </summary>
    /// <param name="addValue"></param>
    public void IncreaseInventoryCount(int addValue)
    {
        // 既存InventoryCount + 増加値
        var newInvCount = this.currentInventoryCount + addValue;
        if (newInvCount >= this.maxInventoryCount) newInvCount = this.maxInventoryCount;
        
        // StatusおよびTEXTを更新
        this.SetCurrentInventoryCount(newInvCount);
        this.SetInventoryCountText();
    }

    /// <summary>
    /// Decrease InventoryCount
    /// </summary>
    /// <param name="subValue"></param>
    public void DecreaseInventoryCount(int subValue)
    {
        // 既存InventoryCount - 減少値
        var newInvCount = this.currentInventoryCount - subValue;
        if (newInvCount <= 0) newInvCount = 0;
        
        // StatusおよびTEXTを更新
        this.SetCurrentInventoryCount(newInvCount);
        this.SetInventoryCountText();
    }

    /// <summary>
    /// Increase MaxInventoryCount
    /// </summary>
    /// <param name="addValue"></param>
    public void IncreaseMaxInventoryCount(int addValue)
    {
        // 既存MaxInventoryCount + 増加値
        var newMaxInvCount = this.maxInventoryCount + addValue;
        
        // StatusおよびTEXTを更新
        this.SetMaxInventoryCount(newMaxInvCount);
        this.SetInventoryCountText();
    }

    /// <summary>
    /// Increase DoorKeyCount
    /// </summary>
    public void IncreaseDoorKeyCount()
    {
        // 既存DoorKeyCount +1
        int newDoorKeyCount = this.currentDoorKeyCount + 1;
        if (newDoorKeyCount >= this.maxDoorKeyCount) newDoorKeyCount = this.maxDoorKeyCount;
        
        // StatusおよびTEXTを更新
        this.SetCurrentDoorKeyCount(newDoorKeyCount);
        this.SetDoorKeyCountText();
    }

    #endregion

    #endregion
    
    
    
    #region [06. 演出 関連]

    #region [var]
    
    

    #endregion



    #region [func]
    /// <summary>
    /// LevelUp演出再生 
    /// </summary>
    /// <param name="onFinished"></param>
    private void LevelUpAnim(Action onFinished)
    {
        Debug.LogFormat("Level Up Anim Playing", DColor.yellow);
        
        DOVirtual.DelayedCall(3f, () =>
        {

            onFinished?.Invoke();
        });
        
    }
    #endregion

    #endregion
}
