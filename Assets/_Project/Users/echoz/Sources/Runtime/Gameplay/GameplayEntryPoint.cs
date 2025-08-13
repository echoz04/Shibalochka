using Sirenix.OdinInspector;
using Sources.Runtime.Gameplay.Inventory;
using Sources.Runtime.Gameplay.Inventory.Items;
using UnityEngine;

namespace Sources._Project.Users.echoz.Sources.Runtime.Gameplay
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private InventoryCell _cellPrefab;
        [SerializeField] private Transform _cellsContainer;
        [SerializeField] private ItemViewRoot _itemPrefab;
        
        private InventoryRoot _inventoryRoot;

        private void Awake()
        {
            _inventoryRoot = new InventoryRoot();
            _inventoryRoot.CreateCells(_cellPrefab, _cellsContainer);
            
            _inventoryView.Initialize(_inventoryRoot);
            
            _inventoryRoot.AddItem(_itemPrefab);
        }
    }
}