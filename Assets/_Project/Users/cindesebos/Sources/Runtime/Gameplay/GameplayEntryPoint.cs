using Sources.Runtime.Gameplay.Inventory;
using Sources.Runtime.Gameplay.MiniGames.Fishing;
using Sources.Runtime.Gameplay.Wallet;
using Sources.Runtime.Project;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Gameplay
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        private CharacterInput _characterInput;
        private InventoryRoot _inventoryRoot;
        private InventoryView _inventoryView;
        private FishingMiniGameBootstrapper _fishingMiniGameBootstrapper;
        private WalletRoot _walletRoot;
        private WalletView _walletView;

        [Inject]
        private void Construct(CharacterInput characterInput, InventoryRoot inventoryRoot, InventoryView inventoryView,
        FishingMiniGameBootstrapper fishingMiniGameBootstrapper, WalletView walletView, WalletRoot walletRoot)
        {
            _characterInput = characterInput;
            _inventoryRoot = inventoryRoot;
            _inventoryView = inventoryView;
            _fishingMiniGameBootstrapper = fishingMiniGameBootstrapper;
            _walletView = walletView;
            _walletRoot = walletRoot;
        }

        private void Awake()
        {
#if UNITY_EDITOR
            ContentManagementSystem.Instance.InventoryRoot = _inventoryRoot;
            ContentManagementSystem.Instance.WalletRoot = _walletRoot;
#endif

            _characterInput.Enable();

            _inventoryRoot.Initialize();
            _inventoryView.Initialize();
            _fishingMiniGameBootstrapper.Initialize();
            _walletView.Initialize();
        }

        private void OnDestroy()
        {
            _characterInput.Disable();
        }
    }
}