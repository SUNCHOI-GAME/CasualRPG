using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    #region [01.コンストラクタ]

    #region [var]

    

    #endregion
    
    
    #region [func]

    private void Start()
    {
        
    }

    #endregion

    #endregion
    
    
    
    #region [02.Spawn Enemy]

    #region [var]

    

    #endregion
    
    
    #region [func]

    private void SpawnEnemy(Action onFinished)
    {
        
        
        onFinished?.Invoke();
    }

    #endregion

    #endregion
    
    
    
    #region [03.Spawn Object]

    #region [var]

    

    #endregion
    
    
    #region [func]

    private void SpawnObject(Action onFinished)
    {
        
        
        onFinished?.Invoke();
    }

    #endregion

    #endregion
    
    
    
    #region [01.Spawn Enemy]

    #region [var]

    

    #endregion
    
    
    #region [func]

    private void SpawnLootBox(Action onFinished)
    {
        
        
        onFinished?.Invoke();
    }

    #endregion

    #endregion
}
