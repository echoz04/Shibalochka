using Sources.Runtime.Gameplay.Configs.Items;
using Sources.Runtime.Gameplay.Inventory.Item;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Services.Builders.Item
{
    public class ItemBuilder : IItemBuilder
    {
        private readonly DiContainer _diContainer;
        private readonly ItemRoot _prefab;

        public ItemBuilder(DiContainer diContainer, ItemRoot prefab)
        {
            _diContainer = diContainer;
            _prefab = prefab;
        }

        public ItemRoot Build(ItemConfig config, Transform container)
        {
            var createdItem = _diContainer.InstantiatePrefabForComponent<ItemRoot>(_prefab, container);

            createdItem.Initialize(config);

            return createdItem;
        }
    }
}
