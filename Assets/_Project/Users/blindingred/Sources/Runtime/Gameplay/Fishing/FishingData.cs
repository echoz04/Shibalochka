using System;

namespace Sources.UI.Services.Fishing
{
    [Serializable]
    public struct FishingData
    {
        public string FishTag;
        public FishingBarrierData[] BarriersData;

        public FishingData(string fishTag, FishingBarrierData[] barriersData)
        {
            FishTag = fishTag;
            BarriersData = barriersData;
        }
    }
}