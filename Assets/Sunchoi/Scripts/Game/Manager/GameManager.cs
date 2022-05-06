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

    /// <summary>
    /// MapGenerator
    /// </summary>
    [SerializeField]
    private MapGeneratingManager mapGeneratingManager;

    [SerializeField]
    private PlayerMovementController playerMovementController;

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
        this.mapGeneratingManager.StartGenerating(this.mapGeneratingManager.WaitForMapGeneratingFinishAsync);
        
        // 終了
        this.mapGeneratingManager.MapGeneratingFinished(this.NextSequence);
    }
    #endregion

    #region [02. Next Sequence]
    /// <summary>
    /// TODO :: デバッグ（後にシーケンス内容を変更）
    /// </summary>
    private void NextSequence()
    {
        this.playerMovementController.ActivePlayerMovement();
    } 

    #endregion
    
    #endregion
}
