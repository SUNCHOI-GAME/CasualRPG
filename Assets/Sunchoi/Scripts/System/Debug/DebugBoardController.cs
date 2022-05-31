using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBoardController : MonoBehaviour
{
    private bool isDebugButtonGroupShown= false; 
    [SerializeField]
    private GameObject debugBoardButtonGroup;


    private void Start()
    {
        this.debugBoardButtonGroup.SetActive(false);
    }

    public void OnClickDebugBoardButton()
    {
        if (!this.isDebugButtonGroupShown)
        {
            this.isDebugButtonGroupShown = true;   
            this.debugBoardButtonGroup.SetActive(true);
        }
        else
        {
            this.isDebugButtonGroupShown = false;   
            this.debugBoardButtonGroup.SetActive(false);
        }
    }

    public void OnClickTimescaleSettingButton(int value)
    {
        Time.timeScale = value;
    }
}
