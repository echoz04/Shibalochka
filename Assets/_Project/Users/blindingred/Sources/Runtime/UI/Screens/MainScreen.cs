using System.Collections.Generic;
using Sources.UI.Services;
using Sources.UI.Services.PowerBar;
using VContainer;

namespace Sources
{
    public class MainScreen : UIScreenBase
    {
        [Inject]
        private void Construct(
            HUDService hudService,
            WalletService walletService,
            PowerBarService powerBarService)
        {
            UIServices = new List<IUIService>
            {
                hudService,
                walletService,
                powerBarService
            };
        }
    }
}