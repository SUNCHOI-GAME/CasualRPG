using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapEventManager : MonoBehaviour
{
    #region [var]
    
    /// <summary>
    /// インスタンス
    /// </summary>
    public static MapEventManager Instance { get; private set; }

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



    #region [01. ExitDoor]
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
    
    
    
    #region [02. DoorKey]
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
    
    
    
    #region [03. LootBox]
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
                this.lootBoxMapEventController.SetLootBoxItem();
            }
        }

        onFinished?.Invoke();
    }
    #endregion


    public void DoWhatMapEventDoes(MapEvent targetMapEvent)
    {
        Debug.LogFormat($"this MapEvent is ::: {targetMapEvent.eventName} :::", DColor.cyan);
        
        switch (targetMapEvent.eventID)
        {
            case 0:
                
                break;
            case 1:
                // Open ExitDoor 
                this.SetExitDoorToOpenState();
                break;
            case 2:
                // 
                this.SetItemToInventory();
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
    private void SetItemToInventory()
    {
        
    }
    
    
    #endregion
}
