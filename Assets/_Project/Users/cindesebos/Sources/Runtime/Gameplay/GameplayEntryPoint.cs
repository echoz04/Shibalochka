using Sources.Runtime.Gameplay.Inventory;
using Sources.Runtime.Gameplay.Wallet;
using Sources.Signals;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sources.Runtime.Gameplay
{
    public class GameplayEntryPoint : IInitializable
    {
        private InventoryRoot _inventoryRoot;
        private WalletRoot _walletRoot;
        private UIScreenManager _uiScreenManager;
        private ISignalBus _signalBus;

        [Inject]
        private void Construct(
            UIScreenManager uiScreenManager,
            ISignalBus signalBus
            )
        {
            _uiScreenManager = uiScreenManager;
            _signalBus = signalBus;
        }

        void IInitializable.Initialize()
        {
            _uiScreenManager.OpenUIScreen<MainScreen>();
            
// #if UNITY_EDITOR
//             ContentManagementSystem.Instance.InventoryRoot = _inventoryRoot;
//             ContentManagementSystem.Instance.WalletRoot = _walletRoot;
// #endif
        }
       
    }
}