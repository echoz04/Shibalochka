using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Runtime.Gameplay.Configs;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Sources.Runtime.Gameplay.Inventory.Item
{
    [RequireComponent(typeof(ItemDragger), typeof(ItemView))]
    public class ItemRoot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public event Action<bool> OnSelected;

        public IEnumerable<ItemCellPoint> CellsPoints => _cellPoints;

        public bool IsSelected { get; set; }

        [SerializeField] private Transform _cellPointsParent;
        [SerializeField] private ItemConfig _config;
        [SerializeField] private ItemDragger _dragger;
        [SerializeField] private ItemView _view;

        private InventoryRoot _inventoryRoot;
        private List<ItemCellPoint> _cellPoints = new();
        private List<InventoryCell> _occupiedInventoryCells = new();
        private Vector3 _previousPosition;
        private bool _canSelect = false;

        private void OnValidate()
        {
            _dragger ??= GetComponent<ItemDragger>();
            _view ??= GetComponent<ItemView>();
        }

        [Inject]
        private void Construct(InventoryRoot inventoryRoot)
        {
            _inventoryRoot = inventoryRoot;
        }

        private void Start()
        {
            _occupiedInventoryCells = new List<InventoryCell>();

            CreateCellsPoints();

            _view.Initialize(this, _config);
            _dragger.Initialize(_inventoryRoot, this);
        }

        public void CreateCellsPoints()
        {
            for (int i = 0; i < _config.CellPointsPosition.Count(); i++)
            {
                var position = _config.CellPointsPosition.ElementAt(i);
                Debug.Log("position by " + i + " is " + position);

                var instance = Instantiate(_config.ItemCellPointPrefab);

                instance.transform.SetParent(_cellPointsParent);
                instance.transform.localPosition = position;

                _cellPoints.Add(instance);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragger.BeginDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _dragger.Drag();

            if (_previousPosition == transform.position)
                return;

            _previousPosition = transform.position;

            _inventoryRoot.HightligchCells(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _dragger.EndDrag();

            if (_inventoryRoot.TryPlaceItem(this))
            {
                if (_occupiedInventoryCells.Count > 0)
                {
                    Vector3 offset = transform.position - _cellPoints[0].Transform.position;
                    Vector3 snapTo = _occupiedInventoryCells[0].transform.position;
                    transform.position = snapTo + offset;
                }

                _canSelect = true;
            }
            else
            {
                _canSelect = false;

                ClearOccupiedInventoryCells();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_occupiedInventoryCells.Count == 0 || _canSelect == false)
                return;

            _dragger.PointClick();

            bool isSelected = _inventoryRoot.TryToggleControlButtons(this);
        }

        public void AddOccupiedInventoryCells(InventoryCell inventoryCell)
        {
            if (_occupiedInventoryCells.Contains(inventoryCell))
                return;

            _occupiedInventoryCells.Add(inventoryCell);
        }

        public void ClearOccupiedInventoryCells()
        {
            foreach (var occupiedInventoryCell in _occupiedInventoryCells)
                occupiedInventoryCell.SetOccupied(false);

            _occupiedInventoryCells.Clear();
        }

        public void Rotate()
        {
            transform.Rotate(0f, 0f, -90f);
        }

        public void SetSelection(bool isSelect)
        {
            IsSelected = isSelect;
            OnSelected?.Invoke(IsSelected);
        }
    }
}
