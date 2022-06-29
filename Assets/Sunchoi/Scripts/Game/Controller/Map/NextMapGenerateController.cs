using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NextMapGenerateController : MonoBehaviour
{
    #region [var]

    #region [01. reference]

    /// <summary>
    /// Map情報
    /// </summary>
    [SerializeField]
    private MapInfo mapInfo;

    #endregion

    #region [02. map data]

#pragma warning disable 414
    /// <summary>
    /// 確実にドアが存在する場合のトリガー
    /// </summary>
    private bool hasNorthDoor = false;
    private bool hasEastDoor = false;
    private bool hasSouthDoor = false;
    private bool hasWestDoor = false;
    /// <summary>
    /// ドアの存在がどちらでもいい場合のトリガー
    /// </summary>
    private bool doesNorthDoorNotMatter = false;
    private bool doesEastDoorNotMatter = false;
    private bool doesSouthDoorNotMatter = false;
    private bool doesWestDoorNotMatter = false;
#pragma warning restore 414

    /// <summary>
    /// 必ず必要なドア、絶対ないドアを保存するリスト
    /// </summary>
    private List<string> mustHaveDoorDirection = new List<string>();
    private List<string> neverHaveDoorDirection = new List<string>();
    /// <summary>
    /// ドアのコンディションによって篩にかけられたマップを保存するリスト
    /// </summary>
    private List<GameObject> generatingQueueList = new List<GameObject>();
    /// <summary>
    /// 生成対象にならないマップを保存するリスト
    /// </summary>
    private List<GameObject> removingQueueList = new List<GameObject>();
    /// <summary>
    /// 生成終了シーケンスに必要ないマップを保存するリスト
    /// </summary>
    private List<GameObject> addictionalRemovableMapList = new List<GameObject>();
    /// <summary>
    /// マップ生成終了ステータスを表す番号
    /// </summary>
    private int mapGeneratingStatusNum = 0;
    #endregion

    #endregion


    #region [func]

    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Start()
    {
        // データ初期化
        this.InitListsAndVariables();

        if(this.mapInfo == null)
            // PrefabのMapInfoを参照
            this.mapInfo = GetComponent<MapInfo>();
    
        // マップ生成終了ステータス番号を初期更新
        this.mapGeneratingStatusNum = this.mapInfo.MapLeftDoorCount;
        
        // Door方向チェック
        this.CheckDoorDirection();
    }

    /// <summary>
    /// Door方向チェック
    /// </summary>
    private void CheckDoorDirection()
    {
        if (this.mapInfo.HasNorthDoor)
            // Door方向にMapがあるかを判定
            this.CheckSpawnedMapOnNextPos(0);

        if (this.mapInfo.HasEastDoor)
            // Door方向にMapがあるかを判定
            this.CheckSpawnedMapOnNextPos(1);

        if (this.mapInfo.HasSouthDoor)
            // Door方向にMapがあるかを判定
            this.CheckSpawnedMapOnNextPos(2);

        if (this.mapInfo.HasWestDoor)
            // Door方向にMapがあるかを判定
            this.CheckSpawnedMapOnNextPos(3);
    }

    /// <summary>
    /// Door方向にMapがあるかを判定
    /// </summary>
    /// <param name="doorDirection"></param>
    private void CheckSpawnedMapOnNextPos(int doorDirection)
    {
        // 次のMap座標
        var nextMapPos =
            GridManager.Instance.NextGridPos(this.transform.position, doorDirection);

        // 次のMap座標にすでにMapがある場合
        if (MapCollector.Instance.CheckMapPosWithList(nextMapPos))
        {
            // マップ生成のステータスを更新
            this.UpdateMapGeneratingStatus();
            // マップを生成すべきドア方向のステータスを更新
            this.UpdateMapDoorStatus(doorDirection);
        }
        // 次のMap座標にすでにMapがない場合
        else
        {
            // 次のMap座標からNorth方向にMap有無チェック
            this.CheckSpawnedMapOnFuturePos(nextMapPos, 0, () =>
            {
                // East方向にMap有無チェック
                this.CheckSpawnedMapOnFuturePos(nextMapPos, 1, () =>
                {
                    // South方向にMap有無チェック
                    this.CheckSpawnedMapOnFuturePos(nextMapPos, 2, () =>
                    {
                        // West方向にMap有無チェック
                        this.CheckSpawnedMapOnFuturePos(nextMapPos, 3, () =>
                        {
                            // 生成対象のリスト作成後、そのリストの候補でマップを生成
                            this.SetNewList(() => { this.GenerateNextMap(nextMapPos, doorDirection); });
                        });
                    });
                });
            });
        }
    }
    
    /// <summary>
    /// Map有無チェック後、ドアのコンディションを保存
    /// </summary>
    /// <param name="nextPos"></param>
    /// <param name="doorDirection"></param>
    /// <param name="onFinished"></param>
    private void CheckSpawnedMapOnFuturePos(Vector2 nextPos, int doorDirection, Action onFinished)
    {
        // その次のMap座標
        var futureMapPos = GridManager.Instance.NextGridPos(nextPos, doorDirection);

        // 次のMap座標にすでにMapがある場合
        var isFutureMapExist = MapCollector.Instance.CheckMapPosWithList(futureMapPos);
        if (isFutureMapExist)
        {
            // すでに存在するMapの情報を参照
            var existMapInfo = MapCollector.Instance.TempMatchedMap.GetComponent<MapInfo>();
            // ドアのコンディションを保存
            switch (doorDirection)
            {
                case 0:
                    if (existMapInfo.HasSouthDoor)
                        this.SetDoorCondition(this.hasNorthDoor = true, this.doesNorthDoorNotMatter = false, 0);
                    else
                        this.SetDoorCondition(this.hasNorthDoor = false, this.doesNorthDoorNotMatter = false, 0);
                    break;
                case 1:
                    if (existMapInfo.HasWestDoor)
                        this.SetDoorCondition(this.hasEastDoor = true, this.doesEastDoorNotMatter = false, 1);
                    else
                        this.SetDoorCondition(this.hasEastDoor = false, this.doesEastDoorNotMatter = false, 1);
                    break;
                case 2:
                    if (existMapInfo.HasNorthDoor)
                        this.SetDoorCondition(this.hasSouthDoor = true, this.doesSouthDoorNotMatter = false, 2);
                    else
                        this.SetDoorCondition(this.hasSouthDoor = false, this.doesSouthDoorNotMatter = false, 2);
                    break;
                case 3:
                    if (existMapInfo.HasEastDoor)
                        this.SetDoorCondition(this.hasWestDoor = true, this.doesWestDoorNotMatter = false, 3);
                    else
                        this.SetDoorCondition(this.hasWestDoor = false, this.doesWestDoorNotMatter = false, 3);
                    break;
            }
        }
        else
        {
            // ドアのコンディションを保存
            switch (doorDirection)
            {
                case 0:
                    this.SetDoorCondition(this.hasNorthDoor = false, this.doesNorthDoorNotMatter = true, 0);
                    break;
                case 1:
                    this.SetDoorCondition(this.hasEastDoor = false, this.doesEastDoorNotMatter = true, 1);
                    break;
                case 2:
                    this.SetDoorCondition(this.hasSouthDoor = false, this.doesSouthDoorNotMatter = true, 2);
                    break;
                case 3:
                    this.SetDoorCondition(this.hasWestDoor = false, this.doesWestDoorNotMatter = true, 3);
                    break;
            }
        }

        onFinished?.Invoke();
    }
    
    /// <summary>
    /// ドアのコンディションを保存
    /// </summary>
    /// <param name="hasThatDoor"></param>
    /// <param name="thatDoorDoesntMatter"></param>
    /// <param name="doorDirection"></param>
    private void SetDoorCondition(bool hasThatDoor, bool thatDoorDoesntMatter, int doorDirection)
    {
        // ドア方向を示す文字列を用意
        var doorString = "";
        switch (doorDirection)
        {
            case 0:
                doorString = "N";
                break;
            case 1:
                doorString = "E";
                break;
            case 2:
                doorString = "S";
                break;
            case 3:
                doorString = "W";
                break;
        }

        // 絶対必要なドア方向をリストに保存
        if (hasThatDoor)
            this.mustHaveDoorDirection.Add(doorString);
        else
        {
            // あってはならないドア方向をリストに保存
            if (!thatDoorDoesntMatter) this.neverHaveDoorDirection.Add(doorString);
        }
    }

    /// <summary>
    /// 最終的に生成するマップをリストアップ
    /// </summary>
    /// <param name="onFinished"></param>
    private void SetNewList(Action onFinished)
    {
        // デフォルトのマップリストから絶対必要なドアをすべて含むマップのみをリストに保存
        foreach (var map in MapGeneratingManager.Instance.MapList.Where(map => map.name.IndexOf(mustHaveDoorDirection[0]) > 0))
        {
            if (mustHaveDoorDirection.Count > 1)
            {
                if (map.name.IndexOf(mustHaveDoorDirection[1]) > 0)
                {
                    if (mustHaveDoorDirection.Count > 2)
                    {
                        if (map.name.IndexOf(mustHaveDoorDirection[2]) > 0)
                        {
                            if (mustHaveDoorDirection.Count == 4)
                            {
                                if (map.name.IndexOf(mustHaveDoorDirection[3]) > 0)
                                {
                                    this.generatingQueueList.Add(map);
                                }
                            }
                            else
                                this.generatingQueueList.Add(map);
                        }
                    }
                    else
                        this.generatingQueueList.Add(map);
                }
            }
            else
                this.generatingQueueList.Add(map);
        }

        // あってはならないドアを含むすべてのマップをリストに保存
        for (int num = 0; num < neverHaveDoorDirection.Count; num++)
        {
            foreach (GameObject map
                     in MapGeneratingManager.Instance.MapList.ToArray().Where(map =>
                         map.name.IndexOf(neverHaveDoorDirection[num], StringComparison.Ordinal) > 0))
            {
                if (!this.removingQueueList.Contains(map))
                    this.removingQueueList.Add(map);
            }
        }
        
        // 両リストで被る要素を排除
        foreach (GameObject map in this.removingQueueList)
        {
            if (this.generatingQueueList.Contains(map))
            {
                this.generatingQueueList.Remove(map);
            }
        }

        onFinished?.Invoke();
    }

    /// <summary>
    /// 次のMapを生成
    /// </summary>
    /// <param name="nextMapPos"></param>
    /// <param name="doorDirectionNum"></param>
    private void GenerateNextMap(Vector2 nextMapPos, int doorDirection)
    {
        // currentTotalMapCollectNum　が　MaxTotalMapCollectNum　を超えない範囲でマップを生成
        if (MapGeneratingManager.Instance.MaxTotalMapCollectNum > MapCollector.Instance.currentTotalMapCollectNum)
        {
            // Random Number
            var randomNum = UnityEngine.Random.Range(0, this.generatingQueueList.Count);
            // マップリストからランダムで生成ターゲットを選抜
            var creatTarget = this.generatingQueueList[randomNum];
            // 生成
            var instancedMap = Instantiate(creatTarget, MapGeneratingManager.Instance.mapRoot);
            // 座標決め
            instancedMap.transform.position = nextMapPos;
            // 生成されたマップを管理するリストに追加
            MapCollector.Instance.AddMapToList(instancedMap);
        
            // マップ情報に生成順の番号を記録
            MapCollector.Instance.currentTotalMapCollectNum += this.mapInfo.MapCollectNum;
            
            // リスト初期化
            this.InitListsAndVariables();
            // マップ生成のステータスを更新
            this.UpdateMapGeneratingStatus();
            // マップを生成すべきドア方向のステータスを更新
            this.UpdateMapDoorStatus(doorDirection);
        }
        else
        {
            // 生成待ちマップリストの中から生成終了シーケンスに必要ないマップをリストアップ
            foreach (var map in this.generatingQueueList)
            {
                if (map.transform.name.Length > this.mustHaveDoorDirection.Count + 7)
                    this.addictionalRemovableMapList.Add(map);
            }
            
            // リストアップした項目と生成待ちマップリストで重複するマップをリストから排除
            foreach (var map in this.addictionalRemovableMapList)
            {
                if (this.generatingQueueList.Contains(map))
                    this.generatingQueueList.Remove(map);
            }
            
            // 生成ターゲット（リストに1つのみに残っている状態なので指定番号は０）
            var creatTarget = this.generatingQueueList[0];
            // 生成
            var instancedMap = Instantiate(creatTarget, MapGeneratingManager.Instance.mapRoot);
            // 座標決め
            instancedMap.transform.position = nextMapPos;
            // 生成されたマップを管理するリストに追加
            MapCollector.Instance.AddMapToList(instancedMap);
        
            // マップ情報に生成順の番号を記録
            MapCollector.Instance.currentTotalMapCollectNum += this.mapInfo.MapCollectNum;
            
            // リスト初期化
            this.InitListsAndVariables();
            // マップ生成のステータスを更新
            this.UpdateMapGeneratingStatus();
            // マップを生成すべきドア方向のステータスを更新
            this.UpdateMapDoorStatus(doorDirection);
        }
    }

    /// <summary>
    /// マップ生成後、各種データを初期化
    /// </summary>
    private void InitListsAndVariables()
    {
        this.generatingQueueList.Clear();
        this.removingQueueList.Clear();
        this.mustHaveDoorDirection.Clear();
        this.neverHaveDoorDirection.Clear();
        this.addictionalRemovableMapList.Clear();
        
        this.hasNorthDoor = false;
        this.hasEastDoor = false;
        this.hasSouthDoor = false;
        this.hasWestDoor = false;
        
        this.doesNorthDoorNotMatter = false;
        this.doesEastDoorNotMatter = false;
        this.doesSouthDoorNotMatter = false;
        this.doesWestDoorNotMatter = false;
    }

    /// <summary>
    /// マップ生成のステータスを更新
    /// </summary>
    private void UpdateMapGeneratingStatus()
    {
        // ステータス番号増加
        this.mapGeneratingStatusNum -= 1;
        // マップを生成すべきドアの数を更新
        this.mapInfo.SetLeftDoorCountDown();
        
        // ステータス番号が４になった場合
        if (this.mapGeneratingStatusNum <= 0)
        {
            // MapInfoに記録
            this.mapInfo.SetGeneratingDone();
            // MapGeneratingManagerに記録
            MapGeneratingManager.Instance.AddAllDoorClosedMapCount();
        }
    }

    /// <summary>
    /// マップ生成未完了個所の再生成のため、マップを生成すべきドア方向のステータスを更新
    /// </summary>
    private void UpdateMapDoorStatus(int doorDirection)
    {
        switch (doorDirection)
        {
            case 0:
                this.mapInfo.SetNorthDoorStatusFalse();
                break;
            case 1:
                this.mapInfo.SetEastDoorStatusFalse();
                break;
            case 2:
                this.mapInfo.SetSouthDoorStatusFalse();
                break;
            case 3: 
                this.mapInfo.SetWestDoorStatusFalse();
                break;
        }
    }
    #endregion
}
