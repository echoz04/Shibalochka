using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sources
{
    public class AlphaTweenButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _hoveredImage;

        [SerializeField] private float _tweenDuration = 0.2f;

        private Tween _alphaTween;

        private void Awake()
        {
            _hoveredImage.color =  new Color(1f,1f,1f,0f);
        }

        public void AddListener(Action callback)
        {
            _button.onClick.AddListener(() => callback?.Invoke());
        }

        public void RemoveAllListeners()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("OnPointerEnter");
            _alphaTween.Kill();
            _alphaTween = _hoveredImage.DOFade(1f, _tweenDuration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
            Debug.Log("OnPointerExit");
            _alphaTween.Kill();
            _alphaTween = _hoveredImage.DOFade(0f, _tweenDuration);
        }
    }
}
