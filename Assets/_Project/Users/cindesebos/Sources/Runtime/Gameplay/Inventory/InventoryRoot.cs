using System.Collections.Generic;
using Sources.Runtime.Gameplay.Configs;
using Sources.Runtime.Services.ProjectConfigLoader;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Gameplay.Inventory
{
    public class InventoryRoot : MonoBehaviour
    {
        [field: SerializeField] public RectTransform ItemsContainer { get; private set; }

        public int Width => _config.InventoryWidth;
        public int Height => _config.InventoryHeigth;
        public float CellSize => _config.InventoryCellSize;
        public float Spacing => _config.InventorySpacing;

        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform _draggingContainer;
        [SerializeField] private InventoryCell _cellPrefab;

        private InventoryConfig _config;
        private InventoryCell[,] _currentGrid;
        [SerializeField] private List<InventoryItem> _currentItems;

        [Inject]
        private void Construct(IProjectConfigLoader projectConfigLoader)
        {
            _config = projectConfigLoader.ProjectConfig.InventoryConfig;
        }

        public void BuildGrid()
        {
            _currentGrid = new InventoryCell[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var cell = Instantiate(_cellPrefab, _content);
                    cell.Initialize(x, y, this, CellSize);
                    _currentGrid[x, y] = cell;
                }
            }
        }

        public void StartDraggingItem(InventoryItem item)
        {
            foreach (var currentItem in _currentItems)
            {
                if (currentItem == item)
                    currentItem.SetRaycastValue(true);
                else
                    currentItem.SetRaycastValue(false);
            }
        }

        public void EndDraggingItem()
        {
            foreach (var currentItem in _currentItems)
                currentItem.SetRaycastValue(true);
        }

        public bool TryPlaceItem(ItemConfig config, Vector2Int basePos, bool rotated, out List<InventoryCell> cells)
        {
            cells = new List<InventoryCell>();

            foreach (var offset in config.CellOffSets)
            {
                Vector2Int off = rotated ? new Vector2Int(-offset.y, offset.x) : offset;
                Vector2Int pos = basePos + off;

                if (pos.x < 0 || pos.y < 0 || pos.x >= Width || pos.y >= Height)
                    return false;

                var cell = _currentGrid[pos.x, pos.y];
                if (cell.IsOccupied)
                    return false;

                cells.Add(cell);
            }

            return true;
        }

        public bool TryRemoveItem(InventoryItem item)
        {
            if (item.CurrentCells == null || item.CurrentCells.Count == 0)
                return false;

            foreach (var cell in item.CurrentCells)
                cell.ClearOccupied();

            item.CurrentCells.Clear();
            _currentItems.Remove(item);
            return true;
        }

        public void HighlightCells(ItemConfig config, Vector2Int basePos, bool rotated)
        {
            ClearHighlight();

            if (TryPlaceItem(config, basePos, rotated, out var cells))
            {
                foreach (var cell in cells)
                    cell.SetHighlight(true);
            }
        }

        public void AddItem(InventoryItem item) =>
            _currentItems.Add(item);

        public void RemoveItem(InventoryItem item) =>
            _currentItems.Remove(item);

        public void MoveToDraggingContainer(InventoryItem item)
        {
            item.transform.SetParent(_draggingContainer);
        }

        public void ClearHighlight()
        {
            foreach (var cell in _currentGrid)
                cell.SetHighlight(false);
        }
    }
}