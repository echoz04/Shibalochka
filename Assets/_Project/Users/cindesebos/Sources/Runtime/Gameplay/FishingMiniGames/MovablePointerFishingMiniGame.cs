using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Runtime.Gameplay.FishingMiniGames
{
    public class MovablePointerFishingMiniGame : IFishingMiniGame
    {
        private const int MinPointerValue = 0;
        private const int MaxPointerValue = 100;
        private const float MoveDuration = 2.45f;

        public event Action OnLaunched;
        public event Action OnEnded;

        private readonly Slider _progressSlider;
        private readonly TextMeshProUGUI _progressText;
        private readonly Slider _pointerSlider;

        private bool _isWorking;
        private Tween _pointerTween;

        public MovablePointerFishingMiniGame(Slider progressSlider, TextMeshProUGUI progressText, Slider pointerSlider)
        {
            _progressSlider = progressSlider;
            _progressText = progressText;
            _pointerSlider = pointerSlider;
        }

        public void Launch()
        {
            _isWorking = true;

            OnLaunched?.Invoke();

            MovePointer();
        }

        private void MovePointer()
        {
            Tween moveToMax = DOTween.To(
                () => _pointerSlider.value,
                value =>
                {
                    _pointerSlider.value = value;
                },
                MaxPointerValue,
                MoveDuration / 2f
            ).SetEase(Ease.InOutSine);

            Tween loopTween = DOTween.To(
                () => _pointerSlider.value,
                value =>
                {
                    _pointerSlider.value = value;
                },
                MinPointerValue,
                MoveDuration
            ).SetEase(Ease.InOutSine)
             .SetLoops(-1, LoopType.Yoyo);

            _pointerTween = DOTween.Sequence()
                .Append(moveToMax)
                .Append(loopTween);
        }

        public void End()
        {
            _pointerTween?.Kill();

            OnEnded?.Invoke();
        }
    }
}