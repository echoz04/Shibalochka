using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Runtime.Gameplay.MiniGames.Fishing
{
    public class FishingMiniGameProgressView : MonoBehaviour
    {
        public float Value
        {
            get => _value;
            set
            {
                _value = Mathf.Clamp(value, 0, 100);

                _slider.value = Value;
                Debug.Log($"{Mathf.RoundToInt(Value)}%     Value is {Value} ");
                _text.text = $"{Mathf.RoundToInt(Value)}%";
            }
        }

        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _text;

        private float _value;

        private void OnValidate()
        {
            _slider ??= GetComponentInChildren<Slider>();
            _text ??= GetComponentInChildren<TextMeshProUGUI>();
        }

        public void AddValue(float newValue)
        {
            Value += newValue;
            RuntimeManager.StudioSystem.setParameterByName("FishingMiniGameProgress", Value);
            RuntimeManager.PlayOneShot("event:/SFX/MiniGames/MG_Success");
        }

        public void RemoveValue(float newValue)
        {
            Value -= newValue;
            RuntimeManager.StudioSystem.setParameterByName("FishingMiniGameProgress", Value);
            RuntimeManager.PlayOneShot("event:/SFX/MiniGames/MG_Failure");
        }

        public void SetValue(float newValue)
        {
            Value = newValue;
        }
    }
}