using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] FactionEnum faction;
    [SerializeField] Material factionMaterial;
    [SerializeField] LineController lineController;
    [SerializeField] UnitSpawner unitSpawner;
    [SerializeField] GoldController goldController;

    private Line _activeLine;

    private void Start()
    {
        _activeLine = lineController.GetFirstActiveLine();
        lineController.HightlightLine(_activeLine);
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

    private void TryToSwitchActiveLine(int index)
    {
        if (lineController.IsLineActive(index))
        {
            _activeLine = lineController.GetLineByIndex(index);
            lineController.HightlightLine(_activeLine);
        }
    }

    public void SpawnUnit(int unitIndex)
    {
        // TODO : Get price from unit spawner and check if there is enough balance

        int cost = unitSpawner.SpawnUnit(unitIndex, _activeLine, faction, factionMaterial, goldController.GetBalance());
        goldController.RemoveBalance(cost);
    }

    public void UpgradeUnit(int unitIndex)
    {
        //TODO: check for price

        unitSpawner.UpgradeUnit(unitIndex);
    }
}