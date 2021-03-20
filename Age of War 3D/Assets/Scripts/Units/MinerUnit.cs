using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerUnit : Unit
{
    [SerializeField] GoldController goldController;
    private bool _isMining = false;

    private void Awake()
    {
        // set _destination to each map's Mining spot
        goldController = GameObject.Find("Player").GetComponent<GoldController>();
    }

    private void Update()
    {
        if (!_isMining && transform.position == _destination)
        {
            _isMining = true;
            StartCoroutine(StartMining());
        }

        if (!_isMining && transform.position != _destination)
        {
            MoveTowardsEnemyBase();
        }
    }

    protected override void MoveTowardsEnemyBase()
    {
        _destination = GameObject.Find("Mining point").transform.position;
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
