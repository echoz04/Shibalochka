using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Sources.Runtime.Gameplay.Inventory.Items
{
    public class ItemInfoDisplayer : MonoBehaviour
    {
        private const float _showDuration = 0.25f;
        private const float _hideDuration = 0.2f;
        private const float _showScale = 1f;
        private const float _hideScale = 0.8f;
        
        public static ItemInfoDisplayer Instance { get; private set; }

        [SerializeField] private GameObject _infoPanel;
        [SerializeField] private CanvasGroup _infoPanelCanvasGroup;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Image _itemImage;
        
        private BaseItemConfig _activeConfig;
        private Tween _currentTween;

        private void Awake()
        {
            Instance = this;
            _infoPanel.SetActive(false);
        }

        public void Display(BaseItemConfig config)
        {
            if (_activeConfig == config)
            {
                HidePanel();
                _activeConfig = null;
                return;
            }

            _activeConfig = config;
            InitializeInfoPanel();
            ShowPanel();
        }

        private void InitializeInfoPanel()
        {
            _infoPanel.SetActive(true);
            _titleText.text = _activeConfig.Title;
            _descriptionText.text = _activeConfig.Description;
            _itemImage.sprite = _activeConfig.Sprite;
        }

        private void ShowPanel()
        {
            _infoPanelCanvasGroup.alpha = 0f;
            _infoPanel.transform.localScale = Vector3.one * _hideScale;

            _currentTween?.Kill();
            _currentTween = DOTween.Sequence()
                .Join(_infoPanelCanvasGroup.DOFade(1f, _showDuration))
                .Join(_infoPanel.transform.DOScale(_showScale, _showDuration).SetEase(Ease.OutBack));
        }

        private void HidePanel()
        {
            _currentTween?.Kill();
            _currentTween = DOTween.Sequence()
                .Join(_infoPanelCanvasGroup.DOFade(0f, _hideDuration))
                .Join(_infoPanel.transform.DOScale(_hideScale, _hideDuration).SetEase(Ease.InBack))
                .OnComplete(() => _infoPanel.SetActive(false));
        }
        
    }
}