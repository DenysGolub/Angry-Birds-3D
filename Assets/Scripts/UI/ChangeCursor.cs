using UnityEngine;
using UnityEngine.EventSystems;

namespace AngryBirds.UI
{
    public class ChangeCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Texture2D _cursorTexture;
        private readonly CursorMode _cursorMode = CursorMode.Auto;
        private readonly Vector2 _hotSpot = Vector2.zero;

        public void OnPointerEnter(PointerEventData eventData)
        {
            Cursor.SetCursor(_cursorTexture, _hotSpot, _cursorMode);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Cursor.SetCursor(null, _hotSpot, _cursorMode);
        }
    }
}