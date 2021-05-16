using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitcher : MonoBehaviour
{
    [Serializable]
    public class ColorSwitch
    {
        [SerializeField] public Renderer meshRenderer;
        [SerializeField] public int materialIndexToColorSwitch;
    }

    [SerializeField] List<ColorSwitch> materialSwitchers;

    public void SwitchColors(Color color)
    {
        for (int i = 0; i < materialSwitchers.Count; i++)
        {
            var materials = materialSwitchers[i].meshRenderer.materials;
            materials[materialSwitchers[i].materialIndexToColorSwitch].color = color;
            materialSwitchers[i].meshRenderer.materials = materials;
        }
    }
}
