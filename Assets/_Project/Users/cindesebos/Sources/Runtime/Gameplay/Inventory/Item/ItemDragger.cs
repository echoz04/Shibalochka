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

            _root.OnDragging += Drag;
        }

        private void OnDestroy()
        {
            _root.OnDragging -= Drag;
        }

        public void Drag()
        {
            transform.position = Mouse.current.position.ReadValue();
        }
    }
}
