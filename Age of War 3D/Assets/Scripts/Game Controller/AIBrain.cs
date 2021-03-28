using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    [SerializeField] AIUnitScanner aiUnitScanner;

    private List<Line> _lines = new List<Line>();

    public void Initialize(List<Line> lines)
    {
        //asi vyuzijes ale ak nie kludne vymaz
        _lines = lines;
        aiUnitScanner.Initialize(lines);
    }

    /*
     mozno by tu mohli byt nejake rozhodnutia... priority list?? necham na teba

    a ze by AIController v nejakych pravidelnych intervaloch si vypytal od AIBrain rozhodnutie

    + AIBrain ma referenciu na AIUnitScanner od ktoreho ziska jednotky na danej lajne

    nemas to staticke ale mas tam public getter na jednotky :D
    aiUnitScanner.LineUnitHolders

    spravit mozno nejaku metodu ze ziskat lajnu kde ma hrac najvacsiu prevahu
     
     */
}