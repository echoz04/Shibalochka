using System;
using System.Collections.Generic;
using Sources.Runtime.Gameplay.Inventory.Items;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Inventory
{
    public class InventoryRoot
    {
        public event Action<InventoryCell[,]> OnCellsCreated;
        public event Action<ItemViewRoot, InventoryCell> OnItemAdded;
        public event Action<ItemViewRoot> OnItemRemoved;

        private const int _width = 8;
        private const int _height = 7;
        private const int _freeCellsHeight = 4;
        
        private List<ItemViewRoot> _storedItems = new();
        
        private InventoryCell[,] _cells;

        public void CreateCells(InventoryCell cellPrefab, Transform cellsContainer)
        {
            _cells = new InventoryCell[_width, _height];

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    var instance = GameObject.Instantiate(cellPrefab, cellsContainer);
                    instance.ToggleImage(false);
                    
                    if(y < _freeCellsHeight)
                        instance.SetUnlockState();
                    else
                        instance.SetLockState();
                    
                    _cells[x, y] = instance;
                }
            }
            
            OnCellsCreated?.Invoke(_cells);
        }
        
        public void AddItem(ItemViewRoot itemPrefab)
        {
            if(itemPrefab == null)
                return;

            var firstFreeCell = FindFirstFreeCell();
            
            firstFreeCell.SetLockState(itemPrefab);
            OnItemAdded?.Invoke(itemPrefab, firstFreeCell);
            _storedItems.Add(itemPrefab);
        }

        public void RemoveItem(InventoryCell targetCell, ItemViewRoot itemViewRoot)
        {
            var item = FindStoredItem(itemViewRoot);
            
            if(itemViewRoot == null || item == null)
                return;
            
            targetCell.SetUnlockState();
            OnItemRemoved?.Invoke(item);
            _storedItems.Remove(item);
        }

        private InventoryCell FindFirstFreeCell()
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    var targetCell = _cells[x, y];
                    
                    if (targetCell.IsFree == true)
                        return targetCell;
                }
            }

            return null;
        }

        private ItemViewRoot FindStoredItem(ItemViewRoot itemViewRoot)
        {
            for (int i = 0; i < _storedItems.Count; i++)
            {
                if(_storedItems[i] == itemViewRoot)
                    return _storedItems[i];
            }
            
            return null;
        }
    }
}
