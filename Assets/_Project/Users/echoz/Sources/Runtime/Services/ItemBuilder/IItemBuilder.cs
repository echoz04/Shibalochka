using Sources.Runtime.Gameplay.Inventory.Items;
using UnityEngine;

namespace Sources.Runtime.Services
{
    public interface IItemBuilder
    {
        ItemViewRoot Build(BaseItemConfig config);
    }
}