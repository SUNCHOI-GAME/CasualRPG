using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region [var]

    #region [01. instance]
    public static GridManager Instance { get; set; }
    #endregion
    
    #region [02. base grid information]
    // マップ座標の間隔
    [SerializeField] 
    private float nextPosXInterval;
    public float NextPosXInterval { get => this.nextPosXInterval; }
    [SerializeField] 
    private float nextPosYInterval;
    public float NextPosYInterval { get => this.nextPosYInterval; }
    #endregion
    
    #endregion
    
    
    #region [func]
    
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public void Start()
    {
        Instance = this;
    }
    
    /// <summary>
    /// 次の座標を計算
    /// </summary>
    /// <param name="prevPos"></param>
    /// <param name="directionNum"></param>
    /// <returns></returns>
    public Vector2 NextGridPos(Vector2 prevPos, int directionNum)
    {
        // Offset値
        var posXOffset = 0f;
        var posYOffset = 0f;
        
        // directionNumによって進行方向分岐（０：North / １：East / ２：South / ３：West）
        switch (directionNum)
        {
            case 0:
                posXOffset = 0f;
                posYOffset = nextPosYInterval;
                break;
            case 1:
                posXOffset = nextPosXInterval;
                posYOffset = 0f;
                break;
            case 2:
                posXOffset = 0f;
                posYOffset = -nextPosXInterval;
                break;
            case 3:
                posXOffset = -nextPosXInterval;
                posYOffset = 0f;
                break;
            default:
                // directionNumが範囲外の場合、エラー表示
                Debug.LogFormat($"ERROR ::: This direction number, {directionNum}, is out of range ::: ", DColor.red);
                break;
        }
        
        // 次の座標
        var nextPos = prevPos + new Vector2(posXOffset, posYOffset);
        
        return nextPos;
    }
    #endregion
}
