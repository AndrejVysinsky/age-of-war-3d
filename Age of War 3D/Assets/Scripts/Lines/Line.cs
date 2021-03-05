using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] List<GameObject> checkPoints;
    [SerializeField] bool isActive;

    public bool IsActive => isActive;

    public Vector3 GetCheckpointPosition(int index, FactionEnum faction)
    {
        if (index < 0 || index >= checkPoints.Count)
            return Vector3.zero;

        //blue needs to go in opposite direction
        if (faction == FactionEnum.Blue)
        {
            index = checkPoints.Count - 1 - index;
        }

        return checkPoints[index].transform.position;
    }

    public bool HasNextCheckpoint(int index, FactionEnum faction)
    {
        //blue needs to go in opposite direction
        if (faction == FactionEnum.Blue)
        {
            index = checkPoints.Count - 1 - index;
            return index >= 0;
        }

        return index < checkPoints.Count;
    }
}