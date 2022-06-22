using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class UIDialogController : MonoBehaviour
{
    
    #region [var]

    #region [00. Reference]
    /// <summary>
    /// UIButtonController
    /// </summary>
    [SerializeField]
    private UIButtonController uIButtonController;
    #endregion

    
    
    #region [01. General]
    [Header(" --- Dialog Objects")]
    /// <summary>
    /// ItemDialogのGameObject
    /// </summary>
    [SerializeField]
    private GameObject dialog_Item;
    public GameObject Dialog_Item { get => this.dialog_Item; }
    /// <summary>
    /// StatusInfoDialogのGameObject
    /// </summary>
    [SerializeField]
    private GameObject dialog_StatusInfo;
    public GameObject Dialog_StatusInfo { get => this.dialog_StatusInfo; }
    /// <summary>
    /// PlayerBattleDialogのGameObject
    /// </summary>
    [SerializeField]
    private GameObject dialog_Battle;
    public GameObject Dialog_Battle { get => this.dialog_Battle; }
    /// <summary>
    /// EventDialogのGameObject
    /// </summary>
    [SerializeField]
    private GameObject dialog_Event;
    public GameObject Dialog_Event { get => this.dialog_Event; }
    /// <summary>
    /// LevelUpDialogのGameObject
    /// </summary>
    [SerializeField]
    private GameObject dialog_LevelUp;
    public GameObject Dialog_LevelUp { get => this.dialog_LevelUp; }
    
    [Header(" --- Dialog Animation")]
    /// <summary>
    /// Dialogアニメーションパターン
    /// </summary>
    [SerializeField]
    private Ease diallogEase;
    /// <summary>
    /// Close時スケール
    /// </summary>
    private Vector2 closeScale = Vector2.zero;
    /// <summary>
    /// Open時スケール
    /// </summary>
    private Vector2 openScale = Vector2.one;
    /// <summary>
    /// Close時のスピード
    /// </summary>
    [SerializeField]
    private float closeSpeed_LongDialog = 0.2f;
    [SerializeField]
    private float closeSpeed_ShortDialog = 0.2f;
    /// <summary>
    /// Open時のスピード
    /// </summary>
    [SerializeField]
    private float openSpeed_LongDialog = 0.2f;
    [SerializeField]
    private float openSpeed_ShortDialog = 0.2f;
    #endregion

    
    
    #region [02. Item Dialog]
    [Header(" --- Item Dialog")]
    /// <summary>
    /// トリガー
    /// </summary>
    [SerializeField]
    private bool isItemDialog = false;
    /// <summary>
    /// Item
    /// </summary>
    [SerializeField]
    private Item currentItem;
    [SerializeField]
    private Transform currentItemTransform;
    /// <summary>
    /// Item Image
    /// </summary>
    [SerializeField]
    private Image itemImage;
    /// <summary>
    /// Item Name
    /// </summary>
    [SerializeField]
    private Text itemName;
    /// <summary>
    /// Game画面タッチ不可にするための暗幕
    /// </summary>
    [SerializeField]
    private GameObject curtain;
    /// <summary>
    /// Yes Button
    /// </summary>
    [SerializeField]
    private Button button_yes;
    #endregion

    
    
    #region [03. TurnDialog]
    /// <summary>
    /// ターン表示Dialog：Player`s Turn
    /// </summary>
    [Header(" --- Turn Dialog")]
    [SerializeField]
    private Transform playerTurnDialog;
    public Transform PlayerTurnDialog { get => this.playerTurnDialog; }
    /// <summary>
    /// ターン表示Dialog：Enemy`s Turn
    /// </summary>
    [SerializeField]
    private Transform enemyTurnDialog;
    public Transform EnemyTurnDialog { get => this.enemyTurnDialog; }
    /// <summary>
    /// ターン表示Dialogのアニメーションパターン
    /// </summary>
    [SerializeField]
    private Ease turnDialogEase;
    #endregion

    
    
    #region [04. BattleDialog]
    /// <summary>
    /// BattleDialogのアニメーションパターン
    /// </summary>
    [Header(" --- Battle Dialog")]
    [SerializeField]
    private Ease battleDialogEase;
    /// <summary>
    /// Closeボタン
    /// </summary>
    [SerializeField]
    private GameObject closeButton_BattleDialog;
    /// <summary>
    /// Battle End View
    /// </summary>
    [SerializeField]
    private GameObject battleEndView;
    [SerializeField]
    private Image battleEndViewBackgroundImage;
    /// <summary>
    /// Result Image Obj
    /// </summary>
    [SerializeField]
    private GameObject winImageObj;
    [SerializeField]
    private GameObject gameOverImageObj;
    /// <summary>
    /// Result Exp Log
    /// </summary>
    [SerializeField]
    private GameObject resultExpLogObj;
    [SerializeField]
    private Text resultExpLogText;
    #endregion
    
    
    
    #region [05. EvnetDialog]
    
    [Header(" --- Event Dialog")]
    /// <summary>
    /// MapEventが発生しているMapのMapInfo
    /// </summary>
    [SerializeField]
    private MapInfo eventDialogTargetMapInfo;
    /// <summary>
    /// MapEventのAnimator
    /// </summary>
    [SerializeField]
    private Animator mapEventAnimator;
    /// <summary>
    /// MapEventのImage
    /// </summary>
    [SerializeField]
    private Image mapEventImage;
    /// <summary>
    /// MapEventLog
    /// </summary>
    [SerializeField]
    private GameObject mapEventLogObj;
    [SerializeField]
    private Text mapEventLogText;
    /// <summary>
    /// Exit Door Log
    /// </summary>
    [SerializeField]
    private Transform exitDoorLog;
    /// <summary>
    /// LootedItem Name Obj
    /// </summary>
    [SerializeField]
    private GameObject lootedItemNameObj;
    [SerializeField]
    private Text lootedItemNameText;
    /// <summary>
    /// LootedItem Description Obj
    /// </summary>
    [SerializeField]
    private GameObject lootedItemDescriptionObj;
    [SerializeField]
    private Text lootedItemDescriptionText;
    /// <summary>
    /// LootedItem Description Obj
    /// </summary>
    [SerializeField]
    private GameObject inventoryVacantInfoLogObj;
    [SerializeField]
    private Text inventoryVacantInfoLogText;
    /// <summary>
    /// Closeボタン
    /// </summary>
    [SerializeField]
    private GameObject closeButton_EventDialog;
    #endregion
    
    #endregion


    #region [func]

    #region [01. コンストラクタ]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Awake()
    {
        // Log表示を初期化
        this.dialog_Item.transform.localScale = this.closeScale;
        this.dialog_StatusInfo.transform.localScale = this.closeScale;
        this.dialog_Event.transform.localScale = this.closeScale;
        this.dialog_Battle.transform.localScale = this.closeScale;
        this.dialog_LevelUp.transform.localScale = this.closeScale;
    }
    #endregion

    
    
    #region [02. Dialog表示/非表示]
    /// <summary>
    /// メニュー表示
    /// </summary>
    /// <param name="tranform"></param>
    public void ShowDialog(Transform dialogTransform, int size)
    {
        // Sizeによって開始時のY座標を切り替え
        float startYPos = 0;
        startYPos = size == 0 ? -345f : -150f;
        // SizeによってOpenSpeedを切り替え
        float speed = 0;
        speed = size == 0 ? this.openSpeed_LongDialog : this.openSpeed_ShortDialog;

        // ボタン押下無効
        this.uIButtonController.DisableButtonTouch();
        
        // 暗幕表示
        this.curtain.SetActive(true);
        
        // スケール変更
        dialogTransform.localScale = this.openScale;
        
        // アニメーション
        dialogTransform.DOLocalMove(new Vector3(0f, 0f, 0f), speed)
            .From(new Vector3(0f, startYPos, 0f))
            .SetEase(this.diallogEase)
            .SetAutoKill(true)
            .SetUpdate(true);

        // スケール固定
        dialogTransform.localScale = this.openScale;
    }
    
    /// <summary>
    /// メニュー非表示
    /// </summary>
    /// <param name="tranform"></param>
    public void CloseDialog(Transform dialogTransform, int size)
    {
        // Sizeによって開始時のY座標を切り替え
        float startYPos = 0;
        startYPos = size == 0 ? -345f : -150f;
        // SizeによってCloseSpeedを切り替え
        float speed = 0;
        speed = size == 0 ? this.closeSpeed_LongDialog : this.closeSpeed_ShortDialog;
        
        // アニメーション
        dialogTransform.DOLocalMove(new Vector3(0f, startYPos, 0f), speed)
            .From(new Vector3(0f, 0f, 0f))
            .SetEase(this.diallogEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                // ボタン押下有効
                this.uIButtonController.EnableButtonTouch();

                // 暗幕表示
                this.curtain.SetActive(false);
                
                // スケール変更
                dialogTransform.localScale = this.closeScale;
                
                // 初期化
                if(dialogTransform.name == "ItemLog")
                {
                    this.SetItemLogNull();
                }
            });
    }
    #endregion
    
    
    
    #region [03. Item Dialog]
    /// <summary>
    /// Item LogのItem画像および名前をセット
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="name"></param>
    public void SetItemDialog(Item item, Transform logTransform)
    {
        // ItemLog上の各種データをセット
        this.itemImage.sprite = item.itemSprite;
        this.itemName.text = item.itemName;

        // 同じItemの対象に再度ItemLogを表示する際のためのデータセット
        this.currentItem = item;
        this.currentItemTransform = logTransform;
        
        // Item格納数のステータスによって挙動を変更
        if(InventoryManager.Instance.InventoryCurrentStorageNum < InventoryManager.Instance.InventoryMaxStorageNum)
        {
            this.button_yes.enabled = true;
            this.button_yes.image.color = Color.white;
        }
        else
        {
            this.button_yes.enabled = false;
            this.button_yes.image.color = Color.gray;
        }

        // トリガーセット
        this.isItemDialog = true;
    }
    
    /// <summary>
    /// ItemLog初期化
    /// </summary>
    public void SetItemLogNull()
    {
        this.itemImage.sprite = null;
        this.itemName.text = null;

        this.currentItem = null;
        this.currentItemTransform = null;
    }

    /// <summary>
    /// YesButton押下時の処理
    /// </summary>
    public void OnClickYesButton()
    {
        if(this.isItemDialog)
        {
            // Inventoryに該当アイテムを追加
            this.currentItemTransform.parent.GetComponent<ItemController>().AddToInventory();
            InventoryManager.Instance.ListItemsOnInventory();
            
            // Log非表示
            this.CloseDialog(this.dialog_Item.transform, 1);
        }
    }

    /// <summary>
    /// NoButton押下時の処理
    /// </summary>
    public void OnClickNoButton()
    {
        if(this.isItemDialog)
        {
            // Log非表示
            this.CloseDialog(this.dialog_Item.transform, 1);
        }
    }
    #endregion

    #region [04. TurnDialog]
    /// <summary>
    /// TurnDialog表示
    /// </summary>
    /// <param name="turnDialog"></param>
    /// <param name="onFinished"></param>
    public void ShowTurnDialog(Transform turnDialog, Action onFinished)
    {
        // スケール変更
        turnDialog.localScale = this.openScale;
        
        // アニメーション
        turnDialog.DOLocalMove(new Vector3(0f, 180.7f, 0f), 1f)
            .From(new Vector3(220f, 180.7f, 0f))
            .SetEase(this.turnDialogEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                onFinished?.Invoke();
            });
    }

    /// <summary>
    /// TurnDialog非表示
    /// </summary>
    /// <param name="turnDialog"></param>
    /// <param name="onFinished"></param>
    public void CloseTurnDialog(Transform turnDialog, Action onFinished)
    {
        // アニメーション
        turnDialog.DOLocalMove(new Vector3(-220f, 180.7f, 0f), 0.75f)
            .From(new Vector3(0f, 180.7f, 0f))
            .SetEase(this.turnDialogEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                onFinished?.Invoke();

                // 座標変更
                turnDialog.localPosition = new Vector3(220f, 180.7f, 0f);
                // スケール変更
                turnDialog.localScale = this.closeScale;
            });
    }

    #endregion

    
    
    #region [05. BattleDialog]
    /// <summary>
    /// BattleDialog表示
    /// </summary>
    /// <param name="battleDialog"></param>
    /// <param name="onFinished"></param>
    public void ShowBattleDialog(Transform battleDialog, Action onFinished)
    {
        // BattleEndView非表示
        this.battleEndView.SetActive(false);
        
        // Closeボタン非表示
        this.closeButton_BattleDialog.SetActive(false);
        
        // スケール変更
        battleDialog.localScale = this.openScale;
        
        // アニメーション
        battleDialog.DOLocalMove(new Vector3(0f, 400f, 0f), 1f)
            .From(new Vector3(0f, 800f, 0f))
            .SetEase(this.battleDialogEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(.35f, () =>
                {
                    // アニメーション
                    battleDialog.DOLocalMove(new Vector3(0f, 0f, 0f), 0.8f)
                        .From(new Vector3(0f, 400f, 0f))
                        .SetEase(this.battleDialogEase)
                        .SetAutoKill(true)
                        .SetUpdate(true)
                        .OnComplete(() =>
                        {
                            onFinished?.Invoke();
                        });
                });
            });
    }

    /// <summary>
    /// Battle End Logを表示
    /// </summary>
    public void ShowBattleEndLog(bool isPlayerWin, Action onFinished)
    {
        // 初期化
        this.winImageObj.transform.localPosition = new Vector3(0f, 200f, 0f);
        this.gameOverImageObj.transform.localPosition = new Vector3(0f, 200f, 0f);
        this.resultExpLogObj.GetComponent<RectTransform>().sizeDelta = new Vector2(180f, 0f);
        
        // Player勝利か否かで、結果ImageやAnimation座標を変更
        var resultImageObj = winImageObj;
        float resultPosY = 40f;
        if (!isPlayerWin) 
        { 
            resultImageObj = this.gameOverImageObj; 
            resultPosY = 0f; 
        }

        // BattleEndView表示
        this.battleEndView.SetActive(true);
        // BackgroundImageのアニメーション
        this.battleEndViewBackgroundImage.DOFade(0.96f, 1f)
            .From(0f)
            .SetEase(Ease.Linear)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(0.3f, () =>
                {
                    // 結果表示Imageのアニメーション
                    resultImageObj.transform.DOLocalMove(new Vector3(0f, resultPosY, 0f), 0.75f)
                        .From(new Vector3(0f, 200f, 0f))
                        .SetEase(this.battleDialogEase)
                        .SetAutoKill(true)
                        .SetUpdate(true)
                        .OnComplete(() =>
                        {
                            if(!isPlayerWin)
                                onFinished?.Invoke();
                            else
                            {
                                // LogTextに獲得EXP量をセット
                                var expValue = BattleManager.Instance.EnemyExpValue;
                                this.resultExpLogText.text = "EXP +" + expValue.ToString();
                                
                                DOVirtual.DelayedCall(0.5f, () =>
                                {
                                    // 獲得EXP表示
                                    this.resultExpLogObj.GetComponent<RectTransform>().DOSizeDelta(new Vector2(180f, 40f), 0.3f)
                                        .From(new Vector2(180f, 0f))
                                        .SetEase(Ease.Linear)
                                        .SetAutoKill(true)
                                        .SetUpdate(true)
                                        .OnComplete(() =>
                                        {
                                            // Player EXP増加
                                            PlayerStatusManager.Instance.IncreaseExp(expValue);

                                            onFinished?.Invoke();
                                        });
                                });
                            }
                        });
                });
            });
    }

    /// <summary>
    /// BattleDialogのCloseボタンを表示
    /// </summary>
    public void ShowBattleDialogCloseButton()
    {
        this.closeButton_BattleDialog.SetActive(true);
    }

    /// <summary>
    /// BattleDialog非表示
    /// </summary>
    /// <param name="battleDialog"></param>
    /// <param name="onFinished"></param>
    public void CloseBattleDialog(Transform battleDialog, Action onFinished)
    {
        // アニメーション
        battleDialog.DOLocalMove(new Vector3(0f, -400f, 0f), 0.75f)
            .From(new Vector3(0f, 0f, 0f))
            .SetEase(this.battleDialogEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                onFinished?.Invoke();

                // 座標変更
                battleDialog.localPosition = new Vector3(0f, 800f, 0f);
                // スケール変更
                battleDialog.localScale = this.closeScale;
            });
    }

    #endregion
    
    
    
    #region [06. EventDialog]
    /// <summary>
    /// EventDialog表示
    /// </summary>
    /// <param name="eventDialog"></param>
    /// <param name="onFinished"></param>
    public void ShowEventDialog(Transform eventDialog, MapInfo mapInfo, Action onFinished)
    {
        // スケール変更
        eventDialog.localScale = this.closeScale;

        // MapEventがあるターゲットMapのMapInfoを記録
        this.eventDialogTargetMapInfo = mapInfo;
        
        // 該当MapのMapEvent情報
        var targetMapEvent = this.eventDialogTargetMapInfo.MapEventController.MapEvent;
        if (targetMapEvent.eventID == 0)
        {
            if (!MapEventManager.Instance.IsExitDoorOpened)
            {
                this.mapEventImage.sprite = targetMapEvent.eventSprite_Start;
                this.mapEventLogText.text = "   扉が固く閉まっている。\n   開けるには鍵が必要だ。";
            }
            else
            {
                this.mapEventImage.sprite = targetMapEvent.eventSprite_Change;
                this.mapEventLogText.text = "   扉の奥に階段が見える。\n   階段に進む？";
            }
        }
        else
        {
            // 開示前のMapEventSpriteをセット
            this.mapEventImage.sprite = targetMapEvent.eventSprite_Start;
        }
        
        // アニメーション
        eventDialog.DOScale(1.0f, this.openSpeed_LongDialog)
            .From(this.closeScale)
            .SetEase(this.battleDialogEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(0.2f, () =>
                {
                    // MapEvent開示
                    this.ShowMapEvent(targetMapEvent , this.eventDialogTargetMapInfo.MapEventController);
                });
                
                onFinished?.Invoke();
            });
    }

    /// <summary>
    /// EventDialog非表示
    /// </summary>
    /// <param name="eventDialog"></param>
    /// <param name="onFinished"></param>
    public void CloseEventDialog(Transform eventDialog, Action onFinished)
    {
        // アニメーション
        eventDialog.DOScale(0f, this.closeSpeed_LongDialog)
            .From(this.openScale)
            .SetEase(this.battleDialogEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                // 初期化
                this.InitEventDialog();
                
                if(this.eventDialogTargetMapInfo.MapEventController.MapEvent.eventID != 0)
                    // 該当MapEventの終了トリガーを発動
                    this.eventDialogTargetMapInfo.SetMapEventFinishedTriggerOn();

                // スケール変更
                eventDialog.localScale = this.closeScale;

                if (!MapEventManager.Instance.IsExitDoorLogShow)
                    onFinished?.Invoke();
                else
                {
                    // 
                    this.ShowExitDoorLog(() =>
                    {
                        MapEventManager.Instance.SetExitDoorLogBoolState(false);
                        
                        onFinished?.Invoke();
                    });
                }
            });
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="onFinished"></param>
    private void ShowExitDoorLog(Action onFinished)
    {
        // アニメーション
        exitDoorLog.DOScale(1.0f, this.openSpeed_LongDialog)
            .From(this.closeScale)
            .SetEase(this.battleDialogEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(1f, () =>
                {
                    // アニメーション
                    exitDoorLog.DOScale(0f, this.closeSpeed_LongDialog)
                        .From(this.openScale)
                        .SetEase(this.battleDialogEase)
                        .SetAutoKill(true)
                        .SetUpdate(true)
                        .OnComplete(() =>
                        {
                            // スケール変更
                            exitDoorLog.localScale = this.closeScale;

                            onFinished?.Invoke();
                        });
                });
            });
    }

    /// <summary>
    /// EventDialog初期化
    /// </summary>
    private void InitEventDialog()
    {
        this.mapEventImage.sprite = null;
        this.mapEventAnimator.transform.localPosition = Vector3.zero;
        this.mapEventLogObj.GetComponent<RectTransform>().sizeDelta = new Vector2(180f, 0f);
        this.lootedItemNameObj.GetComponent<RectTransform>().sizeDelta = new Vector2(180f, 0f);
        this.lootedItemDescriptionObj.GetComponent<RectTransform>().sizeDelta = new Vector2(180f, 0f);
        this.inventoryVacantInfoLogObj.GetComponent<RectTransform>().sizeDelta = new Vector2(180f, 0f);
        
        this.closeButton_EventDialog.SetActive(false);
    }

    // MapEvent開示
    public void ShowMapEvent(MapEvent targetMapEvent, MapEventController targetMapEventController)
    {
        // 
        if (targetMapEvent.eventID != 0)
        {
            // 開示アニメーション再生
            this.mapEventAnimator.SetTrigger("Show");

            // アニメーション再生中のEvent
            this.mapEventAnimator.GetComponent<AnimationCallBack>().EventOnPlayingAnimation(() =>
            {
                // 開示用のMapEventSpriteに変更
                this.mapEventImage.sprite = targetMapEvent.eventSprite_Open;
                this.mapEventLogText.text = targetMapEvent.eventDescription;
            });

            // アニメーション終了後のEvent
            this.mapEventAnimator.GetComponent<AnimationCallBack>().EndAnimation(() =>
            {
                DOVirtual.DelayedCall(0.4f, () =>
                {
                    // 
                    if (targetMapEvent.eventID == 2)
                    {
                        // 開示アニメーション再生
                        this.mapEventAnimator.SetTrigger("Open");

                        // アニメーション再生中のEvent
                        this.mapEventAnimator.GetComponent<AnimationCallBack>().EventOnPlayingAnimation(() =>
                        {
                            // 開示用のMapEventSpriteに変更
                            this.mapEventImage.sprite = targetMapEventController.LootedItem.itemSprite;
                            this.lootedItemNameText.text = targetMapEventController.LootedItem.itemName;
                            this.lootedItemDescriptionText.text = targetMapEventController.LootedItem.itemDescription;

                            DOVirtual.DelayedCall(1f, () =>
                            {
                                // 
                                this.mapEventAnimator.transform.DOLocalMove(new Vector3(0f, 45f, 0f), 1f)
                                    .SetEase(Ease.Linear).SetAutoKill(true).SetUpdate(true)
                                    .OnComplete(() =>
                                    {
                                        // 
                                        this.lootedItemNameObj.GetComponent<RectTransform>()
                                            .DOSizeDelta(new Vector2(180f, 20f), 1f)
                                            .From(new Vector2(180f, 0f)).SetEase(Ease.Linear).SetAutoKill(true)
                                            .SetUpdate(true)
                                            .OnComplete(() =>
                                            {
                                                // 
                                                this.lootedItemDescriptionObj.GetComponent<RectTransform>()
                                                    .DOSizeDelta(new Vector2(180f, 100f), 1f)
                                                    .From(new Vector2(180f, 0f)).SetEase(Ease.Linear).SetAutoKill(true)
                                                    .SetUpdate(true)
                                                    .OnComplete(() =>
                                                    {
                                                        // 
                                                        MapEventManager.Instance.DoWhatMapEventDoes(targetMapEvent, targetMapEventController);
                                                    });
                                            });
                                    });
                            });
                        });
                    }
                    // 
                    else
                    {
                        // 
                        this.mapEventAnimator.transform.DOLocalMove(new Vector3(0f, 45f, 0f), 1f)
                            .SetEase(Ease.Linear).SetAutoKill(true).SetUpdate(true)
                            .OnComplete(() =>
                            {
                                // 
                                this.mapEventLogObj.GetComponent<RectTransform>().DOSizeDelta(new Vector2(180f, 100f), 1f)
                                    .From(new Vector2(180f, 0f)).SetEase(Ease.Linear).SetAutoKill(true).SetUpdate(true)
                                    .OnComplete(() =>
                                    {
                                        // ボタン表示
                                        this.closeButton_EventDialog.SetActive(true);
                                        // 
                                        MapEventManager.Instance.DoWhatMapEventDoes(targetMapEvent, targetMapEventController);
                                    });
                            });;
                    }
                });
            });
        }
        // 
        else
        {
            // 
            this.mapEventAnimator.transform.DOLocalMove(new Vector3(0f, 45f, 0f), 1f)
                .SetEase(Ease.Linear).SetAutoKill(true).SetUpdate(true)
                .OnComplete(() =>
                {
                    // 
                    this.mapEventLogObj.GetComponent<RectTransform>().DOSizeDelta(new Vector2(180f, 100f), 1f)
                        .From(new Vector2(180f, 0f)).SetEase(Ease.Linear).SetAutoKill(true).SetUpdate(true)
                        .OnComplete(() =>
                        {
                            // ボタン表示
                            this.closeButton_EventDialog.SetActive(true);
                            // 
                            MapEventManager.Instance.DoWhatMapEventDoes(targetMapEvent, targetMapEventController);
                        });
                });;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isInventoryMax"></param>
    /// <param name="onFinished"></param>
    public void SetInventoryVacantInfoLog(bool isInventoryMax, Action onFinished)
    {
        if (isInventoryMax)
        {
            this.inventoryVacantInfoLogText.text = "バッグがいっぱいだ\nItemは諦めよう";
            this.inventoryVacantInfoLogText.color = new Color(0.9716981f, 0.4170968f, 0.4228849f);
        }
        else
        {
            this.inventoryVacantInfoLogText.text = "Itemをバッグに入れた";
            this.inventoryVacantInfoLogText.color = new Color(0.9764706f, 0.8901961f, 0.8039216f);
        }
        
        // 
        this.inventoryVacantInfoLogObj.GetComponent<RectTransform>().DOSizeDelta(new Vector2(180f, 30f), 1f)
            .From(new Vector2(180f, 0f)).SetEase(Ease.Linear).SetAutoKill(true).SetUpdate(true)
            .OnComplete(() =>
            {
                // ボタン表示
                this.closeButton_EventDialog.SetActive(true);
                        
                onFinished?.Invoke();
            });
    }

    #region [07.LevelUpDialog]
    /// <summary>
    /// LevelDialog表示
    /// </summary>
    /// <param name="levelUpDialog"></param>
    /// <param name="onFinished"></param>
    public void ShowLevelUpDialog(Transform levelUpDialog, Action onFinished)
    {
        // スケール変更
        levelUpDialog.localScale = this.closeScale;
        
        // アニメーション
        levelUpDialog.DOScale(1f, this.openSpeed_LongDialog)
            .From(this.closeScale)
            .SetEase(this.battleDialogEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                this.PlayLevelUpDialogAnim();
                
                onFinished?.Invoke();
            });
    }

    [Header(" --- LevelUp Dialog")]
    [SerializeField]
    private GameObject levelUpDialogTitleImage;
    [SerializeField]
    private GameObject levelUpDialogSubtitleObj;
    
    [SerializeField]
    private GameObject StatusButtonObj_1;
    [SerializeField]
    private Button StatusButtonComponent_1;
    [SerializeField]
    private Text StatusButtonText_1;
    
    [SerializeField]
    private GameObject StatusButtonObj_2;
    [SerializeField]
    private Button StatusButtonComponent_2;
    [SerializeField]
    private Text StatusButtonText_2;
    
    [SerializeField]
    private GameObject StatusButtonObj_3;
    [SerializeField]
    private Button StatusButtonComponent_3;
    [SerializeField]
    private Text StatusButtonText_3;
    
    [SerializeField]
    private GameObject StatusButtonObj_4;
    [SerializeField]
    private Button StatusButtonComponent_4;
    [SerializeField]
    private Text StatusButtonText_4;
    
    [SerializeField]
    private GameObject StatusButtonObj_5;
    [SerializeField]
    private Button StatusButtonComponent_5;
    [SerializeField]
    private Text StatusButtonText_5;

    [SerializeField]
    private List<GameObject> statusObjList = new List<GameObject>();
    
    [SerializeField]
    private List<Button> statusButtonList = new List<Button>();
    
    [SerializeField]
    private List<Text> statusTextList = new List<Text>();
    
    [SerializeField]
    private List<string> statusStringList = new List<string>();
    
    [SerializeField]
    private GameObject closeButton_LevelUpDialog;

    private void PlayLevelUpDialogAnim()
    {
        this.levelUpDialogTitleImage.transform.localPosition = Vector3.zero;
        this.levelUpDialogSubtitleObj.GetComponent<RectTransform>().sizeDelta = new Vector2(180f, 0f);

        for (int i = 0; i < statusStringList.Count; i++) {
            string tmp = statusStringList[i];
            int randomIndex = UnityEngine.Random.Range(i, statusStringList.Count);
            statusStringList[i] = statusStringList[randomIndex];
            statusStringList[randomIndex] = tmp;
        }
        
        // for (int i = 0; i < statusStringList.Count; i++)
        // {
        //     this.statusTextList[i].text = statusStringList[i];
        // }
        
        var randomStatusCount = UnityEngine.Random.Range(2, 6);

        this.buttonLimitCount = randomStatusCount;

        for (int num = 0; num < randomStatusCount; num++)
        {
            this.statusObjList[num].SetActive(true);
            this.statusButtonList[num].enabled = false;
        }
        
        this.levelUpDialogTitleImage.transform.DOLocalMove(new Vector3(0f, 80f, 0f), 1f)
            .From(new Vector3(0f, 0f, 0f))
            .SetEase(this.diallogEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                this.levelUpDialogSubtitleObj.GetComponent<RectTransform>().DOSizeDelta(new Vector2(180f, 40f), 0.3f)
                    .From(new Vector2(180f, 0f))
                    .SetEase(Ease.Linear)
                    .SetAutoKill(true)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        for (int num = 0; num < randomStatusCount; num++)
                        {
                            this.statusObjList[num].GetComponent<RectTransform>()
                                .DOSizeDelta(new Vector2(180f, 20f), 0.2f)
                                .From(new Vector2(0f, 20f))
                                .SetEase(Ease.Linear)
                                .SetAutoKill(true)
                                .SetUpdate(true)
                                .OnComplete(() =>
                                {
                                    for (int num = 0; num < randomStatusCount; num++)
                                    {
                                        this.statusButtonList[num].enabled = true;
                                    }
                                });
                        }
                    });
            });
    }


    public void OnClickBonusStatusButton(int buttonNum)
    {
        if (this.statusStringList[buttonNum - 1] == "MAXHP")
        {
            this.statusButtonList[buttonNum - 1].GetComponent<Image>().enabled = false;

            var randomStatusValue = UnityEngine.Random.Range(25, 61);

            this.statusTextList[buttonNum - 1].color = new Color(0.1933962f, 0.4911714f, 1f);
            this.statusTextList[buttonNum - 1].text = "Max HP +" + randomStatusValue.ToString();
            
            PlayerStatusManager.Instance.IncreaseMaxHp(randomStatusValue);
        }
        
        if (this.statusStringList[buttonNum - 1] == "ATK")
        {
            this.statusButtonList[buttonNum - 1].GetComponent<Image>().enabled = false;

            var randomStatusValue = UnityEngine.Random.Range(10, 21);

            this.statusTextList[buttonNum - 1].color = new Color(0.1933962f, 0.4911714f, 1f);
            this.statusTextList[buttonNum - 1].text = "ATK +" + randomStatusValue.ToString();
            
            PlayerStatusManager.Instance.IncreaseAttack(randomStatusValue);
        }
        
        if (this.statusStringList[buttonNum - 1] == "CRI")
        {
            this.statusButtonList[buttonNum - 1].GetComponent<Image>().enabled = false;

            var randomStatusValue = UnityEngine.Random.Range(2, 11);

            this.statusTextList[buttonNum - 1].color = new Color(0.1933962f, 0.4911714f, 1f);
            this.statusTextList[buttonNum - 1].text = "CRI +" + randomStatusValue.ToString();
            
            PlayerStatusManager.Instance.IncreaseCritical(randomStatusValue);
        }
        
        if (this.statusStringList[buttonNum - 1] == "DEF")
        {
            this.statusButtonList[buttonNum - 1].GetComponent<Image>().enabled = false;

            var randomStatusValue = UnityEngine.Random.Range(5, 15);

            this.statusTextList[buttonNum - 1].color = new Color(0.1933962f, 0.4911714f, 1f);
            this.statusTextList[buttonNum - 1].text = "DEF +" + randomStatusValue.ToString();
            
            PlayerStatusManager.Instance.IncreaseDefence(randomStatusValue);
        }
        
        if (this.statusStringList[buttonNum - 1] == "AGI")
        {
            this.statusButtonList[buttonNum - 1].GetComponent<Image>().enabled = false;

            var randomStatusValue = UnityEngine.Random.Range(2, 11);

            this.statusTextList[buttonNum - 1].color = new Color(0.1933962f, 0.4911714f, 1f);
            this.statusTextList[buttonNum - 1].text = "AGI +" + randomStatusValue.ToString();
            
            PlayerStatusManager.Instance.IncreaseAgility(randomStatusValue);
        }

        this.AddBonusStatusButtonPushedCount();
    }

    private int buttonLimitCount = 0;
    private int buttonPushedCount = 0;
    
    private void AddBonusStatusButtonPushedCount()
    {
        this.buttonPushedCount += 1;
        
        if(this.buttonPushedCount >= this.buttonLimitCount)
            this.closeButton_LevelUpDialog.SetActive(true);
    }
    
    
    /// <summary>
    /// LevelDialog非表示
    /// </summary>
    /// <param name="levelUpDialog"></param>
    /// <param name="onFinished"></param>
    public void CloseLevelUpDialog(Transform levelUpDialog, Action onFinished)
    {
        // アニメーション
        levelUpDialog.DOScale(0f, this.closeSpeed_LongDialog)
            .From(this.openScale)
            .SetEase(this.battleDialogEase)
            .SetAutoKill(true)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                this.levelUpDialogTitleImage.transform.localPosition = Vector3.zero;
                
                this.levelUpDialogSubtitleObj.GetComponent<RectTransform>().sizeDelta = new Vector2(180f, 0f);

                foreach (var obj in this.statusObjList)
                {
                    obj.SetActive(false);
                    obj.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 20f);
                }
                
                foreach (var button in this.statusButtonList)
                {
                    button.enabled = false;
                    button.GetComponent<Image>().enabled = true;
                }
                
                foreach (var text in this.statusTextList)
                {
                    text.text = "???";
                    text.color = new Color(0.9764706f, 0.8901961f, 0.8039216f);
                }
                
                this.closeButton_LevelUpDialog.SetActive(false);

                this.buttonPushedCount = 0;

                // スケール変更
                levelUpDialog.localScale = this.closeScale;
            });
    }

    #endregion
    #endregion
    #endregion
}
