using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Image highlightImage;

    [SerializeField] Image mapImage;
    [SerializeField] TextMeshProUGUI mapName;

    [Header("Locked state")]
    [SerializeField] Sprite lockedMapSprite;
    [SerializeField] MapEnum lockedMapName;

    [Header("Unlocked state")]
    [SerializeField] Sprite unlockedMapSprite;
    [SerializeField] MapEnum unlockedMapName;

    private bool _clicked;
    private bool _unlocked;
    private MapSelector _mapSelector;

    private void Start()
    {
        if (PlayerPrefs.HasKey(Enum.GetName(typeof(MapEnum), unlockedMapName)) || (int)unlockedMapName == 0)
        {
            _unlocked = true;
            mapImage.sprite = unlockedMapSprite;
            mapName.text = Enum.GetName(typeof(MapEnum), unlockedMapName);
        }
        else
        {
            _unlocked = false;
            mapImage.sprite = lockedMapSprite;
            mapName.text = Enum.GetName(typeof(MapEnum), lockedMapName);
        }

        _mapSelector = GetComponentInParent<MapSelector>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_unlocked == false)
            return;

        _mapSelector.SelectMap(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlightImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_clicked == false)
            highlightImage.enabled = false;
    }

    public void Select()
    {
        _clicked = true;
        highlightImage.enabled = true;
    }

    public void Deselect()
    {
        _clicked = false;
        highlightImage.enabled = false;
    }
}
