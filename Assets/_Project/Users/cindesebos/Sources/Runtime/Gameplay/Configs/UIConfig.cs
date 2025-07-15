using UnityEngine;

namespace Sources.Runtime.Gameplay.Configs.Fish
{
    [System.Serializable]
    public class UIConfig
    {
        [Header("Fishing Mini Game UI")]
        [field: SerializeField] public float FishingPointerMoveDuration { get; private set; } = 2.45f;
        [field: SerializeField] public float FishesMoveSpeed { get; private set; } = 0.25f;
        [field: SerializeField] public float FishingShowDelayBetweenItems { get; private set; } = 0.225f;
        [field: SerializeField] public float FishingFadeDuration { get; private set; } = 0.55f;
        [field: Space(17.5f)]

        [field: SerializeField] public float SliderTimeToMaxValue { get; private set; } = 1f;
        [field: SerializeField] public float ValueToAddOnCommonCatch { get; private set; } = 15f;
        [field: SerializeField] public float ValueToAddOnGoldCatch { get; private set; } = 30f;
        [field: SerializeField] public float ValueToRemoveOnMiss { get; private set; } = 30f;
        [field: Space(17.5f)]

        [Header("Stamina UI")]
        [field: SerializeField] public float StaminaFillDuration { get; private set; } = 2f;
        [field: SerializeField] public float StaminaDrainDuration { get; private set; } = 2.5f;
    }
}