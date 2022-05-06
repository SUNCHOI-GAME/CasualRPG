using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    #region [var]
    /// <summary>
    /// マップを生成すべきドア方向：North
    /// </summary>
    [SerializeField] 
    private bool hasNorthDoor = false;
    public bool HasNorthDoor
    {
        get => hasNorthDoor;
    }
    /// <summary>
    /// マップを生成すべきドア方向：East
    /// </summary>
    [SerializeField] 
    private bool hasEastDoor = false;
    public bool HasEastDoor
    {
        get => hasEastDoor;
    }
    /// <summary>
    /// マップを生成すべきドア方向：South
    /// </summary>
    [SerializeField] 
    private bool hasSouthDoor = false;
    public bool HasSouthDoor
    {
        get => hasSouthDoor;
    }
    /// <summary>
    /// マップを生成すべきドア方向：West
    /// </summary>
    [SerializeField] 
    private bool hasWestDoor = false;
    public bool HasWestDoor
    {
        get => hasWestDoor;
    }

    /// <summary>
    /// Map単体が持つmapCollectNum
    /// </summary>
    [SerializeField]
    private int mapCollectNum = 0;
    public int MapCollectNum
    {
        get => mapCollectNum;
    }
    
    /// <summary>
    /// Map単体が持つドアの数（不変）
    /// </summary>
    [SerializeField]
    private int mapDoorCount = 0;
    public int MapDoorCount
    {
        get => mapDoorCount;
    }
    
    /// <summary>
    /// Map単体が持つドアの残り数（可変）
    /// </summary>
    [SerializeField]
    private int mapLeftDoorCount = 0;
    public int MapLeftDoorCount
    {
        get => mapLeftDoorCount;
    }
    
    /// <summary>
    /// 次のMap生成が終わったか否かを表すトリガー
    /// </summary>
    [SerializeField]
    private bool haveMapDoneGenerating = false;
    public bool HaveMapDoneGenerating
    {
        get => haveMapDoneGenerating;
    }

    #endregion


    #region [func]
    /// <summary>
    /// Map生成終了ステータスをトリガーで保存
    /// </summary>
    public void SetGeneratingDone()
    {
        this.haveMapDoneGenerating = true;
    }

    /// <summary>
    /// マップを生成すべきドアの数を更新
    /// </summary>
    public void SetLeftDoorCountDown()
    {
        this.mapLeftDoorCount -= 1;
    }

    /// <summary>
    /// マップを生成すべきドア方向のステータスを更新
    /// </summary>
    public void SetNorthDoorStatusFalse()
    {
        this.hasNorthDoor = false;
    }
    public void SetEastDoorStatusFalse()
    {
        this.hasEastDoor = false;
    }
    public void SetSouthDoorStatusFalse()
    {
        this.hasSouthDoor = false;
    }
    public void SetWestDoorStatusFalse()
    {
        this.hasWestDoor = false;
    }
    #endregion

}
