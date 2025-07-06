using System;
using NaughtyAttributes;
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

        public bool IsCaught(float pointerValue) => Mathf.Abs(pointerValue - CatchCenterValue) <= CatchRange;
    }
}