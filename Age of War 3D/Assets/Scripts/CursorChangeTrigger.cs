﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class CursorChangeTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            MouseCursor.Instance.SetHandCursor();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            MouseCursor.Instance.SetPointerCursor();
        }

        private void OnMouseEnter()
        {
            MouseCursor.Instance.SetHandCursor();
        }

        private void OnMouseExit()
        {
            MouseCursor.Instance.SetPointerCursor();
        }
    }
}