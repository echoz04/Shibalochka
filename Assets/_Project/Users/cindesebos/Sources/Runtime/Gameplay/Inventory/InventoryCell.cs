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

        private InventoryRoot _inventory;
        private float _size;

        public void Initialize(int x, int y, InventoryRoot inventory, float cellSize)
        {
            X = x; Y = y;
            _inventory = inventory;
            _size = cellSize;

            var rt = GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(X * _size, -Y * _size);
            rt.sizeDelta = new Vector2(_size, _size);

            IsOccupied = false;
            _highlightImage.enabled = false;
            _backgroundImage.color = Color.white;
        }

        public void SetOccupied()
        {
            IsOccupied = true;
            _backgroundImage.color = Color.red;
        }

        public void ClearOccupied()
        {
            IsOccupied = false;
            _backgroundImage.color = Color.white;
        }

        public void SetHighlight(bool enable) => _highlightImage.enabled = enable;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (InventoryItem.CurrentDragging == null) return;
            InventoryItem.CurrentDragging.SetHover(new Vector2Int(X, Y));
            _inventory.HighlightCells(InventoryItem.CurrentDragging.Config, new Vector2Int(X, Y), InventoryItem.CurrentDragging.IsRotated);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (InventoryItem.CurrentDragging == null) return;
            InventoryItem.CurrentDragging.ClearHover();
            _inventory.ClearHighlight();
        }
    }
}