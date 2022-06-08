using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region [01.コンストラクタ]
    #region [var]
    /// <summary>
    /// インスタンス
    /// </summary>
    public static SpawnManager Instance { get; private set; }
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
    #endregion
    #endregion
    
    
    
    #region [02.Spawn Player]

    #region [var]
    [Header(" --- Spawn Player 関連")]
    /// <summary>
    /// PlayerRoot Transform
    /// </summary>
    [SerializeField]
    private Transform playerRootTransform;
    /// <summary>
    /// Player Prefab
    /// </summary>
    [SerializeField]
    private GameObject playerPrefab;
    /// <summary>
    /// 各種UIController
    /// </summary>
    [SerializeField]
    private UIButtonController uIButtonController;
    [SerializeField]
    private UIDialogController uIDialogController;
    /// <summary>
    /// Playerの座標
    /// </summary>
    private Vector3 playerPos;
    #endregion
    
    
    #region [func]
    /// <summary>
    /// Spawn Player
    /// </summary>
    /// <param name="onFinished"></param>
    public void SpawnPlayer(Action onFinished)
    {
        // Map選定
        var collectedMapList = MapCollector.Instance.collectedMapList;
        var randomNum = UnityEngine.Random.Range(0, collectedMapList.Count);
        var mapInfo = collectedMapList[randomNum].GetComponent<MapInfo>();
        
        if (!mapInfo.IsEnemyAlreadySpawned)
        {
            // Playerを生成
            var playerObj = Instantiate(this.playerPrefab, this.playerRootTransform);
            //  PlayerScriptControllerを参照
            var playerScript = playerObj.GetComponent<PlayerScriptController>();
            // Playerの各種基礎データをセット
            playerScript.PlayerMovementController.SetPlayerMovementData(mapInfo.transform.position, playerScript);
            playerScript.PlayerColliderController.SetData(this.uIDialogController);
            this.uIButtonController.SetUIButton(playerScript.PlayerMovementController);

            // Playerの座標を記録
            this.playerPos = mapInfo.transform.position;
            
            // 生成済みトリガー
            mapInfo.SetPlayerSpawnTriggerOn();
            // MapをOpenStateに変更
            mapInfo.SetMapSpriteToOpenState();
            // MapEventが発生しないようにEvent終了トリガーをセット
            mapInfo.SetMapEventFinishedTriggerOn();
            // MapのGameObject名の後ろにEvent名を追加
            mapInfo.SetEventNameOnMapName("PlayerStartPoint");
        }
        
        onFinished?.Invoke();
    }

    #endregion

    #endregion
    
    
    
    #region [02.Spawn Enemy]

    #region [var]
    [Header(" --- Spawn Enemy 関連")]
    /// <summary>
    /// EnemyRoot Transform
    /// </summary>
    [SerializeField]
    private Transform enemyRootTransform;
    /// <summary>
    /// Enemy Prefab
    /// </summary>
    [SerializeField]
    private GameObject enemyPrafab;
    /// <summary>
    /// 各種距離Offset
    /// </summary>
    [SerializeField]
    private float minDistanceToPlayer = 0f;
    [SerializeField]
    private float minDistanceToOtherEnemy = 0f;
    #endregion
    
    
    #region [func]
    /// <summary>
    /// Spawn Enemy
    /// </summary>
    /// <param name="onFinished"></param>
    public void SpawnEnemy(int spawnNum, Action onFinished)
    {
        // 生成されたMapのリスト
        var collectedMapList = MapCollector.Instance.collectedMapList;

        for (int num = 0; num < spawnNum; num++)
        {
            // Map選定
            var randomNum = UnityEngine.Random.Range(0, collectedMapList.Count);
            var mapInfo = collectedMapList[randomNum].GetComponent<MapInfo>();

            // Playerが既にマップにSpawnされていれば、やり直し
            if(mapInfo.IsPlayerAlreadySpawned)
            {
                num -= 1;
                continue;
            }
            
            // 条件比較 (⓵何もSpawnされていないMapか否か　/　⓶Playerと一定距離離れているか否か　/　⓷他Enemyと一定距離離れているか否か)
            // ⓵MapのSpawn状況比較
            // ⓵-1 すでに何かをSpawnしているMapの場合、やり直し
            if (mapInfo.IsEnemyAlreadySpawned)
            {
                num -= 1;
                continue;
            }
            // ⓵-2 Mapに何もSpawnされていない場合、次の比較に移行
            else
            {
                // ⓶Playerとの距離の長さ比較
                // Playerとの距離取得
                var heading = this.playerPos - mapInfo.transform.position;
                var distance = heading.magnitude;
                // ⓶-1 一定距離以内Playerが存在している場合、やり直し
                if (distance < this.minDistanceToPlayer)
                {
                    num -= 1;
                    continue;
                }
                // ⓶-2 Playerと一定の距離を離れている場合、次の比較に移行
                else if (distance >= this.minDistanceToPlayer)
                {
                    // ⓷ 他Enemyとの距離を比較
                    int enemyCount = EnemyManager.Instance.EnemyMovementControllerList.Count;
                    if (enemyCount > 0)
                    {
                        foreach (var enemyMovementController in EnemyManager.Instance.EnemyMovementControllerList)
                        {
                            var enemyHeading = enemyMovementController.transform.position - mapInfo.transform.position;
                            var enemyDistance = enemyHeading.magnitude;

                            if (enemyDistance > this.minDistanceToOtherEnemy)
                                enemyCount -= 1;
                        }
                    }

                    // ⓷-1 一定の距離以内に他Enemyが一個でも存在している場合、やり直し
                    if (enemyCount != 0)
                    {
                        num -= 1;
                        continue;
                    }
                    // ⓷-2 一定の距離以内に他Enemyがいない場合、Enemyを生成
                    else
                    {
                        // Enemyを生成
                        EnemyManager.Instance.SetEnemyOnMap(this.enemyPrafab, this.enemyRootTransform, mapInfo.transform.position);
                        // 生成済みトリガー
                        mapInfo.SetEnemySpawnTriggerOn();
                    }
                }
            }
        }
        
        onFinished?.Invoke();
    }

    #endregion

    #endregion
    // #region [03.Spawn ExitDoor]
    //
    // #region [var]
    // [Header(" --- Spawn ExitDoor 関連")]
    // /// <summary>
    // /// ObjectRoot Transform
    // /// </summary>
    // [SerializeField]
    // private Transform objectRootTransform;
    // /// <summary>
    // /// ExitDoor Prefab
    // /// </summary>
    // [SerializeField]
    // private GameObject exitDoorPrefab;
    // #endregion
    //
    //
    // #region [func]
    //
    // public void SpawnExitDoor(Action onFinished)
    // {
    //     // Map選定
    //     var collectedMapList = MapCollector.Instance.collectedMapList;
    //     
    //     for (int num = 0; num < 1; num++)
    //     {
    //         var randomNum = Random.Range(0, collectedMapList.Count);
    //         var mapInfo = collectedMapList[randomNum].GetComponent<MapInfo>();
    //         
    //         if (mapInfo.IsEnemyAlreadySpawned)
    //         {
    //             num -= 1;
    //             continue;
    //         }
    //         else
    //         {
    //             // ExitDoorを生成
    //             var exitDoorObj = Instantiate(this.exitDoorPrefab, this.objectRootTransform);
    //             exitDoorObj.transform.position = mapInfo.transform.position;
    //             
    //             // 生成済みトリガー
    //             mapInfo.SetEnemySpawnTriggerOn();
    //             // MapEventControllerをセット
    //             mapInfo.SetMapEventController(exitDoorObj.GetComponent<MapEventController>());
    //         }
    //     }
    //
    //     onFinished?.Invoke();
    // }
    //
    // #endregion
    //
    // #endregion
    //
    //
    //
    // #region [04.Spawn LootBox]
    //
    // #region [var]
    // [Header(" --- Spawn LootBox 関連")]
    // /// <summary>
    // /// ItemRoot Transform
    // /// </summary>
    // [SerializeField]
    // private Transform itemRootTransform;
    // /// <summary>
    // /// LootBox Prefab
    // /// </summary>
    // [SerializeField]
    // private GameObject lootBoxPrefab;
    // #endregion
    //
    //
    // #region [func]
    //
    // public void SpawnLootBox(int spawnNum, Action onFinished)
    // {
    //     // Map選定
    //     var collectedMapList = MapCollector.Instance.collectedMapList;
    //
    //     for (int num = 0; num < spawnNum; num++)
    //     {
    //         var randomNum = Random.Range(0, collectedMapList.Count);
    //         var mapInfo = collectedMapList[randomNum].GetComponent<MapInfo>();
    //
    //         if (mapInfo.IsEnemyAlreadySpawned)
    //         {
    //             num -= 1;
    //             continue;
    //         }
    //         else
    //         {
    //             // LootBoxを生成
    //             var lootBoxObj = Instantiate(this.lootBoxPrefab, this.objectRootTransform);
    //             lootBoxObj.transform.position = mapInfo.transform.position;
    //
    //             // 生成済みトリガー
    //             mapInfo.SetEnemySpawnTriggerOn();
    //             // MapEventControllerをセット
    //             mapInfo.SetMapEventController(lootBoxObj.GetComponent<MapEventController>());
    //         }
    //     }
    //     
    //     onFinished?.Invoke();
    // }
    //
    // #endregion
    //
    // #endregion
    //
    //
    //
    // #region [04.Spawn Key]
    //
    // #region [var]
    // [Header(" --- Spawn DoorKey 関連")]
    // /// <summary>
    // /// DoorKey Prefab
    // /// </summary>
    // [SerializeField]
    // private GameObject doorKeyPrefab;
    // #endregion
    //
    //
    // #region [func]
    //
    // public void SpawnDoorKey(Action onFinished)
    // {
    //     // Map選定
    //     var collectedMapList = MapCollector.Instance.collectedMapList;
    //
    //     for (int num = 0; num < 1; num++)
    //     {
    //         var randomNum = Random.Range(0, collectedMapList.Count);
    //         var mapInfo = collectedMapList[randomNum].GetComponent<MapInfo>();
    //
    //         if (mapInfo.IsEnemyAlreadySpawned)
    //         {
    //             num -= 1;
    //             continue;
    //         }
    //         else
    //         {
    //             // DoorKeyを生成
    //             var doorKeyObj = Instantiate(this.doorKeyPrefab, this.itemRootTransform);
    //             doorKeyObj.transform.position = mapInfo.transform.position;
    //             
    //             // 生成済みトリガー
    //             mapInfo.SetEnemySpawnTriggerOn();
    //             // MapEventControllerをセット
    //             mapInfo.SetMapEventController(doorKeyObj.GetComponent<MapEventController>());
    //         }
    //     }
    //     
    //     onFinished?.Invoke();
    // }
    //
    // #endregion
    //
    //#endregion
}
