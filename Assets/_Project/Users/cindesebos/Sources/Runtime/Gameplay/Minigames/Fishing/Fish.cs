using UnityEngine;

namespace Sources.Runtime.Gameplay.MiniGames.Fishing
{
    public class Fish : MonoBehaviour
    {
        [field: SerializeField] public FishType Type { get; private set; }
        [field: SerializeField] public float CatchRange { get; private set; } = 5f;
    }
}