using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    #region [00. コンストラクタ]

    #region [var]
    /// <summary>
    /// インスタンス
    /// </summary>
    public static BattleManager Instance { get; private set; }
    
    [Header(" --- Reference")]
    /// <summary>
    /// UIDialogController
    /// </summary>
    [SerializeField]
    private UIDialogController uIDialogController;
    
    /// <summary>
    /// スタート時のUnitの座標
    /// </summary>
    private Vector3 playerStartPos = new Vector3(-200f, -112.7f, 0f);
    private Vector3 enemyStartPos = new Vector3(200f, 80f, 0f);
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

    /// <summary>
    /// 初期化
    /// </summary>
    private void Init()
    {
        this.playerRootTransform.localPosition = playerStartPos;
        this.enemyRootTransform.localPosition = enemyStartPos;
        this.playerBattleStatusView.localPosition = new Vector3(-85f, 0f, 0f);
        this.enemyBattleStatusView.localPosition = new Vector3(85f, 0f, 0f);
        
        this.playerShortTermTermActionBoolStringList.Clear();
        this.playerLongTermActionBoolStringList.Clear();
        
        this.enemyShortTermTermActionBoolStringList.Clear();
        this.enemyLongTermActionBoolStringList.Clear();
    }
    #endregion

    #endregion
    
    
    
    #region [01. Battle開始]

    #region [var]
    [Header(" --- Enemy Data")]
    /// <summary>
    /// EnemyCollider
    /// </summary>
    [SerializeField]
    private Transform targetEnemyTransform;
    /// <summary>
    /// EnemyInfo
    /// </summary>
    [SerializeField]
    private Enemy enemyInfo;
    /// <summary>
    /// EnemyStatusController
    /// </summary>
    [SerializeField]
    private EnemyStatusController enemyStatusController;
    /// <summary>
    /// EnemyBattlePrefab
    /// </summary>
    [SerializeField]
    private GameObject enemyBattlePrefab;
    
    [Header(" --- Unit Animation Data")]
    /// <summary>
    /// 各UnitのTransform
    /// </summary>
    [SerializeField]
    private Transform playerRootTransform;
    [SerializeField]
    private Transform enemyRootTransform;
    /// <summary>
    /// 各UnitのAnimator
    /// </summary>
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private Animator enemyAnimator;

    /// <summary>
    /// UnitEntry時のアニメーションパターン
    /// </summary>
    [SerializeField]
    private Ease unitEntryEase;
    
    /// <summary>
    /// PlayerBattleStatusView
    /// </summary>
    [SerializeField]
    private Transform playerBattleStatusView;
    /// <summary>
    /// EnemyBattleStatusView
    /// </summary>
    [SerializeField]
    private Transform enemyBattleStatusView;
    
    
    
    /// <summary>
    /// 各種PlayerStatus
    /// </summary>
    [Header(" --- Player Status")]
    [SerializeField]
    private int playerLevel;
    [SerializeField]
    private int playerCurrentHp;
    [SerializeField]
    private int playerMaxHp;
    [SerializeField]
    public int playerAttack;
    [SerializeField]
    public int playerCritical;
    [SerializeField]
    public int playerDefence;
    [SerializeField]
    public int playerAgility;
    
    /// <summary>
    /// 各種EnemyStatusのTEXT
    /// </summary>
    [Header(" --- Enemy Status TEXT")]
    [SerializeField]
    private Text playerLevelText;
    [SerializeField]
    private Text playerHpText;

    /// <summary>
    /// 各種EnemyStatus
    /// </summary>
    [Header(" --- Enemy Status")]
    [SerializeField]
    private string enemyName;
    [SerializeField]
    private int enemyLevel;
    [SerializeField]
    private int enemyCurrentHp;
    [SerializeField]
    private int enemyMaxHp;
    [SerializeField]
    private int enemyAttack;
    [SerializeField]
    private int enemyCritical;

    [SerializeField]
    private int enemyDefence;
    [SerializeField]
    private int enemyAgility;
    
    /// <summary>
    /// 各種EnemyStatusのTEXT
    /// </summary>
    [Header(" --- Enemy Status TEXT")]
    [SerializeField]
    private Text enemyNameText;
    [SerializeField]
    private Text enemyLevelText;
    [SerializeField]
    private Text enemyHpText;
    
    [Header(" --- Enemy Status State Obj")]
    /// <summary>
    /// Enemy Status State Obj
    /// </summary>
    [SerializeField]
    private GameObject enemyAttackStateObj_Up;
    [SerializeField]
    private GameObject enemyAttackStateObj_Down;
    [SerializeField]
    private GameObject enemyCriticalStateObj_Up;
    [SerializeField]
    private GameObject enemyCriticalStateObj_Down;
    [SerializeField]
    private GameObject enemyDefenceStateObj_Up;
    [SerializeField]
    private GameObject enemyDefenceStateObj_Down;
    [SerializeField]
    private GameObject enemyAgilityStateObj_Up;
    [SerializeField]
    private GameObject enemyAgilityStateObj_Down;
    [Header(" --- Player Status State Obj")]
    /// <summary>
    /// Player Status State Obj
    /// </summary>
    [SerializeField]
    private GameObject playerAttackStateObj_Up;
    [SerializeField]
    private GameObject playerAttackStateObj_Down;
    [SerializeField]
    private GameObject playerCriticalStateObj_Up;
    [SerializeField]
    private GameObject playerCriticalStateObj_Down;
    [SerializeField]
    private GameObject playerDefenceStateObj_Up;
    [SerializeField]
    private GameObject playerDefenceStateObj_Down;
    [SerializeField]
    private GameObject playerAgilityStateObj_Up;
    [SerializeField]
    private GameObject playerAgilityStateObj_Down;
    
    /// <summary>
    /// StatusStateObj臨時保存用のGameObjectList
    /// </summary>
    private List<GameObject> statusStateObjList = new List<GameObject>();
    #endregion

    #region [func]

    #region [01. DataSet 関連]
    /// <summary>
    /// Enemyの各種データをセット
    /// </summary>
    /// <param name="enemyTransform"></param>
    /// <param name="enemyInfo"></param>
    public void SetEnemyInfo(Transform enemyTransform, Enemy enemyInfo)
    {
        // ターゲットとなるEnemyColliderのTransformを登録
        this.targetEnemyTransform = enemyTransform;
        // EnemyのScriptableObject上のデータを登録
        this.enemyInfo = enemyInfo;
        // EnemyStatusControllerを登録
        this.enemyStatusController = enemyTransform.parent.GetComponent<EnemyStatusController>();
    }
    
    /// <summary>
    /// UnitのStatusをセット
    /// </summary>
    private void SetUnitStatusData()
    {
        // EnemyのStatusおよびその表示TEXTを更新
        this.SetPlayerStatus(this.SetPlayerStatusText);
        
        // EnemyのStatusおよびその表示TEXTを更新
        this.SetEnemyStatus(this.SetEnemyStatusText);
    }
    
    /// <summary>
    /// PlayerのStatusをセット
    /// </summary>
    private void SetPlayerStatus(Action onFinished)
    {
        this.playerLevel = PlayerStatusManager.Instance.CurrentLevel;
        this.playerCurrentHp = PlayerStatusManager.Instance.CurrentHp;
        this.playerMaxHp = PlayerStatusManager.Instance.MaxHp;
        this.playerAttack = PlayerStatusManager.Instance.Attack;
        this.playerCritical = PlayerStatusManager.Instance.Critical;
        this.playerDefence = PlayerStatusManager.Instance.Defence;
        this.playerAgility = PlayerStatusManager.Instance.Agility;

        onFinished?.Invoke();
    }
    
    /// <summary>
    /// PlayerStatusのTEXTを更新
    /// </summary>
    private void SetPlayerStatusText()
    {
        this.playerLevelText.text = this.playerLevel.ToString();
        this.playerHpText.text = this.playerCurrentHp.ToString() + " / " + this.playerMaxHp.ToString();
    }

    /// <summary>
    /// EnemyのStatusをセット
    /// </summary>
    private void SetEnemyStatus(Action onFinished)
    {
        // セット
        this.enemyName = enemyStatusController.Name;
        this.enemyLevel = enemyStatusController.Level;
        this.enemyCurrentHp = enemyStatusController.CurrentHp;
        this.enemyMaxHp = enemyStatusController.MaxHp;
        this.enemyAttack = enemyStatusController.Attack;
        this.enemyCritical = enemyStatusController.Critical;
        this.enemyDefence = enemyStatusController.Defence;
        this.enemyAgility = enemyStatusController.Agility;

        // UnitのATK比較および結果を表示
        if (this.enemyAttack - this.playerAttack > 0) this.SetActiveStatusStateObj(this.enemyAttackStateObj_Up, this.playerAttackStateObj_Down);
        else this.SetActiveStatusStateObj(this.enemyAttackStateObj_Down, this.playerAttackStateObj_Up);
        // UnitのCRI比較および結果を表示
        if (this.enemyCritical - this.playerCritical > 0) this.SetActiveStatusStateObj(this.enemyCriticalStateObj_Up, this.playerCriticalStateObj_Down);
        else this.SetActiveStatusStateObj(this.enemyCriticalStateObj_Down, this.playerCriticalStateObj_Up);
        // UnitのDEF比較および結果を表示
        if (this.enemyDefence - this.playerDefence> 0) this.SetActiveStatusStateObj(this.enemyDefenceStateObj_Up, this.playerDefenceStateObj_Down);
        else this.SetActiveStatusStateObj(this.enemyDefenceStateObj_Down, this.playerDefenceStateObj_Up);
        // UnitのAGI比較および結果を表示
        if (this.enemyAgility - this.playerAgility> 0) this.SetActiveStatusStateObj(this.enemyAgilityStateObj_Up, this.playerAgilityStateObj_Down);
        else this.SetActiveStatusStateObj(this.enemyAgilityStateObj_Down, this.playerAgilityStateObj_Up);
        
        onFinished?.Invoke();
    }

    /// <summary>
    /// 該当するStatusStateObjを表示
    /// </summary>
    /// <param name="obj_1"></param>
    /// <param name="obj_2"></param>
    private void SetActiveStatusStateObj(GameObject obj_1, GameObject obj_2)
    {
        // Battle終了時に非表示に切り替えるため、該当GameObjectをリストに一次保存
        this.statusStateObjList.Add(obj_1);
        this.statusStateObjList.Add(obj_2);
        
        // 表示
        obj_1.SetActive(true);
        obj_2.SetActive(true);
    }
    
    /// <summary>
    /// 表示中のStatusStateObjを非表示に切り替え
    /// </summary>
    private void SetInactiveStatusStateObj()
    {
        // 非表示
        foreach (var statusStateObj in this.statusStateObjList)
        {
            statusStateObj.SetActive(false);
        }

        // リスト初期化
        this.statusStateObjList.Clear();
    }   

    /// <summary>
    /// EnemyStatusのTEXTを更新
    /// </summary>
    private void SetEnemyStatusText()
    {
        this.enemyNameText.text = this.enemyName;
        this.enemyLevelText.text = this.enemyLevel.ToString();
        this.enemyHpText.text = this.enemyCurrentHp.ToString() + " / " + this.enemyMaxHp.ToString();
    }
    #endregion



    #region [BattleStart時（MainBattle開始前）]
    /// <summary>
    /// BattleStart
    /// Player奇襲時：firstStrikeUnitNum = 0
    /// Enemy奇襲時：firstStrikeUnitNum = 1
    /// </summary>
    /// <param name="enemyTransform"></param>
    /// <param name="firstStrikeUnitNum"></param>
    public void StartBattleAnim(int firstStrikeUnitNum)
    {
        // 初期化
        this.Init();
        
        // EnemyのBattlePrefabを生成
        this.enemyBattlePrefab = Instantiate(this.enemyInfo.battlePrefab, this.enemyRootTransform);
        this.enemyAnimator = enemyBattlePrefab.GetComponent<Animator>();
        
        // UnitのStatusをBattleStatusViewにセット
        this.SetUnitStatusData();

        // ターン保有Unitの奇襲成功可否をランダムで選定
        int randomNum = UnityEngine.Random.Range(0, 2);
        
        // 選定結果によって分岐
        if (firstStrikeUnitNum == 0)
        {
            if (randomNum == 0)
            {
                // Unit登場アニメーションの再生：通常Battle時
                this.UnitEntryAnimOnNormalBattle(() =>
                {
                    // Battle開始直前のLog表示アニメーション
                    this.BattleStartLog(this.playerFirstStrikeFailedString_1, this.playerFirstStrikeFailedString_2, 1);
                });
            }
            else
            {
                // Unit登場アニメーションの再生：Player奇襲Battle時
                this.UnitEntryAnimOnPlayerFirstStrikeBattle(() =>
                {
                    // Battle開始直前のLog表示アニメーション
                    this.BattleStartLog(this.playerFirstStrikeSucceededString_1, this.playerFirstStrikeSucceededString_2, 2);
                });
            }
        }
        else
        {
            if (randomNum == 0)
            {
                // Unit登場アニメーションの再生：通常Battle時
                this.UnitEntryAnimOnNormalBattle(() =>
                {
                    // Battle開始直前のLog表示アニメーション
                    this.BattleStartLog(this.enemyFirstStrikeFailedString_1, this.enemyFirstStrikeFailedString_2, 3);
                });
            }
            else
            {
                // Unit登場アニメーションの再生：Enemy奇襲Battle時
                this.UnitEntryAnimOnEnemyFirstStrikeBattle(() =>
                {
                    // Battle開始直前のLog表示アニメーション
                    this.BattleStartLog(this.enemyFirstStrikeSucceededString_1, this.enemyFirstStrikeSucceededString_2, 4);
                });
            }
        }
    }

    /// <summary>
    /// Unit登場アニメーションの再生：通常Battle時
    /// </summary>
    private void UnitEntryAnimOnNormalBattle(Action onFinished)
    {
        this.playerRootTransform.DOLocalMove(new Vector3(50f, -52f, 0f), 0.5f)
            .From(new Vector3(-200f, -52f, 0f))
            .SetEase(this.unitEntryEase)
            .SetAutoKill(true)
            .SetUpdate(true);
        
        this.enemyRootTransform.DOLocalMove(new Vector3(-42.5f, 68f, 0f), 0.5f)
            .From(new Vector3(200f, 68f, 0f))
            .SetEase(this.unitEntryEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                // StatusViewの表示アニメーション
                this.BattleStatusViewAnim(()=>{ onFinished?.Invoke(); });
            });
    }
    
    /// <summary>
    /// Unit登場アニメーションの再生：Player奇襲Battle時
    /// </summary>
    private void UnitEntryAnimOnPlayerFirstStrikeBattle(Action onFinished)
    {
        this.playerRootTransform.DOLocalMove(new Vector3(50f, -52f, 0f), 0.5f)
            .From(new Vector3(-200f, -52f, 0f))
            .SetEase(this.unitEntryEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                this.enemyRootTransform.DOLocalMove(new Vector3(-42.5f, 68f, 0f), 0.5f)
                .From(new Vector3(200f, 68f, 0f))
                .SetEase(this.unitEntryEase)
                .SetAutoKill(true)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    // StatusViewの表示アニメーション
                    this.BattleStatusViewAnim(()=>{ onFinished?.Invoke(); });
                });
            });
    }
    
    /// <summary>
    /// Unit登場アニメーションの再生：Enemy奇襲Battle時
    /// </summary>
    private void UnitEntryAnimOnEnemyFirstStrikeBattle(Action onFinished)
    {
        this.enemyRootTransform.DOLocalMove(new Vector3(-42.5f, 68f, 0f), 0.5f)
            .From(new Vector3(200f, 68f, 0f))
            .SetEase(this.unitEntryEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                this.playerRootTransform.DOLocalMove(new Vector3(50f, -52f, 0f), 0.5f)
                    .From(new Vector3(-200f, -52f, 0f))
                    .SetEase(this.unitEntryEase)
                    .SetAutoKill(true)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        // StatusViewの表示アニメーション
                        this.BattleStatusViewAnim(()=>{ onFinished?.Invoke(); });
                    });
            });
    }

    /// <summary>
    /// StatusViewのアニメーション再生
    /// </summary>
    /// <param name="onFinished"></param>
    private void BattleStatusViewAnim(Action onFinished)
    {
        this.playerBattleStatusView.DOLocalMove(new Vector3(0f, 18f, 0f), 0.25f)
            .From(new Vector3(-85f, 18f, 0f))
            .SetEase(this.unitEntryEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                
            });
        
        this.enemyBattleStatusView.DOLocalMove(new Vector3(0f, 15f, 0f), 0.25f)
            .From(new Vector3(85f, 15f, 0f))
            .SetEase(this.unitEntryEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                onFinished?.Invoke();
            });
    }
    #endregion
    
    #endregion
    
    #endregion



    #region [02. Battle Log 関連]

    #region [var]
    [Header(" --- Log On Battle Start")]
    /// <summary>
    /// BattleStart時表示するLogのオブイェークトおよびそのText2種
    /// </summary>
    [SerializeField]
    private GameObject battleStartLogObj;
    [SerializeField]
    private Text battleStartLogText_1;
    [SerializeField]
    private Text battleStartLogText_2;
    /// <summary>
    /// Player奇襲成功時Logの表示文
    /// </summary>
    [SerializeField,TextArea(2,10)]
    private String playerFirstStrikeSucceededString_1;
    [SerializeField,TextArea(2,10)]
    private String playerFirstStrikeSucceededString_2;
    /// <summary>
    /// Player奇襲失敗時Logの表示文
    /// </summary>
    [SerializeField,TextArea(2,10)]
    private String playerFirstStrikeFailedString_1;
    [SerializeField,TextArea(2,10)]
    private String playerFirstStrikeFailedString_2;
    /// <summary>
    /// Enemy奇襲成功時Logの表示文
    /// </summary>
    [SerializeField,TextArea(2,10)]
    private String enemyFirstStrikeSucceededString_1;
    [SerializeField,TextArea(2,10)]
    private String enemyFirstStrikeSucceededString_2;
    /// <summary>
    /// Enemy奇襲失敗時Logの表示文
    /// </summary>
    [SerializeField,TextArea(2,10)]
    private String enemyFirstStrikeFailedString_1;
    [SerializeField,TextArea(2,10)]
    private String enemyFirstStrikeFailedString_2;
    
    [Header(" --- Log On Main Battle")]
    /// <summary>
    /// MainBattleのLogおよびLogText
    /// </summary>
    [SerializeField]
    private GameObject mainBattleLogObj;
    [SerializeField]
    private Text mainBattleLogText;
    #endregion

    
    
    #region [func]
    /// <summary>
    /// BattleStart後半ののLog表示アニメーション
    /// </summary>
    /// <param name="textGroupObj"></param>
    /// <param name="onFinished"></param>
    private void BattleStartLog(string logString_1, string logString_2, int firstStrikeType)
    {
        // LogTextの中身を指定
        this.battleStartLogText_1.text = logString_1;
        this.battleStartLogText_2.text = logString_2;
        
        DOVirtual.DelayedCall(.2f, () =>
        {
            // TextGroupObjを表示Stateに変更
            this.battleStartLogObj.SetActive(true);
            
            // Anim⓵
            this.battleStartLogObj.transform.DOLocalMove(new Vector3(0f, 0f, 0f), .5f)
                .From(new Vector3(350f, 0f, 0f))
                .SetEase(Ease.Linear)
                .SetAutoKill(true)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        // Anim⓶
                        this.battleStartLogObj.transform.DOLocalMove(new Vector3(-350f, 0f, 0f), .5f)
                            .From(new Vector3(0f, 0f, 0f))
                            .SetEase(Ease.Linear)
                            .SetAutoKill(true)
                            .SetUpdate(true)
                            .OnComplete(() =>
                            {
                                DOVirtual.DelayedCall(1.25f, () =>
                                {
                                    // Anim⓷
                                    this.battleStartLogObj.transform.DOLocalMove(new Vector3(-700f, 0f, 0f), .5f)
                                        .From(new Vector3(-350f, 0f, 0f))
                                        .SetEase(Ease.Linear)
                                        .SetAutoKill(true)
                                        .SetUpdate(true)
                                        .OnComplete(() =>
                                        {
                                            // TextGroupObjを非表示Stateに変更
                                            this.battleStartLogObj.SetActive(false);
                                            
                                            // LogText初期化
                                            this.battleStartLogText_1.text = null;
                                            this.battleStartLogText_2.text = null;
                                
                                            // 座標初期化
                                            this.battleStartLogObj.transform.localPosition = new Vector3(350f, 0f, 0f);
                                                        
                                            // Battle開始
                                            this.MainBattle(firstStrikeType);
                                        });
                                });
                            });
                    });
                });
        });
        
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logString"></param>
    /// <param name="onFinished"></param>
    public void UnitActionLog(string logString, Action onFinished)
    {
        // LogTextの中身を指定
        this.mainBattleLogText.text = logString;

        DOVirtual.DelayedCall(.2f, () =>
        {
            // TextGroupObjを表示Stateに変更
            this.mainBattleLogObj.SetActive(true);

            // Anim⓵
            this.mainBattleLogObj.GetComponent<RectTransform>().DOSizeDelta(new Vector2(180f, 40f), 0.3f)
                .From(new Vector2(180f, 0f))
                .SetEase(Ease.Linear)
                .SetAutoKill(true)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        // Anim⓶
                        this.mainBattleLogObj.GetComponent<RectTransform>().DOSizeDelta(new Vector2(180f, 0f), 0.2f)
                            .From(new Vector2(180f, 40f))
                            .SetEase(Ease.Linear)
                            .SetAutoKill(true)
                            .SetUpdate(true)
                            .OnComplete(() =>
                            {
                                DOVirtual.DelayedCall(0.5f, () =>
                                {
                                    // TextGroupObjを非表示Stateに変更
                                    this.mainBattleLogObj.SetActive(false);

                                    // LogText初期化
                                    this.mainBattleLogText.text = null;
                                
                                    onFinished?.Invoke();
                                });
                            });
                    });
                });
        });
    }

    #endregion

    #endregion
    
    
    
    #region [03. MainBattle]

    #region [var]
    [Header(" --- Unit Action Probability Offset")]
    /// <summary>
    /// Unit行動選定時の確率Offset
    /// </summary>
    [SerializeField]
    private float actionOffset_Bottom = 0f;
    [SerializeField]
    private float actionOffset_Top = 0f;
    
    [Header(" --- Damage Unit give to Unit")]
    /// <summary>
    /// 該当Unitが相手Unitに与えるダメージ量
    /// </summary>
    [SerializeField]
    private int damage = 0;

    /// <summary>
    /// Playerの短期、長期アニメーションのBool名のリスト
    /// </summary>
    private List<string> playerShortTermTermActionBoolStringList = new List<string>();
    private List<string> playerLongTermActionBoolStringList = new List<string>();
    /// <summary>
    /// Enemyの短期、長期アニメーションのBool名のリスト
    /// </summary>
    private List<string> enemyShortTermTermActionBoolStringList = new List<string>();
    private List<string> enemyLongTermActionBoolStringList = new List<string>();
    
    [Header(" --- Damage Log Animation")]
    /// <summary>
    /// 各UnitのDamageLogおよびText
    /// </summary>
    [SerializeField]
    private Animator playerDamageLogAnimator;
    public Animator PlayerDamageLogAnimator { get => this.playerDamageLogAnimator; }
    [SerializeField]
    private Text playerDamageLogText;
    public Text PlayerDamageLogText { get => this.playerDamageLogText; }
    [SerializeField]
    private Animator enemyDamageLogAnimator;
    public Animator EnemyDamageLogAnimator { get => this.enemyDamageLogAnimator; }
    [SerializeField]
    private Text enemyDamageLogText;
    public Text EnemyDamageLogText { get => this.enemyDamageLogText; }
    #endregion

    
    
    #region [func]
    /// <summary>
    /// Battle開始
    /// </summary>
    private void MainBattle(int firstStrikeType)
    {
        this.PlayerActionTurn();
    }

    #region [01. Playerの行動パターン]
    /// <summary>
    /// Player Action Turn
    /// </summary>
    /// <returns></returns>
    private void PlayerActionTurn()
    {
        Debug.LogFormat("Player Action Turn", DColor.cyan);
        
        // EnemyHPをチェック⓵
        // （EnemyのHPが０だった場合、Battle終了）
        // （EnemyのHPが１以上の場合は、PlayerActionを実行）
        bool isEnemyDeadBeforePlayerAction = (this.enemyCurrentHp == 0);
        if (isEnemyDeadBeforePlayerAction)
        {
            Debug.LogFormat("Battle End", DColor.cyan);
            // TODO :: EndLog表示　→　EndBattle
        }
        
        // Playerの行動
        this.PlayerAction(() =>
        {
            // ShortTermAnimation再生Stateを解除
            this.SetAllShortTermActionBoolOffState(this.playerAnimator);
            this.SetAllShortTermActionBoolOffState(this.enemyAnimator);
            // LongTermAnimation再生Stateを解除
            this.SetAllLongTermActionBoolOffState(this.enemyAnimator);
            
            // EnemyHPをチェック⓶
            // （EnemyのHPが０だった場合、Battle終了）
            // （EnemyのHPが１以上の場合は、EnemyTurnに移行）
            bool isEnemyDeadAfterPlayerAction = (this.enemyCurrentHp == 0);
            if (isEnemyDeadAfterPlayerAction)
            {
                Debug.LogFormat("Battle End", DColor.cyan);
                // TODO :: EndLog表示　→　EndBattle
                // UnitActionLog表示
                this.UnitActionLog(" Playerの勝利！！！", () =>
                {
                    // 終了Logを表示
                    this.ShowEndLog();
                });
            }
            else
            {
                Debug.LogFormat("Next Turn", DColor.cyan);
                    
                // Enemy Turn
                this.EnemyActionTurn();
            }
        });
    }

    /// <summary>
    /// Playerの行動選定
    /// </summary>
    private void PlayerAction(Action onFinished)
    {
        Debug.LogFormat("Player Action", DColor.yellow);
        
        // UnitActionLog表示
        this.UnitActionLog("Playerの\nターン", () =>
        {
            // Action Offset（Player Agility 依存）
            this.actionOffset_Bottom = (100f - this.playerAgility) / 10f;
            this.actionOffset_Top = this.playerAgility * 2f / 10f;
        
            // 乱数選定
            float actionRate = UnityEngine.Random.value * 100f;

            // 確率で行動を分岐
            if ( actionRate < 10f + this.actionOffset_Bottom ) 
                // 混乱
                this.PlayerPanicked(()=>{ onFinished?.Invoke(); });
            else if ( 10f + this.actionOffset_Bottom <= actionRate && actionRate < 50f - this.actionOffset_Top ) 
                // 次の敵の攻撃を防御
                this.PlayerDefence(()=>{ onFinished?.Invoke(); });
            else if ( 50f - this.actionOffset_Top <= actionRate ) 
                // 敵に攻撃（通常攻撃、もしくは、クリティカルヒット）
                this.PlayerAttack(()=>{ onFinished?.Invoke(); });
        });
    }

    /// <summary>
    /// Playerの各種行動
    /// </summary>
    private void PlayerAttack(Action onFinished)
    {
        // 乱数選定
        float attackRate = UnityEngine.Random.value * 100f;
        
        // 確率で攻撃の種類を分岐
        if ( attackRate < this.playerCritical )
        {
            Debug.LogFormat("   Player Give Enemy CRITICAL HIT !!!!!!!!", DColor.yellow);
            
            // UnitActionLogを表示
            this.UnitActionLog("Playerの\n クリティカル攻撃！", () => { });
            
            // Animation再生
            this.SetShortTermActionBoolState(this.playerAnimator, "CriticalAttack", true);
            
            // 相手UnitのDamaged Animation再生
            this.playerAnimator.GetComponent<AnimationCallBack>().EventOnPlayingAnimation(() =>
            {
                // Animation再生
                this.SetShortTermActionBoolState(this.enemyAnimator, "Damaged", true);
                
                // Critical Hit (通常の1.7倍）
                this.damage = Mathf.CeilToInt((float)this.playerAttack * 1.7f);
            
                // Enemyがダメージを受けた際の計算処理
                enemyStatusController.EnemyDamaged(this.damage, ()=>
                {  
                    // EnemyのStatusおよびその表示TEXTを更新
                    this.SetEnemyStatus(this.SetEnemyStatusText);
                });
            });

            this.playerAnimator.GetComponent<AnimationCallBack>().EndAnimation(()=>
            {
                DOVirtual.DelayedCall(0.5f, () => { onFinished?.Invoke(); });
            });
        }
        else if ( this.playerCritical <= attackRate ) 
        {
            Debug.LogFormat("   Player Give Enemy Normal Attack !!!! ", DColor.yellow);
            
            // UnitActionLogを表示
            this.UnitActionLog("Playerの\n通常攻撃", () => { });
            
            // Animation再生
            this.SetShortTermActionBoolState(this.playerAnimator, "NormalAttack", true);
            
            // 相手UnitのDamaged Animation再生
            this.playerAnimator.GetComponent<AnimationCallBack>().EventOnPlayingAnimation(() =>
            {
                // Animation再生
                this.SetShortTermActionBoolState(this.enemyAnimator, "Damaged", true);
                
                // Normal Attack
                this.damage = this.playerAttack;
            
                // Enemyがダメージを受けた際の計算処理
                enemyStatusController.EnemyDamaged(this.damage, ()=>
                {  
                    // EnemyのStatusおよびその表示TEXTを更新
                    this.SetEnemyStatus(this.SetEnemyStatusText);
                });
            });
            
            this.playerAnimator.GetComponent<AnimationCallBack>().EndAnimation(()=>
            {
                DOVirtual.DelayedCall(0.5f, () => { onFinished?.Invoke(); });
            });
        }
    }
    private void PlayerDefence(Action onFinished)
    {
        Debug.LogFormat("   Player DEFENCE!!!!!!!", DColor.yellow);
        
        // Animation再生
        this.SetLongTermActionBoolState(this.playerAnimator, "Defence", true);
        
        // UnitActionLogを表示
        this.UnitActionLog("Playerは\n 防御スタンスを取った！", () =>
        {
            // TODO :: PlayerのDefence処理（次のEnemyターンのみ、Enemyの攻撃をDefence2倍で受けられる）
            
                    
            onFinished?.Invoke();
        });
    }
    private void PlayerPanicked(Action onFinished)
    {
        Debug.LogFormat("   Player Do Nothing", DColor.yellow);    
        
        // Animation再生
        this.SetLongTermActionBoolState(this.playerAnimator, "Panicked", true);
        
        // UnitActionLogを表示
        this.UnitActionLog("Playerは\nパニックになった", () =>
        {
            // TODO :: PlayerのDoNothing処理
            
                    
            onFinished?.Invoke();
        });
    }
    #endregion



    #region [02. Enemyの行動パターン]
    /// <summary>
    /// Enemy Action Turn
    /// </summary>
    /// <returns></returns>
    private void EnemyActionTurn()
    {
        Debug.LogFormat("Enemy Action Turn", DColor.cyan);
        
        // PlayerHPをチェック⓵
        // （PlayerのHPが０だった場合、GAME OVER）
        // （PlayerのHPが１以上の場合は、EnemyActionを実行）
        bool isPlayerDeadBeforeEnemyAction = (this.playerCurrentHp == 0);
        if (isPlayerDeadBeforeEnemyAction)
        {
            Debug.LogFormat("GAME OVER", DColor.cyan);
            // TODO :: EndLog表示　→　GAME OVER
        }
        
        // Enemyの行動
        this.EnemyAction(() =>
        {
            // ShortTermAnimation再生Stateを解除
            this.SetAllShortTermActionBoolOffState(this.enemyAnimator);
            this.SetAllShortTermActionBoolOffState(this.playerAnimator);
            // LongTermAnimation再生Stateを解除
            this.SetAllLongTermActionBoolOffState(this.playerAnimator);
            
            // PlayerHPをチェック⓵
            // （PlayerのHPが０だった場合、GAME OVER）
            // （PlayerのHPが１以上の場合は、PlayerTurnに移行）
            bool isPlayerDeadAfterEnemyAction = (this.playerCurrentHp == 0);
            if (isPlayerDeadAfterEnemyAction)
            {
                Debug.LogFormat("GAME OVER", DColor.cyan);
                // TODO :: EndLog表示　→　GAME OVER
                
                // UnitActionLog表示
                this.UnitActionLog("GAME OVER", () =>
                {
                    // 終了Logを表示
                    this.ShowEndLog();
                });
            }
            else
            {
                // Player Turn
                this.PlayerActionTurn();
            }
        });
    }

    /// <summary>
    /// Enemyの行動選定
    /// </summary>
    private void EnemyAction(Action onFinished)
    {
        Debug.LogFormat("Enemy Action", DColor.yellow);

        // UnitActionLogを表示
        this.UnitActionLog($"{this.enemyName}の\nターン", () =>
        {
            // Action Offset（Enemy Agility 依存）
            this.actionOffset_Bottom = (100f - this.enemyAgility) / 10f;
            this.actionOffset_Top = this.enemyAgility * 2f / 10f;
        
            // 乱数選定
            float actionRate = UnityEngine.Random.value * 100f;

            // 確率で行動を分岐
            if ( actionRate < 10f + this.actionOffset_Bottom ) 
                // 混乱
                this.EnemyPanicked(()=>{ onFinished?.Invoke(); });
            else if ( 10f + this.actionOffset_Bottom <= actionRate && actionRate < 50f - this.actionOffset_Top ) 
                // 次の敵の攻撃を防御
                this.EnemyDefence(()=>{ onFinished?.Invoke(); });
            else if ( 50f - this.actionOffset_Top <= actionRate ) 
                // 敵に攻撃（通常攻撃、もしくは、クリティカルヒット）
                this.EnemyAttack(()=>{ onFinished?.Invoke(); });
        });
    }
    
    /// <summary>
    /// Playerの各種行動
    /// </summary>
    private void EnemyAttack(Action onFinished)
    {
        // 乱数選定
        float attackRate = UnityEngine.Random.value * 100f;
        
        // 確率で攻撃の種類を分岐
        if ( attackRate < this.enemyCritical )
        {
            Debug.LogFormat("   Enemy Gives Player CRITICAL HIT !!!!!!!!", DColor.yellow);
        
            // UnitActionLogを表示
            this.UnitActionLog($"{this.enemyName}の\n クリティカル攻撃！", () => { });

            // Animation再生
            this.SetShortTermActionBoolState(this.enemyAnimator, "CriticalAttack", true);
            
            // 相手UnitのDamaged Animation再生
            this.enemyAnimator.GetComponent<AnimationCallBack>().EventOnPlayingAnimation(() =>
            {
                // Animation再生
                this.SetShortTermActionBoolState(this.playerAnimator, "Damaged", true);
                
                // Critical Hit (通常の1.7倍）
                this.damage = Mathf.CeilToInt((float)this.enemyAttack * 1.7f);
                
                // Playerがダメージを受けた際の計算処理
                PlayerStatusManager.Instance.PlayerDamaged(this.damage, () =>
                {
                    // EnemyのStatusおよびその表示TEXTを更新
                    this.SetPlayerStatus(this.SetPlayerStatusText);
                });
            });
            
            this.enemyAnimator.GetComponent<AnimationCallBack>().EndAnimation(()=>
            {
                DOVirtual.DelayedCall(0.5f, () => { onFinished?.Invoke(); });
            });
        }
        else if ( this.enemyCritical <= attackRate ) 
        {
            Debug.LogFormat("   Enemy Gives Player Normal Attack !!!! ", DColor.yellow);
        
            // UnitActionLogを表示
            this.UnitActionLog($"{this.enemyName}の\n通常攻撃", () => { });

            // Animation再生
            this.SetShortTermActionBoolState(this.enemyAnimator, "NormalAttack", true);
            
            // 相手UnitのDamaged Animation再生
            this.enemyAnimator.GetComponent<AnimationCallBack>().EventOnPlayingAnimation(() =>
            {
                // Animation再生
                this.SetShortTermActionBoolState(this.playerAnimator, "Damaged", true);
                
                // Normal Attack
                this.damage = this.enemyAttack;
                
                // Playerがダメージを受けた際の計算処理
                PlayerStatusManager.Instance.PlayerDamaged(this.damage, () =>
                {
                    // EnemyのStatusおよびその表示TEXTを更新
                    this.SetPlayerStatus(this.SetPlayerStatusText);
                });
            });
            
            this.enemyAnimator.GetComponent<AnimationCallBack>().EndAnimation(() =>
            {
                DOVirtual.DelayedCall(0.5f, () => { onFinished?.Invoke(); });
            });
        }
    }
    private void EnemyDefence(Action onFinished)
    {
        Debug.LogFormat("   Enemy DEFENCE!!!!!!!", DColor.yellow);
        
        // Animation再生
        this.SetLongTermActionBoolState(this.enemyAnimator, "Defence", true);
        
        // UnitActionLogを表示
        this.UnitActionLog($"{this.enemyName}は\n防御スタンスを取った", () =>
        {
            // TODO :: EnemyのDefence処理（次のPlayerターンのみ、Playerの攻撃をDefence2倍で受けられる）
            
            onFinished?.Invoke();
        });
    }
    private void EnemyPanicked(Action onFinished)
    {
        Debug.LogFormat("   Enemy Do Nothing", DColor.yellow);    
        
        // Animation再生
        this.SetLongTermActionBoolState(this.enemyAnimator, "Panicked", true);
        
        // UnitActionLogを表示
        this.UnitActionLog($"{this.enemyName}は何をすれば\nいいかが思いつかない", () =>
        {
            // TODO :: EnemyのDoNothing処理
            
            onFinished?.Invoke();
        });
    }
    
    #endregion

    
    
    
    #region [03. Animation制御]
    /// <summary>
    /// DamageLogのAnimation
    /// </summary>
    /// <param name="unitDamageLogAnimator"></param>
    /// <param name="unitDamageLogText"></param>
    /// <param name="damageValue"></param>
    public void PlayUnitDamageAnim(Animator unitDamageLogAnimator, Text unitDamageLogText, int damageValue)
    {
        var damageLogAnimator = unitDamageLogAnimator;
        var damageLogText = unitDamageLogText;
        damageLogText.text = "-" + damageValue.ToString();
        
        damageLogAnimator.SetTrigger("Play");
    }
    
    /// <summary>
    /// ShortTermアニメーションの再生State切り替え
    /// （ShortTermAnimation：通常攻撃、クリティカル攻撃）
    /// </summary>
    /// <param name="boolName"></param>
    /// <param name="state"></param>
    private void SetShortTermActionBoolState(Animator unitAnimator, string boolName, bool state)
    {
        // AnimationState指定
        unitAnimator.SetBool(boolName, state);
        
        // リストに追加
        if (state)
        {
            if(unitAnimator == this.playerAnimator)
                this.playerShortTermTermActionBoolStringList.Add(boolName);
            else if(unitAnimator == this.enemyAnimator)
                this.enemyShortTermTermActionBoolStringList.Add(boolName);
        }
    }
    
    /// <summary>
    /// ShortTermアニメーションの再生Stateを解除
    /// </summary>
    private void SetAllShortTermActionBoolOffState(Animator unitAnimator)
    {
        // リスト指定
        List<string> list = null; 
        if (unitAnimator == this.playerAnimator)
            list = this.playerShortTermTermActionBoolStringList;
        else if(unitAnimator == this.enemyAnimator)
            list = this.enemyShortTermTermActionBoolStringList;
        
        // アニメーションの再生Stateを解除
        if (list != null)
            foreach (var actionBool in list)
            {
                this.SetShortTermActionBoolState(unitAnimator, actionBool, false);
            }

        // リスト初期化
        if(unitAnimator == this.playerAnimator)
            this.playerShortTermTermActionBoolStringList.Clear();
        else if(unitAnimator == this.enemyAnimator)
            this.enemyShortTermTermActionBoolStringList.Clear();
    }

    /// <summary>
    /// LongTermアニメーションの再生State切り替え
    /// （LongTermAnimation：防御、パニック）
    /// </summary>
    /// <param name="boolName"></param>
    /// <param name="state"></param>
    private void SetLongTermActionBoolState(Animator unitAnimator, string boolName, bool state)
    {
        // AnimationState指定
        unitAnimator.SetBool(boolName, state);
        
        // リストに追加
        if (state)
        {
            if(unitAnimator == this.playerAnimator)
                this.playerLongTermActionBoolStringList.Add(boolName);
            else if(unitAnimator == this.enemyAnimator)
                this.enemyLongTermActionBoolStringList.Add(boolName);
        }
    }

    /// <summary>
    /// LongTermアニメーションの再生Stateを解除
    /// </summary>
    private void SetAllLongTermActionBoolOffState(Animator unitAnimator)
    {
        // リスト指定
        List<string> list = null; 
        if (unitAnimator == this.playerAnimator)
            list = this.playerLongTermActionBoolStringList;
        else if(unitAnimator == this.enemyAnimator)
            list = this.enemyLongTermActionBoolStringList;
        
        // アニメーションの再生Stateを解除
        if (list != null)
            foreach (var actionBool in list)
            {
                this.SetLongTermActionBoolState(unitAnimator, actionBool, false);
            }

        // リスト初期化
        if(unitAnimator == this.playerAnimator)
            this.playerLongTermActionBoolStringList.Clear();
        else if(unitAnimator == this.enemyAnimator)
            this.enemyLongTermActionBoolStringList.Clear();
    }
    #endregion
    
    #endregion

    #endregion
    
    
    
    #region [05. Battle終了]

    #region [var]
    
    #endregion

    
    
    #region [func]

    /// <summary>
    /// 
    /// </summary>
    private void ShowEndLog()
    {
        // Battle結果表示
        this.uIDialogController.ShowBattleEndLog(() =>
        {
            // Closeボタン表示
            this.uIDialogController.ShowBattleDialogCloseButton();
        });
    }
    
    /// <summary>
    /// Battle終了
    /// </summary>
    public void EndBattle()
    {
        // TODO:: 臨時処理。Battle相手のEnemyを破棄。
        EnemyManager.Instance.
            ExcludeEnemyTemporarily(this.targetEnemyTransform.parent.GetComponent<EnemyMovementController>());
        
        // EnemyのBattlePrefabを破棄
        Destroy(this.enemyBattlePrefab);

        // 表示中のStatusStateObjを非表示に切り替え
        this.SetInactiveStatusStateObj();
        
        // BattleDialog非表示
        this.uIDialogController.CloseBattleDialog(this.uIDialogController.Dialog_Battle.transform, () =>
        {
            // Target初期化
            this.targetEnemyTransform = null;
            
            // ゲーム再生を再開
            Time.timeScale = 1f;

            // PlayerTurn中の場合
            if (UnitTurnManager.Instance.IsPlayerAttackPhaseOn)
            {
                // ターン進行を再開
                UnitTurnManager.Instance.SetPlayerAttackPhaseTrigger(false);
            }
            // EnemyTurn中の場合
            else if(UnitTurnManager.Instance.IsEnemyAttackPhaseOn)
            {
                // Enemyの移動を再開
                EnemyManager.Instance.StartEnemyMoveEachCoroutineAgain();
            }
        });
        
        // 初期化
        this.Init();
    }
    #endregion

    #endregion
}
