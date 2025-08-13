using Sources.Runtime.Gameplay.Inventory.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Runtime.Gameplay.Inventory
{
    public class InventoryCell : MonoBehaviour
    {
        public bool IsFree { get; private set; }
        public ItemViewRoot StoredItemViewRoot { get; private set; }
        
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _lockSprite, _unlockSprite;

        private void OnValidate()
        {
            _image ??= GetComponent<Image>();
        }

        public void SetLockState(ItemViewRoot itemViewRoot = null)
        {
            StoredItemViewRoot = itemViewRoot;
            IsFree = false;
            _image.sprite = _lockSprite;
        }

        public void SetUnlockState()
        {
            StoredItemViewRoot = null;
            IsFree = true;
            _image.sprite = _unlockSprite;
        }
    }
}