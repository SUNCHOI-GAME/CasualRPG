using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapEventManager : MonoBehaviour
{
    #region [var]
    
    /// <summary>
    /// インスタンス
    /// </summary>
    public static MapEventManager Instance { get; private set; }

    
    [Header(" --- Setting Events")]
    /// <summary>
    /// ExitDoor Prefab
    /// </summary>
    [SerializeField]
    private GameObject exitDoorPrefab;
    /// <summary>
    /// DoorKey Prefab
    /// </summary>
    [SerializeField]
    private GameObject doorKeyPrefab;
    /// <summary>
    /// LootBox_Common Prefab
    /// </summary>
    [SerializeField]
    private GameObject lootBox_CommonPrefab;

    /// <summary>
    /// ExitDoorOpen関連
    /// </summary>
    private MapEventController exitDoorMapEventController;
    private bool isExitDoorLogShown = false;
    public bool IsExitDoorLogShow { get => this.isExitDoorLogShown; }
    
    private bool isExitDoorOpened= false;
    public bool IsExitDoorOpened { get => this.isExitDoorOpened; }
    
    /// <summary>
    /// ExitDoorOpen関連
    /// </summary>
    private MapEventController lootBoxMapEventController;
    #endregion
    
    
    #region [func]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Start()
    {
        // インスタンス
        Instance = this;
        // 破棄不可
        DontDestroyOnLoad(this.gameObject);
    }

    

    #region [01. Setting Events]

    /// <summary>
    /// Map上にMapEventを生成
    /// </summary>
    /// <param name="onFinished"></param>
    public void SetEvent(Action onFinished)
    {
        // ExitDoorを生成
        this.SetExitDoor(() =>
        {
            // DoorKeyを生成
            this.SetDoorKey(() =>
            {
                // LootBox_Commonを生成
                this.SetLootBox(() =>
                {
                    onFinished?.Invoke();
                });
                
                //onFinished?.Invoke();
            });
        });
    }



    #region [001. SetExitDoor]
    /// <summary>
    /// ExitDoorを生成
    /// </summary>
    /// <param name="onFinished"></param>
    private void SetExitDoor(Action onFinished)
    {
        // Map選定
        var collectedMapList = MapCollector.Instance.collectedMapList;
        
        for (int num = 0; num < 1; num++)
        {
            // Map選定
            var randomNum = Random.Range(0, collectedMapList.Count);
            var mapInfo = collectedMapList[randomNum].GetComponent<MapInfo>();
            
            // PlayerがSpawnしているMapか、既にMapEventが生成されたMapだった場合、やり直し
            if (mapInfo.IsPlayerAlreadySpawned || mapInfo.IsMapEventSet)
            {
                num -= 1;
                continue;
            }
            
            // PlayerがSpawnされていない、且、MapEventが生成されていない場合
            if(!mapInfo.IsPlayerAlreadySpawned && !mapInfo.IsMapEventSet)
            {
                // ExitDoorを生成
                var exitDoorObj = Instantiate(this.exitDoorPrefab, mapInfo.MapEventRoot);
                // ExitDoorのMapEventControllerを個別に記録
                this.exitDoorMapEventController = exitDoorObj.GetComponent<MapEventController>();
                
                // セット済みトリガー
                mapInfo.SetMapEventSettingTriggerOn();
                // MapをOpenStateに変更
                mapInfo.SetMapSpriteToOpenState();
                // MapEventControllerをセット
                mapInfo.SetMapEventController(this.exitDoorMapEventController);
                // MapのGameObject名の後ろにEvent名を追加
                mapInfo.SetEventNameOnMapName("ExitDoor");

                // IndicatorのToTagetとして登録
                UITargetIndicatorController.Instance.SetToTarget(mapInfo.transform);
            }
        }

        onFinished?.Invoke();
    }

    #endregion
    
    
    
    #region [002. SetDoorKey]
    /// <summary>
    /// DoorKeyを生成
    /// </summary>
    /// <param name="onFinished"></param>
    private void SetDoorKey(Action onFinished)
    {
        // Map選定
        var collectedMapList = MapCollector.Instance.collectedMapList;
        
        for (int num = 0; num < 1; num++)
        {
            // Map選定
            var randomNum = Random.Range(0, collectedMapList.Count);
            // 該当MapのMapInfo
            var mapInfo = collectedMapList[randomNum].GetComponent<MapInfo>();
            
            // PlayerがSpawnしているMapか、既にMapEventが生成されたMapだった場合、やり直し
            if (mapInfo.IsPlayerAlreadySpawned || mapInfo.IsMapEventSet)
            {
                num -= 1;
                continue;
            }
            
            // PlayerがSpawnされていない、且、MapEventが生成されていない場合
            if(!mapInfo.IsPlayerAlreadySpawned && !mapInfo.IsMapEventSet)
            {
                // DoorKeyを生成
                var doorKeyObj = Instantiate(this.doorKeyPrefab, mapInfo.MapEventRoot);
                
                // セット済みトリガー
                mapInfo.SetMapEventSettingTriggerOn();
                // MapEventControllerをセット
                mapInfo.SetMapEventController(doorKeyObj.GetComponent<MapEventController>());
                // MapのGameObject名の後ろにEvent名を追加
                mapInfo.SetEventNameOnMapName("DoorKey");
            }
        }

        onFinished?.Invoke();
    }

    #endregion
    
    
    
    #region [003. SetLootBox]
    /// <summary>
    /// LootBox_Commonを生成
    /// </summary>
    /// <param name="onFinished"></param>
    private void SetLootBox(Action onFinished)
    {
        // 全Map対象
        foreach (var map in MapCollector.Instance.collectedMapList)
        {
            // 該当MapのMapInfo
            var mapInfo = map.GetComponent<MapInfo>();
            
            // PlayerがSpawnされていない、且、MapEventが生成されていない場合
            if (!mapInfo.IsPlayerAlreadySpawned && !mapInfo.IsMapEventSet)
            {
                // LootBoxを生成
                var lootBoxObj = Instantiate(this.lootBox_CommonPrefab, mapInfo.MapEventRoot);
                // LootBoxのMapEventControllerを個別に記録
                this.lootBoxMapEventController = lootBoxObj.GetComponent<MapEventController>();
                
                // セット済みトリガー
                mapInfo.SetMapEventSettingTriggerOn();
                // MapEventControllerをセット
                mapInfo.SetMapEventController(this.lootBoxMapEventController);
                // MapのGameObject名の後ろにEvent名を追加
                mapInfo.SetEventNameOnMapName("LootBox");
                
                // LootBoxから出るItemを前もって抽選
                this.lootBoxMapEventController.SetLootBoxItem(this.LootingItem());
            }
        }

        onFinished?.Invoke();
    }
    #endregion

    #endregion



    #region [02. Looting Item]
    [Header(" --- Looting Item")]
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private List<Item> commonItemList = new List<Item>();
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private List<Item> epicItemList = new List<Item>();
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private List<Item> legendItemList = new List<Item>();

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private float commonRate = 0f;
    [SerializeField]
    private float epicRate = 0f;
    [SerializeField]
    private float legendRate = 0f;

    /// <summary>
    /// 
    /// </summary>
    private Item LootingItem()
    {
        Item item = null;

        // 
        this.legendRate = 100f - (this.commonRate + this.epicRate);
        
        // 
        float randomLotNum = UnityEngine.Random.Range(0, 100);
        if (randomLotNum <= this.commonRate)
        {
            int randomItemNum = UnityEngine.Random.Range(0, this.commonItemList.Count);
            item = this.commonItemList[randomItemNum];
        }
        else if (this.commonRate < randomLotNum && randomLotNum <= this.commonRate + this.epicRate)
        {
            int randomItemNum = UnityEngine.Random.Range(0, this.epicItemList.Count);
            item = this.epicItemList[randomItemNum];
        }
        else if (this.commonRate + this.epicRate < randomLotNum && randomLotNum <= 100f)
        {
            int randomItemNum = UnityEngine.Random.Range(0, this.legendItemList.Count);
            item = this.legendItemList[randomItemNum];
        }
        
        return item;
    }

    #endregion
    
    

    #region [03. Event Execution]
    public void DoWhatMapEventDoes(MapEvent targetMapEvent, MapEventController targetMapEventController)
    {
        Debug.LogFormat($"this MapEvent is ::: {targetMapEvent.eventName} :::", DColor.cyan);
        
        switch (targetMapEvent.eventID)
        {
            case 0:
                
                break;
            case 1:
                // Open ExitDoor 
                this.SetExitDoorToOpenState();
                // DoorKey Count +1 
                PlayerStatusManager.Instance.IncreaseDoorKeyCount();
                break;
            case 2:
                // 
                this.SetItemToInventory(targetMapEventController);
                break;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    private void SetExitDoorToOpenState()
    {
        this.SetExitDoorLogBoolState(true);
        this.SetExitDoorBoolState(true);
        this.exitDoorMapEventController.SetExitDoorSpriteToFinishedSprite();
        
        // TODO:: ExitDoorのEvent実行を有効化し、該当マップ到着時StageClear処理を開始
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    public void SetExitDoorLogBoolState(bool state)
    {
        this.isExitDoorLogShown = state;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    public void SetExitDoorBoolState(bool state)
    {
        this.isExitDoorOpened = state;
    }
    
    /// <summary>
    /// 
    /// </summary>
    private void SetItemToInventory(MapEventController targetMapEventController)
    {
        bool isInventoryMax = 
            PlayerStatusManager.Instance.MaxInventoryCount - PlayerStatusManager.Instance.CurrentInventoryCount == 0;

        if (isInventoryMax)
        {
            
        }
        else
        {
            
            // 
            targetMapEventController.AddLootedItemToInventory();
            
            
        }
    }
    #endregion
    
    
    
    #endregion
}
