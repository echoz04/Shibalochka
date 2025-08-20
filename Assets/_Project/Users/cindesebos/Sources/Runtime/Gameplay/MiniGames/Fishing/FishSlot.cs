using System;
using Sources.Runtime.Gameplay.MiniGames.Fishing.FishTypes;
using UnityEngine;

namespace Sources.Runtime.Gameplay.MiniGames.Fishing
{
    [Serializable]
    public class FishSlot
    {
        [Header("Read Only")]
        [field: SerializeField] public Fish CurrentFish { get; set; }
        [field: SerializeField] public bool IsOccupied { get; set; } = false;
        [field: SerializeField] public float CatchCenterValue { get; set; }
        [field: Space]

        [field: SerializeField] public Transform SpawnPoint { get; set; }
        [field: SerializeField] public float CatchRange { get; set; }

        public bool IsAlreadyCaught => !CurrentFish.gameObject.activeInHierarchy;

        public bool IsCaught(float pointerValue)
        {
            if (IsAlreadyCaught == true)
                return false;

            return Mathf.Abs(pointerValue - CatchCenterValue) <= CatchRange;
        }
    }
}