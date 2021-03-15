using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] List<GameObject> checkPoints;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] bool isActive;

    public bool IsActive => isActive;

    private void Start()
    {
        if (IsActive)
        {
            ShowLineRenderer();
        }
    }

    private void ShowLineRenderer()
    {
        lineRenderer.positionCount = checkPoints.Count;

        for (int i = 0; i < checkPoints.Count; i++)
        {
            var position = checkPoints[i].transform.position;
            position.y += 0.01f;

            lineRenderer.SetPosition(i, position);
        }
    }

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

    public void ChangeLineColor(Gradient gradient)
    {
        lineRenderer.colorGradient = gradient;
    }
}