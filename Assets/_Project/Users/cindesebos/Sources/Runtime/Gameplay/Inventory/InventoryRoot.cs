using System;
using UnityEngine;
using Sources.Runtime.Gameplay.Inventory.Item;
using System.Collections.Generic;
using Zenject;
using Sources.Runtime.Services.ProjectConfigLoader;
using UnityEngine.InputSystem;
using Sources.Runtime.Gameplay.MiniGames.Fishing;
using Sources.Runtime.Gameplay.MiniGames;

namespace Sources.Runtime.Gameplay.Inventory
{
    [RequireComponent(typeof(InventoryView))]
    public class InventoryRoot : MonoBehaviour
    {
        public event Action<List<InventoryCell>> OnBuildCells;
        public event Action OnItemAdded;
        public event Action OnItemRemoved;

        public bool IsVisible => _canvas.enabled;

        public int Width => _projectConfigLoader.ProjectConfig.InventoryConfig.InventoryWidth;
        public int Height => _projectConfigLoader.ProjectConfig.InventoryConfig.InventoryHeigth;
        public float CellSize => _projectConfigLoader.ProjectConfig.InventoryConfig.InventoryCellSize;
        public float Spacing => _projectConfigLoader.ProjectConfig.InventoryConfig.InventorySpacing;

        [SerializeField] private Canvas _canvas;
        [SerializeField] private InventoryView _view;
        [SerializeField] private InventoryCell _cellPrefab;
        [SerializeField] private RectTransform _gridContent;
        [SerializeField] private GameObject _controlButtons;

        private CharacterInput _characterInput;
        private IProjectConfigLoader _projectConfigLoader;
        private StaminaHandler _staminaHandler;

        private ItemRoot _previousSelectedItem;
        private ItemRoot _selectedItem;

        private InventoryCell[,] _currentGrid;
        private List<InventoryCell> _allCells = new List<InventoryCell>();

        [Inject]
        private void Construct(CharacterInput characterInput, IProjectConfigLoader projectConfigLoader, StaminaHandler staminaHandler)
        {
            _characterInput = characterInput;
            _projectConfigLoader = projectConfigLoader;
            _staminaHandler = staminaHandler;
        }

        public void Initialize()
        {
            _characterInput.UI.ToggleInventoryVisibility.performed += ToggleVisibility;

            _view.Initialize(this, _projectConfigLoader);

            BuildGrid();

            _canvas.enabled = true;
        }

        private void OnDestroy()
        {
            _characterInput.UI.ToggleInventoryVisibility.performed -= ToggleVisibility;
        }

        private void BuildGrid()
        {
            _currentGrid = new InventoryCell[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var cell = Instantiate(_cellPrefab, _gridContent);
                    cell.Initialize(x, y, CellSize, this);
                    _currentGrid[x, y] = cell;
                    _allCells.Add(cell);

                }
            }
        }

        public void ToggleVisibility(InputAction.CallbackContext context)
        {
            if (_staminaHandler.IsStarted == true)
                return;

            _canvas.enabled = !_canvas.enabled;

            if (_canvas.enabled == true)
                OnBuildCells?.Invoke(_allCells);
        }

        public void HightligchCells(ItemRoot itemRoot)
        {
            ProcessItemPlacement(itemRoot, true);
        }

        public bool TryPlaceItem(ItemRoot itemRoot)
        {
            return ProcessItemPlacement(itemRoot, false);
        }

        public bool TryToggleControlButtons(ItemRoot itemRoot)
        {
            if (_selectedItem == itemRoot)
            {
                itemRoot.SetSelection(false);
                _controlButtons.SetActive(false);
                _selectedItem = null;
                _previousSelectedItem = null;

                return false;
            }

            if (_selectedItem != null)
            {
                itemRoot.SetSelection(false);
                _previousSelectedItem = _selectedItem;
            }

            _selectedItem = itemRoot;
            itemRoot.SetSelection(true);
            _controlButtons.SetActive(true);

            if (_previousSelectedItem != null)
                _previousSelectedItem.SetSelection(false);

            return true;
        }

        public void RotateItem()
        {
            if (_selectedItem == null)
                return;

            _selectedItem.Rotate();
        }

        private bool ProcessItemPlacement(ItemRoot itemRoot, bool highlightOnly)
        {
            itemRoot.ClearOccupiedInventoryCells();
            float radius = CellSize / 1.35f;

            List<InventoryCell> matchedCells = new List<InventoryCell>();
            bool allValid = true;

            foreach (var point in itemRoot.CellsPoints)
            {
                InventoryCell nearestCell = FindNearestCell(point.Transform.position, radius);

                if (nearestCell == null || nearestCell.IsOccupied == true)
                {
                    allValid = false;
                    continue;
                }

                matchedCells.Add(nearestCell);
            }

            foreach (var cell in matchedCells)
            {
                itemRoot.AddOccupiedInventoryCells(cell);

                if (highlightOnly)
                {
                    if (allValid)
                        cell.HighlightValid();
                    else
                        cell.HighlightInvalid();
                }
                else
                {
                    cell.SetOccupied(allValid);

                    if (allValid)
                        cell.HighlightValid();
                    else
                        cell.HighlightInvalid();
                }
            }

            return allValid;
        }

        private InventoryCell FindNearestCell(Vector3 worldPos, float radius)
        {
            foreach (var cell in GetAllCells())
            {
                Vector3 cellPos = cell.transform.position;

                float dx = Mathf.Abs(worldPos.x - cellPos.x);
                float dy = Mathf.Abs(worldPos.y - cellPos.y);

                if (dx <= radius && dy <= radius)
                    return cell;
            }

            return null;
        }

        private IEnumerable<InventoryCell> GetAllCells()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    yield return _currentGrid[x, y];
                }
            }
        }
    }
}
