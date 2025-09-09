using UnityEngine;

namespace Sources.UI.Services.Fishing
{
    [CreateAssetMenu(menuName = "Configs/FishingConfig", fileName = "FishingConfig")]
    public class FishingConfig: ScriptableObject
    {
        [SerializeField] private float _playerGaugeSpeed;
        [SerializeField] private float _failValueSpeed;

        [SerializeField] private FishingData[] _fishingDatas;

        public FishingData GetFishingData()
        {
            var index = Random.Range(0, _fishingDatas.Length);
            return _fishingDatas[index];
        }
    }
}