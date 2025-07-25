using Sources.Runtime.Gameplay.Configs.Items;
using Sources.Runtime.Gameplay.Inventory.Item;
using UnityEngine;

namespace Sources.Runtime.Services.Builders.Item
{
    public interface IItemBuilder
    {
        ItemRoot Build(ItemConfig config, Transform container);
    }
}
