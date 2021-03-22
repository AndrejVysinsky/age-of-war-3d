﻿using UnityEngine;

public class BaseGameController : MonoBehaviour
{
    [SerializeField] protected FactionEnum faction;
    [SerializeField] protected Material factionMaterial;
    [SerializeField] protected LineController lineController;
    [SerializeField] protected UnitSpawner unitSpawner;
    [SerializeField] protected GoldController goldController;
    [SerializeField] protected Outpost outpost;

    protected Line _activeLine;

    public FactionEnum Faction => faction;

    protected virtual void Start()
    {
        var outpostData = outpost.Initialize(faction);

        unitSpawner.OnQueueCapacityChanged(outpostData.MaxQueueCapacity);

        goldController.Initialize(faction);
    }

    public void SpawnUnit(int unitIndex)
    {
        int cost = unitSpawner.SpawnUnit(unitIndex, _activeLine, faction, factionMaterial, goldController.GetBalance());
        goldController.RemoveBalance(cost);
    }

    public void SpawnMinerUnit()
    {
        unitSpawner.SpawnMinerUnit(4, faction, factionMaterial);
    }

    public void UpgradeUnit(int unitIndex)
    {
        int cost = unitSpawner.UpgradeUnit(unitIndex, goldController.GetBalance());
        goldController.RemoveBalance(cost);
    }

    public void UpgradeOutpost()
    {
        int cost = outpost.UpgradeOutpost(goldController.GetBalance(), out OutpostData outpostData);
        goldController.RemoveBalance(cost);

        if (outpostData != null)
        {
            unitSpawner.OnQueueCapacityChanged(outpostData.MaxQueueCapacity);
        }
    }
}