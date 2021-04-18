using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MinerUnit : Unit
{
    [SerializeField] GoldController goldController;
    private bool _isMining = false;

    private Vector3 goldVeinPosition;

    public void Initialize(FactionEnum faction, Material factionMaterial)
    {
        _unitData = unitTiers[0];

        Faction = faction;

        if (colorSwitcher != null)
        {
            colorSwitcher.SwitchColors(factionMaterial.color);
        }

        var goldControllers = FindObjectsOfType<GoldController>();

        for (int i = 0; i < goldControllers.Length; i++)
        {
            if (goldControllers[i].Faction == Faction)
            {
                goldController = goldControllers[i];
            }
        }

        var goldVeins = FindObjectsOfType<GoldVein>();

        for (int i = 0; i < goldVeins.Length; i++)
        {
            if (goldVeins[i].Faction == Faction)
            {
                goldVeinPosition = goldVeins[i].transform.position;

                transform.position = goldVeins[i].MinerSpawnPoint.transform.position;
                _destination = goldVeins[i].GetFreeMiningPoint();
                transform.LookAt(_destination);
            }
        }
    }

    private void Update()
    {
        if (!_isMining && transform.position == _destination)
        {
            _isMining = true;
            transform.LookAt(goldVeinPosition);
            StartCoroutine(StartMining());
        }

        if (!_isMining && transform.position != _destination)
        {
            MoveTowardsObjective();
        }
    }

    protected override void MoveTowardsObjective()
    {
        if (_destination == Vector3.zero)
            return;

        transform.LookAt(_destination);
        transform.position = Vector3.MoveTowards(transform.position, _destination, _unitData.MovementSpeed * Time.deltaTime);
    }

    public override void TakeDamage(float damage)
    {
        // Do nothing
    }
    
    private IEnumerator StartMining()
    {
        while (true)
        {
            // play animation

            var position = transform.position;
            position.Set(position.x, position.y + HEIGHT_OFFSET, position.z);
            var text_holder = Instantiate(dmgTakenText, position, Quaternion.identity);
            text_holder.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("+ " + _unitData.Reward);
            goldController.AddBalance(_unitData.Reward);
            yield return new WaitForSeconds(2f);
        }
    }
}
