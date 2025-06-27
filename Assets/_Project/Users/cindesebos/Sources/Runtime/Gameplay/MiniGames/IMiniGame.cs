using System;
using UnityEngine;

namespace Sources.Runtime.Gameplay.MiniGames
{
    public interface IMiniGame
    {
        event Action OnLaunched;
        event Action OnEnded;

        void Launch();
        void End();
    }
}
