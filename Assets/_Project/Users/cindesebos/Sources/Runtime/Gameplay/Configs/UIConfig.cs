using UnityEngine;

namespace Sources.Runtime.Gameplay.Configs.Fish
{
    [System.Serializable]
    public class UIConfig
    {
        [Header("Fishing Mini Game UI")]
        [field: SerializeField] public float FishingPointerMoveDuration { get; private set; } = 2.45f;
        [field: SerializeField] public float FishingShowDelayBetweenItems { get; private set; } = 0.225f;
        [field: SerializeField] public float FishingFadeDuration { get; private set; } = 0.55f;
        [field: Space]

        [field: SerializeField] public float SliderTimeToMaxValue { get; private set; } = 1f;
    }
}