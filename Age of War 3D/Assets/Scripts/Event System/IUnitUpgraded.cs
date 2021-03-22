using UnityEngine.EventSystems;

public interface IUnitUpgraded : IEventSystemHandler
{
    void OnUnitUpgraded(int upgradedUnitIndex, UnitData upgradedUnitData, FactionEnum faction);
}