using Sources.Runtime.Gameplay.Configs.Items;
using Sources.Runtime.Gameplay.Inventory.Item;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sources.Runtime.Services.Builders.Item
{
    public class ItemBuilder : IItemBuilder
    {
        private readonly IObjectResolver _objectResolver;
        private readonly ItemRoot _prefab;

        [Inject]
        public ItemBuilder(IObjectResolver objectResolver, ItemRoot prefab)
        {
            _objectResolver = objectResolver;
            _prefab = prefab;
        }

        public ItemRoot Build(ItemConfig config, Transform container)
        {
            var createdItem = _objectResolver.Instantiate(_prefab, container);

            createdItem.Initialize(config);

            return createdItem;
        }
    }
}
