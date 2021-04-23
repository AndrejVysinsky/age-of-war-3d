using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : BaseGameController
{
    [SerializeField] AIBrain aiBrain;

    protected override void Start()
    {
        base.Start();
        //nech brain vie ake lajny existuju
        var lines = new List<Line>();
        for (int i = 0; i < lineController.GetNumberOfActiveLines(); i++)
        {
            lines.Add(lineController.GetLineByIndex(i));
        }
        aiBrain.Initialize(lines, goldController, outpost);
        StartCoroutine(SpawnUnit());
    }

    private IEnumerator SpawnUnit()
    {
        while (true)
        {
            var decisions = aiBrain.getNextDecisions();

            foreach (var decision in decisions)
            {
                // TODO: implement unit training 
                ActiveLine = lineController.GetLineByIndex(decision._lineID);
                SpawnUnit((int)decision._unitType);
            }
            yield return new WaitForSeconds(5f);
        }
    }
}