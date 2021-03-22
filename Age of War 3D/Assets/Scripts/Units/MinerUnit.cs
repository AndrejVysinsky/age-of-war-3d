using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerUnit : Unit
{
    [SerializeField] GoldController goldController;
    private bool _isMining = false;

    private Vector3 goldVeinPosition;

    private void Awake()
    {
        // set _destination to each map's Mining spot
        goldController = GameObject.Find("Player").GetComponent<GoldController>();
    }

    public override void Initialize(int unitID, int unitTier, Line line, FactionEnum faction, Material factionMaterial)
    {
        base.Initialize(unitID, unitTier, line, faction, factionMaterial);

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


            goldController.AddBalance(_unitData.Reward);
            yield return new WaitForSeconds(2f);
        }
    }
}
