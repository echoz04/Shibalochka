using Sources.Runtime.Gameplay.Inventory;
using Sources.Runtime.Gameplay.Inventory.Items;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sources.Runtime.Services
{
    public class ItemBuilder : IItemBuilder
    {
        private readonly IObjectResolver _resolver;
        private readonly ItemViewRoot _itemPrefab;

        public ItemBuilder(ItemViewRoot itemPrefab)
        {
            _itemPrefab = itemPrefab;
        }
        
        public ItemViewRoot Build(BaseItemConfig config)
        {
            var insntance = GameObject.Instantiate(_itemPrefab);
            
            insntance.Initialize(config);

            return insntance;
        }
    }
}