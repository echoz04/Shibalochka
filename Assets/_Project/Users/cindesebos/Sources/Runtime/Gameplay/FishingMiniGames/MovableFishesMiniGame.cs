using System;
using TMPro;
using UnityEngine.UI;

namespace Sources.Runtime.Gameplay.FishingMiniGames
{
    public class MovableFishesMiniGame : IFishingMiniGame
    {
        public event Action OnLaunched;
        public event Action OnEnded;

        private readonly Slider _progressSlider;
        private readonly TextMeshProUGUI _progressText;

        public MovableFishesMiniGame(Slider progressSlider, TextMeshProUGUI progressText)
        {
            _progressSlider = progressSlider;
            _progressText = progressText;
        }

        public void Launch()
        {
            OnLaunched?.Invoke();
        }

        public void End()
        {
            OnEnded?.Invoke();
        }
    }
}