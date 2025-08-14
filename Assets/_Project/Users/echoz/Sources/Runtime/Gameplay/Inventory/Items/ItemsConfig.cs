using System.Collections.Generic;
using Sources.Runtime.Gameplay.Inventory.Items.Types;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Inventory.Items
{
    [CreateAssetMenu(fileName = "ItemsConfig", menuName = "Configs/Items/New Items Config")]
    public class ItemsConfig : ScriptableObject
    {
        [field: SerializeField] public FishesConfig AllFishesConfig { get; private set; }
    }
}