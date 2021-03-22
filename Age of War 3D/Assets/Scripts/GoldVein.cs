using System.Collections.Generic;
using UnityEngine;

public class GoldVein : MonoBehaviour
{
    [SerializeField] FactionEnum faction;
    [SerializeField] List<GameObject> miningPoints;
    [SerializeField] GameObject minerSpawnPoint;

    private bool[] isMiningPointTaken;

    public GameObject MinerSpawnPoint => minerSpawnPoint;
    public FactionEnum Faction => faction;

    private void Awake()
    {
        isMiningPointTaken = new bool[miningPoints.Count];
    }

    public Vector3 GetFreeMiningPoint()
    {
        for (int i = 0; i < isMiningPointTaken.Length; i++)
        {
            if (isMiningPointTaken[i] == false)
            {
                isMiningPointTaken[i] = true;
                return miningPoints[i].transform.position;
            }
        }
        return Vector3.zero;
    }
}
