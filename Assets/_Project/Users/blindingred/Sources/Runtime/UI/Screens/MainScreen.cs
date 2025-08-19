using System.Collections.Generic;
using Sources.UI.Services;
using VContainer;

namespace Sources
{
    public class MainScreen : UIScreenBase
    {
        [Inject]
        private void Construct(
            HUDService hudService,
            WalletService walletService)
        {
            UIServices = new List<IUIService>
            {
                hudService,
                walletService
            };
        }
    }
}