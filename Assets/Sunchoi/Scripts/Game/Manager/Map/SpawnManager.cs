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
            var randomNum = Random.Range(0, collectedMapList.Count);
        
            var mapInfo = collectedMapList[randomNum].GetComponent<MapInfo>();
            if (!mapInfo.IsAlreadySpawned)
            {
                // Enemyを生成
                EnemyManager.Instance.SetEnemyOnMap(mapInfo.transform.position);
                // 生成済みトリガー
                mapInfo.SetSpawnTriggerOn();
            }
        }

        onFinished?.Invoke();
    }

    #endregion

    #endregion
    
    
    
    #region [03.Spawn Object]

    #region [var]

    

    #endregion
    
    
    #region [func]

    public void SpawnObject(Action onFinished)
    {
        
        
        onFinished?.Invoke();
    }

    #endregion

    #endregion
    
    
    
    #region [01.Spawn Enemy]

    #region [var]

    

    #endregion
    
    
    #region [func]

    public void SpawnLootBox(Action onFinished)
    {
        
        
        onFinished?.Invoke();
    }

    #endregion

    #endregion
}
