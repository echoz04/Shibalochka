using System.Linq;
using Sources.Runtime.Gameplay.Configs;
using Sources.Runtime.Gameplay.Configs.Fish;
using Sources.Runtime.Gameplay.Configs.Items;
using UnityEngine;
using VContainer;

namespace Sources.Runtime.Gameplay.MiniGames.Fishing
{
    public class MiniGameRewardService : IMiniGameRewardService
    {
        private readonly ProjectConfig _projectConfig;
        private ItemsConfig _itemsConfig;
 
        [Inject]
        public MiniGameRewardService(ProjectConfig projectConfig)
        {
            _projectConfig = projectConfig;
        }

        public void Initialize()
        {
            _itemsConfig = _projectConfig.ItemsConfig;
        }

        public ItemConfig GetRandomItem()
        {
            if (_itemsConfig == null)
                return null;
            
            var rarity = GetRandomRarity();
            var item = GetRandomItem(rarity);

            return !item ? null : item;
        }

        private Rarity GetRandomRarity()
        {
            var rarities = _itemsConfig.RaritiesConfig.ToArray();
            var totalWeight = rarities.Sum(r => r.Chance);

            var roll = Random.Range(0, totalWeight);
            var cumulative = 0;

            foreach (var rarity in rarities)
            {
                cumulative += rarity.Chance;
                if (roll < cumulative)
                    return rarity.Rarity;
            }

            return rarities[0].Rarity;
        }

        private ItemConfig GetRandomItem(Rarity rarity)
        {
            var rarityConfig = _itemsConfig.RaritiesConfig.FirstOrDefault(r => r.Rarity == rarity);

            if (rarityConfig == null)
                return null;

            var mutantChange = Random.Range(0, 100);

            var isMutant = mutantChange < rarityConfig.MutantChance;

            var filteredItems = _itemsConfig.Configs
                .Where(i => i && i.Rarity == rarity && i.IsMutant == isMutant)
                .ToList();

            if (filteredItems.Count == 0)
                return null;

            var randomItem = filteredItems[Random.Range(0, filteredItems.Count)];

            Debug.Log($"Random Type is {rarity}, Mutant Chance Is {mutantChange} And Target Change Is {rarityConfig.MutantChance}, Is Mutant {isMutant}, Random Item Is {randomItem}");

            return randomItem;
        }
    }
}