using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Image highlightImage;

    private bool _clicked;
    private MapSelector _mapSelector;

    private void Start()
    {
        _mapSelector = GetComponentInParent<MapSelector>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
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
