using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIButtonController : MonoBehaviour
{
    #region [var]

    #region [01. 参照]
    [Header(" --- Reference")]
    /// <summary>
    /// PlayerMovementController
    /// </summary>
    [SerializeField]
    private PlayerMovementController playerMovementController;
    /// <summary>
    /// UIMenuController
    /// </summary>
    [SerializeField]
    private UIMenuController uIMenuController;
    /// <summary>
    /// UITargetPointerController
    /// </summary>
    [SerializeField]
    private UITargetPointerController uITargetPointerController;
    #endregion

    #region [02. ボタン]

    [Header(" --- Buttons")]
    /// <summary>
    /// セッティングメニューボタン
    /// </summary>
    [SerializeField]
    private Button settingButton;
    /// <summary>
    /// 移動モード切り替えボタン
    /// </summary>
    [SerializeField]
    private Button movementModeToggleButton;
    /// <summary>
    /// インベントリーボタン
    /// </summary>
    [SerializeField]
    private Button inventoryButton;
    /// <summary>
    /// HUD表示切り替えボタン
    /// </summary>
    [SerializeField]
    private Button hUDActivationToggleButton_On;
    [SerializeField]
    private Button hUDActivationToggleButton_Off;
    /// <summary>
    /// プレイヤーの移動ボタン
    /// </summary>
    [SerializeField]
    private Button[] movementButtonsForPlayer;
    /// <summary>
    /// プレイヤーの移動ボタン:北
    /// </summary>
    [SerializeField]
    private Button northButton;
    public Button NorthButton { get => this.northButton; }
    /// <summary>
    /// プレイヤーの移動ボタン:東
    /// </summary>
    [SerializeField]
    private Button eastButton;
    public Button EastButton { get => this.eastButton; }
    /// <summary>
    /// プレイヤーの移動ボタン:南
    /// </summary>
    [SerializeField]
    private Button southButton;
    public Button SouthButton { get => this.southButton; }
    /// <summary>
    /// プレイヤーの移動ボタン:西
    /// </summary>
    [SerializeField]
    private Button westButton;
    public Button WestButton { get => this.westButton; }
    /// <summary>
    /// カメラの移動ボタン
    /// </summary>
    [SerializeField]
    private Button[] movementButtonsForCamera;
    /// <summary>
    /// Interactボタン
    /// </summary>
    [SerializeField]
    private Button interactButton;
    #endregion

    #region [03. ボタンオブジェクト]
    [Header(" --- Button Objects")]
    /// <summary>
    /// プレイヤー移動ボタンのGameObject
    /// </summary>
    [SerializeField]
    private GameObject movementButtonObjForPlayer;
    /// <summary>
    /// カメラ移動ボタンのGameObject
    /// </summary>
    [SerializeField]
    private GameObject movementButtonObjForCamera;
    /// <summary>
    /// InteractボタンのGameObject
    /// </summary>
    [SerializeField]
    private GameObject interactButtonObj;
    #endregion
    
    #region [04. ボタンオブジェクトリスト]
    [Header(" --- Button Object List")]
    /// <summary>
    /// HUD表示切り替え対象のGameObjectリスト
    /// </summary>
    [SerializeField]
    private GameObject[] buttonObjsForHUDActivation;
    #endregion
    
    #region [05. Image]
    [Header(" --- Button Object Image")]
    /// <summary>
    /// 移動モード切り替えボタンのImage
    /// </summary>
    [SerializeField]
    private Image movementModeToggleButtonImage;
    /// <summary>
    /// InteractボタンtのImage
    /// </summary>
    [SerializeField]
    private Image interactButtonImage;
    
    [Header(" --- Button Image List")]
    /// <summary>
    /// 押下可否切り替え時変更対象ののボタンイメージリスト
    /// </summary>
    [SerializeField]
    private Image[] buttonImagesForDisable;
    [SerializeField]
    private Image[] buttonImagesForDisableExpectMovementButton;
    
    [Header(" --- Alpha(Color) for Button Disable")]
    /// <summary>
    /// 押下不可時のボタンの透明度
    /// </summary>
    [SerializeField]
    private float buttonDisabledImageValue = 0.3f;
    #endregion

    #region [04. トリガー]
    /// <summary>
    /// 移動ボタン表示の切り替えトリガー
    /// </summary>
    private bool isButtonForCameraMovement = false;
    /// <summary>
    /// HUD表示の切り替えトリガー
    /// </summary>
    private bool isHUDActivationOff = false;
    #endregion
    
    #region [05. コールバック]
    /// <summary>
    /// 移動終了コールバック
    /// </summary>
    private Action onCompleteMovement;
    #endregion

    #endregion
    

    #region [func]

    #region [01. コンストラクタ]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public void SetUIButton(PlayerMovementController playerMovementControllerScript)
    {
        // スクリプトセット
        this.playerMovementController = playerMovementControllerScript;
        
        // 各種ボタンの初期化
        this.SetMovementButtonState(this.isButtonForCameraMovement);
        this.SetInteractButtonState(false);
    }
    #endregion
    
    #region [02. ボタンのステート管理]
    /// <summary>
    /// 移動ボタンの表示切り替え
    /// </summary>
    /// <param name="state"></param>
    public void SetMovementButtonState(bool state)
    {
        // カメラ移動ボタンの表示ステート変更
        this.movementButtonObjForCamera.SetActive(state);
        // カメラポインター表示ステート変更
        this.uITargetPointerController.SetCameraPointerAvtivation(state);
        
        // プレイヤー移動ボタンの表示ステート変更
        this.movementButtonObjForPlayer.SetActive(!state);
        
        // カメラ座標のリセット
        this.playerMovementController.ResetCameraPosition();
    }

    /// <summary>
    /// HUD表示切り替え
    /// </summary>
    /// <param name="state"></param>
    private void SetHUDActivationState(bool state)
    {
        foreach (var button in this.buttonObjsForHUDActivation)
        {
            // 対象ボタンの表示ステート変更
            button.SetActive(state);
        }
    }
    
    /// <summary>
    /// Interactボタンの表示ステート切り替え
    /// </summary>
    /// <param name="state"></param>
    public void SetInteractButtonState(bool state)
    {
        this.interactButtonObj.SetActive(state);
    }
    #endregion
    
    #region [03. ボタン押下処理]
    /// <summary>
    /// Settingsボタン、または、Inventoryボタン押下時の処理
    /// </summary>
    public void OnClickMenuButton(Transform transform)
    {
        // 該当メニューを表示
        this.uIMenuController.OnClickShowMenuButton(transform);
    }
    
    /// <summary>
    /// 移動ボタンの表示切り替えボタン押下時の処理
    /// </summary>
    public void OnClickChangeMovementButtonStateButton()
    {
        if (this.isButtonForCameraMovement)
        {
            // トリガーをセット
            this.isButtonForCameraMovement = false;
            // ボタンImageの表示切り替え
            // TODO :: アルファ時にSprite切り替えに変更
            this.movementModeToggleButtonImage.color = Color.white;
        }
        else
        {
            // トリガーをセット
            this.isButtonForCameraMovement = true;
            // ボタンImageの表示切り替え
            // TODO :: アルファ時にSprite切り替えに変更
            this.movementModeToggleButtonImage.color = Color.green;
        }
        
        // 移動ボタンの表示切り替え
        this.SetMovementButtonState(this.isButtonForCameraMovement);
    }

    /// <summary>
    /// 移動ボタンの表示切り替えボタン押下時の処理
    /// </summary>
    public void OnClickChangeHUDActivationStateButton()
    {
        if (this.isHUDActivationOff)
        {
            // トリガーをセット
            this.isHUDActivationOff = false;
            // ボタン切り替え
            this.hUDActivationToggleButton_On.gameObject.SetActive(true);
            this.hUDActivationToggleButton_Off.gameObject.SetActive(false);
        }
        else
        {
            // トリガーをセット
            this.isHUDActivationOff = true;
            // ボタン切り替え
            this.hUDActivationToggleButton_On.gameObject.SetActive(false);
            this.hUDActivationToggleButton_Off.gameObject.SetActive(true);
        }

        // HUDの表示切り替え
        this.SetHUDActivationState(!this.isHUDActivationOff);
    }

    /// <summary>
    /// カメラ移動ボタン押下時の処理を中継
    /// </summary>
    /// <param name="directionStr"></param>
    public void OnClickCameraMovementButton(string directionStr)
    {
        // ボタン押下を一時的に無効化
        this.SetMovementButtonEnableState(this.playerMovementController.CameraMoveSpeed);
        // 実際の処理
        this.playerMovementController.OnClickCameraMovementButton(directionStr);
    }
    
    /// <summary>
    /// Interactボタン押下時の処理
    /// </summary>
    public void OnClickInteractButton()
    {
        // Itemに接触している場合
        var item = PlayerStatusManager.Instance.CurrentContactingItem;
        if (item != null)
        {
            PlayerStatusManager.Instance.OnClickInteractButton();
        }
    }
    #endregion
    
    #region [04. ボタンコンポネントのステート管理]
    /// <summary>
    /// 移動ボタンのコンポネントEnableステート切り替え
    /// </summary>
    /// <param name="delay"></param>
    private void SetMovementButtonEnableState(float delay)
    {
        // 各種ボタンコンポネントをDisable
        // TODO :: ボタンImageの表示切り替え処理を追加
        this.settingButton.enabled = false;
        this.inventoryButton.enabled = false;
        this.movementModeToggleButton.enabled = false;
        this.interactButton.enabled = false;
        foreach (var button in this.movementButtonsForPlayer)　button.enabled = false;
        foreach (var button in this.movementButtonsForCamera)　button.enabled = false;
        
        // ボタンイメージ変更
        this.SetButtonImageForDisable();

        // 遅延処理
        DOVirtual.DelayedCall(delay, () =>
        {
            // 各種ボタンコンポネントをEnable
            // TODO :: ボタンImageの表示切り替え処理を追加
            this.settingButton.enabled = true;
            this.inventoryButton.enabled = true;
            this.movementModeToggleButton.enabled = true;
            this.interactButton.enabled = true;
            foreach (var button in this.movementButtonsForPlayer)　button.enabled = true;
            foreach (var button in this.movementButtonsForCamera)　button.enabled = true;
            
            // ボタンイメージ変更
            this.SetButtonImageForEnable();
        });
    }
    
    /// <summary>
    /// DisableButtonTouch
    /// </summary>
    public void DisableButtonTouch()
    {
        // 各種ボタンコンポネントをDisable
        this.settingButton.enabled = false;
        this.inventoryButton.enabled = false;
        this.movementModeToggleButton.enabled = false;
        this.interactButton.enabled = false;
        foreach (var button in this.movementButtonsForPlayer)　button.enabled = false;
        foreach (var button in this.movementButtonsForCamera)　button.enabled = false;

        // ボタンイメージ変更
        this.SetButtonImageForDisable();
    }
    
    /// <summary>
    /// EnableButtonTouch
    /// </summary>
    public void EnableButtonTouch()
    {
        // 各種ボタンコンポネントをEnable
        this.settingButton.enabled = true;
        this.inventoryButton.enabled = true;
        this.movementModeToggleButton.enabled = true;
        this.interactButton.enabled = true;
        foreach (var button in this.movementButtonsForPlayer)　button.enabled = true;
        foreach (var button in this.movementButtonsForCamera)　button.enabled = true;
        
        // ボタンイメージ変更
        this.SetButtonImageForEnable();
        this.EnableButtonTouchExpectMovementButton();
    }
    
    /// <summary>
    /// EnableButtonTouch
    /// </summary>
    public void EnableButtonTouchExpectMovementButton()
    {
        // 各種ボタンコンポネントをEnable
        this.settingButton.enabled = true;
        this.inventoryButton.enabled = true;
        this.movementModeToggleButton.enabled = true;
        this.interactButton.enabled = true;
        foreach (var button in this.movementButtonsForCamera)　button.enabled = true;
        
        // ボタンイメージ変更
        foreach (var image in this.buttonImagesForDisableExpectMovementButton)
            image.GetComponent<UIButtonImageStateController>().SetEnabledSprite();
        
        UnitTurnManager.Instance.GetMapInfo();
    }

    /// <summary>
    /// ボタンイメージ変更：Disable
    /// </summary>
    private void SetButtonImageForDisable()
    {
        foreach (var image in this.buttonImagesForDisable)
            image.GetComponent<UIButtonImageStateController>().SetDisabledSprite();
    }
    
    /// <summary>
    /// ボタンイメージ変更：Enable
    /// </summary>
    private void SetButtonImageForEnable()
    {
        foreach (var image in this.buttonImagesForDisable)
            image.GetComponent<UIButtonImageStateController>().SetEnabledSprite();
    }

    public void SetEachMovementButtonEnableState(Button button, bool state)
    {
        button.enabled = state;
        if (state)
            button.GetComponent<UIButtonImageStateController>().SetEnabledSprite();
        else
            button.GetComponent<UIButtonImageStateController>().SetDisabledSprite();
    }
    #endregion

    #endregion
}
