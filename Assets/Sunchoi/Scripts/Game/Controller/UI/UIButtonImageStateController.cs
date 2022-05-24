using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonImageStateController : MonoBehaviour
{
    /// <summary>
    /// 選択可能時のButtonSprite
    /// </summary>
    [SerializeField]
    private Sprite enabledSprite;
    /// <summary>
    /// 選択不可時のButtonSprite
    /// </summary>
    [SerializeField]
    private Sprite disabledSprite;

    
    
    /// <summary>
    /// 選択可能時のButtonSpriteをセット
    /// </summary>
    public void SetEnabledSprite()
    {
        this.GetComponent<Image>().sprite = enabledSprite;
    }
    
    /// <summary>
    /// 選択不可時のButtonSpriteをセット
    /// </summary>
    public void SetDisabledSprite()
    {
        this.GetComponent<Image>().sprite = disabledSprite;
    }
}
