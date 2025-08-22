using Sources.Runtime.Gameplay.Inventory;
using Sources.Runtime.Gameplay.Wallet;
using Sources.Signals;
using VContainer;
using VContainer.Unity;

namespace Sources.Runtime.Gameplay
{
    public class GameplayEntryPoint : IInitializable
    {
        private InventoryRoot _inventoryRoot;
        private WalletRoot _walletRoot;
        private ISignalBus _signalBus;

        [Inject]
        private void Construct(
            ISignalBus signalBus
            )
        {
            _signalBus = signalBus;
        }

        void IInitializable.Initialize()
        {
            _signalBus.Fire(new ScreenChangeSignal(typeof(MainScreen)));
            
// #if UNITY_EDITOR
//             ContentManagementSystem.Instance.InventoryRoot = _inventoryRoot;
//             ContentManagementSystem.Instance.WalletRoot = _walletRoot;
// #endif
        }
       
    }
}