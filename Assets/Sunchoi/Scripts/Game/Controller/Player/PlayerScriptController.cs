using UnityEngine;

public class PlayerScriptController : MonoBehaviour
{
    #region [01. コンストラクタ]

    #region [var]

    /// <summary>
    /// インスタンス
    /// </summary>
    public static PlayerScriptController Instance { get; private set; }

    #endregion


    #region [func]

    private void Start()
    {
        // インスタンス
        Instance = this;
    }

    #endregion

    #endregion
    
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
}
