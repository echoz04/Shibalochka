using System;

namespace Sources.Runtime.Gameplay.MiniGames
{
    public interface IMiniGame
    {
        void Launch();
        void End(bool isWin);
    }
}
