using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Runtime.Gameplay.Inventory.Items
{
    public class ItemInfoDisplayer : MonoBehaviour
    {
        public static ItemInfoDisplayer Instance { get; private set; }

        [SerializeField] private GameObject _infoPanel;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Image _itemImage;
        
        private BaseItemConfig _activeCofig;

        private void Awake()
        {
            Instance = this;
        }

        public void Display(BaseItemConfig config)
        {
            if (_activeCofig == config)
            {
                _infoPanel.SetActive(false);
                _activeCofig = null;
                
                return;
            }
            
            _activeCofig = config;

            InitializeInfoPanel();
        }

        private void InitializeInfoPanel()
        {
            _infoPanel.SetActive(true);
            _titleText.text = _activeCofig.Title;
            _descriptionText.text = _activeCofig.Description;
            _itemImage.sprite = _activeCofig.Sprite;
        }
    }
}