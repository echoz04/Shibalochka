using System;
using UnityEngine;

namespace Sources.Runtime.Gameplay.MiniGames
{
    public class MovablePointerMiniGame : IMiniGame
    {
        public event Action OnLaunched;
        public event Action OnEnded;

        public MovablePointerMiniGame()
        {
        }

        public void Launch()
        {
            OnLaunched?.Invoke();
        }

        public void End()
        {
            OnEnded?.Invoke();
        }
    }
}