using UnityEngine;

namespace Sources.Runtime.Gameplay.Inventory.Items
{
    public class BaseItemConfig : ScriptableObject
    {
        [field: SerializeField] public string Title { get; private set; }
    }
}
