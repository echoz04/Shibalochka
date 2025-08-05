using System;
using Sources.Runtime.Gameplay.Inventory;
using Sources.Runtime.Gameplay.Wallet;
using Sources.Runtime.Project;
using VContainer;
using VContainer.Unity;

namespace Sources.Runtime.Gameplay
{
    public class GameplayEntryPoint : IInitializable, IDisposable
    {
        private CharacterInput _characterInput;
        private InventoryRoot _inventoryRoot;
        private WalletRoot _walletRoot;

        [Inject]
        private void Construct(CharacterInput characterInput, InventoryRoot inventoryRoot, WalletRoot walletRoot)
        {
            _characterInput = characterInput;
            _inventoryRoot = inventoryRoot;
            _walletRoot = walletRoot;
        }

        void IInitializable.Initialize()
        {
            
#if UNITY_EDITOR
            ContentManagementSystem.Instance.InventoryRoot = _inventoryRoot;
            ContentManagementSystem.Instance.WalletRoot = _walletRoot;
#endif

            _characterInput.Enable();
        }

        void IDisposable.Dispose()
        {
            _characterInput.Disable();
            _characterInput?.Dispose();
        }
    }
}