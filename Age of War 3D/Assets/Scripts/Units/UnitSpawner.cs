using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] GameObject spawnPoint;
    [SerializeField] GameObject destinationPoint;
    [SerializeField] FactionEnum faction;

    //testing
    [SerializeField] GameObject unitPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (faction == FactionEnum.Blue)
        {
            StartCoroutine(SpawnerForAI());
        }
    }

    public IEnumerator SpawnerForAI()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            SpawnUnit(unitPrefab);
        }
    }

    public void SpawnUnit(GameObject unitPrefab)
    {
        var unitObject = Instantiate(unitPrefab, transform);

        unitObject.transform.position = spawnPoint.transform.position;

        unitObject.GetComponent<Unit>().Initialize(destinationPoint.transform.position, faction);
    }
}
