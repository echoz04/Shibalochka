using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sources.Runtime.Gameplay.Inventory
{
    public class InventoryCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool IsOccupied { get; private set; }

        [SerializeField] private Image _highlightImage;
        [SerializeField] private Image _backgroundImage;

        private InventoryService _inventory;
        private float _size;

        private Color _defaultColor = new Color(1f, 1f, 1f, 0.1f);
        private Color _occupiedColor = new Color(1f, 0f, 0f, 0.3f);

        public void Initialize(int x, int y, InventoryService inventory, float cellSize)
        {
            X = x;
            Y = y;
            _inventory = inventory;
            _size = cellSize;

            var rectTransform = GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(X * _size, Y * _size);
            rectTransform.sizeDelta = new Vector2(_size, _size);

            IsOccupied = false;
            _highlightImage.enabled = false;
            _backgroundImage.color = _defaultColor;
        }

        public void SetOccupied(InventoryItem item)
        {
            IsOccupied = true;
            _backgroundImage.color = _occupiedColor;

            item.transform.SetParent(transform, false);
            item.transform.localPosition = Vector3.zero;
        }

        public void ClearOccupied()
        {
            IsOccupied = false;
            _backgroundImage.color = _defaultColor;
        }

        public void SetHighlight(bool enable)
        {
            _highlightImage.enabled = enable;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (InventoryItem.CurrentDragging == null)
                return;

            var drag = InventoryItem.CurrentDragging;
            Vector2Int basePosition = new Vector2Int(X, Y);
            drag.SetHover(basePosition);
            _inventory.HighlightCells(drag.Config, basePosition, drag.IsRotated);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (InventoryItem.CurrentDragging == null)
                return;

            InventoryItem.CurrentDragging.ClearHover();
            _inventory.ClearHighlight();
        }
    }
}
