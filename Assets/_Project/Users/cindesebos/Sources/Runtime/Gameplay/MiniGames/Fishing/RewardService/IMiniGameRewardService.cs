using Sources.Runtime.Gameplay.Configs.Items;

namespace Sources.Runtime.Gameplay.MiniGames.Fishing
{
    public interface IMiniGameRewardService
    {
        void Initialize();
        ItemConfig GetRandomItem();
    }
}