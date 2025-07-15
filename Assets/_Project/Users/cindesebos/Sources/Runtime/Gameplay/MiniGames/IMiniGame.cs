using System;

namespace Sources.Runtime.Gameplay.MiniGames
{
    public interface IMiniGame
    {
        event Action OnLaunched;
        event Action<bool> OnEnded;

        void Launch();
        void End(bool isWin);
    }
}
