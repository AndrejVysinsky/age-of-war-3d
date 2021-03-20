using System.Collections;
using UnityEngine;

public class AIController : BaseGameController
{
    protected override void Start()
    {
        base.Start();
        StartCoroutine(SpawnUnit());
    }

    private IEnumerator SpawnUnit()
    {
        while (true)
        {
            int activeLines = lineController.GetNumberOfActiveLines();

            //max is exclusive
            int randomLineIndex = Random.Range(0, activeLines);
            int randomUnitIndex = Random.Range(0, unitSpawner.NumberOfDifferentUnits);

            _activeLine = lineController.GetLineByIndex(randomLineIndex);

            SpawnUnit(randomUnitIndex);

            yield return new WaitForSeconds(1.7f);
        }
    }
}