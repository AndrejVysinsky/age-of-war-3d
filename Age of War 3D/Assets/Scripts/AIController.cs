using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] FactionEnum faction;
    [SerializeField] Material factionMaterial;
    [SerializeField] LineController lineController;
    [SerializeField] UnitSpawner unitSpawner;

    [SerializeField] List<GameObject> unitPrefabs;

    private Line _activeLine;

    private void Start()
    {
        StartCoroutine(SpawnUnit());
    }

    private IEnumerator SpawnUnit()
    {
        while (true)
        {
            int activeLines = lineController.GetNumberOfActiveLines();

            //max is exclusive
            int randomLineIndex = Random.Range(0, activeLines);
            int randomUnit = Random.Range(0, unitPrefabs.Count);

            _activeLine = lineController.GetLineByIndex(randomLineIndex);

            SpawnUnit(unitPrefabs[randomUnit]);

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SpawnUnit(GameObject unitPrefab)
    {
        unitSpawner.SpawnUnit(unitPrefab, _activeLine, faction, factionMaterial);
    }
}