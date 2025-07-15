using UnityEngine;

namespace Sources.Runtime.Gameplay.Inventory
{
    [CreateAssetMenu(fileName = "Fish Config", menuName = "Configs/New Item Config")]
    public class ItemConfig : ScriptableObject
    {
        [field: SerializeField] public string Title { get; private set; }
        [field: SerializeField] public Vector2Int[] CellOffsets { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}
