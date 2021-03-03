using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] FactionEnum faction;
    [SerializeField] LineController lineController;
    [SerializeField] UnitSpawner unitSpawner;

    private Line _activeLine;

    private void Start()
    {
        _activeLine = lineController.GetFirstActiveLine();
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
        }
    }

    public void SpawnUnit(GameObject unitPrefab)
    {
        unitSpawner.SpawnUnit(unitPrefab, _activeLine, faction);
    }
}