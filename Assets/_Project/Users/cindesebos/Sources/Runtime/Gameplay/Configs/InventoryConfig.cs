using UnityEngine;
using Sirenix.OdinInspector;

namespace Sources.Runtime.Gameplay.Configs
{
    [System.Serializable]
    public class InventoryConfig
    {
        [Title("Inventory UI Settings")]

        [SerializeField, MinValue(1), MaxValue(20), LabelText("Inventory Width")] private int _inventoryWidth;
        public int InventoryWidth => _inventoryWidth;

        [SerializeField, MinValue(1), MaxValue(20), LabelText("Inventory Height")] private int _inventoryHeight;
        public int InventoryHeigth => _inventoryHeight;

        [SerializeField, MinValue(10), MaxValue(200), LabelText("Cell Size (px)")] private float _cellSize;
        public float InventoryCellSize => _cellSize;

        [SerializeField, MinValue(0), MaxValue(50), LabelText("Cell Spacing (px)")] private float _spacing;
        public float InventorySpacing => _spacing;

        [SerializeField, MinValue(0), MaxValue(50), LabelText("Cells Spawn Animation Duration")] private float _cellsSpawnAnimationDuration;
        public float CellsSpawnAnimationDuration => _cellsSpawnAnimationDuration;

        [SerializeField, MinValue(0), MaxValue(50), LabelText("Delay Between Cells Spawn Animation")] private float _delayBetweenCellsSpawnAnimation;
        public float DelayBetweenCellsSpawnAnimation => 100 * _delayBetweenCellsSpawnAnimation;

        [Title("Items UI Settings")]

        [SerializeField, MinValue(0), MaxValue(5), LabelText("Item Dragging Animations Duration")] private float _itemAnimationsDuration;
        public float ItemAnimationsDuration => _itemAnimationsDuration;

        [SerializeField, MinValue(0), MaxValue(2), LabelText("Item Scale While Dragging")] private float _itemDraggingScale;
        public float ItemDraggingScale => _itemDraggingScale;
    }
}
