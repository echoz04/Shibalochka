using R3;

namespace Sources.Runtime.Gameplay.Wallet.Currency
{
    public abstract class BaseCurrency<T>
    {
        public ReadOnlyReactiveProperty<T> Value => _value;

        protected readonly ReactiveProperty<T> _value;

        protected BaseCurrency(T initialValue) =>
            _value = new ReactiveProperty<T>(initialValue);

        public abstract bool TryAdd(T amount);
        public abstract bool TrySpend(T amount);

        protected void SetValue(T newValue) => _value.Value = newValue;

        protected T GetValue() =>
            _value.Value;
    }
}
