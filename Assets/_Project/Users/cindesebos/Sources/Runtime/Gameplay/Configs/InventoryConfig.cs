using UnityEngine;

namespace Sources.Runtime.Gameplay.Configs
{
    [System.Serializable]
    public class InventoryConfig
    {
        [Header("Inventory UI")]
        [field: SerializeField] public int InventoryWidth { get; private set; } = 5;
        [field: SerializeField] public int InventoryHeigth { get; private set; } = 5;
        [field: SerializeField] public float InventoryCellSize { get; private set; } = 50f;
    }
}