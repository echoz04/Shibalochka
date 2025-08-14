using UnityEngine;

namespace Sources.Runtime.Gameplay.Inventory.Items
{
    public class BaseItemConfig : ScriptableObject
    {
        [field: SerializeField] public string Title { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
    }
}
