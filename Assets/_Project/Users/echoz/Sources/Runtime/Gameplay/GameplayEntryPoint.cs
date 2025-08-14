using Sirenix.OdinInspector;
using Sources.Runtime.Gameplay.Inventory;
using Sources.Runtime.Gameplay.Inventory.Items;
using Sources.Runtime.Services;
using UnityEngine;

namespace Sources._Project.Users.echoz.Sources.Runtime.Gameplay
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        [Header("Imporatant Fields To Initialize Systems")]
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private InventoryCell _cellPrefab;
        [SerializeField] private Transform _cellsContainer;
        [SerializeField] private ItemViewRoot _itemPrefab;
        
        [Header("Inventory Facade")]
        [SerializeField] private BaseItemConfig _itemConfig;
        [SerializeField] private ItemsConfig _itemsConfig;
        
        private IItemBuilder _itemBuilder;
        private InventoryFacade _inventoryFacade;
        private InventoryRoot _inventoryRoot;

        private void Awake()
        {
            RegisterInventory();
            RegisterItemBuilder();
            RegisterInventoryFacade();

            _inventoryView.Initialize(_inventoryRoot);
            
            _inventoryRoot.CreateCells(_cellPrefab, _cellsContainer);
            
            _inventoryFacade.BuildItemWithConfig(_itemConfig);
            _inventoryFacade.BuildFishWithRandomConfig();
            _inventoryFacade.BuildFishWithRandomConfig();
            _inventoryFacade.BuildFishWithRandomConfig();
        }

        private void RegisterInventory()
        {
            _inventoryRoot = new InventoryRoot();
        }
        
        private void RegisterItemBuilder()
        {
            _itemBuilder = new ItemBuilder(_itemPrefab);
        }

        private void RegisterInventoryFacade()
        {
            _inventoryFacade = new(_inventoryRoot, _itemBuilder, _itemsConfig);
        }
    }
}