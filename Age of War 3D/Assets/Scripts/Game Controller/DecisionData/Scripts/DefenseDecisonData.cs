using UnityEngine;

[CreateAssetMenu(fileName = "Defense Decision Data", menuName = "AI/Defense Decision Data")]
public class DefenseDecisonData : ScriptableObject
{
    [SerializeField] float aiBaseHealthFactor;
    [SerializeField] float health_dmgRatioFactor;
    [SerializeField] float currentAttackFactor;

    public float AiBaseHealthFactor => aiBaseHealthFactor;
    public float Health_dmgRatioFactor => health_dmgRatioFactor;

    public float CurrentAttackFactor => currentAttackFactor;

}
