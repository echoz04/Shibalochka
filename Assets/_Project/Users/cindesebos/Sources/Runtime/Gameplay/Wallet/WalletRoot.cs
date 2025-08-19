using Sources.Runtime.Gameplay.Wallet.Currency;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Wallet
{
    public class WalletRoot
    {
        public Money Money { get; private set; }

        public WalletRoot()
        {
            Money = new Money(0);
        }
    }
}
