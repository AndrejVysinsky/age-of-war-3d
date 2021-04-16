﻿using System.Collections.Generic;
using UnityEngine;

public class MapSelector : MonoBehaviour
{
    [SerializeField] List<MapCard> mapCards;
    [SerializeField] SceneLoader sceneLoader;

    private int _selectedMapIndex = 0;

    private void OnEnable()
    {
        _selectedMapIndex = 0;
        DeselectOtherMaps();
    }

    public void SelectMap(MapCard mapCard)
    {
        _selectedMapIndex = mapCards.IndexOf(mapCard);

        DeselectOtherMaps();
    }

    private void DeselectOtherMaps()
    {
        for (int i = 0; i < mapCards.Count; i++)
        {
            if (i == _selectedMapIndex)
            {
                mapCards[i].Select();
            }
            else
            {
                mapCards[i].Deselect();
            }
        }
    }

    public void LoadSelectedMap()
    {
        sceneLoader.LoadScene(_selectedMapIndex + 1);
    }
}
