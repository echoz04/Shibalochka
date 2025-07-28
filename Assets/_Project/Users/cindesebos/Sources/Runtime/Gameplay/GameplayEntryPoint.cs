using Sources.Runtime.Gameplay.Inventory;
using Sources.Runtime.Gameplay.MiniGames.Fishing;
using Sources.Runtime.Project;
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
#if UNITY_EDITOR
            ContentManagementSystem.Instance.InventoryRoot = _inventoryRoot;
#endif

            _characterInput.Enable();
            _inventoryRoot.Initialize();
            _fishingMiniGameBootstrapper.Initialize();
        }

        private void OnDestroy()
        {
            _characterInput.Disable();
        }
    }
}