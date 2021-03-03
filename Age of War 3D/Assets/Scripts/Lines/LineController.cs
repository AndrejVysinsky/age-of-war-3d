using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField] List<Line> lines;

    public int GetNumberOfActiveLines()
    {
        return lines.Count;
    }

    public Line GetFirstActiveLine()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].IsActive)
            {
                return lines[i];
            }
        }
        return null;
    }

    public bool IsLineActive(int index)
    {
        if (index < 0 || index >= lines.Count)
            return false;

        return lines[index].IsActive;
    }

    public Line GetLineByIndex(int index)
    {
        return lines[index];
    }
}