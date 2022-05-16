using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region [var]

    #region [01. instance]
    /// <summary>
    /// インスタンス
    /// </summary>
    public static GameManager Instance { get; private set; }
    #endregion
    
    #region [02. reference]
    
    
    #endregion

    #endregion

    
    #region [func]

    #region [00. コンストラクタ]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Awake()
    {
        // FPS制限
        Application.targetFrameRate = 60;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Start()
    {
        // インスタンス
        Instance = this;
        // 破棄不可
        DontDestroyOnLoad(this.gameObject);
        
        // 画面スリープ不可
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        // マップ自動生成シーケンス
        this.MapGeneratingSequence();
    }
    #endregion
    
    #region [01. Map Generating Sequence]
    /// <summary>
    /// マップ自動生成シーケンス
    /// </summary>
    private void MapGeneratingSequence()
    {
        // 開始
        MapGeneratingManager.Instance.StartGenerating(MapGeneratingManager.Instance.WaitForMapGeneratingFinishAsync);
        
        // 終了
        MapGeneratingManager.Instance.MapGeneratingFinished(this.SpawnSequence);
    }
    #endregion

    #region [02. Spawn Sequence]
    /// <summary>
    /// 各種GameObjectのSpawnシーケンス
    /// </summary>
    private void SpawnSequence()
    {
        // PlayerをSpawn
        SpawnManager.Instance.SpawnPlayer(() =>
        {
            // EnemyをSpawn
            SpawnManager.Instance.SpawnEnemy(1, () =>
            {
                // ExitDoorをSpawn
                SpawnManager.Instance.SpawnExitDoor(() =>
                {
                    // LootBoxをSpawn
                    SpawnManager.Instance.SpawnLootBox(1, () =>
                    {
                        // DoorKeyをSpawn
                        SpawnManager.Instance.SpawnDoorKey(() =>
                        {
                        
                        });
                    });
                });
            });
        });
    } 

    #endregion
    
    #endregion
}
