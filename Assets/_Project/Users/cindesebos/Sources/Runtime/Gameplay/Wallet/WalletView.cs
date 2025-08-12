using R3;
using TMPro;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sources.Runtime.Gameplay.Wallet
{
    public class WalletView : MonoBehaviour, IInitializable
    {
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private GameObject _winPanel;

        private WalletRoot _root;

        private CompositeDisposable _compositeDisposable = new CompositeDisposable();

        [Inject]
        private void Construct(WalletRoot root)
        {
            _root = root;
        }

        void IInitializable.Initialize()
        {
            _moneyText.text = _root.Money.Value.ToString();

            _root.Money.Value.Subscribe(value =>
            {
                if (value >= 1000)
                {
                    _winPanel.SetActive(true);
                    Debug.Log("YOu win");
                }

                _moneyText.text = value.ToString();
            }).AddTo(_compositeDisposable);
        }

        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }
    }
}
