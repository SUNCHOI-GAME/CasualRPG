using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEventController : MonoBehaviour
{
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

    [SerializeField]
    private MapEventController exitDoorMapEventController;
    
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
        if (this.mapEvent.name.ToUpper() == "EXITDOOR")
            return;
        
        this.eventSprite.sprite = mapEvent.eventSprite_Finished;
        var color = this.eventSprite.color;
        this.eventSprite.color = new Color(color.r, color.g, color.b, this.eventSpriteFinishedAlpha);
    }
    
    public void SetExitDoorSpriteToFinishedSprite()
    {
        this.eventSprite.sprite = mapEvent.eventSprite_Finished;
    }

    public void SetLootBoxItem()
    {
        Debug.LogFormat("Loot Box Itemの抽選を終了");
    }
}
