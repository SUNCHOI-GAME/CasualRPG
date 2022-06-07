using System;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    #region [var]
    /// <summary>
    /// マップを生成すべきドア方向：North
    /// </summary>
    [Header(" --- Map生成 関連")]
    [SerializeField] 
    private bool hasNorthDoor = false;
    public bool HasNorthDoor { get => hasNorthDoor; }
    /// <summary>
    /// マップを生成すべきドア方向：East
    /// </summary>
    [SerializeField] 
    private bool hasEastDoor = false;
    public bool HasEastDoor { get => hasEastDoor; }
    /// <summary>
    /// マップを生成すべきドア方向：South
    /// </summary>
    [SerializeField] 
    private bool hasSouthDoor = false;
    public bool HasSouthDoor { get => hasSouthDoor; }
    /// <summary>
    /// マップを生成すべきドア方向：West
    /// </summary>
    [SerializeField] 
    private bool hasWestDoor = false;
    public bool HasWestDoor { get => hasWestDoor; }
    /// <summary>
    /// Map単体が持つmapCollectNum
    /// </summary>
    [SerializeField]
    private int mapCollectNum = 0;
    public int MapCollectNum { get => mapCollectNum; }
    /// <summary>
    /// Map単体が持つドアの数（不変）
    /// </summary>
    [SerializeField]
    private int mapDoorCount = 0;
    public int MapDoorCount { get => mapDoorCount; }
    /// <summary>
    /// Map単体が持つドアの残り数（可変）
    /// </summary>
    [SerializeField]
    private int mapLeftDoorCount = 0;
    public int MapLeftDoorCount { get => mapLeftDoorCount; }
    /// <summary>
    /// 次のMap生成が終わったか否かを表すトリガー
    /// </summary>
    [SerializeField]
    private bool haveMapDoneGenerating = false;
    public bool HaveMapDoneGenerating { get => haveMapDoneGenerating; }
    
    /// <summary>
    /// 移動可能方向のトリガー
    /// </summary>
    [Header(" --- Unit移動 関連")]
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
    /// Spawn終了のトリガー
    /// </summary>
    [Header(" --- Player Spawn 関連")]
    [SerializeField]
    private bool isPlayerAlreadySpawned = false;
    public bool IsPlayerAlreadySpawned { get => isPlayerAlreadySpawned; }
    [Header(" --- Enemy Spawn 関連")]
    [SerializeField]
    private bool isEnemyAlreadySpawned = false;
    public bool IsEnemyAlreadySpawned { get => isEnemyAlreadySpawned; }
    
    /// <summary>
    /// MapEventSet終了のトリガー
    /// </summary>
    [Header(" --- Map Event Set 関連")]
    [SerializeField]
    private bool isMapEventSet = false;
    public bool IsMapEventSet { get => isMapEventSet; }
    /// <summary>
    /// MapEventのTransform
    /// </summary>
    [SerializeField]
    private Transform mapEventRoot;
    public Transform MapEventRoot { get => mapEventRoot; }
    /// <summary>
    /// 
    /// </summary>
    /// 
    [Header(" --- Map Event 発生 関連")]
    [SerializeField]
    private bool isMapEventFinished = false;
    public bool IsMapEventFinished { get => isMapEventFinished; }

    [SerializeField]
    private SpriteRenderer mapSpriteRenderer;

    [SerializeField]
    private Sprite mapOpenSprite;

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
        this.mapSpriteRenderer.sprite = this.mapOpenSprite;
    }
    #endregion

}
