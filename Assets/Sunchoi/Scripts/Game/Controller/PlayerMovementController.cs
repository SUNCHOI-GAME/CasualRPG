using System;
using System.Collections;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    #region [var]

    #region [01. ボタン押下判定関連]
    // PlayerMovementState
    [SerializeField]
    private PlayerStateManager.PlayerMovementState movementState;
     
    /// <summary>
    /// 移動スピード
    /// </summary>
    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private float sneakingSpeed = .5f;
    [SerializeField]
    private float walkingSpeed = 1.5f;
    [SerializeField]
    private float runningSpeed = 3f;

    /// <summary>
    /// Pointer座標
    /// </summary>
    [SerializeField]
    private Transform pointer;
    #endregion
    
    #endregion

    #region [func]

    #region [00. コンストラクタ] 
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public void ActivePlayerMovement()
    {
        // 移動速度初期化
        this.moveSpeed = walkingSpeed;
        // PlayerMovementStateをセット
        PlayerStateManager.SetPlayerMovementState(PlayerStateManager.PlayerMovementState.Walking);
        // 現在のPlayerMovementState表示を更新
        this.movementState = PlayerStateManager.MovementState;
        
        // Pointer座標初期化
        this.pointer.position = this.transform.position;
        
        // 移動ボタン入力判定コルーチンの開始
        this.CatchPlayerMovementInputAsync();
        // 移動速度変更ボタン入力判定コルーチンの開始
        this.CatchPlayerChangingSpeedInputAsync();
    }
    #endregion

    #region [01. 移動ボタン入力判定]
    /// <summary>
    /// 移動ボタン入力コルーチンの開始
    /// </summary>
    private void CatchPlayerMovementInputAsync()
    {
        // コルーチンスタート
        GlobalCoroutine.Play(this.CatchPlayerMovementInput(), "CatchPlayerMovementInput", null);
    }

    /// <summary>
    /// 移動ボタン入力判定コルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator CatchPlayerMovementInput()
    {
        Debug.LogFormat($"【Coroutine】  Player Movement Input Activated", DColor.white);
        
        while (true)
        {
            // Player移動
            this.MoveToPoint(this.pointer.position);

            // 入力判定
            if (Vector3.Distance(transform.position, pointer.position) <= .05f)
            {
                // 横移動、縦移動
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    this.pointer.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                }
                if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    this.pointer.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                }
            }
            
           //yield return new WaitForFixedUpdate();
           yield return null;
        }
    }

    /// <summary>
    /// Player移動
    /// </summary>
    /// <param name="point"></param>
    private void MoveToPoint(Vector3 point)
    {
        // 移動
        this.transform.position =
            Vector3.MoveTowards(this.transform.position, point, moveSpeed * Time.deltaTime);
    }
    #endregion


    #region [02. 移動速度変ボタン更入力判定]
    /// <summary>
    /// 移動速度変更ボタン入力コルーチンの開始
    /// </summary>
    private void CatchPlayerChangingSpeedInputAsync()
    {
        // コルーチンスタート
        GlobalCoroutine.Play(this.CatchPlayerChangingSpeedInput(), "CatchPlayerChangingSpeedInput", null);
    }
    
    /// <summary>
    /// 移動速度変更ボタン入力コルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator CatchPlayerChangingSpeedInput() 
    {
        Debug.LogFormat($"【Coroutine】  Player Changing Speed Input Activated", DColor.white);
        
        while (true)
        {
            // Running
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                this.moveSpeed = runningSpeed;
                PlayerStateManager.SetPlayerMovementState(PlayerStateManager.PlayerMovementState.Running);
            }
            if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                this.moveSpeed = walkingSpeed;
                PlayerStateManager.SetPlayerMovementState(PlayerStateManager.PlayerMovementState.Walking);
            }
                
            // Sneaking
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                this.moveSpeed = sneakingSpeed;
                PlayerStateManager.SetPlayerMovementState(PlayerStateManager.PlayerMovementState.Sneaking);
            }
            if(Input.GetKeyUp(KeyCode.LeftControl))
            {
                this.moveSpeed = walkingSpeed;
                PlayerStateManager.SetPlayerMovementState(PlayerStateManager.PlayerMovementState.Walking);
            }

            // 更新
            this.movementState = PlayerStateManager.MovementState;
            
            //yield return new WaitForFixedUpdate();
            yield return null;
        }
    }
    #endregion


    #endregion
}
