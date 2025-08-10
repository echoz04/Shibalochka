using System;
using UnityEngine;
using Sources.Runtime.Gameplay.Inventory.Item;
using System.Collections.Generic;
using Zenject;
using Sources.Runtime.Services.ProjectConfigLoader;
using UnityEngine.InputSystem;
using Sources.Runtime.Gameplay.MiniGames.Fishing;
using Sources.Runtime.Gameplay.MiniGames;
using Sources.Runtime.Gameplay.Configs.Items;
using Sources.Runtime.Gameplay.Camera;
using Sources.Runtime.Services.Builders.Item;
using Sources.Runtime.Gameplay.Map;
using Sources.Runtime.Gameplay.Wallet;

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
        [SerializeField] private InventoryCell _cellPrefab;
        [SerializeField] private RectTransform _gridContent;
        [SerializeField] private GameObject _controlButtons;
        [SerializeField] private Transform _rewardsPanel;

        private CharacterInput _characterInput;
        private IProjectConfigLoader _projectConfigLoader;
        private StaminaHandler _staminaHandler;
        private CameraRotator _cameraRotator;
        private IItemBuilder _itemBuilder;
        private MapView _mapView;
        private WalletRoot _wallerRoot;

        private ItemRoot _previousSelectedItem;
        private ItemRoot _selectedItem;

        private InventoryCell[,] _currentGrid;
        [SerializeField] private List<ItemRoot> _unUsedItems = new();
        [SerializeField] private List<ItemRoot> _storingFishes = new();
        private List<InventoryCell> _allCells = new List<InventoryCell>();

        [Inject]
        private void Construct(CharacterInput characterInput, IProjectConfigLoader projectConfigLoader, StaminaHandler staminaHandler,
        CameraRotator cameraRotator, IItemBuilder itemBuilder, MapView mapView, WalletRoot walletRoot)
        {
            _characterInput = characterInput;
            _projectConfigLoader = projectConfigLoader;
            _staminaHandler = staminaHandler;
            _cameraRotator = cameraRotator;
            _itemBuilder = itemBuilder;
            _mapView = mapView;
            _wallerRoot = walletRoot;
        }

        public void Initialize()
        {
            _characterInput.UI.ToggleInventoryVisibility.performed += ctx => ToggleVisibility();

            BuildGrid();

            _canvas.enabled = false;
        }

        private void OnDestroy()
        {
            _characterInput.UI.ToggleInventoryVisibility.performed -= ctx => ToggleVisibility();
        }

        private void BuildGrid()
        {
            _currentGrid = new InventoryCell[Width, Height];

            Debug.Log("Heigh is " + Height);
            Debug.Log("Width is " + Width);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var cell = Instantiate(_cellPrefab, _gridContent);
                    cell.Initialize(x, y);
                    _currentGrid[x, y] = cell;
                    _allCells.Add(cell);

                }
            }
        }

        public bool TryAddItem(ItemConfig itemConfig)
        {
            Debug.Log("Try To Add " + itemConfig.TitleLid);

            if (itemConfig == null)
            {
                Debug.LogWarning("ItemConfig is null, cannot add item.");

                return false;
            }

            var item = _itemBuilder.Build(itemConfig, _rewardsPanel);
            _unUsedItems.Add(item);

            return true;
        }

        public void ToggleVisibility()
        {
            if (_staminaHandler.IsStarted == true || _mapView.IsVisible == true)
                return;

            _canvas.enabled = !_canvas.enabled;

            if (_canvas.enabled == true)
            {
                OnBuildCells?.Invoke(_allCells);
                _cameraRotator.Disable();
            }
            else
            {
                _cameraRotator.Enable();
            }
        }

        public void HightligchCells(ItemRoot itemRoot)
        {
            ProcessItemPlacement(itemRoot, true);
        }

        public bool TryPlaceItem(ItemRoot itemRoot)
        {
            bool isPlaced = ProcessItemPlacement(itemRoot, false);

            if (isPlaced == true)
            {
                if (_storingFishes.Contains(itemRoot) == false)
                    _storingFishes.Add(itemRoot);

                _unUsedItems.Remove(itemRoot);
            }
            else
            {
                if (_unUsedItems.Contains(itemRoot) == false)
                    _unUsedItems.Add(itemRoot);

                if (_storingFishes.Contains(itemRoot) == true)
                    _storingFishes.Remove(itemRoot);
            }

            return isPlaced;
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

        public void SellAllFishes()
        {
            foreach (var item in _storingFishes)
            {
                if (item == null)
                    break;

                var fishPrice = item.Config.Price;

                _wallerRoot.Money.TryAdd(fishPrice);

                item.Remove();
            }

            _storingFishes.Clear();
        }

        public void SellFish()
        {
            if (_selectedItem == null)
                return;

            var fishPrice = _selectedItem.Config.Price;

            _wallerRoot.Money.TryAdd(fishPrice);

            _selectedItem.Remove();

            _storingFishes.Remove(_selectedItem);
        }

        public void RotateItem()
        {
            if (_selectedItem == null)
                return;

            _selectedItem.Rotate();
        }

        public void DeleteItem()
        {
            if (_selectedItem == null)
                return;


            _selectedItem.Delete(_rewardsPanel);
        }

        public void ConfirmItem()
        {
            if (_selectedItem == null)
                return;

            _selectedItem.Confirm();
        }

        public void RemoveItem()
        {
            if (_selectedItem == null)
                return;

            _selectedItem.Remove();
        }

        public void MoveItemToRewardsPanel()
        {
            if (_selectedItem == null)
                return;

            _selectedItem.Delete(_rewardsPanel);
        }

        private bool ProcessItemPlacement(ItemRoot itemRoot, bool highlightOnly)
        {
            itemRoot.ClearOccupiedInventoryCells();
            float radius = CellSize;

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

        public void ShowInventoryAfterMiniGame()
        {
            Debug.Log("Try To Remove Unused items");

            foreach (var item in _unUsedItems)
                item.Remove();

            _unUsedItems.Clear();

            ToggleVisibility();
        }

        private InventoryCell FindNearestCell(Vector3 worldPos, float radius)
        {
            InventoryCell closest = null;
            float closestDistance = float.MaxValue;

            foreach (var cell in GetAllCells())
            {
                float distance = Vector2.Distance(new Vector2(worldPos.x, worldPos.y), new Vector2(cell.transform.position.x, cell.transform.position.y));

                if (distance <= radius && distance < closestDistance)
                {
                    closest = cell;
                    closestDistance = distance;
                }
            }

            return closest;
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
