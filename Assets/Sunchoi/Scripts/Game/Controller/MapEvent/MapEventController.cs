using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEventController : MonoBehaviour
{
    #region [var]

    [Header(" --- Map Event Common")]
    /// <summary>
    /// ScriptableIbject上のMapEvent情報 
    /// </summary>
    [SerializeField]
    private MapEvent mapEvent;
    public MapEvent MapEvent { get => this.mapEvent; }

    /// <summary>
    /// EventPrefabの画像表示
    /// </summary>
    [SerializeField]
    private SpriteRenderer eventSprite;
    /// <summary>
    /// EventFinished時適応するEventSpriteのアルファ値
    /// </summary>
    [SerializeField]
    private float eventSpriteFinishedAlpha = 0.35f;
    
    [Header(" --- Looted Item")]
    /// <summary>
    /// LootBoxから出たアイテム
    /// </summary>
    [SerializeField]
    private Item lootedItem;
    public Item LootedItem { get => this.lootedItem; }

    #endregion


    #region [func]

    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Awake()
    {
        this.eventSprite.sprite = this.mapEvent.eventSprite_Start;
    }

    /// <summary>
    /// Event終了後、表示画像を終了Stateのものに変更
    /// </summary>
    public void SetSpriteToFinishedSprite()
    {
        // MapEventがExitDoorの場合は処理しない
        if (this.mapEvent.name.ToUpper() == "EXITDOOR")
            return;
        
        // 画像および色を変更
        this.eventSprite.sprite = mapEvent.eventSprite_Finished;
        var color = this.eventSprite.color;
        this.eventSprite.color = new Color(color.r, color.g, color.b, this.eventSpriteFinishedAlpha);
    }
    
    /// <summary>
    /// DoorKey取得後、ExitDoorの表示画像を変更
    /// </summary>
    public void SetExitDoorSpriteToFinishedSprite()
    {
        this.eventSprite.sprite = mapEvent.eventSprite_Change;
    }

    /// <summary>
    /// LootBoxから出たアイテムを保存
    /// </summary>
    /// <param name="item"></param>
    public void SetLootedItem(Item item)
    {
        this.lootedItem = item;
    }
    
    /// <summary>
    /// ItemをInventoryListに追加
    /// </summary>
    public void AddLootedItemToInventory()
    {
        // InventoryListに追加
        InventoryManager.Instance.AddList(this.lootedItem);
        // InventoryListの並びを更新
        InventoryManager.Instance.ListItemsOnInventory();
    }

    #endregion
}
