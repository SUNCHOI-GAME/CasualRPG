using System.Collections;
using System.Collections.Generic;
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

    #region [04. 移動関連]
    /// <summary>
    /// 移動可能方向のトリガー
    /// </summary>
    [Header(" --- 移動関連")]
    [SerializeField] 
    private bool canMoveToNorth = false;
    public bool CanMoveToNorth { get => canMoveToNorth; }   
    [SerializeField] 
    private bool canMoveToEast = false;
    public bool CanMoveToEast { get => canMoveToEast; }   
    [SerializeField] 
    private bool canMoveToSouth = false;
    public bool CanMoveToSouth { get => canMoveToSouth; }   
    [SerializeField] 
    private bool canMoveToWest = false;
    public bool CanMoveToWest { get => canMoveToWest; }

    /// <summary>
    /// １回目移動不可時の２回目の移動方向を決めるためのリスト
    /// </summary>
    [SerializeField] 
    private List<string> nextDirectionList = new List<string>();
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

        // 移動可能方向をセット
        this.GetMapInfo();
    }
    #endregion

    #region [02. 移動]
    /// <summary>
    /// 次の移動方向をセット
    /// </summary>
    public void SetNextDirection()
    {
        var randomNum = 0;
        randomNum = Random.Range(0, this.nextDirectionList.Count);
        
        this.MoveEnemyToNextDirection(this.nextDirectionList[randomNum]);
    }
    
    /// <summary>
    /// Enemy移動
    /// </summary>
    /// <param name="directionStr"></param>
    public void MoveEnemyToNextDirection(string directionStr)
    {
        // 大文字に統一
        var str = directionStr.ToUpper();

        // 移動しない場合
        if (str == "STAY")
        {
            // TODO :: Stay時のアニメーション制御を追加。

            return;
        }
        
        // 敵ポインターの座標を変更
        switch (str)
        {
            case "UP":
                if(this.canMoveToNorth)
                    this.pointerTransformForEnemy.position += new Vector3(0f, 1f * this.enemyMoveValueOffset, 0f);
                else
                {
                    this.SetNextDirection(); 
                    return;
                }
                break;
            case "DOWN":
                if(this.canMoveToSouth)
                    this.pointerTransformForEnemy.position += new Vector3(0f, -1f * this.enemyMoveValueOffset, 0f);
                else
                {
                    this.SetNextDirection(); 
                    return;
                }
                break;
            case "RIGHT":
                if(this.canMoveToEast)
                    this.pointerTransformForEnemy.position += new Vector3(1f * this.enemyMoveValueOffset, 0f, 0f);
                else
                {
                    this.SetNextDirection(); 
                    return;
                }
                break;
            case "LEFT":
                if(this.canMoveToWest)
                    this.pointerTransformForEnemy.position += new Vector3(-1f * this.enemyMoveValueOffset, 0f, 0f);
                else
                {
                    this.SetNextDirection(); 
                    return;
                }
                break;
        }
        // 敵の移動アニメーションを再生
        this.transform
            .DOLocalMove(this.pointerTransformForEnemy.position, this.enemyMoveSpeed)
            .SetEase(this.enemyMovementEase);
    }
    #endregion
    
    #region [03. 移動方向セット関連]
    /// <summary>
    /// 移動可能方向を検索
    /// </summary>
    public void GetMapInfo()
    {
        // this.nextDirectionList.Add("up");
        // this.nextDirectionList.Add("right");
        // this.nextDirectionList.Add("down");
        // this.nextDirectionList.Add("left");
        // リスト初期化
        this.nextDirectionList.Clear();

        // 現在の敵の座標
        var enemyPos = this.transform.position;
        
        // 生成済みMapリストと比較
        foreach (var map in MapCollector.Instance.collectedMapList)
        {
            if (map.transform.position == enemyPos)
            {
                // 各トリガーをセット
                var info = map.gameObject.GetComponent<MapInfo>();
                this.canMoveToNorth = info.CanMoveToNorth;
                this.canMoveToEast = info.CanMoveToEast;
                this.canMoveToSouth = info.CanMoveToSouth;
                this.canMoveToWest = info.CanMoveToWest;
                
                // 移動可能方向をリストアップ
                if(this.canMoveToNorth)
                    this.nextDirectionList.Add("up");
                if(this.canMoveToEast)
                    this.nextDirectionList.Add("right");
                if(this.canMoveToSouth)
                    this.nextDirectionList.Add("down");
                if(this.canMoveToWest)
                    this.nextDirectionList.Add("left");
                
                // リストに留まる選択肢を追加
                this.nextDirectionList.Add("Stay");
                
                return;
            }
        }
    }
    #endregion
    #endregion
}
