using R3;

namespace Sources.Runtime.Gameplay.Wallet.Currency
{
    public class Money : BaseCurrency<int>
    {
        public Money(int initialValue) : base(initialValue) { }

        public override bool TryAdd(int amount)
        {
            if (amount < 0) return false;

            SetValue(GetValue() + amount);

            return true;
        }

        public override bool TrySpend(int amount)
        {
            if (amount < 0 || amount > GetValue()) return false;

            SetValue(GetValue() - amount);

            return true;
        }
    }
}