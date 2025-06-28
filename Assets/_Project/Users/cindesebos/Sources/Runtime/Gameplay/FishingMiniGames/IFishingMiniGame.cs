using System;
using UnityEngine;

namespace Sources.Runtime.Gameplay.FishingMiniGames
{
    public interface IFishingMiniGame
    {
        event Action OnLaunched;
        event Action OnEnded;

        void Launch();
        void SpawnFish(Fish fish);
        void End();
    }
}
