using System;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerStatusManager : MonoBehaviour
{
    #region [01. コンストラクタ]

    #region [var]
    /// <summary>
    /// インスタンス
    /// </summary>
    public static PlayerStatusManager Instance { get; private set; }
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
    
    #region [02. Contacing Sources]

    #region [var]
    /// <summary>
    /// トリガー：Item
    /// </summary>
    [Header(" --- Item関連")]
    [SerializeField]
    private bool isSourceItem = false;
    public bool IsSourceItem { get => this.isSourceItem; }
    /// <summary>
    /// 現在接触しているItem
    /// </summary>
    [SerializeField]
    private Item currentContactingItem;
    [SerializeField]
    private Transform currentContactingColliderTransform;
    public Item CurrentContactingItem { get => this.currentContactingItem; }
    /// <summary>
    /// UILogController
    /// </summary>
    [SerializeField]
    private UILogController uILogController;
    /// <summary>
    /// UIButtonController
    /// </summary>
    [SerializeField]
    private UIButtonController uIButtonController;

    #endregion

    #region [func]
    /// <summary>
    /// 現在接触しているItemをセット
    /// </summary>
    /// <param name="item"></param>
    public void SetCurrentContactingItem(Item item, Transform collider, bool state)
    {
        // データセット
        this.currentContactingItem = item;
        this.currentContactingColliderTransform = collider;
        
        // トリガーセット
        this.isSourceItem = state;
        // Interactボタン表示ステートの切り替え
        this.uIButtonController.SetInteractButtonState(state);
    }
    
    /// <summary>
    /// Interact
    /// </summary>
    public void OnClickInteractButton()
    {
        if(this.isSourceItem)
            this.uILogController.SetItemLog(this.currentContactingItem, this.currentContactingColliderTransform);
    }
    #endregion

    #endregion

}
