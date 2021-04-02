using UnityEngine;

[CreateAssetMenu(fileName = "Attack Decision Data", menuName = "AI/Attack Decision Data")]
public class AttackDecisionData : ScriptableObject
{
    [SerializeField] float playerLowCashFactor;
    [SerializeField] float aiCashFactor;
    [SerializeField] float unitsTrainedFactor;
    [SerializeField] float minerSpawnedFactor;
    [SerializeField] float health_dmgRatioFactor;

    public float PlayerLowCashFactor => playerLowCashFactor;
    public float AICashFactor => aiCashFactor;
    public float UnitsTrainedFactor => unitsTrainedFactor;
    public float MinerSpawnedFactor => minerSpawnedFactor;
    public float Health_dmgRatioFactor => health_dmgRatioFactor;

}
