using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.UI.Services
{
    public class FadeView : UIViewBase
    {
        [SerializeField] private Image _fadeImage;

        private Tween _tween;

        public void FadeIn(Action onComplete = null)
        {
            Fade(true, onComplete).Forget();
        }

        public void FadeOut(Action onComplete = null)
        {
            Fade(false, onComplete).Forget();
        }

        private async UniTask Fade(bool isIn, Action onComplete = null)
        {
            var startValue = isIn ? 0f : 1f;
            var targetValue = isIn ? 1f : 0f;
            var ease = isIn ? Ease.OutCubic : Ease.InCubic;
            
            _fadeImage.color = new Color(0, 0, 0, startValue);
            
            _tween.Kill();
            
            Show();
            
            if (!isIn)
            {
                await UniTask.WaitForSeconds(1); 
            }
            
            _tween = _fadeImage.DOFade(targetValue, 1f).SetEase(ease).OnComplete(
                () =>
                {
                    onComplete?.Invoke();
                    Hide();
                });
        }
    }
}