﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

public interface IOutpostUpgraded : IEventSystemHandler
{
    void OnOutpostUpgraded(OutpostData outpostData, FactionEnum faction, float dmgTaken);
}