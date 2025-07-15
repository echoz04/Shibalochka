using System.Collections.Generic;
using Sources.Runtime.Gameplay.Configs;
using Sources.Runtime.Services.ProjectConfigLoader;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Inventory
{
    public class InventoryService
    {
        public int Width => _config.InventoryWidth;
        public int Height => _config.InventoryHeigth;
        public float CellSize => _config.InventoryCellSize;

        private InventoryCell _cellPrefab;
        private InventoryConfig _config;
        private RectTransform _gridRoot;

        private InventoryCell[,] _currentGrid;

        public InventoryService(InventoryCell cellPrefab, RectTransform gridRoot)
        {
            _cellPrefab = cellPrefab;
            _gridRoot = gridRoot;
        }

        public void SetConfig(IProjectConfigLoader projectConfigLoader) => _config = projectConfigLoader.ProjectConfig.InventoryConfig;

        public void BuildGrid()
        {
            _currentGrid = new InventoryCell[Width, Height];

            _gridRoot.sizeDelta = new Vector2(Width * CellSize, Height * CellSize);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var cell = GameObject.Instantiate(_cellPrefab, _gridRoot);

                    cell.Initialize(x, y, this, CellSize);

                    _currentGrid[x, y] = cell;
                }
            }
        }

        public bool CanPlaceItem(ItemConfig itemConfig, Vector2Int basePosition, bool rotated, out List<InventoryCell> cells)
        {
            cells = new List<InventoryCell>();

            foreach (var offset in itemConfig.CellOffsets)
            {
                Vector2Int offSetPosition = rotated ? new Vector2Int(-offset.y, offset.x) : offset;

                Vector2Int newPosition = basePosition + offSetPosition;

                if (newPosition.x < 0 || newPosition.y < 0 || newPosition.x >= Width || newPosition.y >= Height)
                    return false;

                var cell = _currentGrid[newPosition.x, newPosition.y];

                if (cell.IsOccupied == true)
                    return false;

                cells.Add(cell);
            }

            return true;
        }

        public bool TryPlaceItem(InventoryItem item)
        {
            if (item.HasValidHover == false || CanPlaceItem(item.Config, item.LastHoverPosition, item.IsRotated, out var cells))
            {
                item.CancelPlacement();

                return true;
            }

            TryRemoveItem(item);

            foreach (var cell in cells)
                cell.SetOccupied(item);

            item.OnPlaced(cells);

            return false;
        }

        public bool TryRemoveItem(InventoryItem item)
        {
            if (item.CurrentCells != null)
            {
                foreach (var cell in item.CurrentCells)
                    cell.ClearOccupied();

                item.CurrentCells.Clear();

                return true;
            }

            return false;
        }

        public void HighlightCells(ItemConfig itemConfig, Vector2Int basePosition, bool rotated)
        {
            ClearHighlight();

            if (CanPlaceItem(itemConfig, basePosition, rotated, out var cells))
            {
                foreach (var cell in cells)
                    cell.SetHighlight(true);
            }
        }

        public void ClearHighlight()
        {
            foreach (var grid in _currentGrid)
                grid.SetHighlight(false);
        }
    }
}