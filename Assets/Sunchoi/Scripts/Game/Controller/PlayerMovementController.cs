using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerMovementController : MonoBehaviour
{
    #region [var]

    #region [01. ボタン押下判定関連]
    /// <summary>
    /// 移動スピード
    /// </summary>
    [SerializeField]
    private float moveSpeed = 10f;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private float moveValueOffset = 5f;

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
        // Pointer座標初期化
        this.pointer.position = this.transform.position;
        
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
            this.MoveToPoint(this.pointer.position);

            // 入力判定
            if (Vector3.Distance(transform.position, pointer.position) <= .05f)
            {
                // 横移動、縦移動
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    this.pointer.position += new Vector3(Input.GetAxisRaw("Horizontal") * this.moveValueOffset, 0f, 0f);
                }
                if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    this.pointer.position += new Vector3(0f, Input.GetAxisRaw("Vertical") * this.moveValueOffset, 0f);
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
            Vector3.MoveTowards(this.transform.position, point, moveSpeed * Time.deltaTime);
    }
    #endregion

    #region [02. タッチボタン入力時処理]

    private bool isCameraMovementActivated = false;
    private bool isButtonPressed = false;
    
    [SerializeField, Header("移動アニメーション")]
    private Ease movementEase;
    
    public void OnClickMovementButton(string directionStr)
    {
        var str = directionStr.ToUpper();
        
        if (!this.isCameraMovementActivated)
        {
            if (!this.isButtonPressed)
            {
                this.isButtonPressed = true;
                
                switch (str)
                {
                    case "UP":
                        this.pointer.position += new Vector3(0f, 1f * this.moveValueOffset, 0f);
                        break;
                    case "DOWN":
                        this.pointer.position += new Vector3(0f, -1f * this.moveValueOffset, 0f);
                        break;
                    case "RIGHT":
                        this.pointer.position += new Vector3(1f * this.moveValueOffset, 0f, 0f);
                        break;
                    case "LEFT":
                        this.pointer.position += new Vector3(-1f * this.moveValueOffset, 0f, 0f);
                        break;
                }

                this.transform.DOLocalMove(this.pointer.position, this.moveSpeed).SetEase(this.movementEase);

                DOVirtual.DelayedCall(this.moveSpeed, () => { this.isButtonPressed = false; });
            }
        }
        else
        {
            switch (str)
            {
                case "UP":

                    break;
                case "DOWN":

                    break;
                case "LEFT":

                    break;
                case "RIGHT":

                    break;
            }
        }
    }
    #endregion

    #endregion
}
