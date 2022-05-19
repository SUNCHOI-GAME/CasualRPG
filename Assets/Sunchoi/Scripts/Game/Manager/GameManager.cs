using System;
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
        
        // Title表示
        TitleController.Instance.SetTitle(true);
    }
    #endregion
    
    #region [01. Map Generating Sequence]
    /// <summary>
    /// Map自動生成シーケンス
    /// </summary>
    public void MapGeneratingSequence()
    {
        // Map生成開始
        MapGeneratingManager.Instance.StartGenerating(MapGeneratingManager.Instance.WaitForMapGeneratingFinishAsync);
    }
    #endregion
    
    #region [02. Transition Effect]
    /// <summary>
    /// TransitionEffect再生
    /// </summary>
    public void PlayTransitionEffectIn()
    {
        // 再生
        TransitionEffect.Instance.PlayEffectIn(() =>
        {
            // Title非表示
            TitleController.Instance.SetTitle(false);
            
            // Map自動生成シーケンス
            this.MapGeneratingSequence();
        
            // Map生成終了
            MapGeneratingManager.Instance.MapGeneratingFinished(() =>
            {
                this.SpawnSequence();
            
                // TransitionEffect再生
                this.PlayTransitionEffectOut();
            });
        });
    }
    public void PlayTransitionEffectOut()
    {
        // 再生
        TransitionEffect.Instance.PlayEffectOut(() =>
        {
            
        });
    }
    #endregion

    #region [03. Spawn Sequence]
    /// <summary>
    /// 各種GameObjectのSpawnシーケンス
    /// </summary>
    public void SpawnSequence()
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
