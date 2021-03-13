﻿using UnityEngine;

public class BaseGameController : MonoBehaviour
{
    [SerializeField] protected FactionEnum faction;
    [SerializeField] protected Material factionMaterial;
    [SerializeField] protected LineController lineController;
    [SerializeField] protected UnitSpawner unitSpawner;
    [SerializeField] protected GoldController goldController;

    protected Line _activeLine;

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