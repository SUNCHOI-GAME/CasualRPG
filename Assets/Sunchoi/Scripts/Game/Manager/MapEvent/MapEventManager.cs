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
    /// 
    /// </summary>
    [SerializeField]
    private GameObject exitDoorPrefab;
    public GameObject ExitDoorPrefab { get => this.exitDoorPrefab; }
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private GameObject doorKeyPrefab;
    public GameObject DoorKeyPrefab { get => this.doorKeyPrefab; }
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private GameObject lootBox_CommonPrefab;
    public GameObject LootBox_CommonPrefab { get => this.lootBox_CommonPrefab; }
    #endregion

    
    
    #region [func]
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        // インスタンス
        Instance = this;
        // 破棄不可
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="onFinished"></param>
    public void SetEvent(Action onFinished)
    {
        this.SetExitDoor(() =>
        {
            this.SetDoorKey(() =>
            {
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
    /// 
    /// </summary>
    /// <param name="onFinished"></param>
    private void SetExitDoor(Action onFinished)
    {
        // Map選定
        var collectedMapList = MapCollector.Instance.collectedMapList;
        
        for (int num = 0; num < 1; num++)
        {
            var randomNum = Random.Range(0, collectedMapList.Count);
            var mapInfo = collectedMapList[randomNum].GetComponent<MapInfo>();
            
            if (mapInfo.IsPlayerAlreadySpawned)
            {
                num -= 1;
                continue;
            }
            else
            {
                // ExitDoorを生成
                var exitDoorObj = Instantiate(this.exitDoorPrefab, mapInfo.MapEventRoot);
                
                // セット済みトリガー
                mapInfo.SetMapEventSettingTriggerOn();
            }
        }

        onFinished?.Invoke();
    }

    #endregion
    
    
    
    #region [02. DoorKey]
    /// <summary>
    /// 
    /// </summary>
    /// <param name="onFinished"></param>
    private void SetDoorKey(Action onFinished)
    {
        // Map選定
        var collectedMapList = MapCollector.Instance.collectedMapList;
        
        for (int num = 0; num < 1; num++)
        {
            var randomNum = Random.Range(0, collectedMapList.Count);
            var mapInfo = collectedMapList[randomNum].GetComponent<MapInfo>();
            
            if (mapInfo.IsPlayerAlreadySpawned || mapInfo.IsEnemyAlreadySpawned)
            {
                num -= 1;
                continue;
            }
            
            if(!mapInfo.IsPlayerAlreadySpawned && !mapInfo.IsEnemyAlreadySpawned)
            {
                // DoorKeyを生成
                var doorKeyObj = Instantiate(this.doorKeyPrefab, mapInfo.MapEventRoot);
                
                // セット済みトリガー
                mapInfo.SetMapEventSettingTriggerOn();
            }
        }

        onFinished?.Invoke();
    }

    #endregion
    
    
    
    #region [03. LootBox]
    /// <summary>
    /// 
    /// </summary>
    /// <param name="onFinished"></param>
    private void SetLootBox(Action onFinished)
    {
        // Map選定
        var collectedMapList = MapCollector.Instance.collectedMapList;
        
        foreach (var map in collectedMapList)
        {
            var mapInfo = map.GetComponent<MapInfo>();
                
            if (!mapInfo.IsPlayerAlreadySpawned && !mapInfo.IsMapEventSet)
            {
                // LootBoxを生成
                var lootBoxObj = Instantiate(this.lootBox_CommonPrefab, mapInfo.MapEventRoot);
                
                // セット済みトリガー
                mapInfo.SetMapEventSettingTriggerOn();
            }
        }

        onFinished?.Invoke();
    }

    #endregion
    
    #endregion
}
