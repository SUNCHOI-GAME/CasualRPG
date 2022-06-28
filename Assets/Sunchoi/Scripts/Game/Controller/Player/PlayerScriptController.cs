using UnityEngine;

public class PlayerScriptController : MonoBehaviour
{
    #region [var]

    #region [00. コンストラクタ]

    /// <summary>
    /// インスタンス
    /// </summary>
    public static PlayerScriptController Instance { get; private set; }

    #endregion

    #region [01. スクリプト]

    /// <summary>
    /// PlayerMovementController
    /// </summary>
    [SerializeField]
    private PlayerMovementController playerMovementController;
    public PlayerMovementController PlayerMovementController { get => this.playerMovementController; }
    /// <summary>
    /// PlayerMovementController
    /// </summary>
    [SerializeField]
    private PlayerColliderController playerColliderController;
    public PlayerColliderController PlayerColliderController { get => this.playerColliderController; }

    #endregion
    
    #endregion


    #region [func]

    #region [00. コンストラクタ]

    private void Start()
    {
        // インスタンス
        Instance = this;
    }

    #endregion
    
    #endregion
}
