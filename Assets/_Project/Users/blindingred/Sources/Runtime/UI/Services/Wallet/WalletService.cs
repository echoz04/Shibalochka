using UnityEngine;
using VContainer;

namespace Sources.UI.Services
{
    public class WalletService: IUIService
    {
        private WalletView _view;
        public IUIView View => _view;

        [Inject]
        private void Construct(
            WalletView walletView)
        {
            _view = walletView;
        }

        public void Enable()
        {
            Subscribe();
            _view.Show();
            _view.gameObject.SetActive(true);
        }

        public void Disable()
        {
            Unsubscribe();
            _view.Hide();
            _view.gameObject.SetActive(false);
        }

        public void Subscribe()
        {
        }

        public void Unsubscribe()
        {
            
        }
    }
}