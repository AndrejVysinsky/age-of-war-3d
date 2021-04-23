using UnityEngine;

[CreateAssetMenu(fileName = "New Arrow Ability Data", menuName = "Abilities/Arrow Ability Data")]
public class ArrowAbilityData : ScriptableObject
{
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] new string name;
    [SerializeField] Sprite sprite;
    [SerializeField] float damage;
    [SerializeField] float cooldown;
    [SerializeField] int numberOfArrows;
    [SerializeField] int upgradePrice;

    public GameObject ArrowPrefab => arrowPrefab;
    public string Name => name;
    public Sprite Sprite => sprite;
    public float Damage => damage;
    public float Cooldown => cooldown;
    public int NumberOfArrows => numberOfArrows;
    public int UpgradePrice => upgradePrice;
}