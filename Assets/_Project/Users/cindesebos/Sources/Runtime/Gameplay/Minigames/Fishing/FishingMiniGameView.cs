using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.Runtime.Services.ProjectConfigLoader;
using UnityEngine;

namespace Sources.Runtime.Gameplay.MiniGames.Fishing
{
    public class FishingMiniGameView : MonoBehaviour
    {
        [SerializeField] private List<FishingCanvasItem> _items;
        [SerializeField] private GameObject _miniGame;

        private IProjectConfigLoader _projectConfigLoader;
        private FishingMiniGameBootstrapper _fishingMiniGameBootstrapper;

        public void Initialize(IProjectConfigLoader projectConfigLoader, FishingMiniGameBootstrapper fishingMiniGameBootstrapper)
        {
            _projectConfigLoader = projectConfigLoader;
            _fishingMiniGameBootstrapper = fishingMiniGameBootstrapper;

            _fishingMiniGameBootstrapper.OnEnded += OnEnded;
        }

        public async UniTask OnShow()
        {
            _miniGame.gameObject.SetActive(true);

            ResetItems();

            var tasks = new List<UniTask>();

            for (int i = 0; i < _items.Count; i++)
            {
                tasks.Add(ShowItemAsync(_items[i], _projectConfigLoader.ProjectConfig.UIConfig.FishingShowDelayBetweenItems * i));
            }

            await UniTask.WhenAll(tasks);
        }

        private void ResetItems()
        {
            foreach (var item in _items)
            {
                SetItemAlpha(item, 0);
            }
        }

        private async UniTask ShowItemAsync(FishingCanvasItem item, float delay)
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

        private void OnEnded()
        {
            _miniGame.gameObject.SetActive(false);
        }

        private void SetItemAlpha(FishingCanvasItem item, float alpha)
        {
            item.CanvasGroup.alpha = alpha;
        }

        private void OnDestroy()
        {
            _fishingMiniGameBootstrapper.OnEnded -= OnEnded;
        }
    }

    [System.Serializable]
    public class FishingCanvasItem
    {
        public GameObject GameObject;
        public CanvasGroup CanvasGroup;
        public Transform Transform => GameObject.transform;
    }
}