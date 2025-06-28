using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.Runtime.Services.ProjectConfigLoader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Runtime.Gameplay.FishingMiniGames
{
    public class FishingMiniGameView : MonoBehaviour
    {
        [SerializeField] private List<FishingCanvasItem> _items;

        private IProjectConfigLoader _projectConfigLoader;

        public void Initialize(IProjectConfigLoader projectConfigLoader)
        {
            _projectConfigLoader = projectConfigLoader;
        }

        public async UniTask OnShow()
        {
            Debug.Log("FishingMiniGameView.OnShow");

            Reset();

            var tasks = new List<UniTask>();

            for (int i = 0; i < _items.Count; i++)
            {
                Debug.Log($"Showing item {i} with delay {_projectConfigLoader.ProjectConfig.UIConfig.FishingShowDelayBetweenItems * i}");

                tasks.Add(ShowElementAsync(_items[i], _projectConfigLoader.ProjectConfig.UIConfig.FishingShowDelayBetweenItems * i));
            }

            await UniTask.WhenAll(tasks);
        }

        private void Reset()
        {
            foreach (var item in _items)
            {
                SetAlpha(item, 0);
            }
        }

        private async UniTask ShowElementAsync(FishingCanvasItem item, float delay)
        {
            item.GameObject.SetActive(true);
            item.CanvasGroup.alpha = 0f;
            Vector3 targetScale = item.Transform.localScale;
            item.Transform.localScale = Vector3.zero;

            var fadeTask = item.CanvasGroup
                .DOFade(1f, _projectConfigLoader.ProjectConfig.UIConfig.FishingFadeDuration)
                .SetDelay(delay)
                .SetEase(Ease.OutQuad)
                .AsyncWaitForCompletion()
                .AsUniTask();

            var scaleTask = item.Transform
                .DOScale(targetScale, _projectConfigLoader.ProjectConfig.UIConfig.FishingFadeDuration)
                .SetDelay(delay)
                .SetEase(Ease.OutBack)
                .AsyncWaitForCompletion()
                .AsUniTask();

            await UniTask.WhenAll(fadeTask, scaleTask);
        }

        private void SetAlpha(FishingCanvasItem item, float alpha)
        {
            item.CanvasGroup.alpha = alpha;
        }
    }
}