using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class PlayerMovementController : MonoBehaviour
{
    #region [var]

    #region [01. 各種数値]
    [Header(" --- 移動スピード")]
    /// <summary>
    /// プレイヤーの移動スピード
    /// </summary>
    [SerializeField]
    private float playerMoveSpeed = 1f;
    public float PlayerMoveSpeed { get => this.playerMoveSpeed; }
    /// <summary>
    /// カメラの移動スピード
    /// </summary>
    [SerializeField]
    private float cameraMoveSpeed = 0.3f;
    public float CameraMoveSpeed { get => this.cameraMoveSpeed; }
    
    [Header(" --- オフセット")]
    /// <summary>
    /// プレイヤーの移動距離
    /// </summary>
    [SerializeField]
    private float playerMoveValueOffset = 5f;
    /// <summary>
    /// カメラの移動距離
    /// </summary>
    [SerializeField]
    private float cameraMoveValueOffset = 10f;
    #endregion

    #region [02. アニメーションパターン]
    [Header(" --- 移動時アニメーションの再生パターン（DOTween）")]
    /// <summary>
    /// プレイヤーの移動時のアニメーションパターン
    /// </summary>
    [SerializeField]
    private Ease playerMovementEase;
    /// <summary>
    /// カメラ移動時のアニメーションパターン
    /// </summary>
    [SerializeField]
    private Ease cameraMovementEase;
    #endregion

    #region [03. Transform]
    [Header(" --- Transform")]
    /// <summary>
    /// カメラのTransform
    /// </summary>
    [SerializeField]
    private Transform cameraTransform;
    /// <summary>
    /// プレイヤーポインターのTransform
    /// </summary>
    [FormerlySerializedAs("pointerForPlayer")]
    [SerializeField]
    private Transform pointerTransformForPlayer;
    /// <summary>
    /// カメラポインターのTransform
    /// </summary>
    [FormerlySerializedAs("pointeTransformForCamera")]
    [FormerlySerializedAs("pointeForCamera")]
    [SerializeField]
    private Transform pointerTransformForCamera;
    #endregion
    
    #endregion

    
    #region [func]

    #region [00. コンストラクタ] 
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public void ActivePlayerMovement()
    {
        // Pointer座標初期化
        this.pointerTransformForPlayer.position = this.transform.position;
        
        // 移動ボタン入力判定コルーチンの開始
        // this.CatchPlayerMovementInputAsync();
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
            this.MoveToPoint(this.pointerTransformForPlayer.position);

            // 入力判定
            if (Vector3.Distance(transform.position, pointerTransformForPlayer.position) <= .05f)
            {
                // 横移動、縦移動
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    this.pointerTransformForPlayer.position += new Vector3(Input.GetAxisRaw("Horizontal") * this.playerMoveValueOffset, 0f, 0f);
                }
                if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    this.pointerTransformForPlayer.position += new Vector3(0f, Input.GetAxisRaw("Vertical") * this.playerMoveValueOffset, 0f);
                }
            }
            
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
            Vector3.MoveTowards(this.transform.position, point, playerMoveSpeed * Time.deltaTime);
    }
    #endregion

    #region [02. タッチボタン入力時処理]
    /// <summary>
    /// プレイヤの移動処理
    /// </summary>
    /// <param name="directionStr"></param>
    public void OnClickPlayerMovementButton(string directionStr)
    {
        // 大文字に統一
        var str = directionStr.ToUpper();
        // プレイヤーポインターの座標を変更
        switch (str)
        {
            case "UP":
                this.pointerTransformForPlayer.position += new Vector3(0f, 1f * this.playerMoveValueOffset, 0f);
                break;
            case "DOWN":
                this.pointerTransformForPlayer.position += new Vector3(0f, -1f * this.playerMoveValueOffset, 0f);
                break;
            case "RIGHT":
                this.pointerTransformForPlayer.position += new Vector3(1f * this.playerMoveValueOffset, 0f, 0f);
                break;
            case "LEFT":
                this.pointerTransformForPlayer.position += new Vector3(-1f * this.playerMoveValueOffset, 0f, 0f);
                break;
        }
        // カメラポインターを同期
        this.pointerTransformForCamera.position = this.pointerTransformForPlayer.position;
        // プレイヤーの移動アニメーションを再生
        this.transform
            .DOLocalMove(this.pointerTransformForPlayer.position, this.playerMoveSpeed)
            .SetEase(this.playerMovementEase);
    }

    /// <summary>
    /// カメラの移動処理
    /// </summary>
    /// <param name="directionStr"></param>
    public void OnClickCameraMovementButton(string directionStr)
    {
        // 大文字に統一
        var str = directionStr.ToUpper();
        // カメラポインターの座標を変更
        switch (str)
        {
            case "UP":
                this.pointerTransformForCamera.position += new Vector3(0f, 1f * this.cameraMoveValueOffset, 0f);
                break;
            case "DOWN":
                this.pointerTransformForCamera.position += new Vector3(0f, -1f * this.cameraMoveValueOffset, 0f);
                break;
            case "RIGHT":
                this.pointerTransformForCamera.position += new Vector3(1f * this.cameraMoveValueOffset, 0f, 0f);
                break;
            case "LEFT":
                this.pointerTransformForCamera.position += new Vector3(-1f * this.cameraMoveValueOffset, 0f, 0f);
                break;
            case "RESET":
                this.pointerTransformForCamera.position = this.transform.position;
                break;
        }

        // カメラの移動アニメーションを再生
        this.cameraTransform
            .DOMove(this.pointerTransformForCamera.position, this.cameraMoveSpeed)
            .SetEase(this.cameraMovementEase);
    }

    /// <summary>
    /// カメラ座標のリセット処理
    /// </summary>
    public void ResetCameraPosition()
    {
        if(this.pointerTransformForCamera.position != this.transform.position)
            this.OnClickCameraMovementButton("RESET");
    }
    #endregion

    #endregion
}
