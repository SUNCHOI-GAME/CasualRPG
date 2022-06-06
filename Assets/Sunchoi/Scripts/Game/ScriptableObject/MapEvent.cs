using UnityEngine;

[CreateAssetMenu(fileName ="New MapEvent", menuName = "MapEvent/Create New MapEvent")]
public class MapEvent : ScriptableObject
{
    /// <summary>
    /// EventID
    /// </summary>
    public int eventID;
    /// <summary>
    /// EventSprite
    /// </summary>
    public Sprite eventSprite_Start;
    public Sprite eventSprite_Open;
    public Sprite eventSprite_Change;
    public Sprite eventSprite_Finished;
    /// <summary>
    /// EventName
    /// </summary>
    public string eventName;
    /// <summary>
    /// EventDescription
    /// </summary>
    [TextArea(10,10)] public string eventDescription;
}
