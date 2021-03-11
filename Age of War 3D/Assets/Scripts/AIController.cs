using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] FactionEnum faction;
    [SerializeField] Material factionMaterial;
    [SerializeField] LineController lineController;
    [SerializeField] UnitSpawner unitSpawner;
    [SerializeField] GoldController goldController;

    private Line _activeLine;

    private void Start()
    {
        StartCoroutine(SpawnUnit());
    }

    private IEnumerator SpawnUnit()
    {
        while (true)
        {
            int activeLines = lineController.GetNumberOfActiveLines();

            //max is exclusive
            int randomLineIndex = Random.Range(0, activeLines);
            int randomUnitIndex = Random.Range(0, unitSpawner.NumberOfDifferentUnits);

            _activeLine = lineController.GetLineByIndex(randomLineIndex);

            SpawnUnit(randomUnitIndex);

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SpawnUnit(int unitIndex)
    {
        int cost = unitSpawner.SpawnUnit(unitIndex, _activeLine, faction, factionMaterial, goldController.GetBalance());
        goldController.RemoveBalance(cost);
    }

    public void UpgradeUnit(int unitIndex)
    {
        //TODO: check for price

        unitSpawner.UpgradeUnit(unitIndex);
    }
}