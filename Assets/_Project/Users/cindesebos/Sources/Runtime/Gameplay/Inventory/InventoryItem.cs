using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;
using Sources.Runtime.Gameplay.Configs;

namespace Sources.Runtime.Gameplay.Inventory
{
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public static InventoryItem CurrentDragging;

        [field: SerializeField] public ItemConfig Config { get; private set; }

        public bool IsRotated { get; private set; }
        public Vector2Int LastHoverPosition { get; private set; }
        public bool HasValidHover { get; private set; }
        public List<InventoryCell> CurrentCells { get; private set; }

        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _image;

        private InventoryRoot _inventory;
        private Vector3 _originalPosition;
        private Transform _originalParent;

        [Inject]
        private void Construct(InventoryRoot inventory)
        {
            _inventory = inventory;
            CurrentCells = new List<InventoryCell>();
            IsRotated = false;
            HasValidHover = false;
        }

        private void Awake()
        {
            _image.sprite = Config.Icon;
            _image.alphaHitTestMinimumThreshold = 0.1f;

            var rt = GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0, 1);
            rt.pivot = new Vector2(0, 1);

            _originalPosition = transform.position;
            _originalParent = transform.parent;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _inventory.StartDraggingItem(this);

            _inventory.TryRemoveItem(this);
            CurrentDragging = this;

            _inventory.MoveToDraggingContainer(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (HasValidHover == true && _inventory.TryPlaceItem(Config, LastHoverPosition, IsRotated, out var cells) == true)
            {
                _inventory.TryRemoveItem(this);

                foreach (var cell in cells)
                    cell.SetOccupied();

                CurrentCells = cells;

                transform.SetParent(_inventory.ItemsContainer, false);
                SnapToCells(cells);

                _inventory.AddItem(this);
            }
            else
            {
                transform.SetParent(_originalParent, false);
                transform.position = _originalPosition;
                CurrentCells.Clear();
                _inventory.RemoveItem(this);
            }

            HasValidHover = false;
            _inventory.ClearHighlight();
            CurrentDragging = null;
            _inventory.EndDraggingItem();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (CurrentCells.Count > 0)
                return;

            IsRotated = !IsRotated;
            transform.Rotate(Vector3.forward, -90f);
        }

        public void SetHover(Vector2Int cellPosition)
        {
            LastHoverPosition = cellPosition;
            HasValidHover = true;
        }

        public void ClearHover() => HasValidHover = false;

        public void SetRaycastValue(bool value) =>
            _image.raycastTarget = value;

        private void SnapToCells(List<InventoryCell> cells)
        {
            float cellSize = _inventory.CellSize;
            float spacing = _inventory.Spacing;

            Vector2Int min = new Vector2Int(int.MaxValue, int.MaxValue);

            foreach (var cell in cells)
            {
                if (cell.X < min.x) min.x = cell.X;
                if (cell.Y < min.y) min.y = cell.Y;
            }

            var rt = GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0, 1);
            rt.pivot = new Vector2(0, 1);

            float totalCellSize = cellSize + spacing;

            Vector2 pos = new Vector2(min.x * totalCellSize, -min.y * totalCellSize);
            rt.anchoredPosition = pos;
        }
    }
}