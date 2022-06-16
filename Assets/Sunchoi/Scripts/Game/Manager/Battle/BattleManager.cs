using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine.UI;

public enum BattleState
{
    Start,
    FirstStrike,
    PlayerTurn,
    EnemyTurn,
    Win,
    Lost
}

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
    
    [Header(" --- Battle State")]
    /// <summary>
    /// Battle State
    /// </summary>
    [SerializeField]
    private BattleState battleState;
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
    /// EnemyCollider
    /// </summary>
    [SerializeField]
    private Enemy enemyInfo;
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
    #endregion

    #region [func]

    public void SetEnemyInfo(Transform enemyTransform, Enemy enemyInfo)
    {
        // ターゲットとなるEnemyColliderのTransformを登録
        this.targetEnemyTransform = enemyTransform;
        // EnemyのScriptableObject上のデータを登録
        this.enemyInfo = enemyInfo;
    }

    /// <summary>
    /// Battle開始
    /// Player奇襲時：firstStrikeUnitNum = 0
    /// Enemy奇襲時：firstStrikeUnitNum = 1
    /// </summary>
    /// <param name="enemyTransform"></param>
    /// <param name="firstStrikeUnitNum"></param>
    public void StartBattleAnim(int firstStrikeUnitNum)
    {
        // 初期化
        this.Init();
     
        // Battle State : Start
        this.battleState = BattleState.Start;
        
        // EnemyのBattlePrefabを生成
        this.enemyBattlePrefab = Instantiate(this.enemyInfo.battlePrefab, this.enemyRootTransform);
        this.enemyAnimator = enemyBattlePrefab.GetComponent<Animator>();

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
        this.playerBattleStatusView.DOLocalMove(new Vector3(0f, 0f, 0f), 0.25f)
            .From(new Vector3(-85f, 0f, 0f))
            .SetEase(this.unitEntryEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                
            });
        
        this.enemyBattleStatusView.DOLocalMove(new Vector3(0f, 0f, 0f), 0.25f)
            .From(new Vector3(85f, 0f, 0f))
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



    #region [02. Battle開始Log]

    #region [var]
    [Header(" --- Log On Battle Start")]
    /// <summary>
    /// BattleStart時表示するLogのオブイェークトおよびそのText2種
    /// </summary>
    [SerializeField]
    private GameObject battleStartTextObj;
    [SerializeField]
    private Text battleStartText_1;
    [SerializeField]
    private Text battleStartText_2;
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
    #endregion

    
    
    #region [func]
    /// <summary>
    /// Battle開始直前のLog表示アニメーション
    /// </summary>
    /// <param name="textGroupObj"></param>
    /// <param name="onFinished"></param>
    private void BattleStartLog(string logString_1, string logString_2, int firstStrikeType)
    {
        // LogTextの中身を指定
        this.battleStartText_1.text = logString_1;
        this.battleStartText_2.text = logString_2;
        
        DOVirtual.DelayedCall(.2f, () =>
        {
            // TextGroupObjを表示Stateに変更
            this.battleStartTextObj.SetActive(true);
            
            // Anim⓵
            this.battleStartTextObj.transform.DOLocalMove(new Vector3(0f, 0f, 0f), .5f)
                .From(new Vector3(350f, 0f, 0f))
                .SetEase(Ease.Linear)
                .SetAutoKill(true)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        // Anim⓶
                        this.battleStartTextObj.transform.DOLocalMove(new Vector3(-350f, 0f, 0f), .5f)
                            .From(new Vector3(0f, 0f, 0f))
                            .SetEase(Ease.Linear)
                            .SetAutoKill(true)
                            .SetUpdate(true)
                            .OnComplete(() =>
                            {
                                DOVirtual.DelayedCall(1.25f, () =>
                                {
                                    // Anim⓷
                                    this.battleStartTextObj.transform.DOLocalMove(new Vector3(-700f, 0f, 0f), .5f)
                                        .From(new Vector3(-350f, 0f, 0f))
                                        .SetEase(Ease.Linear)
                                        .SetAutoKill(true)
                                        .SetUpdate(true)
                                        .OnComplete(() =>
                                        {
                                            // TextGroupObjを非表示Stateに変更
                                            this.battleStartTextObj.SetActive(false);
                                            
                                            // LogText初期化
                                            this.battleStartText_1.text = null;
                                            this.battleStartText_2.text = null;
                                
                                            // 座標初期化
                                            this.battleStartTextObj.transform.localPosition = new Vector3(350f, 0f, 0f);
                                                        
                                            // Battle開始
                                            this.Battle(firstStrikeType);
                                        });
                                });
                            });
                    });
                });
        });
    }
    #endregion

    #endregion
    
    
    
    #region [03. Battle]

    #region [var]
    
    #endregion

    
    
    #region [func]
    /// <summary>
    /// Battle開始
    /// </summary>
    private void Battle(int firstStrikeType)
    {
        this.battleState = BattleState.PlayerTurn;
        //StartCoroutine(PlayerAction());
    }

    /// <summary>
    /// Player Action
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayerAction()
    {
        // 
        bool isEnemyDead = false; // TODO:: EnemyのHPが0か否かを判定 

        yield return new WaitForSecondsRealtime(1f);
        
        Debug.LogFormat("Player Action_1", DColor.yellow);

        yield return new WaitForSecondsRealtime(2f);
        
        // 
        if (isEnemyDead)
        {
            // TODO :: EndLog表示　→　EndBattle
        }
        else
        {
            Debug.LogFormat("Player Action_2", DColor.yellow);
            
            // Enemy Action
            this.battleState = BattleState.EnemyTurn;
            StartCoroutine(EnemyAction());
        }

        yield return null;
    }
    
    /// <summary>
    /// Enemy Action
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyAction()
    {
        // 
        bool isPlayerDead = true; // TODO:: PlayerのHPが0か否かを判定 

        yield return new WaitForSecondsRealtime(1f);
        
        Debug.LogFormat("Enemy Action_1", DColor.yellow);
        
        yield return new WaitForSecondsRealtime(2f);
        
        // 
        if (isPlayerDead)
        {
            // TODO :: EndLog表示　→　GAME OVER
            this.EndBattle();
        }
        else
        {
            Debug.LogFormat("Enemy Action_2", DColor.yellow);
            
            // Player Action
            this.battleState = BattleState.PlayerTurn;
            StartCoroutine(PlayerAction());
        }
        
        yield return null;
    }
    
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
        
    }
    
    /// <summary>
    /// Battle終了
    /// </summary>
    public void EndBattle()
    {
        // TODO:: 臨時処理。Battle相手のEnemyを破棄。
        EnemyManager.Instance.
            ExcludeEnemyTemporarily(this.targetEnemyTransform.parent
                .GetComponent<EnemyMovementController>());
        
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
