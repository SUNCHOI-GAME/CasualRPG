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
    
    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        this.eventSprite.sprite = this.mapEvent.eventSprite_Start;
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetSpriteToFinishedSprite()
    {
        this.eventSprite.sprite = mapEvent.eventSprite_Finished;
        var color = this.eventSprite.color;
        this.eventSprite.color = new Color(color.r, color.g, color.b, this.eventSpriteFinishedAlpha);
    }
}
