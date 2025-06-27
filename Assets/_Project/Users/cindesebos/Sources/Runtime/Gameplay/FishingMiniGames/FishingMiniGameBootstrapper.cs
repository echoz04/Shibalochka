using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Runtime.Gameplay.FishingMiniGames
{
    public class FishingMiniGameBootstrapper : MonoBehaviour
    {
        private const float InitialProgressValue = 0;
        private const float InitialSliderValue = 50;
        [SerializeField] private FishingMiniGameView _view;
        [Space]

        [SerializeField] private Slider _progressSlider;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private Slider _pointerSlider;

        private IFishingMiniGame _currentMiniGame;

        private void Start()
        {
            Initialize();

            Launch();
        }

        public void Initialize()
        {
            _progressSlider.value = InitialProgressValue;
            _progressText.text = $"{Mathf.RoundToInt(InitialProgressValue)}%";
            _pointerSlider.value = InitialSliderValue;

            _currentMiniGame = new MovablePointerFishingMiniGame(_progressSlider, _progressText, _pointerSlider);
        }

        public async UniTask Launch()
        {
            await _view.OnShow();

            _currentMiniGame.Launch();
        }
    }
}