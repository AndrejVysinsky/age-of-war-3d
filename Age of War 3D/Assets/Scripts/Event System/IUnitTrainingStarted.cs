using UnityEngine.EventSystems;

public interface IUnitTrainingStarted : IEventSystemHandler
{
    void OnUnitTrainingStarted(int unitIndex, FactionEnum faction);
}