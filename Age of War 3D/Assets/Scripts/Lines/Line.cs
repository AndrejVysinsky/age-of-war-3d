using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] List<GameObject> checkPoints;
    [SerializeField] bool isActive;

    public bool IsActive => isActive;

    public Vector3 GetCheckpointPosition(int index)
    {
        if (index < 0 || index >= checkPoints.Count)
            return Vector3.zero;

        return checkPoints[index].transform.position;
    }

    public bool HasNextCheckpoint(int index)
    {
        return index < checkPoints.Count;
    }
}