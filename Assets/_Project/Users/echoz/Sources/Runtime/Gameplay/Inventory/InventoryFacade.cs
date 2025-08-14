using Sources.Runtime.Gameplay.Inventory.Items;
using Sources.Runtime.Services;
using System.Linq;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Inventory
{
    public class InventoryFacade
    {
        private readonly InventoryRoot _inventoryRoot;
        private readonly IItemBuilder _itemBuilder;
        private readonly ItemsConfig _itemsConfig;

        public InventoryFacade(InventoryRoot inventoryRoot, IItemBuilder itemBuilder, ItemsConfig itemsConfig)
        {
            _inventoryRoot = inventoryRoot;
            _itemBuilder = itemBuilder;
            _itemsConfig = itemsConfig;
        }

        public void BuildItemWithConfig(BaseItemConfig config)
        {
            var instncae = _itemBuilder.Build(config);

            _inventoryRoot.AddItem(instncae);
        }

        public void BuildFishWithRandomConfig()
        {
            var configs = _itemsConfig.AllFishesConfig.AllConfigs;

            var config = configs.OrderBy(x => UnityEngine.Random.value).FirstOrDefault();

            if (config == null)
                return;

            var instncae = _itemBuilder.Build(config);

            _inventoryRoot.AddItem(instncae);
        }
    }
}