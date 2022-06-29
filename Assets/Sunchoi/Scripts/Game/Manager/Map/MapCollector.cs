using System.Collections.Generic;
using UnityEngine;

public class MapCollector : MonoBehaviour
{
    #region [var]

    #region [01. instance]
    /// <summary>
    /// インスタンス
    /// </summary>
    public static MapCollector Instance { get; set; }
    #endregion
    
    #region [02. map data]
    /// <summary>
    /// 生成したMapのリスト
    /// </summary>
    public List<GameObject> collectedMapList = new List<GameObject>();
    /// <summary>
    /// 現在のMapCollectNum
    /// </summary>
    public int currentTotalMapCollectNum = 0;
    public int CurrentTotalMapCollectNum
    {
        get => currentTotalMapCollectNum;
    }
    #endregion
    
    #region [03. temporary saving info]
    /// <summary>
    /// 座標検索でマッチしたマップのGameObjectを一時的に保存
    /// </summary>
    private GameObject tempMatchedMap = null;
    public GameObject TempMatchedMap
    {
        get => tempMatchedMap;
    }
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
    /// リストに生成したMapを追加
    /// </summary>
    /// <param name="mapObj"></param>
    public void AddMapToList(GameObject mapObj)
    {
        collectedMapList.Add(mapObj);
    }

    /// <summary>
    /// リスト上のMapとPositionが重複するかどうかをチェック
    /// </summary>
    /// <param name="targetMapPos"></param>
    /// <returns></returns>
    public bool CheckMapPosWithList(Vector2 targetMapPos)
    {
        var isMatched = false;
        
        foreach (GameObject mapGameObject in collectedMapList)
        {
            if (mapGameObject.transform.position == new Vector3(targetMapPos.x, targetMapPos.y, 0))
            {
                isMatched = true;
                // 該当するMapのGameObjectを保存
                this.SaveMatchedMap(mapGameObject);
            }
        }

        return isMatched;
    }

    /// <summary>
    /// 座標検索でマッチしたGameObjectをTempMatchedMap保存
    /// </summary>
    /// <param name="matchedMap"></param>
    private void SaveMatchedMap(GameObject matchedMap)
    {
        this.tempMatchedMap = matchedMap;
    }

    /// <summary>
    /// データ初期化
    /// </summary>
    public void ResetData()
    {
        // 臨時保存用のGameOcject
        this.tempMatchedMap = null;
        // 現在のMapCollectNum
        this.currentTotalMapCollectNum = 0;
        // 生成済みマップの破棄
        for (int num = 0; num < this.collectedMapList.Count; num++)
        {
            var targetMap = this.collectedMapList[num];
            Destroy(targetMap);
        }
        // リスト 
        this.collectedMapList.Clear();
    }

    #endregion
}
