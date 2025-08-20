using System;
using Sources.Runtime.Services.SceneLoader;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sources.Runtime.Gameplay.Map
{
    public class BaseMapSelectorButton : MonoBehaviour, IPointerDownHandler
    {
        public static BaseMapSelectorButton Instance { get; private set; }

        public static event Action<Scene> IslandSelected;
        public static event Action IslandUnselected;

        [SerializeField] private Scene _islandToLoad;

        [SerializeField] private Image _sourceImage;
        [SerializeField] private Sprite _selectedSprite, _unSelectedSprite;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Instance != null)
            {
                Instance.UnSelect();

                if (Instance == this)
                {
                    Instance = null;

                    return;
                }
            }

            _sourceImage.sprite = _selectedSprite;
            Instance = this;
            IslandSelected?.Invoke(_islandToLoad);
        }

        public void UnSelect()
        {
            _sourceImage.sprite = _unSelectedSprite;
            IslandUnselected?.Invoke();
        }
    }
}
