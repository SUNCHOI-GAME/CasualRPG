using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class EnemyMovementController : MonoBehaviour
{
    #region [var]

    #region [01. 各種数値]
    [Header(" --- 移動スピード")]
    /// <summary>
    /// 敵の移動スピード
    /// </summary>
    [SerializeField]
    private float enemyMoveSpeed = 1f;
    public float EnemyMoveSpeed { get => this.enemyMoveSpeed; }
    
    [Header(" --- オフセット")]
    /// <summary>
    /// 敵の移動距離
    /// </summary>
    [SerializeField]
    private float enemyMoveValueOffset = 5f;
    #endregion

    #region [02. アニメーションパターン]
    [Header(" --- 移動時アニメーションの再生パターン（DOTween）")]
    /// <summary>
    /// 敵の移動時のアニメーションパターン
    /// </summary>
    [SerializeField]
    private Ease enemyMovementEase;
    #endregion

    #region [03. Transform]
    [Header(" --- Transform")]
    /// <summary>
    /// 敵ポインターのTransform
    /// </summary>
    [FormerlySerializedAs("pointerForPlayer")]
    [SerializeField]
    private Transform pointerTransformForEnemy;
    #endregion
    
    #endregion

    
    #region [func]

    #region [01. コンストラクタ] 
    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Start()
    {
        // Pointer座標初期化
        this.pointerTransformForEnemy.position = this.transform.position;
    }
    #endregion

    #region [02. タッチボタン入力時処理]
    /// <summary>
    /// 敵の移動処理
    /// </summary>
    /// <param name="directionStr"></param>
    public void MoveEnemy(string directionStr)
    {
        // 大文字に統一
        var str = directionStr.ToUpper();
        // 敵ポインターの座標を変更
        switch (str)
        {
            case "UP":
                this.pointerTransformForEnemy.position += new Vector3(0f, 1f * this.enemyMoveValueOffset, 0f);
                break;
            case "DOWN":
                this.pointerTransformForEnemy.position += new Vector3(0f, -1f * this.enemyMoveValueOffset, 0f);
                break;
            case "RIGHT":
                this.pointerTransformForEnemy.position += new Vector3(1f * this.enemyMoveValueOffset, 0f, 0f);
                break;
            case "LEFT":
                this.pointerTransformForEnemy.position += new Vector3(-1f * this.enemyMoveValueOffset, 0f, 0f);
                break;
        }
        // 敵の移動アニメーションを再生
        this.transform
            .DOLocalMove(this.pointerTransformForEnemy.position, this.enemyMoveSpeed)
            .SetEase(this.enemyMovementEase);
    }
    #endregion
    #endregion
}
