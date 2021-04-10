using UnityEngine;

namespace Assets.Scripts
{
    public class MouseCursor : MonoBehaviour
    {
        public static MouseCursor Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Texture2D pointerCursor;
        [SerializeField] Texture2D handCursor;
        [SerializeField] Vector2 cursorOffSet;

        private void Start()
        {
            SetPointerCursor();
        }

        public void SetHandCursor()
        {
            Cursor.SetCursor(handCursor, cursorOffSet, CursorMode.Auto);
        }

        public void SetPointerCursor()
        {
            Cursor.SetCursor(pointerCursor, cursorOffSet, CursorMode.Auto);
        }
    }
}
