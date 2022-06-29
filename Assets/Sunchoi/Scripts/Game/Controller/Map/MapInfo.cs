using System;
using System.Diagnostics;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    #region [var]
    
    [Header(" --- Map生成 関連")]
    /// <summary>
    /// マップを生成すべきドア方向：North
    /// </summary>
    [SerializeField] 
    private bool hasNorthDoor = false;
    public bool HasNorthDoor { get => this.hasNorthDoor; }
    /// <summary>
    /// マップを生成すべきドア方向：East
    /// </summary>
    [SerializeField] 
    private bool hasEastDoor = false;
    public bool HasEastDoor { get => this.hasEastDoor; }
    /// <summary>
    /// マップを生成すべきドア方向：South
    /// </summary>
    [SerializeField] 
    private bool hasSouthDoor = false;
    public bool HasSouthDoor { get => this.hasSouthDoor; }
    /// <summary>
    /// マップを生成すべきドア方向：West
    /// </summary>
    [SerializeField] 
    private bool hasWestDoor = false;
    public bool HasWestDoor { get => this.hasWestDoor; }
    /// <summary>
    /// Map単体が持つmapCollectNum
    /// </summary>
    [SerializeField]
    private int mapCollectNum = 0;
    public int MapCollectNum { get => this.mapCollectNum; }
    /// <summary>
    /// Map単体が持つドアの数（不変）
    /// </summary>
    [SerializeField]
    private int mapDoorCount = 0;
    public int MapDoorCount { get => this.mapDoorCount; }
    /// <summary>
    /// Map単体が持つドアの残り数（可変）
    /// </summary>
    [SerializeField]
    private int mapLeftDoorCount = 0;
    public int MapLeftDoorCount { get => this.mapLeftDoorCount; }
    /// <summary>
    /// 次のMap生成が終わったか否かを表すトリガー
    /// </summary>
    [SerializeField]
    private bool haveMapDoneGenerating = false;
    public bool HaveMapDoneGenerating { get => this.haveMapDoneGenerating; }
    
    [Header(" --- Unit移動 関連")]
    /// <summary>
    /// 移動可能方向のトリガー
    /// </summary>
    [SerializeField] 
    private bool canMoveToNorth = false;
    public bool CanMoveToNorth { get => this.canMoveToNorth; }   
    [SerializeField] 
    private bool canMoveToEast = false;
    public bool CanMoveToEast { get => this.canMoveToEast; }   
    [SerializeField] 
    private bool canMoveToSouth = false;
    public bool CanMoveToSouth { get => this.canMoveToSouth; }   
    [SerializeField] 
    private bool canMoveToWest = false;
    public bool CanMoveToWest { get => this.canMoveToWest; }

    /// <summary>
    /// Spawn終了のトリガー
    /// </summary>
    [Header(" --- Player Spawn 関連")]
    private bool isPlayerAlreadySpawned = false;
    public bool IsPlayerAlreadySpawned { get => this.isPlayerAlreadySpawned; }
    [Header(" --- Enemy Spawn 関連")]
    private bool isEnemyAlreadySpawned = false;
    public bool IsEnemyAlreadySpawned { get => this.isEnemyAlreadySpawned; }
    
    [Header(" --- Map Event Set 関連")]
    /// <summary>
    /// MapEventSet終了のトリガー
    /// </summary>
    private bool isMapEventSet = false;
    public bool IsMapEventSet { get => this.isMapEventSet; }
    /// <summary>
    /// MapEventのTransform
    /// </summary>
    [SerializeField]
    private Transform mapEventRoot;
    public Transform MapEventRoot { get => this.mapEventRoot; }
    /// <summary>
    /// MapEventController
    /// </summary>
    private MapEventController mapEventController;
    public MapEventController MapEventController { get => this.mapEventController; }
    
    [Header(" --- Map Event 発生 関連")]
    /// <summary>
    /// MapEvent終了トリガー
    /// </summary>
    private bool isMapEventFinished = false;
    public bool IsMapEventFinished { get => this.isMapEventFinished; }
    /// <summary>
    /// MapのSpriteRenderer
    /// </summary>
    [SerializeField]
    private SpriteRenderer mapSpriteRenderer;
    
    [Header(" --- Map 表示 関連")]
    /// <summary>
    /// Map到達時表示されるSprite
    /// </summary>
    [SerializeField]
    private Sprite mapOpenSprite;
    /// <summary>
    /// Mapの表示切り替えトリガー
    /// </summary>
    private bool isMapOpened = false;
    public bool IsMapOpened { get => this.isMapOpened; }
    /// <summary>
    /// Map通路の影Sprite
    /// </summary>
    [SerializeField]
    private GameObject mapCorridorSprite_N;
    public GameObject MapCorridorSprite_N { get => this.mapCorridorSprite_N; }   
    [SerializeField]
    private GameObject mapCorridorSprite_E;
    public GameObject MapCorridorSprite_E { get => this.mapCorridorSprite_E; }   
    [SerializeField]
    private GameObject mapCorridorSprite_S;
    public GameObject MapCorridorSprite_S { get => this.mapCorridorSprite_S; }   
    [SerializeField]
    private GameObject mapCorridorSprite_W;
    public GameObject MapCorridorSprite_W { get => this.mapCorridorSprite_W; }   

    #endregion


    #region [func]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Start()
    {
        // Unit移動のための移動可能方向をセット
        this.canMoveToNorth = this.hasNorthDoor;
        this.canMoveToEast = this.hasEastDoor;
        this.canMoveToSouth = this.hasSouthDoor;
        this.canMoveToWest = this.hasWestDoor;
    }

    /// <summary>
    /// Map生成終了ステータスをトリガーで保存
    /// </summary>
    public void SetGeneratingDone()
    {
        this.haveMapDoneGenerating = true;
    }

    /// <summary>
    /// マップを生成すべきドアの数を更新
    /// </summary>
    public void SetLeftDoorCountDown()
    {
        this.mapLeftDoorCount -= 1;
    }

    /// <summary>
    /// マップを生成すべきドア方向のステータスを更新
    /// </summary>
    public void SetNorthDoorStatusFalse()
    {
        this.hasNorthDoor = false;
    }
    public void SetEastDoorStatusFalse()
    {
        this.hasEastDoor = false;
    }
    public void SetSouthDoorStatusFalse()
    {
        this.hasSouthDoor = false;
    }
    public void SetWestDoorStatusFalse()
    {
        this.hasWestDoor = false;
    }

    /// <summary>
    /// SpawnおよびSetting終了トリガー発動
    /// </summary>
    public void SetPlayerSpawnTriggerOn()
    {
        this.isPlayerAlreadySpawned = true;
    }
    public void SetEnemySpawnTriggerOn()
    {
        this.isEnemyAlreadySpawned = true;
    }
    public void SetMapEventSettingTriggerOn()
    {
        this.isMapEventSet = true;
    }

    /// <summary>
    /// セット済みMapEventのMapEventControllerを保存
    /// </summary>
    /// <param name="targetMapEventController"></param>
    public void SetMapEventController(MapEventController targetMapEventController)
    {
        this.mapEventController = targetMapEventController;
    }

    /// <summary>
    /// Map名の未松にPlayerSpawnやセットされたEvent名を追加
    /// </summary>
    /// <param name="eventName"></param>
    public void SetEventNameOnMapName(string eventName)
    {
        var thisTransform = this.transform;
        thisTransform.name = thisTransform.name + "_" + eventName;
    }
    
    /// <summary>
    /// MapEvent消化トリガーをセット
    /// </summary>
    public void SetMapEventFinishedTriggerOn()
    {
        this.isMapEventFinished = true;
    }

    /// <summary>
    /// MapEventを消化したMapをOpenStateに変更
    /// </summary>
    public void SetMapSpriteToOpenState()
    {
        this.isMapOpened = true;
        // Mapの表示画像をOpenStateの画像に変更
        this.mapSpriteRenderer.sprite = this.mapOpenSprite;

        // 通路の影表示を更新
        this.SetCorridorShadow();
    }

    /// <summary>
    /// MapEvent終了後、MapEvent画像を終了Stateの画像に変更
    /// </summary>
    public void SetMapEventToFinishedState()
    {
        if(this.mapEventController != null)
            this.mapEventController.SetSpriteToFinishedSprite();
    }
    

    /// <summary>
    /// 通路の影表示を更新
    /// </summary>
    private void SetCorridorShadow()
    {
        // Mapの間隔
        var nextPosX = GridManager.Instance.NextPosXInterval;
        var nextPosY = GridManager.Instance.NextPosYInterval;
        
        // 出口方向のMapを検索し、ターゲットMapの状態によって通路影の表示を変更
        if (this.canMoveToNorth)
            this.FindNeighbor(new Vector3(0, nextPosY, 0), this.mapCorridorSprite_N, "South");
        
        if (this.canMoveToEast)
            FindNeighbor(new Vector3(nextPosX, 0f, 0f), this.mapCorridorSprite_E, "West");
        
        if (this.canMoveToSouth)
            this.FindNeighbor(new Vector3(0, -nextPosY, 0), this.mapCorridorSprite_S, "North");
        
        if (this.canMoveToWest)
            this.FindNeighbor(new Vector3(-nextPosX, 0, 0), this.mapCorridorSprite_W, "East");
    }
    
    /// <summary>
    /// ターゲットMapの状態によって通路影の表示を変更
    /// </summary>
    /// <param name="AddictionalPos"></param>
    /// <param name="corridorSpriteObj"></param>
    private void FindNeighbor(Vector3 AddictionalPos, GameObject corridorSpriteObj, string oppositeDirection)
    {
        // ターゲットとなるMapの座標
        var neighborTransformPosition = this.transform.localPosition + AddictionalPos;
        
        // 座標が一致するMapを検索
        foreach (var map in MapCollector.Instance.collectedMapList)
        {
            if (map.transform.localPosition == neighborTransformPosition)
            {
                // MapInfo
                var mapInfo = map.GetComponent<MapInfo>();
                
                GameObject oppositeCorridorSpriteObj = null;
                switch (oppositeDirection.ToUpper())
                {
                    case "NORTH":
                        oppositeCorridorSpriteObj = mapInfo.MapCorridorSprite_N;
                        break;
                    case "EAST":
                        oppositeCorridorSpriteObj = mapInfo.MapCorridorSprite_E;
                        break;
                    case "SOUTH":
                        oppositeCorridorSpriteObj = mapInfo.MapCorridorSprite_S;
                        break;
                    case "WEST":
                        oppositeCorridorSpriteObj = mapInfo.MapCorridorSprite_W;
                        break;
                }
                
                // ターゲットMapが既にOpenStateだった場合
                if (mapInfo.IsMapOpened)
                {
                    // ターゲットMap側の影を非表示
                    mapInfo.SetCorridorState(oppositeCorridorSpriteObj, false);
                }
                // ターゲットMapが既にOpenStateでない場合
                else
                {
                    // 現在Map側の影を表示
                    this.SetCorridorState(corridorSpriteObj, true);
                    continue;
                }
            }
        }
    }

    /// <summary>
    /// Map通路の影表示切り替え
    /// </summary>
    /// <param name="corridorSpriteObj"></param>
    /// <param name="state"></param>
    public void SetCorridorState(GameObject corridorSpriteObj, bool state)
    {
        corridorSpriteObj.SetActive(state);
    }
    #endregion

}
