using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;
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
    private Vector3 playerStartPos = new Vector3(-200f, -80f, 0f);
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
        this.playerTransform.localPosition = playerStartPos;
        this.enemyTransform.localPosition = enemyStartPos;
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
    
    [Header(" --- Unit Animation Data")]
    /// <summary>
    /// 各UnitのTransform
    /// </summary>
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Transform enemyTransform;
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
    #endregion

    #region [func]
    /// <summary>
    /// Battle開始
    /// Player奇襲時：firstStrikeUnitNum = 0
    /// Enemy奇襲時：firstStrikeUnitNum = 1
    /// </summary>
    /// <param name="enemyTransform"></param>
    /// <param name="firstStrikeUnitNum"></param>
    public void StartBattleAnim(Transform enemyTransform, int firstStrikeUnitNum)
    {
        // ターゲットとなるEnemyのColliderを登録
        this.targetEnemyTransform = enemyTransform;
        
        // 初期化
        this.Init();

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
                    this.BattleStartLog(this.playerFirstStrikeFailedObj, () => { });
                });
            }
            else
            {
                // Unit登場アニメーションの再生：Player奇襲Battle時
                this.UnitEntryAnimOnPlayerFirstStrikeBattle(() =>
                {
                    // Battle開始直前のLog表示アニメーション
                    this.BattleStartLog(this.playerFirstStrikeSucceededObj, () => { });
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
                    this.BattleStartLog(this.enemyFirstStrikeFailedObj, () => { });
                });
            }
            else
            {
                // Unit登場アニメーションの再生：Enemy奇襲Battle時
                this.UnitEntryAnimOnEnemyFirstStrikeBattle(() =>
                {
                    // Battle開始直前のLog表示アニメーション
                    this.BattleStartLog(this.enemyFirstStrikeSucceededObj, () => { });
                });
            }
        }
    }
    
    /// <summary>
    /// Unit登場アニメーションの再生：通常Battle時
    /// </summary>
    private void UnitEntryAnimOnNormalBattle(Action onFinished)
    {
        this.playerTransform.DOLocalMove(new Vector3(50f, -80f, 0f), 0.5f)
            .From(new Vector3(-200f, -80f, 0f))
            .SetEase(this.unitEntryEase)
            .SetAutoKill(true)
            .SetUpdate(true);
        
        this.enemyTransform.DOLocalMove(new Vector3(-50f, 80f, 0f), 0.5f)
            .From(new Vector3(200f, 80f, 0f))
            .SetEase(this.unitEntryEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                onFinished?.Invoke();
            });
    }
    
    /// <summary>
    /// Unit登場アニメーションの再生：Player奇襲Battle時
    /// </summary>
    private void UnitEntryAnimOnPlayerFirstStrikeBattle(Action onFinished)
    {
        this.playerTransform.DOLocalMove(new Vector3(50f, -80f, 0f), 0.5f)
            .From(new Vector3(-200f, -80f, 0f))
            .SetEase(this.unitEntryEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                this.enemyTransform.DOLocalMove(new Vector3(-50f, 80f, 0f), 0.5f)
                .From(new Vector3(200f, 80f, 0f))
                .SetEase(this.unitEntryEase)
                .SetAutoKill(true)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    onFinished?.Invoke();
                });
            });
    }
    
    /// <summary>
    /// Unit登場アニメーションの再生：Enemy奇襲Battle時
    /// </summary>
    private void UnitEntryAnimOnEnemyFirstStrikeBattle(Action onFinished)
    {
        this.enemyTransform.DOLocalMove(new Vector3(-50f, 80f, 0f), 0.5f)
            .From(new Vector3(200f, 80f, 0f))
            .SetEase(this.unitEntryEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                this.playerTransform.DOLocalMove(new Vector3(50f, -80f, 0f), 0.5f)
                    .From(new Vector3(-200f, -80f, 0f))
                    .SetEase(this.unitEntryEase)
                    .SetAutoKill(true)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        onFinished?.Invoke();
                    });
            });
    }
    #endregion
    
    #endregion



    #region [02. Battle開始Log]

    #region [var]
    [Header(" --- Log On Battle Start")]
    /// <summary>
    /// Player奇襲成功時のLog
    /// </summary>
    [SerializeField]
    private GameObject playerFirstStrikeSucceededObj;
    /// Player奇襲失敗時のLog
    /// </summary>
    [SerializeField]
    private GameObject playerFirstStrikeFailedObj;
    /// Enemy奇襲成功時のLog
    /// </summary>
    [SerializeField]
    private GameObject enemyFirstStrikeSucceededObj;
    /// <summary>
    /// Enemy奇襲失敗時のLog
    /// </summary>
    [SerializeField]
    private GameObject enemyFirstStrikeFailedObj;
    

    #endregion

    
    #region [func]
    /// <summary>
    /// Battle開始直前のLog表示アニメーション
    /// </summary>
    /// <param name="textGroupObj"></param>
    /// <param name="onFinished"></param>
    private void BattleStartLog(GameObject textGroupObj, Action onFinished)
    {
        DOVirtual.DelayedCall(.2f, () =>
        {
            // TextGroupObjを表示Stateに変更
            textGroupObj.SetActive(true);
            
            // Anim⓵
            textGroupObj.transform.DOLocalMove(new Vector3(0f, 0f, 0f), .5f)
                .From(new Vector3(350f, 0f, 0f))
                .SetEase(Ease.Linear)
                .SetAutoKill(true)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        // Anim⓶
                        textGroupObj.transform.DOLocalMove(new Vector3(-350f, 0f, 0f), .5f)
                            .From(new Vector3(0f, 0f, 0f))
                            .SetEase(Ease.Linear)
                            .SetAutoKill(true)
                            .SetUpdate(true)
                            .OnComplete(() =>
                            {
                                DOVirtual.DelayedCall(1.25f, () =>
                                {
                                    // Anim⓷
                                    textGroupObj.transform.DOLocalMove(new Vector3(-700f, 0f, 0f), .5f)
                                        .From(new Vector3(-350f, 0f, 0f))
                                        .SetEase(Ease.Linear)
                                        .SetAutoKill(true)
                                        .SetUpdate(true)
                                        .OnComplete(() =>
                                        {
                                            // TextGroupObjを非表示Stateに変更
                                            textGroupObj.SetActive(false);
                                
                                            // 座標初期化
                                            textGroupObj.transform.localPosition = new Vector3(350f, 0f, 0f);
                                                        
                                            onFinished?.Invoke();
                                        });
                                });
                            });
                    });
                });
        });
    }
    #endregion

    #endregion
    
    
    
    #region [03. Battle開始]

    #region [var]
    
    #endregion

    #region [func]
    /// <summary>
    /// Battle開始
    /// </summary>
    public void StartBattle()
    {
        
    }
    #endregion

    #endregion
    
    
    
    #region [05. Battle終了]

    #region [var]
    
    #endregion

    #region [func]
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
