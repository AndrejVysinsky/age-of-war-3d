using UnityEngine;

[CreateAssetMenu(fileName = "New Outpost Data", menuName = "Outpost/Outpost Data")]
public class OutpostData : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] Sprite sprite;
    [SerializeField] float health;
    [SerializeField] int maxQueueCapacity;
    [SerializeField] int upgradePrice;
    [SerializeField] int maxMinerUnits;

    public string Name => name;
    public Sprite Sprite => sprite;
    public float Health => health;
    public int MaxQueueCapacity => maxQueueCapacity;
    public int UpgradePrice => upgradePrice;
    public int MaxMinerUnits => maxMinerUnits;
}