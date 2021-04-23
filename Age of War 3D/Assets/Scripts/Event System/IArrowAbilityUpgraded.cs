using UnityEngine.EventSystems;

public interface IArrowAbilityUpgraded : IEventSystemHandler
{
    void OnArrowAbilityUpgraded(ArrowAbilityData arrowAbilityData, FactionEnum faction);
}