using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Sources.Runtime.Gameplay.Inventory
{
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public static InventoryItem CurrentDragging;

        [field: SerializeField] public ItemConfig Config { get; private set; }

        public bool IsOccupied { get; private set; }
        public bool IsRotated { get; private set; }
        public Vector2Int LastHoverPosition { get; private set; }
        public bool HasValidHover { get; private set; }
        public List<InventoryCell> CurrentCells { get; private set; }

        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _image;
        [SerializeField] private CanvasGroup _canvasGroup;

        private InventoryService _inventory;
        private Vector3 _originalPosition;
        private Transform _originalParent;
        private Vector2 _pointerOffset;

        private void OnValidate()
        {
            _canvas ??= GetComponentInParent<Canvas>();
            _canvasGroup ??= GetComponent<CanvasGroup>();
            _image ??= GetComponent<Image>();
        }

        [Inject]
        private void Construct(InventoryService inventory)
        {
            _inventory = inventory;

            CurrentCells = new List<InventoryCell>();

            IsRotated = false;
            HasValidHover = false;
        }

        private void Awake()
        {
            _image.sprite = Config.Icon;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _inventory.TryRemoveItem(this);

            CurrentDragging = this;

            _originalPosition = transform.position;
            _originalParent = transform.parent;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out _pointerOffset);

            transform.SetParent(_canvas.transform, true);
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 globalPoint;

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            _canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out globalPoint))
        {
            transform.position = globalPoint - new Vector3(_pointerOffset.x, _pointerOffset.y, 0f);
        }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (HasValidHover)
            {
                _inventory.TryPlaceItem(this);
            }
            else
            {
                transform.SetParent(_canvas.transform, false);
                transform.position = eventData.position;

                CurrentCells.Clear();
            }
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

        public void OnPlaced(List<InventoryCell> cells)
        {
            CurrentCells = cells;

            transform.SetParent(cells[0].transform, false);
            transform.localPosition = Vector3.zero;
        }

        public void CancelPlacement()
        {
            transform.SetParent(_originalParent, false);

            transform.position = _originalPosition;
        }
    }
}
