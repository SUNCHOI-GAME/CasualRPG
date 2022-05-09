using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITargetPointerController : MonoBehaviour
{
    #region [var]
    
    #region [01. Pointerイメージ]
    /// <summary>
    /// カメラ移動時のポインター
    /// </summary>
    [SerializeField]
    private Image cameraPointer;
    #endregion

    #endregion


    #region [func]

    #region [01. Pointer表示]
    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    public void SetCameraPointerAvtivation(bool state)
    {
        this.cameraPointer.enabled = state;
    }
    #endregion

    #endregion
}
