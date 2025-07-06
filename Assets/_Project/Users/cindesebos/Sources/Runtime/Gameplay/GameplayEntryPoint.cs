using Sources.Runtime.Gameplay.Inventory;
using Sources.Runtime.Gameplay.MiniGames.Fishing;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Gameplay
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        private CharacterInput _characterInput;
        private InventoryRoot _inventoryRoot;
        private FishingMiniGameBootstrapper _fishingMiniGameBootstrapper;

        [Inject]
        private void Construct(CharacterInput characterInput, InventoryRoot inventoryRoot, FishingMiniGameBootstrapper fishingMiniGameBootstrapper)
        {
            _characterInput = characterInput;
            _inventoryRoot = inventoryRoot;
            _fishingMiniGameBootstrapper = fishingMiniGameBootstrapper;
        }

        private void Awake()
        {
            _characterInput.Enable();
            _inventoryRoot.BuildGrid();
            _fishingMiniGameBootstrapper.Initialize();
        }

        private void OnDestroy()
        {
            _characterInput.Disable();
        }
    }
}