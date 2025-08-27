using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Sources.UI.Services.PowerBar
{
    public class PowerBarView:UIViewBase
    {
        [SerializeField] private GameObject _stamina;
        [SerializeField] private Image _sliderImage;
        [SerializeField] private GameObject _maxPowerSign;
        
        [Inject]
        private void Construct()
        {

        }

        public void SetBarValue(float value)
        {
            _sliderImage.fillAmount = value;
        }

        public async UniTask ShowMaxPowerSign()
        {
            // TODO Добавить звук
            _maxPowerSign.SetActive(true);
            await UniTask.WaitForSeconds(1f);
            _maxPowerSign.SetActive(false);
        }
    }
}