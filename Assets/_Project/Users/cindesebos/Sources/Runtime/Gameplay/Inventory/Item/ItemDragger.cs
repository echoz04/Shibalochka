using UnityEngine;
using UnityEngine.InputSystem;

namespace Sources.Runtime.Gameplay.Inventory.Item
{
    public class ItemDragger : MonoBehaviour
    {
        private InventoryRoot _inventoryRoot;
        private ItemRoot _root;

        public void Initialize(InventoryRoot inventoryRoot, ItemRoot root)
        {
            _inventoryRoot = inventoryRoot;
            _root = root;
        }

        public void BeginDrag()
        {
            // Optional: Add drag start logic
        }

        public void Drag()
        {
            transform.position = Mouse.current.position.ReadValue();
        }

        public void EndDrag()
        {
        }

        public void PointClick()
        {
            // Optional: Add click logic
        }
    }
}
