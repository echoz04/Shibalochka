using UnityEngine;
using Sources.Runtime.Gameplay.FishingMiniGames.Types;

namespace Sources.Runtime.Gameplay.FishingMiniGames
{
    public class Fish : MonoBehaviour
    {
        [field: SerializeField] public FishType Type { get; private set; }
    }
}