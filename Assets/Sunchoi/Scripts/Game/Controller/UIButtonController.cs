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
    #endregion

    #region [02. ボタン]

    [Header(" --- Buttons")]
    /// <summary>
    /// プレイヤーの移動ボタン
    /// </summary>
    [SerializeField]
    private Button settingButton;
    /// <summary>
    /// プレイヤーの移動ボタン
    /// </summary>
    [SerializeField]
    private Button inventoryButton;
    /// <summary>
    /// プレイヤーの移動ボタン
    /// </summary>
    [SerializeField]
    private Button toggleButton;
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
    /// 移動ボタン表示切り替え用ToggleボタンのGameObject
    /// </summary>
    [SerializeField]
    private GameObject buttonStateToggleButton;
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

    #region [04. トリガー]
    /// <summary>
    /// 移動ボタン表示の切り替えトリガー
    /// </summary>
    private bool isButtonForCameraMovement = false;
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
        this.SetMovementButton(this.isButtonForCameraMovement);
    }
    #endregion
    
    #region [02. 移動ボタンのステート管理]
    /// <summary>
    /// 移動ボタンの表示切り替え
    /// </summary>
    /// <param name="state"></param>
    public void SetMovementButton(bool state)
    {
        // カメラ移動ボタンの表示ステート
        this.movementButtonObjForCamera.SetActive(state);
        // プレイヤー移動ボタンの表示ステート
        this.movementButtonObjForPlayer.SetActive(!state);
        
        // カメラ座標のリセット
        this.playerMovementController.ResetCameraPosition();
    }
    #endregion
    
    #region [03. ボタン押下処理]
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
            this.buttonStateToggleButton.GetComponent<Image>().color = Color.white;
        }
        else
        {
            // トリガーをセット
            this.isButtonForCameraMovement = true;
            // ボタンImageの表示切り替え
            // TODO :: アルファ時にSprite切り替えに変更
            this.buttonStateToggleButton.GetComponent<Image>().color = Color.green;
        }
        
        // 移動ボタンの表示切り替え
        this.SetMovementButton(this.isButtonForCameraMovement);
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
        this.toggleButton.enabled = false;
        foreach (var button in this.movementButtonsForPlayer)　button.enabled = false;
        foreach (var button in this.movementButtonsForCamera)　button.enabled = false;

        // 遅延処理
        DOVirtual.DelayedCall(delay, () =>
        {
            // 各種ボタンコンポネントをEnable
            // TODO :: ボタンImageの表示切り替え処理を追加
            this.settingButton.enabled = true;
            this.inventoryButton.enabled = true;
            this.toggleButton.enabled = true;
            foreach (var button in this.movementButtonsForPlayer)　button.enabled = true;
            foreach (var button in this.movementButtonsForCamera)　button.enabled = true;
        });
    }
    #endregion

    #endregion
    
    
    

    
    
    

    

    

    
}
