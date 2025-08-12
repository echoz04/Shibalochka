using System;
using System.Collections.Generic;
using Sources.Runtime.Gameplay.Inventory.Items;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Inventory
{
    public class InventoryRoot
    {
        public event Action<ItemViewRoot> OnItemAdded;
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
                    
                    if(y < _freeCellsHeight)
                        instance.SetUnlockState();
                    else
                        instance.SetLockState();
                    
                    _cells[x, y] = instance;
                }
            }
        }
        
        public void AddItem(ItemViewRoot itemViewRoot)
        {
            if(itemViewRoot == null)
                return;
            
            OnItemAdded?.Invoke(itemViewRoot);
            _storedItems.Add(itemViewRoot);
        }

        public void RemoveItem(ItemViewRoot itemViewRoot)
        {
            if(itemViewRoot == null || _storedItems.Contains(itemViewRoot) == false)
                return;
            
            OnItemRemoved?.Invoke(itemViewRoot);
            _storedItems.Remove(itemViewRoot);
        }
    }
}
