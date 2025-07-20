using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Sources.Runtime.Gameplay.MiniGames.Fishing
{
    public class FishingMiniGameProgressView : MonoBehaviour
    {
        public float Value
        {
            get => _value;
            set
            {
                _value = Mathf.Clamp(value, 0, 100);

                _slider.value = _value;

                // Отменяем прошлый твин, если есть
                _delayedSlider.DOKill();

                // Запускаем отставание
                DOVirtual.DelayedCall(_delay, () =>
                {
                    _delayedSlider.DOValue(_value, _delayDuration).SetEase(Ease.OutQuad);
                });

                _text.text = $"{Mathf.RoundToInt(_value)}%";
            }
        }

        [SerializeField] private Slider _slider;
        [SerializeField] private Slider _delayedSlider;
        [SerializeField] private TextMeshProUGUI _text;

        [SerializeField] private float _delay = 0.1f; 
        [SerializeField] private float _delayDuration = 0.5f;

        private float _value;

        private void OnValidate()
        {
            _slider ??= GetComponentInChildren<Slider>();
            _text ??= GetComponentInChildren<TextMeshProUGUI>();

            if (_delayedSlider == null)
            {
                var sliders = GetComponentsInChildren<Slider>();
                if (sliders.Length > 1)
                    _delayedSlider = sliders[1]; // автонастройка второго слайдера
            }
        }

        public void AddValue(float newValue)
        {
            Value += newValue;
            RuntimeManager.StudioSystem.setParameterByName("FishingMiniGameProgress", Value);
            RuntimeManager.PlayOneShot("event:/SFX/MiniGames/MG_Success");
        }

        public void RemoveValue(float newValue)
        {
            Value -= newValue;
            RuntimeManager.StudioSystem.setParameterByName("FishingMiniGameProgress", Value);
            RuntimeManager.PlayOneShot("event:/SFX/MiniGames/MG_Failure");
        }

        public void SetValue(float newValue)
        {
            Value = newValue;
        }
    }
}
