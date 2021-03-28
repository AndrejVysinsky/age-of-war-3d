using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : BaseGameController
{
    [SerializeField] AIBrain aiBrain;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(SpawnUnit());

        //nech brain vie ake lajny existuju
        var lines = new List<Line>();
        for (int i = 0; i < lineController.GetNumberOfActiveLines(); i++)
        {
            lines.Add(lineController.GetLineByIndex(i));
        }
        aiBrain.Initialize(lines);
    }

    private IEnumerator SpawnUnit()
    {
        while (true)
        {
            int activeLines = lineController.GetNumberOfActiveLines();

            //max is exclusive
            int randomLineIndex = Random.Range(0, activeLines);
            int randomUnitIndex = Random.Range(0, unitSpawner.NumberOfDifferentUnits - 1);

            _activeLine = lineController.GetLineByIndex(randomLineIndex);

            SpawnUnit(randomUnitIndex);

            yield return new WaitForSeconds(1.7f);
        }
    }
}