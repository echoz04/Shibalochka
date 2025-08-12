using Sources.Runtime.Gameplay.Inventory.Items;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private Transform _itemsContainer;
        
        private InventoryRoot _root;
        
        public void Initialize(InventoryRoot root)
        {
            _root = root;

            _root.OnItemAdded += AddItemToContainer;
            _root.OnItemRemoved += RemoveItemFromContainer;
        }

        private void OnDestroy()
        {
            _root.OnItemAdded -= AddItemToContainer;
            _root.OnItemRemoved -= RemoveItemFromContainer;
        }

        private void AddItemToContainer(ItemViewRoot itemViewRoot)
        {
            itemViewRoot.transform.SetParent(_itemsContainer);
        }

        private void RemoveItemFromContainer(ItemViewRoot itemViewRoot)
        {
            itemViewRoot.transform.SetParent(null);
        }
    }
}