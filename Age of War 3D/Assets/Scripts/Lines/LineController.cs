using System;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField] List<Line> lines;

    public List<GameObject> LinePath { get; set; } = new List<GameObject>();
    public List<Unit> UnitsInLine { get; set; } = new List<Unit>();
}