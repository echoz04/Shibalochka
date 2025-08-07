using DG.Tweening;
using UnityEngine.UI;
using FMOD.Studio;
using Sources.Runtime.Gameplay.Configs;

namespace Sources.Runtime.Gameplay.MiniGames.Fishing.Types
{
    public class MovablePointerFishingMiniGame : IMiniGame
    {
        private const int MinPointerValue = 0;
        private const int MaxPointerValue = 100;

        private readonly Slider _pointerSlider;
        private readonly ProjectConfig _projectConfig;

        private Tween _pointerTween;

        private EventInstance _loopedSound;

        public MovablePointerFishingMiniGame(Slider pointerSlider, ProjectConfig projectConfig)
        {
            _pointerSlider = pointerSlider;
            _projectConfig = projectConfig;
        }

        public void Launch()
        {
            //_loopedSound = RuntimeManager.CreateInstance("event:/SFX/GameSFX/Fishing_Reel");
            //_loopedSound.start();
            //_loopedSound.setParameterByName("FishingMiniGame", 1);

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
                _projectConfig.UIConfig.FishingPointerMoveDuration / 2f
            ).SetEase(Ease.InOutSine);

            Tween loopTween = DOTween.To(
                () => _pointerSlider.value,
                value =>
                {
                    _pointerSlider.value = value;
                },
                MinPointerValue,
                _projectConfig.UIConfig.FishingPointerMoveDuration
            ).SetEase(Ease.InOutSine)
             .SetLoops(-1, LoopType.Yoyo);

            _pointerTween = DOTween.Sequence()
                .Append(moveToMax)
                .Append(loopTween);
        }

        public void End(bool isWin)
        {
            _loopedSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _loopedSound.release();

            if (_pointerTween == null)
                return;

            _pointerTween.Kill();
            _pointerTween = null;
        }
    }
}