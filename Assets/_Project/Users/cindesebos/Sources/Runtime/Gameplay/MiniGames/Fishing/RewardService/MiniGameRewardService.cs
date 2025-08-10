using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Runtime.Gameplay.Configs.Fish;
using Sources.Runtime.Gameplay.Configs.Items;
using Sources.Runtime.Services.ProjectConfigLoader;
using UnityEngine;

namespace Sources.Runtime.Gameplay.MiniGames.Fishing
{
    public class MiniGameRewardService : IMiniGameRewardService
    {
        private readonly IProjectConfigLoader _projectConfigLoader;
        private ItemsConfig _itemsConfig;

        public MiniGameRewardService(IProjectConfigLoader projectConfigLoader)
        {
            _projectConfigLoader = projectConfigLoader;
        }

        public void Initialize()
        {
            _itemsConfig = _projectConfigLoader.ProjectConfig.ItemsConfig;
        }

        public ItemConfig GetRandomItem()
        {
            if (_itemsConfig == null)
                return null;

            Rarity rarity = GetRandomRarity();
            ItemConfig item = GetRandomItem(rarity);

            if (item == null)
                return null;

            return item;
        }

        private Rarity GetRandomRarity()
        {
            var rarities = _itemsConfig.RaritiesConfig;
            int totalWeight = rarities.Sum(r => r.Chance);

            int roll = UnityEngine.Random.Range(0, totalWeight);
            int cumulative = 0;

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
            RaritiesConfig rarityConfig = _itemsConfig.RaritiesConfig.FirstOrDefault(r => r.Rarity == rarity);

            if (rarityConfig == null)
                return null;

            int mutantChange = UnityEngine.Random.Range(0, 100);

            bool isMutant = mutantChange < rarityConfig.MutantChance;

            var filteredItems = _itemsConfig.Configs
                .Where(i => i != null && i.Rarity == rarity && i.IsMutant == isMutant)
                .ToList();

            if (filteredItems.Count == 0)
                return null;

            var randomItem = filteredItems[UnityEngine.Random.Range(0, filteredItems.Count)];

            Debug.Log($"Random Type is {rarity}, Mutant Chance Is {mutantChange} And Target Change Is {rarityConfig.MutantChance}, Is Mutant {isMutant}, Random Item Is {randomItem}");

            return randomItem;
        }
    }
}