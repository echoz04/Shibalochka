using UnityEngine;

namespace Sources.Runtime.Gameplay.Inventory.Item
{
    public class ItemCellPoint : MonoBehaviour
    {
        public Transform Transform => transform;

        public bool IsPlaced { get; set; }
    }
}
