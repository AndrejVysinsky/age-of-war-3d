﻿using UnityEngine;

public class BaseGameController : MonoBehaviour
{
    [SerializeField] protected FactionEnum faction;
    [SerializeField] protected Material factionMaterial;
    [SerializeField] protected LineController lineController;
    [SerializeField] protected UnitSpawner unitSpawner;
    [SerializeField] protected GoldController goldController;
    [SerializeField] protected Outpost outpost;
    [SerializeField] protected ArrowAbility arrowAbility;

    public Line ActiveLine { get; protected set; }
    public FactionEnum Faction => faction;
    public Outpost Outpost => outpost;

    protected virtual void Start()
    {
        var outpostData = outpost.Initialize(faction);
        arrowAbility.Initialize(faction);

        unitSpawner.OnQueueCapacityChanged(outpostData.MaxQueueCapacity);

        goldController.Initialize(faction);
    }

    /// <returns>Returns true on successful spawn.</returns>
    public bool SpawnUnit(int unitIndex)
    {
        if (unitIndex >= 4)
        {
            return SpawnMinerUnit();
        }

        int cost = unitSpawner.SpawnUnit(unitIndex, ActiveLine, faction, factionMaterial, goldController.GetBalance());
        goldController.RemoveBalance(cost);

        return cost != 0;
    }

    public bool SpawnMinerUnit()
    {
        return unitSpawner.SpawnMinerUnit(4, faction, factionMaterial, goldController.GetBalance());
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

        SpawnMinerUnit();

        if (outpostData != null)
        {
            unitSpawner.OnQueueCapacityChanged(outpostData.MaxQueueCapacity);
        }
    }

    public void UpgradeArrowAbility()
    {
        int cost = arrowAbility.UpgradeArrowAbility(goldController.GetBalance());
        goldController.RemoveBalance(cost);
    }

    public void UseArrowAbility()
    {
        int cost = arrowAbility.UseAbility(goldController.GetBalance());
        goldController.RemoveBalance(cost);
    }
}