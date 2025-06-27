using System;
using UnityEngine;

namespace Sources.Runtime.Gameplay.MiniGames
{
    public class MiniGameBootstrapper : MonoBehaviour
    {
        private IMiniGame _miniGame;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _miniGame = new MovablePointerMiniGame();
        }

        public void Launch()
        {
            _miniGame.Launch();
        }
    }
}