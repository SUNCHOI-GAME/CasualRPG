
public static class PlayerStateManager
{
    #region [01. Movement State]

    #region [var]
    /// <summary>
    /// プレイヤーの移動ステートの種類
    /// </summary>
    public enum PlayerMovementState
    {
        Walking = 0,
        Sneaking = 1,
        Running = 2
    }
    /// <summary>
    /// プレイヤーの移動ステート
    /// </summary>
    private static PlayerMovementState movementState;
    public static PlayerMovementState MovementState { get => movementState; }
    #endregion

    #region [func]
    /// <summary>
    /// プレイヤーの移動ステートをセット
    /// </summary>
    /// <param name="state"></param>
    public static void SetPlayerMovementState(PlayerMovementState state)
    {
        movementState = state;
    }
    #endregion
    
    #endregion
}
