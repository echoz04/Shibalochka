using System;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using Sources.Runtime.Gameplay.Configs;
using Sources.Runtime.Services.ProjectConfigLoader;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Sources.Runtime.Gameplay.Inventory.Item
{
    [RequireComponent(typeof(ItemDragger), typeof(ItemView))]
    public class ItemRoot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public event Action OnBeginDragging;
        public event Action OnDragging;
        public event Action OnEndDragging;
        public event Action<bool> OnSelected;

        public IEnumerable<ItemCellPoint> CellsPoints => _cellPoints;

        public bool IsSelected { get; set; }

        [SerializeField] private Transform _cellPointsParent;
        [SerializeField] private ItemConfig _config;
        [SerializeField] private ItemDragger _dragger;
        [SerializeField] private ItemView _view;

        private InventoryRoot _inventoryRoot;
        private IProjectConfigLoader _projectConfigLoader;
        private List<ItemCellPoint> _cellPoints = new();
        private List<InventoryCell> _occupiedInventoryCells = new();
        private Vector3 _previousPosition;
        private bool _canSelect = false;
        private bool _isDragging = false;

        private void OnValidate()
        {
            _dragger ??= GetComponent<ItemDragger>();
            _view ??= GetComponent<ItemView>();
        }

        [Inject]
        private void Construct(InventoryRoot inventoryRoot, IProjectConfigLoader projectConfigLoader)
        {
            _inventoryRoot = inventoryRoot;
            _projectConfigLoader = projectConfigLoader;
        }

        private void Start()
        {
            _occupiedInventoryCells = new List<InventoryCell>();

            CreateCellsPoints();

            _view.Initialize(this, _config, _projectConfigLoader.ProjectConfig.InventoryConfig);
            _dragger.Initialize(_inventoryRoot, this);
        }

        public void CreateCellsPoints()
        {
            for (int i = 0; i < _config.CellPointsPosition.Count(); i++)
            {
                var position = _config.CellPointsPosition.ElementAt(i);

                var instance = Instantiate(_config.ItemCellPointPrefab);

                instance.transform.SetParent(_cellPointsParent);
                instance.transform.localPosition = position;

                _cellPoints.Add(instance);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_occupiedInventoryCells.Count == 0)
            {
                Drag();

                return;
            }
            else
            {
                if (IsSelected == true)
                {
                    Drag();

                    return;
                }
            }

            _isDragging = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isDragging == false)
                return;

            OnDragging?.Invoke();

            if (_previousPosition == transform.position)
                return;

            _previousPosition = transform.position;

            _inventoryRoot.HightligchCells(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_isDragging == false)
                return;

            OnEndDragging?.Invoke();

            PlaceItem();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_occupiedInventoryCells.Count == 0 || _canSelect == false)
                return;

            _inventoryRoot.TryToggleControlButtons(this);
        }

        private void Drag()
        {
            OnBeginDragging?.Invoke();

            RuntimeManager.PlayOneShot("event:/SFX/UI/UI_Open");
            _isDragging = true;
            _inventoryRoot.TryToggleControlButtons(this);
        }

        private void PlaceItem(bool isImmediatelyPlace = true)
        {
            if (_inventoryRoot.TryPlaceItem(this))
            {
                if (_occupiedInventoryCells.Count > 0)
                {
                    Vector3 offset = transform.position - _cellPoints[0].Transform.position;
                    Vector3 snapTo = _occupiedInventoryCells[0].transform.position;
                    transform.position = snapTo + offset;
                }

                _canSelect = true;

                if (isImmediatelyPlace == true)
                {
                    RuntimeManager.PlayOneShot("event:/SFX/UI/UI_Close");
                }
            }
            else
            {
                _canSelect = false;

                ClearOccupiedInventoryCells();
            }
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
            RuntimeManager.PlayOneShot("event:/SFX/UI/UI_PointerEnter");
            transform.Rotate(0f, 0f, -90f);

            PlaceItem(false);
        }

        public void SetSelection(bool isSelected)
        {
            if (isSelected)
                RuntimeManager.PlayOneShot("event:/SFX/UI/UI_Slot_Select");
            else
                RuntimeManager.PlayOneShot("event:/SFX/UI/UI_Slot_Deselect");

            IsSelected = isSelected;
            OnSelected?.Invoke(IsSelected);
        }

        public void Delete(Transform newParent)
        {
            _inventoryRoot.TryToggleControlButtons(this);
            SetSelection(false);
            ClearOccupiedInventoryCells();
            transform.SetParent(newParent);
            transform.localPosition = Vector3.zero;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);
        }
    }
}
