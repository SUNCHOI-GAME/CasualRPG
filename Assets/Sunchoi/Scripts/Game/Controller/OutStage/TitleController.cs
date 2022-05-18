using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    #region [01. コンストラクタ]

    #region [var]
    /// <summary>
    /// インスタンス
    /// </summary>
    public static TitleController Instance { get; private set; }
    #endregion
    
    
    #region [func]

    private void Start()
    {
        // インスタンス
        Instance = this;
    }

    #endregion

    #endregion
    
    
    
    #region [02. UI表示]

    #region [var]
    /// <summary>
    /// BackgroundのGameObject
    /// </summary>
    [SerializeField]
    private GameObject background;
    /// <summary>
    /// TitleMainのGameObject
    /// </summary>
    [SerializeField]
    private GameObject title;
    #endregion
    
    
    #region [func]
    /// <summary>
    /// Title表示
    /// </summary>
    public void SetTitle()
    {
        this.background.SetActive(true);
        this.title.SetActive(true);
    }
    #endregion

    #endregion
    
    
    
    #region [03. ボタン押下時]

    #region [var]
    
    #endregion
    
    
    #region [func]
    /// <summary>
    /// Startボタン押下時の処理
    /// </summary>
    public void OnClickStartButton()
    {
        // Map自動生成シーケンス
        GameManager.Instance.MapGeneratingSequence();
        
        // Map生成終了
        MapGeneratingManager.Instance.MapGeneratingFinished(() =>
        {
            GameManager.Instance.SpawnSequence();
            
            this.background.SetActive(false);
            this.title.SetActive(false);
        });
    }
    #endregion

    #endregion

    
    
    
}
