using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.Runtime.Services.ProjectConfigLoader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Runtime.Gameplay.FishingMiniGames
{
    public class MovablePointerFishingMiniGame : IFishingMiniGame
    {
        private const int MinPointerValue = 0;
        private const int MaxPointerValue = 100;

        public event Action OnLaunched;
        public event Action OnEnded;

        private readonly Slider _progressSlider;
        private readonly TextMeshProUGUI _progressText;
        private readonly Slider _pointerSlider;
        private readonly CharacterInput _characterInput;
        private readonly IProjectConfigLoader _projectConfigLoader;

        private Tween _pointerTween;

        public MovablePointerFishingMiniGame(Slider progressSlider, TextMeshProUGUI progressText, Slider pointerSlider, CharacterInput characterInput, IProjectConfigLoader projectConfigLoader)
        {
            _progressSlider = progressSlider;
            _progressText = progressText;
            _pointerSlider = pointerSlider;
            _characterInput = characterInput;
            _projectConfigLoader = projectConfigLoader;
        }

        public void Launch()
        {
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
                _projectConfigLoader.ProjectConfig.UIConfig.FishingPointerMoveDuration / 2f
            ).SetEase(Ease.InOutSine);

            Tween loopTween = DOTween.To(
                () => _pointerSlider.value,
                value =>
                {
                    _pointerSlider.value = value;
                },
                MinPointerValue,
                _projectConfigLoader.ProjectConfig.UIConfig.FishingPointerMoveDuration
            ).SetEase(Ease.InOutSine)
             .SetLoops(-1, LoopType.Yoyo);

            _pointerTween = DOTween.Sequence()
                .Append(moveToMax)
                .Append(loopTween);
        }

        public void End()
        {
            OnEnded?.Invoke();

            if (_pointerTween == null)
                return;

            _pointerTween.Kill();
            _pointerTween = null;
        }
    }
}