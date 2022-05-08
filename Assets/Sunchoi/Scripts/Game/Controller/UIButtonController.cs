using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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
    private Button hUDActivationToggleButton;
    /// <summary>
    /// プレイヤーの移動ボタン
    /// </summary>
    [SerializeField]
    private Button[] movementButtonsForPlayer;
    /// <summary>
    /// カメラの移動ボタン
    /// </summary>
    [SerializeField]
    private Button[] movementButtonsForCamera;
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
    [Header(" --- Button Objects")]
    /// <summary>
    /// 移動モード切り替えボタンのImage
    /// </summary>
    [SerializeField]
    private Image movementModeToggleButtonImage;
    /// <summary>
    /// HUD表示切り替えボタンのImage
    /// </summary>
    [SerializeField]
    private Image hUDActivationToggleButtonImage;
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

    #endregion
    

    #region [func]

    #region [01. コンストラクタ]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Awake()
    {
        // 移動ボタンの初期化
        this.SetMovementButtonState(this.isButtonForCameraMovement);
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
    #endregion
    
    #region [03. ボタン押下処理]
    /// <summary>
    /// Settingsボタン押下時の処理
    /// </summary>
    public void OnClickSettingsButton()
    {
        // 該当メニューを表示
        this.uIMenuController.SetSettingsMenuActivationState(true);
    }
    
    /// <summary>
    /// Inventoryボタン押下時の処理
    /// </summary>
    public void OnClickInventoryButton()
    {
        // 該当メニューを表示
        this.uIMenuController.SetInventoryMenuActivationState(true);
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
            // ボタンImageの表示切り替え
            // TODO :: アルファ時にSprite切り替えに変更
            this.hUDActivationToggleButtonImage.color = Color.white;
        }
        else
        {
            // トリガーをセット
            this.isHUDActivationOff = true;
            // ボタンImageの表示切り替え
            // TODO :: アルファ時にSprite切り替えに変更
            this.hUDActivationToggleButtonImage.color = Color.green;
        }

        // HUDの表示切り替え
        this.SetHUDActivationState(!this.isHUDActivationOff);
    }
    
    /// <summary>
    /// プレイヤー移動ボタン押下時の処理を中継
    /// </summary>
    /// <param name="directionStr"></param>
    public void OnClickPlayerMovementButton(string directionStr)
    {
        // ボタン押下を一時的に無効化
        this.SetMovementButtonEnableState(this.playerMovementController.PlayerMoveSpeed);
        // 実際の処理
        this.playerMovementController.OnClickPlayerMovementButton(directionStr);
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
        this.movementModeToggleButton.enabled = false;
        foreach (var button in this.movementButtonsForPlayer)　button.enabled = false;
        foreach (var button in this.movementButtonsForCamera)　button.enabled = false;

        // 遅延処理
        DOVirtual.DelayedCall(delay, () =>
        {
            // 各種ボタンコンポネントをEnable
            // TODO :: ボタンImageの表示切り替え処理を追加
            this.settingButton.enabled = true;
            this.inventoryButton.enabled = true;
            this.movementModeToggleButton.enabled = true;
            this.movementModeToggleButton.enabled = true;
            foreach (var button in this.movementButtonsForPlayer)　button.enabled = true;
            foreach (var button in this.movementButtonsForCamera)　button.enabled = true;
        });
    }
    #endregion

    #endregion
    
    
    

    
    
    

    

    

    
}
