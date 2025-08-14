using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sources.Runtime.Gameplay.Inventory.Items
{
    public class ItemViewRoot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _image;
        
        private BaseItemConfig _config;

        private void OnValidate()
        {
            _image ??= GetComponentInChildren<Image>();
        }

        public void Initialize(BaseItemConfig config)
        {
            _config = config;
            
            _image.sprite = _config.Sprite;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ItemInfoDisplayer.Instance.Display(_config);
        }
    }
}