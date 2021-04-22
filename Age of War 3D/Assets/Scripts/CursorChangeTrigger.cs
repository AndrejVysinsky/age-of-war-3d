using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorChangeTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Tooltip("Special check because of closing buttons.")]
    [SerializeField] bool pointerOnClick;

    private static bool _isOverUI = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isOverUI = true;

        MouseCursor.Instance.SetHandCursor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isOverUI = false;

        MouseCursor.Instance.SetPointerCursor();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (pointerOnClick == false)
            return;

        MouseCursor.Instance.SetPointerCursor();
    }

    private void OnMouseEnter()
    {
        if (_isOverUI)
            return;

        MouseCursor.Instance.SetHandCursor();
    }

    private void OnMouseExit()
    {
        if (_isOverUI)
            return;

        MouseCursor.Instance.SetPointerCursor();
    }
}