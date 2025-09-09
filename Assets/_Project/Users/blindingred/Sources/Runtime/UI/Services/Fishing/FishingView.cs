using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Sources.UI.Services.Fishing
{
    public class FishingView:UIViewBase
    {
        [SerializeField] private Slider _playerBar;
        [SerializeField] private Slider _failBar;
        [SerializeField] private GameObject _stopSign;
        [SerializeField] private GameObject _goSign;
        [SerializeField] private GameObject _fishingCoil;
        [SerializeField] private RectTransform _barriersRect;
        [SerializeField] private RectTransform[] _barriers;

        private Dictionary<int, RectTransform> _mappedBarriers;

        [Inject]
        private void Construct()
        {
            _mappedBarriers = new Dictionary<int, RectTransform>();
            _playerBar.value = 0f;
            _failBar.value = 0f;
        }
        
        public void SetupFishingView(FishingBarrier[] fishingBarriers)
        {
            for (var i = 0; i < fishingBarriers.Length; i++)
            {
                var barrier = fishingBarriers[i];
                var positionOnBar = CalculatePositionByValue(barrier.PositionValue);
                Debug.Log($"Fishing barrier #{i}: value position {barrier.PositionValue} : vector position {positionOnBar} ");
                _barriers[i].anchoredPosition = positionOnBar;
                _mappedBarriers[i] = _barriers[i];
            }
        }
        
        public void SetPlayerBarValue(float value)
        {
            // Debug.Log($"Setting player bar value to {value}");
            _playerBar.value = value;
        }
        
        public void SetFailBarValue(float value)
        {
            _failBar.value = value;
        }

        public void DestroyBarrier(int index)
        {
            _mappedBarriers[index].gameObject.SetActive(false);
        }

        public void SetStopSignState(bool state)
        {
            _stopSign.SetActive(state);
        }

        public void SetGoSignState(bool state)
        {
            _goSign.SetActive(state);
        }

        public Vector2 CalculatePositionByValue(float value)
        {
            var height = _barriersRect.rect.height;
            var pos = Vector2.zero;
            pos.y = -height * _barriersRect.pivot.y + value * height;
            return pos;
        }
    }
}