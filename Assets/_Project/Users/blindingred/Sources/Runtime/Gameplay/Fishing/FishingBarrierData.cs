using System;

namespace Sources.UI.Services.Fishing
{
    [Serializable]
    public struct FishingBarrierData
    {
        public int MinBarrierTaps;
        public int MaxBarrierTaps;
        public float MinBarrierPosition;
        public float MaxBarrierPosition;

        public FishingBarrierData(int minBarrierTaps, int maxBarrierTaps, float minBarrierPosition, float maxBarrierPosition)
        {
            MinBarrierTaps = minBarrierTaps;
            MaxBarrierTaps = maxBarrierTaps;
            MinBarrierPosition = minBarrierPosition;
            MaxBarrierPosition = maxBarrierPosition;
        }
    }
}