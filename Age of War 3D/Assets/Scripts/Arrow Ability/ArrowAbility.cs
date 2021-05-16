using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAbility : MonoBehaviour
{
    [SerializeField] List<ArrowAbilityData> arrowAbilityTiers;

    private ArrowAbilityData _arrowAbilityData;
    private int _arrowAbilityTier;

    private FactionEnum _faction;

    public float Cooldown => _arrowAbilityData.Cooldown;
    public ArrowAbilityData ArrowAbilityData => _arrowAbilityData;

    public ArrowAbilityData Initialize(FactionEnum faction)
    {
        _faction = faction;

        Upgrade(0);

        return _arrowAbilityData;
    }

    public int UpgradeArrowAbility(int balance)
    {
        if (_arrowAbilityTier >= arrowAbilityTiers.Count - 1)
            return 0;

        int upgradePrice = arrowAbilityTiers[_arrowAbilityTier].UpgradePrice;

        if (balance < upgradePrice || upgradePrice == 0)
            return 0;

        Upgrade(_arrowAbilityTier + 1);

        return upgradePrice;
    }

    private void Upgrade(int tier)
    {
        _arrowAbilityTier = tier;
        _arrowAbilityData = arrowAbilityTiers[_arrowAbilityTier];

        EventManager.Instance.ExecuteEvent<IArrowAbilityUpgraded>((x, y) => x.OnArrowAbilityUpgraded(_arrowAbilityData, _faction));
    }

    public int UseAbility(int balance)
    {
        if (balance < _arrowAbilityData.Price)
        {
            return 0;
        }

        var lineBounds = GetComponent<BaseGameController>().ActiveLine.GetLineBounds();

        for (int i = 0; i < _arrowAbilityData.NumberOfArrows; i++)
        {
            float randomX = Random.Range(-lineBounds.extents.x, lineBounds.extents.x);
            randomX += lineBounds.center.x;

            float randomZ = Random.Range(-lineBounds.extents.z, lineBounds.extents.z);
            randomZ += lineBounds.center.z;

            float targetHeight = lineBounds.center.y - lineBounds.extents.y;

            var randomPosition = new Vector3(randomX, 20, randomZ);

            float delay = Random.Range(1f, 3f);

            StartCoroutine(SpawnArrow(delay, randomPosition, targetHeight));
        }

        return _arrowAbilityData.Price;
    }

    private IEnumerator SpawnArrow(float delay, Vector3 position, float targetHeight)
    {
        yield return new WaitForSeconds(delay);

        var arrow = Instantiate(_arrowAbilityData.ArrowPrefab, transform);
        arrow.GetComponent<Arrow>().Initialize(position, targetHeight, _arrowAbilityData.Damage, _faction);
    }
}