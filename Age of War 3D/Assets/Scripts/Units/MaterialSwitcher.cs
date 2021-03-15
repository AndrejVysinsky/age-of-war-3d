using System;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    [Serializable]
    public class MaterialSwitch
    {
        [SerializeField] public MeshRenderer meshRenderer;
        [SerializeField] public int materialIndexToSwitch;
    }

    [SerializeField] List<MaterialSwitch> materialSwitchers;

    public void SwitchMaterials(Material material)
    {
        for (int i = 0; i < materialSwitchers.Count; i++)
        {
            var materials = materialSwitchers[i].meshRenderer.materials;
            materials[materialSwitchers[i].materialIndexToSwitch] = material;
            materialSwitchers[i].meshRenderer.materials = materials;
        }
    }
}
