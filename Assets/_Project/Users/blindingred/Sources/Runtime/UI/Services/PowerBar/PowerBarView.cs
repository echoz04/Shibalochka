using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Sources.UI.Services.PowerBar
{
    public class PowerBarView:UIViewBase
    {
        [SerializeField] private GameObject _stamina;
        [SerializeField] private Image _sliderImage;
        
        [Inject]
        private void Construct()
        {

        }

        public void SetBarValue(float value)
        {
            _sliderImage.fillAmount = value;
        }
    }
}