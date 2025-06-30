using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Sources.Runtime.Services.ProjectConfigLoader;
using Sources.Runtime.Utilities;

namespace Sources.Runtime.Gameplay.MiniGames.Fishing
{
    public class MovableFishesMiniGame : IMiniGame
    {
        private const float FishSpeed = 0.25f;

        public event Action OnLaunched;
        public event Action<bool> OnEnded;

        private readonly IEnumerable<FishSlot> _fishSlots;
        private readonly Vector3 _startPosition;
        private readonly Vector3 _endPosition;
        private readonly IProjectConfigLoader _projectConfigLoader;
        private readonly Slider _pointerSlider;
        private readonly UnityEngine.Camera _camera;
        private readonly Dictionary<Transform, FishSlot> _fishMap = new();

        private List<Tweener> _fishTweeners = new();

        public MovableFishesMiniGame(IEnumerable<FishSlot> fishSlots, Transform[] edgePoints, IProjectConfigLoader projectConfigLoader,
        Slider pointerSlider, UnityEngine.Camera camera)
        {
            _fishSlots = fishSlots;
            _startPosition = edgePoints[0].transform.position;
            _endPosition = edgePoints[1].transform.position;
            _projectConfigLoader = projectConfigLoader;

            _pointerSlider = pointerSlider;
            _camera = camera;

            foreach (var slot in _fishSlots)
            {
                if (slot?.CurrentFish != null)
                    _fishMap[slot.CurrentFish.transform] = slot;
            }
        }


        public void Launch()
        {
            OnLaunched?.Invoke();

            MoveFishes();
        }

        private void MoveFishes()
        {
            foreach (var fish in _fishSlots)
            {
                if (fish == null) continue;

                var fishTransform = fish.CurrentFish.transform;
                StartFishLoop(fishTransform);
            }
        }

        private void StartFishLoop(Transform fishTransform)
        {
            float distance = Vector3.Distance(fishTransform.position, _endPosition);
            float duration = distance / _projectConfigLoader.ProjectConfig.UIConfig.FishesMoveSpeed;

            var tweener = fishTransform.DOMove(_endPosition, duration)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    if (_fishMap.TryGetValue(fishTransform, out var slot))
                    {
                        Vector3 fishWorldPos = fishTransform.position;
                        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(_camera, fishWorldPos);
                        slot.CatchCenterValue = Extensions.MapScreenPositionToSliderValue(screenPos.x, _pointerSlider, _camera);
                    }
                })
                .OnComplete(() =>
                {
                    fishTransform.position = _startPosition;
                    StartFishLoop(fishTransform);
                });

            _fishTweeners.Add(tweener);
        }

        public void End(bool isWin)
        {
            OnEnded?.Invoke(isWin);

            foreach (var tweener in _fishTweeners)
                tweener?.Kill();

            _fishTweeners.Clear();
        }
    }
}