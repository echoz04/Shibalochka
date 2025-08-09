using Sources.Runtime.Services.SceneLoader;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sources
{
    public class BaseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _sourceImage;
        [SerializeField] private Sprite _selectedSprite, _unSelectedSprite;
        [SerializeField] private TextMeshProUGUI _text;

        private Color _selectedColor;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _sourceImage.sprite = _selectedSprite;

            if (ColorUtility.TryParseHtmlString("#FFE0A7", out Color color))
                _text.color = color;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _sourceImage.sprite = _unSelectedSprite;

            if (ColorUtility.TryParseHtmlString("#542C00", out Color color))
                _text.color = color;
        }
    }
}
