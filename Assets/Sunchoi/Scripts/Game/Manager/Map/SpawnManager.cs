using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

    private void Start()
    {
        // インスタンス
        Instance = this;
    }

    #endregion
    #endregion
    
    
    
    #region [02.Spawn Player]

    #region [var]
    [Header(" --- Spawn Player 関連")]
    /// <summary>
    /// Player Transform
    /// </summary>
    [SerializeField]
    private Transform playerTransform;
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
    private UILogController uILogController;
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
        var randomNum = Random.Range(0, collectedMapList.Count);
        var mapInfo = collectedMapList[randomNum].GetComponent<MapInfo>();
        
        if (!mapInfo.IsAlreadySpawned)
        {
            // PlayerPrefabを生成
            var playerObj = Instantiate(this.playerPrefab, this.playerTransform);

            var playerScript = playerObj.GetComponent<PlayerScriptController>();
            // Playerの各種基礎データをセット
            playerScript.PlayerMovementController.SetPlayerMovementData(mapInfo.transform.position, playerScript);
            playerScript.PlayerColliderController.SetData(this.uILogController);
            this.uIButtonController.SetUIButton(playerScript.PlayerMovementController);

            // Playerの座標を記録
            this.playerPos = mapInfo.transform.position;
            
            // 生成済みトリガー
            mapInfo.SetSpawnTriggerOn();
        }
        
        onFinished?.Invoke();
    }

    #endregion

    #endregion
    
    
    
    #region [02.Spawn Enemy]

    #region [var]
    [Header(" --- Spawn Enemy 関連")]
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
        var collectedMapList = MapCollector.Instance.collectedMapList;

        for (int num = 0; num < spawnNum; num++)
        {
            // Map選定
            var randomNum = Random.Range(0, collectedMapList.Count);
            var mapInfo = collectedMapList[randomNum].GetComponent<MapInfo>();

            // 条件比較 (⓵何もSpawnされていないMapか否か　/　⓶Playerと一定距離離れているか否か　/　⓷他Enemyと一定距離離れているか否か)
            // ⓵MapのSpawn状況比較
            // ⓵-1 すでに何かをSpawnしているMapの場合、やり直し
            if (mapInfo.IsAlreadySpawned)
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
                        EnemyManager.Instance.SetEnemyOnMap(mapInfo.transform.position);
                        // 生成済みトリガー
                        mapInfo.SetSpawnTriggerOn();
                    }
                }
            }
        }
        
        onFinished?.Invoke();
    }

    #endregion

    #endregion
    
    
    
    #region [03.Spawn ExitDoor]

    #region [var]
    [Header(" --- Spawn ExitDoor 関連")]
    /// <summary>
    /// Object Transform
    /// </summary>
    [SerializeField]
    private Transform objectTransform;
    /// <summary>
    /// ExitDoor Prefab
    /// </summary>
    [SerializeField]
    private GameObject exitDoorPrefab;
    #endregion
    
    
    #region [func]

    public void SpawnExitDoor(Action onFinished)
    {
        // Map選定
        var collectedMapList = MapCollector.Instance.collectedMapList;
        
        for (int num = 0; num < 1; num++)
        {
            var randomNum = Random.Range(0, collectedMapList.Count);
            var mapInfo = collectedMapList[randomNum].GetComponent<MapInfo>();
            
            if (mapInfo.IsAlreadySpawned)
            {
                num -= 1;
                continue;
            }
            else
            {
                // PlayerPrefabを生成
                var exitDoorObj = Instantiate(this.exitDoorPrefab, this.objectTransform);
                exitDoorObj.transform.position = mapInfo.transform.position;
                
                // 生成済みトリガー
                mapInfo.SetSpawnTriggerOn();
            }
        }

        onFinished?.Invoke();
    }

    #endregion

    #endregion
    
    
    
    #region [04.Spawn LootBox]

    #region [var]
    [Header(" --- Spawn LootBox 関連")]
    /// <summary>
    /// Item Transform
    /// </summary>
    [SerializeField]
    private Transform itemTransform;
    /// <summary>
    /// LootBox Prefab
    /// </summary>
    [SerializeField]
    private GameObject lootBoxPrefab;
    #endregion
    
    
    #region [func]

    public void SpawnLootBox(int spawnNum, Action onFinished)
    {
        // Map選定
        var collectedMapList = MapCollector.Instance.collectedMapList;

        for (int num = 0; num < spawnNum; num++)
        {
            var randomNum = Random.Range(0, collectedMapList.Count);
            var mapInfo = collectedMapList[randomNum].GetComponent<MapInfo>();

            if (mapInfo.IsAlreadySpawned)
            {
                num -= 1;
                continue;
            }
            else
            {
                // PlayerPrefabを生成
                var lootBoxObj = Instantiate(this.lootBoxPrefab, this.itemTransform);
                lootBoxObj.transform.position = mapInfo.transform.position;

                // 生成済みトリガー
                mapInfo.SetSpawnTriggerOn();
            }
        }
        
        onFinished?.Invoke();
    }

    #endregion

    #endregion
}
