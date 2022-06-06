using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    private void Awake()
    {
        this.eventSprite.sprite = this.mapEvent.eventSprite_Start;
    }
}
