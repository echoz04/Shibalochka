using UnityEngine;
using UnityEngine.UI;

namespace Sources.Runtime.Gameplay.Inventory
{
    public class InventoryCell : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _lockSprite, _unlockSprite;

        private void OnValidate()
        {
            _image ??= GetComponent<Image>();
        }

        public void SetLockState()
        {
            _image.sprite = _lockSprite;
        }

        public void SetUnlockState()
        {
            _image.sprite = _unlockSprite;
        }
    }
}