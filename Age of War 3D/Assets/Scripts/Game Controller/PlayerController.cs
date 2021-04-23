using System;
using UnityEngine;

public class PlayerController : BaseGameController
{
    public ArrowAbility ArrowAbility => arrowAbility;

    protected override void Start()
    {
        base.Start();
        ActiveLine = lineController.GetFirstActiveLine();
        lineController.HightlightLine(ActiveLine);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            HandleSwitchLineInput();
        }
    }

    private void HandleSwitchLineInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TryToSwitchActiveLine(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TryToSwitchActiveLine(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TryToSwitchActiveLine(2);
        }
    }

    public void TryToSwitchActiveLine(int index)
    {
        if (lineController.IsLineActive(index))
        {
            ActiveLine = lineController.GetLineByIndex(index);
            lineController.HightlightLine(ActiveLine);
        }
    }    
}