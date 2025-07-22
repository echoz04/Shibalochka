using UnityEngine;
using Sirenix.OdinInspector;

namespace Sources.Runtime.Gameplay.Configs
{
    [System.Serializable]
    public class UIConfig
    {
        [Title("Fishing Mini Game UI")]

        [SerializeField, LabelText("Pointer Move Duration"), MinValue(0f)] private float _fishingPointerMoveDuration = 2.45f;
        public float FishingPointerMoveDuration => _fishingPointerMoveDuration;

        [SerializeField, LabelText("Fishes Move Speed"), MinValue(0f)] private float _fishesMoveSpeed = 0.25f;
        public float FishesMoveSpeed => _fishesMoveSpeed;

        [SerializeField, LabelText("Minimum Waiting Time To Catch A Fish"), MinValue(0f)] private float _minimumWaitingTime = 3f;
        public float MinimumWaitingTime => _minimumWaitingTime;

        [SerializeField, LabelText("Maximum Waiting Time To Catch A Fish"), MinValue(0f)] private float _maximumWaitingTime = 3f;
        public float MaximumWaitingTime => _maximumWaitingTime;

        [SerializeField, LabelText("Time To Catch A Fish"), MinValue(0f)] private float _timeToCatchFish = 0.45f;
        public float TimeToCatchFish => _timeToCatchFish;

        [SerializeField, LabelText("Show Delay Between Items"), MinValue(0f)] private float _fishingShowDelayBetweenItems = 0.225f;
        public float FishingShowDelayBetweenItems => _fishingShowDelayBetweenItems;

        [SerializeField, LabelText("Fade Duration"), MinValue(0f)] private float _fishingFadeDuration = 0.55f;
        public float FishingFadeDuration => _fishingFadeDuration;

        [SerializeField, LabelText("Click Cooldown"), MinValue(0f)] private float _fishingClickCooldown = 0.1f;
        public float FishingClickCooldown => _fishingClickCooldown;

        [SerializeField, LabelText("Slider delay Duration"), MinValue(0f)]
        private float _delayDuration = 2.45f;

        [Space(17.5f)]

        [SerializeField, LabelText("Slider Time To Max"), MinValue(0f)] private float _sliderTimeToMaxValue = 1f;
        public float SliderTimeToMaxValue => _sliderTimeToMaxValue;

        [SerializeField, LabelText("Value On Common Catch"), MinValue(0f)] private float _valueToAddOnCommonCatch = 10f;
        public float ValueToAddOnCommonCatch => _valueToAddOnCommonCatch;

        [SerializeField, LabelText("Value On Gold Catch"), MinValue(0f)] private float _valueToAddOnGoldCatch = 15f;
        public float ValueToAddOnGoldCatch => _valueToAddOnGoldCatch;

        [SerializeField, LabelText("Value On Miss"), MinValue(0f)] private float _valueToRemoveOnMiss = 25f;
        public float ValueToRemoveOnMiss => _valueToRemoveOnMiss;

        [Space(17.5f)]
        [Title("Stamina UI")]

        [SerializeField, LabelText("Stamina Fill Duration"), MinValue(0f)] private float _staminaFillDuration = 2f;
        public float StaminaFillDuration => _staminaFillDuration;

        [SerializeField, LabelText("Stamina Drain Duration"), MinValue(0f)] private float _staminaDrainDuration = 2.5f;
        public float StaminaDrainDuration => _staminaDrainDuration;
    }
}