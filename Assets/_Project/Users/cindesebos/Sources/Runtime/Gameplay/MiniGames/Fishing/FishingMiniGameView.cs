using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.Runtime.Gameplay.Configs;
using UnityEngine;

namespace Sources.Runtime.Gameplay.MiniGames.Fishing
{
    public class FishingMiniGameView : MonoBehaviour
    {
        [SerializeField] private List<FishingCanvasItem> _items;
        [SerializeField] private GameObject _miniGame;
        [SerializeField] private GameObject _catchWindow;

        private ProjectConfig _projectConfig;
        private FishingMiniGameBootstrapper _bootstrapper;

        public void Initialize(FishingMiniGameBootstrapper bootstrapper, ProjectConfig projectConfig)
        {
            _bootstrapper = bootstrapper;
            _projectConfig = projectConfig;

            _bootstrapper.OnCatchTimeStarted += OnCatchTimeStarted;
            _bootstrapper.OnCatchTiming += OnCatchTiming;
            _bootstrapper.OnCatchTimeEnded += OnCatchTimeEnded;
        }

        private void OnDestroy()
        {
            _bootstrapper.OnCatchTimeStarted -= OnCatchTimeStarted;
            _bootstrapper.OnCatchTiming -= OnCatchTiming;
            _bootstrapper.OnCatchTimeEnded -= OnCatchTimeEnded;
        }

        public async UniTask Show()
        {
            _miniGame.gameObject.SetActive(true);

            ResetItems();

            var tasks = new List<UniTask>();

            for (int i = 0; i < _items.Count; i++)
            {
                tasks.Add(ShowItemAsync(_items[i], _projectConfig.UIConfig.FishingShowDelayBetweenItems * i));
            }

            await UniTask.WhenAll(tasks);
        }

        public void OnCatchTimeStarted()
        {

        }

        public void OnCatchTiming()
        {
            _catchWindow.SetActive(true);
        }

        public void OnCatchTimeEnded()
        {
            _catchWindow.SetActive(false);
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
                .DOFade(1f, _projectConfig.UIConfig.FishingFadeDuration)
                .SetDelay(delay)
                .SetEase(Ease.OutQuad)
                .AsyncWaitForCompletion()
                .AsUniTask();

            var scaleTask = item.Transform
                .DOScale(targetScale, _projectConfig.UIConfig.FishingFadeDuration)
                .SetDelay(delay)
                .SetEase(Ease.OutBack)
                .AsyncWaitForCompletion()
                .AsUniTask();

            await UniTask.WhenAll(fadeTask, scaleTask);
        }

        public void Hide()
        {
            _miniGame.gameObject.SetActive(false);
        }

        private void SetItemAlpha(FishingCanvasItem item, float alpha)
        {
            item.CanvasGroup.alpha = alpha;
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